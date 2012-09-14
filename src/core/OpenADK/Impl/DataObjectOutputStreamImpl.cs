//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using OpenADK.Library.Global;
using OpenADK.Library.Infra;
using OpenADK.Util;

namespace OpenADK.Library.Impl
{
    /// <summary>  The abstract base class for OutputStreams to which Publishers write the
    /// results of a query. Although this class has a generic name that might imply
    /// it is a generic output stream for SIFDataObjects, it is really very specific
    /// to SIF_Request processing and is not intended to be used generically.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <since>  Adk 1.0
    /// </since>
    public abstract class DataObjectOutputStreamImpl : IDataObjectOutputStream, IDisposable
    {
        /// <summary>  Called when the Publisher.onQuery method has thrown a SifException,
        /// indicating an error should be returned in the SIF_Response body
        /// </summary>
        public abstract void SetError( SIF_Error error );

        /// <summary>
        /// The SIF_Request message ID
        /// </summary>
        protected internal String fReqId;

        /// <summary>
        /// The SIF_Request destination ID
        /// </summary>
        protected internal String fDestId;

        /// <summary>  Construct a new DataObjectOutputStream</summary>
        /// <returns> A new DataObjectOutputStream object, which will always be a
        /// an instanceof DataObjectOutputStreamImpl as defined by the
        /// <c>adkglobal.factory.DataObjectOutputStream</c> system property.
        /// 
        /// </returns>
        public static DataObjectOutputStreamImpl NewInstance()
        {
            String cls = Properties.GetProperty( "adkglobal.factory.DataObjectOutputStream" );
            if ( cls == null ) {
                return new DataObjectOutputFileStream();
            }
            else {
                try {
                    return (DataObjectOutputStreamImpl) ClassFactory.CreateInstance( cls );
                }
                catch ( Exception thr ) {
                    throw new AdkException
                        ( "Adk could not create an instance of the class " + cls + ": " + thr, null,
                          thr );
                }
            }
        }

        /// <summary>  Initialize the output stream. This method must be called after creating
        /// a new instance of this class and before writing any SIFDataObjects to
        /// the stream.
        /// 
        /// </summary>
        /// <param name="zone">The Zone associated with messages that will be written
        /// to the stream
        /// </param>
        /// <param name="query">The Query object to use as a source for any SIF_Element
        /// filter restrictions that might be placed on this SIF_Response
        /// </param>
        /// <param name="requestSourceId">The SourceId of the associated SIF_Request message
        /// </param>
        /// <param name="requestMsgId">The MsgId of the associated SIF_Request message
        /// </param>
        /// <param name="requestSIFMessageVersion">The version of the SIF_Message envelope
        /// of the SIF_Request message
        /// </param>
        /// <param name="maxSize">The maximum size of rendered SifDataObject that will be
        /// accepted by this stream. If a SifDataObject is written to the stream
        /// and its size exceeds this value after rendering the object to an XML
        /// stream, an ObjectTooLargeException will be thrown by the <i>write</i>
        /// method
        /// </param>
        public abstract void Initialize(
            IZone zone,
            Query query,
            String requestSourceId,
            String requestMsgId,
            SifVersion requestSIFMessageVersion,
            int maxSize);


        /// <summary>  Initialize the output stream. This method must be called after creating
        /// a new instance of this class and before writing any SIFDataObjects to
        /// the stream.
        /// 
        /// </summary>
        /// <param name="zone">The Zone associated with messages that will be written
        /// to the stream
        /// </param>
        /// <param name="fieldRestrictions">Any field restrictions that were specified in the
        /// SIF_Request message. Call <see cref="OpenADK.Library.Query.FieldRestrictions"/>
        /// to obtain this array
        /// </param>
        /// <param name="requestSourceId">The SourceId of the associated SIF_Request message
        /// </param>
        /// <param name="requestMsgId">The MsgId of the associated SIF_Request message
        /// </param>
        /// <param name="requestSIFMessageVersion">The version of the SIF_Message envelope
        /// of the SIF_Request message
        /// </param>
        /// <param name="maxSize">The maximum size of rendered SifDataObject that will be
        /// accepted by this stream. If a SifDataObject is written to the stream
        /// and its size exceeds this value after rendering the object to an XML
        /// stream, an ObjectTooLargeException will be thrown by the <i>write</i>
        /// method
        /// </param>
        public abstract void Initialize(
            IZone zone,
            IElementDef [] fieldRestrictions,
            String requestSourceId,
            String requestMsgId,
            SifVersion requestSIFMessageVersion,
            int maxSize );

