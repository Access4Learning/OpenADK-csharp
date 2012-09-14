//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using OpenADK.Library.Tools.Cfg;
using OpenADK.Library.Tools.XPath;
using OpenADK.Util;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  Manages a set of mapping rules that define how to tranform a set of application 
    /// field values into SIF Data Objects</summary>
    /// <remarks>
    /// <para>
    /// The Mappings class is a powerful facility 
    /// for programmatically transforming sets of data into SifDataObject
    /// instances based on XPath-like mapping rules. These rules can be read from
    /// any DOM Document or configuration file read and written by the AgentConfig 
    /// class, which makes them easily customizable by the end-user.</para>
    /// 
    /// <para>
    /// Mappings are arranged into a selection hierarchy so that different sets of
    /// rules can be created for individual zones, agent SourceIds, and versions of 
    /// SIF. Rules at lower levels in the hierarchy are inherited from higher levels. 
    /// Thus, you could create a default set of rules describing how to produce 
    /// StudentPersonal objects for all zones, and extend or override these rules in 
    /// a set of child rules that are specific to an individual zone. When selecting
    /// the Mappings object for the SIF Message being processed, the <c>select</c> 
    /// method would choose the zone-specific set of rules over the default rules,
    /// but would inherit any default field mappings not explicitly overridden. This
    /// technique is very useful when the mapping rules of a SIF Agent differ from
    /// zone-to-zone or from one version of SIF to the next.</para>
    /// 
    /// Overview
    /// <para>
    /// Each Mappings object is comprised of one or more of the following:
    /// </para><para>
    /// <c>ObjectMappings</c> that defines how to map fields of the
    /// application to a SIF Data Object's elements and attributes. Create
    /// one ObjectMapping per SIF Data Object type (e.g. for "StudentPersonal",
    /// "BusInfo", etc.) An <c>ObjectMapping</c> instance is in turn
    /// comprised of one or more <c>FieldMapping</c> objects, each of
    /// which is comprised of a <c>Rule</c>. Two Rule implementations
    /// are currently defined: <c>XPathRule</c>, which uses an XPath-like
    /// query string to describe how to interpret or build a SIF element or
    /// attribute; and <c>OtherIdRule</c>, which describes how to
    /// interpret or build <c>&lt;OtherId&gt;</c> values.
    /// </para><para>
    /// <c>ValueSets</c> that define a simple mapping from codes and
    /// constants used in the application to equivalent codes and constants
    /// used by SIF. A <c>ValueSet</c> could be used, for example, to
    /// define a mapping table for Grade Levels, Ethnicity Codes, English
    /// Proficiency Codes, etc.
    /// </para><para>
    /// User-defined properties comprised of a name and value pair.
    /// </para>
    /// Mappings Hierarchy
    /// <para>
    /// Mappings objects form a hierarchy comprised of a <i>root</i> Mappings object --
    /// which is always present and does not have an ID -- and one or more child
    /// Mappings of the root. The root serves as a container for all other Mappings
    /// objects. Each child of the root container must be assigned a unique ID.
    /// Mappings may also be assigned an optional ZoneId filter, SourceId filter,
    /// an SIF Version filter.</para>
    /// 
    /// <para>
    /// The unique ID is used to select a Mappings object at runtime when multiple
    /// groups of Mappings are present. If you only have one Mappings object, it is
    /// recommended that it be given an ID of "default". However, agents that both
    /// provide and consume objects will usually define two groups of Mappings
    /// objects: one used in the translation of incoming messages and another used
    /// in the production of outgoing messages. In this case it is recommended that
    /// two Mappings objects be defined, one with an ID of "incoming" or similar and
    /// the other with an ID of "outgoing" or similar.</para>
    /// 
    /// <para>
    /// When Mappings are nested, child Mappings inherit the filters, ObjectMappings,
    /// ValueSets, and properties of the parent.</para>
    /// 
    /// 
    /// <b>SourceId Filter</b>
    /// <para>
    /// When a SourceId filter is assigned to a Mappings object, the <c>select</c>
    /// method will exclude any Mappings object that does not include the SourceId
    /// passed to that method. If you have tested your agent with several Student
    /// Information Systems, for example, you may wish to define the unique mapping
    /// rules for each SIS in a separate Mappings object where the SourceId filter
    /// includes the SourceId of the SIS agent.</para>
    /// 
    /// 
    /// <b>ZoneId Filter</b>
    /// <para>
    /// When a ZoneId filter is assigned to a Mappings object, the <c>select</c>
    /// method will exclude any Mappings instance that does not include the ZoneId
    /// passed to that method. The ZoneId filter can be used to customize mappings
    /// on a zone-by-zone basis.</para>
    /// 
    /// <b>SifVersion Filter</b>
    /// <para>
    /// When a SifVersion filter is assigned to a Mappings object, the <c>select</c>
    /// method will exclude any Mappings instance that does not include the SIF Version
    /// passed to that method. Such a filter can be used to customize mappings for
    /// a specific version of SIF. This is often necessary if the tag names of
    /// elements or attributes have changed from one version of SIF to the next. In
    /// this case, the XPath-like rules that you include in a Mappings object must
    /// reflect the element and attribute names of each version of SIF.</para>
    /// 
    /// <b>Mappings Rules</b>
    /// <para>
    /// Mapping <i>rules</i> are comprised of one or more ObjectMapping and FieldMapping
    /// objects that define how to map a field of the local application's database
    /// to a SIF Data Object element or attribute. When multiple mappings are
    /// defined for a field, the first one that evaluates true is selected. The
    /// <c>map</c> method that accepts a <c>SifElement</c> object
    /// evaluates the rules of a Mappings object against a SIF Data Object instance
    /// to produce a IDictionary of values. Each entry in the table is a key/value pair
    /// where the key is the name of the local application field and the value is
    /// the value from the SIF Data Object that mapped to that field. Thus, an agent
    /// can call the <c>map</c> method to obtain a table of values for each
    /// SIF Data Object it is processing. The Mappings class can also be used to
    /// produce a SIF Data Object instance from a IDictionary prepared by the agent.
    /// Call the <c>map</c> method that accepts a IDictionary instance to return
    /// a <c>SifElement</c> object.</para>
    /// <para>
    /// Mappings are comprised of a hierarchy of ObjectMapping and FieldMapping
    /// object. The ObjectMapping class encapsulates a SIF Data Object type such as
    /// StudentPersonal and BusInfo. Each ObjectMapping contains one or more
    /// FieldMapping objects to define a field-level mapping. Mapping rules are
    /// specified in an XPath-like format relative to the SIF Data Object type named
    /// in the ObjectMapping.
    /// </para>
    ///
    /// <b>XML Configuration</b>
    /// <para>
    /// The <c>populate</c> method constructs a Mappings hierarchy from a
    /// parsed DOM Document. The populate method can only be called on the root
    /// Mappings container. Consult the Mappings.dtd file in the Adk's Extras
    /// directory for the expected schema.
    /// </para>
    /// <para>Author: Eric Petersen</para>
    /// <para>Version: Adk 1.0</para>
    /// </remarks>
    /// <example>
    /// To use the Mappings class,
    /// <para>
    /// Create a Mappings instance and populate it with Mappings children.
    /// The easiest way to accomplish this is to call the <c>read</c>
    /// method to read an XML configuration file or the <c>populate</c>
    /// method to populate a Mappings instance from a DOM <c>Document</c>.
    /// Refer to the Adk Developer Guide for more information.</para>
    /// <para>
    /// When your agent needs to map local application field values to SIF
    /// Data Object elements and attributes, call the <c>select</c>
    /// method to select the appropriate Mappings instance based on ZoneId,
    /// SourceId, and SIF Version. The <c>select</c> method returns
    /// a Mappings instance.</para>
    /// Call the <c>map</c> method to produce a SifDataObject from a
    /// IDictionary of field values prepared by your application (i.e. when
    /// preparing outbound messages), or to populate a IDictionary from a
    /// SifDataObject (i.e. when processing inbound messages).
    /// </example>
    public class Mappings
    {
        /// <summary>Gets or  Sets the optional DOM XmlElement associated with this Mappings instance. The
        /// DOM XmlElement is set when a Mappings object is populated from a DOM Document.
        /// </summary>
        public XmlElement XmlElement
        {
            get { return fNode; }

            set { fNode = value; }
        }

        /// <summary>  Gets the unique ID of this Mappings instance</summary>
        public string Id
        {
            get { return fId; }
        }

        /// <summary>  Gets the parent Mappings instance</summary>
        public Mappings Parent
        {
            get { return fParent; }
        }

        /// <summary>  Return an array of all Mappings children</summary>
        public Mappings[] Children
        {
            get
            {
                Mappings[] arr = new Mappings[fChildren == null ? 0 : fChildren.Count];
                if (fChildren != null)
                {
                    fChildren.Values.CopyTo(arr, 0);
                }
                return arr;
            }
        }

        /// <summary>  Count the number of Mappings children</summary>
        public int ChildCount
        {
            get { return fChildren == null ? 0 : fChildren.Count; }
        }

        /// <summary>  Gets the names of all user-defined properties currently set on this Mappings instance.
        /// 
        /// </summary>
        /// <returns> An array of property names
        /// </returns>
        public string[] PropertyNames
        {
            get
            {
                string[] names = new string[fProps != null ? fProps.Count : 0];
                if (fProps != null)
                {
                    fProps.Keys.CopyTo(names, 0);
                }
                return names;
            }
        }

        /// <summary>  Returns the SourceId filters in effect for this Mappings instance as a
        /// comma-delimited string.
        /// 
        /// </summary>
        /// <returns> A comma-delimited string of the SourceId in the filter
        /// </returns>
        public string SourceIdFilterString
        {
            get
            {
                StringBuilder b = new StringBuilder();
                if (fSourceIds != null)
                {
                    for (int i = 0; i < fSourceIds.Length; i++)
                    {
                        if (b.Length > 0)
                        {
                            b.Append(",");
                        }
                        b.Append(fSourceIds[i]);
                    }
                }

                return b.ToString();
            }
        }

        /// <summary>  Returns the Zone ID filters in effect for this Mappings instance as a
        /// comma-delimited string.
        /// 
        /// </summary>
        /// <returns> A comma-delimited string of the SourceId in the filter
        /// </returns>
        public string ZoneIdFilterString
        {
            get
            {
                StringBuilder b = new StringBuilder();
                if (fZoneIds != null)
                {
                    for (int i = 0; i < fZoneIds.Length; i++)
                    {
                        if (b.Length > 0)
                        {
                            b.Append(",");
                        }
                        b.Append(fZoneIds[i]);
                    }
                }

                return b.ToString();
            }
        }

        /// <summary>  Returns the SIF Version filters in effect for this Mappings instance as a
        /// comma-delimited string.
        /// 
        /// </summary>
        /// <returns> A comma-delimited string of the SourceId in the filter
        /// </returns>
        public string SIFVersionFilterString
        {
            get
            {
                StringBuilder b = new StringBuilder();
                if (fSifVersions != null)
                {
                    for (int i = 0; i < fSifVersions.Length; i++)
                    {
                        if (b.Length > 0)
                        {
                            b.Append(",");
                        }
                        b.Append(fSifVersions[i].ToString());
                    }
                }

                return b.ToString();
            }
        }

        //  These are not declared final so vendors can change them at runtime if needed
        public static string XML_MAPPINGS = "mappings";
        public static string XML_OBJECT = "object";
        public static string XML_FIELD = "field";
        public static string XML_VALUESET = "valueset";


        /// <summary>  The parent Mappings object</summary>
        protected internal Mappings fParent;

        /// <summary>  Child Mappings objects keyed by ID</summary>
        protected internal IDictionary<String, Mappings> fChildren = null;

        /// <summary>  Optional DOM XmlElement associated with this Mappings</summary>
        protected internal XmlElement fNode;

        /// <summary>  The ID of this Mappings object</summary>
        protected internal string fId;

        /// <summary>  SourceId Filter. A list of SourceIds that define the SIF Agents to which
        /// this mapping applies. When null, this Mappings object applies to all
        /// SourceIds.
        /// </summary>
        protected internal string[] fSourceIds;

        /// <summary>  ZoneId Filter. A list of ZoneIds that define the Zones to which this
        /// mapping applies. When null, this Mappings object applies to all ZoneIds.
        /// </summary>
        protected internal string[] fZoneIds;

        /// <summary>  SifVersion Filter. A list of SifVersion instances that define the versions
        /// of SIF to which this mapping applies. When null, this Mappings object
        /// applies to all versions of SIF.
        /// </summary>
        protected internal SifVersion[] fSifVersions;

        /// <summary>  ObjectMappings. Each entry is an ObjectMapping keyed by object name.</summary>
        protected internal Dictionary<String, ObjectMapping> fObjRules = null;

        /// <summary>  ValueSetMappings. Each entry is a ValueSet keyed by a unique string ID.</summary>
        protected internal Dictionary<String, ValueSet> fValueSets = null;

        /// <summary>  User-defined properties for this Mappings object</summary>
        protected internal Dictionary<String, String> fProps = null;


        /// <summary>  Constructs the root-level Mappings container</summary>
        public Mappings()
            : this(null, null, null, null, null)
        {
        }

        /// <summary>  Constructs a child Mappings object with no filters.
        /// 
        /// </summary>
        /// <param name="parent">The parent Mappings object
        /// </param>
        /// <param name="id">A unique string ID for this Mappings object
        /// </param>
        public Mappings(Mappings parent,
                        string id)
            : this(parent, id, null, null, null)
        {
        }

        /// <summary>  Constructs a child Mappings object with optional filters.
        /// 
        /// </summary>
        /// <param name="parent">The root Mappings object
        /// </param>
        /// <param name="id">A unique identifier for this Mappings object
        /// </param>
        /// <param name="sourceIdFilter">A comma-delimited list of SourceIds or null if no
        /// SourceId filter should be applied to this Mappings object
        /// </param>
        /// <param name="zoneIdFilter">A comma-delimited list of ZoneIds or null if no
        /// SourceId filter should be applied to this Mappings object
        /// </param>
        /// <param name="sifVersionFilter">A comma-delimited list of SIF Versions, in the
        /// form "1.0r1", or null if no SIF Version filter should be applied to
        /// this Mappings object
        /// </param>
        public Mappings(Mappings parent,
                        string id,
                        string sourceIdFilter,
                        string zoneIdFilter,
                        string sifVersionFilter)
        {
            fParent = parent;
            fId = id;
            SetSourceIdFilter(sourceIdFilter);
            SetZoneIdFilter(zoneIdFilter);
            SetSIFVersionFilter(sifVersionFilter);
        }

        /// <summary>  Creates a copy of this Mappings object and adds the copy as a child of
        /// the specified parent. Note the root Mappings container cannot be copied. 
        /// If this method is called on the root an exception is raised.
        /// 
        /// This method performs a "deep copy", such that a copy is made of each 
        /// child Mappings, ObjectMapping, FieldMapping, ValueSet, and Property 
        /// instance.
        /// 
        /// </summary>
        /// <param name="newParent">The parent Mappings instance
        /// </param>
        /// <returns> A "deep copy" of this root Mappings object
        /// 
        /// </returns>
        /// <exception cref="OpenADK.Library.Tools.Mapping.AdkMappingException"> AdkMappingException thrown if this method is not called on the
        /// root Mappings container
        /// </exception>
        public Mappings Copy(Mappings newParent)
        {
            if (IsRoot())
            {
                throw new AdkMappingException
                    ("Mappings.copy cannot be called on the root Mappings container", null);
            }

            //	Create a new Mappings instance
            Mappings m = new Mappings(newParent, fId);

            //	Copy the DOM XmlElement if there is one
            if (fNode != null && newParent.fNode != null)
            {
                m.fNode = (XmlElement) newParent.fNode.OwnerDocument.ImportNode( fNode, false );
            }
             newParent.AddChild( m );

            //	Copy the filters
            m.SetSourceIdFilter(SourceIdFilterString);
            m.SetZoneIdFilter(ZoneIdFilterString);
            m.SetSIFVersionFilter(SIFVersionFilterString);

            //  Copy all Mappings children
            if (fChildren != null)
            {
                foreach (string key in fChildren.Keys)
                {
                    Mappings ch = fChildren[key];
                    m.AddChild(ch.Copy(m));
                }
            }

            //  Copy all ObjectMapping children
            if (fObjRules != null)
            {
                foreach (string key in fObjRules.Keys)
                {
                    ObjectMapping ch = fObjRules[key];
                    ObjectMapping copy = ch.Copy(m);
                    m.AddRules(copy, false);
                    //				if( m.fNode != null )
                    //					m.fNode.AppendChild( copy.fNode );
                }
            }

            //  Copy fValueSets
            if (fValueSets != null)
            {
                foreach (string key in fValueSets.Keys)
                {
                    ValueSet vs = fValueSets[key];
                    m.AddValueSet(vs.Copy(m));
                }
            }

            //	Copy properties
            if (fProps != null)
            {
                foreach (string key in fProps.Keys)
                {
                    string val = fProps[key];
                    m.SetProperty(key, val);
                }
            }

            return m;
        }

        /// <summary>  Determines if this is the root Mappings container. Applications may call
        /// the <c>select</c> and <c>map</c> methods on the root
        /// container only.
        /// 
        /// </summary>
        /// <returns> true if this is the root Mappings container, otherwise false
        /// </returns>
        public bool IsRoot()
        {
            return fParent == null;
        }

        /// <summary>  Gets the root Mappings container.</summary>
        public Mappings GetRoot()
        {
            Mappings parent = this;
            while (parent.fParent != null)
            {
                parent = parent.fParent;
            }

            return parent;
        }

        /// <summary>  Adds a Mappings child</summary>
        public void AddChild(Mappings m)
        {
            if (fChildren == null)
            {
                fChildren = new Dictionary<String, Mappings>();
            }

            fChildren[m.Id] = m;

            //	If there is a DOM XmlElement associated with this Mappings, and the new child
            //	also has a XmlElement, attach it
            if (fNode != null && m.fNode != null)
            {
                fNode.AppendChild(m.fNode);
            }
        }

        /// <summary> 	Create a Mappings child </summary>
        public Mappings CreateChild(string id)
        {
            Mappings m = new Mappings(this, id);

            try
            {
                if (fNode != null)
                {
                    m.fNode = m.ToDom(fNode.OwnerDocument);
                }
            }
            catch (Exception se)
            {
                throw new AdkMappingException(se.Message, null, se);
            }

            AddChild(m);

            return m;
        }

        /// <summary>  Removes a Mappings child</summary>
        public void RemoveChild(Mappings m)
        {
            if (fChildren != null)
            {
                fChildren.Remove(m.Id);
            }
            //	If there is a DOM XmlElement associated with this Mappings, and the new child
            //	also has a XmlElement, detatch it
            if (fNode != null && m.fNode != null)
            {
                fNode.RemoveChild(m.fNode);
            }
        }

        /// <summary>  Sets a SourceId filter on this Mappings object.
        /// 
        /// </summary>
        /// <param name="filter">A comma-delimited list of SourceIds that define the agents
        /// this Mappings object applies to. If the filter includes an asterisk,
        /// the Mappings object will have no SourceId filter regardless of whether
        /// any other SourceIds are specified in the <i>filter</i> string.
        /// </param>
        public void SetSourceIdFilter(string filter)
        {
            fSourceIds = BuildFilterString(filter);

            if (fNode != null)
            {
                fNode.SetAttribute("sourceId", filter);
            }
        }

        /// <summary>  Sets a ZoneId filter on this Mappings object.
        /// 
        /// </summary>
        /// <param name="filter">A comma-delimited list of ZoneIds that define the zones
        /// this Mappings object applies to. If the filter includes an asterisk,
        /// the Mappings object will have no ZoneId filter regardless of whether
        /// any other ZoneIds are specified in the <i>filter</i> string.
        /// </param>
        public void SetZoneIdFilter(string filter)
        {
            fZoneIds = BuildFilterString(filter);

            if (fNode != null)
            {
                fNode.SetAttribute("zoneId", filter);
            }
        }

        /// <summary>  Sets a SifVersion filter on this Mappings object.
        /// 
        /// </summary>
        /// <param name="filter">A comma-delimited list of SifVersion strings, in the form
        /// "1.0r1", that define the zones this Mappings object applies to. If
        /// the filter includes an asterisk, the Mappings object will have no
        /// SifVersion filter regardless of whether any other SIFVersions are
        /// specified in the <i>filter</i> string.
        /// </param>
        public void SetSIFVersionFilter(string filter)
        {
            string[] sifVersionFilters = BuildFilterString(filter);
            if (sifVersionFilters != null)
            {
                fSifVersions = new SifVersion[sifVersionFilters.Length];
                for (int a = 0; a < sifVersionFilters.Length; a++)
                {
                    fSifVersions[a] = SifVersion.Parse(sifVersionFilters[a]);
                }
            }
            else
            {
                fSifVersions = null;
            }
            if (fNode != null)
            {
                fNode.SetAttribute("sifVersion", filter);
            }
        }

        /// <summary>  Set a user-defined property.
        /// 
        /// </summary>
        /// <param name="name">The property name
        /// </param>
        /// <param name="val">The property value
        /// </param>
        public void SetProperty(string name,
                                string val)
        {
            if (fProps == null)
            {
                fProps = new Dictionary<String, String>();
            }

            if (name != null)
            {
                fProps[name] = val;
            }

            if (fNode != null)
            {
                XmlUtils.SetProperty(fNode, name, val);
            }
        }

        /// <summary>  Gets a user-defined property.
        /// 
        /// </summary>
        /// <param name="name">The property name
        /// </param>
        /// <param name="defaultValue">The value to return if the property is not defined
        /// 
        /// </param>
        /// <returns> The value of the property
        /// </returns>
        public string GetProperty(string name,
                                  string defaultValue)
        {
            if (fProps == null || name == null)
            {
                return defaultValue;
            }
            string s = fProps[name];

            return s != null ? s : defaultValue;
        }

        /// <summary>  Determines if a user-defined property is currently set on this Mappings instance.
        /// 
        /// </summary>
        /// <param name="name">The property name
        /// 
        /// </param>
        /// <returns> true if the property is set; otherwise false
        /// </returns>
        public bool HasProperty(string name)
        {
            return fProps != null && fProps.ContainsKey(name);
        }

        /// <summary>  Populate the Mappings hierarchy from an XML Document.
        /// 
        /// This method can only be called on the root Mappings object or an exception 
        /// is raised. It reads all &lt;mapping&gt; elements from the Document to build
        /// the Mappings hierarchy.
        /// 
        /// </summary>
        /// <param name="doc">A DOM <i>Document</i> that defines one or more &lt;mappings&gt;
        /// elements from which to populate the Mappings hierarchy. Note the root
        /// Mappings object is a object and therefore not represented by 
        /// a XmlElement in this Document.
        /// 
        /// </param>
        /// <param name="parent">A DOM <i>XmlElement</i> in the source document that should be
        /// considered the parent XmlElement of this Mappings object. This parameter is
        /// required because the root Mappings object is a object that 
        /// is not represented in the DOM graph, so some other node usually 
        /// serves as its parent (e.g. the &lt;agent&gt; node in an AgentConfig
        /// configuration file)
        /// 
        /// </param>
        /// <exception cref="OpenADK.Library.Tools.Cfg.AdkConfigException"> AdkConfigException is thrown if a required element or attribute
        /// is missing or has an invalid value
        /// 
        /// </exception>
        /// <exception cref="OpenADK.Library.Tools.Mapping.AdkMappingException"> AdkMappingException is thrown if this method is called on
        /// a Mappings instance other than the root Mappings container
        /// </exception>
        public void Populate(XmlDocument doc,
                             XmlElement parent)
        {
            if (fParent != null)
            {
                throw new AdkMappingException
                    ("Mappings.populate can only be called on the root Mappings container", null);
            }

            fNode = parent;
            Populate(doc.DocumentElement, this);
        }

        protected internal void Populate(XmlElement node,
                                         Mappings parent)
        {
            // TODO: We should probably be using a GetElementsby name query here
            // I think the java implementation might have been doing a recursive search here
            foreach (XmlElement n in new XmlUtils.XmlElementEnumerator(node))
            {
                if (n.Name.ToUpper() == XML_MAPPINGS.ToUpper())
                {
                    //  Get the ID
                    string id = n.GetAttribute("id");

                    //  Create a new child Mappings object
                    Mappings mappings = new Mappings(parent, id);
                    mappings.XmlElement = n;
                    if (parent.fChildren == null)
                    {
                        parent.fChildren = new Dictionary<String, Mappings>();
                    }
                    parent.fChildren[mappings.Id] = mappings;

                    //  Set a SifVersion filter if present
                    string ver = n.GetAttribute("sifVersion");
                    if (ver.Trim().Length > 0)
                    {
                        mappings.SetSIFVersionFilter(ver);
                    }

                    //  Set a ZoneId filter if present
                    string zoneIds = n.GetAttribute("zoneId");
                    if (zoneIds.Trim().Length > 0)
                    {
                        mappings.SetZoneIdFilter(zoneIds);
                    }

                    //  Set a SourceId filter if present
                    string sourceIds = n.GetAttribute("sourceId");
                    if (sourceIds.Trim().Length > 0)
                    {
                        mappings.SetSourceIdFilter(sourceIds);
                    }

                    //  Populate the Mappings object with rules
                    Populate(n, mappings);
                }
                else
                {
                    if (n.Name.ToUpper() == XML_OBJECT.ToUpper())
                    {
                        if (n.ParentNode.Name == XML_MAPPINGS)
                        {
                            string obj = n.GetAttribute(XML_OBJECT);
                            if (obj == null)
                            {
                                throw new AdkConfigException
                                    ("<object> element must have an object attribute");
                            }

                            ObjectMapping om = new ObjectMapping(obj);
                            om.XmlElement = n;
                            parent.AddRules(om, false);
                            PopulateObject(n, om);
                        }
                    }
                    else
                    {
                        if (n.Name.ToUpper() == "PROPERTY")
                        {
                            if (n.ParentNode.Name == XML_MAPPINGS)
                            {
                                parent.SetProperty
                                    (n.GetAttribute(AdkXmlConstants.Property.NAME),
                                     n.GetAttribute(AdkXmlConstants.Property.VALUE));
                            }
                        }
                        else
                        {
                            if (n.Name.ToUpper() == "VALUESET")
                            {
                                if (n.ParentNode.Name == XML_MAPPINGS)
                                {
                                    ValueSet set_Renamed =
                                        new ValueSet
                                            (n.GetAttribute("id"), n.GetAttribute("title"), n);
                                    parent.AddValueSet(set_Renamed);
                                    PopulateValueSet(n, set_Renamed);
                                }
                            }
                            else
                            {
                                Populate(n, parent);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>  Parse the <c>&lt;field&gt;</c> children of an <c>&lt;object&gt;</c> element.
        /// 
        /// </summary>
        /// <param name="element">A DOM <i>XmlElement</i> encapsulating an <c>&lt;object&gt;</c> element
        /// </param>
        /// <param name="parent">The parent ObjectMapping instance
        /// </param>
        protected internal void PopulateObject(XmlElement element,
                                               ObjectMapping parent)
        {
            foreach (XmlElement n in new XmlUtils.XmlElementEnumerator(element))
            {
                if (n.Name.ToUpper() == XML_FIELD.ToUpper())
                {
                    FieldMapping fm = FieldMapping.FromXml( parent, n );
                    parent.AddRule( fm, false );
                }
            }
        }

        /// <summary>  Parse the <c>&lt;value&gt;</c> children of a <c>&lt;valueset&gt;</c>
        /// element.
        /// 
        /// </summary>
        /// <param name="element">A DOM <i>XmlElement</i> encapsulating a <c>&lt;valueset&gt;</c> element
        /// </param>
        /// <param name="parent">The parent ValueSet instance
        /// </param>
        protected internal void PopulateValueSet(XmlElement element,
                                                 ValueSet parent)
        {
            foreach (XmlElement n in new XmlUtils.XmlElementEnumerator(element))
            {
                if( String.Compare( "value", n.Name, true ) == 0 )
                {
                    String name = GetAttribute( n, "name");
                    if(name==null)
                    {
                        throw new AdkConfigException( "<value> name= attribute is required" );
                    }
                    
                    String sifValue = n.InnerText;
                    if (String.IsNullOrEmpty(sifValue))
                    {
                        throw new AdkConfigException("<value>SIF Value is Required</value>");
                    }
                    String title = n.GetAttribute("title");

                    parent.Define(name, sifValue, title, n);

                    String def = n.GetAttribute( "default");
                    if(!string.IsNullOrEmpty( def ) )
                    {
                        String ifNull = n.GetAttribute( "ifnull");
                        bool renderIfDefault = false;
                        if (!String.IsNullOrEmpty( ifNull))
                        { // cool phrase!
                            renderIfDefault = String.Compare( ifNull, "default", true) == 0;
                        }
                        bool inboundDefault = false;
                        bool outboundDefault = false;
                        if ( String.Compare( def, "both", true ) == 0 || String.Compare( def, "true", true ) == 0 )
                        {
                            inboundDefault = true;
                            outboundDefault = true;
                        }
                        if (!inboundDefault && String.Compare(def, "inbound", true) == 0 )
                        {
                            inboundDefault = true;
                        }
                        if (!outboundDefault && String.Compare(def, "outbound", true) == 0)
                        {
                            outboundDefault = true;
                        }

                        if (inboundDefault)
                        {
                            parent.SetAppDefault(name, renderIfDefault);
                        }
                        if (outboundDefault)
                        {
                            parent.SetSifDefault(sifValue, renderIfDefault);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Gets the attribute value or returns null if the attribute is undefined
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private string GetAttribute( XmlElement element,  string attr )
        {
           if( element == null )
           {
               return null;
           }
            XmlAttribute attrNode = element.GetAttributeNode( attr );
            if( attr == null )
            {
                return null;
            }
            return attrNode.Value;
        }

        /// <summary>  Gets the Mappings object with the specified ID.
        /// 
        /// Unlike the <c>select</c> methods, <c>getMappings</c> should
        /// be called when the agent knows the specific Mappings object it is going
        /// to use to perform a mapping operation. Conversely, the <c>select</c>
        /// methods select the most appropriate Mappings object based on the ZoneId,
        /// SourceId, and SifVersion values passed to those methods.
        /// 
        /// </summary>
        /// <param name="id">The unique string identifier of the Mappings object to return.
        /// 
        /// </param>
        /// <returns> The Mappings instance with the specified ID, or <c>null</c>
        /// if no match was found.
        /// 
        /// </returns>
        /// <exception cref="AdkMappingException">  thrown if this method is not called on the
        /// root Mappings container
        /// </exception>
        public Mappings GetMappings(string id)
        {
            Mappings[] ch = Children;
            for (int i = 0; i < ch.Length; i++)
            {
                if (ch[i].fId != null && ch[i].fId.Equals(id))
                {
                    return ch[i];
                }
            }

            return null;
        }

        /// <summary>  Selects the appropriate Mappings object to use for <c>map</c>
        /// operations. Call this method on the root Mappings object to obtain a
        /// Mappings instance, then call its <c>map</c> method to perform a
        /// mapping operation.
        /// 
        /// 
        /// The selection process is as follows:
        /// 
        /// <list type="bullet">
        /// <item><term>
        /// All children of this Mappings are evaluated as a group in a
        /// <b>flat list</b>. The group is ordered by "restrictiveness". The
        /// more filters defined for a Mappings the more restrictive it is
        /// considered. Thus, a Mappings that defines a ZoneId, SourceId,
        /// and SifVersion filter will be evaluated before a less-restrictive
        /// Mappings that defines only a SourceId filter. Because children
        /// inherit the filters of their parent, a child can never be less
        /// restrictive than its parent. Two or more Mappings with equal
        /// restrictiveness are evaluated in natural order, so care should be
        /// taken to not organize child Mappings objects with equal filters
        /// or the first one present will always be selected.
        /// <br/><br/>
        /// </term></item>
        /// <item><term>
        /// The ordered Mappings are evaluated sequentially. For each, the
        /// ZoneId, SourceId, and Version are compared against any filters
        /// in effect for the Mappings instance. Any Mappings that does not
        /// pass the criteria is eliminated from the list of candidates.
        /// <br/><br/>
        /// </term></item>
        /// <item><term>
        /// If no child Mappings pass the selection process, the Mappings
        /// on which this method is called is returned (i.e. self is returned)
        /// This ensures that this method always returns a non-null Mappings
        /// instance to the caller.
        /// <br/><br/>
        /// </term></item>
        /// </list>
        /// 
        /// </summary>
        /// <param name="zoneId">Restricts the selection of a Mappings object to only
        /// those that allow this ZoneId. This parameter may be null.
        /// </param>
        /// <param name="sourceId">Restricts the selection of a Mappings object to only
        /// those that allow this SourceId. This parameter may be null.
        /// </param>
        /// <param name="version">Restricts the selection of a Mappings object to only
        /// those that allow this version of SIF. This parameter may be null.
        /// 
        /// </param>
        /// <returns> The first Mappings child instance that matches the criteria or
        /// null if no match was found
        /// </returns>
        public Mappings Select(string zoneId,
                               string sourceId,
                               SifVersion version)
        {
            if (fParent == null)
            {
                throw new AdkMappingException
                    ("Mappings.select cannot be called on the root-level Mappings container; it can only be called on one of the root's children",
                     null);
            }
            if (fParent != GetRoot())
            {
                throw new AdkMappingException
                    ("Mappings.select can only be called on a top-level Mappings object (i.e. a child of the root Mappings container)",
                     null);
            }

            //  Optimization: If no children, return self
            if (ChildCount == 0)
            {
                return this;
            }

            //  Group all of the child Mappings, if any, in a flat list
            List<Mappings> v = new List<Mappings>();
            v.Add(this);
            GroupDescendents(this, v);

            //  Order all Mappings by "restrictiveness"
            Candidate[] candidates = new Candidate[v.Count];
            for (int i = 0; i < candidates.Length; i++)
            {
                candidates[i] = new Candidate(v[i]);
            }


            Array.Sort(candidates,
                       delegate(Candidate c1, Candidate c2) { return Comparer<int>.Default.Compare(c1.restrictiveness, c2.restrictiveness); });


            Mappings selection = this;
            int highScore = 0;

            for (int i = 0; i < candidates.Length; i++)
            {
                int score = 0;

                int eval;
                if (zoneId != null)
                {
                    eval = candidates[i].fMapping.AllowsZoneId(zoneId);
                    if (eval == -1)
                    {
                        continue;
                    }
                    score += eval;
                }

                if (sourceId != null)
                {
                    eval = candidates[i].fMapping.AllowsSourceId(sourceId);
                    if (eval == -1)
                    {
                        continue;
                    }
                    score += eval;
                }

                if (version != null)
                {
                    eval = candidates[i].fMapping.AllowsVersion(version);
                    if (eval == -1)
                    {
                        continue;
                    }
                    score += eval;
                }

                if (score > highScore)
                {
                    highScore = score;
                    selection = candidates[i].fMapping;
                }
            }

            return selection;
        }


        /// <summary>
        /// Selects an appropriate MappingsContext object to use for an inbound <c>Map</c>
        /// operation. Call this method on the root Mappings object to obtain a
        /// MappingsContext instance, then call its <c>Map</c> method to perform an inbound
        /// mapping operation.
        /// </summary>
        /// <param name="elementDef">The ElementDef of the Element being mapped to</param>
        /// <param name="version">The SIFVersion to use when evaluating mappings XPaths.</param>
        /// <param name="zoneId">The ID of the zone that this mappings operation is being performed on </param>
        /// <param name="sourceId">The Source ID of the destination agent that this mappings is being performed for</param>
        /// <returns>a MappingsContext that can be used for evaluating mappings</returns>
        /// <exception cref="AdkMappingException"/>
        public MappingsContext SelectInbound(
            IElementDef elementDef,
            SifVersion version,
            String zoneId,
            String sourceId)
        {
            return SelectContext(MappingDirection.Inbound, elementDef, version, zoneId, sourceId);
        }


        /// <summary>
        /// Selects an appropriate MappingsContext object to use for an inbound <c>Map</c>
        /// operation. Call this method on the root Mappings object to obtain a
        /// MappingsContext instance, then call its <c>Map</c> method to perform an inbound
        /// mapping operation.
        /// </summary>
        /// <param name="elementDef">The ElementDef of the Element being mapped to</param>
        /// <param name="message">The Message being mapped from</param>
        /// <returns>a MappingsContext that can be used for evaluating mappings</returns>
        /// <exception cref="AdkMappingException"/>
        public MappingsContext SelectInbound(IElementDef elementDef, SifMessageInfo message)
        {
            return
                SelectContext(MappingDirection.Inbound, elementDef, message.SifVersion, message.Zone.ZoneId,
                              message.SourceId);
        }


        /// <summary>
        ///  Selects an appropriate MappingsContext object to use for an outbound <c>Map</c>
        ///  operation. Call this method on the root Mappings object to obtain a
        ///  MappingsContext instance, then call its <c>map</c> method to perform an outbound
        /// mapping operation.
        /// </summary>
        /// <param name="elementDef">The ElementDef of the Element being mapped to</param>
        /// <param name="version">The SIFVersion to use when evaluating mappings XPaths.</param>
        /// <param name="zoneId">The ID of the zone that this mappings operation is being performed on </param>
        /// <param name="sourceId">The Source ID of the destination agent that this mappings is being performed for</param>
        /// <returns>a MappingsContext that can be used for evaluating mappings</returns>
        /// <exception cref="AdkMappingException"/>
        public MappingsContext SelectOutbound(
            IElementDef elementDef,
            SifVersion version,
            String zoneId,
            String sourceId)
        {
            return SelectContext(MappingDirection.Outbound, elementDef, version, zoneId, sourceId);
        }


        /// <summary>
        ///  Selects an appropriate MappingsContext object to use for an outbound <c>Map</c>
        ///  operation. Call this method on the root Mappings object to obtain a
        ///  MappingsContext instance, then call its <c>map</c> method to perform an outbound
        /// mapping operation.
        /// </summary>
        /// <param name="elementDef">The ElementDef of the Element being mapped to</param>
        /// <param name="message">The Message being mapped from</param>
        /// <returns>a MappingsContext that can be used for evaluating mappings</returns>
        /// <exception cref="AdkMappingException"/>
        public MappingsContext SelectOutbound(IElementDef elementDef, SifMessageInfo message)
        {
            return
                SelectContext(MappingDirection.Outbound, elementDef, message.LatestSIFRequestVersion,
                              message.Zone.ZoneId, message.SourceId);
        }


        /**
	 *  Selects an appropriate MappingsContext object to use for <code>map</code>
	 *  operations. Call this method on the root Mappings object to obtain a
	 *  MappingsContext instance, then call its <code>map</code> method to perform a
	 *  mapping operation.
	 *  
	 * @param direction The MappingsDirection that this mapping will use
	 * @param elementDef The ElementDef of the Element being mapped to
	 * @param version The SIFVersion to use when evaluating mappings XPaths.
	 * @param zoneId The ID of the zone that this mappings operation is being performed on 
	 * @param sourceId The Source ID of the destination agent that this mappings is being performed for
	 * @return a MappingsContext that can be used for evaluating mappings
	 * @throws ADKMappingException
	 */

        private MappingsContext SelectContext(
            MappingDirection direction,
            IElementDef elementDef,
            SifVersion version,
            String zoneId,
            String sourceId)
        {
            // Select the mappings instance
            Mappings m = Select(zoneId, sourceId, version);
            // Create a mappings context, that retains the filters and metadata associated
            // with this mappings operation
            MappingsContext mc = MappingsContext.Create(m, direction, version, elementDef);
            return mc;
        }


        /// <summary>  Recursively adds all children of the Mappings to the supplied Vector</summary>
        private void GroupDescendents(Mappings m,
                                      List<Mappings> v)
        {
            if (m.fChildren != null)
            {
                foreach (Mappings ch in m.fChildren.Values)
                {
                    v.Add(ch);
                    GroupDescendents(ch, v);
                }
            }
        }

        /// <summary>  Produce a table of field values from a SIF Data Object.
        /// 
        /// This <c>map</c> method populates the supplied IDictionary with element
        /// or attribute values from the SifDataObject by evaluating all field rules
        /// defined by this Mappings object for the associated SIF Data Object type.
        /// Field rules are only evaluated if no entry currently exists in the
        /// IDictionary. Consequently, the caller may pre-load the IDictionary with known
        /// field values if possible and they will not be overridden by the mapping
        /// process.
        /// 
        /// 
        /// This method is intended to obtain element and attribute values from a
        /// SifDataObject <i>consumed</i> by the agent when processing SIF
        /// Events or SIF Responses. In contrast, the other form of the <c>map</c>
        /// method is intended to populate a new SifDataObject instance when an
        /// agent is <i>publishing</i> objects to a zone.
        /// 
        /// 
        /// To use this method,
        /// 
        /// <ol>
        /// <item><term>
        /// Create a IDictionary and optionally populate it with known field
        /// values that will not be subject to the mapping process. If pre-loading
        /// the IDictionary, the key of each entry should be the local
        /// application-defined field name and the value should be the string
        /// value of that field. Any field added to the IDictionary before calling
        /// this method will not be subject to mapping rules.
        /// </term></item>
        /// <item><term>
        /// Call this <c>map</c> method, passing the SifDataObject
        /// instance to retrieve field values from for insertion into the
        /// IDictionary. The method first looks up the ObjectMapping instance
        /// corresponding to the SIF Data Object type. If no ObjectMapping
        /// has been defined for the object type, no action is taken and the
        /// method returns successfully without exception. Otherwise, all
        /// field rules defined by the ObjectMapping are evaluated in order.
        /// If a rule evaluates successfully, the corresponding element or
        /// attribute value will be inserted into the IDictionary. A rule will
        /// not be evaluated if the associated field already exists in the
        /// IDictionary.
        /// </term></item>
        /// </ol>
        /// 
        /// </summary>
        /// <param name="dataObject">The SifDataObject from which to retrieve element and
        /// attribute values from when performing the mapping operation.
        /// 
        /// </param>
        /// <param name="results">A IDictionary to receive the results of the mapping, where
        /// each entry in the map is keyed by the local application-defined name
        /// of a field and the value is the text value of the corresponding
        /// element or attribute in the SifDataObject.
        /// 
        /// </param>
        /// <exception cref="AdkMappingException">  thrown if an error occurs while evaluating
        /// a field rule
        /// </exception>
        public void MapInbound(SifDataObject dataObject,
                               IFieldAdaptor results)
        {
            MapInbound(dataObject, results, Adk.SifVersion);
        }

        /// <summary>  Produce a table of field values from a SIF Data Object.
        /// 
        /// This form of the <c>map</c> method allows the client to specify 
        /// whether it is performing an inbound or outbound mapping operation.
        /// Currently, the direction flag is used to invoke automatic ValueSet
        /// translations on fields that have a <i>ValueSet</i> attribute.
        /// 
        /// </summary>
        /// <param name="data">The SifDataObject from which to retrieve element and
        /// attribute values from when performing the mapping operation.
        /// 
        /// </param>
        /// <param name="results">A IDictionary to receive the results of the mapping, where
        /// each entry in the map is keyed by the local application-defined name
        /// of a field and the value is the text value of the corresponding
        /// element or attribute in the SifDataObject.</param>
        /// <param name="version">The SifVersion associated with the mapping operation. 
        /// For inbound SIF_Event and SIF_Response messages, this value should 
        /// be obtained by calling <c>getSIFVersion</c> on the 
        /// <i>SifMessageInfo</i> parameter passed to the message handler. For 
        /// inbound SIF_Request messages, it should be obtained by calling the
        /// <c>SifMessageInfo.getSIFRequestVersion</c> method. For 
        /// outbound messages, this value should be obtained by calling
        /// <c>Adk.getSIFVersion</c> to get the version of SIF the class
        /// framework was initialized with. Note when this parameter is 
        /// <c>null</c>, no SIF Version filtering will be applied to 
        /// field mapping rules.
        /// 
        /// </param>
        /// <exception cref="AdkMappingException">  thrown if an error occurs while evaluating
        /// a field rule
        /// 
        /// @since Adk 1.5
        /// </exception>
        public void MapInbound(SifDataObject data,
                               IFieldAdaptor results,
                               SifVersion version)
        {
            ObjectMapping om = GetRules(data.ElementDef.Tag(data.SifVersion), true);
            if (om != null)
            {
                SifXPathContext xpathContext = SifXPathContext.NewSIFContext(data, version);
                if (results is IXPathVariableLibrary)
                {
                    xpathContext.AddVariables("", (IXPathVariableLibrary) results);
                }
                IList<FieldMapping> list = GetRulesList(om, version, MappingDirection.Inbound);
                MapInbound(xpathContext, results, data, list, version);
            }
        }

        private IList<FieldMapping> GetRulesList(ObjectMapping om, SifVersion version, MappingDirection direction)
        {
            IList<FieldMapping> list = om.GetRulesList(true);
            // Remove any items that should be filtered out

            for (int a = list.Count - 1; a > -1; a--)
            {
                MappingsFilter filt = list[a].Filter;
                //	Filter out this rule?
                if (filt != null)
                {
                    if (!filt.EvalDirection(direction) ||
                        !filt.EvalVersion(version))
                    {
                        list.RemoveAt(a);
                    }
                }
            }
            return list;
        }

        internal void MapInbound(
            SifXPathContext xpathContext,
            IFieldAdaptor results,
            SifElement inboundObject,
            ICollection<FieldMapping> fields,
            SifVersion version)
        {
#if PROFILED
		if( BuildOptions.PROFILED ){
			ProfilerUtils.profileStart( String.valueOf( com.OpenADK.sifprofiler.api.OIDs.ADK_INBOUND_TRANSFORMATIONS ), inboundObject.getElementDef(), null );
		}
#endif

            FieldMapping lastRule = null;
            try
            {
                foreach (FieldMapping rule in fields)
                {
                    lastRule = rule;
                    if (!results.HasField(rule.FieldName))
                    {
                        SifSimpleType val = rule.Evaluate(xpathContext, version, true);
                        if (val != null)
                        {
                            if (rule.ValueSetID != null &&
                                val is SifString)
                            {
                                String currentValue = val.ToString();
                                //	Perform automatic ValueSet translation
                                // TT 199. Perform a more detailed valueset translation. 
                                // If there is a default value set, use it if there is
                                // no match found in the value set
                                ValueSet vs = GetValueSet(rule.ValueSetID, true);
                                if (vs != null)
                                {
                                    currentValue = vs.TranslateReverse(currentValue, rule.DefaultValue);
                                }
                                val = new SifString(currentValue);
                            }
                            results.SetSifValue(rule.FieldName, val, rule);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (lastRule != null)
                {
                    throw new AdkMappingException(
                        "Unable to evaluate field rule: " + lastRule.fRule + " : " + e.Message, null, e);
                }
                throw new AdkMappingException(e.ToString(), null, e);
            }
        }


        /// <summary>  Populate a SifDataObject from values in the supplied IDictionary by evaluating
        /// all field rules for the associated SIF Data Object type. For each key in
        /// the IDictionary, the corresponding field rule is evaluated to assign the
        /// IDictionary value to the appropriate element or attribute in the SifDataObject.
        /// If a field is not represented in the IDictionary, its associated rule will
        /// not be evaluated.
        /// 
        /// This method is intended to populate a new SifDataObject instance when an
        /// agent is <i>publishing</i> objects to a zone. The other form of the
        /// <c>map</c> method is intended to obtain element and attribute values
        /// from a SifDataObject <i>consumed</i> by the agent when processing SIF
        /// Events or SIF Responses.
        /// 
        /// To use this method,
        /// 
        /// <ol>
        /// <item><term>
        /// Create a IDictionary and populate it with all known field values. The
        /// key of each entry must be the local application-defined name of
        /// the field -- the same field name used in field rules -- and the
        /// value is the string value to assign to the corresponding element
        /// or attribute in the SifDataObject when a field rule matches.
        /// </term></item>
        /// <item><term>
        /// Create a SifDataObject instance of the appropriate type (e.g. a
        /// OpenADK.Library.us.Student.StudentPersonal instance if
        /// the mapping will be applied to an incoming &lt;StudentPersonal&gt;
        /// message).
        /// </term></item>
        /// <item><term>
        /// Call this <c>map</c> method to apply all field values in
        /// the IDictionary to corresponding elements and/or attributes in the
        /// SifDataObject. The method first looks up the ObjectMapping
        /// instance corresponding to the SIF Data Object type. If no ObjectMapping
        /// has been defined for the object type, no action is taken and the
        /// method returns successfully without exception. Otherwise, all
        /// field rules defined by the ObjectMapping are evaluated in order.
        /// </term></item>
        /// </ol>
        /// 
        /// </summary>
        /// <param name="adaptor">An IFieldAdaptor that contains field values to assign to the
        /// supplied SifDataObject, where each entry in the map is keyed by the
        /// local application-defined name of a field and the value is the text
        /// value to assign to the corresponding element or attribute of the
        /// SifDataObject.
        /// 
        /// </param>
        /// <param name="dataObject">The SifDataObject to assign field values to
        /// 
        /// </param>
        /// <exception cref="AdkMappingException">  thrown if an error occurs while
        /// evaluating a field rule
        /// </exception>
        public void MapOutbound(IFieldAdaptor adaptor,
                                SifDataObject dataObject)
        {
            MapOutbound(adaptor, dataObject, new DefaultValueBuilder(adaptor));
        }

        /// <summary>  Populate a SifDataObject from values in the supplied IDictionary.
        /// 
        /// This form of the <c>map</c> method that accepts a custom 
        /// <c>ValueBuilder</c> implementation to evaluate value expressions 
        /// in XPath-like query strings. The <c>map</c> method uses the 
        /// DefaultValueBuilder class as its built-in implementation, but you can 
        /// supply your own by calling this method instead.
        /// 
        /// </summary>
        /// <param name="adaptor">An IFieldAdaptor that contains field values to assign to the
        /// supplied SifDataObject, where each entry in the map is keyed by the
        /// local application-defined name of a field and the value is the text
        /// value to assign to the corresponding element or attribute of the
        /// SifDataObject.
        /// 
        /// </param>
        /// <param name="adaptor">An IFieldAdaptor containing field values</param>
        /// <param name="data">The SifDataObject to assign field values to
        /// 
        /// </param>
        /// <param name="valueBuilder">A custom ValueBuilder implementation to evaluate
        /// value expressions in XPath-like query strings
        /// 
        /// </param>
        /// <exception cref="AdkMappingException">  thrown if an error occurs while
        /// evaluating a field rule
        /// </exception>
        public void MapOutbound(IFieldAdaptor adaptor,
                                SifDataObject data,
                                IValueBuilder valueBuilder)
        {
            MapOutbound(adaptor, data, valueBuilder, Adk.SifVersion);
        }


        /// <summary>
        /// Populate a SifDataObject from values in the supplied IDictionary.
        /// </summary>
        /// <param name="adaptor">An IFieldAdaptor that contains field values to assign to the
        /// supplied SifDataObject, where each entry in the map is keyed by the
        /// local application-defined name of a field and the value is the text
        /// value to assign to the corresponding element or attribute of the
        /// SifDataObject.</param>
        /// <param name="dataObject">The SifDataObject to assign field values to</param>
        /// <param name="version">The SifVersion associated with the mapping operation. 
        /// For inbound SIF_Event and SIF_Response messages, this value should 
        /// be obtained by calling <c>getSIFVersion</c> on the 
        /// <i>SifMessageInfo</i> parameter passed to the message handler. For 
        /// inbound SIF_Request messages, it should be obtained by calling the
        /// <c>SifMessageInfo.getSIFRequestVersion</c> method. For 
        /// outbound messages, this value should be obtained by calling
        /// <c>Adk.getSIFVersion</c> to get the version of SIF the class
        /// framework was initialized with. Note when this parameter is 
        /// <c>null</c>, no SIF Version filtering will be applied to 
        /// field mapping rules.</param>
        public void MapOutbound(IFieldAdaptor adaptor,
                                SifDataObject dataObject,
                                SifVersion version)
        {
            MapOutbound(adaptor, dataObject, new DefaultValueBuilder(adaptor), version);
        }

        /// <summary>  Populate a SifDataObject from values in the supplied IDictionary.
        /// 
        /// This form of the <c>map</c> method allows the caller to specify 
        /// whether it is performing an inbound or outbound mapping operation,
        /// as well as the version of SIF associated with the SIF Data Object
        /// that's being mapped. These values are used to filter field mapping rules. 
        /// The direction flag is also used to invoke automatic ValueSet 
        /// translations on fields that have a <i>ValueSet</i> attribute.
        /// 
        /// </summary>
        /// <param name="adaptor">An IFieldAdaptor that contains field values to assign to the
        /// supplied SifDataObject, where each entry in the map is keyed by the
        /// local application-defined name of a field and the value is the text
        /// value to assign to the corresponding element or attribute of the
        /// SifDataObject.
        /// 
        /// </param>
        /// <param name="dataObject">The SifDataObject to assign field values to
        /// 
        /// </param>
        /// <param name="valueBuilder">A custom ValueBuilder implementation to evaluate
        /// value expressions in XPath-like query strings
        /// 
        /// </param>
        /// <param name="version">The SifVersion associated with the mapping operation. 
        /// For inbound SIF_Event and SIF_Response messages, this value should 
        /// be obtained by calling <c>getSIFVersion</c> on the 
        /// <i>SifMessageInfo</i> parameter passed to the message handler. For 
        /// inbound SIF_Request messages, it should be obtained by calling the
        /// <c>SifMessageInfo.getSIFRequestVersion</c> method. For 
        /// outbound messages, this value should be obtained by calling
        /// <c>Adk.getSIFVersion</c> to get the version of SIF the class
        /// framework was initialized with. Note when this parameter is 
        /// <c>null</c>, no SIF Version filtering will be applied to 
        /// field mapping rules.
        /// 
        /// </param>
        /// <exception cref="AdkMappingException">  thrown if an error occurs while
        /// evaluating a field rule</exception>
        public void MapOutbound(IFieldAdaptor adaptor,
                                SifDataObject dataObject,
                                IValueBuilder valueBuilder,
                                SifVersion version)
        {
            ObjectMapping om = GetRules(dataObject.ElementDef.Tag(version), true);
            if (om != null)
            {
                SifXPathContext xpathContext = SifXPathContext.NewSIFContext(dataObject, version);
                if (adaptor is IXPathVariableLibrary)
                {
                    xpathContext.AddVariables("", (IXPathVariableLibrary) adaptor);
                }

                IList<FieldMapping> list = GetRulesList(om, version, MappingDirection.Outbound);
                MapOutbound(xpathContext, adaptor, dataObject, list, valueBuilder, version);
            }
        }

        internal void MapOutbound(
            SifXPathContext context,
            IFieldAdaptor adaptor,
            SifElement dataObject,
            ICollection<FieldMapping> fields,
            IValueBuilder valueBuilder,
            SifVersion version)
        {
#if PROFILED		
		if( BuildOptions.PROFILED ){
			ProfilerUtils.profileStart( String.valueOf( com.OpenADK.sifprofiler.api.OIDs.ADK_OUTBOUND_TRANSFORMATIONS ), dataObject.getElementDef(), null );
		}
#endif

            SifFormatter textFormatter = Adk.TextFormatter;
            FieldMapping lastRule = null;
            try
            {
                foreach (FieldMapping fm in fields)
                {
                    lastRule = fm;
                    String fieldName = fm.Alias;
                    if (fieldName == null)
                    {
                        fieldName = fm.FieldName;
                    }

                    if (fieldName == null || fieldName.Length == 0)
                    {
                        throw new AdkMappingException(
                            "Mapping rule for " + dataObject.ElementDef.Name + "[" + fm.FieldName +
                            "] must specify a field name", null);
                    }

                    if (adaptor.HasField(fieldName) || fm.HasDefaultValue)
                    {
                        //
                        //  For outbound mapping operations, only process 
                        //	XPathRules. All other rule types, like OtherIdRule,
                        //  are only intended to be used for inbound mappings.
                        //
                        if (fm.fRule is XPathRule)
                        {
                            XPathRule rule = (XPathRule) fm.fRule;
                            //  Lookup or create the element/attribute referenced by the rule
                            String ruledef = rule.XPath;
                            if (ruledef == null || ruledef.Trim().Length == 0)
                            {
                                throw new AdkMappingException(
                                    "Mapping rule for " + dataObject.ElementDef.Name + "[\"" + fieldName +
                                    "\"] must specify a path to an element or attribute", null);
                            }

                            // TT 199 If the FieldMapping has an "ifnull" value of "suppress", 
                            // don't render a result

                            // Determine if this element should be created before attempting to create it
                            // If the value resolves to null and the IFNULL_SUPPRESS flag is set, the element
                            // should not be created. That's why we have to look up the ElementDef first
                            TypeConverter typeConverter = null;
                            IElementDef def = rule.LookupTargetDef(dataObject.ElementDef);
                            if (def != null)
                            {
                                typeConverter = def.TypeConverter;
                            }
                            if (typeConverter == null)
                            {
                                typeConverter = SifTypeConverters.STRING;
                                // TODO: Perhaps the following exception should be thrown when
                                // in STRICT mode
                                //	throw new ADKMappingException( "Element {" + def.name() +
                                //				"} from rule \"" + ruledef + "\" does not have a data type definition.", null );
                            }

                            SifSimpleType mappedValue;
                            mappedValue = adaptor.GetSifValue(fieldName, typeConverter, fm);

                            // Perform a valueset translation, if applicable
                            if (mappedValue != null &&
                                mappedValue is SifString &&
                                fm.ValueSetID != null)
                            {
                                String textValue = mappedValue.ToString();
                                //	Perform automatic ValueSet translation
                                ValueSet vs = GetValueSet(fm.ValueSetID, true);
                                if (vs != null)
                                {
                                    // TT 199. Perform a more detailed valueset translation. 
                                    // If there is a default value for this field, use it if there is
                                    // no match found in the value set
                                    textValue = vs.Translate(textValue, fm.DefaultValue);
                                }
                                mappedValue = new SifString(textValue);
                            }

                            bool usedDefault = false;
                            if (mappedValue == null || mappedValue.RawValue == null)
                            {
                                // If the FieldMapping has a Default value, use that, unless
                                // it is explicitly suppressed
                                if (fm.NullBehavior != MappingBehavior.IfNullSuppress && fm.HasDefaultValue)
                                {
                                    mappedValue = fm.GetDefaultValue(typeConverter, textFormatter);
                                    usedDefault = true;
                                }
                                else
                                {
                                    continue;
                                }
                            }


                            if (!usedDefault)
                            {
                                String valueExpression = rule.ValueExpression;
                                if (valueExpression != null)
                                {
                                    // This XPath rule has a value assignment expression at the end of it
                                    String value = valueBuilder.Evaluate(valueExpression);
                                    mappedValue = typeConverter.Parse(textFormatter, value);
                                }
                            }

                            // If we have a null value to assign at this point, move on to the next rule
                            if (mappedValue == null || mappedValue.RawValue == null)
                            {
                                continue;
                            }

                            // At this point, mappedValue should not be null. We are committeed
                            // to building out the path and setting the value.
                            INodePointer pointer = rule.CreatePath(context, version);
                            //  If the element/attribute does not have a value, assign one.
                            //	If it does have a value, it was already assigned by the XPath 
                            //	rule in the lookupByXPath method above and should not be 
                            //	changed.
                            //
                            if (pointer != null)
                            {
                                Element pointedElement = (Element) pointer.Value;
                                SifSimpleType elementValue = pointedElement.SifValue;
                                if (elementValue == null || elementValue.RawValue == null)
                                {
                                    if (mappedValue is SifString)
                                    {
                                        // Now that we have the actual element, we may need to create convert the
                                        // data if we were unable to resolve the TypeConverter above. This only happens
                                        // in cases involving surrogates where the rule.lookupTargetDef( dataObject.getElementDef() );
                                        // fails to find the target ElementDef
                                        TypeConverter converter = pointedElement.ElementDef.TypeConverter;
                                        if (converter != null && converter.DataType != mappedValue.DataType)
                                        {
                                            mappedValue = converter.Parse(textFormatter, mappedValue.ToString());
                                        }
                                    }

                                    // This check for null should really not be necessary, 
                                    // however keepingit in for now
                                    if (mappedValue != null)
                                    {
                                        pointedElement.SifValue = mappedValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (lastRule != null)
                {
                    throw new AdkMappingException(
                        "Unable to evaluate field rule: " + lastRule.fRule + " : " + e.Message, null, e);
                }
                throw new AdkMappingException(e.ToString(), null, e);
            }
#if PROFILED
		finally
		{

			if( BuildOptions.PROFILED )
				ProfilerUtils.profileStop();
		}
#endif
        }

        /// <summary>  Returns the SourceId filters in effect for this Mappings instance.
        /// A Mappings instances always inherits the filters of its ancestry.
        /// 
        /// </summary>
        /// <returns> An array of SourceIds or null if this Mappings object applies to all SIF Agents
        /// </returns>
        /// <seealso cref="SetSourceIdFilter">
        /// </seealso>
        public string[] GetSourceIdFilter()
        {
            if (fParent == GetRoot())
            {
                return fSourceIds;
            }

            Hashtable m = new Hashtable();
            Mappings parent = this;
            while (parent.fParent != null)
            {
                string[] list = parent.fSourceIds;
                if (list != null)
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        m[list[i]] = null;
                    }
                }

                parent = parent.fParent;
            }

            string[] arr = new string[m.Count];
            m.Keys.CopyTo(arr, 0);
            return arr;
        }

        /// <summary>  Returns the ZoneId filters in effect for this Mappings instance.
        /// A Mappings instances always inherits the filters of its ancestry.
        /// 
        /// </summary>
        /// <returns> An array of ZoneIds or null if this Mappings object applies to all zones
        /// </returns>
        /// <seealso cref="SetZoneIdFilter">
        /// </seealso>
        public string[] GetZoneIdFilter()
        {
            if (fParent == GetRoot())
            {
                return fZoneIds;
            }

            Hashtable m = new Hashtable();
            Mappings parent = this;
            while (parent.fParent != null)
            {
                string[] list = parent.fZoneIds;
                if (list != null)
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        m[list[i]] = null;
                    }
                }

                parent = parent.fParent;
            }

            string[] arr = new string[m.Count];
            m.Keys.CopyTo(arr, 0);

            return arr;
        }

        /// <summary>  Returns the SifVersion filters in effect for this Mappings instance.
        /// A Mappings instances always inherits the filters of its ancestry.
        /// 
        /// </summary>
        /// <returns> An array of SifVersion objects or null if this Mappings object applies to all versions of SIF
        /// </returns>
        /// <seealso cref="SetSIFVersionFilter">
        /// </seealso>
        public SifVersion[] GetSIFVersionFilter()
        {
            if (fParent == GetRoot())
            {
                return fSifVersions;
            }

            Hashtable m = new Hashtable();

            Mappings parent = this;
            while (parent.fParent != null)
            {
                SifVersion[] list = parent.fSifVersions;
                if (list != null)
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        m[list[i]] = null;
                    }
                }

                parent = parent.fParent;
            }

            SifVersion[] arr = new SifVersion[m.Count];
            m.Keys.CopyTo(arr, 0);
            return arr;
        }

        /// <summary>  Determines if this nested Mappings instance allows the specified SourceId.
        /// 
        /// This method is only called on nested children of a Mappings object, never
        /// on a top-level Mappings object because top-level parents always allow all
        /// ZoneIds, SourceIds, and SIF Versions.
        /// 
        /// 
        /// </summary>
        /// <param name="sourceId">An agent's SourceId
        /// 
        /// </param>
        /// <returns> true if the SourceId is permitted by the SourceId filter in effect
        /// and can therefore be considered for selection by the <c>select</c>
        /// method; false if the SourceId is not included in the SourceId filter and
        /// should therefore not be considered for selection
        /// </returns>
        public int AllowsSourceId(string sourceId)
        {
            //  Allows all SourceIds?
            if (fSourceIds == null || fSourceIds.Length == 0)
            {
                return 0;
            }

            for (int i = 0; i < fSourceIds.Length; i++)
            {
                if (fSourceIds[i].Equals(sourceId))
                {
                    return 1;
                }
            }

            return -1;
        }

        /// <summary>  Determines if this nested Mappings instance allows the specified ZoneId.
        /// 
        /// This method is only called on nested children of a Mappings object, never
        /// on a top-level Mappings object because top-level parents always allow all
        /// ZoneIds, SourceIds, and SIF Versions.
        /// 
        /// 
        /// </summary>
        /// <param name="zoneId">A ZoneId
        /// 
        /// </param>
        /// <returns> true if the ZoneId is permitted by the ZoneId filter in effect
        /// and can therefore be considered for selection by the <c>select</c>
        /// method; false if the ZoneId is not included in the ZoneId filter and
        /// should therefore not be considered for selection
        /// </returns>
        public int AllowsZoneId(string zoneId)
        {
            //  Allows all zones?
            if (fZoneIds == null || fZoneIds.Length == 0)
            {
                return 0;
            }

            for (int i = 0; i < fZoneIds.Length; i++)
            {
                if (fZoneIds[i].Equals(zoneId))
                {
                    return 1;
                }
            }

            return -1;
        }

        /// <summary>  Determines if this nested Mappings instance allows the specified version of SIF
        /// 
        /// This method is only called on nested children of a Mappings object, never
        /// on a top-level Mappings object because top-level parents always allow all
        /// ZoneIds, SourceIds, and SIF Versions.
        /// 
        /// 
        /// </summary>
        /// <param name="version">A SifVersion instance describing a version of SIF
        /// 
        /// </param>
        /// <returns> true if the version is permitted by the SifVersion filter in effect
        /// and can therefore be considered for selection by the <c>select</c>
        /// method; false if the version is not included in the SifVersion filter and
        /// should therefore not be considered for selection
        /// </returns>
        public int AllowsVersion(SifVersion version)
        {
            //  Allows all versions?
            if (fSifVersions == null || fSifVersions.Length == 0)
            {
                return 0;
            }

            for (int i = 0; i < fSifVersions.Length; i++)
            {
                if (fSifVersions[i].Equals(version))
                {
                    return 1;
                }
            }

            return -1;
        }

        /// <summary>  Add a ValueSet to this Mappings instance</summary>
        public void AddValueSet(ValueSet vset)
        {
            if (fValueSets == null)
            {
                fValueSets = new Dictionary<String, ValueSet>();
            }

            fValueSets[vset.fId] = vset;

            if (vset.fNode == null && fNode != null)
            {
                //  Create a new <valueset>
                vset.fNode = fNode.OwnerDocument.CreateElement("valueset");
                vset.fNode.SetAttribute("id", vset.fId);
                if (vset.fTitle != null)
                {
                    vset.fNode.SetAttribute("title", vset.fTitle);
                }

                fNode.AppendChild(vset.fNode);

                //  Add <value> elements to the <valueset>...
                ValueSetEntry[] entries = vset.Entries;
                for (int i = 0; i < entries.Length; i++)
                {
                    entries[i].Node = fNode.OwnerDocument.CreateElement("value");
                    entries[i].Node.SetAttribute("name", entries[i].Name);
                    if (entries[i].Title != null)
                    {
                        entries[i].Node.SetAttribute("title", entries[i].Title);
                    }
                    entries[i].Node.InnerText = entries[i].Value;
                    vset.fNode.AppendChild(entries[i].Node);
                }
            }
        }

        /// <summary>  Remove a ValueSet from this Mappings instance.
        /// 
        /// If the ValueSet has a DOM XmlElement attached to it, it is removed from its 
        /// parent XmlElement and dereferenced.
        /// 
        /// </summary>
        /// <param name="vset">The ValueSet to remove
        /// </param>
        public void RemoveValueSet(ValueSet vset)
        {
            if (fValueSets != null)
            {
                if (vset.fNode != null)
                {
                    //	Remove the ValueSet's node from its parent's DOM XmlElement
                    vset.fNode.ParentNode.RemoveChild(vset.fNode);

                    //	Remove all children and dereference the entrys' Nodes
                    ValueSetEntry[] entries = vset.Entries;
                    for (int i = 0; i < entries.Length; i++)
                    {
                        if (entries[i].Node != null && entries[i].Node.ParentNode != null)
                        {
                            entries[i].Node.ParentNode.RemoveChild(entries[i].Node);
                        }
                        entries[i].Node = null;
                    }
                }

                //	Remove from the lookup table
                fValueSets.Remove(vset.fId);

                //	Dereference the node
                vset.fNode = null;
            }
        }

        /// <summary>  Remove a ValueSet from this Mappings instance.
        /// 
        /// If a ValueSet with the specified ID is found, it is removed from this
        /// Mappings and returned; otherwise no action is taken. If the ValueSet has
        /// a DOM XmlElement attached to it, it is removed from its parent XmlElement and 
        /// dereferenced.
        /// 
        /// </summary>
        /// <param name="id">The ID of the ValueSet to remove
        /// </param>
        public ValueSet RemoveValueSet(string id)
        {
            ValueSet vs = GetValueSet(id, false);
            if (vs != null)
            {
                RemoveValueSet(vs);
            }

            return vs;
        }

        /// <summary>  Gets a ValueSet by ID.</summary>
        /// <param name="id">The unique ID of the ValueSet
        /// </param>
        /// <param name="inherit">true to inherit the ValueSet from the Mappings ancestry
        /// if not found as a child of this Mappings object; false to return null
        /// if no ValueSet is found
        /// </param>
        public ValueSet GetValueSet(string id,
                                    bool inherit)
        {
            if (id == null)
            {
                return null;
            }

            if (!inherit)
            {
                return fValueSets != null ? fValueSets[id] : null;
            }

            Mappings m = this;
            ValueSet vs = null;
            while (m != null && vs == null)
            {
                if (m.fValueSets != null)
                {
                    vs = m.fValueSets[id];
                }
                m = m.Parent;
            }

            return vs;
        }

        /// <summary>  Gets all ValueSets for this Mappings instance.</summary>
        /// <param name="inherit">true to include ValueSets from the Mappings ancestry in
        /// the returned array, false to include only ValueSets defined by this
        /// Mappings object
        /// </param>
        public ValueSet[] GetValueSets(bool inherit)
        {
            Hashtable results = new Hashtable();

            Mappings m = this;

            do
            {
                if (m.fValueSets != null)
                {
                    foreach (ValueSet vs in m.fValueSets.Values)
                    {
                        if (vs != null && !results.ContainsKey(vs.fId))
                        {
                            results[vs.fId] = vs;
                        }
                    }
                }

                m = m.Parent;
            } while (m != null && inherit);

            ValueSet[] arr = new ValueSet[results.Count];
            results.Values.CopyTo(arr, 0);
            return arr;
        }

        /// <summary>  Add an ObjectMapping definition to this Mappings instance</summary>
        public void AddRules(ObjectMapping om)
        {
            AddRules(om, true);
        }

        /// <summary>  Add an ObjectMapping definition to this Mappings instance</summary>
        protected internal void AddRules(ObjectMapping om,
                                         bool buildDomTree)
        {
            if (om.fParent != null)
            {
                throw new SystemException
                    ("ObjectMapping is already a child of a Mappings instance");
            }

            om.fParent = this;

            if (fObjRules == null)
            {
                fObjRules = new Dictionary<String, ObjectMapping>();
            }
            fObjRules[om.ObjectType] = om;

            if (om.fNode == null && buildDomTree && fNode != null)
            {
                om.fNode = fNode.OwnerDocument.CreateElement(XML_OBJECT);
                if (om.ObjectType != null)
                {
                    om.fNode.SetAttribute("object", om.ObjectType);
                }
                fNode.AppendChild(om.fNode);
            }
        }

        /// <summary>  Remove an ObjectMapping definition from this Mappings instance</summary>
        public void RemoveRules(ObjectMapping om)
        {
            if (fObjRules != null)
            {
                if (fNode != null && om.fNode != null)
                {
                    fNode.RemoveChild(om.fNode);
                }
                fObjRules.Remove(om.ObjectType);
            }
        }

        /// <summary>  Gets all ObjectMappings defined for this Mappings instance, including
        /// those inherited by its parents.
        /// </summary>
        /// <returns> An array of all ObjectMappings
        /// </returns>
        public ObjectMapping[] GetObjectMappings()
        {
            return GetObjectMappings(true);
        }

        /// <summary>  Gets all ObjectMappings defined for this Mappings instance, optionally
        /// including those inherited by its parents.
        /// </summary>
        /// <returns> An array of all ObjectMappings
        /// </returns>
        public ObjectMapping[] GetObjectMappings(bool inherit)
        {
            Hashtable hashSet = new Hashtable();

            Mappings m = this;
            while (m != null)
            {
                if (m.fObjRules != null)
                {
                    foreach (ObjectMapping om in m.fObjRules.Values)
                    {
                        if (!hashSet.ContainsKey(om.ObjectType))
                        {
                            hashSet[om.ObjectType] = om;
                        }
                    }
                }

                if (inherit)
                {
                    m = m.fParent;
                }
                else
                {
                    m = null;
                }
            }

            ObjectMapping[] arr = new ObjectMapping[hashSet.Count];
            hashSet.Values.CopyTo(arr, 0);
            return arr;
        }

        public ObjectMapping GetObjectMapping(string objectType,
                                              bool inherit)
        {
            if (!inherit)
            {
                return fObjRules == null ? null : fObjRules[objectType];
            }

            Mappings m = this;

            while (m != null)
            {
                if (m.fObjRules != null)
                {
                    if (m.fObjRules.ContainsKey(objectType))
                    {
                        return m.fObjRules[objectType];
                    }
                }

                m = m.fParent;
            }

            return null;
        }

        /// <summary>  Return an array of all ObjectMapping definitions for a given object type.</summary>
        /// <param name="obj">A SIF Data Object (e.g. "StudentPersonal")
        /// </param>
        /// <param name="inherit">True to inherit the ObjectMapping from the ancestry if
        /// this Mappings instance does not define it
        /// </param>
        public ObjectMapping GetRules(SifDataObject obj,
                                      bool inherit)
        {
            return obj != null ? GetRules(obj.ElementDef.Name, inherit) : null;
        }

        /// <summary>  Returns the ObjectMapping for a given object type.</summary>
        /// <param name="objType">The name of a SIF Data Object (e.g. "StudentPersonal")
        /// </param>
        /// <param name="inherit">True to inherit the ObjectMapping from the ancestry if
        /// this Mappings instance does not define it
        /// </param>
        public ObjectMapping GetRules(string objType,
                                      bool inherit)
        {
            Mappings m = this;
            while (m != null)
            {
                if (m.fObjRules != null)
                {
                    foreach (ObjectMapping om in fObjRules.Values)
                    {
                        if (om.ObjectType.Equals(objType))
                        {
                            return om;
                        }
                    }
                }

                if (inherit)
                {
                    m = m.fParent;
                }
                else
                {
                    m = null;
                }
            }

            return null;
        }


        /// <summary>  Produces a DOM XmlElement graph of this Mappings object
        /// 
        /// </summary>
        /// <param name="doc">The parent document
        /// </param>
        /// <returns> A DOM XmlElement graph representing this Mappings object
        /// </returns>
        public XmlElement ToDom(XmlDocument doc)
        {
            if (doc == null)
            {
                return null;
            }

            XmlElement mappingsNode = doc.CreateElement(XML_MAPPINGS);
            mappingsNode.SetAttribute("id", fId == null ? "" : fId);

            if (fSifVersions != null && fSifVersions.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                for (int i = 0; i < fSifVersions.Length; i++)
                {
                    if (i != 0)
                    {
                        buf.Append(",");
                    }
                    buf.Append(fSifVersions[i]);
                }
                mappingsNode.SetAttribute("sifVersion", buf.ToString());
            }

            if (fSourceIds != null && fSourceIds.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                for (int i = 0; i < fSourceIds.Length; i++)
                {
                    if (i != 0)
                    {
                        buf.Append(",");
                    }
                    buf.Append(fSourceIds[i]);
                }
                mappingsNode.SetAttribute("sourceId", buf.ToString());
            }

            if (fZoneIds != null && fZoneIds.Length > 0)
            {
                StringBuilder buf = new StringBuilder();
                for (int i = 0; i < fZoneIds.Length; i++)
                {
                    if (i != 0)
                    {
                        buf.Append(",");
                    }
                    buf.Append(fZoneIds[i]);
                }
                mappingsNode.SetAttribute("zoneId", buf.ToString());
            }

            //  Write out each <object>...
            ObjectMapping[] objects = GetObjectMappings(false);
            foreach (ObjectMapping oMapping in objects)
            {
                XmlElement objNode = doc.CreateElement(XML_OBJECT);
                mappingsNode.AppendChild(objNode);
                objNode.SetAttribute(XML_OBJECT, oMapping.ObjectType);

                //  Write out each <field>...
                foreach (FieldMapping mapping in oMapping.GetRulesList(false))
                {
                    XmlElement fieldNode = doc.CreateElement(XML_FIELD);
                    objNode.AppendChild(fieldNode);
                    fieldNode.SetAttribute("name", mapping.FieldName);

                    if (mapping.DefaultValue != null)
                    {
                        fieldNode.SetAttribute("default", mapping.DefaultValue);
                    }
                    if (mapping.Alias != null)
                    {
                        fieldNode.SetAttribute("alias", mapping.Alias);
                    }
                    if (mapping.ValueSetID != null)
                    {
                        fieldNode.SetAttribute("valueSet", mapping.ValueSetID);
                    }

                    // Allow the MappingsFilter class to write it's values to the node.
                    MappingsFilter.Save(mapping.Filter, fieldNode);

                    mapping.fRule.ToXml(fieldNode);
                }
            }

            return mappingsNode;
        }

        /// <summary>  Returns the Mappings as a string in XML form</summary>
        public string ToXml()
        {
            return ToXml(false);
        }

        /// <summary>  Returns the Mappings as a string in XML form.
        /// 
        /// </summary>
        /// <param name="renderAsRuntimeMappings">true to inherit object and field rules so
        /// this Mappings is rendered with dynamic content as it would be evaluated
        /// at runtime. This can be useful for diagnostics (i.e. displaying the
        /// current state of a Mappings object), but not for rendering to a
        /// configuration file.
        /// </param>
        public string ToXml(bool renderAsRuntimeMappings)
        {
            //  TODO: This should really call toXML( Document ) and then render
            //  the XmlElement to an output stream. But that method didn't exist at the
            //  time this was written, so...

            StringBuilder b = new StringBuilder();

            b.Append("\n<");
            b.Append(XML_MAPPINGS);
            if (fId != null)
            {
                b.Append(" id='" + fId + "'");
            }

            if (fSifVersions != null && fSifVersions.Length > 0)
            {
                b.Append(" sifVersion='");
                for (int i = 0; i < fSifVersions.Length; i++)
                {
                    if (i != 0)
                    {
                        b.Append(",");
                    }
                    b.Append(fSifVersions[i]);
                }
                b.Append("'");
            }

            if (fSourceIds != null && fSourceIds.Length > 0)
            {
                b.Append(" sourceId='");
                for (int i = 0; i < fSourceIds.Length; i++)
                {
                    if (i != 0)
                    {
                        b.Append(",");
                    }
                    b.Append(fSourceIds[i]);
                }
                b.Append("'");
            }

            if (fZoneIds != null && fZoneIds.Length > 0)
            {
                b.Append(" zoneId='");
                for (int i = 0; i < fZoneIds.Length; i++)
                {
                    if (i != 0)
                    {
                        b.Append(",");
                    }
                    b.Append(fZoneIds[i]);
                }
                b.Append("'");
            }

            b.Append(">");

            //  Write out each <object>...
            ObjectMapping[] objects = GetObjectMappings();
            for (int i = 0; i < objects.Length; i++)
            {
                b.Append("\n\t<");
                b.Append(XML_OBJECT);
                b.Append(" name='");
                b.Append(objects[i].ObjectType);
                b.Append("'>");

                //  Write out each <field>...
                IList<FieldMapping> rules = objects[i].GetRulesList(renderAsRuntimeMappings);
                foreach (FieldMapping field in rules)
                {
                    b.Append("\n\t\t<");
                    b.Append(XML_FIELD);
                    b.Append(" name='");
                    b.Append(field.FieldName);
                    b.Append("' ");

                    if (field.DefaultValue != null)
                    {
                        b.Append("default='");
                        b.Append(field.DefaultValue);
                        b.Append("' ");
                    }

                    if (field.Alias != null)
                    {
                        b.Append("alias='");
                        b.Append(field.Alias);
                        b.Append("' ");
                    }

                    if (field.ValueSetID != null)
                    {
                        b.Append("valueSet='");
                        b.Append(field.ValueSetID);
                        b.Append("' ");
                    }

                    if (field.Filter != null)
                    {
                        string dir = null;
                        if (field.Filter.Direction == MappingDirection.Inbound)
                        {
                            dir = "inbound";
                        }
                        else if (field.Filter.Direction == MappingDirection.Outbound)
                        {
                            dir = "outbound";
                        }

                        if (dir != null)
                        {
                            b.Append("direction='");
                            b.Append(dir);
                            b.Append("' ");
                        }

                        if (field.Filter.HasVersionFilter)
                        {
                            b.Append("sifVersion='");
                            b.Append(field.Filter.SifVersion);
                            b.Append("' ");
                        }
                    }

                    b.Append(">");
                    b.Append(field.fRule.ToString());
                    b.Append("</");
                    b.Append(XML_FIELD);
                    b.Append(">");
                }

                b.Append("\n\t</");
                b.Append(XML_OBJECT);
                b.Append(">");
            }

            b.Append("\n</");
            b.Append(XML_MAPPINGS);
            b.Append(">\n");

            return b.ToString();
        }

        internal class Candidate
        {
            internal Mappings fMapping;
            internal int restrictiveness;

            public Candidate(Mappings m)
            {
                fMapping = m;

                //  The "restrictiveness" score is based on a count of all filters
                string[] f = m.GetZoneIdFilter();
                if (f != null)
                {
                    restrictiveness += f.Length;
                }
                f = m.GetSourceIdFilter();
                if (f != null)
                {
                    restrictiveness += f.Length;
                }
                SifVersion[] v = m.GetSIFVersionFilter();
                if (v != null)
                {
                    restrictiveness += v.Length;
                }
            }
        }

        /// <summary>
        /// Creates and returns a string array, based on the comma-delimited string sent to it. If the string
        /// contains an asterisk as on of the elements, it returns a null string
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private string[] BuildFilterString(string filter)
        {
            string[] returnVal = null;
            if (filter != null)
            {
                returnVal = filter.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (returnVal.Length == 0)
                {
                    return null;
                }
                else
                {
                    foreach ( string sourceid in returnVal )
                    {
                        if ( sourceid == "*" )
                        {
                            return null;
                        }
                    }
                }
            }
           
            return returnVal;
        }
    }
}
