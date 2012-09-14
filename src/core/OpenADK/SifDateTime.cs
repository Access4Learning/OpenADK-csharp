//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
   [Serializable]
    public class SifDateTime : AdkDataType<DateTime?>
    {
        public SifDateTime( DateTime? value )
            : base( value ) {}


        protected override SifTypeConverter<DateTime?> GetTypeConverter()
        {
            return SifTypeConverters.DATETIME;
        }
    }
}
