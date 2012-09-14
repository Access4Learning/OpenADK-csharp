//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Xml.XPath;

namespace OpenADK.Library.Tools.XPath
{
    /// <summary>
    /// This field represents a simple element with no attributes
    /// </summary>
    internal class SimpleFieldElementPointer : SimpleFieldPointer
    {
        /// <summary>
        /// Creates a new pointer representing an XML Element with a simple text value
        /// </summary>
        /// <param name="parentPointer">The parent of this pointer</param>
        /// <param name="element">The element being pointed to</param>
        /// <param name="version">The SifVersion to use for resolving XPaths</param>
        public SimpleFieldElementPointer( INodePointer parentPointer, Element element, SifVersion version )
            : base( parentPointer, element, version )
        {
        }

        /// <summary>
        /// One of the XPathNodeType values representing the current node. 
        /// </summary>
        public override XPathNodeType NodeType
        {
            get { return XPathNodeType.Element; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node 
        /// is an empty element without an end element tag. 
        /// </summary>
        public override bool IsEmptyElement
        {
            get { return fElement.SifValue == null; }
        }

        /// <summary>
        /// Returns a cloned instance of this NodePointer
        /// </summary>
        /// <returns></returns>
        public override INodePointer Clone()
        {
            return new SimpleFieldElementPointer( Parent, fElement, Version );
        }
    }
}
