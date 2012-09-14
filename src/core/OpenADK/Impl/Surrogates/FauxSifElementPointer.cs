//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml.XPath;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Impl.Surrogates
{
    /// <summary>
    /// Represents a NodePointer that wraps around a complex type represented
    /// by a SIFElement class. The SIF Element it wraps is assumed to have a single
    /// child node, which can be an attribute or text value
    /// </summary>
    internal class FauxSifElementPointer : FauxElementPointer
    {
        private FauxElementPointer fChild;
        private String fChildName;

        public FauxSifElementPointer( INodePointer parent, string fauxName )
            : base( parent, fauxName, false )
        {
        }

        /// <summary>
        /// returns the raw node pointed to by this pointer
        /// </summary>
        public override object Node
        {
            get
            {
                // This object doesnt actually represent "anything",
                // but return the child so that it maps to something
                return fChild.Node;
            }
        }

        public void SetChild( FauxElementPointer singleChild,
                              String childName
            )
        {
            fChild = singleChild;
            fChildName = childName;
        }


        public override Object GetBaseValue()
        {
            return null;
        }

        public override void SetValue( Object value )
        {
            Object childObj = fChild.Node;
            if ( childObj != null && childObj is Element )
            {
                SetFieldValue( (Element) childObj, value );
            }
        }

        public override INodePointer CreateAttribute( SifXPathContext context, string name )
        {
            if ( fChild.IsAttribute && fChildName.Equals( name ) )
            {
                return fChild;
            }
            return base.CreateAttribute( context, name );
        }

        public override INodePointer CreateChild( SifXPathContext context, string name, int i )
        {
            // Do nothing. The child has already been created
            return fChild;
        }


        /// <summary>
        /// One of the XPathNodeType values representing the current node. 
        /// </summary>
        public override XPathNodeType NodeType
        {
            get { return XPathNodeType.Element; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node 
        /// is an empty element without an end element tag. 
        /// </summary>
        public override bool IsEmptyElement
        {
            get { return fChild.IsAttribute; }
        }


        /// <summary>
        /// Returns a cloned instance of this NodePointer
        /// </summary>
        /// <returns></returns>
        public override INodePointer Clone()
        {
            FauxSifElementPointer fep = new FauxSifElementPointer( Parent, Name );
            fep.SetChild( fChild, fChildName );
            return fep;
        }


        /// <summary>
        /// Returns an INodeIterator that iterates instance representing the first attribute of 
        /// the current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        public override INodeIterator GetAttributes()
        {
            if ( fChild.IsAttribute )
            {
                return new SingleNodeIterator( fChild );
            }
            return null;
        }

        /// <summary>
        /// Returns an INodeIterator instance that iterates the children  
        /// of the current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        public override INodeIterator GetChildren()
        {
            if ( fChild.IsAttribute )
            {
                return null;
            }
            return new SingleNodeIterator( fChild );
        }
    }
}
