//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Xml;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  The abstract base class for all Mappings rules
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public abstract class Rule
    {
        /// <summary>  Evaluates this rule against a SIFDataObject and returns the text value
        /// of the element or attribute that satisfied the query.
        /// 
        /// </summary>
        /// <param name="context">The SifXPathcontext instance to use for object traversal </param>
        /// <param name="version">The SIF version that is in effect</param>
        /// <returns>The SimpleType representing the value of the element or attribute that
        /// satisfied the query, or null if no match found
        /// </returns>
        public abstract SifSimpleType Evaluate(SifXPathContext context, SifVersion version);

        /// <summary>  Produces a duplicate of this Rule object
        /// 
        /// </summary>
        /// <returns> A "deep copy" of this Rule object
        /// </returns>
        public abstract Rule Copy(FieldMapping newParent);

        /// <summary>  Render this Rule as an DOM XmlElement</summary>
        /// <param name="parent">The parent XmlElement
        /// </param>
        public abstract void ToXml(XmlElement parent);
    }
}
