//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>DataObjectOutputStream is supplied to the <c>IPublisher.OnRequest</c>message.
    /// handler to allow agents to stream an arbitrarily large set of SifDataObjects when 
    /// responding to SIF_Request messages. The ADK handles packetizing the objects 
    /// into SIF_Response packets, which are stored in a cache on the local file system 
    /// until they can be delivered to the zone.</summary>
    /// <remarks>
    /// ADK 1.5.1 introduces a mechanism to decouple SIF_Request processing from 
    ///	SIF_Response delivery, which is necessary when implementing the StudentLocator 
    ///	message choreography or to place inbound SIF_Request messages in a work queue
    ///	for processing at a later time. Call the <see cref="IDataObjectOutputStream.DeferResponse"/>
    ///	method to inform 
    ///	the ADK that you will take responsibility for processing the SIF_Request message 
    ///	at a later time after the <c>Publisher.onRequest</c> method has completed. 
    ///	Note the ADK immediately acknowledges the SIF_Request when <c>OnRequest</c>
    ///	returns, but because the <c>DeferResponse</c> method has been called it 
    ///	does not attempt to send cached SIF_Response packets to the zone. When your agent 
    ///	is ready to process the request, it can use the <see cref="OpenADK.Library.SifResponseSender"/> 
    ///	class to stream, packetize, and deliver SIF_Responses. See the class comments
    ///	</remarks>
    public interface IDataObjectOutputStream
    {
        /// <summary>  Write a SifDataObject to the stream</summary>
        /// <param name="data">A SIFDataObject instance to write to the output stream</param>
        /// <exception cref="AdkException"></exception>
        void Write( SifDataObject data );

        /// <summary>
        /// Defer sending SIF_Response messages and ignore any objects written to this stream.
        /// </summary>
        /// <remarks>
        /// See the <see cref="OpenADK.Library.SifResponseSender"/> class comments for
        /// more information about using this method.
        /// </remarks>
        /// <since>ADK 1.5.1</since>
        void DeferResponse();

        /// <summary>
        /// Tells the DataObjectOutputStream to automatically filter out any SIFDataObjects that do
        /// not match the conditions specified in the provided Query object.
        /// </summary>
        /// <remarks>Any SIFDataObject that does not meet the conditions specified in the Query will not be
        /// written to the underlying data stream.
        /// </remarks>
        /// <value>The Query object to use when filtering data or <c>null</c> to remove the filter </value>
       Query Filter { get; set; }
    }
}

// Synchronzied with DataObjectOutputStream.java Branch Library-ADK-1.5.1 Version 3
