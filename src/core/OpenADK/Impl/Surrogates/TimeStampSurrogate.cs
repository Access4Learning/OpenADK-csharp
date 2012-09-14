//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using OpenADK.Library.Infra;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Impl.Surrogates
{
    /// <summary>
    /// Surrogate for rendering or reading SIF 1.x Date and Time fields into the
    /// new SIF 2.0 DateTime datatype
    /// </summary>
    internal class TimeStampSurrogate : AbstractRenderSurrogate, IRenderSurrogate
    {
        private String fDateElement = "Date";
        private String fTimeElement = "Time";

        public TimeStampSurrogate( IElementDef def )
            : base( def )
        {
            if ( def == InfraDTD.SIF_HEADER_SIF_TIMESTAMP )
            {
                fDateElement = "SIF_Date";
                fTimeElement = "SIF_Time";
            }
        }

        public void RenderRaw( XmlWriter writer,
                               SifVersion version,
                               Element o,
                               SifFormatter formatter )
        {
            DateTime? timeStamp = ((SifDateTime) o.SifValue).Value;
            if ( timeStamp.HasValue )
            {
                String xmlDate = formatter.ToDateString( timeStamp );
                WriteSimpleElement( writer, fDateElement, xmlDate );
                SIFTimeSurrogate.WriteSIFTime( writer, formatter, fTimeElement, timeStamp.Value );
            }
        }

        public bool ReadRaw( XmlReader reader,
                             SifVersion version,
                             SifElement parent,
                             SifFormatter formatter )
        {
            string name = reader.LocalName;

            if ( name.Equals( fDateElement ) )
            {
                String dateValue = ConsumeElementTextValue( reader, version );
                DateTime? date = formatter.ToDate( dateValue );
                if ( date.HasValue )
                {
                    DateTime tsValue = date.Value;
                    SifDateTime dateTime = new SifDateTime( tsValue );
                    parent.SetField( dateTime.CreateField( parent, fElementDef ) );
                }
            }
            else if ( name.Equals( fTimeElement ) )
            {
                String timeValue = ConsumeElementTextValue( reader, version );
                DateTime? time = formatter.ToTime( timeValue );
                if ( time.HasValue )
                {
                    DateTime val = time.Value;
                    // See if the Timestamp field already exists on the parent
                    SimpleField timeStampField = parent.GetField( fElementDef );
                    if ( timeStampField == null )
                    {
                        // Doesn't exist, create it
                        SifDateTime dateTime = new SifDateTime( val );
                        parent.SetField( dateTime.CreateField( parent, fElementDef ) );
                    }
                    else
                    {
                        // Exists, update the time portion of the date
                        SifDateTime sdt = (SifDateTime) timeStampField.SifValue;
                        if ( sdt != null && sdt.Value.HasValue )
                        {
                            DateTime tsValue = sdt.Value.Value;
                            tsValue = tsValue.Add( val.TimeOfDay );
                            sdt = new SifDateTime( tsValue );
                            // Overwrite the current value
                            parent.SetField( sdt.CreateField( parent, fElementDef ) );
                        }
                    }
                }
            }
            else
            {
                return false;
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
            return null;
        }


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


        /// <summary>
        /// Gets the element name or path to the element in this version of SIF
        /// </summary>
        public string Path
        {
            get { return "SIF_Date"; }
        }
    }
}
