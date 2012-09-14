//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using OpenADK.Library.Tools.Cfg;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  Encapsulates an &lt;OtherId&gt; field mapping
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class OtherIdMapping
    {


        protected internal string fType;
        protected internal string fPrefix;
        protected internal XmlElement fNode;

        /// <summary>  Constructor</summary>
        protected internal OtherIdMapping()
            : this(null, null, null)
        {
        }

        /// <summary>  Constructor</summary>
        /// <param name="type">The value of the OtherId/@Type attribute to match (e.g. 'ZZ')
        /// </param>
        /// <param name="prefix">The prefix value to match (e.g. 'GRADE:')
        /// </param>
        public OtherIdMapping(string type,
                              string prefix)
            : this(type, prefix, null)
        {
        }

        public OtherIdMapping(string type,
                              string prefix,
                              XmlElement node)
        {
            fType = type;
            fPrefix = prefix;
            fNode = node;
        }


        /// <summary>  Gets the &lt;OtherId&gt; Type attribute value that must be present
        /// for this rule to evaluate true
        /// </summary>
        /// <returns> The value of the Type attribute that must be present for
        /// this rule to evaluate true (e.g. "ZZ", "06", etc.)
        /// </returns>
        /// <summary>  Sets the &lt;OtherId&gt; Type attribute value that must be present
        /// for this rule to evaluate true
        /// </summary>
        /// <value>The value of the Type attribute that must be present for
        /// this rule to evaluate true (e.g. "ZZ", "06", etc.)
        /// <value>
        public virtual string Type
        {
            get { return fType; }

            set
            {
                fType = value;

                if (fNode != null && (Object) value != null)
                {
                    fNode.SetAttribute("type", value);
                }
            }
        }

        /// <summary>  Gets the optional OtherId <i>prefix</i> string that must be present at
        /// the beginning of the OtherId value for this rule to evaluate true.
        /// </summary>
        /// <returns> The prefix string agreed upon by two or more agents to identify
        /// multiple &lt;OtherId&gt; instances (e.g. "HOMEROOM:", "BARCODE:",
        /// "GRADE:", etc.)
        /// </returns>
        /// <summary>  Gets the optional OtherId <i>prefix</i> string that must be present at
        /// the beginning of the OtherId value for this rule to evaluate true.
        /// Prefix strings are not officially part of the SIF 1.0 specification but
        /// are typically agreed upon by vendors and used with Type "ZZ"
        /// (e.g. <code>&lt;OtherId Type="ZZ"&gt;GRADE:8&lt;/OtherId&gt;</code>)
        /// 
        /// </summary>
        /// <value>prefix string agreed upon by two or more agents to identify
        /// multiple &lt;OtherId&gt; instances (e.g. "HOMEROOM:", "BARCODE:",
        /// "GRADE:", etc.)
        /// </value>
        public virtual string Prefix
        {
            get { return fPrefix; }

            set
            {
                fPrefix = value;

                if (fNode != null && (Object) value != null)
                {
                    fNode.SetAttribute("prefix", value);
                }
            }
        }



        /// <summary>  Produces a duplicate of this Rule object
        /// 
        /// </summary>
        /// <returns> A "deep copy" of this Rule object
        /// </returns>
        public virtual OtherIdMapping Copy()
        {
            OtherIdMapping m = new OtherIdMapping();
            m.fType = fType;
            m.fPrefix = fPrefix;
            //  Copy the DOM XmlElement
            m.fNode = fNode == null ? null : (XmlElement) fNode.CloneNode(false);
            return m;
        }

        public static OtherIdMapping FromXml(ObjectMapping parent, FieldMapping field, XmlElement element)
        {
            //  The OtherId type= attribute is required
            String type = element.GetAttribute("type");
            if (type == null)
                type = element.GetAttribute("Type");
            if (type == null)
                throw new AdkConfigException("Field mapping rule " + parent.ObjectType + "." + field.FieldName +
                                             " specifies an <OtherId> without a 'type' attribute");

            //	The OtherId prefix= attribute is required
            String prefix = element.GetAttribute("prefix");
            if (prefix == null)
                prefix = element.GetAttribute("Prefix");
            if (prefix == null)
                throw new AdkConfigException("Field mapping rule " + parent.ObjectType + "." + field.FieldName +
                                             " specifies an <OtherId> without a 'prefix' attribute");

            //  Create a new OtherIdMapping as a child of the FieldMapping
            OtherIdMapping id = new OtherIdMapping(type, prefix, element);
            return id;
        }
    }
}
