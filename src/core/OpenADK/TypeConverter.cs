//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Data;

namespace OpenADK.Library
{
    /// <summary>
    /// An 
    /// </summary>
    public interface TypeConverter
    {
        /// <summary>
        /// Parses the given XML string value into a SIFSimpleType instance
        /// </summary>
        /// <param name="formatter">the formatter to use for the specific version of SIF being parsed</param>
        /// <param name="xmlValue">The XML string value</param>
        /// <returns>A SIFSimpleType instance</returns>
        /// <exception cref="AdkParsingException">thrown if the value cannot be parsed</exception>
        SifSimpleType Parse( SifFormatter formatter,
                             string xmlValue );

        /// <summary>
        /// Converts the native datatype to the datatype used by the ADK. e.g. converts an 'int' to a 'SifInt'
        /// </summary>
        /// <param name="nativeType"></param>
        /// <returns></returns>
        SifSimpleType GetSifSimpleType( object nativeType );

        /// <summary>
        /// Parses the XML string value and returns the proper SimpleField instance to hold
        /// the element value
        /// </summary>
        /// <param name="parent">The parent SIFElement of this field</param>
        /// <param name="id">The metadata definition of the field</param>
        /// <param name="formatter">The formatter to use for the specific version of SIF being parsed</param>
        /// <param name="xmlValue">A string representing the XML payload being used for this version of SIF</param>
        /// <returns>A simple field initialized with the proper value</returns>
        /// <exception cref="AdkParsingException">thrown if the value cannot be parsed according to the
        /// formatting rules for this version of SIF</exception>
        SimpleField ParseField(
            SifElement parent,
            IElementDef id,
            SifFormatter formatter,
            string xmlValue );


        /// <summary>
        /// The SIF data type of this field from the SifDataType enum
        /// </summary>
        /// <returns></returns>
        SifDataType DataType { get; }

        /// <summary>
        /// The data type of this field from the DbType enum
        /// </summary>
        DbType DbType { get; }

        /// <summary>
        /// The native .Net TypeCode for this data type
        /// </summary>
        TypeCode TypeCode { get; }
    }
}
