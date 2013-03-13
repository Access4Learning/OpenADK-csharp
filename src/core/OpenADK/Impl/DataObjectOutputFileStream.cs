//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using System.Text;
using OpenADK.Library.Global;
using OpenADK.Library.Infra;
using OpenADK.Util;

namespace OpenADK.Library.Impl
{
    /// <summary>  An implementation of the DataObjectOutputStream interface that packetizes
    /// SIF_Response packets to the agent work directory.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class DataObjectOutputFileStream : DataObjectOutputStreamImpl
    {
        private FileInfo fFile;
        private Query fQuery;
        private IElementDef[] fQueryRestrictions;
        private String fWorkDir;
        private long fCurSize;
        private long fMaxSize;
        private long fEnvSize;
        private int fCurPacket = 0;
        private SIF_Error fError;
        private ZoneImpl fZone;
        private SifVersion fRenderAsVersion;
        private Stream fCurrentOutputStream;
        private bool fDeferResponses;

        /// <summary>
        /// The Query filter used to filter data. If set, each call to write() makes an evaluation
        /// of the data based on this filter. It the data does not meet the conditions of the query,
        /// the object is not written to the output stream.
        /// </summary>
        private Query fFilter;

        /// <summary>
        /// This field is set to true in certain cases by SIFResposeSender. If it is set to true,
        /// the final packet that is written by this class will have the SIF_MorePackets flag set to 'Yes' 
        /// </summary>
        private bool fMorePackets = false;


        /// <summary>  Initialize the output stream. This method must be called after creating
        /// a new instance of this class and before writing any SIFDataObjects to
        /// the stream.
        /// </summary>
        /// <param name="zone">The Zone associated with messages that will be written to the stream
        /// </param>
        /// <param name="query">The Query from the SIF_Request message
        /// </param>
        /// <param name="requestSourceId">The SourceId of the associated SIF_Request message
        /// </param>
        /// <param name="requestMsgId">The MsgId of the associated SIF_Request message
        /// </param>
        /// <param name="requestSIFVersion">The version of the SIF_Message envelope of the
        /// SIF_Request message (if specified and different than the SIF_Message
        /// version, the SIF_Request/SIF_Version element takes precedence).
        /// SIF_Responses will be encapsulated in a message envelope matching
        /// this version and SifDataObject contents will be rendered in this
        /// version
        /// </param>
        /// <param name="maxSize">The maximum size of rendered SifDataObject that will be
        /// accepted by this stream. If a SifDataObject is written to the stream
        /// and its size exceeds this value after rendering the object to an XML
        /// stream, an ObjectTooLargeException will be thrown by the <i>write</i>
        /// method
        /// </param>
        public override void Initialize(
            IZone zone,
            Query query,
            String requestSourceId,
            String requestMsgId,
            SifVersion requestSIFVersion,
            int maxSize )
        {
            fQuery = query;
            Initialize( zone, query == null ? null : query.FieldRestrictions, requestSourceId, requestMsgId,
                        requestSIFVersion, maxSize );
        }


        /// <summary>  Initialize the output stream. This method must be called after creating
        /// a new instance of this class and before writing any SIFDataObjects to
        /// the stream.
        /// </summary>
        /// <param name="zone">The Zone associated with messages that will be written to the stream
        /// </param>
        /// <param name="queryRestrictions">The fields that should be returned according to the Query 
        /// that was specified in the SIF_Request message
        /// </param>
        /// <param name="requestSourceId">The SourceId of the associated SIF_Request message
        /// </param>
        /// <param name="requestMsgId">The MsgId of the associated SIF_Request message
        /// </param>
        /// <param name="requestSIFVersion">The version of the SIF_Message envelope of the
        /// SIF_Request message (if specified and different than the SIF_Message
        /// version, the SIF_Request/SIF_Version element takes precedence).
        /// SIF_Responses will be encapsulated in a message envelope matching
        /// this version and SifDataObject contents will be rendered in this
        /// version
        /// </param>
        /// <param name="maxSize">The maximum size of rendered SifDataObject that will be
        /// accepted by this stream. If a SifDataObject is written to the stream
        /// and its size exceeds this value after rendering the object to an XML
        /// stream, an ObjectTooLargeException will be thrown by the <i>write</i>
        /// method
        /// </param>
        public override void Initialize(
            IZone zone,
            IElementDef[] queryRestrictions,
            String requestSourceId,
            String requestMsgId,
            SifVersion requestSIFVersion,
            int maxSize )
        {
            fZone = (ZoneImpl) zone;
            fQueryRestrictions = queryRestrictions;
            fReqId = requestMsgId;
            fDestId = requestSourceId;
            fMaxSize = maxSize;
            fCurPacket = 0;
            fRenderAsVersion = requestSIFVersion;

            //
            //  Messages written to this stream are stored in the directory
            //  "%adk.home%/work/%zoneId%_%zoneHost%/responses". One or more files
            //  are written to this directory, where each file has the name
            //  "destId.requestId.{packet}.pkt". As messages are written to the
            //  stream, the maxSize property is checked to determine if the size of
            //  the current file will be larger than the maxSize. If so, the file is
            //  closed and the packet number incremented. A new file is then created
            //  for the message and all subsequent messages until maxSize is again
            //  exceeded.

            //  Ensure work directory exists
            fWorkDir = ResponseDelivery.GetSourceDirectory( DeliveryType, zone );
            DirectoryInfo dir = new DirectoryInfo( fWorkDir );
            if ( !dir.Exists )
            {
                dir.Create();
            }

            //  Get the size of the SIF_Message envelope to determine the actual
            //  packet size we're producing
            fEnvSize = CalcEnvelopeSize( fZone );
        }

        /// <summary>  Start writing messages to a new packet file. The current packet file
        /// stream is closed, the packet number incremented by one, and a new packet
        /// file created. All subsequent calls to the <i>write</i> method will render
        /// messages to the newly-created packet file.
        /// </summary>
        private void NewPacket()
        {
            Close();

            fCurPacket++;
            fCurSize = fEnvSize;

            //  Create output file and stream
            fFile = CreateOutputFile();
            fCurrentOutputStream = CreateOutputStream();
        }

        /// <summary>  Create a File descriptor of the current output file</summary>
        private FileInfo CreateOutputFile()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append( fWorkDir );
            builder.Append( Path.DirectorySeparatorChar );
            ResponseDelivery.SerializeResponsePacketFileName( builder, fDestId, fReqId, fCurPacket, fRenderAsVersion,
                                                              (fError != null) );
            return new FileInfo( builder.ToString() );
        }

