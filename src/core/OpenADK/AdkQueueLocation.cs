//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  Flags and constants used throughout the Adk.
    /// 
    /// <b>Provisioning Flags</b>
    /// 
    /// The PROV_ flags are used to control the provisioning process when the agent
    /// registers with zones and topics. These flags are typically passed to methods
    /// such as <c>Zone.connect</c>, <c>Topic.setSubscriber</c>, <c>Topic.setPublisher</c>,
    /// and <c>Agent.shutdown</c>.
    /// 
    /// 
    /// SIF provisioning messages include:
    /// 
    /// <ul>
    /// <li><c>&lt;SIF_Register&gt;</c></li>
    /// <li><c>&lt;SIF_Unregister&gt;</c></li>
    /// <li><c>&lt;SIF_Provide&gt;</c></li>
    /// <li><c>&lt;SIF_Unsubscribe&gt;</c></li>
    /// <li><c>&lt;SIF_Subscribe&gt;</c></li>
    /// <li><c>&lt;SIF_Unsubscribe&gt;</c></li>
    /// </ul>
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Flags]
    public enum AdkQueueLocation
    {
        /// <summary>  Identifies the Agent Local Queue (as opposed to the agent's queue on the ZIS)</summary>
        QUEUE_LOCAL = 0x00000001,

        /// <summary>  Identifies the agent's queue on the ZIS (as opposed to the Agent Local Queue)</summary>
        QUEUE_SERVER = 0x00000002,
    }
}
