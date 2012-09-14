//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Tools.XPath
{
    internal class SingleNodeIterator : INodeIterator
    {
        private INodePointer fPointer;
        private int fPosition = -1;

        public SingleNodeIterator( INodePointer iteratedItem )
        {
            fPointer = iteratedItem;
        }

        private SingleNodeIterator( INodePointer iteratedItem, int position )
        {
            fPointer = iteratedItem;
            fPosition = position;
        }

        /// <summary>
        /// The parent node of the collection being iterated
        /// </summary>
        public INodePointer ParentNode
        {
            get { return fPointer.Parent; }
        }

        /// <summary>
        /// The next node in the collection or Null if at the end of the collection
        /// </summary>
        public INodePointer MoveNext()
        {
                fPosition++;
                return GetPointer();
        }

        private INodePointer GetPointer()
        {
            if ( fPosition == 0 )
            {
                return fPointer;
            }
            return null;
        }

        /// <summary>
        /// The previous node in the collection, or NULL if at the beginning of the collection
        /// </summary>
        public INodePointer MovePrevious()
        {
                fPosition--;
                return GetPointer();
        }

        public INodeIterator Clone()
        {
            return new SingleNodeIterator( fPointer, fPosition );
        }

        public int Count
        {
            get { return 1; }
        }
    }
}
