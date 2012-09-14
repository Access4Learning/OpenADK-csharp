//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;

namespace OpenADK.Library
{
    public class SifElementList : TypedElementList<SifElement>
    {
        internal SifElementList( IList<SifElement> items )
            : base( items ) {}

        internal SifElementList() {}
    }
}