        /// <summary>
        /// Closes the data stream
        /// </summary>
        public abstract void Close();

        /// <summary>  Called by the class framework when the Publisher.onQuery method has
        /// returned successfully
        /// </summary>
        public abstract void Commit();

        /// <summary>  Called by the class framework when the Publisher.onQuery method has
        /// thrown an exception other than SifException
        /// </summary>
        public abstract void Abort();

        /// <summary>  Write a SifDataObject to the stream</summary>
        public abstract void Write( SifDataObject data );

        /// <summary>
        /// Defer sending SIF_Response messages and ignore any objects written to this stream.
        /// </summary>
        /// <remarks>
        /// See the <see cref="OpenADK.Library.SifResponseSender"/> class comments for
        /// more information about using this method.
        /// </remarks>
        /// <since>ADK 1.5.1</since>
        public abstract void DeferResponse();

        /// <summary>  Calculate the size of a SIF_Response minus the SIF_ObjectData content.</summary>
        protected virtual long CalcEnvelopeSize( ZoneImpl zone )
        {
            long size = 400;

            try {
                SIF_Response rsp = new SIF_Response();
                rsp.SIF_MorePackets = "Yes";
                rsp.SIF_RequestMsgId = fReqId;
                rsp.SIF_PacketNumber = 100000000;
                SIF_ObjectData data = new SIF_ObjectData();
                data.TextValue = " ";
                rsp.SIF_ObjectData = data;

                SIF_Header hdr = rsp.Header;

                hdr.SIF_Timestamp = DateTime.Now;
                hdr.SIF_MsgId = "012345678901234567890123456789012";
                hdr.SIF_SourceId = zone.Agent.Id;
                hdr.SIF_Security = zone.Dispatcher.secureChannel();
                hdr.SIF_DestinationId = fDestId;

                using ( MemoryStream buffer = new MemoryStream() ) {
                    SifWriter writer = new SifWriter( buffer );
                    writer.Write( rsp );
                    writer.Flush();
                    size = buffer.Length + 10;
                    writer.Close();
                    buffer.Close();
                }
            }
            catch( Exception ex ) {
                zone.Log.Warn( "Error calculating packet size: " + ex, ex );
            }

            return size;
        }


        /// <summary>
        /// Tells the DataObjectOutputStream to automatically filter out any SIFDataObjects that do
        /// not match the conditions specified in the provided Query object.
        /// </summary>
        /// <remarks>Any SIFDataObject that does not meet the conditions specified in the Query will not be
        /// written to the underlying data stream.
        /// </remarks>
        /// <value>The Query object to use when filtering data or <c>null</c> to remove the filter </value>
       public abstract Query Filter { get; set; }


        /// <summary>
        /// Gets or sets the value that will be set to the SIF_MorePackets element in the message
        /// after this DataObjectOutputStream is closed
        /// </summary>
        public abstract YesNo SIF_MorePackets
        {
            get; set;
        }


        /// <summary>
        /// Sets the value that will be used for the number of the first packet created by the output stream
        /// </summary>
        /// <exception cref="InvalidOperationException">thrown if setting the value after an object has 
        /// already been written to the output stream</exception>
        public abstract int SIF_PacketNumber
        {
            get; set;
        }



        #region IDisposable Members

        /// <summary>
        /// Release any unmanaged resources held on to by this object
        /// </summary>
        public abstract void Dispose();

        #endregion
    }
}

