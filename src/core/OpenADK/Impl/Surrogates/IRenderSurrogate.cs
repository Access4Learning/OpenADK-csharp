//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Impl.Surrogates
{
    public interface IRenderSurrogate
    {
        /// <summary>
        /// Render this element using the underlying SIFWriter
        /// </summary>
        /// <param name="writer">The XmlWriter to write to</param>
        /// <param name="version">The SifVersion to write in</param>
        /// <param name="o">The Element to write</param>
        /// <exception cref="AdkParsingException">Can be thrown if an error occcurs</exception>
        /// <param name="formatter">The formatter to use for converting XSD datatypes</param>
        void RenderRaw( XmlWriter writer,
                        SifVersion version,
                        Element o,
                        SifFormatter formatter );


        /// <summary>
        ///  Called by the parser when it is on an element it cannot parse. Multiple RenderSurrogates
        ///  may be called during a failed parse operation. If the RenderSurrogate successfully parses
        /// the XML, it returns true
        /// </summary>
        /// <remarks>
        /// Note: Unlike the Java ADK, Surrogates in the the .NET ADK do not have to worry about
        /// completely consuming the XMLElement and advancing to the next tag. The .NET 
        /// Surrogates are handed a reader that only allows reading the current node and 
        /// the parent reader is automatically advanced when the surrogate is done.
        /// </remarks>
        /// <param name="reader">An XmlReader that is positioned on the current node</param>
        /// <param name="version">The SifVersion being parsed</param>
        /// <param name="parent">The SifElement that is the parent of this field</param>
        /// <param name="formatter">The SifFormatter to use for parsing XSD datatyps</param>
        /// <returns>True if the surrogates successfully parses the node. False if the node is not
        /// recognizee by the surrogate.</returns>
        bool ReadRaw( XmlReader reader,
                      SifVersion version,
                      SifElement parent,
                      SifFormatter formatter );

        /// <summary>
        /// Creates a child element, if supported by this node
        /// </summary>
        /// <param name="parentPointer"></param>
        /// <param name="formatter"></param>
        /// <param name="version"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        INodePointer CreateChild( INodePointer parentPointer, SifFormatter formatter, SifVersion version,
                                  SifXPathContext context );

        /// <summary>
        /// Called by the ADK XPath traversal code when it is traversing the given element
        /// in a legacy version of SIF
        /// </summary>
        /// <param name="parentPointer">The parent element pointer</param>
        /// <param name="element">The Element to create a node pointer for</param>
        /// <param name="version">The SIFVersion in effect</param>
        /// <returns>A NodePointer representing the current element</returns>
        INodePointer CreateNodePointer( INodePointer parentPointer, Element element, SifVersion version );


        /// <summary>
        /// Gets the element name or path to the element in this version of SIF
        /// </summary>
        String Path { get; }
    }
}
