//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Impl.Surrogates
{
    internal class SIFTimeSurrogate : AbstractRenderSurrogate, IRenderSurrogate
    {
        public SIFTimeSurrogate( IElementDef def )
            : base( def )
        {
        }

        public void RenderRaw( XmlWriter writer,
                               SifVersion version,
                               Element o,
                               SifFormatter formatter )
        {
            String elementName = fElementDef.Name;
            SifTime time = (SifTime) o.SifValue;
            if ( time.Value.HasValue )
            {
                WriteSIFTime( writer, formatter, elementName, time.Value.Value );
            }
        }

        /// <summary>
        /// Writes a SIF 1.x &lt;SIF_Time&gt; element
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="formatter"></param>
        /// <param name="elementName"></param>
        /// <param name="time"></param>
        public static void WriteSIFTime(
            XmlWriter writer,
            SifFormatter formatter,
            String elementName,
            DateTime time )
        {
            String xmlTime = formatter.ToTimeString( time );
            writer.WriteStartElement( elementName );
            writer.WriteAttributeString( "Zone", Sif1xFormatter.FormatTimeZone( time ) );
            writer.WriteValue( xmlTime );
            writer.WriteEndElement();
        }

        public bool ReadRaw( XmlReader reader,
                             SifVersion version,
                             SifElement parent,
                             SifFormatter formatter )
        {
            String elementName = fElementDef.Name;
            if ( !reader.LocalName.Equals( elementName ) )
            {
                return false;
            }

            String value = ConsumeElementTextValue( reader, version );

            if ( value != null && value.Length > 0 )
            {
                DateTime? time = formatter.ToTime( value );
                SifTime sifTime = new SifTime( time );
                parent.SetField( sifTime.CreateField( parent, fElementDef ) );
            }
            return true;
        }

        /// <summary>
        /// Creates a child element, if supported by this node
        /// </summary>
        /// <param name="parentPointer"></param>
        /// <param name="formatter"></param>
        /// <param name="version"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public INodePointer CreateChild( INodePointer parentPointer, SifFormatter formatter, SifVersion version,
                                         SifXPathContext context )
        {
            // TODO Auto-generated method stub
            return null;
        }

        #region IRenderSurrogate Members

        /// <summary>
        /// Called by the ADK XPath traversal code when it is traversing the given element
        /// in a legacy version of SIF
        /// </summary>
        /// <param name="parentPointer">The parent element pointer</param>
        /// <param name="element">The Element to create a node pointer for</param>
        /// <param name="version">The SIFVersion in effect</param>
        /// <returns>A NodePointer representing the current element</returns>
        public INodePointer CreateNodePointer( INodePointer parentPointer, Element element, SifVersion version )
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Gets the element name or path to the element in this version of SIF
        /// </summary>
        public string Path
        {
            get { return "SIF_Time"; }
        }
    }
}
