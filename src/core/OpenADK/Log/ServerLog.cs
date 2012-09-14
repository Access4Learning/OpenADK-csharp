//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Log
{
    /// <summary> 	Provides access to the Zone Integration Server log.
    /// 
    /// ServerLog functionality can be customized and extended by adding one or 
    /// more <i>ServerLogModule</i> implementations to the chain of loggers. The 
    /// logging chain is hierarchical, comprised of ServerLog instances at the Adk,
    /// Agent, and Zone levels. Whenever a server-side logging operation is 
    /// performed on a zone, it is delegated to the <i>ServerLogModule</i> instances 
    /// at each level in the hierarchy, beginning with the Zone.
    /// 
    /// To customize server-side logging on a global basis, call the 
    /// <c>Adk.getServerLog</c> static function and use the methods below to
    /// manipulate <i>ServerLogModule</i> instances at the root level of the 
    /// hierarchy:
    /// 
    /// <ul>
    /// <li><c>addLogger</c></li>
    /// <li><c>removeLogger</c></li>
    /// <li><c>clearLoggers</c></li>
    /// <li><c>getLoggers</c></li>
    /// </ul> * 
    /// 
    /// Similarly, to customize logging on an agent-global basis or per-zone basis, 
    /// call the <c>Agent.getServerLog</c> or <c>Agent.getServerLog( Zone )</c>
    /// methods to obtain the ServerLog for the Agent or a Zone, respectively.
    /// 
    /// 
    /// </summary>
    public class ServerLog
    {
        /// <summary> 	Get a list of all registered ServerLogModule modules that comprise the
        /// chain of loggers.
        /// 
        /// </summary>
        /// <seealso cref="AddLogger">
        /// </seealso>
        /// <seealso cref="RemoveLogger">
        /// </seealso>
        /// <seealso cref="ClearLoggers">
        /// 
        /// @since Adk 1.5
        /// </seealso>
        public virtual IServerLogModule [] GetLoggers()
        {
            lock ( fLoggers ) {
                IServerLogModule [] arr = new IServerLogModule[fLoggers.Count];
                fLoggers.CopyTo( arr, 0 );
                return arr;
            }
        }

        /// <summary> 	Global registry of ServerLog instances keyed by ID</summary>
        private static IDictionary<String, ServerLog> sInstances =
            new Dictionary<String, ServerLog>();

        /// <summary> 	The parent ServerLog</summary>
        private ServerLog fParent;

        /// <summary>The ID</summary>
        private string fID;

        /// <summary> 	The zone to which log entries will be reported</summary>
        private IZone fZone;

        /// <summary> 	ServerLogModule objects to which log information will be posted</summary>
        private IList<IServerLogModule> fLoggers = new List<IServerLogModule>();

        /// <summary> 	Protected constructor; clients must call <c>getInstance</c></summary>
        private ServerLog( string id,
                           IZone zone )
        {
            fID = id;
            fZone = zone;

            //	Determine the parent
            if ( id.Equals( Adk.LOG_IDENTIFIER ) ) {
                fParent = null;
            }
            else if ( id.Equals( Agent.LOG_IDENTIFIER ) ) {
                fParent = GetInstance( Adk.LOG_IDENTIFIER, zone );
            }
            else {
                fParent = GetInstance( Agent.LOG_IDENTIFIER, zone );
            }
        }

        /// <summary>
        /// Returns the ID of this logger instance
        /// </summary>
        public string Id
        {
            get { return fID; }
        }


        /// <summary> 	Get a ServerLog instance with the specified ID.
        /// 
        /// This method is intended to be called internally by the Adk. You
        /// should call the <c>Adk.getServerLog</c>, <c>Agent.getServerLog</c>, 
        /// or <c>Zone.getServerLog</c> methods to obtain a ServerLog instance
        /// rather than directly calling this method.
        /// 
        /// </summary>
        /// <param name="id">The ID identifying the ServerLog to return
        /// </param>
        /// <param name="zone">The zone that is currently in scope</param>
        /// <returns> A ServerLog instance 
        /// </returns>
        public static ServerLog GetInstance( string id,
                                             IZone zone )
        {
            if ( id == null ) {
                throw new ArgumentException( "ID cannot be null" );
            }

            ServerLog log = null;

            if ( !sInstances.TryGetValue( id, out log ) ) {
                log = new ServerLog( id, zone );
                sInstances[id] = log;
            }

            return log;
        }

        /// <summary> 	Adda a ServerLogModule to the chain of loggers.
        /// 
        /// </summary>
        /// <param name="logger">A <i>ServerLogModule</i> implementation
        /// 
        /// </param>
        /// <seealso cref="RemoveLogger">
        /// </seealso>
        /// <seealso cref="ClearLoggers">
        /// </seealso>
        /// @since Adk 1.5
        /// </seealso>
        public virtual void AddLogger( IServerLogModule logger )
        {
            lock ( fLoggers ) {
                if ( !fLoggers.Contains( logger ) ) {
                    fLoggers.Add( logger );
                }
            }
        }

        /// <summary> 	Remove a ServerLogModule from the chain of loggers.
        /// 
        /// </summary>
        /// <param name="logger">A <i>ServerLogModule</i> implementation
        /// 
        /// </param>
        /// <seealso cref="AddLogger">
        /// </seealso>
        /// <seealso cref="ClearLoggers">
        /// </seealso>
        /// <seealso cref="GetLoggers">
        /// 
        /// @since Adk 1.5
        /// </seealso>
        public virtual void RemoveLogger( IServerLogModule logger )
        {
            lock ( fLoggers ) {
                fLoggers.Remove( logger );
            }
        }

        /// <summary> 	Clear all ServerLogModule modules from the chain of loggers.
        /// 
        /// </summary>
        /// <seealso cref="AddLogger">
        /// </seealso>
        /// <seealso cref="RemoveLogger">
        /// </seealso>
        /// <seealso cref="GetLoggers">
        /// 
        /// @since Adk 1.5
        /// </seealso>
        public virtual void ClearLoggers()
        {
            lock ( fLoggers ) {
                fLoggers.Clear();
            }
        }

        /// <summary> 	Copies all registered ServerLogModules for this ServerLog into the
        /// supplied Vector.
        /// @since Adk 1.5
        /// </summary>
        protected internal virtual void GetLoggersInto( IList<IServerLogModule> target )
        {
            lock ( fLoggers ) {
                foreach ( IServerLogModule o in fLoggers ) {
                    target.Add( o );
                }
            }
        }

        /// <summary> 	Post a SIF_LogEntry to the server.
        /// Use this form of the <c>log</c> method to post a simple
        /// informative message to the server.
        /// </summary>
        /// <param name="message">A textual description of the error 
        /// </param>
        public virtual void Log( string message )
        {
            Log( LogLevel.INFO, message, null, null, - 1, - 1, null, null );
        }

        /// <summary> 	Post a SIF_LogEntry to the server.
        /// 
        /// Use this form of the <c>log</c> method to post an
        /// error, warning, or informative message to the server with an
        /// description, extended description, and optional application-defined
        /// error code.
        /// 
        /// </summary>
        /// <param name="level">The LogLevel to assign to this log entry
        /// </param>
        /// <param name="desc">A textual description of the error
        /// </param>
        /// <param name="extDesc">Extended error description, or <c>null</c> if no 
        /// value is to be assigned to the SIF_LogEntry/SIF_ExtDesc element
        /// </param>
        /// <param name="appCode">Error code specific to the application posting the log 
        /// entry, or <c>null</c> if no value is to be assigned to the 
        /// SIF_LogEntry/SIF_ApplicationCode element
        /// </param>
        public virtual void Log( LogLevel level,
                                 string desc,
                                 string extDesc,
                                 string appCode )
        {
            Log( level, desc, extDesc, appCode, - 1, - 1, null, null );
        }

        /// <summary> 	Post a SIF_LogEntry to the server.
        /// 
        /// Use this form of the <c>log</c> method to post an
        /// error, warning, or informative message to the server with an category
        /// and code enumerated by the SIF Specification.
        /// 
        /// </summary>
        /// <param name="level">The LogLevel to assign to this log entry
        /// </param>
        /// <param name="desc">A textual description of the error
        /// </param>
        /// <param name="extDesc">Extended error description, or <c>null</c> if no 
        /// value is to be assigned to the SIF_LogEntry/SIF_ExtDesc element
        /// </param>
        /// <param name="appCode">Error code specific to the application posting the log 
        /// entry, or <c>null</c> if no value is to be assigned to the 
        /// SIF_LogEntry/SIF_ApplicationCode element
        /// </param>
        /// <param name="category">The SIF_Category value to assign to this log entry, as
        /// defined by the SIF Specification
        /// </param>
        /// <param name="category">The SIF_Code value to assign to this log entry, as
        /// defined by the SIF Specification
        /// </param>
        public virtual void Log( LogLevel level,
                                 string desc,
                                 string extDesc,
                                 string appCode,
                                 int category,
                                 int code )
        {
            Log( level, desc, extDesc, appCode, category, code, null, null );
        }

        /// <summary> 	Post a SIF_LogEntry to the server.
        /// 
        /// Use this form of the <c>log</c> method to post a simple
        /// error, warning, or informative message to the server that references a
        /// SIF Message and optionally a set of SIF Data Objects previously received
        /// by the agent.
        /// 
        /// </summary>
        /// <param name="level">The LogLevel to assign to this log entry
        /// </param>
        /// <param name="message">A textual description of the error
        /// </param>
        /// <param name="info">The <i>SifMessageInfo</i> instance from the Adk message
        /// handler implementation identifying a SIF Message received by the agent  
        /// </param>
        /// <param name="objects">One or more SifDataObject instances received in the message
        /// identified by the <i>info</i> parameter
        /// </param>
        public virtual void Log( LogLevel level,
                                 string message,
                                 SifMessageInfo info,
                                 SifDataObject [] objects )
        {
            Log( level, message, null, null, - 1, - 1, info, objects );
        }

        /// <summary> 	Post a SIF_LogEntry to the server.
        /// 
        /// Use this form of the <c>log</c> method to post an
        /// error, warning, or informative message to the server that references a
        /// SIF Message and optionally a set of SIF Data Objects previously received
        /// by the agent. The log entry can also have an extended error description 
        /// and application-defined error code.
        /// 
        /// </summary>
        /// <param name="level">The LogLevel to assign to this log entry
        /// </param>
        /// <param name="desc">A textual description of the error
        /// </param>
        /// <param name="extDesc">Extended error description, or <c>null</c> if no 
        /// value is to be assigned to the SIF_LogEntry/SIF_ExtDesc element
        /// </param>
        /// <param name="appCode">Error code specific to the application posting the log 
        /// entry, or <c>null</c> if no value is to be assigned to the 
        /// SIF_LogEntry/SIF_ApplicationCode element
        /// </param>
        /// <param name="info">The <i>SifMessageInfo</i> instance from the Adk message
        /// handler implementation identifying a SIF Message received by the agent  
        /// </param>
        /// <param name="objects">One or more SifDataObject instances received in the message
        /// identified by the <i>info</i> parameter
        /// </param>
        public virtual void Log( LogLevel level,
                                 string desc,
                                 string extDesc,
                                 string appCode,
                                 SifMessageInfo info,
                                 SifDataObject [] objects )
        {
            Log( level, desc, extDesc, appCode, - 1, - 1, info, objects );
        }

        /// <summary> 	Post a SIF_LogEntry to the server.
        /// 
        /// Use this form of the <c>log</c> method to post an error, warning, 
        /// or informative message to the server that references a
        /// SIF Message and optionally a set of SIF Data Objects previously received
        /// by the agent. The log entry is assigned a category and code defined by
        /// the SIF Specification, and may have an extended error description and 
        /// optional application-defined error code.
        /// 
        /// </summary>
        /// <param name="level">The LogLevel to assign to this log entry
        /// </param>
        /// <param name="desc">A textual description of the error
        /// </param>
        /// <param name="extDesc">Extended error description, or <c>null</c> if no 
        /// value is to be assigned to the SIF_LogEntry/SIF_ExtDesc element
        /// </param>
        /// <param name="appCode">Error code specific to the application posting the log 
        /// entry, or <c>null</c> if no value is to be assigned to the 
        /// SIF_LogEntry/SIF_ApplicationCode element
        /// </param>
        /// <param name="category">The SIF_Category value to assign to this log entry, as
        /// defined by the SIF Specification
        /// </param>
        /// <param name="category">The SIF_Code value to assign to this log entry, as
        /// defined by the SIF Specification
        /// </param>
        /// <param name="info">The <i>SifMessageInfo</i> instance from the Adk message
        /// handler implementation identifying a SIF Message received by the agent  
        /// </param>
        /// <param name="objects">One or more SifDataObject instances received in the message
        /// identified by the <i>info</i> parameter
        /// </param>
        public virtual void Log( LogLevel level,
                                 string desc,
                                 string extDesc,
                                 string appCode,
                                 int category,
                                 int code,
                                 SifMessageInfo info,
                                 params SifDataObject [] objects )
        {
            if ( fZone == null ) {
                throw new SystemException
                    ( "ServerLog.log can only be called on a zone's ServerLog instance" );
            }

            string msg = null;
            SIF_LogEntry le = null;
            if ( Adk.SifVersion.CompareTo( SifVersion.SIF15r1 ) >= 0 ) {
                //	Create a SIF_LogEntry
                le = new SIF_LogEntry();
                le.SetSource( LogSource.AGENT );
                le.SetLogLevel( LogLevel.Wrap( level == null ? "Unknown" : level.ToString() ) );
                if ( desc != null ) {
                    le.SIF_Desc = desc;
                }
                if ( extDesc != null ) {
                    le.SIF_ExtendedDesc = extDesc;
                }
                if ( appCode != null ) {
                    le.SIF_ApplicationCode = appCode;
                }
                if ( category != - 1 ) {
                    le.SIF_Category = category.ToString();
                }
                if ( code != - 1 ) {
                    le.SIF_Code = code;
                }

                //	Reference a SIF_Message?
                if ( info != null ) {
                    try {
                        SIF_Header headerCopy = (SIF_Header) info.SIFHeader.Clone();
                        SIF_LogEntryHeader sleh = new SIF_LogEntryHeader();
                        sleh.SIF_Header = headerCopy;

                        //	Assign to SIF_OriginalHeader			
                        le.SIF_OriginalHeader = sleh;
                    }
                    catch ( Exception ex ) {
                        fZone.Log.Warn
                            ( "Unable to clone SIF_Header for SIF_LogEntry event:" + ex.Message, ex );
                    }
                }

                if ( objects != null ) {
                    SIF_LogObjects slos = new SIF_LogObjects();
                    le.SIF_LogObjects = slos;

                    for ( int i = 0; i < objects.Length; i++ ) {
                        if ( objects[i] == null ) {
                            continue;
                        }

                        //	Package into a SIF_LogObject and add to the repeatable list
                        //	of SIF_LogEntry/SIF_LogObjects
                        SIF_LogObject lo = new SIF_LogObject();
                        lo.ObjectName = objects[i].ObjectType.Tag( info.SifVersion );
                        lo.AddChild( (SifElement) objects[i].Clone() );
                        slos.Add( lo );
                    }
                }
            }
            else {
                // 	When running in SIF 1.1 or earlier, there is no
                //	SIF_LogEntry support. Build a string that can be
                //	written to the local zone log, including as much
                //	information from the would-be SIF_LogEntry as 
                //	possible.

                StringBuilder b = new StringBuilder();

                b.Append( "Server Log [Level=" );
                b.Append( level == null ? "Unknown" : level.ToString() );

                if ( category != - 1 && code != - 1 ) {
                    b.Append( ", Category=" );
                    b.Append( category );
                    b.Append( ", Code=" );
                    b.Append( code );
                }
                if ( appCode != null ) {
                    b.Append( ", AppCode=" );
                    b.Append( appCode );
                }

                b.Append( "] " );

                if ( desc != null ) {
                    b.Append( desc );
                }
                if ( extDesc != null ) {
                    b.Append( ". " + extDesc );
                }

                msg = b.ToString();
            }

            //	Post the the server		
            IServerLogModule [] chain = _getLogChain( fZone );
            for ( int i = 0; i < chain.Length; i++ ) {
                if ( le != null ) {
                    chain[i].Log( fZone, le );
                }
                else {
                    chain[i].Log( fZone, msg );
                }
            }
        }

        private IServerLogModule [] _getLogChain( IZone zone )
        {
            List<IServerLogModule> v = new List<IServerLogModule>();

            ServerLog parent = this;
            while ( parent != null ) {
                parent.GetLoggersInto( v );
                parent = parent.fParent;
            }

            return v.ToArray();
        }
    }
}
