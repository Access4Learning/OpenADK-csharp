//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Xml.XPath;

namespace OpenADK.Library.Tools.XPath
{
    public interface IPointer
    {
        /// <summary>
        /// Gets the value that this NodePointer points to
        /// </summary>
        object Value { get; }

        void SetValue( object value );

        /// <summary>
        /// A String that contains the qualified Name of the current node, or String.Empty 
        /// if the current node does not have a name (for example, text or comment nodes). 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// One of the XPathNodeType values representing the current node. 
        /// </summary>
        XPathNodeType NodeType { get; }
    }
}
