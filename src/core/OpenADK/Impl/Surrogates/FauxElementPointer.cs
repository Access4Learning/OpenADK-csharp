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
    /// Represents a node pointer that points to a path supported by the ADK from SIF 1.x, but
    /// for which there is no actual element. For Example, the GradYear/@Type attribute exists in SIF1.x,
    /// but doesn't exist in SIF 2.x. FauxElementPointer could be built based on a SIF 1.x XPath, but
    /// will actually point to the in-memory SIF 2.x field.
    /// </summary>
    public abstract class FauxElementPointer : AbstractNodePointer
    {
        private String fFieldName;
        private bool fIsAttribute;

        protected FauxElementPointer( INodePointer parent, String fauxName, bool isAttribute ) : base( parent )
        {
            fFieldName = fauxName;
            fIsAttribute = isAttribute;
        }

        public bool IsAttribute
        {
            get { return fIsAttribute; }
        }

        /// <summary>
        /// One of the XPathNodeType values representing the current node. 
        /// </summary>
        public override XPathNodeType NodeType
        {
            get
            {
                if ( IsAttribute )
                {
                    return XPathNodeType.Attribute;
                }
                else
                {
                    return XPathNodeType.Element;
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node 
        /// is an empty element without an end element tag. 
        /// </summary>
        public override bool IsEmptyElement
        {
            get { return false; }
        }

        public override string Name
        {
            get { return fFieldName; }
        }

        protected void SetFieldValue( Element field, Object value )
        {
            SifSimpleType sifValue = null;
            if ( value is SifSimpleType )
            {
                sifValue = (SifSimpleType) value;
            }
            else
            {
                sifValue = field.ElementDef.TypeConverter.GetSifSimpleType( value );
            }

            field.SifValue = sifValue;
        }
    }
}
