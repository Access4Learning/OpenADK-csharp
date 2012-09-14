//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Data;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>
    /// Implements IFieldAdaptor for the ADK, based on an IDataReader instance.
    /// </summary>
    /// <remarks>
    /// To use this class, create an instance of it based on an open IDataReader instance. You
    /// are responsible for navigation of the IDataReader by calling IDataReader.Read. When you are
    /// ready to map the current IDataReader row to a SIFElement, pass this class into the <c>MappingsContext.MapOutbound()</c>
    /// method.
    /// </remarks>
    public class DataReaderAdaptor : IFieldAdaptor
    {
        private readonly IDataReader fReader;
        private readonly ArrayList fColumns;

        public DataReaderAdaptor( IDataReader reader )
        {
            fReader = reader;
            fColumns = new ArrayList();
            foreach (DataRowView row in reader.GetSchemaTable().DefaultView )
            {
                fColumns.Add(row["ColumnName"].ToString().ToUpper());
            }
        }

        #region IFieldAdaptor Members

        /// <summary>
        /// Returns whether the field being requested for mapping has a value that can
        /// be mapped to SIF
        /// </summary>
        /// <param name="fieldName">The field name being mapped to SIF</param>
        /// <returns><c>True</c> if there is a value for the specified field that
        /// should be mapped to SIF</returns>
        public bool HasField(string fieldName)
        {
            return SafeGetOrdinal(fieldName) > -1;
        }

        #endregion

        /// <summary>
        /// Sets a value that has been retrieved from a SIF Element in an inbound field
        /// mapping operation.
        /// </summary>
        /// <param name="fieldName">The field name that is mapped to a SIFElement</param>
        /// <param name="value">The value of the SIF element</param>
        /// <param name="mapping">The FieldMappings that will be used to set this value or null</param>
        public void SetSifValue(string fieldName, SifSimpleType value, FieldMapping mapping)
        {
            throw new NotSupportedException("Unable to set values to a DataReader");
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
        public SifSimpleType GetSifValue(string fieldName, TypeConverter typeConverter, FieldMapping mapping)
        {
            int ordinal = SafeGetOrdinal(fieldName);
            if( ordinal == -1 )
            {
                return null;
            }
            object value = fReader.GetValue(ordinal);
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
        public object GetValue(string key)
        {
            int ordinal = SafeGetOrdinal(key);
            if (ordinal == -1)
            {
                return null;
            }
            return fReader.GetValue(ordinal);
        }

        private int SafeGetOrdinal(string key)
        {
            if( key == null )
            {
                return -1;
            }
            return fColumns.IndexOf( key.ToUpper() );
        }
    }
}
