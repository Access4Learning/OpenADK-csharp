//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;

namespace OpenADK.Library
{
    ////////////////////////////////////////////////////////////////////////////////
    //
    //  Copyright ©2001-2008 OpenADK
    //  All rights reserved.
    //
    //  This software is the confidential and proprietary information of
    //  Data Solutions ("Confidential Information").  You shall not disclose
    //  such Confidential Information and shall use it only in accordance with the
    //  terms of the license agreement you entered into with Data Solutions.
    //
    /// <summary>	The base interface for all listener interfaces related to the exchange of
    /// messages over the underlying messaging infrastructure. For example, 
    /// SIFMessagingListener extends this interface for the Schools Interoperability
    /// Framework.
    /// </summary>
    public interface IMessagingListener
    {
        /// <summary> 	Called when a message has been received by the framework, before it is
        /// dispatched to the Adk's message handlers. 
        /// 
        /// An agent can implement this method to count the number of messages 
        /// received, to signal the user interface or other interested parties that 
        /// a message has arrived, to examine or change the raw content of incoming
        /// messages before they're dispatched to the framework, or to prevent the 
        /// framework from dispatching messages to message handlers. When filtering 
        /// messages, return <code>true</code> to allow the framework to process 
        /// this message, <code>false</code> to silently discard the message, or 
        /// throw an AdkException to return an error to the server.
        /// 
        /// 
        /// </summary>
        /// <param name="messageType">A message type constant (e.g. <code>SifDtd.SIF_EVENT</code>)	
        /// </param>
        /// <param name="message">A StringBuffer containing the message to be sent. The
        /// contents of the buffer may be modified by this method.
        /// 
        /// </param>
        /// <returns>a value indicating what to do with the message
        /// </returns>
        /// <exception cref="AdkException">  may be thrown by this method to return an error 
        /// acknowledgement to the server
        /// </exception>
        MessagingReturnCode OnMessageReceived( SifMessageType messageType,
                                               StringBuilder message );

        /// <summary> 	Called when a message has been received by the framework and successfully
        /// dispatched to the Adk's message handlers. If a message is received but 
        /// not processed by a message handler -- either because of an error or
        /// because there is no <i>Publisher</i>, <i>Subscriber</i>, or <i>QueryResults</i>
        /// message handler to handle it -- this method is never called.
        /// 
        /// </summary>
        /// <param name="messageType">A message type constant (e.g. <code>SifDtd.SIF_EVENT</code>)	
        /// </param>
        /// <param name="info">The MessageInfo instance associated with the message
        /// </param>
        /// <exception cref="OpenADK.Library.AdkException"> AdkException An AdkException may be thrown by this method 
        /// to return an error acknowledgement to the server
        /// </exception>
        void OnMessageProcessed( SifMessageType messageType,
                                 IMessageInfo info );

        /// <summary> 	Called when a message is about to be sent by the framework.
        /// 
        /// An agent can implement this method to filter outbound messages in order
        /// to change the message content or prevent the framework from sending a
        /// message to the server. When filtering messages, return <code>true</code> 
        /// to allow the framework to send this message or <code>false</code> to 
        /// silently discard the message.
        /// 
        /// 
        /// </summary>
        /// <param name="messageType">A message type constant (e.g. <code>SifDtd.SIF_EVENT</code>)	
        /// </param>
        /// <param name="info">The MessageInfo instance associated with the message
        /// </param>
        /// <param name="message">A StringBuffer containing the message to be sent. The
        /// contents of the buffer may be modified by this method.
        /// </param>
        bool OnSendingMessage( SifMessageType messageType,
                               IMessageInfo info,
                               StringBuilder message );

        /// <summary> 	Called when a message has been sent by the framework.
        /// 
        /// An agent can implement this method to count the number of messages 
        /// sent or to signal the user interface or other interested parties that a 
        /// message has been sent.
        /// 
        /// </summary>
        /// <param name="messageType">A message type constant defined by a subclass of	this interface
        /// </param>
        /// <param name="info">The MessageInfo instance associated with the message
        /// </param>
        /// <param name="receipt">The acknowledgement or receipt returned by the server
        /// </param>
        void OnMessageSent( SifMessageType messageType,
                            IMessageInfo info,
                            Object receipt );
    }
}
