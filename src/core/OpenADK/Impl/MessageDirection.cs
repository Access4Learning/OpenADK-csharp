//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Impl
{
    /// <summary>
    /// Summary description for MessageDirection.
    /// </summary>
    [Flags]
    public enum MessageDirection
    {
        /// <summary>  Indicates the Incoming message direction</summary>
        Incoming = 0x01,
        /// <summary>  Indicates the Outgoing message direction</summary>
        Outgoing = 0x02,
        /// <summary>  Indicates either Incoming or Outgoing message direction</summary>
        All = Incoming | Outgoing
    }
}
