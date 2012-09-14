//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// A list of SIF datatypes supported by the ADK
    /// </summary>
    public enum SifDataType
    {
        /// <summary>
        /// Represents the SIF <code>boolean</code> datatype
        /// </summary>
        Boolean,
        /// <summary>
        /// Represents the SIF <code>int</code> datatype
        /// </summary>
        Int,
        /// <summary>
        /// Represents the SIF <code>uint</code> datatype
        /// </summary>
        UInt,
        /// <summary>
        /// Represents the SIF <code>long</code> datatype
        /// </summary>
        Long,
        /// <summary>
        /// Represents the SIF <code>float</code> datatype
        /// </summary>
        Float,
        /// <summary>
        /// Represents the SIF <code>string</code>, <code>token</code>, and <code>normalizedString</code> datatypes
        /// </summary>
        String,
        /// <summary>
        /// Represents the SIF <code>date</code> datatype
        /// </summary>
        Date,
        /// <summary>
        /// Represents the SIF <code>datetime</code> datatype
        /// </summary>
        DateTime,
        /// <summary>
        /// Represents the SIF <code>time</code> datatype
        /// </summary>
        Time,
        /// <summary>
        /// Represents the SIF <code>decimal</code> datatype
        /// </summary>
        Decimal,
        /// <summary>
        /// Represents the SIF <code>duration</code> datatype
        /// </summary>
        Duration
    }
}