        /// <summary>  Create the underlying output stream</summary>
        private Stream CreateOutputStream()
        {
            Close();
            return new FileStream( fFile.FullName, FileMode.Create );
        }

        /// <summary>  Write a SifDataObject to the stream</summary>
        public override void Write( SifDataObject data )
        {
            // Check to see if the data object is null or if the
            // deferResponses() property has been set
            if ( data == null || fDeferResponses )
            {
                return;
            }

            // Check to see if a SIF_Error has already been written
            if ( fError != null )
            {
                throw new AdkException
                    ( "A SIF_Error has already been written to the stream", fZone );
            }

            // If the autoFilter property has been set, determine if this object meets the 
            // conditions of the filter
            if ( fFilter != null )
            {
                if ( !fFilter.Evaluate( data ) )
                {
                    // TODO: Perhaps this feature should log any objects not written to the output 
                    // stream if extended logging is enabled
                    return;
                }
            }

            try
            {
                if ( fCurrentOutputStream == null || fZone.Properties.OneObjectPerResponse )
                {
                    NewPacket();
                }

                using ( MemoryStream buffer = new MemoryStream() )
                {
                    //  Write to memory stream first so we can determine if the resulting
                    //  message will fit in the current packet
                    WriteObject( data, buffer, fRenderAsVersion, fQueryRestrictions );

                    if ( (buffer.Length + fCurSize) > fMaxSize )
                    {
                        // If the current packet size is equal to the envelope size (e.g. no objects 
                        // have been written), we have exceeded the size of the buffer and need to abort 
                        if ( buffer.Length == fEnvSize )
                        {
                            String errorMessage = "Publisher result data in packet " + fCurPacket + " too large (" +
                                                  buffer.Length + " [Data] + " + fEnvSize + " [Sif Envelope] > " +
                                                  fMaxSize + ")";
                            if ( fZone.Properties.OneObjectPerResponse )
                            {
                                errorMessage += " [1 Object per Response Packet]";
                            }
                            throw new AdkException( errorMessage, fZone );
                        }
                        else
                        {
                            //  Create new packet for this object
                            NewPacket();
                        }
                    }

                    if ( (Adk.Debug & AdkDebugFlags.Message_Content) != 0 )
                    {
                        buffer.Seek( 0, SeekOrigin.Begin );
                        StreamReader reader = new StreamReader( buffer, SifIOFormatter.ENCODING );
                        string message = reader.ReadToEnd();
                        fZone.Log.Debug
                            ( "Writing object to SIF_Response packet #" + fCurPacket + ":\r\n" +
                              message );
                    }

                    buffer.Seek( 0, SeekOrigin.Begin );
                    Streams.CopyStream( buffer, fCurrentOutputStream );
                    fCurSize += buffer.Length;
                    buffer.Close();
                }
            }
            catch ( Exception ioe )
            {
                throw new AdkException
                    (
                    "Failed to write Publisher result data (packet " + fCurPacket + ") to " +
                    fFile.FullName + ": " +
                    ioe, fZone );
            }
        }

