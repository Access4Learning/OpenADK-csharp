//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using OpenADK.Library.Infra;

namespace OpenADK.Library
{
    /// <summary>  Encapsulates information about a <c>SIF_Message</c>, including its
    /// payload type, header fields, and raw XML message content.
    /// 
    /// An instance of this class is passed to the <i>MessageInfo</i> parameter of
    /// all Adk message handlers like Subscriber.onEvent, Publisher.onQuery, and
    /// QueryResults.onQueryResults so that implementations of those methods can
    /// access header fields or XML content associated with an incoming message.
    /// Callers should cast the <i>MessageInfo</i> object to a SifMessageInfo type
    /// in order to call the methods of this class that are specific to the Schools
    /// Interoperability Framework.
    /// 
    /// 
    /// Note that raw XML content is only retained if the "<c>adk.messaging.keepMessageContent</c>"
    /// agent property is enabled. Otherwise, the <c>getMessage</c> method
    /// returns a <c>null</c> value. Refer to the AgentProperties class for a
    /// description of all agent and zone properties.
    /// 
    /// 
    /// To use SifMessageInfo, cast the <i>MessageInfo</i> parameter as shown below.
    /// 
    /// 
    /// <c>
    /// &nbsp;&nbsp;&nbsp;&nbsp;public void onEvent( Event event, Zone zone, MessageInfo info )<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;{<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SifMessageInfo inf = (SifMessageInfo)info;<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;String sourceId = inf.getSourceId();<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;String msgId = inf.getMsgId();<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SifVersion version = inf.getSIFVersion();<br/><br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;// Display some information about this SIF_Event...<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;System.out.println( "SIF_Event message with ID " + msgId + <br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" received from agent " + sourceId + <br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" in zone " + zone.getZoneId() + "."<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" This is a SIF " + version.toString() + " message." );<br/>
    /// <br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;...<br/>
    /// </c>
    /// </p>
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public sealed class SifMessageInfo : IMessageInfo
    {
        private IDictionary<string, string> fAttr = new Dictionary<string, string>();
        private IDictionary fObjects = new ListDictionary();
        private SifContext [] fContexts;
        private IZone fZone;
        private SifMessageType fPayload;
        private string fMessage;
        private SIF_Header fHeader;
        private SifVersion fPayloadVersion;

        /// <summary>  Called by the Adk to construct a SifMessageInfo instance</summary>
        public SifMessageInfo()
            {}

        /// <summary>  Called by the Adk to construct a SifMessageInfo</summary>
        /// <param name="msg">The SIF_Message
        /// </param>
        /// <param name="zone">The associated zone
        /// </param>
        public SifMessageInfo( SifMessagePayload msg,
                               IZone zone )
        {
            fZone = zone;
            fPayload = Adk.Dtd.GetElementType( msg.ElementDef.Name );
            if ( zone.Properties.KeepMessageContent ) {
                try {
                    StringWriter sw = new StringWriter();
                    SifWriter writer = new SifWriter( sw );
                    writer.Write( msg );
                    writer.Flush();
                    writer.Close();
                    sw.Close();

                    fMessage = sw.ToString();
                }
                catch {
                    // Do nothing
                }
            }

            //  Set SIF_Header values
            fHeader = msg.Header;
            IList<SifContext> contexts = msg.SifContexts;
            fContexts = new SifContext[ contexts.Count ];
            contexts.CopyTo( fContexts, 0 );

            //  Set information about the message payload
            fPayloadVersion = msg.SifVersion;
            switch ( fPayload ) {
                case SifMessageType.SIF_Request:
                    {
                        SIF_Request req = (SIF_Request) msg;
                        fObjects["SIF_MaxBufferSize"] = req.SIF_MaxBufferSize;
                        fObjects["SIF_RequestVersions"] = req.parseRequestVersions( fZone.Log );
                    }
                    break;

                case SifMessageType.SIF_Response:
                    {
                        SIF_Response rsp = (SIF_Response) msg;
                        this.SIFRequestMsgId = rsp.SIF_RequestMsgId;
                        fObjects["SIF_PacketNumber"] = rsp.SIF_PacketNumber;
                        SetAttribute( "SIF_MorePackets", rsp.SIF_MorePackets );
                    }
                    break;
            }
        }

        /// <summary>Gets or sets the SIF_Header encapsulated by this object.</summary>
        /// <value> The SIF_Header instance extracted from the message passed to
        /// the constructor or assigned to the Message.
        /// </value>
        public SIF_Header SIFHeader
        {
            get { return fHeader; }

            set { fHeader = value; }
        }

        /// <summary>  Gets the zone from which the message originated.</summary>
        /// <value> The Zone instance from which the message originated
        /// </value>
        public IZone Zone
        {
            get { return fZone; }
        }

        /// <summary>Gets the SIF payload message type.</summary>
        /// <value>Returns the int value of a SifMessageType enum value</value>
        /// <remarks>The value returned from <c>PayloadType</c> can be safely cast to 
        /// a <see cref="SifMessageType"/>
        /// </remarks>
        public int PayloadType
        {
            get { return (int) fPayload; }
        }

        /// <summary>  Gets the SIF payload message element tag.</summary>
        /// <value> The element tag of the message (e.g. "SIF_Request")
        /// </value>
        public string PayloadTag
        {
            get { return Adk.Dtd.GetElementTag( (int) fPayload ); }
        }

        /// <summary>  Gets the SIF_Message header timestamp.</summary>
        /// <value> The <c>SIF_Header/SIF_Date</c> and <c>SIF_Header/SIF_Time</c> 
        /// element values as a Date instance, identifying the time and date the
        /// message was sent
        /// </value>
        /// <seealso cref="TimeZone">
        /// </seealso>
        public DateTime? Timestamp
        {
            get { return fHeader.SIF_Timestamp; }
        }

        /// <summary>  Gets the value of the <c>SIF_MsgId</c> header element</summary>
        /// <value> The value of the <c>SIF_Header/SIF_MsgId</c> element, the
        /// unique GUID assigned to the message by its sender 
        /// </value>
        public string MsgId
        {
            get { return fHeader.SIF_MsgId; }
        }

        /// <summary>  Gets the value of the <c>SIF_SourceId</c> header element</summary>
        /// <returns> The value of the <c>SIF_Header/SIF_SourceId</c> element,
        /// which identifies the agent that originated the message 
        /// </returns>
        public string SourceId
        {
            get { return fHeader.SIF_SourceId; }
        }

        /// <summary>  Gets the value of the <c>SIF_DestinationId</c> header element</summary>
        /// <returns> The value of the optional <c>SIF_Header/SIF_SourceId</c> element.
        /// When present, it identifies the agent to which the message should be routed
        /// by the zone integration server.
        /// </returns>
        public string DestinationId
        {
            get { return fHeader.SIF_DestinationId; }
        }

        /// <summary>  Gets the value of the optional <c>SIF_Security/SIF_SecureChannel/SIF_AuthenticationLevel</c> header element</summary>
        /// <returns> The authentication level or zero if not specified
        /// </returns>
        public int AuthenticationLevel
        {
            get
            {
                try {
                    return
                        Int32.Parse
                            ( fHeader.SIF_Security.SIF_SecureChannel.SIF_AuthenticationLevel );
                }
                catch {
                    return 0;
                }
            }
        }

        /// <summary>  Gets the value of the optional <c>SIF_Security/SIF_SecureChannel/SIF_EncryptionLevel</c> header element</summary>
        /// <returns> The encryption level or zero if not specified
        /// </returns>
        public int EncryptionLevel
        {
            get
            {
                try {
                    return Int32.Parse( fHeader.SIF_Security.SIF_SecureChannel.SIF_EncryptionLevel );
                }
                catch {
                    return 0;
                }
            }
        }

        /// <summary>  Gets the content of the raw XML message</summary>
        /// <returns> The raw XML message content as it was received by the Adk. If
        /// the <c>adk.keepMessageContent</c> agent or zone property has
        /// a value of "false" (the default), null is returned.
        /// </returns>
        public string Message
        {
            get { return fMessage; }
        }

        /// <summary>  Gets the version of SIF associated with the message. The version is
        /// determined by inspecting the <i>xmlns</i> attribute of the SIF_Message
        /// envelope.
        /// </summary>
        /// <returns> A SifVersion object identifying the version of SIF associated
        /// with the message
        /// </returns>
        /// <seealso cref="SifRequestVersion">
        /// </seealso>
        public SifVersion SifVersion
        {
            get { return fPayloadVersion; }
        }

        /// <summary>  For SIF_Response messages, gets or sets the SIF_MsgId of the associated SIF_Request.</summary>
        /// <value> The value of the <c>SIF_Header/SIF_RequestMsgId</c> element, or
        /// null if the message encapsulated by this SifMessageInfo instance is not a 
        /// SIF_Response message
        /// </value>
        public string SIFRequestMsgId
        {
            get { return GetAttribute( "SIF_RequestMsgId" ); }

            set { SetAttribute( "SIF_RequestMsgId", value ); }
        }

        /// <summary>  For SIF_Request messages, gets  or sets the SIF version responses should conform to.</summary>
        /// <value> The value of the <c>SIF_Request/SIF_Version</c> element or 
        /// null if the message is not a SIF_Request message
        /// </value>
        public SifVersion[] SIFRequestVersions
        {
            get { return (SifVersion []) fObjects["SIF_RequestVersions"]; }

            set { fObjects["SIF_RequestVersions"] = value; }
        }

        /// <summary>
        /// For SIF_Request messages, gets the latest SIF version
        /// that was requested by the requestor and is supported by the ADK
        /// </summary>
        /// <value>The value of the <code>SIF_Request/SIF_Version</code> element or
        /// null if the message is not a SIF_Request message</value>
        public SifVersion LatestSIFRequestVersion
        {
            get { return Adk.GetLatestSupportedVersion( SIFRequestVersions ); }
        }


        /// <summary>
        /// Returns information about the request, including the original request time and 
        /// user state information
        /// </summary>
        public IRequestInfo SIFRequestInfo
        {
            get { return (IRequestInfo) fObjects["SIFRequestInfo"]; }
            set { fObjects["SIFRequestInfo"] = value; }
        }


        /// <summary>  For SIF_Request messages, identifies the type of object requested</summary>
        /// <value> An ElementDef constant from the SifDtd class
        /// </value>
        public IElementDef SIFRequestObjectType
        {
            get { return (IElementDef) fObjects["SIF_RequestObjectType"]; }

            set { fObjects["SIF_RequestObjectType"] = value; }
        }

        /// <summary>  For SIF_Response messages, gets the packet number</summary>
        /// <returns> The int value of the <c>SIF_Response/SIF_PacketNumber</c>
        /// element or null if the message is not a SIF_Response message
        /// </returns>
        public int? PacketNumber
        {
            get { return (int?) fObjects["SIF_PacketNumber"]; }
        }

        /// <summary>  For SIF_Response messages, determines if more packets are to be expected</summary>
        /// <returns> The string value of the <c>SIF_Response/SIF_MorePackets</c>
        /// element or null if the message is not a SIF_Response message or the
        /// element is missing.
        /// </returns>
        public bool MorePackets
        {
            get
            {
                string s = GetAttribute( "SIF_MorePackets" );
                return s == null ? false : s.ToUpper().Equals( "yes".ToUpper() );
            }
        }

        /// <summary>  For SIF_Request messages, gets the maximum packet size of result packets</summary>
        /// <returns> The value of the <c>SIF_Request/SIF_MaxBufferSize</c>
        /// element or zero if the message is not a SIF_Request message or the
        /// buffer size could not be converted to an integer
        /// </returns>
        public int? MaxBufferSize
        {
            get { return (int?) fObjects["SIF_MaxBufferSize"]; }
        }

        public string [] AttributeNames
        {
            get { return null; }
        }


        public string GetAttribute( string attr )
        {
            string attributeValue = null;
            fAttr.TryGetValue( attr, out attributeValue );
            return attributeValue;
        }

        public void SetAttribute( string attr,
                                  string val )
        {
            fAttr[attr] = val;
        }


        /// <summary>
        /// Gets the SIF Contexts that this message applies to
        /// </summary>
        public SifContext [] SIFContexts
        {
            get { return fContexts; }
        }

        public static SifMessageInfo Parse(TextReader reader, bool keepMessage, IZone zone)
        {
            StringWriter writer = null;

            try
            {
                if ( keepMessage )
                {
                    writer = new StringWriter();
                }

                //  Header info, payload type, and full message if desired
                SifMessageInfo inf = new SifMessageInfo();
                inf.fZone = zone;

                int ch;
                int elements = 0;
                int bytes = 0;
                bool inTag = false;
                bool inHeader = false;
                bool storValue = false;
                bool ack = false, response = false;

                //  Buffer for retaining message content
                char[] buf = keepMessage ? new char[1024] : null;

                //  Tag buffer size is 16*2, enough for largest SIF10r1 element name
                StringBuilder tag = new StringBuilder( 16*2 );
                //  Value buffer size is 16*3, enough for GUID
                StringBuilder value = new StringBuilder( 16*3 );

                //  SIF 1.0r1 Optimization: as soon as we parse the SIF_Header and
                //  optionally the SIF_OriginalMsgId and SIF_OriginalSourceId elements
                //  of a SIF_Ack we're done. So continue for as long as
                //  required_elements != 3.
                //
                int required_elements = 0;

                while ( reader.Peek() > -1 && required_elements != 3 )
                {
                    ch = reader.Read();

                    if ( keepMessage )
                    {
                        buf[bytes++] = (char) ch;
                        if ( bytes == buf.Length - 1 )
                        {
                            writer.Write( buf, 0, bytes );
                            bytes = 0;
                        }
                    }

                    if ( ch == '<' )
                    {
                        inTag = true;
                        storValue = false;
                    }
                    else if ( ch == ' ' && inTag )
                    {
                        inTag = false;
                        storValue = true; // attributes follow
                    }
                    else if ( ch == '>' )
                    {
                        if ( storValue )
                        {
                            Console.WriteLine( "Attributes: " + value.ToString() );
                        }

                        //  We now have text of next element
                        switch ( elements )
                        {
                            case 0:
                                //  Ensure first element is <SIF_Message>
                                if ( !tag.ToString().Equals( "SIF_Message" ) )
                                {
                                    throw new AdkMessagingException( "Message does not begin with SIF_Message", zone );
                                }
                                break;

                            case 1:
                                //
                                //  Payload element (e.g. "SIF_Ack", "SIF_Register", etc.)
                                //  Ask the DTD object for a type code for this message, store
                                //  it as the payload type in SIFMessageInfo. If zero, it means
                                //  the element is not recognized as a valid payload type for
                                //  this version of SIF.
                                //
                                inf.fPayload = Adk.Dtd.GetElementType( tag.ToString() );
                                if ( inf.fPayload == 0 )
                                {
                                    throw new AdkMessagingException(
                                        "<" + tag.ToString() + "> is not a valid payload message", zone );
                                }

                                //  Is this a SIF_Ack or SIF_Response?
                                ack = (inf.fPayload == SifMessageType.SIF_Ack);
                                response = ack ? false : (inf.fPayload == SifMessageType.SIF_Response);
                                if ( !ack && !response )
                                    required_elements += 2;
                                else if ( response )
                                    required_elements += 1;

                                break;

                            default:
                                String s = tag.ToString();
                                if ( inHeader )
                                {
                                    //  End of a header element, or </SIF_Header>...
                                    if ( s[0] == '/' )
                                    {
                                        if ( s.Equals( "/SIF_Header" ) )
                                        {
                                            inHeader = false;
                                            required_elements++;
                                        }
                                        else if ( !(s.StartsWith( "/SIF_Sec" )) )
                                        {
                                            inf.SetAttribute( s.Substring( 1 ), value.ToString() );
                                        }
                                    }
                                    else
                                        storValue = true;
                                }
                                else // if ( !inHeader )
                                {
                                    if ( s.Equals( "SIF_Header" ) )
                                    {
                                        //  Begin <SIF_Header>
                                        // TODO: This class maintains SIF_Header information in the fHeader
                                        // variable. This particular parsing mechanism doesn't re-create a SIF_Header
                                        // element. Therefore if the parse method is used, the only way to get these
                                        // properties back out is to use the getAttribute() call.
                                        inHeader = true;
                                    }
                                    else if ( ack )
                                    {
                                        //  SIF_Ack / SIF_OriginalSourceId or SIF_OriginalMsgId
                                        if ( s.StartsWith( "SIF_Orig" ) )
                                            storValue = true;
                                        else if ( s.StartsWith( "/SIF_Orig" ) )
                                        {
                                            required_elements++;
                                            inf.SetAttribute( s.Substring( 1 ), value.ToString() );
                                        }
                                    }
                                    else if ( response )
                                    {
                                        //  SIF_Response / SIF_RequestMsgId
                                        if ( s.StartsWith( "SIF_Req" ) )
                                            storValue = true;
                                        else if ( s.StartsWith( "/SIF_Req" ) )
                                        {
                                            required_elements++;
                                            inf.SetAttribute( s.Substring( 1 ), value.ToString() );
                                        }
                                    }
                                }

                                value.Length = 0;
                                break;
                        }

                        inTag = false;
                        tag.Length = 0;
                        elements++;
                    }
                    else
                    {
                        if ( inTag )
                            tag.Append( (char) ch );
                        else if ( storValue )
                            value.Append( (char) ch );
                    }
                }

                if ( writer != null )
                {
                    //  Read the remainder of the input stream and copy it to the
                    //  output buffer
                    if ( bytes > 0 )
                        writer.Write( buf, 0, bytes );
                    while ( reader.Peek() > -1 )
                    {
                        bytes = reader.Read( buf, 0, buf.Length - 1 );
                        writer.Write( buf, 0, bytes );
                    }

                    //  Store message content
                    writer.Flush();
                    inf.fMessage = writer.GetStringBuilder().ToString();
                }

                return inf;
            }
            finally
            {
                if ( writer != null )
                {
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append( "SIFMessageInfo Contents\r\n{\r\n" );
            foreach ( KeyValuePair<String, String> entry in fAttr ) {
                str.Append( "\t" );
                str.Append( entry.Key );
                str.Append( '=' );
                str.Append( entry.Value );
                str.Append( "\r\n" );
            }

            foreach ( KeyValuePair<String, Object> entry in fObjects ) {
                str.Append( "\t" );
                str.Append( entry.Key );
                str.Append( '=' );
                if ( entry.Value.GetType().IsArray ) {
                    object [] values = entry.Value as object [];
                    str.Append( "{ " );
                    foreach ( object val in values ) {
                        str.Append( val );
                        str.Append( ',' );
                    }
                    str.Append( " }" );
                }
                else {
                    str.Append( entry.Value );
                }
                str.Append( "\r\n" );
            }

            str.Append( "}" );

            return str.ToString();
        }
    }
}
