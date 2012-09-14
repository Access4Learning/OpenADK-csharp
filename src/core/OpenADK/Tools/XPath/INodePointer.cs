//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Tools.XPath
{
    public interface INodePointer : IPointer
    {
        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node 
        /// is an empty element without an end element tag. 
        /// </summary>
        bool IsEmptyElement { get; }

        /// <summary>
        /// returns the raw node pointed to by this pointer
        /// </summary>
        object Node { get; }

        /// <summary>
        /// Returns an INodeIterator that iterates instance representing the first attribute of 
        /// the current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        INodeIterator GetAttributes();


        /// <summary>
        /// Returns a cloned instance of this NodePointer
        /// </summary>
        /// <returns></returns>
        INodePointer Clone();


        /// <summary>
        /// Returns an INodeIterator instance that iterates the children  
        /// of the current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        INodeIterator GetChildren();

        /// <summary>
        /// A String representing the ID value of the node to which you want to move.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        INodePointer GetChildById( string id );


        /// <summary>
        /// The parent of this node pointer
        /// </summary>
        INodePointer Parent { get; }

        /// <summary>
        /// Creates a child of this node
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        INodePointer CreateChild( SifXPathContext context, string name, int i );

        /// <summary>
        /// Creates an attribute on this node
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        INodePointer CreateAttribute( SifXPathContext context, string name );
    }
}