        /// <summary>
        /// Commits the writing of all objects and sends the objects
        /// </summary>
        public override void Commit()
        {
            try
            {
                if ( fDeferResponses )
                {
                    Abort();
                }
                else
                {
                    //  If no objects or SIF_Errors have been written to the stream, we still
                    //  need to return an empty SIF_Response to the ZIS.
                    if ( fCurrentOutputStream == null )
                    {
                        try
                        {
                            NewPacket();
                            Close();
                        }
                        catch ( IOException ioe )
                        {
                            throw new AdkException
                                (
                                "Could not commit the stream because of an IO error writing an empty SIF_Response packet: " +
                                ioe, fZone );
                        }
                    }

                    String responseFileName = ResponseDelivery.SerializeResponseHeaderFileName(fDestId, fReqId, fMorePackets);

                    //  Write out "destId.requestId." file to signal the Publisher has finished
                    //  writing all responses successfully. This file will hang around until
                    //  all "requestId.{packet}.pkt" files have been sent to the ZIS by the Adk,
                    //  a process that could occur over several agent sessions if the agent
                    //  is abruptly terminated.
                    //
                    String fileName = fWorkDir + Path.DirectorySeparatorChar + responseFileName;

                    try
                    {
                        FileInfo publisherFile = new FileInfo( fileName );
                        using ( Stream publisherFileStream = publisherFile.Create() )
                        {
                            publisherFileStream.Close();
                        }
                    }
                    catch ( IOException ioe )
                    {
                        fZone.Log.Warn("Unable to create SIF_Response header file: " + fileName + ". " + ioe.Message, ioe);
                    }

                    OnCommitted();
                }
            }
            finally
            {
                fZone = null;
            }
        }

        protected virtual void OnCommitted()
        {
            fZone.GetResponseDelivery().Process();
        }



        /// <summary>  Called when the Publisher.OnQuery method has thrown a SifException,
        /// indicating an error should be returned in the SIF_Response body
        /// </summary>
        public override void SetError( SIF_Error error )
        {
            fError = error;

            //
            //  Write a SIF_Response packet that contains only this SIF_Error
            //
            try
            {
                NewPacket();
                SifWriter writer = new SifWriter( fCurrentOutputStream );
                writer.SuppressNamespace( true );
                writer.Write( error, fRenderAsVersion );
                writer.Close();
            }
            catch ( IOException ioe )
            {
                throw new AdkException
                    (
                    "Failed to write Publisher SIF_Error data (packet " + fCurPacket + ") to " +
                    fFile.FullName + ": " +
                    ioe, fZone );
            }
        }

        /// <summary>
        /// Aborts the current write, closes the string, and deletes all files created during this operations
        /// </summary>
        public override void Abort()
        {
            Close();
            string filter = fDestId + "." + fReqId + "*";
            DirectoryInfo dir = new DirectoryInfo( fWorkDir );
            foreach ( FileInfo file in dir.GetFiles( filter ) )
            {
                file.Delete();
            }
            fZone = null;
        }

