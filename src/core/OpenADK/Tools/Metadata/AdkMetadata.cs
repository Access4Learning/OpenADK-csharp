//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using Edustructures.SifWorks.Impl;

namespace Edustructures.SifWorks.Tools.Metadata
{
    public class AdkMetadata
    {
        /// <summary>
        /// Metadata flag: Identifies a repeatable element
        /// </summary>
        public const byte MD_REPEATABLE = ElementDefImpl.FD_REPEATABLE;


        private static AdkMetadata gSingleton = null;

        private AdkMetadata() {}

        public static AdkMetadata GetInstance()
        {
            if ( !Adk.Initialized ) {
                throw new InvalidOperationException( "ADK is not initialized" );
            }

            if ( gSingleton == null ) {
                gSingleton = new AdkMetadata();
            }

            return gSingleton;
        }


        /// <summary>
        /// Define a top-level SIF Data Object
        /// </summary>
        /// <param name="name">The tag name of the data object</param>
        /// <param name="earliestVersion">The earliest version of the specification this attribute should
        /// be recognized in</param>
        /// <returns>An IElementDef instance encapsulating metadata for this data object</returns>
        public IElementDef DefineDataObject( String name,
                                             SifVersion earliestVersion )
        {
            IElementDef ed =
                new ElementDefImpl
                    ( null, name, null, 0, "custom", ElementDefImpl.FD_OBJECT, earliestVersion, SifVersion.LATEST );
            SifDtd.sElementDefs.Add( name, ed );
            return ed;
        }


        /// <summary>
        /// Define a complex element
        /// </summary>
        /// <param name="name">The tag name of the element</param>
        /// <param name="earliestVersion">The earliest version of the specification this attribute should
        /// be recognized in</param>
        /// <returns></returns>
        /// <remarks>
        /// 	A complex element has attributes and/or elements of its own and is represented 
        /// 	by its own class. Complex elements must be defined with this method, and then 
        /// 	added as a child element to a SIF Data Object by calling <see cref="DefineChildElement"/>.
        /// </remarks>
        public IElementDef DefineElement( String name,
                                          SifVersion earliestVersion )
        {
            return new ElementDefImpl( null, name, null, 0, "custom", (byte) 0, earliestVersion, SifVersion.LATEST );
        }

        /// <summary>
        /// Define an attribute of a SIF Data Object.
        /// </summary>
        /// <param name="parent">The IElementDef constant identifying the parent data object</param>
        /// <param name="name">The tag name of the attribute</param>
        /// <param name="sequence">The zero-based sequence number of the attribute</param>
        /// <param name="earliestVersion">The earliest version of the specification this attribute should
        /// be recognized in</param>
        /// <returns>An IElementDef instance encapsulating metadata for this attribute</returns>
        public IElementDef DefineAttribute( IElementDef parent,
                                            String name,
                                            int sequence,
                                            SifVersion earliestVersion )
        {
            if ( parent == null ) {
                throw new ArgumentException( "Parent cannot be null" );
            }

            IElementDef ed =
                new ElementDefImpl
                    ( parent, name, null, sequence, "custom", ElementDefImpl.FD_ATTRIBUTE,
                      earliestVersion, SifVersion.LATEST );
            SifDtd.sElementDefs.Add( parent.Name + "_" + name, ed );
            return ed;
        }


        /// <summary>Define a field of a SIF Data Object.</summary>
        /// <remarks>
        /// A field is a simple child element that has no attributes or elements of its own and 
        /// is not represented by its own class. For example, the <c>&lt;LocalId&gt;</c> 
        /// common element in SIF 1.5. Internally, the ADK stores fields more efficiently than
        /// complex elements.
        /// </remarks>
        /// <param name="parent">The IElementDef constant identifying the parent data object or element</param>
        /// <param name="name">The tag name of the element</param>
        /// <param name="sequence">The zero-based sequence number of the element</param>
        /// <param name="flags">Optional flags for this field (e.g. <c>MD_REPEATABLE</c>), or zero
        /// if no flags are applicable</param>
        /// <param name="earliestVersion">The earliest version of the specification this element should
        /// be recognized in</param>
        /// <returns></returns>
        public IElementDef DefineField( IElementDef parent,
                                        String name,
                                        int sequence,
                                        byte flags,
                                        SifVersion earliestVersion )
        {
            if ( parent == null ) {
                throw new ArgumentException( "Parent cannot be null" );
            }

            IElementDef ed =
                new ElementDefImpl
                    ( parent, name, null, sequence, "custom",
                      (byte) (ElementDefImpl.FD_FIELD | flags), earliestVersion, SifVersion.LATEST );
            SifDtd.sElementDefs.Add( parent.Name + "_" + name, ed );
            return ed;
        }


        /// <summary>
        /// Define a complex child element of a SIF Data Object.
        /// </summary>
        /// <param name="parent">The IElementDef constant identifying the parent data object or element</param>
        /// <param name="element">The IElementDef constant of an existing element, such as a
        /// Common Element (e.g. <c>SifDtd.NAME</c>)</param>
        /// <param name="sequence">The zero-based sequence number of the element</param>
        /// <param name="flags">Optional flags for this field (e.g. <c>MD_REPEATABLE</c>), or zero
        /// if no flags are applicable</param>
        /// <param name="earliestVersion">The earliest version of the specification this element should
        /// be recognized in</param>
        /// <returns>An IElementDef instance encapsulating metadata for this element</returns>
        public IElementDef DefineChildElement( IElementDef parent,
                                               IElementDef element,
                                               int sequence,
                                               byte flags,
                                               SifVersion earliestVersion )
        {
            if ( parent == null ) {
                throw new ArgumentException( "Parent cannot be null" );
            }
            if ( element == null ) {
                throw new ArgumentException( "Element cannot be null" );
            }

            IElementDef ed =
                new ElementDefImpl
                    ( parent, element.Name, null, sequence, element.Package, flags, earliestVersion, SifVersion.LATEST );
            SifDtd.sElementDefs.Add( parent.Name + "_" + element.Name, ed );
            return ed;
        }
    }
}
