//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
   [Serializable]
    public class SifInt : AdkDataType<int?>
    {
        public SifInt( int? value )
            : base( value ) {}


        protected override SifTypeConverter<int?> GetTypeConverter()
        {
            return SifTypeConverters.INT;
        }
    }
}
