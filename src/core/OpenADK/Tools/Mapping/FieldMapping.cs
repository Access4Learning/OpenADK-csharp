//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;
using System.Xml;
using OpenADK.Library.Tools.Cfg;
using OpenADK.Library.Tools.XPath;
using OpenADK.Util;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  A FieldMapping defines how to map a local application field to an element or
    /// attribute of the SIF Data Object type encapsulated by the parent ObjectMapping.
    /// Each FieldMapping is associated with a <i>Rule</i> that is evaluated at
    /// runtime to carry out the actual mapping operation on a SifDataObject instance.
    /// The way the rule behaves is up to its implementation.
    /// 
    /// A FieldMapping may have a default value. If set, the default value is
    /// assigned to the SIF element or attribute if the corresponding field value is
    /// null or undefined. This is useful if you wish to ensure that a specific SIF
    /// element/attribute always has a value regardless of whether or not there is a
    /// corresponding value in your application's database.
    /// 
    /// 
    /// The application-defined field name that is associated with a FieldMapping
    /// must be unique; that is, there cannot be more than one FieldMapping for the
    /// same application field. However, if you wish to map the same field to more
    /// than one SIF element or attribute, you can create an <i>alias</i>. An alias
    /// is a FieldMapping that has a unique field name but refers to another field.
    /// For example, if your application defines the field STUDENT_NUM and you wish
    /// to define two FieldMappings for that field, create an alias:
    /// 
    /// 
    /// <code>
    /// // Create the default mapping<br/>
    /// FieldMapping fm = new FieldMapping("STUDENT_NUM","OtherId[@Type='06']");<br/><br/>
    /// <br/>
    /// // Create an alias (the field name must be unique)<br/>
    /// FieldMapping zz = new FieldMapping("MYALIAS","OtherId[@Type='ZZ']");<br/>
    /// zz.setAlias( "STUDENT_NUM" );<br/><br/>
    /// </code>
    /// 
    /// In the above example, the "STUDENT_NUM" mapping produces an &lt;OtherId&gt;
    /// element with its Type attribute set to '06'. The "MYALIAS" mapping produces
    /// a second &lt;OtherId&gt; element with its Type attribute set to 'ZZ'. Both
    /// elements will have the value of the application-defined STUDENT_NUM field.
    /// Note that if MYALIAS were an actual field name of your application, however,
    /// the value of the &lt;OtherId Type='ZZ'&gt; element would be equal to that
    /// field's value. When creating aliases be sure to choose a name that does not
    /// conflict with the real field names used by your application.
    /// 
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class FieldMapping
    {
        private const String ATTR_VALUESET = "valueset";
        private const String ATTR_ALIAS = "alias";
        private const String ATTR_DEFAULT = "default";
        private const String ATTR_NAME = "name";
        private const String ATTR_DATATYPE = "datatype";
        private const String ATTR_SIFVERSION = "sifVersion";
        private const String ATTR_DIRECTION = "direction";
        private const String ATTR_IFNULL = "ifnull";


        private MappingBehavior fNullBehavior = MappingBehavior.IfNullUnspecified;
        protected internal Rule fRule;
        protected string fField;
        protected string fDefValue;
        protected string fAlias;
        protected string fValueSet;
        protected MappingsFilter fFilter;
        internal XmlElement fNode;
        private SifDataType fDatatype = SifDataType.String;

        /// <summary>  Constructor</summary>
        public FieldMapping()
            : this(null, (string) null, (XmlElement) null)
        {
        }

        /// <summary>  Constructs a FieldMapping with an XPath-like rule</summary>
        /// <param name="name">The name of the local application field that maps to the
        /// SIF Data Object element or attribute described by this FieldMapping
        /// </param>
        /// <param name="rule">An XPath-like query string described by the <code>SifDtd.lookupByXPath</code>
        /// method
        /// </param>
        public FieldMapping(string name,
                            string rule)
            : this(name, rule, null)
        {
        }

        public FieldMapping(string name,
                            string rule,
                            XmlElement node)
        {
            fField = name;
            if (rule != null)
            {
                SetRule(rule);
            }
            fNode = node;
        }

        /// <summary>  Constructs a FieldMapping with an &lt;OtherId&gt; rule.
        /// 
        /// </summary>
        /// <param name="name">The name of the local application field that maps to the
        /// SIF Data Object element or attribute described by this FieldMapping
        /// </param>
        /// <param name="rule">An OtherIdMapping object that describes how to select
        /// a &lt;OtherId&gt; element during a mapping operation
        /// </param>
        public FieldMapping(string name,
                            OtherIdMapping rule)
            : this(name, rule, null)
        {
        }

        /// <summary>  Constructs a FieldMapping with an &lt;OtherId&gt; rule.
        /// 
        /// </summary>
        /// <param name="name">The name of the local application field that maps to the
        /// SIF Data Object element or attribute described by this FieldMapping
        /// </param>
        /// <param name="rule">An OtherIdMapping object that describes how to select
        /// a &lt;OtherId&gt; element during a mapping operation
        /// </param>
        /// <param name="node">The XmlElement that this FieldMapping draws its configuration from</param>
        public FieldMapping(string name,
                            OtherIdMapping rule,
                            XmlElement node)
        {
            fField = name;
            SetRule(rule);
            fNode = node;
        }


        /// <summary>  Gets the optional DOM XmlElement associated with this FieldMapping instance. 
        /// The DOM XmlElement is usually set by the parent ObjectMapping instance when a 
        /// FieldMapping is populated from a DOM Document.
        /// </summary>
        /// <summary>  Sets the optional DOM XmlElement associated with this FieldMapping instance. 
        /// The DOM XmlElement is usually set by the parent ObjectMapping instance when a 
        /// FieldMapping is populated from a DOM Document.
        /// </summary>
        public virtual XmlElement XmlElement
        {
            get { return fNode; }

            set { fNode = value; }
        }


        /**
 * Creates a new FieldMapping instance and populates its properties from
 * the given XML Element
 * @param parent 
 * @param element
 * @return a new FieldMapping instance
 * @throws ADKConfigException If the FieldMapping cannot read expected 
 * 		values from the DOM Node
 */

        public static FieldMapping FromXml(
            ObjectMapping parent,
            XmlElement element)
        {
            if (element == null)
            {
                throw new ArgumentException("Argument: 'element' cannot be null");
            }

            String name = element.GetAttribute(ATTR_NAME);
            FieldMapping fm = new FieldMapping();
            fm.SetNode(element);
            fm.FieldName = name;
            fm.DefaultValue = XmlUtils.GetAttributeValue(element, ATTR_DEFAULT);
            fm.Alias = XmlUtils.GetAttributeValue(element, ATTR_ALIAS);
            fm.ValueSetID = XmlUtils.GetAttributeValue(element, ATTR_VALUESET);

            String ifNullBehavior = element.GetAttribute(ATTR_IFNULL);
            if (ifNullBehavior.Length > 0)
            {
                if (String.Compare(ifNullBehavior, "default", true) == 0)
                {
                    fm.NullBehavior = MappingBehavior.IfNullDefault;
                }
                else if (String.Compare(ifNullBehavior, "suppress", true) == 0)
                {
                    fm.NullBehavior = MappingBehavior.IfNullSuppress;
                }
            }

            String dataType = element.GetAttribute(ATTR_DATATYPE);
            if (dataType != null && dataType.Length > 0)
            {
                try
                {
                    fm.DataType = (SifDataType) Enum.Parse(typeof (SifDataType), dataType, true);
                }
                catch (FormatException iae)
                {
                    Adk.Log.Warn("Unable to parse datatype '" + dataType + "' for field " + name, iae);
                }
            }

            String filtVer = element.GetAttribute(ATTR_SIFVERSION);
            String filtDir = element.GetAttribute(ATTR_DIRECTION);

            if( !(String.IsNullOrEmpty( filtVer )) || !(String.IsNullOrEmpty(filtDir )) )
            {
                MappingsFilter filt = new MappingsFilter();

                if (!String.IsNullOrEmpty(filtVer))
                    filt.SifVersion = filtVer;

                if (!String.IsNullOrEmpty(filtDir))
                {
                    if (String.Compare(filtDir, "inbound", true) == 0)
                        filt.Direction = MappingDirection.Inbound;
                    else if (String.Compare(filtDir, "outbound", true) == 0)
                        filt.Direction = MappingDirection.Outbound;
                    else
                        throw new AdkConfigException(
                            "Field mapping rule for " + parent.ObjectType + "." + fm.FieldName +
                            " specifies an unknown Direction flag: '" + filtDir + "'");
                }
                fm.Filter = filt;
            }


            //  FieldMapping must either have node text or an <otherid> child
            XmlElement otherIdNode = XmlUtils.GetFirstElementIgnoreCase(element, "otherid");
            if (otherIdNode == null)
            {
                String def = element.InnerText;
                if (def != null)
                    fm.SetRule(def);
                else
                    fm.SetRule("");
            }
            else
            {
                fm.SetRule(OtherIdMapping.FromXml(parent, fm, otherIdNode), otherIdNode);
            }

            return fm;
        }

        /**
         * Writes the values of this FieldMapping to the specified XML Element
         * 
         * @param element The XML Element to write values to
         */

        public void ToXml(XmlElement element)
        {
            XmlUtils.SetOrRemoveAttribute(element, ATTR_NAME, fField);
            if (fDatatype == SifDataType.String)
            {
                element.RemoveAttribute(ATTR_DATATYPE);
            }
            else
            {
                element.SetAttribute(ATTR_DATATYPE, fDatatype.ToString());
            }
            XmlUtils.SetOrRemoveAttribute(element, ATTR_DEFAULT, fDefValue);
            XmlUtils.SetOrRemoveAttribute(element, ATTR_ALIAS, fAlias);
            XmlUtils.SetOrRemoveAttribute(element, ATTR_VALUESET, fValueSet);

            MappingsFilter filt = Filter;
            if (filt != null)
            {
                WriteFilterToXml(filt, element);
            }
            WriteNullBehaviorToXml(fNullBehavior, element);
            fRule.ToXml(element);
        }

        /**
         * Writes the mapping filter to an XML Element
         * @param filter
         * @param element The XML Element to write the filter to
         */

        private void WriteFilterToXml(MappingsFilter filter, XmlElement element)
        {
            if (filter == null)
            {
                element.RemoveAttribute(ATTR_SIFVERSION);
                element.RemoveAttribute(ATTR_DIRECTION);
            }
            else
            {
                if (filter.HasVersionFilter)
                {
                    element.SetAttribute(ATTR_SIFVERSION, filter.SifVersion);
                }
                else
                {
                    element.RemoveAttribute(ATTR_SIFVERSION);
                }

                MappingDirection direction = filter.Direction;
                if (direction == MappingDirection.Inbound)
                {
                    element.SetAttribute(ATTR_DIRECTION, "inbound");
                }
                else if (direction == MappingDirection.Outbound)
                {
                    element.SetAttribute(ATTR_DIRECTION, "outbound");
                }
                else
                {
                    element.RemoveAttribute(ATTR_DIRECTION);
                }
            }
        }

        private void WriteNullBehaviorToXml(MappingBehavior behavior, XmlElement element)
        {
            switch (behavior)
            {
                case MappingBehavior.IfNullDefault:
                    element.SetAttribute(ATTR_IFNULL, "default");
                    break;
                case MappingBehavior.IfNullSuppress:
                    element.SetAttribute(ATTR_IFNULL, "suppress");
                    break;
                default:
                    element.RemoveAttribute(ATTR_IFNULL);
                    break;
            }
        }


        /// <summary>  Gets or sets the name of the local application field that maps to the SIF Data
        /// Object element or attribute
        /// </summary>
        /// <value> The local application field name. (This value will be used as
        /// the key in HashMaps populated by the Mappings.map methods)
        /// </value>
        public virtual string FieldName
        {
            get { return fField; }

            set
            {
                fField = value;

                if (fNode != null && value != null)
                {
                    fNode.SetAttribute("name", value);
                }
            }
        }

        /// <summary>Gets or sets the ID of the ValueSet that should be used to translate the value 
        /// of this field.
        /// </summary>
        /// <remarks>
        /// Note: The Mappings classes do not automatically perform translations if
        /// this attribute is defined. Rather, it is provided so that agents can 
        /// associate a ValueSet with a field in the Mappings configuration file, 
        /// and have a means of looking up that association at runtime.
        /// 
        ///</remarks>
        /// <returns> The value passed to the <code>setValueSetID</code> method
        /// 
        /// @since Adk 1.5
        /// 
        /// </returns>
        public virtual string ValueSetID
        {
            get { return fValueSet; }

            set
            {
                fValueSet = value;
                if (fValueSet != null && fValueSet.Trim().Length == 0)
                {
                    fValueSet = null;
                }

                if (fNode != null)
                {
                    if (value != null)
                    {
                        fNode.SetAttribute("valueset", fValueSet);
                    }
                    else
                    {
                        fNode.RemoveAttribute("valueset");
                    }
                }
            }
        }

        /// <summary>  Gets or sets the default value for this field when no corresponding element or
        /// attribute is found in the SIF Data Object. The Mapping.map methods will
        /// create an entry in the HashMap with this default value.
        /// 
        /// </summary>
        /// <value> The default string value for this field
        /// </value>
        public virtual string DefaultValue
        {
            get { return fDefValue; }

            set
            {
                fDefValue = value;
                if (fNode != null)
                {
                    if (value != null)
                    {
                        fNode.SetAttribute("default", value);
                    }
                    else
                    {
                        fNode.RemoveAttribute("default");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the default value 
        /// </summary>
        /// <param name="converter">The type converter to use</param>
        /// <param name="formatter">The formatter to use for the version of SIF</param>
        /// <returns></returns>
        public SifSimpleType GetDefaultValue(TypeConverter converter, SifFormatter formatter)
        {
            if (fDefValue != null && converter != null)
            {
                try
                {
                    return converter.Parse(formatter, fDefValue);
                }
                catch (AdkParsingException adkpe)
                {
                    throw new AdkMappingException(adkpe.Message, null, adkpe);
                }
            }
            return null;
        }

        /// <summary>
        ///  Quickly determines whether this field mapping has a default value defined
        /// without going through the extra work of actually resolving the default value
        /// </summary>
        /// <value>True if this field mapping has a default value defined</value>
        public bool HasDefaultValue
        {
            get { return fDefValue != null; }
        }

        /// <summary>Gets or sets an alias of another field mapping.</summary>
        /// <value> The name of the field for which this entry is an alias, or null
        /// if this FieldMapping is not an alias.</value>
        /// <remarks> Defines this FieldMapping to be an alias of another field mapping. During
        /// the mapping process, the FieldMapping will be applied if the referenced
        /// field exists in the Map provided to the Mappings.map method. Aliases are
        /// required when an application wishes to map a single application field to
        /// more than one element or attribute in the SIF Data Object.
        /// 
        /// To use aliases, create a FieldMapping where the field name is a unique
        /// name and the alias is the name of an existing field. For example, to map
        /// an application-defined field named "STUDENT_NUM" to more than one
        /// element/attribute in the SIF Data Object,
        /// </remarks>
        /// <example>
        /// <code>
        /// // Create the default mapping<br/>
        /// FieldMapping fm = new FieldMapping("STUDENT_NUM","OtherId[@Type='06']");
        /// // Create an alias; the field name must be unique<br/>
        /// FieldMapping fm2 = new FieldMapping("STUDENT_NUM_B","OtherId[@Type='ZZ']=STUDENTID:$(STUDENTNUM)");<br/>
        /// </code>
        ///</example> 
        public virtual string Alias
        {
            get { return fAlias; }

            set
            {
                fAlias = value;

                if (fNode != null)
                {
                    if (value != null)
                    {
                        fNode.SetAttribute("alias", value);
                    }
                    else
                    {
                        fNode.RemoveAttribute("alias");
                    }
                }
            }
        }

        /// <summary>Gets or sets optional filtering attributes.</summary>
        /// <value> A MappingsFilter instance or null if none defined for
        /// this field rule
        /// </value>
        public virtual MappingsFilter Filter
        {
            get { return fFilter; }

            set
            {
                fFilter = value;
                if (fNode != null)
                {
                    MappingsFilter.Save(fFilter, fNode);
                }
            }
        }


        /// <summary>
        /// Returns the key to a Field Mapping. The Key of a field mapping consists
        /// of it's alias or field name and any filters that are defined
        /// </summary>
        public string Key
        {
            get
            {
                StringBuilder key = new StringBuilder();
                key.Append(fField);
                if (fAlias != null)
                {
                    key.Append('_');
                    key.Append(fAlias);
                }
                if (fFilter != null)
                {
                    if (fFilter.HasDirectionFilter)
                    {
                        key.Append('_');
                        key.Append(fFilter.Direction);
                    }
                    if (fFilter.HasVersionFilter)
                    {
                        key.Append('_');
                        key.Append(fFilter.SifVersion);
                    }
                }
                return key.ToString();
            }
        }


        /// <summary>  Creates a copy this ObjectMapping instance.
        /// 
        /// </summary>
        /// <returns> A "deep copy" of this object
        /// </returns>
        public virtual FieldMapping Copy(ObjectMapping newParent)
        {
            FieldMapping m = new FieldMapping();

            if (fNode != null && newParent.fNode != null)
            {
                m.fNode = (XmlElement) newParent.fNode.OwnerDocument.ImportNode(fNode, false);
            }

            m.FieldName = fField;
            m.DefaultValue = fDefValue;
            m.Alias = fAlias;
            m.ValueSetID = fValueSet;
            m.NullBehavior = fNullBehavior;

            if (fFilter != null)
            {
                MappingsFilter filtCopy = new MappingsFilter();
                filtCopy.fVersion = fFilter.fVersion;
                filtCopy.fDirection = fFilter.fDirection;
                m.Filter = filtCopy;
            }

            m.DataType = fDatatype;

            if (fRule != null)
            {
                m.fRule = fRule.Copy(m);
            }
            else
            {
                m.fRule = null;
            }

            return m;
        }

        public SifSimpleType Evaluate(SifXPathContext xpathContext, SifVersion version, bool returnDefault)
        {
            SifSimpleType value = null;
            if (fRule != null)
            {
                value = fRule.Evaluate(xpathContext, version);
            }
            if (value == null && fDefValue != null && returnDefault)
            {
                // TODO: Support all data types
                try
                {
                    return SifTypeConverters.GetConverter(fDatatype).Parse(Adk.TextFormatter, fDefValue);
                }
                catch (AdkParsingException adkpe)
                {
                    throw new AdkSchemaException(
                        "Error parsing default value: '" + fDefValue + "' for field " + fField + " : " + adkpe, null,
                        adkpe);
                }
            }

            return value;
        }


        /// <summary>  Sets this FieldMapping rule to an XPath-like query string</summary>
        /// <param name="definition">An XPath-like query string described by the
        /// <code>SifDtd.lookupByXPath</code> method
        /// </param>
        public void SetRule(string definition)
        {
            fRule = new XPathRule(definition);
            if (fNode != null)
            {
                fNode.InnerText = definition;
            }
        }

        /// <summary>  Sets this object's rule to an "&lt;OtherId&gt; rule"</summary>
        /// <param name="otherId">An OtherIdMapping object that describes how to select
        /// a &lt;OtherId&gt; element during a mapping operation
        /// </param>
        public void SetRule(OtherIdMapping otherId)
        {
            fRule = new OtherIdRule(otherId);
            if (fNode != null)
            {
                fRule.ToXml(fNode);
            }
        }

        public void SetRule(OtherIdMapping otherId,
                            XmlElement node)
        {
            fRule = new OtherIdRule(otherId, node);
        }

        /// <summary>  Gets the field mapping rule</summary>
        /// <returns> A Rule instance
        /// </returns>
        public Rule GetRule()
        {
            return fRule;
        }

        private void SetNode(XmlElement element)
        {
            fNode = element;
        }

        public MappingBehavior NullBehavior
        {
            get { return fNullBehavior; }
            set
            {
                if (value < MappingBehavior.IfNullUnspecified || value > MappingBehavior.IfNullSuppress)
                {
                    throw new ArgumentException("Value must be one of the FieldMapping.IFNULL_XXX constants.");
                }
                fNullBehavior = value;
                if (fNode != null)
                {
                    WriteNullBehaviorToXml(value, fNode);
                }
            }
        }

        /// <summary>
        /// Gets or sets the data type that this FieldMapping represents
        /// </summary>
        public SifDataType DataType
        {
            get { return fDatatype; }
            set
            {
                fDatatype = value;
                if (fNode != null)
                {
                    if (fDatatype == SifDataType.String)
                    {
                        fNode.RemoveAttribute(ATTR_DATATYPE);
                    }
                    else
                    {
                        fNode.SetAttribute(ATTR_DATATYPE, fDatatype.ToString());
                    }
                }
            }
        }
    }
}
