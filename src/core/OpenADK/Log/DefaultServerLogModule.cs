//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Log
{
    /// <summary> 	The default <c>ServerLogModule</c> implementation for server logging. 
    /// An instance of this class is installed by default when the Adk is 
    /// initialized.</summary>
    /// <remarks>
    /// <para>
    /// DefaultServerLogModule writes to the server log by constructing and
    /// reporting a SIF_LogEntry object as an Add SIF_Event when the <c>log</c>
    /// method is called. Because SIF_LogEntry was introduced in SIF 1.5, an 
    /// event is only reported when the agent's default SIF Version is 1.5 or 
    /// later. No action is taken for earlier versions.
    /// </para>
    /// <para>
    /// By default, DefaultServerLogModule echos all messages to the zone's 
    /// local logging framework Category. Call the <c>setEcho</c> method to disable 
    /// this functionality. Follow these steps to obtain the DefaultServerLogModule 
    /// that is installed by the Adk when it is initialized:</para>
    /// <list type="bullet">
    ///	<item><term>
    ///			Access <c>Adk.ServerLog</c> to obtain the ServerLog
    ///			instance at the root of the logging chain
    ///		</term></item>
    ///		<item><term>
    ///			Use the <c>ServerLog.Loggers</c> property to obtain
    ///			an array of all <i>ServerLogModule</i>s
    ///	</term></item>
    ///		<item><term>
    ///			Read the <c>ID</c> property on each element in the
    ///			array until you encounter one that returns "DefaultServerLogModule".
    ///			Alternatively, you can use test each
    ///			element to determine if it is an instance of this class.
    ///		</term></item>
    ///		<item><term>
    ///			Set the <c>Echo</c> Property to enable/disable echoing of
    ///			SIF_LogEntry information to the local zone log. (SIF_LogEntry
    ///			is always echoed when the ADK is initialized to use a version
    ///			of SIF prior to 1.5)
    ///		</term></item>
    ///		<item><term>
    ///			Set the <c>ReportEvents</c> property to enable/disable the reporting
    ///			of SIF_LogEntry events to the zone. This method can be used to
    ///			disable SIF_LogEntry reporting to zones while continuing to allow 
    ///			log messages to be echoed to the local zone log.
    ///		</term></item>
    ///		</list>
    /// </remarks>
    public class DefaultServerLogModule : IServerLogModule
    {
        private bool fEcho = true;
        private bool fReportEvents = true;

        /// <summary> 	Gets the ID of this logger</summary>
        /// <value> The ID of this ServerLogModule instance
        /// </value>
        public String ID
        {
            get { return "DefaultServerLogModule"; }
        }


        /// <summary>
        /// Determines if SIF_LogEntry objects should be echoed to the local
        /// client-side zone log
        /// </summary>
        /// <value><c>TRUE</c> to echo SIF_LogEntry information to the client-side zone log
        /// <c>FALSE</c> to disable echoing. Note that SIF_LogEntry is always
        /// echoed to the local zone log when the ADK is initialized with a version of
        /// SIF prior to 1.5</value>
        public bool Echo
        {
            get { return fEcho; }

            set { fEcho = value; }
        }

        /// <summary>
        /// Determines if SIF_LogEntry objects are reported to the zone. This method can 
        /// be called to disable SIF_LogEntry reporting to zones while continuing to allow 
        /// log messages to be echoed to the local zone log.
        /// </summary>
        /// <value> <c>true</c> to report SIF_LogEntry events, <c>false</c>
        /// to disable reporting of events</value>
        public bool ReportEvents
        {
            get { return fReportEvents; }
            set { fReportEvents = value; }
        }

        /// <summary> 	Post a string message to the server log.
        /// 
        /// The implementation of this method constructs a SIF_LogEntry object 
        /// with the <c>LogLevel</c> attribute set to "Info" and the 
        /// <c>SIF_Desc</c> element set to the text message passed to
        /// the <c>message</c> parameter. The SIF_LogEntry object is then 
        /// reported to the zone as a SIF_Event by delegating to the <c>log</c>
        /// method that accepts a SIF_LogEntry parameter.
        /// 
        /// </summary>
        /// <param name="zone">The zone on the server to post the message to
        /// </param>
        /// <param name="message">The message text
        /// </param>
        public void Log( IZone zone,
                         String message )
        {
            //	If SIF 1.5 or later, encapsulate in a SIF_LogEntry and
            //	report it to the zone. Otherwise just write the message
            //	to the local zone log.
            if ( Adk.SifVersion.CompareTo( SifVersion.SIF15r1 ) >= 0 ) {
                SIF_LogEntry le = new SIF_LogEntry();
                le.SetLogLevel( LogLevel.INFO );
                le.SIF_Desc = message;
                Log( zone, le );
            }
            else {
                zone.Log.Debug( message );
            }
        }

        /// <summary> 	Post a SIF_LogEntry to the server.
        /// 
        /// </summary>
        /// <param name="zone">The Zone to post log information to
        /// </param>
        /// <param name="data">The SIF_LogEntry object to post as an Add Event 
        /// </param>
        public void Log( IZone zone,
                         SIF_LogEntry data )
        {
            if ( data == null ) {
                return;
            }

            if ( fEcho ) {
                StringBuilder b = new StringBuilder();
                b.Append( "Server Log [Level=" );
                b.Append( data.LogLevel );

                String category = data.SIF_Category;
                int? code = data.SIF_Code;
                if ( category != null && code.HasValue ) {
                    b.Append( ", Category=" );
                    b.Append( category );
                    b.Append( ", Code=" );
                    b.Append( code );
                }
                String appCode = data.SIF_ApplicationCode;
                if ( appCode != null ) {
                    b.Append( ", AppCode=" );
                    b.Append( appCode );
                }

                b.Append( "] " );

                String desc = data.SIF_Desc;
                if ( desc != null ) {
                    b.Append( desc );
                }
                desc = data.SIF_ExtendedDesc;
                if ( desc != null ) {
                    b.Append( ". " + desc );
                }

                zone.Log.Debug( b.ToString() );
            }

            if ( fReportEvents && Adk.SifVersion.CompareTo( SifVersion.SIF15r1 ) >= 0 ) {
                try {
                    zone.ReportEvent( data, EventAction.Add );
                }
                catch ( Exception ex ) {
                    zone.Log.Debug( "Error reporting SIF_LogEntry event to zone: " + ex );
                }
            }
        }
    }
}
