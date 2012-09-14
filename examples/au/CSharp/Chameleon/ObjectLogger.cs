//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using OpenADK.Library;
using OpenADK.Library.Infra;
using OpenADK.Library.Log;

namespace Library.Examples.Chameleon
{
    /// <summary>
    /// Summary description for ChamelonObjectLogger.
    /// </summary>
    public class ObjectLogger : IQueryResults, ISubscriber, IMessagingListener
    {
        private string fDir;

        /// <summary>
        /// Tracks the current response number 
        /// </summary>
        private int fResponseOrdinal = 1;

        private Chameleon fAgent;
        private IDictionary fRefIdTracker = new Hashtable();

        public ObjectLogger( Chameleon agent )
        {
            fAgent = agent;
            fDir = Path.Combine( agent.HomeDir, "Chameleon" ) + "\\";
            if ( fAgent.Properties.GetProperty( "chameleon.autoClearLog", false ) )
            {
                try
                {
                    DirectoryInfo inf = new DirectoryInfo( fDir );
                    if ( inf.Exists )
                    {
                        inf.Delete( true );
                    }
                }
                catch ( Exception ex )
                {
                    Adk.Log.Error( "Error clearing the log files", ex );
                }
            }
        }


        public void OnQueryResults( IDataObjectInputStream data,
                                    SIF_Error error,
                                    IZone zone,
                                    IMessageInfo info )
        {
            SifMessageInfo smi = (SifMessageInfo) info;
            if ( error != null )
            {
                Adk.Log.Warn( "Received Error Response: " + error.SIF_Desc );
            }
            else
            {
                string debug =
                    string.Format
                        ( "Received Response for {0} from zone: {1}. Packet {2} of {3}",
                          data.ObjectType, zone.ZoneId, smi.PacketNumber,
                          smi.MorePackets ? smi.PacketNumber + "+" : smi.PacketNumber + " (FINAL)" );
                zone.Log.Info( debug );
                zone.ServerLog.Log
                    ( LogLevel.INFO, debug, null, "1003", LogEntryCodes.CATEGORY_SUCCESS,
                      LogEntryCodes.CODE_SUCCESS, smi, null );
                bool logToconsole = fAgent.getChameleonProperty( zone, "logConsole", false );
                if ( fAgent.getChameleonProperty( zone, "logResponses", true ) )
                {
                    Log
                        ( fDir + Path.DirectorySeparatorChar + zone.ZoneId +
                          Path.DirectorySeparatorChar + "Responses\\" + data.ObjectType.Name + "\\" +
                          data.ObjectType.Name + DateTime.Now.ToFileTime().ToString() + ".xml", data,
                          smi, logToconsole );
                }
            }
        }

        public void OnEvent( Event evnt,
                             IZone zone,
                             IMessageInfo info )
        {
            bool logToconsole = fAgent.getChameleonProperty( zone, "logConsole", false );
            SifMessageInfo smi = (SifMessageInfo) info;
            string debug =
                string.Format
                    ( "Received {0} Event from {1} in Zone: {2}", evnt.ActionString, smi.SourceId,
                      zone.ZoneId );
            zone.ServerLog.Log
                ( LogLevel.INFO, debug, null, "1003", LogEntryCodes.CATEGORY_SUCCESS,
                  LogEntryCodes.CODE_SUCCESS, smi, null );
            Adk.Log.Info( debug );
            Log
                ( fDir + Path.DirectorySeparatorChar + zone.ZoneId + Path.DirectorySeparatorChar +
                  "Events\\" + evnt.ObjectType.Name + "\\" + evnt.ObjectType.Name +
                  DateTime.Now.ToFileTime().ToString() + ".xml", evnt.Data, smi, logToconsole );
        }

