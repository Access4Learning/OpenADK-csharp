//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Xml.XPath;
using System.Xml.Xsl;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Tools.Mapping
{
    /**
     * 
     * 	A FieldAdaptor implementation that contains field values to assign to the
     *  supplied SIFDataObject, where each entry in the map is keyed by the
     *  local application-defined name of a field and the value is the native
     *  Java datatype of the corresponding element or attribute of the
     *  SIFDataObject, such as a Calender, String, Integer, Boolean, etc.<p>
     *   
     *    To use this class,<p>
     *
     *  <ol>
     *      <li>
     *          Create an instance and optionally populate the Map with known field
     *          values that will not be subject to the mapping process. If pre-loading
     *          the Map, the key of each entry should be the local
     *          application-defined field name and the value should be the native data type
     *          value of that field. Any field added to the Map before calling
     *          this method will not be subject to mapping rules, unless the 
     *          {@link com.OpenADK.Library.tools.mapping.ObjectMapAdaptor#setOverwriteValues(boolean)}
     *           property is set to <code>True</code>.
     *      </li>
     *      <li>
     *          Use this class instance with the {@link com.OpenADK.Library.tools.mapping.Mappings}
     *          class,by calling the appropriate <code>map</code> method and passing the SIFDataObject
     *          instance to retrieve field values from for insertion into the
     *          Map. The method first looks up the ObjectMapping instance
     *          corresponding to the SIF Data Object type. If no ObjectMapping
     *          has been defined for the object type, no action is taken and the
     *          method returns successfully without exception. Otherwise, all
     *          field rules defined by the ObjectMapping are evaluated in order.
     *          If a rule evaluates successfully, the corresponding element or
     *          attribute value will be inserted into the HashMap. A rule will
     *          not be evaluated if the associated field already exists in the
     *          Map, unless the 
     *          {@link com.OpenADK.Library.tools.mapping.ObjectMapAdaptor#setOverwriteValues(boolean)}
     *          property is set to <code>True</code>.
     *      </li>
     *  </ol>
     *   
     *   @see com.OpenADK.Library.tools.mapping.Mappings
     *    
     * @author Andrew Elmhorst
     * 
     * @version 2.0
     *
     */

    public class ObjectMapAdaptor : IFieldAdaptor, IXPathVariableLibrary
    {
        protected IDictionary fMap;
        private bool fOverwriteValues = false;

        /// <summary>
        /// Creates an instance of ObjectMapAdaptor that uses the specified Map
        /// </summary>
        /// <param name="map">The <c>IDictionary</c> to use for SIF Data mapping operations</param>
        public ObjectMapAdaptor(IDictionary map)
        {
            fMap = map;
        }

  
        /// <summary>
        /// Converts a SIF datatype value to the java native type to be stored
        /// in the Map. The default implementation of this class stores the
        /// native Java value, but subclasses can override this method to convert
        /// the value to the form they want to use. 
        /// </summary>
        /// <param name="value">The SIF value to stored in the Map</param>
        /// <returns>The converted value stored in the Map</returns>
        protected virtual Object ToMapValue(SifSimpleType value)
        {
            return value.RawValue;
        }

        /// <summary>
        /// Converts the object stored in the Map to the SIF data type. The data
        /// type required by SIF is available by examining the {@link SIFTypeConverter}
        /// passed in to the method.
        /// </summary>
        /// <param name="mapValue">The value that was retrieved from the Map</param>
        /// <param name="typeConverter">A representation of the desired SIF datatype</param>
        /// <returns>The converted SIF value</returns>
        protected virtual SifSimpleType FromMapValue(Object mapValue, TypeConverter typeConverter)
        {
            return typeConverter.GetSifSimpleType(mapValue);
        }

        /// <summary>
        /// Gets the value of the specified field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public Object GetValue(String fieldName)
        {
            return fMap[fieldName];
        }


        /// <summary>
        /// Returns true if the specified field exists
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public bool HasField(String fieldName)
        {
            return fMap.Contains(fieldName);
        }

        /// <summary>
        /// Sets the specified value
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="resultingValue"></param>
        /// <param name="mapping"></param>
        public void SetSifValue(String fieldName, SifSimpleType resultingValue, FieldMapping mapping)
        {
            if (fOverwriteValues || !fMap.Contains(fieldName))
            {
                Object mapValue = ToMapValue(resultingValue);
                fMap[fieldName] = mapValue;
            }
        }


        /// <summary>
        /// Gets the specified value from the underlying IDictionary
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="typeConverter"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public SifSimpleType GetSifValue(String fieldName, TypeConverter typeConverter, FieldMapping mapping)
        {
            if (fMap.Contains(fieldName))
            {
                Object value = fMap[fieldName];
                return FromMapValue(value, typeConverter);
            }
            else
            {
                // No value in the Map. Return null
                return null;
            }
        }


        /// <summary>
        /// Gets the underyling IDictionary instance
        /// </summary>
        public IDictionary Dictionary
        {
            get { return fMap; }
            set { fMap = value; }
        }


        /// <summary>
        /// Gets the collection of keys from the underlying map
        /// </summary>
        public ICollection Keys
        {
            get { return fMap.Keys; }
        }



        /// <summary>
        ///  This setting influences inbound mapping operations. If set to <c>True</c>,
        ///  data coming from SIF can overwrite existing values in the Map. The
        ///  default value is <c>False</c>
        public bool OverwriteValues
        {
            get { return fOverwriteValues; }
            set { fOverwriteValues = value; }
        }



        public IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            return new ObjectMapVariable(name, this );
        }

        private class ObjectMapVariable : IXsltContextVariable
        {
            private string fVariableName;
            private ObjectMapAdaptor fData;
            public ObjectMapVariable( String variableName, ObjectMapAdaptor data )
            {
                fVariableName = variableName;
                fData = data;
            }


            ///<summary>
            ///Evaluates the variable at runtime and returns an object that represents the value of the variable.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Object"></see> representing the value of the variable. Possible return types include number, string, Boolean, document fragment, or node set.
            ///</returns>
            ///
            ///<param name="xsltContext">An <see cref="T:System.Xml.Xsl.XsltContext"></see> representing the execution context of the variable. </param>
            public object Evaluate(XsltContext xsltContext)
            {
                return fData.GetValue( fVariableName );
            }

            ///<summary>
            ///Gets a value indicating whether the variable is local.
            ///</summary>
            ///
            ///<returns>
            ///true if the variable is a local variable in the current context; otherwise, false.
            ///</returns>
            ///
            public bool IsLocal
            {
                // This variable is contained in a map outside of the XPathContext,
                // so it is not local
                get { return true; }
            }

            ///<summary>
            ///Gets a value indicating whether the variable is an Extensible Stylesheet Language Transformations (XSLT) parameter. This can be a parameter to a style sheet or a template.
            ///</summary>
            ///
            ///<returns>
            ///true if the variable is an XSLT parameter; otherwise, false.
            ///</returns>
            ///
            public bool IsParam
            {
                get { return false; }
            }

            ///<summary>
            ///Gets the <see cref="T:System.Xml.XPath.XPathResultType"></see> representing the XML Path Language (XPath) type of the variable.
            ///</summary>
            ///
            ///<returns>
            ///The <see cref="T:System.Xml.XPath.XPathResultType"></see> representing the XPath type of the variable.
            ///</returns>
            ///
            public XPathResultType VariableType
            {
                get
                {
                    // TODO: This perhaps needs to be more dynamic, 
                     return SifXsltContext.GetXPathResultType( typeof(String));
                }
            }
        }
    }
}
