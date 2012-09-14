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
    public class SifLong : AdkDataType<long?>
    {
        public SifLong(long? value)
            : base(value) { }


        protected override SifTypeConverter<long?> GetTypeConverter()
        {
            return SifTypeConverters.LONG;
        }
    }
}
