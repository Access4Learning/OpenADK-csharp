//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  Called when the Adk cannot dispatch an inbound message to a message handler.
    /// 
    /// The Adk does not require that you implement this interface. However, if you
    /// want to customize the default error handling, you can implement this interface
    /// and register your handler with a Zone or the Agent object. This is done by
    /// calling the <code>Zone.setErrorHandler</code> or <code>Agent.setErrorHandler</code>
    /// methods. The class framework attempts to call the zone handler first followed
    /// by the agent handler.
    /// 
    /// UndeliverableMessageHandler is called in the following situations:
    /// 
    /// <ul>
    /// <li>
    /// <b>Dispatching Errors</b><br/><br/>
    /// If a SIF_Event, SIF_Request, or SIF_Response
    /// message is received but the Adk cannot determine the <i>Subscriber</i>,
    /// <i>Publisher</i>, or <i>QueryResults</i> message handler to dispatch
    /// the message to, the <code>onDispatchError</code> method is invoked.
    /// This condition may arise if you are using topic classes but have not
    /// established a <i>Topic</i> for the SIF data object type associated
    /// with the incoming message.<br/><br/>
    /// 
    /// Default Behavior: If there is no UndeliverableMessageHandler registered
    /// with the Zone or Agent object, the Adk returns an error SIF_Ack to
    /// the ZIS indicating the specified SIF data object type is not
    /// supported by the agent:<br/><br/>
    /// 
    /// <code>
    /// &lt;SIF_Ack&gt;<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&lt;SIF_Error&gt;<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;SIF_Category&gt;0&lt;/SIF_Category&gt;<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;SIF_Code&gt;0&lt;/SIF_Code&gt;<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;SIF_Desc&gt;The agent does not support this object type&lt;/SIF_Desc&gt;<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&lt;SIF_ExtendedDesc&gt;BusInfo&lt;/SIF_ExtendedDesc&gt;<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&lt;/SIF_Error&gt;<br/>
    /// &lt;/SIF_Ack&gt;<br/>
    /// </code>
    /// <br/><br/>
    /// 
    /// </li>
    /// </ul>
    /// 
    /// </summary>
    public interface IUndeliverableMessageHandler
    {
        /// <summary>  Called when the Adk cannot dispatch a SIF_Event, SIF_Request, or SIF_Response message
        /// 
        /// </summary>
        /// <param name="message">The message that failed to dispatch
        /// </param>
        /// <param name="zone">The zone the message was received on
        /// </param>
        /// <param name="info">Additional information about the message (e.g. its header fields)
        /// </param>
        /// <returns> true if this method has handled the error, or false if the Adk
        /// should apply its default error handling
        /// 
        /// </returns>
        /// <exception cref="OpenADK.Library.SifException"> SifException If a SifException is thrown, the Adk returns a SIF_Ack message
        /// to the Zone Integration Server using the error category, error code,
        /// description and extended description provided by the exception.
        /// Otherwise, a success SIF_Ack is returned.
        /// </exception>
        bool OnDispatchError( SifMessagePayload message,
                              IZone zone,
                              IMessageInfo info );
    }
}
