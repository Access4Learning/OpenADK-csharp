//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;

namespace OpenADK.Library
{
    /// <summary>  A SIF Time value.
    /// 
    /// SifTime extends the <c>OpenADK.Library.us.Common.Time</c> class 
    /// that encapsulates SIF Time elements in SIF Data Objects. An instance of this 
    /// class can be used whenever a parameter of that type is passed to a SIF Data 
    /// Object class. SifTime adds methods to set and get SIF Time values using the
    /// standard .Net DateTime class
    /// 
    /// </summary>
    /// <version>  Adk 1.0
    /// </version>
   [Serializable]
    public class SifTime : AdkDataType<DateTime?>
    {
        public SifTime( DateTime? timeValue )
            : base( timeValue ) {}


        protected override SifTypeConverter<DateTime?> GetTypeConverter()
        {
            return SifTypeConverters.TIME;
        }
       
    }
}
