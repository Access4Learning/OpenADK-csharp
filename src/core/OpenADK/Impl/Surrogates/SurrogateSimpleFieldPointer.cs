//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Impl.Surrogates
{
    internal class SurrogateSimpleFieldPointer : SurrogateElementPointer<Element>
    {
        public SurrogateSimpleFieldPointer( INodePointer parent, string fauxName, Element pointedNode,
                                            Boolean isAttribute )
            : base( parent, fauxName, pointedNode, isAttribute )
        {
        }


        public override void SetValue( Object value )
        {
            SetFieldValue( getElement(), value );
        }


        /// <summary>
        /// Returns a cloned instance of this NodePointer
        /// </summary>
        /// <returns></returns>
        public override INodePointer Clone()
        {
            return new SurrogateSimpleFieldPointer( this.Parent, this.Name, fElement, this.IsAttribute );
        }
    }
}
