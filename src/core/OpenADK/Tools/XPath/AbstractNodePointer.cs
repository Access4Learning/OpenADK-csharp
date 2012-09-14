//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml.XPath;

namespace OpenADK.Library.Tools.XPath
{
    public abstract class AbstractNodePointer : INodePointer
    {
        private INodePointer fParentPointer;

        public AbstractNodePointer( INodePointer parent )
        {
            fParentPointer = parent;
        }

        public abstract object GetBaseValue();


        /// <summary>
        /// Gets the value that this NodePointer points to
        /// </summary>
        public virtual object Value
        {
            get
            {
                object val = GetBaseValue();
                if ( val == null )
                {
                    val = Node;
                }
                return val;
            }
        }

        public abstract void SetValue( object value );

        /// <summary>
        /// One of the XPathNodeType values representing the current node. 
        /// </summary>
        public abstract XPathNodeType NodeType { get; }

        public virtual INodePointer CreateAttribute( SifXPathContext context, string name )
        {
            throw new InvalidOperationException(
                string.Format(
                    "Cannot create an object for path {0}/{1}. Creating a child is not supported by this type of node: {2}({3})",
                    Name, name, NodeType, GetType().FullName ) );
        }

        /// <summary>
        /// A String that contains the qualified Name of the current node, or String.Empty 
        /// if the current node does not have a name (for example, text or comment nodes). 
        /// </summary>
        public abstract string Name { get; }


        public virtual INodePointer CreateChild( SifXPathContext context, string name, int i )
        {
            throw new InvalidOperationException( "Cannot create an object for path " + Name + "/" + name +
                                                 ". Creating a child is not supported by this type of node: " +
                                                 NodeType );
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node 
        /// is an empty element without an end element tag. 
        /// </summary>
        public abstract bool IsEmptyElement { get; }


        /// <summary>
        /// returns the raw node pointed to by this pointer
        /// </summary>
        public abstract object Node { get; }

        /// <summary>
        /// Returns an INodeIterator that iterates instance representing the first attribute of 
        /// the current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        public virtual INodeIterator GetAttributes()
        {
            return null;
        }


        /// <summary>
        /// Returns a cloned instance of this NodePointer
        /// </summary>
        /// <returns></returns>
        public abstract INodePointer Clone();

        /// <summary>
        /// Returns an INodeIterator instance that iterates the children  
        /// of the current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        public virtual INodeIterator GetChildren()
        {
            return null;
        }

        /// <summary>
        /// Returns an INodePointer instance if successful moving to the parent node of the 
        /// current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        public INodePointer Parent
        {
            get { return fParentPointer; }
        }


        /// <summary>
        /// A String representing the ID value of the node to which you want to move.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public INodePointer GetChildById( string id )
        {
            // Currently looking up elements by ID is not supported
            return null;
        }
    }
}
