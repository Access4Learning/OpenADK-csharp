//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;

namespace OpenADK.Library.Impl.Surrogates
{
    public class AbstractRenderSurrogate
    {
        protected IElementDef fElementDef;

        public AbstractRenderSurrogate( IElementDef def )
        {
            fElementDef = def;
        }

        protected static String ConsumeElementTextValue( XmlReader reader,
                                                         SifVersion version )

        {
            String value = null;
            try
            {
                value = ReadElementTextValue( reader );
                reader.Read();
            }
            catch ( Exception xse )
            {
                ThrowParseException( xse, reader.LocalName, version );
            }
            return value;
        }

        /// <summary>
        /// Consumes the text value of an element and returns with the
        /// XMLReader positioned on the EndElement
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected static String ReadElementTextValue( XmlReader reader )

        {
            String value = null;
            XmlNodeType nodeType = reader.NodeType;
            while ( nodeType == XmlNodeType.Element )
            {
                // If the element exists, return an empty string, rather than null
                value = "";
                reader.Read();
                nodeType = reader.NodeType;
            }
            if ( nodeType == XmlNodeType.Text )
            {
                value = reader.Value;
            }
            // Move the cursor to the end tag
            reader.Read();
            return value;
        }


        protected static void ThrowParseException( Exception ex,
                                                   String elementDefName,
                                                   SifVersion version )
        {
            throw new AdkParsingException
                ( "Unable to parse element or attribute '" + elementDefName + "' :" +
                  (ex == null ? "" : ex.Message) + " (SIF " + version.ToString() + ")", null, ex );
        }

        protected static void ThrowParseException( String errorMessage,
                                                   SifVersion version )
        {
            throw new AdkParsingException( errorMessage + " (SIF " + version.ToString() + ")", null );
        }

        protected static void WriteSimpleElement( XmlWriter writer,
                                                  String elementName,
                                                  String xmlValue )
        {
            writer.WriteElementString( elementName, xmlValue );
        }
    }
}
