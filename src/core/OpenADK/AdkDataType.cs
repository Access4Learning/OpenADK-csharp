//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Data;

namespace OpenADK.Library
{
    /// <summary>
    /// Wraps a .Net value type in an immutable instance that can be used
    /// to construct SimpleFields that represent properties in SIF data objects
    /// </summary>
    /// <typeparam name="T">The datatype that this SIFSimpleType wraps, such as
    /// Integer, Calendar, String, etc.</typeparam>
    [Serializable]
    public abstract class AdkDataType<T> : SifSimpleType
    {
        /// <summary>
        /// The .Net value type that this SIFSimpleType wraps. This value
        /// must only be set from a constructor in order to keep the type
        /// immutable
        /// </summary>
        protected  readonly T fValue;


        /// <summary>
        /// Creates a SIFSimpleType with the appropriate value
        /// </summary>
        /// <param name="value"></param>
        protected AdkDataType( T value )
        {
            fValue = value;
        }

        /// <summary>
        /// Returns the .Net native type for this SIF data type
        /// </summary>
        public override object RawValue
        {
            get { return fValue; }
        }


        /// <summary>
        /// Returns the underling value that this SIFSimpleType wraps
        /// </summary>
        public T Value
        {
            get { return fValue; }
        }


        /// <summary>
        /// Returns the string representation of this data field, using the specified formatter
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public override String ToString( SifFormatter formatter )
        {
            return GetTypeConverter().ToString( formatter, this );
        }

        /// <summary>
        /// Returns the TypeConverter that can be used to convert SIF XML values to 
        /// native values
        /// </summary>
        public override TypeConverter TypeConverter
        {
            get { return GetTypeConverter(); }
        }


        /// <summary>
        /// Allows inheritors to return the type converter to use for this type
        /// </summary>
        /// <returns></returns>
        protected abstract SifTypeConverter<T> GetTypeConverter();


        /// <summary>
        /// The SIF data type of this field from the SifDataType enum
        /// </summary>
        /// <returns></returns>
        public override SifDataType DataType
        {
            get { return TypeConverter.DataType; }
        }

        /// <summary>
        /// The data type of this field from the DbType enum
        /// </summary>
        public override DbType DbType
        {
            get { return TypeConverter.DbType; }
        }

        /// <summary>
        /// Should the XML writer encode the value that this datatype produces?
        /// </summary>
        public override bool DoNotEncode
        {
            get { return true; }
        }



        /// <summary>
        /// Returns a key suitable for use in a HashTable
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if ( fValue != null ) {
                return fValue.GetHashCode();
            }
            else {
                return -1;
            }
        }


    }
}
