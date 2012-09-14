//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  A SifDataObject implementation that wraps raw XML messages.
    /// 
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class SifDataObjectXml : SifDataObject
    {
        /// <summary>  Sets the raw XML message content</summary>
        public virtual String Xml
        {
            set { fXml = value; }
        }

        protected internal String fXml;

        /// <summary>  Constructs a SifDataObject with XML content</summary>
        /// <param name="def">The ElementDef constant from the SifDtd class identifying
        /// the data object represented by the XML
        /// </param>
        /// <param name="xml">The XML content
        /// </param>
        public SifDataObjectXml( IElementDef def,
                                 String xml )
            : base( Adk.SifVersion, def )
        {
            fXml = xml;
        }


        /// <summary>
        /// This property always throws an exception. RefId cannot be set or gotten from a SifDataObjectXml instance.
        /// </summary>
        /// <exception cref="OpenADK.Library.AdkNotSupportedException">Always thrown from this property</exception>
        public override String RefId
        {
            get
            {
                throw new AdkNotSupportedException
                    ( "The RefId Get method cannot be called on SifDataObjectXml instances", null );
            }
            set
            {
                throw new AdkNotSupportedException
                    ( "The RefId Set method cannot be called on SifDataObjectXml instances", null );
            }
        }

        /// <summary>  Constructs a SifDataObject with XML content</summary>
        /// <param name="version">The version of SIF to associate with this object</param>
        /// <param name="def">The ElementDef constant from the SifDtd class identifying
        /// the data object represented by the XML</param>
        /// <param name="xml">The XML content</param>
        public SifDataObjectXml( SifVersion version,
                                 IElementDef def,
                                 String xml )
            : base( version, def )
        {
            fXml = xml;
        }

        /// <summary>  Gets the raw XML message content</summary>
        public override String ToXml()
        {
            return fXml;
        }

        /// <summary>
        /// This object does not define a key field
        /// </summary>
        /// <returns></returns>
        public override IElementDef[] KeyFields
        {
            get { return new IElementDef[0]; }
        }
    }
}
