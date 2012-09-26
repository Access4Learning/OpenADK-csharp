//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using OpenADK.Library.Impl;
using OpenADK.Library.Log;
using OpenADK.Util;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace OpenADK.Library
{
    /// <summary>  A global class representing the Library® Adk class framework.
    /// 
    /// Prior to calling any Adk methods or referencing any static constants, agents
    /// must first initialize the Adk by calling the static <c>Adk.initialize</c>
    /// method. The default <c>initialize</c> method loads all SIF Data Object
    /// (Sdo) modules and selects the latest version of SIF as the default version
    /// used to render SIF Messages. If you use this method, ensure that the proper
    /// assemblies are available on the path. Other forms of the <c>initialize</c>
    /// method allow you to choose the specific Sdo modules that are loaded as well
    /// as the version of SIF that will be the default for this agent session.
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public sealed class Adk
    {
        /// <summary>
        /// The default pattern used for writing to the logging framework
        /// </summary>
        public const string DEFAULT_LOG_PATTERN = "%d %-5p [%c] %m%n";

        /// <summary>
        /// The Identifier that is used to identify the ADK itself for logging operations ("ADK")
        /// </summary>
        public const string LOG_IDENTIFIER = "ADK";


        /// <summary>  Gets the versions of SIF supported by the Adk</summary>
        /// <returns> An array of SifVersion objects
        /// </returns>
        public static SifVersion[] SupportedSIFVersions
        {
            get { return sSupportedVersionsArray; }
        }

        private static readonly SifVersion[] sSupportedVersionsArray = new SifVersion[]
            {
                SifVersion.SIF11,
                SifVersion.SIF15r1,
                SifVersion.SIF20,
                SifVersion.SIF20r1,
                SifVersion.SIF21,
                SifVersion.SIF22,
                SifVersion.SIF23,
                SifVersion.SIF24,
                SifVersion.SIF25
            };


        /// <summary>  Initialize the Adk to use the latest version of SIF and all SIF Data
        /// Object (Sdo) libraries. Calling this method when the Adk has already been initialized has no effect.
        /// </summary>
        /// <exception cref="AdkException">If the ADK cannot be initialized</exception>
        public static void Initialize()
        {
            Initialize(SifVersion.LATEST, SIFVariant.SIF_US, int.MaxValue);
        }

        /// <summary>
        /// Initialize the Adk to use the latest version of SIF and all SIF Data Object (Sdo) libraries for the
        /// specified variant. Calling this method when the Adk has already been initialized has no effect.
        /// </summary>
        /// <param name="sifVariant">SIF variant (locale) used.</param>
        public static void Initialize(SIFVariant sifVariant)
        {
            Initialize(SifVersion.LATEST, sifVariant, int.MaxValue);
        }

        /// <summary>  Initialize the Adk to use the specified version of SIF.
        /// 
        /// Calling this method when the Adk has already been initialized has no effect.
        /// 
        /// This method must be called at agent startup to initialize 
        /// various resources of the Adk, establish global settings for the 
        /// class framework, and set the default version of SIF to which
        /// all messages originating from the agent will conform.
        /// 
        /// Beginning with Adk 1.5.0, this method also configures the global 
        /// Adk <c>ServerLog</c> instance with a logging module that 
        /// will be inherited by the ServerLog of all zones. It installs a 
        /// single logging module implementation: <c>DefaultServerLogModule</c>. 
        /// The behavior of this module is to report SIF_LogEntry objects to 
        /// the zone integration server via an Add SIF_Event message whenever 
        /// <c>ServerLog.reportLogEntry</c> is called on a zone and 
        /// the agent is running in SIF 1.5 or later. DefaultServerLogModule
        /// also echos server log messages to the zone's local logging framework Category
        /// so that agents do not need to duplicate logging to both the server
        /// and local agent log. If you want to install a custom <i>ServerLogModule</i> 
        /// implementation -- or need to adjust the settings of the default 
        /// module installed when the Adk is initialized -- call the 
        /// <c>Adk.getServerLog</c> method to obtain a reference to the 
        /// root of the <c>ServerLog</c> chain, then call its methods 
        /// to add and remove modules. Refer to the ServerLog class for more 
        /// information on server logging.
        /// 
        /// </summary>
        /// <param name="version">The version of SIF that will be used by the agent this
        /// session. Supported versions are enumerated by constants of the
        /// <see cref="OpenADK.Library.SifVersion"/> class. Once initialized,
        /// the version cannot be changed.
        /// </param>
        /// <param name="sdoLibraries">One or more of the constants defined by the SdoLibrary
        /// class, identifying the SIF Data Object libraries to be loaded into
        /// memory (e.g. SdoLibraryType.STUDENT | SdoLibraryType.HR )
        /// 
        /// </param>
        /// <exception cref="AdkException">If the ADK cannot be initialized</exception>
        /// <exception cref="OpenADK.Library.AdkNotSupportedException"> AdkNotSupportedException is thrown if the specified SIF
        /// version is not supported by the Adk, or if the <i>sdoLibraries</i>
        /// parameter is invalid
        /// </exception>
        public static void Initialize(SifVersion version, SIFVariant dataModel,
                                       int sdoLibraries)
        {
            if (Initialized)
            {
                return;
            }

            //	TODO: This code could be done in more of a modular way so that it
            //	doesn't need to be hand-edited each time a new SIFVariant is added.
            if (dataModel == SIFVariant.SIF_US)
            {
                sDtd = new OpenADK.Library.us.SifDtd();
            }
            else if (dataModel == SIFVariant.SIF_UK)
            {
                sDtd = new OpenADK.Library.uk.SifDtd();
            }
            else if (dataModel == SIFVariant.SIF_AU)
            {
                sDtd = new OpenADK.Library.au.SifDtd();
            }

            //if (dataModel == null)
            //{
            //    Console.Out.WriteLine("ERROR - No variant DTD loaded");
            //}
            //else
            //{
                Console.Out.WriteLine("Using datamodel - " + dataModel.ToString("G"));
            //}

            try
            {
                InitLogging();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }


            if (version == null)
            {
                throw new ArgumentException("SifVersion cannot be null");
            }

            sSingleton = new Adk();

            // the default formatter for String APIs in the ADK is the SIF 1.x formatter
            // for backwards compatibility
            sSingleton.fDefaultFormatter = SifDtd.SIF_1X_FORMATTER;

            //
            //	Set up the ServerLog
            //

            ServerLog sl = ServerLog;
            if (sl != null)
            {
                sl.AddLogger(new DefaultServerLogModule());
            }

            //  HTTP and HTTPS transports always available
            TransportPlugin tp = new HttpTransportPlugin();
            sSingleton.fTransports[tp.Protocol] = tp;

            tp = new HttpsTransportPlugin();
            sSingleton.fTransports[tp.Protocol] = tp;

            if (Debug != AdkDebugFlags.None)
            {
                sLog.Debug("Using Adk " + AdkVersion);
            }

            sSingleton.fVersion = version;

            //Dtd.LoadLibraries(sdoLibraries);
            ((DTDInternals)Dtd).LoadLibraries((int)sdoLibraries);

        }


        /// <summary>
        /// Returns the highes SIFVersion supported by the current instance of the ADK
        /// from the list of candidate versions. This method is helpful to agents
        /// during SIF_Request processing. A SIF_Request can contain multiple versions
        /// and the responding agent may want to choose the lastest supported version
        /// from the list.
        /// </summary>
        /// <param name="candidates">If null, or zero-length, the ADK.getSIFVersion() value will be returned</param>
        /// <returns></returns>
        public static SifVersion GetLatestSupportedVersion(SifVersion[] candidates)
        {
            CheckInit();
            if (candidates == null || candidates.Length == 0)
            {
                return SifVersion;
            }

            SifVersion returnVal = null;
            foreach (SifVersion candidate in candidates)
            {
                if (returnVal == null || candidate.CompareTo(returnVal) > 0)
                {
                    returnVal = candidate;
                }
            }
            if (returnVal == null)
            {
                returnVal = SifVersion;
            }
            return returnVal;
        }


        /// <summary>  Gets the transport protocols available to agents.</summary>
        /// <returns> An array of transport protocol strings (e.g. "http")
        /// </returns>
        public static string[] TransportProtocols
        {
            get
            {
                CheckInit();
                ArrayList arr = new ArrayList(sSingleton.fTransports.Count);
                foreach (TransportPlugin pi in sSingleton.fTransports.Values)
                {
                    if (!pi.Internal)
                    {
                        arr.Add(pi.Protocol);
                    }
                }
                return (string[])arr.ToArray(typeof(string));
            }
        }

        /// <summary>
        /// Installs a new TransportProtocol
        /// </summary>
        /// <param name="tp"></param>
        public static void Install(TransportPlugin tp)
        {
            if (tp == null)
            {
                throw new ArgumentNullException("TransportPlugin cannot be null");
            }
            CheckInit();
            sSingleton.fTransports[tp.Protocol.ToLower()] = tp;
        }


        /// <summary>
        /// Utility method to generate a GUID for SIF Data Objects and messages.
        /// </summary>
        /// <returns>a new GUID formatted as a SIF Refid</returns>
        public static string MakeGuid()
        {
            return SifFormatter.GuidToSifRefID(Guid.NewGuid());
        }

        /// <summary>  Gets an installed transport protocol</summary>
        /// <param name="protocol">The transport protocol name (e.g. "http", "https", etc.)
        /// </param>
        /// <returns> The plugin class that represents this protocol, for internal use
        /// by the class framework
        /// </returns>
        internal static TransportPlugin GetTransportProtocol(string protocol)
        {
            if (protocol == null)
            {
                throw new ArgumentNullException("Protocol cannot be null");
            }
            CheckInit();

            TransportPlugin returnValue;
            sSingleton.fTransports.TryGetValue(protocol.ToLower(), out returnValue);
            return returnValue;
        }

        #region Private Fields

        private SifVersion fVersion;

        private IDictionary<String, TransportPlugin> fTransports =
            new Dictionary<String, TransportPlugin>();

        #endregion

        #region Static Members

        /// <summary>
        /// Has the Adk Been Initialized
        /// </summary>
        public static bool Initialized
        {
            get { return sSingleton != null; }
        }

        /// <summary>  Returns the root logging framework Category for the Adk.
        /// Agents that wish to customize Adk logging may call this method to
        /// obtain the root logging framework Category.
        /// </summary>
        /// <remarks>The Adk uses the log4net framework for logging. If your application
        /// uses log4net and you would like to set up your own appenders rather than using
        /// the rudimentary file or console appender used by the Adk, then initialize log4net
        /// before calling Adk.Initialize(). If you wish to specify a custom log4net settings
        /// for the ADK, the ADK's Logger is named "ADK". Also note that Adk Debug logging is
        /// controlled by the Adk.Debug property, which should be set with the appropriate
        /// AdkDebugFlags</remarks>
        /// <value>The ILog instance used by the ADK</value>
        public static ILog Log
        {
            get { return sLog; }
        }

        /// <summary> 	Gets the root ServerLog instance for the Adk.</summary>
        /// <remarks>
        /// Agents that wish to customize Adk server-side logging may call this 
        /// method to obtain the class framework's root ServerLog instance. Call 
        /// any of the following methods to set up the chain of loggers that will
        /// be inherited by the Agent and all Zones:
        /// 
        /// <list type="bullet">
        /// <item><term><c>addLogger</c></term></item>
        /// <item><term><c>removeLogger</c></term></item>
        /// <item><term><c>clearLoggers</c></term></item>
        /// <item><term><c>getLoggers</c></term></item>
        /// </list>
        /// 
        /// Unlike client-side logging, server logging requires a connection to a 
        /// Zone Integration Server. Because the current SIF 1.x infrastructure does 
        /// not allow connections to servers independent of a zone, the logging 
        /// methods of ServerLog are useful only when called within the context of a 
        /// zone. Therefore, calling any of the logging methods on the ServerLog 
        /// instance returned by this method will result in an IllegalStateException. 
        /// This method is provided only to set up the ServerLog logger chain at
        /// the global Adk level. 
        /// </remarks>
        /// <returns> The Adk's root ServerLog instance</returns>
        /// <since>Adk 1.5</since> 
        public static ServerLog ServerLog
        {
            get { return sServerLog; }
        }


        /// <summary>  Redirects all log output to the specified file.</summary>
        /// <param name="logFilePath">The log file</param>
        public static void SetLogFile(string logFilePath)
        {
            // It seems redundant, but we need to close any 
            // existing appenders first to prevent an UnauthorizedAccessException 
            // if this is method is called twice with the same filename
            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            hierarchy.Root.RemoveAllAppenders();

            FileAppender fileAppender = new FileAppender();
            fileAppender.Layout = new PatternLayout(DEFAULT_LOG_PATTERN);
            fileAppender.File = logFilePath;
            fileAppender.ActivateOptions();

            SetLogAppender
                (
                hierarchy.Root,
                fileAppender,
                Level.Debug
                );
        }

        /// <summary>
        /// Sets the Appender that the ADK will use for logging.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="appender"></param>
        /// <param name="level"></param>
        private static void SetLogAppender(Logger logger,
                                            IAppender appender,
                                            Level level)
        {
            if (logger != null)
            {
                logger.RemoveAllAppenders();
                logger.AddAppender(appender);
                logger.Repository.Threshold = level;
                logger.Repository.Configured = true;
            }
            else
            {
                throw new AdkException
                    ("Unable to initialize log4net framework, ADK Logger is null", null);
            }
        }


        /// <summary>  Determines if the specified version of SIF is supported by the Adk</summary>
        /// <param name="version">The version of SIF </param>
        /// <returns>True if the specified version is supported by the ADK</returns>
        public static bool IsSIFVersionSupported(SifVersion version)
        {
            return Array.BinarySearch(sSupportedVersionsArray, version) > -1;
        }


        /// <summary>
        /// The SIFFormatter used by default for backwards-compatible non-typed
        /// APIs in the ADK, such as <see cref="Element.TextValue"/>. The default
        /// formatter used by the ADK is the SIF 1.x formatter for backwards compatibility.
        /// If you are using the strongly-typed APIs, such as <see cref="Element.TextValue"/>,
        /// this setting has no effect.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default SIFFormatter used by the ADK is the formatter for SIF 1.5.
        /// This means that agents that were based on the 1.x version of the ADK will
        /// continue to get the SIF 1.x String version when calling an API that returns
        /// a string value from a SIF Element.
        /// </para>
        /// </remarks>
        public static SifFormatter TextFormatter
        {
            get
            {
                CheckInit();
                return sSingleton.fDefaultFormatter;
            }
            set
            {
                CheckInit();
                sSingleton.fDefaultFormatter = value;
            }
        }


        /// <summary>
        /// The Dtd that the ADK is currently using to parse and write SIF Objects
        /// </summary>
        public static IDtd Dtd
        {
            get { return sDtd; }
        }


        /// <summary>Gets the version of SIF used by the agent</summary>
        public static SifVersion SifVersion
        {
            get
            {
                CheckInit();
                return sSingleton.fVersion;
            }
            set
            {
                if (!IsSIFVersionSupported(value))
                    throw new AdkNotSupportedException("SIF " + sSingleton.fVersion + " is not supported by the ADK", null);

                sSingleton.fVersion = (value == null ? SifVersion.LATEST : value);

                if (debug != AdkDebugFlags.None)
                {
                    Log.DebugFormat("Using SIF {0}", sSingleton.fVersion);
                }

            }
        }

        /// <summary>  Gets the Adk build version</summary>
        /// <returns> The Adk build version string (e.g. "1.0.4")
        /// </returns>
        public static Version AdkVersion
        {
            get { return typeof(Adk).Assembly.GetName().Version; }
        }

        /// <summary>Gets a reference to the global ISIFPrimitives object used for SIF messaging</summary>
        internal static ISIFPrimitives Primitives
        {
            get
            {
                CheckInit();
                if (sSingleton.fImpl == null)
                {
                    sSingleton.fImpl = new SIFPrimitives();
                }
                return sSingleton.fImpl;
            }
        }


        /// <summary>  Utility to check that the Adk has been initialized.
        /// @throws an InternalError if the <c>initialize</c> function has not been
        /// successfully called
        /// </summary>
        private static void CheckInit()
        {
            if (!Initialized)
            {
                throw new ApplicationException
                    ("The Adk is not initialized. Please call Adk.Initialize()");
            }
        }

        /// <summary>  The root log Category. Subcategories exist for the Agent and each zone,
        /// where the hierarchy is "Adk.Agent$<i>zoneId</i>". The Adk uses the root
        /// Category when writing log events prior to the initialization of the
        /// Agent class.
        /// </summary>
        private static ILog sLog = LogManager.GetLogger(LOG_IDENTIFIER);

        /// <summary>  The root ServerLog instance. Subcategories exist for the Agent and each 
        /// zone, where the hierarchy is "ADK.Agent$<i>zoneId</i>". The Adk uses the 
        /// root ServerLog instance only to establish the global chain of loggers;
        /// no actual logging is performed outside the context of a zone.
        /// </summary>
        private static ServerLog sServerLog = ServerLog.GetInstance(LOG_IDENTIFIER, null);

        /// <summary>Static single instance of the Adk object for this  machine </summary>
        private static Adk sSingleton;

        /// <summary>
        /// The default SifFormatter used by the agent if not formatter is specified.
        /// By default, the formatter that is selected is the SIF 1.x formatter
        /// </summary>
        private SifFormatter fDefaultFormatter;

        private static IDtd sDtd = null;

        /// <summary>Global primitives object </summary>
        private ISIFPrimitives fImpl;


        private Adk()
        {
            // This class can only be initialized from Initialize()
        }

        private static bool _initLogging = false;

        private static void InitLogging()
        {
            if (!_initLogging)
            {
                string logFile = Properties.GetProperty("adk.log.file");
                if (logFile != null)
                {
                    try
                    {
                        SetLogFile(logFile);
                    }
                    catch (IOException ioe)
                    {
                        throw new AdkException
                            ("Could not initialize log file (\"+logFile+\"): " + ioe, null, ioe);
                    }
                }
                else
                {
                    // No logging to file is configured. Add a console appender, but only if there are no 
                    // appenders currently configured in the logging framework and no appenders configured in
                    // The ADK's logger. The appender is added to the root. That way any other components that
                    // are using log4net will write to the same log. 

                    // NOTE: If someone wishes to use their own custom appenders, they can initialize log4net
                    // before calling Adk.Initialize(). This method must make sure that it does no initialization
                    // of log4net if it has already been done.
                    Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
                    if (hierarchy != null &&
                         !hierarchy.Configured &&
                         hierarchy.Root.Appenders.Count == 0)
                    {
                        Logger adkLog = Log.Logger as Logger;
                        if (adkLog.Appenders.Count == 0)
                        {
                            ConsoleAppender consoleApp = new ConsoleAppender();
                            consoleApp.Layout = new PatternLayout(DEFAULT_LOG_PATTERN);
                            consoleApp.ActivateOptions();
                            SetLogAppender(hierarchy.Root, consoleApp, Level.Debug);
                        }
                    }
                }
                _initLogging = true;
            }
        }


        /// <summary>  The Adk debugging level determines which types of messages will be
        /// submitted to the logging environment by the class framework.</summary>
        /// <remarks>
        /// To eliminate all Adk-generated debug log messages set this value to <c>AdkDebugFlags.None</c>. The default is
        /// <c>AdkDebugFlags.Very_Detailed</c>, which includes all debug flags except <c>AdkDebugFlags.Message_Content</c>.
        /// </remarks>
        public static AdkDebugFlags Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        private static AdkDebugFlags debug = AdkDebugFlags.Very_Detailed;


        #endregion
    }
}

// Synchronized with ADK.Java Branch Library-ADK-1.5.0 Version 5
