//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Data;

namespace OpenADK.Library
{
    /// <summary>
    /// A collection of Type converter instances that can be used for converting native types
    /// to the value represented in SIF XML documents
    /// </summary>
    public class SifTypeConverters
    {

        /// <summary>
        /// Gets a type converter for the specified data type
        /// </summary>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public static TypeConverter GetConverter(SifDataType datatype)
        {
            switch (datatype)
            {
                case SifDataType.Boolean:
                    return SifTypeConverters.BOOLEAN;
                case SifDataType.Int:
                    return SifTypeConverters.INT;
                case SifDataType.UInt:
                    return SifTypeConverters.INT;
                case SifDataType.Long:
                    return SifTypeConverters.LONG;
                case SifDataType.String:
                    return SifTypeConverters.STRING;
                case SifDataType.Date:
                    return SifTypeConverters.DATE;
                case SifDataType.DateTime:
                    return SifTypeConverters.DATETIME;
                case SifDataType.Time:
                    return SifTypeConverters.TIME;
                case SifDataType.Decimal:
                    return SifTypeConverters.DECIMAL;
                case SifDataType.Duration:
                    return SifTypeConverters.DURATION;
                case SifDataType.Float:
                    return SifTypeConverters.FLOAT;
                default:
                    throw new NotImplementedException("Support for " + datatype + " is not yet implemented in the Adk.");
            }
        }

        /// <summary>
        /// A type converter that can convert SIF int XML values or .Net int?
        /// values to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<int?> INT = new SifIntConverter();

        /// <summary>
        /// A type converter that can convert SIF int XML values or .Net int?
        /// values to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<long?> LONG = new SifLongConverter();

        /// <summary>
        /// A type converter that can convert SIF float XML values or .Net float?
        /// values to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<float?> FLOAT = new SifFloatConverter();

        /// <summary>
        /// A type converter that can convert SIF bool XML values or .Net bool?
        /// values to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<bool?> BOOLEAN = new SifBooleanConverter();


        /// <summary>
        /// A type converter that can convert SIF decimal XML values or .Net decimal?
        /// values to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<decimal?> DECIMAL = new SifDecimalConverter();

        /// <summary>
        /// A type converter that can convert SIF duration XML values or .Net TimeSpan?
        /// values to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<TimeSpan?> DURATION = new SifDurationConverter();


        /// <summary>
        /// A type converter that can convert SIF date XML values or .Net DateTime?
        /// values representing a date to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<DateTime?> DATE = new SifDateConverter();


        /// <summary>
        /// A type converter that can convert SIF datetime XML values or .Net DateTime?
        /// values representing a date time to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<DateTime?> DATETIME = new SifDateTimeConverter();

        /// <summary>
        /// A type converter that can convert SIF time XML values to .Net DateTime?
        /// values representing a time to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<DateTime?> TIME = new SifTimeConverter();

        /// <summary>
        /// A type converter that can convert SIF String, Token, or normalizedString 
        /// XML values or .Net String values values to a SIFSimpleType
        /// </summary>
        public static SifTypeConverter<String> STRING = new SifStringConverter();


        private sealed class SifIntConverter : SifTypeConverter<int?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse(SifFormatter formatter,
                                                 string xmlValue)
            {
                try
                {
                    return new SifInt( formatter.ToInt( xmlValue ) );
                }
                catch ( FormatException fe )
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to an Integer: " + fe.Message, null, fe );
                }
            }

