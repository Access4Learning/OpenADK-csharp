//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    [Flags]
    public enum ProvisioningFlags
    {
        /// <summary>  Provisioning Option: No provisioning action should be taken</summary>
        None = 0x00000000,
        /// <summary>  Provisioning Option: Send a SIF_Register message</summary>
        Register = 0x00000001,
        /// <summary>  Provisioning Option: Send a SIF_Unregister message</summary>
        Unregister = 0x00000002,
        /// <summary>  Provisioning Option: Send a SIF_Provide message</summary>
        Provide = 0x00000004,
        /// <summary>  Provisioning Option: Send a SIF_Unprovide message</summary>
        Unprovide = 0x00000008,
        /// <summary>  Provisioning Option: Send a SIF_Subscribe message</summary>
        Subscribe = 0x00000010,
        /// <summary>  Provisioning Option: Send a SIF_Unsubscribe message</summary>
        Unsubscribe = 0x00000020,
        /// <summary>  Instruct the Adk to put the agent to sleep upon successful connection
        /// to the zone. The agent is responsible for waking up the agent when it
        /// is ready to begin receiving messages. This flags should be passed to
        /// the <c>Zone.connect</c> method to prevent the agent from receiving
        /// messages upon a successful connection to the zone.
        /// </summary>
        SleepOnConnect = 0x10000000
    }
}
