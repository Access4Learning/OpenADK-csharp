//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;

namespace OpenADK.Library.Tools.XPath
{
    internal class AdkAttributeIterator : AdkNodeIterator
    {
        internal AdkAttributeIterator( SifElementPointer parent ) : base( parent )
        {
            SifVersion version = parent.Version;

            SifElement node = (SifElement) parent.Node;

            // Capture all fields
            foreach ( SimpleField field in node.GetFields() )
            {
                IElementDef fieldDef = field.ElementDef;
                if ( fieldDef.IsSupported( version )
                     && fieldDef.IsAttribute( version ) )
                {
                    addNodeToIterate( field );
                }
            }
        }

        private AdkAttributeIterator( SifElementPointer parent, IList<Element> children ):base(parent )
        {
            fChildNodes = children;
        }


        /// <summary>
        /// Called on the subclass when a specific NodePointer is requested by SIFXPath
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override INodePointer GetNodePointer( SifElementPointer parent, Element element )
        {
            return SimpleFieldPointer.Create( parent, (SimpleField) element );
        }

        public override INodeIterator Clone()
        {
            AdkAttributeIterator clone = new AdkAttributeIterator(fParent, fChildNodes);
            clone.fPosition = fPosition;
            return clone;
        }
    }
}
