//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Data;

namespace OpenADK.Library.Tools.Mapping
{
    public abstract class DataMapper : IFieldAdaptor
    {
        private DataColumnCollection fColumns;

        /// <summary>
        /// Creates an instance of DataMapper, using the specified columns
        /// </summary>
        /// <param name="columns"></param>
        public DataMapper( DataColumnCollection columns )
        {
            fColumns = columns;   
        }

        /// <summary>
        /// Returns whether the field being requested for mapping has a value that can
        /// be mapped to SIF
        /// </summary>
        /// <param name="fieldName">The field name being mapped to SIF</param>
        /// <returns><c>True</c> if there is a value for the specified field that
        /// should be mapped to SIF</returns>
        public bool HasField(string fieldName)
        {
            return fColumns.IndexOf(fieldName) > -1;
        }

        /// <summary>
        /// Sets a value that has been retrieved from a SIF Element in an inbound field
        /// mapping operation.
        /// </summary>
        /// <param name="fieldName">The field name that is mapped to a SIFElement</param>
        /// <param name="value">The value of the SIF element</param>
        /// <param name="mapping">The FieldMappings that will be used to set this value or null</param>
        public abstract void SetSifValue(string fieldName, SifSimpleType value, FieldMapping mapping);

        /// <summary>
        ///  Gets a value from the underlying data store to be used in an outbound
        /// field mapping operation
        /// </summary>
        /// <param name="fieldName"> The field name that is mapped to a SIFElement</param>
        /// <param name="typeConverter">The converter class for the requested SIF data type</param>
        /// <param name="mapping">The FieldMapping this value was generated from or null</param>
        /// <returns>The value to set to the SIF element. This value must contain the
        /// SIFSimpleType subclass represented by the SIFTypeConverter passed in to the 
        /// method.</returns>
        public abstract SifSimpleType GetSifValue(string fieldName, TypeConverter typeConverter, FieldMapping mapping);

        /// <summary>
        /// Returns the underlying value being stored for the field.
        /// </summary>
        /// <remarks>
        ///  This method is not called during Mappings operations. It may be called by
        /// classes such as the <see cref="OpenADK.Library.IValueBuilder"/> class
        /// </remarks>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract object GetValue(string key);

        
        /// <summary>
        /// Returns the ordinal index of the specified column, or -1 if it doesn't exist
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected int SafeGetOrdinal( string fieldName )
        {
            return fColumns.IndexOf( fieldName );
        }


    }
}
