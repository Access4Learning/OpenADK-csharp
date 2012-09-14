//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Data;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>
    /// Implements IFieldAdaptor around a DataRow instance
    /// </summary>
    /// <remarks>
    /// To use this class, create an instance of it, based around a DataRow instance. If you
    ///  are navigating a DataTable, you are responsible for setting the <see cref="SourceRow"/> property
    /// as you are navigating through the table for either in inbound or outbound mapping
    /// </remarks>
    public class DataRowAdaptor : DataMapper
    {
        private DataRow fDataRow;


        public DataRowAdaptor(DataRow row) : base(row.Table.Columns)
        {
            fDataRow = row;   
        }


        /// <summary>
        /// Sets a value that has been retrieved from a SIF Element in an inbound field
        /// mapping operation.
        /// </summary>
        /// <param name="fieldName">The field name that is mapped to a SIFElement</param>
        /// <param name="value">The value of the SIF element</param>
        /// <param name="mapping">The FieldMappings that will be used to set this value or null</param>
        public override void SetSifValue(string fieldName, SifSimpleType value, FieldMapping mapping)
        {
            int ordinal = SafeGetOrdinal(fieldName);
            if( ordinal > -1 )
            {
                fDataRow[ordinal] = value.RawValue;
            }
        }

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
        public override SifSimpleType GetSifValue(string fieldName, TypeConverter typeConverter, FieldMapping mapping)
        {
            int ordinal = SafeGetOrdinal(fieldName);
            if (ordinal == -1)
            {
                return null;
            }
            object value = fDataRow[ordinal];
            return typeConverter.GetSifSimpleType(value);

        }

        /// <summary>
        /// Returns the underlying value being stored for the field.
        /// </summary>
        /// <remarks>
        ///  This method is not called during Mappings operations. It may be called by
        /// classes such as the <see cref="OpenADK.Library.IValueBuilder"/> class
        /// </remarks>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object GetValue(string key)
        {
            int ordinal = SafeGetOrdinal(key);
            if (ordinal == -1)
            {
                return null;
            }
            return fDataRow[ordinal];
        }


        /// <summary>
        /// Gets or sets the current row
        /// </summary>
        public DataRow SourceRow
        {
            get
            {
                return fDataRow;
            }
            set
            {
                fDataRow = value;
            }
        }

    }
}
