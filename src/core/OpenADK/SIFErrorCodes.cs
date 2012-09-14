//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>	Defines SIF 1.0r1 error category and code constants.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class SifErrorCodes
    {
        /// <summary>  Generic error</summary>
        public const int XML_GENERIC_ERROR_1 = 1;

        /// <summary>  Message is not well formed</summary>
        public const int XML_MALFORMED_2 = 2;

        /// <summary>  Generic validation error</summary>
        public const int XML_GENERIC_VALIDATION_3 = 3;

        /// <summary>  Invalid value for element</summary>
        public const int XML_INVALID_VALUE_4 = 4;

        /// <summary>  Reserved</summary>
        public const int XML_RESERVED_5 = 5;

        /// <summary>  Missing mandatory element</summary>
        public const int XML_MISSING_MANDATORY_ELEMENT_6 = 6;


        /// <summary>  Generic error</summary>
        public const int AUTH_GENERIC_ERROR_1 = 1;

        /// <summary>  Generic authentication error (with signature)</summary>
        public const int AUTH_GENERIC_AUTH_ERROR_2 = 2;

        /// <summary>  Missing sender's certificate</summary>
        public const int AUTH_NO_SENDER_CERT_3 = 3;

        /// <summary>  Invalid certificate</summary>
        public const int AUTH_INVALID_CERT_4 = 4;

        /// <summary>  Sender's certificate is not trusted</summary>
        public const int AUTH_SENDER_NOT_TRUSTED_5 = 5;

        /// <summary>  Expired certificate</summary>
        public const int AUTH_EXPIRED_CERT_6 = 6;

        /// <summary>  Invalid signature</summary>
        public const int AUTH_INVALID_SIG_7 = 7;

        /// <summary>  Invalid encryption algorithm (only accepts MD4)</summary>
        public const int AUTH_INVALID_ENCRYPT_ALGO_8 = 8;

        /// <summary>  Missing public key of the receiver (when decrypting message)</summary>
        public const int AUTH_NO_PUBLIC_KEY_9 = 9;

        /// <summary>  Missing private key of the receiver (when decrypting message)</summary>
        public const int AUTH_NO_PRIVATE_KEY_10 = 10;


        /// <summary>  Generic error</summary>
        public const int ACCESS_GENERIC_ERROR_1 = 1;

        /// <summary>  No permission to Register</summary>
        public const int ACCESS_REGISTER_DENIED_2 = 2;

        /// <summary>  No permission to Provide this object</summary>
        public const int ACCESS_PROVIDE_DENIED_3 = 3;

        /// <summary>  No permission to Subscribe to this SIF_Event</summary>
        public const int ACCESS_SUBSCRIBE_DENIED_4 = 4;

        /// <summary>  No permission to Request this object</summary>
        public const int ACCESS_REQUEST_DENIED_5 = 5;

        /// <summary>  No permission to Respond to this object request</summary>
        public const int ACCESS_RESPOND_DENIED_6 = 6;

        /// <summary>  No permission to Report SIF_Events</summary>
        public const int ACCESS_REPORT_DENIED_7 = 7;

        /// <summary>  No permission to administer policies</summary>
        public const int ACCESS_ADMIN_DENIED_8 = 8;

        /// <summary>  SIF_SourceId is not registered</summary>
        public const int ACCESS_UNKNOWN_SOURCEID_9 = 9;

        /// <summary>  No permission to report SIF_Event Add</summary>
        public const int ACCESS_SIFEVENT_ADD_DENIED_10 = 10;

        /// <summary>  No permission to report SIF_Event Change</summary>
        public const int ACCESS_SIFEVENT_CHANGE_DENIED_11 = 11;

        /// <summary>  No permission to report SIF_Event Delete</summary>
        public const int ACCESS_SIFEVENT_DELETE_DENIED_12 = 12;


        /// <summary>  Generic error</summary>
        public const int REG_GENERIC_ERROR_1 = 1;

        /// <summary>  The SIF_SourceId is invalid</summary>
        public const int REG_INVALID_SOURCEID_2 = 2;

        /// <summary>  Requested transport protocol is unsupported</summary>
        public const int REG_UNSUPPORTED_WIRE_PROTO_3 = 3;

        /// <summary>  Requested SIF Version(s) not supported</summary>
        public const int REG_UNSUPPORTED_SIFVERSION_4 = 4;

        /// <summary>  Requested Maximum Packet Size is too small</summary>
        public const int REG_SMALL_MAXPACKETSIZE_6 = 6;

        /// <summary>  ZIS requires an encrypted transport</summary>
        public const int REG_SECURE_TRANSPORT_REQUIRED_7 = 7;

        /// <summary>  Reserved</summary>
        public const int REG_RESERVED_8 = 8;

        /// <summary>  Agent is registered for Push mode</summary>
        public const int REG_PUSH_EXPECTED_9 = 9;


        /// <summary>  Generic error</summary>
        public const int PROVISION_GENERIC_ERROR_1 = 1;

        /// <summary>  Reserved</summary>
        public const int PROVISION_RESERVED_2 = 2;

        /// <summary>  Invalid object</summary>
        public const int PROVISION_INVALID_OBJ_3 = 3;

        /// <summary>  Object already has a provider (SIF_Provide message)</summary>
        public const int PROVISION_ALREADY_HAS_PROVIDER_4 = 4;

        /// <summary>  Not the provider of the object (SIF_Unprovide message)</summary>
        public const int PROVISION_NOT_REGISTERED_PROVIDER_5 = 5;


        /// <summary>  Generic error</summary>
        public const int SUBSCR_GENERIC_ERROR_1 = 1;

        /// <summary>  Reserved</summary>
        public const int SUBSCR_RESERVED_2 = 2;

        /// <summary>  Invalid object</summary>
        public const int SUBSCR_INVALID_OBJ_3 = 3;

        /// <summary>  Not a subscriber of the object (SIF_Unsubscribe message)</summary>
        public const int SUBSCR_NOT_A_SUBSCRIBER_4 = 4;


        /// <summary>  Generic error</summary>
        public const int REQRSP_GENERIC_ERROR_1 = 1;

        /// <summary>  Reserved</summary>
        public const int REQRSP_RESERVED_2 = 2;

        /// <summary>  Invalid object</summary>
        public const int REQRSP_INVALID_OBJ_3 = 3;

        /// <summary>  No Provider</summary>
        public const int REQRSP_NO_PROVIDER_4 = 4;

        /// <summary>  Reserved</summary>
        public const int REQRSP_RESERVED_5 = 5;

        /// <summary>  Reserved</summary>
        public const int REQRSP_RESERVED_6 = 6;

        /// <summary>  Responder does not support requested SIF_Version</summary>
        public const int REQRSP_UNSUPPORTED_SIFVERSION_7 = 7;

        /// <summary>  Responder does not support requested SIF_MaxBufferSize</summary>
        public const int REQRSP_UNSUPPORTED_MAXBUFFERSIZE_8 = 8;

        /// <summary>  Responder does not support the query</summary>
        public const int REQRSP_UNSUPPORTED_QUERY_9 = 9;

        /// <summary>
        /// Invalid SIF_RequestMsgId specified in SIF_Response
        /// </summary>
        public const int REQRSP_INVALID_SIFREQ_MSGID = 10;
        
        /// <summary>
        /// SIF_Response is larger than requested SIF_MaxBufferSize
        /// </summary>
        public const int REQRSP_RESP_LARGER_MAXBUFFERSIZE = 11;
        
        /// <summary>
        /// SIF_PacketNumber is invalid in SIF_Response
        /// </summary>
        public const int REQRSP_PACKETNUMBER_INVALID = 12;
        
        /// <summary>
        /// SIF_Response does not match any SIF_Version from SIF_Request
        /// </summary>
        public const int REQRSP_INVALID_VERSION = 13;
        
        /// <summary>
        /// SIF_DestinationId does not match SIF_SourceId from SIF_Request
        /// </summary>
        public const int REQRSP_DESTINATION_ID_DOES_NOT_MATCH = 14;
        
        /// <summary>
        /// No support for SIF_ExtendedQuery
        /// </summary>
        public const int REQRSP_NO_SUPPORT_FOR_SIF_EXT_QUERY = 15;
        
        /// <summary>
        /// SIF_RequestMsgId deleted from cache due to timeout
        /// </summary>
        public const int REQRSP_REQUEST_DELETED_CACHE_TIMEOUT = 16;
        
        /// <summary>
        /// SIF_RequestMsgId deleted from cache by administrator
        /// </summary>
        public const int REQRSP_REQUEST_DELETED_ADMIN = 17;

        /// <summary>
        /// SIF_Request cancelled by requesting agent
        /// </summary>
        public const int REQRSP_REQUEST_DELETED_AGENT = 18;


        /// <summary>  Generic error</summary>
        public const int EVENT_GENERIC_ERROR_1 = 1;

        /// <summary>  Reserved</summary>
        public const int EVENT_RESERVED_2 = 2;

        /// <summary>  Invalid event</summary>
        public const int EVENT_INVALID_EVENT_3 = 3;


        /// <summary>  Generic error</summary>
        public const int WIRE_GENERIC_ERROR_1 = 1;

        /// <summary>  Requested protocol is not supported</summary>
        public const int WIRE_PROTO_NOT_SUPPORTED_2 = 2;

        /// <summary>  Secure channel requested and no secure path exists</summary>
        public const int WIRE_NO_SECURITY_AVAIL_3 = 3;

        /// <summary>  Unable to establish connection</summary>
        public const int WIRE_NO_CONNECTION_4 = 4;


        /// <summary>  Generic system error</summary>
        public const int SYS_GENERIC_ERROR_1 = 1;


        /// <summary>  Generic Agent Message Handling error</summary>
        /// <deprecated> Use GENERIC_GENERIC_ERROR_1 instead
        /// </deprecated>
        public const int AGENT_GENERIC_ERROR_1 = 1;

        /// <summary>  Generic Message Handling error</summary>
        public const int GENERIC_GENERIC_ERROR_1 = 1;

        /// <summary>  Message not supported</summary>
        /// <deprecated> Use GENERIC_MESSAGE_NOT_SUPPORTED_2 instead
        /// </deprecated>
        public const int AGENT_MESSAGE_NOT_SUPPORTED_2 = 2;

        /// <summary>  Message not supported</summary>
        public const int GENERIC_MESSAGE_NOT_SUPPORTED_2 = 2;

        /// <summary>  Version not supported</summary>
        public const int GENERIC_VERSION_NOT_SUPPORTED_3 = 3;

        /// <summary>  Context not supported</summary>
        public const int GENERIC_CONTEXT_NOT_SUPPORTED_4 = 4;
    }
}
