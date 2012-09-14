//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    [Serializable]
    public class SifFloat : AdkDataType<float?>
    {
        public SifFloat(float? value)
            : base(value) { }


        protected override SifTypeConverter<float?> GetTypeConverter()
        {
            return SifTypeConverters.FLOAT;
        }
    }
}
