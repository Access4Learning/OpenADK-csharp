//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
   [Serializable]
    public class SifString : AdkDataType<string>
    {
        public SifString( string value )
            : base( value ) {}


        protected override SifTypeConverter<string> GetTypeConverter()
        {
            return SifTypeConverters.STRING;
        }

        /// <summary>
        /// This type does require encoding and returns false
        /// </summary>
        public override bool DoNotEncode
        {
            get { return false; }
        }


        /// <summary>
        /// Returns a string representation of the value, formatted for the specified version of SIF
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public override String ToString(SifVersion version)
        {
            return fValue;
        }

        /// <summary>
        /// Returns the text representation of this value in 1.x format by default.
        /// </summary>
        /// <remarks>
        /// To change the default formatter used for rendering text values, call
        /// <see cref="Adk.TextFormatter"/>
        /// </remarks>
        /// <returns></returns>
        public override String ToString()
        {
            return fValue;
        }


        /// <summary>
        /// Returns the string representation of this data field, using the specified formatter
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public override String ToString(SifFormatter formatter)
        {
            return fValue;
        }

        /// <summary>
        /// Evaluates the string value to determine if it
        /// equals the string value of this object
        /// </summary>
        /// <param name="value">the value to compare</param>
        /// <returns>true if the specified string equals the 
        /// value of this object</returns>
        public bool ValueEquals(String value)
        {
            if (value == null)
            {
                return fValue == null;
            }
            else
            {
                return value.Equals(fValue);
            }
        }


    }
}
