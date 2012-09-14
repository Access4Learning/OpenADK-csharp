//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Xml.XPath;

namespace OpenADK.Library.Tools.XPath
{
    internal class SimpleFieldAttributePointer : SimpleFieldPointer
    {
        /// <summary>
        /// Creates a new pointer to a SimpleField that is represented as an XML Attribute
        /// </summary>
        /// <param name="parentPointer">The parent pointer</param>
        /// <param name="element">The attribute being wrapped</param>
        /// <param name="version">The SifVersion</param>
        public SimpleFieldAttributePointer
            (
            INodePointer parentPointer,
            Element element,
            SifVersion version )
            : base( parentPointer, element, version )
        {
        }

        /// <summary>
        /// One of the XPathNodeType values representing the current node. 
        /// </summary>
        public override XPathNodeType NodeType
        {
            get { return XPathNodeType.Attribute; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node 
        /// is an empty element without an end element tag. 
        /// </summary>
        public override bool IsEmptyElement
        {
            get { return false; }
        }

        /// <summary>
        /// Returns a cloned instance of this NodePointer
        /// </summary>
        /// <returns></returns>
        public override INodePointer Clone()
        {
            return new SimpleFieldAttributePointer( Parent, fElement, Version );
        }
    }
}
