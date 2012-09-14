//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  A Rule class to evaluate XPath-like queries as defined by the
    /// <code>SifDtd.lookupByXPath</code> method
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public class XPathRule : Rule
    {
        internal string fDef = null;
        private IElementDef fTargetDef;

        /* For outbound mappings that contain an expression containing the '=' sign,
	    * this field contains the index of that sign for easy parsing
	    */
        private int fValueIndex = -1;
        private SifXPathExpression fExpression;


        /// <summary>  Constructor</summary>
        /// <param name="definition">An XPath-like query (e.g. "@RefId",
        /// "StudentAddress/Address[@Type='H','M']/Street/Line1") to evaluate
        /// against the SifDataObject passed to the <code>evaluate</code> method
        /// </param>
        public XPathRule(string definition)
        {
            fDef = definition;
        }

        /// <summary>  Produces a duplicate of this Rule object
        /// 
        /// </summary>
        /// <returns> A "deep copy" of this Rule object
        /// </returns>
        public override Rule Copy(FieldMapping newParent)
        {
            XPathRule clone = new XPathRule(fDef);
            if (newParent.fNode != null)
            {
                newParent.fNode.InnerText = fDef;
            }

            return clone;
        }

        /// <summary>  Render this XPathRule as an XML DOM XmlElement</summary>
        public override void ToXml(XmlElement parent)
        {
            parent.InnerText = fDef;
        }

        /// <summary>  Render this Rule as an XML element</summary>
        public override string ToString()
        {
            return fDef;
        }

        public String XPath
        {
            get { return fDef; }
        }

        public String ValueExpression
        {
            get
            {
                if (fValueIndex == -1)
                {
                    return null;
                }
                return fDef.Substring(fValueIndex);
            }
        }

        public String PathExpression
        {
            get
            {
                if (fExpression == null)
                {
                    Compile();
                }
                if (fValueIndex == -1)
                {
                    return fDef;
                }
                return fDef.Substring(0, fValueIndex - 1);
            }
        }


        /// <summary>  Evaluates this rule against a SifDataObject and returns the text value
        /// of the element or attribute that satisfied the query.
        /// 
        /// </summary>
        /// <param name="context">The SIFXPathContext to evaluate this rule against</param>
        /// <param name="version">The SIF version to use when evaluating this rule</param>
        /// <returns> The text value of the element or attribute that satisfied the
        /// query, or null if no match found
        /// </returns>
        public override SifSimpleType Evaluate(SifXPathContext context, SifVersion version)
        {
            // TODO: This could be done in the constructor, but the ADK outbound mapping
            // syntax sometimes cannot be compiled because it uses proprietary syntax
            // Therefore, compile the expression the first time it is used for a mapping
            if (fExpression == null)
            {
                Compile();
            }

            Object value = fExpression.GetValue(context);
            if (value == null)
            {
                return null;
            }
            else if (value is Element)
            {
                return ((Element) value).SifValue;
            }
            else
            {
                return new SifString(value.ToString());
            }
        }

        public INodePointer CreatePath(SifXPathContext context, SifVersion version)
        {
            if (fExpression == null)
            {
                Compile();
            }

            return fExpression.CreatePath(context);
        }


        private void Compile()
        {
            // If there is a value assignment in the rule, chop it off
            String sqp = fDef;
            int lastEqualsSign = fDef.LastIndexOf("=");
            if (lastEqualsSign > -1)
            {
                int lastBracket = fDef.LastIndexOf("]");
                if (lastBracket < lastEqualsSign)
                {
                    sqp = fDef.Substring(0, lastEqualsSign);
                    fValueIndex = lastEqualsSign + 1;
                }
            }

            fExpression = SifXPathContext.Compile(sqp);
        }

        /// <summary>
        /// Looks up the ElementDef that this XPathRule points to by XPath
        /// </summary>
        /// <param name="parent">The parent object metadata object, representing the root of the path</param>
        /// <returns>The ElementDef that this XPathRule points to</returns>
        public IElementDef LookupTargetDef(IElementDef parent)
        {
            if (fTargetDef == null)
            {
                fTargetDef = Adk.Dtd.LookupElementDefBySQP(parent, PathExpression);
            }
            return fTargetDef;
        }
    }
}
