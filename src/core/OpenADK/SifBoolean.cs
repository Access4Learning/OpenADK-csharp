//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
   [Serializable]
    public class SifBoolean : AdkDataType<bool?>
    {
        public SifBoolean( bool? value )
            : base( value ) {}


        protected override SifTypeConverter<bool?> GetTypeConverter()
        {
            return SifTypeConverters.BOOLEAN;
        }
    }
}
