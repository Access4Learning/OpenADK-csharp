//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using OpenADK.Library.Impl;
using OpenADK.Library.Log;
using log4net;
using OpenADK.Library.us;

namespace OpenADK.Library
{
    /// <summary>  The base class for a SIF Agent.
    /// 
    /// Derive your own class from this one and pass the <i>SourceId</i> to the
    /// superclass constructor. Call the <c>initialize</c> method to initialize
    /// the agent prior to use.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class Agent
    {
        /// <summary>
        /// The Identifier that is used to identify the ADK itself for logging operations ("ADK")
        /// </summary>
        public const string LOG_IDENTIFIER = Adk.LOG_IDENTIFIER + ".Agent";


        /// <summary>  The root log Category. Subcategories exist for each zone, where the
        /// subcategory name is "ADK.Agent$<i>zoneId</i>". The Adk uses the root
        /// Category when writing log events that are not associated with a
        /// specific zone. Your agent may also use this Category to post log
        /// events.
        /// </summary>
        protected internal static ILog Log;

        /// <summary>  The root ServerLog instance. Subcategories exist for each zone, where 
        /// the subcategory name is "ADK.Agent$<i>zoneId</i>". The Agent uses the
        /// root ServerLog instance only to establish the agent-global chain of 
        /// loggers; no actual logging is performed outside the context of a zone.
        /// </summary>
        protected internal static ServerLog serverLog;

        /// <summary>  The agent's SourceId</summary>
        protected internal string fSourceId;

        /// <summary>  The display name used in SIF_Register/SIF_Name and in HTTP "UserAgent" headers</summary>
        protected internal string fName;

        /// <summary>  The IZoneFactory for this agent. Each agent has its own IZoneFactory in
        /// case the application vendor creates multiple Agent instances in the same
        /// virtual machine, a rare case but one that it supported
        /// </summary>
        private IZoneFactory fZoneFactory;

        /// <summary>  The ITopicFactory for this agent. Each agent has its own ITopicFactory in
        /// case the application vendor creates multiple Agent instances in the same
        /// virtual machine, a rare case but one that it supported
        /// </summary>
        private ITopicFactory fTopicFactory;

        /// <summary>  The UndeliverableMessageHandler for the agent</summary>
        private IUndeliverableMessageHandler fErrHandler;

        /// <summary>  Agent and default zone properties</summary>
        private AgentProperties fProps;

        /// <summary>  Initialization state</summary>
        private bool fInit = false;

        /// <summary>  Is the agent in the process of shutting down?</summary>
        private bool fShutdownInProgress = false;

        /// <summary>  Has the agent been shutdown?</summary>
        private bool fShutdown = false;

        /// <summary>
        /// The Object that configured this instance of the agent, e.g. an instance of AgentConfig
        /// </summary>
        private Object fConfigurationSource;

        /// <summary>  The Subscribers registered with this agent. The map is keyed by SIF
        /// data object names (e.g. "StudentPersonal"). If a ISubscriber is registered
        /// for all object types, it is keyed by ElementDef SIFDTD.SIF_MESSAGE.
        /// </summary>
        protected internal IDictionary<IElementDef, ISubscriber> fSubs = new Dictionary<IElementDef, ISubscriber>();

        /// <summary>  The Publishers registered with this agent. The map is keyed by SIF
        /// data object names (e.g. "StudentPersonal"). If a IPublisher is registered
        /// for all object types, it is keyed by ElementDef SIFDTD.SIF_MESSAGE.
        /// </summary>
        protected internal IDictionary<IElementDef, IPublisher> fPubs = new Dictionary<IElementDef, IPublisher>();


        /// <summary>  The IQueryResults objects registered with this agent. The map is keyed by
        /// SIF data object names (e.g. "StudentPersonal"). If a IQueryResults object
        /// is registered for all object types, it is keyed by ElementDef SIFDTD.SIF_MESSAGE.
        /// </summary>
        protected internal IDictionary<IElementDef, IQueryResults> fQueryResults =
            new Dictionary<IElementDef, IQueryResults>();

        /// <summary> 	Manages the MessagingListeners</summary>
        private List<IMessagingListener> fMessagingListeners = new List<IMessagingListener>();


        /// <summary>
        /// The TransportManager instances that manages all open transports for this agent 
        /// </summary>
        private readonly TransportManagerImpl fTransportManager;


        /// <summary>  Gets the IZoneFactory for this agent. The IZoneFactory is used to create
        /// IZone instances to represent logical SIF zones. An application can also
        /// call IZoneFactory methods to enumerate the Zones that have been created.
        /// 
        /// 
        /// </summary>
        /// <returns> The agent's IZoneFactory
        /// </returns>
        public virtual IZoneFactory IZoneFactory
        {
            get
            {
                _checkInit();
                return fZoneFactory;
            }
        }

        /// <summary>  Gets the ITopicFactory for this agent. The ITopicFactory is used to create
        /// Topic instances to aggregate publish and subscribe activity for a given
        /// type of SIF data object across one or more zones. An application can
        /// also call ITopicFactory methods to enumerate the Topics that have been
        /// created.
        /// 
        /// </summary>
        /// <returns> The agent's ITopicFactory
        /// </returns>
        public virtual ITopicFactory TopicFactory
        {
            get
            {
                _checkInit();
                return fTopicFactory;
            }
        }

        /// <summary>  Gets the ZoneFactory for this agent. The ZoneFactory is used to create
        /// Zone instances to represent logical SIF zones. An application can also
        /// call ZoneFactory methods to enumerate the Zones that have been created.
        /// 
        /// 
        /// </summary>
        /// <returns> The agent's ZoneFactory
        /// </returns>
        public virtual IZoneFactory ZoneFactory
        {
            get
            {
                _checkInit();
                return fZoneFactory;
            }
        }


        /// <summary>  Gets the properties for this agent.
        /// 
        /// The agent properties serve as defaults for new IZone objects created by
        /// the IZoneFactory. Properties may be customized on a zone-by-zone basis.
        /// If a property is not specified for a given zone, its value is inherited
        /// from the AgentProperties object returned by this method. Note this
        /// method returns the same object as getDefaultZoneProperties.
        /// 
        /// Agent properties should be set prior to calling the Agent.initialize
        /// method.
        /// 
        /// </summary>
        /// <returns> The agent properties
        /// </returns>
        public virtual AgentProperties Properties
        {
            get
            {
                if ( fProps == null )
                {
                    fProps = new AgentProperties( this );
                }

                return fProps;
            }
        }

        /// <summary>  Convenience method to get the default HTTP transport properties</summary>
        public virtual HttpProperties DefaultHttpProperties
        {
            get { return (HttpProperties) GetDefaultTransportProperties( "http" ); }
        }

        /// <summary>  Convenience method to get the default HTTPS transport properties</summary>
        public virtual HttpsProperties DefaultHttpsProperties
        {
            get { return (HttpsProperties) GetDefaultTransportProperties( "https" ); }
        }

        /// <summary>  Gets the default properties for Zones created by the IZoneFactory.</summary>
        /// <returns> The default zone properties
        /// </returns>
        public virtual AgentProperties DefaultZoneProperties
        {
            get { return Properties; }
        }


        /// <summary>  Gets the agent's <i>home directory</i>. By default, the home directory
        /// is the directory the agent was started from (if defined, the "adk.home"
        /// System property overrides this value.) You may override this method to
        /// return a home directory specific to your product's installation
        /// directory structure.
        /// 
        /// The Agent Runtime creates all work directories and files relative to
        /// the home directory. In some cases the Class Framework creates workspaces
        /// even when the Agent Runtime is not enabled.
        /// 
        /// </summary>
        /// <returns> The agent's home directory
        /// </returns>
        public virtual string HomeDir
        {
            get
            {
                string dir = Properties.GetProperty( "adk.home" );
                if ( dir != null )
                {
                    return dir;
                }
                else
                {
                    // default to the current process directory
                    return Environment.CurrentDirectory;
                }
            }
        }


        public string WorkDir
        {
            get
            {
                string home = HomeDir;
                if ( !home.EndsWith( Path.DirectorySeparatorChar.ToString() ) )
                {
                    home += Path.DirectorySeparatorChar;
                }
                return home + "work";
            }
        }

        /// <summary>Gets or sets the agent's SourceId</summary>
        /// <value> The string name that uniquely identifies this agent in a SIF IZone.
        /// </value>
        /// <remarks>
        /// ID is a string name that uniquely identifies this agent in
        /// a SIF IZone. SIF does not specify any restrictions on the length or
        /// characters that may appear in the SourceId.
        /// </remarks>
        public virtual string Id
        {
            get { return fSourceId; }

            set
            {
#if PROFILED
				{
					// TODO: Implement Support for Profiling
					Agent.log.debug( "SIF Profiling Harness support enabled in ADK" );

					//	Establish the SIFProfilerClient name (i.e. "sourceId_ADK")
					ProfilerUtils.setProfilerName( getId() + "_ADK" );
				}
#endif
                fSourceId = value;
            }
        }

        /// <summary>Gets or sets the descriptive name of the SIF Agent.</summary>
        /// <value>A descriptive name for the agent</value>
        /// <remarks>
        /// <para>This string is used to identify the agent whenever a descriptive name is
        /// preferred over the agent ID. The class framework uses this string for the
        /// value of the &lt;SIF_Name&gt; when during agent registration.</para>
        /// <para>By default, the agent ID is used as the descriptive name. This method
        /// must be called at agent initialization time prior to connecting to
        /// zones.</para>
        /// </remarks>
        /// <seealso cref="Id"/>
        public virtual string Name
        {
            get
            {
                if ( fName == null )
                {
                    return Id;
                }

                return fName;
            }

            set { fName = value; }
        }

        /// <summary>  Determines if the agent has been initialized</summary>
        /// <returns> true if the <c>initialize</c> method was called successfully;
        /// false if that method has not been called successfully or the agent
        /// has since been shut down.
        /// </returns>
        public virtual bool Initialized
        {
            get { return fInit; }
        }

        /// <summary>  Determines if the agent has been shutdown.
        /// 
        /// </summary>
        /// <returns> true if the <c>shutdown</c> method was called and the
        /// agent is either in the process of shutting down or has finished
        /// shutting down. Note the agent is considered to be shutdown even
        /// if the <c>shutdown</c> method fails.
        /// </returns>
        public virtual bool IsShutdown
        {
            get { return fShutdownInProgress || !fInit; }
        }



        /// <summary>Return a read-only collection of all MessagingListeners registered with the agent</summary>
        public virtual ICollection<IMessagingListener> MessagingListeners
        {
            get { return fMessagingListeners.AsReadOnly(); }
        }

        /// <summary>Gets or sets the IUndeliverableMessageHandler to be called when a dispatching
        /// error occurs on a zone but no handler is registered with that zone. </summary>
        /// <value>The handler to call when the Adk cannot dispatch an inbound message</value>
        /// <seealso cref="ErrorHandler"/>
        public virtual IUndeliverableMessageHandler ErrorHandler
        {
            get { return fErrHandler; }

            set { fErrHandler = value; }
        }


        static Agent()
        {
            Log = LogManager.GetLogger( LOG_IDENTIFIER );
            serverLog = ServerLog.GetInstance( LOG_IDENTIFIER, null );
        }

        /// <summary>Constructor</summary>
        /// <param name="agentId">The string name that uniquely identifies this agent in SIF Zones.
        /// This string is used as the <c>SourceId</c> in all SIF message
        /// headers created by the agent.
        /// </param>
        public Agent( string agentId )
        {
            if ( agentId == null || agentId.Trim().Length == 0 )
            {
                AdkUtils._throw
                    ( new ArgumentException( "Agent ID cannot be null or a blank string" ), Log );
            }

            fSourceId = agentId;

            ObjectFactory factory = ObjectFactory.GetInstance();
            fZoneFactory = (IZoneFactory) factory.CreateInstance( ObjectFactory.ADKFactoryType.ZONE, this );
            fTopicFactory = (ITopicFactory) factory.CreateInstance( ObjectFactory.ADKFactoryType.TOPIC, this );

            fTransportManager = new TransportManagerImpl();
        }

      
        /// <summary>  Gets the default properties for a transport protocol.</summary>
        /// <remarks>
        /// Each transport protocol supported by the Adk is represented by a class
        /// that implements the Transport interface. Transports are identified by
        /// a string such as "http" or "https". Like Zones, each Transport instance
        /// is associated with a set of properties specific to the transport
        /// protocol. Such properties may include IP address, port, SSL security
        /// attributes, and so on. The default properties for a given transport
        /// protocol may be obtained by calling this method.
        /// </remarks>
        /// <param name="protocol">The protocol to get default properties for e.g. "http"</param>
        /// <returns> The default properties for the specified protocol</returns>
        /// <exception cref="AdkTransportException"> is thrown if the protocol is not supported
        /// by the Adk
        /// </exception>
        public virtual TransportProperties GetDefaultTransportProperties( string protocol )
        {
            return fTransportManager.GetDefaultTransportProperties( protocol );
        }

        /// <summary>  Initialize the agent.</summary>
        /// <remarks>
        /// An application must call this method to initialize the class framework
        /// and runtime. No other methods can be called until the agent has been
        /// successfully initialized. When the agent exits it is important that the
        /// <c>shutdown</c> method be called to safely release the resources
        /// allocated by the runtime.
        /// 
        /// If an application overrides this method, it should call the superclass
        /// implementation <i>after</i> performing its own initialization.
        /// </remarks>
        /// <seealso cref="Shutdown()"></seealso>
        /// <exception cref="Exception">If the agent is unable to initialize due to a file or resource exception</exception>
        /// <exception cref="AdkException">Thrown if the agent has already
        /// been initialized
        /// </exception>
        public virtual void Initialize()
        {
            lock ( this )
            {
                if ( fInit )
                {
                    AdkUtils._throw( new AdkException( "Agent is already initialized", null ), Log );
                }
                if ( fShutdownInProgress )
                {
                    AdkUtils._throw
                        ( new AdkException( "Agent is in the process of shutting down", null ), Log );
                }

#if EVAL
                try
                {
                    new E( this );
                }
                catch( Exception )
                {
                    Console.WriteLine( "Corrupt ADK Evaluation Edition or internal error. Please report this notice to OpenADK Tech Support." );
                    Environment.Exit( 0 );
                }
#endif

                if ( (Adk.Debug & AdkDebugFlags.Lifecycle) != 0 )
                {
                    Log.Info( "Initializing agent..." );
                }

                //  Verify home directory exists and can be written to
                AssertDirectoryExists( HomeDir, "Home" );
                //  Verify work directory exists and can be written to
                AssertDirectoryExists( WorkDir, "Work" );

#if PROFILED
				{
					// TODO: Implement Support for Profiling
					System.out.println( "SIF Profiling Harness support enabled in ADK" );

					//	Establish the SIFProfilerClient name (i.e. "sourceId_ADK")
					ProfilerUtils.setProfilerName( getId() + "_ADK" );
				}
#endif
                //Initialize the TransportManager
                fTransportManager.Activate( this );


                fShutdownInProgress = false;
                fInit = true;

                if ( (Adk.Debug & AdkDebugFlags.Lifecycle) != 0 )
                {
                    Log.Info( "Agent initialized" );
                }
            }
        }

        private void AssertDirectoryExists( string path,
                                            string logName )
        {
            DirectoryInfo dir = new DirectoryInfo( path );
            if ( !dir.Exists )
            {
                if ( (Adk.Debug & AdkDebugFlags.Lifecycle) != 0 )
                {
                    Log.Debug
                        ( string.Format( "Creating {0} directory: {1}", logName, dir.FullName ) );
                }
                dir.Create();
            }

            if ( !Directory.Exists( dir.FullName ) )
            {
                AdkUtils._throw
                    ( new AdkException
                          ( string.Format
                                ( "The {0} directory is not a directory: {1}", logName, path ), null ),
                      Log );
            }
        }

        /// <summary>  Shutdown the agent.
        /// 
        /// This method should always be called before the application ends. It
        /// closes resources held by the class framework and runtime.
        /// 
        /// Calling this form of shutdown does not send any provisioning messages.
        /// </summary>
        public virtual void Shutdown()
        {
            Shutdown( ProvisioningFlags.None );
        }

        /// <summary>  Shutdown the agent.</summary>
        /// <remarks>
        /// <para>
        /// This method should always be called before the application ends. It
        /// closes resources held by the Adk Class Framework and Adk Agent Runtime.
        /// </para>
        /// <para>
        /// Provisioning messages are sent as follows:
        /// 
        /// <ul>
        /// <li>
        /// If the agent is using Adk-managed provisioning, a <c>&lt;
        /// SIF_Unregister&gt;</c> message is sent to each zone to which
        /// the agent is connected if the AdkFlags.PROV_UNREGISTER
        /// flag is specified. <c>&lt;SIF_Unsubscribe&gt;</c> and
        /// <c>&lt;SIF_Unprovide&gt;</c> messages are sent to each
        /// zone joined to a Topic when the AdkFlags.PROV_UNSUBSCRIBE
        /// and AdkFlags.PROV_UNPROVIDE flags are specified, respectively.
        /// When Adk-managed provisioning is disabled, no provisioning
        /// messages are sent to zones.
        /// </li>
        /// <li>
        /// If Agent-managed provisioning is enabled, the ProvisioningOptions
        /// flags have no affect. The agent must explicitly call the
        /// IZone.sifUnregister, IZone.sifUnsubscribe, and IZone.sifUnprovide
        /// methods to manually send those messages to each zone.
        /// </li>
        /// <li>
        /// If ZIS-managed provisioning is enabled, no provisioning messages
        /// are sent by the agent regardless of the ProvisioningOptions
        /// used and the methods are called.
        /// </li>
        /// </ul>
        /// </para>
        /// </remarks>
        /// <param name="provisioningOptions">The options from the AdkFlags enum</param>
        /// 
        /// <seealso cref="Initialize">
        /// </seealso>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public virtual void Shutdown( ProvisioningFlags provisioningOptions )
        {
            if ( !fInit )
            {
                return;
            }

            fShutdownInProgress = true;
            if ( (Adk.Debug & AdkDebugFlags.Lifecycle) != 0 )
            {
                Log.Info( "Shutting down agent..." );
            }

            //	Close the SIFProfilerClient if supported
#if PROFILED
			{
				// TODO: Implement Support for Profiling
				com.OpenADK.sifprofiler.SIFProfilerClient prof = 
					com.OpenADK.sifprofiler.SIFProfilerClient.getInstance( ProfilerUtils.getProfilerName() );
				if( prof != null ) 
				{
					try 
					{
						prof.close();
					} 
					catch( Exception ex ) 
					{
					}
				}
			}
#endif

            try
            {
                //  JAVA_TODO:  Unsubscribe, Unprovide topics

                //  Disconnect and shutdown each zone...
                IZoneFactory zf = IZoneFactory;
                IZone[] zones = zf.GetAllZones();
                for ( int i = 0; i < zones.Length; i++ )
                {
                    zones[i].Disconnect( provisioningOptions );
                    ((ZoneImpl) zones[i]).Shutdown();
                }

                if ( fTransportManager != null )
                {
                    //  Shutdown transports
                    if ( (Adk.Debug & AdkDebugFlags.Lifecycle) != 0 )
                    {
                        Log.Info( "Shutting down Transports..." );
                    }
                    fTransportManager.Shutdown();
                }

                //  Close RequestCache
                try
                {
                    RequestCache rc = RequestCache.GetInstance( this );
                    if ( rc != null )
                    {
                        rc.Close();
                    }
                }
                catch
                {
                    // Do nothing
                }

                if ( (Adk.Debug & AdkDebugFlags.Lifecycle) != 0 )
                {
                    Log.Debug( "Agent shutdown complete" );
                }
            }
            finally
            {
                fInit = false;
                fShutdown = true;
            }
        }

        /// <summary>  Puts all connected zones into sleep mode.
        /// 
        /// For each zone in the connected state, a SIF_Sleep message is sent to the
        /// IZone Integration Server to request that this agent's queue be put into
        /// sleep mode for that zone. If successful, the ZIS should not deliver
        /// further messages to this agent until it is receives a SIF_Register or
        /// SIF_Wakeup message from the agent. Note the Adk keeps an internal sleep
        /// flag for each zone, which is initialized when the <c>connect</c>
        /// method is called by sending a SIF_Ping to the ZIS. This flag is set so
        /// that the Adk will return a Status Code 8 ("Receiver is sleeping") in
        /// response to any message received by the ZIS for the duration of the
        /// session.
        /// 
        /// 
        /// </summary>
        /// <exception cref="AdkException">  thrown if the SIF_Sleep message is unsuccessful.
        /// The Adk will attempt to send a SIF_Sleep to all connected zones; the
        /// exception describes the zone or zones that failed
        /// </exception>
        public virtual void Sleep()
        {
            lock ( this )
            {
                _checkInit();

                AdkMessagingException err = null;

                IZone[] zones = fZoneFactory.GetAllZones();
                for ( int i = 0; i < zones.Length; i++ )
                {
                    try
                    {
                        zones[i].Sleep();
                    }
                    catch ( AdkMessagingException ex )
                    {
                        if ( err == null )
                        {
                            err =
                                new AdkMessagingException
                                    ( "An error occurred sending SIF_Sleep to zone \"" +
                                      zones[i].ZoneId + "\"", zones[i] );
                        }
                        err.Add( ex );
                    }
                }

                if ( err != null )
                {
                    AdkUtils._throw( err, Log );
                }
            }
        }

        /// <summary>  Wakes up all connected zones if currently in sleep mode.
        /// 
        /// For each connected zone, a SIF_Wakeup message is sent to the IZone
        /// Integration Server to request that sleep mode be removed from this agent's
        /// queue for that zone. Note the Adk keeps an internal sleep flag for each
        /// zone, which is initialized when the <c>connect</c> method is called
        /// by sending a SIF_Ping to the ZIS. This flag is cleared so that the Adk
        /// will no longer return a Status Code 8 ("Receiver is sleeping") in response
        /// to messages received by the ZIS.
        /// 
        /// 
        /// </summary>
        /// <exception cref="AdkException">  thrown if the SIF_Wakeup message is unsuccessful.
        /// The Adk will attempt to send a SIF_Wakeup to all connected zones; the
        /// exception describes the zone or zones that failed
        /// </exception>
        public virtual void Wakeup()
        {
            lock ( this )
            {
                _checkInit();

                AdkMessagingException err = null;

                IZone[] zones = fZoneFactory.GetAllZones();
                for ( int i = 0; i < zones.Length; i++ )
                {
                    try
                    {
                        zones[i].WakeUp();
                    }
                    catch ( AdkMessagingException ex )
                    {
                        if ( err == null )
                        {
                            err =
                                new AdkMessagingException
                                    ( "An error occurred sending SIF_Wakeup to zone \"" +
                                      zones[i].ZoneId + "\"", zones[i] );
                        }
                        err.Add( ex );
                    }
                }

                if ( err != null )
                {
                    AdkUtils._throw( err, Log );
                }
            }
        }

        /// <summary>  Register a global IPublisher message handler with this agent for all SIF object types.
        /// 
        /// Note agents typically register message handlers with Topics or with Zones
        /// instead of with the Agent. The message fDispatcher first
        /// delivers messages to Topics, then to Zones, and finally to the Agent
        /// itself.
        /// 
        /// In order to receive SIF_Request messages, the agent is expected to be
        /// registered as a Provider of one or more object types in at least one
        /// zone. This method does not send SIF_Provide messages to any zones.
        /// 
        /// </summary>
        /// <param name="IPublisher">An object that implements the <c>IPublisher</c>
        /// interface to respond to SIF_Request queries received by the agent.
        /// This object will be called whenever a SIF_Request is received by
        /// and no other object in the message dispatching chain has processed
        /// the message.
        /// </param>
        public virtual void SetPublisher( IPublisher IPublisher )
        {
            SetPublisher( IPublisher, null );
        }

        /// <summary>  Register a global IPublisher message handler with the agent for the specified SIF object type.
        /// 
        /// Note agents typically register message handlers with Topics or with Zones
        /// instead of with the Agent. The message fDispatcher first delivers messages 
        /// to Topics, then to Zones, and finally to the Agent itself.
        /// 
        /// In order to receive SIF_Request messages, the agent is expected to be
        /// registered as a Provider of one or more object types in at least one
        /// zone. This method does not send SIF_Provide messages to any zones.
        /// 
        /// </summary>
        /// <param name="publisher">An object that implements the <c>IPublisher</c>
        /// interface to respond to SIF_Request queries received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This IPublisher will be called whenever a
        /// SIF_Request is received and no other object in the message dispatching
        /// chain has processed the message.
        /// 
        /// </param>
        /// <param name="objectType">A constant from the SifDtd class that identifies the
        /// type of SIF Data Object this IPublisher will respond to.
        /// </param>
        public virtual void SetPublisher( IPublisher publisher,
                                          IElementDef objectType )
        {
            if ( publisher == null )
            {
                AdkUtils._throw
                    ( new ArgumentException( "IPublisher object cannot be null" ), GetLog() );
            }

            if ( objectType == null )
            {
                fPubs[SifDtd.SIF_MESSAGE] = publisher;
            }
            else
            {
                fPubs[objectType] = publisher;
            }
        }

        /// <summary>  Register a global ISubscriber message handler with the agent for 
        /// all SIF object types in the SIF_Default context
        /// 
        /// Note agents typically register message handlers with Topics or with Zones
        /// instead of with the Agent. The message fDispatcher first
        /// delivers messages to Topics, then to Zones, and finally to the Agent
        /// itself.
        /// 
        /// In order to receive SIF_Event messages, the agent is expected to be
        /// registered as a ISubscriber of one or more object types in at least one
        /// zone. This method does not send SIF_Subscribe messages to any zones.
        /// 
        /// </summary>
        /// <param name="subscriber">An object that implements the <c>ISubscriber</c>
        /// interface to respond to SIF_Event notifications received by the agent.
        /// This object will be called whenever a SIF_Event is received and no
        /// other object in the message dispatching chain has processed the
        /// message.
        /// </param>
        public virtual void SetSubscriber( ISubscriber subscriber )
        {
            SetSubscriber( subscriber, null );
        }

        /// <summary>  Register a global ISubscriber message handler with this agent 
        /// for the specified SIF object type in the default SIF Context.
        /// 
        /// Note agents typically register message handlers with Topics or with Zones
        /// instead of with the Agent. The message fDispatcher first
        /// delivers messages to Topics, then to Zones, and finally to the Agent
        /// itself.
        /// 
        /// In order to receive SIF_Event messages, the agent is expected to be
        /// registered as a ISubscriber of one or more object types in at least one
        /// zone. This method does not send SIF_Subscribe messages to any zones.
        /// 
        /// </summary>
        /// <param name="subscriber">An object that implements the <c>ISubscriber</c>
        /// interface to respond to SIF_Event notifications received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This ISubscriber will be called whenever a
        /// SIF_Event is received and no other object in the message dispatching
        /// chain has processed the message.
        /// 
        /// </param>
        /// <param name="objectType">A constant from the SifDtd class that identifies the
        /// type of SIF Data Object this ISubscriber will respond to.
        /// </param>
        public virtual void SetSubscriber( ISubscriber subscriber,
                                           IElementDef objectType )
        {
            if ( subscriber == null )
            {
                AdkUtils._throw
                    ( new ArgumentException( "ISubscriber object cannot be null" ), GetLog() );
            }

            if ( objectType == null )
            {
                fSubs[SifDtd.SIF_MESSAGE] = subscriber;
            }
            else
            {
                fSubs[objectType] = subscriber;
            }
        }

        /// <summary>  Register a global IQueryResults message handler with this agent for all SIF object types.
        /// 
        /// Note agents typically register message handlers with Topics or with
        /// Zones instead of with the Agent. The message fDispatcher first
        /// delivers messages to Topics, then to Zones, and finally to the Agent
        /// itself.
        /// 
        /// </summary>
        /// <param name="IQueryResults">An object that implements the <c>IQueryResults</c>
        /// interface to respond to SIF_Response query results received by the
        /// agent. This object will be called whenever a SIF_Response is received
        /// and no other object in the message dispatching chain has processed
        /// the message.
        /// </param>
        public virtual void SetQueryResults( IQueryResults IQueryResults )
        {
            SetQueryResults( IQueryResults, null );
        }

        /// <summary>  Register a global IQueryResults message handler object with this 
        /// agent for the specified SIF object type in the default SIF Context
        /// 
        /// Note agents typically register message handlers with Topics or with
        /// Zones instead of with the Agent. The message fDispatcher first
        /// delivers messages to Topics, then to Zones, and finally to the Agent
        /// itself.
        /// 
        /// </summary>
        /// <param name="queryResults">An object that implements the <c>IQueryResults</c>
        /// interface to respond to SIF_Response query results received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This IQueryResults object will be called whenever
        /// a SIF_Response is received and no other object in the message
        /// dispatching chain has processed the message.
        /// 
        /// </param>
        /// <param name="objectType">A constant from the SifDtd class that identifies the
        /// type of SIF Data Object this IQueryResults message handler will
        /// respond to.
        /// </param>
        public virtual void SetQueryResults( IQueryResults queryResults,
                                             IElementDef objectType )
        {
            if ( queryResults == null )
            {
                AdkUtils._throw
                    ( new ArgumentException( "IQueryResults object cannot be null" ), GetLog() );
            }

            if ( objectType == null )
            {
                fQueryResults[SifDtd.SIF_MESSAGE] = queryResults;
            }
            else
            {
                fQueryResults[objectType] = queryResults;
            }
        }

        /// <summary>  Gets the global IPublisher message handler registered with the Agent for the 
        /// specified SIF object type.</summary>
        /// <param name="context">The SIF context to look up the ReportPublisher for.
        /// The default implementaion of Agent only returns handlers for the  SIF default
        /// context.</param>/// <param name="objectType">A SifDtd constant identifying a SIF Data Object type
        /// (e.g. <c>SifDtd.STUDENTPERSONAL</c>)
        /// 
        /// </param>
        /// <returns> The IPublisher message handler registered for this object type by the 
        /// agent when it called the <c>setPublisher</c> method, or <c>null</c> if 
        /// no IPublisher has been registered for the specified object type.
        /// </returns>
        public virtual IPublisher GetPublisher( SifContext context, IElementDef objectType )
        {
            IPublisher p = null;
            if ( SifContext.DEFAULT.Equals( context ) )
            {
                p = (IPublisher) fPubs[objectType];
                if ( p == null )
                {
                    p = (IPublisher) fPubs[SifDtd.SIF_MESSAGE];
                }
            }

            return p;
        }





        /// <summary>  Gets the global ISubscriber message handler registered with the Agent for 
        /// the specified SIF object type.
        /// 
        /// </summary>
        ///  <param name="context">The SIF context to look up the ReportPublisher for.
        /// The default implementaion of Agent only returns handlers for the  SIF default
        /// context.</param>
        /// <param name="objectType">A SifDtd constant identifying a SIF Data Object type
        /// (e.g. <c>SifDtd.STUDENTPERSONAL</c>)
        /// 
        /// </param>
        /// <returns> The ISubscriber registered for this object type by the agent when
        /// it called the <c>setSubscriber</c> method, or <c>null</c> 
        /// if no ISubscriber has been registered for the specified object type.
        /// </returns>
        public virtual ISubscriber GetSubscriber( SifContext context, IElementDef objectType )
        {
            ISubscriber s = null;
            if ( SifContext.DEFAULT.Equals( context ) )
            {
                s = fSubs[objectType];
                if ( s == null )
                {
                    s = fSubs[SifDtd.SIF_MESSAGE];
                }
            }
            return s;
        }


        /// <summary>
        /// Gets the global QueryResults message handler registered with the Agent for the specified SIF object type.
        /// </summary>
        /// <param name="context">The SIF context to look up the QueryResults handler for.
        /// The default implementation of Agent only returns handlers for 
        /// SIFContext.DEFAULT</param>
        /// <param name="objectType">The QueryResults object registered for this object type by the
        /// agent when it called the <code>setQueryResults</code> method, or 
        /// <code>null</code> if no QueryResults object has been registered
        /// for the specified object type.</param>
        /// <returns></returns>
        public virtual IQueryResults GetQueryResults( SifContext context, IElementDef objectType )
        {
            IQueryResults q = null;
            if ( SifContext.DEFAULT.Equals( context ) )
            {
                q = (IQueryResults) fQueryResults[objectType];
                if ( q == null )
                {
                    q = (IQueryResults) fQueryResults[SifDtd.SIF_MESSAGE];
                }
            }
            return q;
        }


        /// <summary> 	Register a <i>MessagingListener</i> to listen to messages received by the
        /// message handlers of this class.
        /// 
        /// NOTE: Agents may register a MessagingListener with the Agent or IZone
        /// classes. When a listener is registered with both classes, it will be 
        /// called twice. Consequently, it is recommended that most implementations 
        /// choose to register MessagingListeners with only one of these classes 
        /// depending on whether the agent is interested in receiving global
        /// notifications or notifications on only a subset of zones.
        /// 
        /// </summary>
        /// <param name="listener">a MessagingListener implementation
        /// </param>
        public virtual void AddMessagingListener( IMessagingListener listener )
        {
            fMessagingListeners.Add( listener );
        }

        /// <summary> 	Remove a <i>MessagingListener</i> previously registered with the
        /// <c>addMessagingListener</c> method.
        /// 
        /// </summary>
        /// <param name="listener">a MessagingListener implementation
        /// </param>
        public virtual void RemoveMessagingListener( IMessagingListener listener )
        {
            fMessagingListeners.Remove( listener );
        }

        /// <summary>  Purge all pending incoming and/or outgoing messages from this agent's
        /// queue. Affects all zones with which the agent is currently connected.
        /// See also the Topic.purgeQueue and IZone.purgeQueue methods to purge all
        /// zones associated with a topic or a specific zone, respectively.
        /// 
        /// <ul>
        /// <li>
        /// If the Agent Local Queue is enabled, messages are permanently
        /// and immediately removed from the queue. Any messages in transit
        /// are not affected.
        /// </li>
        /// <li>
        /// If the underlying messaging protocol offers a mechanism to clear
        /// the agent's queue, it is invoked. (SIF 1.0 does not have such a
        /// mechanism.)
        /// <li>
        /// Otherwise, all incoming messages received by the agent having a
        /// timestamp earlier than or equal to the time this method is called
        /// are discarded. This behavior persists until the agent is
        /// terminated or until a message is received having a later
        /// timestamp.
        /// </li>
        /// </ul>
        /// 
        /// </summary>
        /// <param name="incoming">true to purge incoming messages
        /// </param>
        /// <param name="outgoing">true to purge outgoing messages (e.g. pending SIF_Events)
        /// when the Agent Local Queue is enabled
        /// </param>
        public virtual void PurgeQueue( bool incoming,
                                        bool outgoing )
        {
        }

        /// <summary>Utility method to generate a GUID for SIF Data Objects and messages</summary>
        /// <returns>A new GUID</returns>
        public static string MakeGUID()
        {
            return SifFormatter.GuidToSifRefID( Guid.NewGuid() );
        }

        /// <summary>  Gets the root logging Category for this agent.</summary>
        public static ILog GetLog()
        {
            return Log;
        }

        /// <summary>  Gets the logging framework Category for a specific zone.</summary>
        public static ILog GetLog( IZone zone )
        {
            ILog zlog = LogManager.GetLogger( Adk.LOG_IDENTIFIER + ".Agent$" + zone.ZoneId );
            return zlog == null ? Log : zlog;
        }

        /// <summary> 	Gets the agent-global ServerLog instance.
        /// 
        /// Agents that wish to customize server-side logging may call this 
        /// method to obtain the global Agent ServerLog instance. Call any of the 
        /// following methods to set up the chain of loggers that will be inherited 
        /// by all Zones:
        /// 
        /// <ul>
        /// <li><c>addLogger</c></li>
        /// <li><c>removeLogger</c></li>
        /// <li><c>clearLoggers</c></li>
        /// <li><c>getLoggers</c></li>
        /// </ul>
        /// 
        /// Unlike client-side logging, server logging requires a connection to a 
        /// IZone Integration Server. Because the current SIF 1.x infrastructure does 
        /// not allow connections to servers independent of a zone, the logging 
        /// methods of ServerLog are useful only when called within the context of a 
        /// zone. Therefore, calling any of the logging methods on the ServerLog 
        /// instance returned by this method will result in an IllegalStateException. 
        /// This method is provided only to set up the ServerLog logger chain at
        /// the global Agent level. 
        /// 
        /// </summary>
        /// <returns> The agent's ServerLog instance
        /// 
        /// @since Adk 1.5
        /// </returns>
        public static ServerLog GetServerLog()
        {
            return serverLog;
        }

        /// <summary> 	Gets the ServerLog for a specific zone.
        /// 
        /// This form of <c>getServerLog</c> is provided for consistency with 
        /// the <c>getLog</c> method. Note you may also call the 
        /// <c>IZone.getServerLog</c> method directly to obtain a ServerLog for 
        /// for a zone.
        /// 
        /// </summary>
        /// <param name="zone">The zone to obtain a ServerLog instance for
        /// </param>
        /// <returns> The ServerLog instance for the zone
        /// </returns>
        public static ServerLog GetServerLog( IZone zone )
        {
            return zone.ServerLog;
        }

        /// <summary>  Helper routine to check that the <c>initialize</c> method has been called
        /// @throws AdkException if not initialized
        /// </summary>
        private void _checkInit()
        {
            if ( fShutdown )
            {
                throw new LifecycleException( "Agent has been shutdown" );
            }

            if ( !fInit )
            {
                Console.Out.WriteLine( new StackTrace( true ).ToString() );
                AdkUtils._throw( new LifecycleException( "Agent not initialized" ), Log );
            }
        }


        /// <summary>
        ///  Gets and Sets the source of this agent's configuration. This property can be
        ///  used by other subsystems in the ADK to retrieve the configuration instance
        ///  and pull additional configuration information.
        /// </summary>
        /// <value>The object that was used to configure the agent.</value>
        public object ConfigurationSource
        {
            get { return fConfigurationSource; }
            set { fConfigurationSource = value; }
        }


        /// <summary>
        /// Returns the TransportManager instance for this agent.
        /// </summary>
        /// <remarks>
        /// TransportManager manages the state of all open transports
        /// known to this agent instance. The TransportManager will
        /// be null until <see cref="Initialize()"/>  is called.
        /// </remarks>
        public ITransportManager TransportManager
        {
            get { return fTransportManager; }
        }
    }
}
