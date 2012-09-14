//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using OpenADK.Library.Global;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;

namespace OpenADK.Library
{
    /// <summary>
    /// Helper class to send SIF_Response messages when an agent defers automatic
    /// sending of responses in its implementation of the <c>IPublisher.OnRequest</c>
    ///	message handler. This class is designed to be used when the <see cref="OpenADK.Library.IDataObjectOutputStream.DeferResponse"/>
    ///	method is called during SIF_Request processing, and must be used outside of the
    /// Publisher.OnRequest message handler.
    /// </summary>	
    /// <remarks>
    /// <para>
    /// By design, the ADK automatically sends one or more SIF_Response messages to
    /// the zone when control is returned from the <c>Publisher.OnRequest</c>  
    /// message handler. SIF_Responses are sent in a background thread without any
    /// further intervention on the agent's part. The ADK takes care of proper data
    /// object rendering and packetizing based on the parameters of the SIF_Request
    /// message, and includes a SIF_Error element in the payload if an exception is
    /// thrown from the onRequest method.</para>
    /// <para>
    ///	Although this behavior is convenient and very appropriate for most agent 
    /// implementations, there are cases when agents require greater control over SIF 
    /// Request &amp; Response messaging. Specifically, agents sometimes need the 
    /// ability to decouple SIF_Response processing from SIF_Request processing. 
    /// For example, implementing the SIF 1.5 StudentLocator message choreography may 
    /// require that SIF_Response messages are sent hours or days after the initial 
    /// SIF_Request is received. Or, an agent might queue requests in a database
    /// table and work on them later when it has available resources. Both of these 
    /// scenarios can be achieved by calling the <see cref="OpenADK.Library.IDataObjectOutputStream.DeferResponse"/>
    /// method on the <c>IDataObjectOutputStream</c> passed to the <c>Publisher.OnRequest</c> 
    /// message handler. This method signals the ADK to ignore any SifDataObjects 
    /// written to the stream and to defer the sending of SIF_Response messages. The 
    /// agent must send its own SIF_Response messages at a later time by using this 
    /// SifResponseSender helper class
    /// </para>
    /// To use this class,
    /// 
    /// <ul>
    ///	<li>
    ///		When the agent is ready to send SIF_Response messages for a SIF_Request
    ///		that was received at an earlier time, instantiate a SifResponseSender.
    ///	</li>
    ///	<li>
    ///		Call the <see cref="SifResponseSender.Open"/> method and pass it the Zone you wish to send 
    ///		SIF_Response messages to. You must also pass the SIF_Version and 
    ///		SIF_MaxBufferSize value from the original SIF_Request message. (Be sure
    ///		to obtain these values from the <c>SifMessageInfo</c> parameter 
    ///		in your <c>Publisher.OnRequest</c> implementation so you can pass them to 
    ///		the <see cref="SifResponseSender.Open"/> method when using this class.)
    ///	</li>
    ///	<li>
    ///		Repeatedly call the <see cref="SifResponseSender.Write(SifDataObject)"/> method, once for each 
    ///		SIFDataObject that should be included in the SIF_Response stream
    ///	</li>
    ///	<li>
    ///		Call the <see cref="SifResponseSender.Write(SIF_Error)"/> method to include a SIF_Error element 
    ///		in the SIF_Response stream
    ///	</li>
    ///	<li>
    ///		When finished, call the <see cref="Close"/> method. The ADK will automatically 
    ///		package the objects into one or more SIF_Response packets in the same way it 
    ///		does for normal request/response processing via the <c>Publisher.OnRequest</c> 
    ///		method.
    ///	</li>
    ///	</ul>
    ///	</remarks>
    /// <since>ADK 1.5.1</since>
    public class SifResponseSender : IDisposable
    {
        protected IZone fZone;
        protected DataObjectOutputStreamImpl fOut = null;