            /// <summary>
            /// Returns a SifSiimpleType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType( int? nativeType )
            {
                return new SifInt( nativeType );
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="value">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString( SifFormatter formatter,
                                             AdkDataType<int?> value )
            {
                int? i = value.Value;
                if ( i.HasValue ) {
                    return formatter.ToString( i );
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.Int; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.Int32; }
            }

            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.Int32; }
            }
        }

        private sealed class SifLongConverter : SifTypeConverter<long?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse(SifFormatter formatter,
                                                 string xmlValue)
            {
                try
                {
                    return new SifLong( formatter.ToLong( xmlValue ) );
                }
                catch ( FormatException fe )
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to a Long: " + fe.Message, null, fe );
                }
            }

            /// <summary>
            /// Returns a SifSiimpleType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType(long? nativeType)
            {
                return new SifLong(nativeType);
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="value">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString(SifFormatter formatter,
                                             AdkDataType<long?> value)
            {
                long? i = value.Value;
                if (i.HasValue)
                {
                    return formatter.ToString(i);
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.Long; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.Int64; }
            }

            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.Int64; }
            }
        }

        private sealed class SifFloatConverter : SifTypeConverter<float?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse(SifFormatter formatter,
                                                 string xmlValue)
            {
                try
                {
                    return new SifFloat( formatter.ToFloat( xmlValue ) );
                }
                catch ( FormatException fe )
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to a Float: " + fe.Message, null, fe );
                }
            }


            /// <summary>
            /// Returns a SifSiimpleType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType(float? nativeType)
            {
                return new SifFloat(nativeType);
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="value">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString(SifFormatter formatter,
                                             AdkDataType<float?> value)
            {
                float? i = value.Value;
                if (i.HasValue)
                {
                    return formatter.ToString(i);
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.Float; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.Single; }
            }

            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.Single; }
            }
        }

        private sealed class SifDecimalConverter : SifTypeConverter<decimal?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse(SifFormatter formatter,
                                                 string xmlValue)
            {
                try
                {
                    return new SifDecimal( formatter.ToDecimal( xmlValue ) );
                }
                catch ( FormatException fe )
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to a Decimal: " + fe.Message, null, fe );
                }
            }

            /// <summary>
            /// Returns a SifSiimpleType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType( decimal? nativeType )
            {
                return new SifDecimal( nativeType );
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="sifType">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString( SifFormatter formatter,
                                             AdkDataType<decimal?> sifType )
            {
                if ( sifType.Value.HasValue ) {
                    return formatter.ToString( sifType.Value );
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.Decimal; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.Decimal; }
            }

            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.Decimal; }
            }
        }

        private sealed class SifBooleanConverter : SifTypeConverter<bool?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse( SifFormatter formatter,
                                                 string xmlValue )
            {
                try
                {
                    return new SifBoolean( formatter.ToBool( xmlValue ) );
                }
                catch (FormatException fe)
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to a boolean: " + fe.Message, null, fe);
                }
            }

            /// <summary>
            /// Returns a SifSiimpleType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType( bool? nativeType )
            {
                return new SifBoolean( nativeType );
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="sifType">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString( SifFormatter formatter,
                                             AdkDataType<bool?> sifType )
            {
                if ( sifType.Value.HasValue ) {
                    return formatter.ToString( sifType.Value );
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.Boolean; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.Boolean; }
            }

            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.Boolean; }
            }
        }

        private sealed class SifDurationConverter : SifTypeConverter<TimeSpan?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse(SifFormatter formatter,
                                                 string xmlValue)
            {
                try
                {
                    return new SifDuration( formatter.ToTimeSpan( xmlValue ) );
                }
                catch ( FormatException fe )
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to an Integer: " + fe.Message, null, fe );
                }
            }

            /// <summary>
            /// Returns a SifSiimpleType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType( TimeSpan? nativeType )
            {
                return new SifDuration( nativeType );
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="sifType">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString( SifFormatter formatter,
                                             AdkDataType<TimeSpan?> sifType )
            {
                if( sifType.Value.HasValue )
                {
                    return formatter.ToString( sifType.Value );
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.Duration; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.Time; }
            }

            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.Object; }
            }
        }



        private sealed class SifStringConverter : SifTypeConverter<string>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse(SifFormatter formatter,
                                                 string xmlValue)
            {
                try
                {
                    return new SifString( xmlValue );
                }
                catch ( FormatException fe )
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to a String: " + fe.Message, null, fe );
                }
            }

            /// <summary>
            /// Returns a SifSiimpleType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType( string nativeType )
            {
                return new SifString( nativeType );
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="sifType">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString( SifFormatter formatter,
                                             AdkDataType<string> sifType )
            {
                return sifType.Value;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.String; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.String; }
            }

            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.String; }
            }
        }


        private class SifDateTimeConverter : SifTypeConverter<DateTime?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse(SifFormatter formatter,
                                                 string xmlValue)
            {
                try
                {
                    return new SifDateTime( formatter.ToDateTime( xmlValue ) );
                }
                catch ( FormatException fe )
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to an Integer: " + fe.Message, null, fe );
                }
            }

            /// <summary>
            /// Returns a AdkDataType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType( DateTime? nativeType )
            {
                return new SifDateTime( nativeType );
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="sifType">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString( SifFormatter formatter,
                                             AdkDataType<DateTime?> sifType )
            {
                if ( sifType.Value.HasValue ) {
                    return formatter.ToDateTimeString( sifType.Value );
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.DateTime; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.DateTime; }
            }

            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.DateTime; }
            }
        }


        private class SifDateConverter : SifTypeConverter<DateTime?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse( SifFormatter formatter,
                                                 string xmlValue )
            {
                try 
                {
                return new SifDate( formatter.ToDate( xmlValue ) );
                }
                catch (FormatException fe)
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to a DateTime: " + fe.Message, null, fe);
                }
            }

            /// <summary>
            /// Returns a AdkDataType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType( DateTime? nativeType )
            {
                return new SifDate( nativeType );
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="sifType">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString( SifFormatter formatter,
                                             AdkDataType<DateTime?> sifType )
            {
                if ( sifType.Value.HasValue ) {
                    return formatter.ToDateString( sifType.Value );
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.Date; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.Date; }
            }


            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.DateTime; }
            }
        }

        private class SifTimeConverter : SifTypeConverter<DateTime?>
        {
            /// <summary>
            /// Parses the given XML string value into a SIFSimpleType instance
            /// </summary>
            /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
            /// <param name="xmlValue">The XML string value</param>
            /// <returns>A SIFSimpleType instance</returns>
            /// <exception cref="AdkTypeParseException">thrown if the value cannot be parsed</exception>
            public override SifSimpleType Parse(SifFormatter formatter,
                                                 string xmlValue)
            {
                try
                {
                    return new SifTime( formatter.ToTime( xmlValue ) );
                }
                catch ( FormatException fe )
                {
                    throw new AdkTypeParseException(
                        "Error converting value '" + xmlValue + "' to a Time object: " + fe.Message, null, fe );
                }
            }

            /// <summary>
            /// Returns a AdkDataType instance that wraps the specified native value
            /// </summary>
            /// <param name="nativeType">An instance of the native type object, such as
            /// a String or DateTime</param>
            /// <returns>The approprirate AdkDataType instantiation for this instance</returns>
            public override SifSimpleType GetSifSimpleType( DateTime? nativeType )
            {
                return new SifTime( nativeType );
            }

            /// <summary>
            /// Converts the given SIFSimpleType instance to an XML string value
            /// </summary>
            /// <param name="formatter">The formatter to sue for the current version of SIF being written to</param>
            /// <param name="sifType">The value to write</param>
            /// <returns>A string representing the XML payload used for the specified version
            /// of SIF. All types are nullable in SIF, so the resulting value could be null/</returns>
            public override String ToString( SifFormatter formatter,
                                             AdkDataType<DateTime?> sifType )
            {
                if ( sifType.Value.HasValue ) {
                    return formatter.ToTimeString( sifType.Value );
                }
                return null;
            }


            /// <summary>
            /// A value from the SifDataType enum
            /// </summary>
            public override SifDataType DataType
            {
                get { return SifDataType.Time; }
            }

            /// <summary>
            /// The System.Data.DbType value for this data type
            /// </summary>
            public override DbType DbType
            {
                get { return DbType.Time; }
            }


            /// <summary>
            /// The native .Net TypeCode for this data type
            /// </summary>
            public override TypeCode TypeCode
            {
                get { return TypeCode.DateTime; }
            }
        }

    }
}
