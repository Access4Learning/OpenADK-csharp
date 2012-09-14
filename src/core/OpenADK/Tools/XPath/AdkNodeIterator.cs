//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;

namespace OpenADK.Library.Tools.XPath
{
    internal abstract class AdkNodeIterator : INodeIterator
    {
        protected IList<Element> fChildNodes;
        protected SifElementPointer fParent;
        internal int fPosition = -1;

        /// <summary>
        ///  Creates a new instance of ADKNodeIterator
        /// </summary>
        /// <param name="parent"></param>
        protected AdkNodeIterator( SifElementPointer parent )
        {
            fParent = parent;
            fChildNodes = new List<Element>();
        }


        /// <summary>
        ///  Creates a new instance of ADKNodeIterator
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="nodesToIterate"></param>
        protected AdkNodeIterator( SifElementPointer parent,
                                   IList<Element> nodesToIterate )
        {
            fParent = parent;
            fChildNodes = nodesToIterate;
        }



        /// <summary>
        /// The parent node of the collection being iterated
        /// </summary>
        public INodePointer ParentNode
        {
            get { return fParent; }
        }

        /// <summary>
        /// The next node in the collection or Null if at the end of the collection
        /// </summary>
        public INodePointer MoveNext()
        {
            fPosition++;
            return GetCurrent();
        }

        /// <summary>
        /// The previous node in the collection, or NULL if at the beginning of the collection
        /// </summary>
        public INodePointer MovePrevious()
        {
            fPosition--;
            return GetCurrent();
        }

        public abstract INodeIterator Clone();
        
        public int Count
        {
            get { return fChildNodes.Count; }
        }

        private INodePointer GetCurrent()
        {
            if ( fPosition < 0 || fPosition >= fChildNodes.Count )
            {
                return null;
            }
            return GetNodePointer( fParent, fChildNodes[fPosition] );
        }


        /// <summary>
        /// Called on the subclass when a specific NodePointer is requested by SIFXPath
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected abstract INodePointer GetNodePointer( SifElementPointer parent, Element element );


        /// <summary>
        /// Called by subclasses when they have found a new node that should be iterated
        /// </summary>
        /// <param name="node"></param>
        protected void addNodeToIterate( Element node )
        {
            fChildNodes.Add( node );
        }
    }
}
