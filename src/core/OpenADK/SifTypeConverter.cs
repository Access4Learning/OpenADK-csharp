//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Data;

namespace OpenADK.Library
{
    /// <summary>
    /// Represents a set of type converters that use SIFFormatter instances to convert
    /// Strings into SIFSimpleType instances
    /// </summary>
    /// <typeparam name="T">The datatype that a particular type converter is capable of converting,
    /// such as bool? DateTime?, etc.</typeparam>
    public abstract class SifTypeConverter<T> : TypeConverter
    {
        SifSimpleType TypeConverter.Parse( SifFormatter formatter,
                                           string xmlValue )
        {
            return Parse( formatter, xmlValue );
        }

        /// <summary>
        /// Converts the native datatype to the datatype used by the ADK. e.g. converts an 'int' to a 'SifInt'
        /// </summary>
        /// <param name="nativeType"></param>
        /// <returns>A SifsimpleType of the appropriate value</returns>
        /// <exception cref="InvalidCastException">This conversion is not supported</exception>
        public virtual SifSimpleType GetSifSimpleType( object nativeType )
        {
            if ( nativeType == null ) {
                return GetSifSimpleType( default(T) );
            }
            if ( !(nativeType is T) ) {
                // Try converting the object. This will throw a ClassCastExcption if it does not succeed
                nativeType = Convert.ChangeType( nativeType, this.TypeCode );
            }
            return GetSifSimpleType( (T) nativeType );
        }

        /// <summary>
        /// Parses the given XML string value into a SIFSimpleType instance
        /// </summary>
        /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
        /// <param name="xmlValue">The XML string value</param>
        /// <returns>A SIFSimpleType instance</returns>
        /// <exception cref="AdkParsingException">thrown if the value cannot be parsed</exception>
        public abstract SifSimpleType Parse( SifFormatter formatter,
                                             string xmlValue );

        /// <summary>
        /// Returns a SifSiimpleType instance that wraps the specified native value
        /// </summary>
        /// <param name="nativeType">An instance of the native type object, such as
        /// a String or DateTime</param>
        /// <returns>The appropriate AdkDataType instantiation for this instance</returns>
        public abstract SifSimpleType GetSifSimpleType( T nativeType );

        /// <summary>
        /// Converts the given SIFSimpleType instance to an XML string value
        /// </summary>
        /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
        /// <param name="value">The value to write</param>
        /// <returns>A string representing the XML payload used for the specified version
        /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
        public abstract String ToString( SifFormatter formatter,
                                         AdkDataType<T> value );

        /// <summary>
        /// A value from the SifDataType enum
        /// </summary>
        public abstract SifDataType DataType { get; }

        /// <summary>
        /// The System.Data.DbType value for this data type
        /// </summary>
        public abstract DbType DbType { get; }

        /// <summary>
        /// The native .Net TypeCode for this data type
        /// </summary>
        public abstract TypeCode TypeCode { get; }

        /// <summary>
        /// Parses the XML string value and returns the proper SimpleField instance to hold
        /// the element value
        /// </summary>
        /// <param name="parent">The parent SIFElement of this field</param>
        /// <param name="id">The metadata definition of the field</param>
        /// <param name="formatter">The formatter to use for the specific version of SIF being parsed</param>
        /// <param name="xmlValue">A string representing the XML payload being used for this version of SIF</param>
        /// <returns>A simple field initialized with the proper value</returns>
        /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed according to the
        /// formatting rules for this version of SIF</exception>
        public SimpleField ParseField(
            SifElement parent,
            IElementDef id,
            SifFormatter formatter,
            string xmlValue )
        {
            return Parse( formatter, xmlValue ).CreateField( parent, id );
        }
    }
}
