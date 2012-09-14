//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Impl.Surrogates
{
    public class FauxAttribute : FauxElementPointer, INodeIterator
    {
        private String fValue;
        private int fPosition = -1;

        public FauxAttribute( INodePointer parent, string fauxName, String value ) : base( parent, fauxName, true )
        {
            fValue = value;
        }

        /// <summary>
        /// Gets the value that this NodePointer points to
        /// </summary>
        public override Object GetBaseValue()
        {
            return fValue;
        }

        public override void SetValue( object value )
        {
            if ( value == null )
            {
                fValue = null;
            }
            else
            {
                fValue = value.ToString();
            }
        }


        public override Object Node
        {
            get { return fValue; }
        }


        /// <summary>
        /// Returns a cloned instance of this NodePointer
        /// </summary>
        /// <returns></returns>
        public override INodePointer Clone()
        {
            return new FauxAttribute( Parent, Name, fValue );
        }

        INodeIterator INodeIterator.Clone()
        {
            return new FauxAttribute( Parent, Name, fValue );
        }

        /// <summary>
        /// The parent node of the collection being iterated
        /// </summary>
        public INodePointer ParentNode
        {
            get { return this.Parent; }
        }

        public int Count
        {
            get { return 1; }
        }

        /// <summary>
        /// The next node in the collection or Null if at the end of the collection
        /// </summary>
        public INodePointer MoveNext()
        {
            fPosition++;
            if( fPosition == 0 )
            {
                return this;
            }
            return null;
        }

        /// <summary>
        /// The previous node in the collection, or NULL if at the beginning of the collection
        /// </summary>
        public INodePointer MovePrevious()
        {
            fPosition--;
            if (fPosition == 0)
            {
                return this;
            }
            return null;
        }

 
    }
}
