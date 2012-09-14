//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Impl.Surrogates
{
    public abstract class SurrogateElementPointer<T> : FauxElementPointer
        where T : Element

    {
        protected T fElement;

        protected SurrogateElementPointer( INodePointer parent, String fauxName )
            : base( parent, fauxName, false )
        {
        }

        protected SurrogateElementPointer( INodePointer parent, String fauxName, T pointedNode, bool isAttribute )
            : base( parent, fauxName, isAttribute )
        {
            fElement = pointedNode;
        }


        protected void setElement( T node )
        {
            fElement = node;
        }

        protected T getElement()
        {
            return fElement;
        }

        public override object GetBaseValue()
        {
            return fElement;
        }

        public override Object Node
        {
            get { return fElement; }
        }
    }
}
