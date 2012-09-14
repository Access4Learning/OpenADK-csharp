//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Text;
using System.Xml;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  A Rule class to evaluate &lt;OtherId&gt; queries as defined by the OtherIdMapping class.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    //[Serializable]
    public class OtherIdRule : Rule //, System.Runtime.Serialization.ISerializable
    {
        protected internal OtherIdMapping fMapping;

        protected internal XmlElement fNode;

        public OtherIdRule(OtherIdMapping mapping)
            : this(mapping, null)
        {
        }

        public OtherIdRule(OtherIdMapping mapping,
                           XmlElement node)
        {
            fMapping = mapping;
            fNode = node;
        }

        /// <summary>  Produces a duplicate of this Rule object
        /// 
        /// </summary>
        /// <returns> A "deep copy" of this Rule object
        /// </returns>
        public override Rule Copy(FieldMapping newParent)
        {
            OtherIdRule m = new OtherIdRule(fMapping == null ? null : fMapping.Copy());
            if (fNode != null && newParent.fNode != null)
            {
                m.fNode = (XmlElement) fNode.CloneNode(false);
            }

            return m;
        }

        /// <summary>  Evaluates this rule against a SifDataObject and returns the text value
        /// of the <c>&lt;OtherId&gt;</c> element that satisfied the query. If
        /// the OtherIdMapping passed to the constructor included a <i>prefix</i>
        /// attribute, the returned value will exclude the prefix string.
        /// 
        /// </summary>
        /// <param name="data">The SifDataObject the rule is evaluated against
        /// </param>
        /// <returns> The value of the <c>&lt;OtherId&gt;</c> element that
        /// satisfied the query (excluding the prefix string if applicable), or
        /// null if no match found
        /// </returns>
        /// <param name="version">The SIFVersion to use while evaluating the data object</param>
        private SifSimpleType Evaluate(SifDataObject data, SifVersion version)
        {
            if (data != null)
            {
                //
                //  Search all of the OtherId children for one that matches the type
                //  and optionally the prefix specified by the fMapping
                //
                ICollection  otherIdList = (ICollection) data.GetChild("OtherIdList");
                if (otherIdList != null)
                {
                    foreach (SifElement otherId in otherIdList)
                    {
                        //  Compare the Type attribute
                        SimpleField typ = otherId.GetField("Type");
                        if (typ == null || !typ.TextValue.Equals(fMapping.Type))
                        {
                            continue;
                        }

                        //  Optionally compare the prefix and if its a match return
                        //  all text after the prefix string
                        String prefix = fMapping.Prefix;
                        if (prefix != null)
                        {
                            String val = otherId.TextValue;
                            if (val != null && val.StartsWith(prefix))
                            {
                                return new SifString(val.Substring(prefix.Length));
                            }
                        }
                        else
                        {
                            return otherId.SifValue;
                        }
                    }
                }
            }
            return null;
        }

        public override SifSimpleType Evaluate(SifXPathContext context, SifVersion version)
        {
            SifDataObject sdo = (SifDataObject) context.ContextElement;
            return Evaluate(sdo, version);
        }

        /// <summary>  Render this OtherIdRule as an XML DOM XmlElement</summary>
        public override void ToXml(XmlElement parent)
        {
            if (fNode == null)
            {
                XmlElement n = parent.OwnerDocument.CreateElement("OtherId");
                if (fMapping.Type != null)
                {
                    n.SetAttribute("type", fMapping.Type);
                }
                if (fMapping.Prefix != null)
                {
                    n.SetAttribute("prefix", fMapping.Prefix);
                }

                parent.AppendChild(n);
            }
            else
            {
                parent.AppendChild(fNode);
            }
        }

        /// <summary>  Return the string representation of this OtherIdRule as XML text</summary>
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append("<OtherId type='");
            if (fMapping.Type != null)
            {
                b.Append(fMapping.Type);
            }
            b.Append("'");
            if (fMapping.Prefix != null)
            {
                b.Append(" prefix='");
                b.Append(fMapping.Prefix);
                b.Append("'");
            }

            b.Append("/>");

            return b.ToString();
        }
    }
}
