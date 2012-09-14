//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Tools.Mapping
{
    public interface IFieldAdaptor
    {
        /// <summary>
        /// Returns whether the field being requested for mapping has a value that can
        /// be mapped to SIF
        /// </summary>
        /// <param name="fieldName">The field name being mapped to SIF</param>
        /// <returns><c>True</c> if there is a value for the specified field that
        /// should be mapped to SIF</returns>
        bool HasField(String fieldName);

        /// <summary>
        /// Sets a value that has been retrieved from a SIF Element in an inbound field
        /// mapping operation.
        /// </summary>
        /// <param name="fieldName">The field name that is mapped to a SIFElement</param>
        /// <param name="value">The value of the SIF element</param>
        /// <param name="mapping">The FieldMappings that will be used to set this value or null</param>
        void SetSifValue(String fieldName, SifSimpleType value, FieldMapping mapping);

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
        SifSimpleType GetSifValue(String fieldName, TypeConverter typeConverter, FieldMapping mapping);


        /// <summary>
        /// Returns the underlying value being stored for the field.
        /// </summary>
        /// <remarks>
        ///  This method is not called during Mappings operations. It may be called by
        /// classes such as the <see cref="OpenADK.Library.IValueBuilder"/> class
        /// </remarks>
        /// <param name="key"></param>
        /// <returns></returns>
        Object GetValue(String key);
    }


}