        /// <summary>
        /// Open the SIFResponseSender to send SIF_Response messages to a specific zone.
        /// </summary>
        /// <param name="zone">The zone to send messages to</param>
        /// <param name="sifRequestMsgId">The SIF_MsgId from the original SIF_Request message.
        /// You can obtain this by accessing the value returned from <see cref="OpenADK.Library.SifMessageInfo.MsgId"/>
        /// on the SIFMessageInfo parameter passed to the Publisher.onRequest
        /// method. NOTE: Do not call <see cref="OpenADK.Library.SifMessageInfo.SIFRequestMsgId"/>
        /// as it will return a <c>null</c> value and is only intended to be called
        /// on SIF_Response messages.</param>
        /// <param name="sifRequestSourceId">The SIF_SourceId from the original SIF_Request message.
        /// You can obtain this by accessing the value returned from <see cref="OpenADK.Library.SifMessageInfo.SourceId"/>
        /// on the SIFMessageInfo parameter passed to the Publisher.onRequest
        /// method.</param>
        /// <param name="sifVersion">The SIF_Version value from the original SIF_Request message.
        /// You can obtain this by accessing the value returned from <see cref="OpenADK.Library.SifMessageInfo.SIFRequestVersions"/>
        /// on the SIFMessageInfo parameter passed to the Publisher.onRequest
        /// method.</param>
        /// <param name="maxBufferSize">The SIF_MaxBufferSize value from the original SIF_Request.
        /// You can obtain this by accessing the value returned from <see cref="OpenADK.Library.SifMessageInfo.MaxBufferSize"/>
        /// on the SIFMessageInfo parameter passed to the Publisher.onRequest
        /// method.</param>
        /// <param name="fieldRestrictions">An optional array of ElementDef constants, obtained or 
        ///	reconstructed from the <see cref="OpenADK.Library.Query.FieldRestrictions"/>
        ///	property from the original SIF_Request, that identify the subset of SIF elements 
        ///	to include in the data objects written to SIF_Response messages. If this array 
        ///	is provided, data objects will only have those elements specified; otherwise 
        ///	data objects contain all of their elements.</param>
        ///	<exception cref="System.ArgumentException">thrown if any of the parameter are invalid</exception>
        ///	<exception cref="AdkException">thrown if an error occurs preparing the output stream</exception>
        public void Open(
            IZone zone,
            String sifRequestMsgId,
            String sifRequestSourceId,
            SifVersion sifVersion,
            int maxBufferSize,
            IElementDef [] fieldRestrictions )
        {
            fZone = zone;
            fOut = DataObjectOutputStreamImpl.NewInstance();
            fOut.Initialize
                ( zone, fieldRestrictions, sifRequestSourceId, sifRequestMsgId, sifVersion,
                  maxBufferSize );
        }


