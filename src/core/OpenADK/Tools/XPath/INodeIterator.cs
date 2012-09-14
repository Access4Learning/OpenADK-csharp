//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Tools.XPath
{
    /// <summary>
    /// Represents an iterator over an ADK Element's children or attributes
    /// </summary>
    public interface INodeIterator
    {
        /// <summary>
        /// The parent node of the collection being iterated
        /// </summary>
        INodePointer ParentNode { get; }

        /// <summary>
        /// The next node in the collection or Null if at the end of the collection
        /// </summary>
        INodePointer MoveNext();

        /// <summary>
        /// The previous node in the collection, or NULL if at the beginning of the collection
        /// </summary>
        INodePointer MovePrevious();

        INodeIterator Clone();

        int Count { get;}
    }
}
