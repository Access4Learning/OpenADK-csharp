//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
   [Serializable]
    public class SifDecimal : AdkDataType<decimal?>
    {
        public SifDecimal( decimal? value )
            : base( value ) {}


        protected override SifTypeConverter<decimal?> GetTypeConverter()
        {
            return SifTypeConverters.DECIMAL;
        }
    }
}
