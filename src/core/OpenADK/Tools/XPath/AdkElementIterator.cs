//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;

namespace OpenADK.Library.Tools.XPath
{
    internal class AdkElementIterator : AdkNodeIterator
    {
        public AdkElementIterator( SifElementPointer parent, IList<Element> nodesToIterate )
            : base( parent, nodesToIterate )
        {
        }


        /// <summary>
        /// Called on the subclass when a specific NodePointer is requested by SIFXPath
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override INodePointer GetNodePointer( SifElementPointer parent, Element element )
        {
            if ( element is SimpleField )
            {
                return SimpleFieldPointer.Create( parent, (SimpleField) element );
            }
            else
            {
                return SifElementPointer.Create( parent, (SifElement) element, parent.Version );
            }
        }

        public override INodeIterator Clone()
        {
            AdkElementIterator clone = new AdkElementIterator( fParent, fChildNodes );
            clone.fPosition = fPosition;
            return clone;
        }
    }
}