        private void Log( string fileName,
                          IDataObjectInputStream input,
                          SifMessageInfo info,
                          bool logToConsole )
        {
            try
            {
                // Ensure that the directory is there
                FileInfo file = new FileInfo( fileName );
                file.Directory.Create();
                int numObjects = 0;

                using ( Stream outStream = File.OpenWrite( fileName ) )
                {
                    using ( TextWriter twriter = new StreamWriter( outStream, Encoding.UTF8 ) )
                    {
                        twriter.WriteLine( "<SIF_ObjectData>" );
                        twriter.Flush();
                        SifWriter writer = new SifWriter( twriter );
                        SifWriter consoleWriter = null;
                        if ( logToConsole )
                        {
                            consoleWriter = new SifWriter( Console.Out );
                        }

                        SifDataObject o;
                        while ( (o = input.ReadDataObject()) != null )
                        {
                            numObjects++;
                            Track( o );
                            writer.Write( o );
                            if ( logToConsole )
                            {
                                consoleWriter.Write( o );
                                Console.WriteLine();
                            }
                        }
                        writer.Flush();
                        if ( logToConsole )
                        {
                            consoleWriter.Flush();
                        }
                        twriter.WriteLine( "</SIF_ObjectData>" );
                    }

                    outStream.Close();

                    Console.WriteLine
                        ( "Received {0} objects for {1}, Packet# {2}", numObjects,
                          input.ObjectType.Name, info.PacketNumber );
                }
            }
            catch ( Exception ex )
            {
                Adk.Log.Error( ex.Message, ex );
            }
        }

        private void Track( SifDataObject o )
        {
            try
            {
                if ( fRefIdTracker.Contains( o.RefId ) )
                {
                    using (
                        Stream outStream =
                            File.OpenWrite
                                ( "Duplicate " + o.RefId + "-" + o.ElementDef.Name + "-" +
                                  DateTime.Now.ToFileTime().ToString() + ".xml" ) )
                    {
                        using ( TextWriter wr = new StreamWriter( outStream ) )
                        {
                            wr.WriteLine
                                ( String.Format( "Duplicate Object with Refid: {0}", o.RefId ) );
                            wr.WriteLine
                                ( "Original:" + ((SifDataObject) fRefIdTracker[o.RefId]).ToXml() );
                            wr.WriteLine( "New:" + o.ToXml() );
                        }
                    }
                }
                else
                {
                    fRefIdTracker.Add( o.RefId, o );
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                Adk.Log.Warn( ex.Message, ex );
            }
        }

        public void OnQueryPending( IMessageInfo info,
                                    IZone zone )
        {
            Adk.Log.Info
                ( string.Format
                      ( "Requested {0} from zone: {1}",
                        ((SifMessageInfo) info).SIFRequestObjectType.Name, zone.ZoneId ) );
        }

        [MethodImpl( MethodImplOptions.Synchronized )]
        public MessagingReturnCode OnMessageReceived( SifMessageType messageType,
                                                      StringBuilder message )
        {
            return MessagingReturnCode.Process;
        }

        public void OnMessageProcessed( SifMessageType messageType,
                                        IMessageInfo info )
        {
            string fileName = null;
            SifMessageInfo smi = (SifMessageInfo) info;
            if ( messageType == SifMessageType.SIF_Response )
            {
                // Log Query responses in the standard OpenADK Message tracing format
                //N-ObjectType-ZoneID-SourceID-PacketNum|”error”.txt
                fileName =
                    string.Format
                        ( "{0:00000}-{1}-{2}-{3}-{4}.txt", fResponseOrdinal++,
                          smi.SIFRequestObjectType.Name, smi.Zone.ZoneId, smi.SourceId,
                          smi.PacketNumber );
            }
            else if ( messageType == SifMessageType.SIF_Event )
            {
                fileName =
                    string.Format
                        ( "{0:00000}-{1}-{2}-{3}-SIF_Event.txt", fResponseOrdinal++, "ChangeThis",
                          smi.Zone.ZoneId, smi.SourceId );
            }
            if ( fileName != null )
            {
                try
                {
                    FileInfo file = new FileInfo( fDir + "\\Raw\\" + fileName );
                    file.Directory.Create();
                    using (
                        StreamWriter writer = new StreamWriter( file.FullName, true, Encoding.UTF8 )
                        )
                    {
                        writer.Write( smi.Message );
                        writer.Close();
                    }
                }
                catch ( Exception ex )
                {
                    Adk.Log.Warn( ex.Message, ex );
                }
            }
        }

        public bool OnSendingMessage( SifMessageType messageType,
                                      IMessageInfo info,
                                      StringBuilder message )
        {
            return true;
        }

        public void OnMessageSent( SifMessageType messageType,
                                   IMessageInfo info,
                                   object receipt )
        {
        }
    }
}
