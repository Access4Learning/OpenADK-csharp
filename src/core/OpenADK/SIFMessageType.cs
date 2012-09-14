//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Summary description for SifMessageType.
    /// </summary>
    public enum SifMessageType
    {
        /// <summary>  Indicates message type is not important</summary>
        Any = -1,

        /// <summary>
        /// Indicates that the specified value is not a SifMessageType
        /// </summary>
        None = 0,

        /// <summary>Message Type identifying a SIF_Ack</summary>
        SIF_Ack = 1,
        /// <summary>MessageType constant identifying a SIF_Event</summary>
        SIF_Event = 2,
        /// <summary>MessageType constant identifying a SIF_Request</summary>
        SIF_Request = 5,
        /// <summary>MessageType constant identifying a SIF_Response</summary>
        SIF_Response = 6,
        /// <summary>MessageType constant identifying a SIF_SystemControl</summary>
        SIF_SystemControl = 8,
        /// <summary>MessageType constant identifying a SIF_Register</summary>
        SIF_Register = 4,
        /// <summary>MessageType constant identifying a SIF_Unregister</summary>
        SIF_Unregister = 10,
        /// <summary>MessageType constant identifying a SIF_Subscribe</summary>
        SIF_Subscribe = 7,
        /// <summary>MessageType constant identifying a SIF_Unsubscribe</summary>
        SIF_Unsubscribe = 11,
        /// <summary>MessageType constant identifying a SIF_Provide</summary>
        SIF_Provide = 3,
        /// <summary>MessageType constant identifying a SIF_Unprovide</summary>
        SIF_Unprovide = 9,
        /// <summary>MessageType constant identifying a SIF_ZoneStatus message</summary>
        SIF_ZoneStatus = 12,
        /// <summary>MessageType constant identifying a SIF_Provision message</summary>
        SIF_Provision = 13
    }
}
