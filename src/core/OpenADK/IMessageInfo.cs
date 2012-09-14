//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>Provides information about an XML message, such as envelope and header attributes,
    ///the source of the message, and optionally the raw XML document that comprises
    ///the message.</summary>
    /// <remarks>
    /// <para>A concrete implementation of this interface is available for each 
    /// messaging protocol supported by the ADK. For instance, the <see cref="SifMessageInfo"/>
    /// class provides access to specific information regarding Schools Interoperability 
    /// Framework <c>SIF_Message</c> envelopes and <c>SIF_Header</c> headers.</para>
    /// <para><b>Raw XML Message Content</b></para>
    /// <para>
    /// When the <c>adk.keepMessageContent</c> agent property is enabled, the
    /// <see cref="IMessageInfo.Message"/> property will return the XML document that was received as a
    /// single String. When this option is disabled (the default), the ADK does not keep 
    /// a copy of raw message content in memory and will return a <c>null</c> value.</para>
    /// <para><b>Using MessageInfo in Message Handlers</b></para>
    /// <para>
    /// A MessageInfo instance is passed as a parameter to message handlers such as 
    /// <c>Publisher.onRequest</c> and <c>Subscriber.onEvent</c>.
    /// Cast the parameter to a concrete type to obtain protocol-specific information
    /// about the message.</para></remarks>
    /// <example>
    /// For example, the following shows how to obtain the value of 
    /// the <c>SIF_SourceId</c> and <c>SIF_MsgId</c> elements from the
    /// <c>SIF_Header</c>, as well as the version of SIF declared by the 
    /// <c>SIF_Message</c> envelope:
    /// 
    ///  <code>
    ///      public void onEvent( Event event, Zone zone, MessageInfo info ) 
    ///      { 
    ///          // Cast to SIFMessageInfo to get SIF-specific header info 
    ///          SIFMessageInfo inf = (SIFMessageInfo)info;  
    ///          // Obtain SIF-specific header values 
    ///          String sourceId = inf.getSourceId(); 
    ///          String msgId = inf.getMsgId(); 
    ///          SIFVersion version = inf.getSIFVersion();  
    ///          // Display some information about this SIF_Event... 
    ///          System.out.println( "SIF_Event message with ID " + msgId +  
    ///              " received from agent " + sourceId +  
    ///              " in zone " + zone.getZoneId() + "." 
    ///              " This is a SIF " + version.toString() + " message." ); 
    ///   
    ///          ... 
    ///  </code>
    ///  </example>
    ///  <author>Eric Petersen</author>
    ///  <since>ADK 1.0</since>
    public interface IMessageInfo
    {
        /// <summary>  Gets the SIF payload message type</summary>
        /// <returns> A <c>MSGTYP_</c> constant from the SifDtd class
        /// (e.g. <c>SifDtd.MSGTYPE_REQUEST</c>)
        /// </returns>
        int PayloadType { get; }

        /// <summary>  Gets the SIF payload message element tag</summary>
        /// <returns> The element tag of the message (e.g. "SIF_Request")
        /// </returns>
        string PayloadTag { get; }

        /// <summary>  Gets the zone from which the message originated</summary>
        IZone Zone { get; }

        /// <summary>  Gets the names of all attributes. Implementations of this interface may
        /// define attributes to hold values specific to the messaging protocol.
        /// </summary>
        /// <remarks>
        /// <seealso cref="IMessageInfo.SetAttribute"/>
        /// <seealso cref="IMessageInfo.GetAttribute"/>
        /// </remarks>
        string [] AttributeNames { get; }

        /// <summary>  Gets the content of the raw XML message</summary>
        /// <returns> The raw XML message content as it was received by the Adk. If
        /// the <c>adk.keepMessageContent</c> agent or zone property has
        /// a value of "false" (the default), null is returned.
        /// </returns>
        string Message { get; }

        /// <summary>  Gets the value of an attribute. Implementations of this interface may
        /// define attributes to hold values specific to the messaging protocol.
        /// </summary>
        /// <remarks>
        /// <seealso cref="IMessageInfo.SetAttribute"/>
        /// <seealso cref="IMessageInfo.AttributeNames"/>
        /// </remarks>
        string GetAttribute( string attr );

        /// <summary>  Sets the value of an attribute. Implementations of this interface may
        /// define attributes to hold values specific to the messaging protocol.
        /// </summary>
        /// <remarks>
        /// <seealso cref="IMessageInfo.GetAttribute"/>
        /// <seealso cref="IMessageInfo.AttributeNames"/>
        /// </remarks>
        void SetAttribute( string attr,
                           string val );
    }
}