        /// <summary>
        /// Defer sending SIF_Response messages and ignore any objects written to this stream.
        /// </summary>
        /// <remarks>
        /// See the <see cref="OpenADK.Library.SifResponseSender"/> class comments for more
        /// information about using this method.
        /// <seealso cref="OpenADK.Library.SifResponseSender"/>
        /// </remarks>
        /// <since>ADK 1.5.1</since>
        public override void DeferResponse()
        {
            fDeferResponses = true;
        }


        /// <summary>
        /// Closes the current stream
        /// </summary>
        public override void Close()
        {
            if ( fCurrentOutputStream != null  )
            {
                fCurrentOutputStream.Close();
            }
        }

        /// <summary>
        /// The zone that is currently in scope
        /// </summary>
        protected ZoneImpl Zone
        {
            get { return fZone; }
        }

        /// <summary>
        /// Recalculates the envelope size
        /// </summary>
        protected void RecalcEnvSize()
        {
            fEnvSize = CalcEnvelopeSize( fZone );
        }

        /// <summary>
        /// Returns the type of delivery stream this is
        /// </summary>
        protected virtual ResponseDeliveryType DeliveryType
        {
            get { return ResponseDeliveryType.Generic; }
        }

        /// <summary>
        /// Returns whether Responses are being deferred
        /// </summary>
        protected bool ResponsesDeferred
        {
            get { return fDeferResponses; }
        }

        /// <summary>
        /// The directory where responses are being queued until they are sent
        /// </summary>
        protected string WorkDir
        {
            get { return fWorkDir; }
        }

        /// <summary>
        /// Writes the object to the outgoing data stream
        /// </summary>
        /// <param name="data"></param>
        /// <param name="buffer"></param>
        /// <param name="version"></param>
        /// <param name="fieldRestrictions"></param>
        protected virtual void WriteObject( SifDataObject data,
                                            Stream buffer,
                                            SifVersion version,
                                            IElementDef[] fieldRestrictions )
        {
            SifWriter writer = new SifWriter( buffer );
            writer.SuppressNamespace( true );
            if ( fQuery != null )
            {
                fQuery.SetRenderingRestrictionsTo( data );
            }
            else if ( fQueryRestrictions != null )
            {
                writer.Filter = fQueryRestrictions;
            }
            writer.Write( data, version );
            writer.Flush();
        }


        /// <summary>
        /// Tells the DataObjectOutputStream to automatically filter out any SIFDataObjects that do
        /// not match the conditions specified in the provided Query object.
        /// </summary>
        /// <remarks>Any SIFDataObject that does not meet the conditions specified in the Query will not be
        /// written to the underlying data stream.
        /// </remarks>
        /// <value>The Query object to use when filtering data or <c>null</c> to remove the filter </value>
        public override Query Filter
        {
            get { return fFilter; }

            set { fFilter = value; }
        }

        /// <summary>
        /// Gets or sets the value that will be set to the SIF_MorePackets element in the message
        /// after this DataObjectOutputStream is closed
        /// </summary>
        public override YesNo SIF_MorePackets
        {
            get { return fMorePackets ? YesNo.YES : YesNo.NO; }
            set { fMorePackets = value == YesNo.YES; }
        }

        /// <summary>
        /// Sets the value that will be used for the number of the first packet created by the output stream
        /// </summary>
        /// <exception cref="InvalidOperationException">thrown if setting the value after an object has 
        /// already been written to the output stream</exception>
        public override int SIF_PacketNumber
        {
            get
            { // Special case: If newPacket() has not been called yet, 
                // fWriter will be null, in which case, we need to add one to fCurPacket
                // to get the actual value of the packet
                if (fCurrentOutputStream == null)
                {
                    return fCurPacket + 1;
                }
                else
                {
                    return fCurPacket;
                }
            }
            set
            { // If fWriter is not initialized, set the fCurPacket value to 
                // 1 value less (allows it to be properly incremented in newPacket())
                if (fCurrentOutputStream == null)
                {
                    fCurPacket = value - 1;
                }
                else
                {
                    throw new InvalidOperationException("Cannot set the packet number after objects have already been written");
                }
            }
        }

        #region Cleanup and Disposal

        /// <summary>
        /// Cleans up the resources held on to by this object
        /// </summary>
        public override void Dispose()
        {
            Dispose( true );
        }

        private void Dispose( bool disposing )
        {
            Close();
        }

        #endregion
    }
}
