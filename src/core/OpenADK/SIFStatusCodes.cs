//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>	Defines SIF 1.0r1 status code constants.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class SifStatusCodes
    {
        /// <summary>  Success. SIF_Status / SIF_Data may contain additional data (ZIS only) ("0")</summary>
        public const int SUCCESS_0 = 0;

        /// <summary>  Receiver is sleeping ("8")</summary>
        public const int SLEEPING_8 = 8;

        /// <summary>  Already registered using this protocol ("4")</summary>
        public const int ALREADY_REGISTERED_4 = 4;

        /// <summary>  Final SIF_Ack. Processing of a previously message acknowledged with INTERMEDIATE_ACK is now complete. Discard the referenced message (Agent only) ("3")</summary>
        public const int FINAL_ACK_3 = 3;

        /// <summary>  Already registered as a provider of this object ("6")</summary>
        public const int ALREADY_PROVIDER_6 = 6;

        /// <summary>  Immediate SIF_Ack. Message is persisted or processing is complete. Discard the referenced message (Agent only) ("1")</summary>
        public const int IMMEDIATE_ACK_1 = 1;

        /// <summary>  Already subscribed to this object ("5")</summary>
        public const int ALREADY_SUBSCRIBED_5 = 5;

        /// <summary>  Already have a message with this MsgId from sender ("7")</summary>
        public const int DUPLICATE_MESSAGE_7 = 7;

        /// <summary>  No messages available. This is returned when an agent is trying to pull messages from a ZIS and there are no messages available ("9")</summary>
        public const int NO_MESSAGES_9 = 9;

        /// <summary>  Intermediate SIF_Ack. Message processing will take time. The message referenced must still be persisted. Expect a FINAL_ACK at a later time (Agent only) ("2")</summary>
        public const int INTERMEDIATE_ACK_2 = 2;
    }
}
