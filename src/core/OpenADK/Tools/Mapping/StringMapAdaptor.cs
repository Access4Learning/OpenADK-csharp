//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>
    /// A FieldAdaptor implementation that contains field values to assign to the
    /// supplied SIFDataObject, where each entry in the map is keyed by the
    /// local application-defined name of a field and the value is the text
    /// value to assign to the corresponding element or attribute of the
    /// SIFDataObject, in the SIF 1.5r1 text format.
    /// </summary>
    /// <remarks>
    /// <para>
    ///  This class is most useful for agents that were written using the 1.x version
    ///  of the ADK and offers compatibility with the Mappings implementation used in
    ///  that release. Values mapped to and from the Map used in this class will match
    ///  the textual representation of those values in the 1.x version of the ADK. For example,
    ///  a SIF date field that is mapped to this class will map to and from the SIF 1.5 format 
    ///  for dates, which was yyyyMMdd.<p>
    /// </para>
    /// <para>
    ///  The Data-to-Text formatting is controlled by the {@link com.OpenADK.Library.ADK#getTextFormatter()}
    ///  property, which defaults to SIF 1.5 formatting. If you wish to supply your own text formatter,
    ///  you can call the constructor overload that accepts a SIFFormatter instance.
    /// </para>
    /// <para>
    ///  To use this class,
    /// <list type="List">
    /// <item>
    ///  Create an instance and optionally populate the Map with known field
    ///  values that will not be subject to the mapping process. If pre-loading
    ///  the Map, the key of each entry should be the local
    ///  application-defined field name and the value should be the string
    ///  value of that field. Any field added to the Map before calling
    ///  this method will not be subject to mapping rules, unless the <see cref="ObjectMapAdaptor#OverwriteValues"/>  
    /// property is set to <code>True</code>.
    /// </item>
    /// <item>
    /// Use this class instance with the Mappings
    /// class,by calling the appropriate <code>map</code> method and passing the SIFDataObject
    /// instance to retrieve field values from for insertion into the
    /// Map. The method first looks up the ObjectMapping instance
    /// corresponding to the SIF Data Object type. If no ObjectMapping
    /// has been defined for the object type, no action is taken and the
    /// method returns successfully without exception. Otherwise, all
    /// field rules defined by the ObjectMapping are evaluated in order.
    /// If a rule evaluates successfully, the corresponding element or
    /// attribute value will be inserted into the HashMap. A rule will
    /// not be evaluated if the associated field already exists in the
    /// Map, unless the <see cref="ObjectMapAdaptor#OverwriteValues"/>  
    /// property is set to <code>True</code>.
    /// </item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="Adk#TextFormatter"/>
    public class StringMapAdaptor : ObjectMapAdaptor
    {
        private SifFormatter fDataFormatter;

        /// <summary>
        /// Creates a new instance of a StringMapAdaptor which uses the ADK default TextFormatter
        /// </summary>
        /// <param name="dataMap">The IDictionary instance to use for getting and setting values</param>
        public StringMapAdaptor(IDictionary dataMap) : this(dataMap, Adk.TextFormatter)
        {
        }

        /// <summary>
        /// Creates a new instance of a StringMapAdaptor
        /// </summary>
        /// <param name="dataMap">The IDictionary instance to use for getting and setting values</param>
        /// <param name="formatter">The SifFormatter to use for getting and setting text values</param>
        public StringMapAdaptor(IDictionary dataMap, SifFormatter formatter) : base(dataMap)
        {
            fDataFormatter = formatter;
        }


        /// <summary>
        /// Sets the value to the underlying dictionary using a SifFormatter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override Object ToMapValue(SifSimpleType value)
        {
            // Store items in the map using a string value
            return value.ToString(fDataFormatter);
        }

        /// <summary>
        /// Gets the value from the underlying dictionary using the SifFormatter
        /// </summary>
        /// <param name="mapValue"></param>
        /// <param name="typeConverter"></param>
        /// <returns></returns>
        protected override SifSimpleType FromMapValue(Object mapValue, TypeConverter typeConverter)
        {
            // Items are stored in the map as string values. Convert them using the typeConverter's parse method
            if (mapValue == null)
            {
                return null;
            }
            try
            {
                return typeConverter.Parse(fDataFormatter, (String) mapValue);
            }
            catch (AdkParsingException adkpe)
            {
                throw new FormatException(adkpe.Message);
            }
        }
    }
}
