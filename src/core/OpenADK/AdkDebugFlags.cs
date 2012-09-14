//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Summary description for AdkDebugLevel.
    /// </summary>
    [Flags]
    public enum AdkDebugFlags
    {
        /// <summary>  Adk debugging flag indicates whether Transport setup is traced</summary>
        Transport = 0x00000004,

        /// <summary>  Adk debugging flag indicates whether Message Dispatching actions are traced</summary>
        Messaging = 0x00000008,

        /// <summary>  Adk debugging flag indicates whether SIF_Event Message Dispatching actions are traced</summary>
        Messaging_Event_Dispatching = 0x00000010,

        /// <summary>  Adk debugging flag indicates whether SIF_Response processing is traced</summary>
        Messaging_Response_Processing = 0x00000020,

        /// <summary>  Adk debugging flag indicates whether SIF_SystemControl/SIF_GetMessage messages are traced</summary>
        Messaging_Pull = 0x00000040,

        /// <summary>  Adk debugging flag indicates whether message headers are traced</summary>
        Messaging_Detailed = 0x00000080,

        /// <summary>  Adk debugging flag indicates whether message content is traced</summary>
        Message_Content = 0x00000100,

        /// <summary>  Adk debugging flag indicates whether provisioning activity is traced.
        /// Note other debugging flags may cause provisioning messages to be logged
        /// even when this flag is not set.
        /// </summary>
        Provisioning = 0x00000200,

        /// <summary>  Adk debugging flag indicates whether Agent Runtime activity is traced.</summary>
        Runtime = 0x00001000,
        
        /// <summary>
        /// ADK debugging flag indicates whether ADK policy is traced.
        /// </summary>
        Policy = 0x00000400,

        /// <summary>  Adk debugging flag indicates whether agent startup/shutdown and
        /// initializaion activity is logged
        /// </summary>
        Lifecycle = 0x10000000,

        /// <summary>  Adk debugging flag indicates whether exceptions are logged</summary>
        Exceptions = 0x20000000,

        /// <summary>  Adk debugging flag indicates whether agent and zone properties are logged</summary>
        Properties = 0x40000000,

        /// <summary>  Adk debugging flag to enable all debugging output</summary>
        All = -1, //0xFFFFFFFF,

        /// <summary>  Adk debugging flag to disable debugging output</summary>
        None = 0x0,


        /// <summary>  Minimal debugging flags (exceptions, provisioning)</summary>
        Minimal = Exceptions | Provisioning,


        /// <summary>  Moderate debugging flags (exceptions, provisioning, messaging, lifecycle)</summary>
        Moderate = Minimal | Messaging | Policy | Lifecycle,

        /// <summary>  Moderate debugging flags, with MESSAGING_PULL</summary>
        Moderate_With_Pull = Moderate | Messaging_Pull,


        /// <summary>  Detailed debugging flags (exceptions, provisioning, messaging, detailed messaging, transport)</summary>
        Detailed = Moderate_With_Pull | Transport | Messaging_Detailed,

        /// <summary>  Very detailed debugging flags (exceptions, provisioning, messaging, detailed messaging, transport, event dispatching, properties)</summary>
        Very_Detailed =
            Detailed | Messaging_Event_Dispatching | Messaging_Response_Processing | Properties,
    }
}
