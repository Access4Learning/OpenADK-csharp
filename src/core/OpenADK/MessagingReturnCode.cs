//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Summary description for MessagingReturnCode.
    /// </summary>
    public enum MessagingReturnCode
    {
        /// <summary>indicates the message should be discarded</summary>
        Discard = 0,
        /// <summary>indicates the message should be processed</summary>
        Process = 1,
        /// <summary>this method has changed the content of the <c>message</c> parameter.
        /// The message should be reparsed and processed</summary>
        Reparse = 2
    }
}
