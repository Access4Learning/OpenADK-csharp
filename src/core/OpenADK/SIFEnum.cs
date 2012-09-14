//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  Describes the acceptable values for an enumerated SIF attribute or element
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    [Serializable]
    public class SifEnum : SifString
    {
        public SifEnum( String value )
            : base( value ) {}
    }
}
