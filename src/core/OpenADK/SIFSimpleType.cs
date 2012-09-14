//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Data;

namespace OpenADK.Library
{
    /// <summary>
    /// The abstract and base definition for a specific data type supported by SIF. DataTypes are
    /// immutable and can be converted to and from datatypes supported by the SIF Specication in any
    /// version of SIF
    /// </summary>
    [Serializable]
    public abstract class SifSimpleType
    {
        /// <summary>
        /// Create a SimpleField from the value that this type represents
        /// </summary>
        /// <param name="parent">The SIFElement that will be the parent of the field</param>
        /// <param name="id">The metatdata definition of the field being set</param>
        /// <returns>A SimpleField instance initialized with the proper value</returns>
        public virtual SimpleField CreateField( SifElement parent,
                                                IElementDef id )
        {
            return new SimpleField( parent, id, this );
        }


        /// <summary>
        /// Returns the string representation of this data field, using the specified formatter
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public abstract String ToString( SifFormatter formatter );

        /// <summary>
        /// Gets the native .Net value of this SIF data type
        /// </summary>
        public abstract object RawValue { get; }

        /// <summary>
        /// Gets the TypeConverter used for this SIF data type
        /// </summary>
        public abstract TypeConverter TypeConverter { get; }

        /// <summary>
        /// Returns whether or not this data type may need XML encoding applied to it
        /// </summary>
        public abstract bool DoNotEncode { get; }

        /// <summary>
        /// Returns a string representation of the value, formatted for the specified version of SIF
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public virtual String ToString( SifVersion version )
        {
            return ToString( Adk.Dtd.GetFormatter( version ) );
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
            return ToString( Adk.TextFormatter );
        }

        /// <summary>
        /// The SIF data type of this field from the SifDataType enum
        /// </summary>
        /// <returns></returns>
        public abstract SifDataType DataType{ get;}
        

        /// <summary>
        /// The data type of this field from the DbType enum
        /// </summary>
        public abstract DbType DbType{ get;}


        /// <summary>
        /// Overriden to test value equality. The underlying <see cref="SifEnum.Value"/> property is compared to 
        /// determine if it is equal.
        /// </summary>
        /// <param name="obj">The SifEnum to compare against</param>
        /// <returns>True if the objects have the same value, otherwise False</returns>
        public override bool Equals(object obj)
        {
            // Test reference comparison first ( fastest )
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (ReferenceEquals(obj, null))
            {
                return false;
            }

            SifSimpleType comparedEnum = obj as SifSimpleType;
            if (ReferenceEquals(comparedEnum, null))
            {
                return false;
            }

            if (this.RawValue == null)
            {
                return comparedEnum.RawValue == null;
            }
            return this.RawValue.Equals(comparedEnum.RawValue);
        }

        /// <summary>
        /// Overriden to test value equality. The underlying <see cref="SifEnum.Value"/> property is compared to 
        /// determine if it is equal.
        /// </summary>
        /// <param name="obj1">The first SifEnum to compare</param>
        /// <param name="obj2">The second SifEnum to compare</param>
        /// <returns>True if the objects have the same value, otherwise False</returns>
        public static bool operator ==(SifSimpleType obj1,
                                        SifSimpleType obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (!ReferenceEquals(obj1, null))
            {
                return obj1.Equals(obj2);
            }
            return false;
        }

        /// <summary>
        /// Overriden to test value equality. The underlying <see cref="SifEnum.Value"/> property is compared to 
        /// determine if it is not equal.
        /// </summary>
        /// <param name="obj1">The first SifEnum to compare</param>
        /// <param name="obj2">The second SifEnum to compare</param>
        /// <returns>False if the objects have the same value, otherwise True</returns>
        public static bool operator !=(SifSimpleType obj1,
                                        SifSimpleType obj2)
        {
            return !(obj1 == obj2);
        }


        public abstract override int GetHashCode();
    }
}