        /// <summary>
        /// Open the SIFResponseSender to send SIF_Response messages to a specific zone.
        /// </summary>
        /// <param name="zone">The zone to send messages to</param>
        /// <param name="sifRequestMsgId">The SIF_MsgId from the original SIF_Request message.
        /// You can obtain this by accessing the value returned from <see cref="OpenADK.Library.SifMessageInfo.MsgId"/>
        /// on the SIFMessageInfo parameter passed to the Publisher.onRequest
        /// method. NOTE: Do not call <see cref="OpenADK.Library.SifMessageInfo.SIFRequestMsgId"/>
        /// as it will return a <c>null</c> value and is only intended to be called
        /// on SIF_Response messages.</param>
        /// <param name="sifRequestSourceId">The SIF_SourceId from the original SIF_Request message.
        /// You can obtain this by accessing the value returned from <see cref="OpenADK.Library.SifMessageInfo.SourceId"/>
        /// on the SIFMessageInfo parameter passed to the Publisher.onRequest
        /// method.</param>
        /// <param name="sifVersion">The SIF_Version value from the original SIF_Request message.
        /// You can obtain this by accessing the value returned from <see cref="OpenADK.Library.SifMessageInfo.SIFRequestVersions"/>
        /// on the SIFMessageInfo parameter passed to the Publisher.onRequest
        /// method.</param>
        /// <param name="maxBufferSize">The SIF_MaxBufferSize value from the original SIF_Request.
        /// You can obtain this by accessing the value returned from <see cref="OpenADK.Library.SifMessageInfo.MaxBufferSize"/>
        /// on the SIFMessageInfo parameter passed to the Publisher.onRequest
        /// method.</param>
        /// <param name="query">An optional array of ElementDef constants, obtained or 
        ///	reconstructed from the <see cref="OpenADK.Library.Query.FieldRestrictions"/>
        ///	property from the original SIF_Request, that identify the subset of SIF elements 
        ///	to include in the data objects written to SIF_Response messages. If this array 
        ///	is provided, data objects will only have those elements specified; otherwise 
        ///	data objects contain all of their elements.</param>
        ///	<exception cref="System.ArgumentException">thrown if any of the parameter are invalid</exception>
        ///	<exception cref="AdkException">thrown if an error occurs preparing the output stream</exception>
        public void Open(
            IZone zone,
            String sifRequestMsgId,
            String sifRequestSourceId,
            SifVersion sifVersion,
            int maxBufferSize,
            Query query)
        {
            fZone = zone;
            fOut = DataObjectOutputStreamImpl.NewInstance();
            fOut.Initialize
                (zone, query, sifRequestSourceId, sifRequestMsgId, sifVersion,
                  maxBufferSize);
        }


        /// <summary>
        /// Write a SIFDataObject to the output stream
        /// </summary>
        /// <param name="sdo"></param>
        public void Write( SifDataObject sdo )

        {
            _checkOpen();

            fOut.Write( sdo );
        }

        /// <summary>
        /// Write a SIF_Error to the output stream
        /// </summary>
        /// <param name="error">A SIF_Error instance</param>
        public void Write( SIF_Error error )
        {
            _checkOpen();

            fOut.SetError( error );
        }

        /// <summary>
        /// Close the stream and send one or more SIF_Response packets to the zone.
        /// </summary>
        public void Close()
        {
            _checkOpen();

            try {
                fOut.Close();
            }
            catch ( IOException ioe ) {
                throw new AdkException( "Failed to close SIFResponseSender stream: " + ioe, fZone );
            }

            fOut.Commit();
        }

        private void _checkOpen()
        {
            if ( fOut == null ) {
                throw new InvalidOperationException( "SIFResponseSender is not open" );
            }
        }


        /// <summary>
        ///  Gets or Sets the starting packet number for SIF_Responses.
        /// </summary>
        /// <remarks>
        ///  By default, the SIFResponseSender class will automatically set the starting 
        ///  packet number to 1 and increment the number automatically for each packet. 
        ///  However, some agents may need to respond to SIF_Requests with multiple, asynchronous
        ///  responses. In that case, the agent developer is responsible for keeping track of the
        ///  packet numbers that were previously sent and setting the correct starting packet number
        ///  for the next set of SIF_Response packtets.
        /// </remarks>
        /// <exception cref="InvalidOperationException">thrown if this property is set after objects have
        /// already been written</exception>
        public int SIF_PacketNumber
        {
            get { return fOut.SIF_PacketNumber; }
            set { fOut.SIF_PacketNumber = value; }
        }

        /// <summary>
        /// Gets or sets the value that will be set on the final SIF_Response packet
        /// </summary>
        public YesNo SIF_MorePackets
        {
            get { return fOut.SIF_MorePackets; }
            set { fOut.SIF_MorePackets = value; }
        }


        #region IDisposable Members

        public void Dispose()
        {
            if ( fOut != null ) {
                fOut.Dispose();
                fOut = null;
            }
        }

        #endregion
    }
}
