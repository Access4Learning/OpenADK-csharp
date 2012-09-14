//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Summary description for EventAction.
    /// </summary>
    public enum EventAction
    {
        /// <summary>  Action code is undefined</summary>
        Undefined = 0,

        /// <summary>  Action code indicating the data was <i>added</i> by the publisher</summary>
        Add = 1,

        /// <summary>  Action code indicating the data was <i>changed</i> by the publisher</summary>
        Change = 2,

        /// <summary>  Action code indicating the data was <i>deleted</i> by the publisher</summary>
        Delete = 3
    }
}
