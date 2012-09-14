//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Summary description for SifParserFlags.
    /// </summary>
    [Flags]
    public enum SifParserFlags
    {

        None = 0,
        /// <summary>
        /// Flag that indicates that SIFParser should expect a nested SIF_Message 
        /// </summary>
        ExpectInnerEnvelope = 1
    }
}
