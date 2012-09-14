//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library
{
   [Serializable]
    public class SifDuration : AdkDataType<TimeSpan?>
    {
        public SifDuration( TimeSpan? value )
            : base( value ) { }


        protected override SifTypeConverter<TimeSpan?> GetTypeConverter()
        {
            return SifTypeConverters.DURATION;
        }
    }
}
