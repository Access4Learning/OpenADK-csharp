//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Xml.XPath;
using OpenADK.Library.Impl.Surrogates;

namespace OpenADK.Library.Tools.XPath
{
    public class SifElementPointer : AdkElementPointer
    {
        protected SifElement fSifElement;

        /// <summary>
        /// Creates a new SifElementPointer
        /// </summary>
        /// <param name="parentPointer">The parent of this pointer</param>
        /// <param name="element">The element being wrapped</param>
        /// <param name="version">The SifVersion in effect</param>
        public SifElementPointer(
            INodePointer parentPointer,
            SifElement element,
            SifVersion version )
            : base( parentPointer, element, version )
        {
            fSifElement = element;
        }


        /// <summary>
        /// The set of values that can be returned from the <code>getChildAddDirective<code>
        /// method
        /// </summary>
        public enum AddChildDirective
        {
            /// <summary>
            /// It is OK to add this child
            /// </summary>
            ADD,
            /// <summary>
            /// Don't add this child because the element is not repeatable and
            /// another already exists as a child of this element 
            /// </summary>
            DONT_ADD_NOT_REPEATABLE
        }

        /// <summary>
        /// Use by SIFXPathContext when determining if it can add a child
        /// or not
        /// </summary>
        /// <param name="childName"></param>
        /// <returns></returns>
        public AddChildDirective GetAddChildDirective( string childName )
        {
            SifElement sifElement = (SifElement) fElement;
            if ( sifElement.ChildCount == 0 )
            {
                // There are no children. Don't even check for repeatability.
                // This child can be added.
                return AddChildDirective.ADD;
            }
            IElementDef candidate = GetChildDef( childName );
            if ( candidate.Field )
            {
                // Don't evaluate repeatability
                return AddChildDirective.ADD;
            }

            SifElement instance = sifElement.GetChild( candidate );
            if ( instance == null )
            {
                // There are no siblings of this type. This child can be
                // added
                return AddChildDirective.ADD;
            }
            if ( candidate.IsRepeatable( Version ) )
            {
                // This element is repeatable. This child can be added
                return AddChildDirective.ADD;
            }

            // A sibling exists, and the element is not repeatable
            return AddChildDirective.DONT_ADD_NOT_REPEATABLE;
        }


        /// <summary>
        ///  Reutrns a child def with the requested name
        /// </summary>
        /// <param name="name">The name to look up</param>
        /// <returns>The ElementDef representing the requested name</returns>
        private IElementDef GetChildDef( string name )
        {
            IElementDef subEleDef = Adk.Dtd.LookupElementDef( fElementDef, name );
            if ( subEleDef == null )
            {
                subEleDef = Adk.Dtd.LookupElementDef( name );
                if ( subEleDef == null )
                    throw new ArgumentException( name
                                                 + " is not a recognized element of "
                                                 + fElementDef.Tag( Version ) );
            }
            return subEleDef;
        }


        /// <summary>
        /// One of the XPathNodeType values representing the current node. 
        /// </summary>
        public override XPathNodeType NodeType
        {
            get
            {
                if ( fElement.Parent == null )
                {
                    return XPathNodeType.Root;
                }
                else
                {
                    return XPathNodeType.Element;
                }
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current node is an empty element without an end element tag. 
        /// </summary>
        public override bool IsEmptyElement
        {
            get { return !(((SifElement) fElement).ChildCount > 0 || fElement.ElementDef.HasSimpleContent); }
        }


        /// <summary>
        /// Returns a cloned instance of this NodePointer
        /// </summary>
        /// <returns></returns>
        public override INodePointer Clone()
        {
            return new SifElementPointer( Parent, fSifElement, Version );
        }


        internal static INodePointer Create( INodePointer parent, SifElement element, SifVersion version )
        {
            if ( version.Major < 2 )
            {
                IElementDef fieldDef = element.ElementDef;
                IRenderSurrogate rs = fieldDef.GetVersionInfo( version ).GetSurrogate();
                if ( rs != null )
                {
                    return rs.CreateNodePointer( parent, element, version );
                }
            }
            return new SifElementPointer( parent, element, version );
        }


        public override INodePointer CreateAttribute( SifXPathContext context, String name )
        {
            IElementDef subEleDef = GetChildDef( name );
            SifElement sifElement = (SifElement) fElement;

            if ( subEleDef.Field )
            {
                SifSimpleType ssf = subEleDef.TypeConverter.GetSifSimpleType( null );
                SimpleField sf = ssf.CreateField( sifElement, subEleDef );
                sifElement.SetField( sf );
                return SimpleFieldPointer.Create( this, sf );
            }

            throw new ArgumentException(
                "Factory could not create a child node for path: " + Name
                + "/" + name );
        }

        public override INodePointer CreateChild( SifXPathContext context, string name, int i )
        {
            SifVersion version = Version;
            IElementDef subEleDef = GetChildDef( name );
            SifFormatter formatter = Adk.Dtd.GetFormatter( version );
            SifElement sifElement = (SifElement) fElement;

            // Check to see if this child has a render surrogate defined
            IRenderSurrogate rs = subEleDef.GetVersionInfo( version ).GetSurrogate();
            if ( rs != null )
            {
                return rs.CreateChild( this, formatter, version, context );
            }

            if ( subEleDef.Field )
            {
                SifSimpleType ssf = subEleDef.TypeConverter.GetSifSimpleType( null );
                SimpleField sf = formatter.SetField( sifElement, subEleDef, ssf, version );
                return SimpleFieldPointer.Create( this, sf );
            }
            else
            {
                SifElement newEle = SifElement.Create( sifElement, subEleDef );
                formatter.AddChild( sifElement, newEle, version );
                return new SifElementPointer( this, newEle, version );
            }
        }

        public override void SetValue( object rawValue )
        {
            SifSimpleType sst = GetSIFSimpleTypeValue( fElementDef, rawValue );
            fElement.SifValue = sst;
        }

        /// <summary>
        /// Returns an INodeIterator that iterates instance representing the first attribute of 
        /// the current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        public override INodeIterator GetAttributes()
        {
            return new AdkAttributeIterator( this );
        }

        /// <summary>
        /// Returns an INodeIterator instance that iterates the children  
        /// of the current node; otherwise, <c>Null</c>. 
        /// </summary>
        /// <returns></returns>
        public override INodeIterator GetChildren()
        {
            SifVersion version = Version;


            // Get all of the Element fields and children that match the
            // NodeTest into a list
            SifFormatter formatter = Adk.Dtd.GetFormatter( version );
            IList<Element> elements = formatter.GetContent( fSifElement, version );
            
            if( elements.Count == 0 )
            {
                return null;
            } else if ( elements.Count == 1 )
            {
                Element singleChild = elements[0];
                if ( singleChild is SimpleField )
                {
                    return new SingleNodeIterator( SimpleFieldPointer.Create( this, (SimpleField) singleChild ) );
                }
                else
                {
                    return new SingleNodeIterator( Create( this, (SifElement) singleChild, version ) );
                }
            }


            return new AdkElementIterator( this, elements );
        }
    }
}
