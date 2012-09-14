//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Refer to the Developer Guide for more information on the ADK's
    /// provisioning modes.
    /// </summary>
    public enum AgentProvisioningMode
    {
        /// <summary>  The ADK uses the ZIS-Managed Provisioning mode for this zone. ADK
        /// methods such as <code>Agent.connect</code> and <code>Topic.subscribe</code>
        /// that would normally cause the ADK to send a provisioning message will
        /// not send the message when this mode is enabled. Provisioning messages
        /// include: SIF_Register and SIF_Unregister, SIF_Subscribe and SIF_Unsubscribe,
        /// and SIF_Provide and SIF_Unprovide.
        /// 
        /// Refer to the Developer Guide for more information on the ADK's
        /// provisioning modes.
        /// </summary>
        Zis = 1,

        /// <summary>  The ADK uses the ADK-Managed Provisioning mode for this zone. When
        /// enabled, the ADK sends provisioning messages at the appropriate times
        /// when methods such as <code>Agent.connect</code> and <code>Topic.subscribe</code>
        /// are called. 
        /// </summary>
        Adk = 2,

        /// <summary>  The ADK uses the Agent-Managed Provisioning mode for this zone. When
        /// enabled, the ADK does not send any provisioning messages. Agents must
        /// explicitly call the following methods of the Zone class to perform all
        /// provisioning tasks:
        /// 
        /// <ul>
        /// <li><code>Zone.SifRegister</code> and <code>Zone.SifUnregister</code></li>
        /// <li><code>Zone.SifSubscribe</code> and <code>Zone.SifUnsubscribe</code></li>
        /// <li><code>Zone.SifProvide</code> and <code>Zone.SifUnprovide</code></li>
        /// </ul>
        /// </summary>
        Agent = 3
    }
}
