//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Summary description for GroupOperator.
    /// </summary>
    [Flags]
    public enum GroupOperator
    {
        /// <summary>Signifies no operator </summary>
        None = 0x00,
        /// <summary>Logical OR </summary>
        Or = 0x10,
        /// <summary>Logical AND </summary>
        And = 0x20
    }
}
