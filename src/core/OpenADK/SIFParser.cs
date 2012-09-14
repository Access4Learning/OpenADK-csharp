//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using OpenADK.Library.Global;
using OpenADK.Library.Impl;
using OpenADK.Library.Impl.Surrogates;
using OpenADK.Library.Infra;
using OpenADK.Util;
using log4net;

namespace OpenADK.Library
{
    /// <summary>
    /// Summary description for SifParser.
    /// </summary>
    public sealed class SifParser
    {
        /// <summary>
        /// The constructor is made private so that access can only be done using the 
        /// factory method.
        /// </summary>
        private SifParser()
        {
        }

        /// <summary>
        /// Factory method for creating a new instance of a Sif Parser
        /// </summary>
        /// <returns></returns>
        public static SifParser NewInstance()
        {
            return new SifParser();
        }

        /// <summary>
        /// Parses Xml text into a SIFElement
        /// </summary>
        /// <param name="str"></param>
        /// <returns>A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)</returns>
        /// <exception cref="OpenADK.Library.AdkParsingException"> AdkParsingException is thrown if unable to parse the message
        /// </exception>
        public SifElement Parse( string str )
        {
            return Parse( str, null );
        }

        /// <summary>  Parses a SIF data element into a <c>SifElement</c></summary>
        /// <param name="msg">The content to parse</param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known</param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)</returns>
        /// <exception cref="OpenADK.Library.AdkParsingException"> AdkParsingException is thrown if unable to parse the message
        /// </exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading the message content
        /// </exception>
        public SifElement Parse( string msg,
                                 IZone zone )
        {
            return Parse( msg, zone, 0, null );
        }

        /// <summary>Parses a SIF data element into a <c>SifElement</c></summary>
        /// <param name="msg">The content to parse</param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known</param>
        /// <param name="flags">One or more SifParserFlags</param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)</returns>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        public SifElement Parse( string msg,
                                 IZone zone,
                                 SifParserFlags flags )
        {
            return Parse( msg, zone, flags, null );
        }

        /// <summary>  Parses a SIF data element into a <c>SifElement</c>.
        /// 
        /// </summary>
        /// <param name="msg">The content to parse
        /// </param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known
        /// </param>
        /// <param name="flags">One or more <c>FLG_</c> constants, or zero if no
        /// flags are applicable
        /// </param>
        /// <param name="version">The version of SIF that will be associated with the
        /// returned object. By default, SifParser uses the default version of
        /// SIF in effect for the agent when parsing messages that do not have
        /// a SIF_Message envelope. By specifying a value to this parameter, you
        /// can change the version of SIF associated with the returned object in
        /// the event there is no SIF_Message envelope present in the XML
        /// content. Note that when parsing XML content with a SIF_Message
        /// envelope, SifParser ignores this parameter and instead uses the
        /// version indicated by the <i>Version</i> and <i>xmlns</i> attributes
        /// 
        /// </param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)
        /// 
        /// </returns>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        public SifElement Parse( string msg,
                                 IZone zone,
                                 SifParserFlags flags,
                                 SifVersion version )
        {
            using ( StringReader reader = new StringReader( msg ) )
            {
                SifElement element = Parse( reader, zone, flags, version );
                reader.Close();
                return element;
            }
        }

        /// <summary>  Parses a SIF data element into a <c>SifElement</c>.</summary>
        /// <param name="msg">The content to parse</param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known</param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)</returns>
        /// <remarks>
        /// In order to be SIFCompliant, the TextReader must be using UTF-8 Encoding to read the underlying binary data
        /// </remarks>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        public SifElement Parse( TextReader msg,
                                 IZone zone )
        {
            return Parse( msg, zone, 0, null );
        }

        /// <summary>  Parses a SIF data element into a <c>SifElement</c>.
        /// </summary>
        /// <param name="msg">The content to parse
        /// </param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known
        /// </param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)
        /// </returns>
        ///  <remarks>
        /// In order to be SIFCompliant, the TextReader must be using UTF-8 Encoding to read the underlying binary data
        /// </remarks>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        /// <param name="flags">Reserved for future use</param>
        public SifElement Parse( TextReader msg,
                                 IZone zone,
                                 SifParserFlags flags )
        {
            return Parse( msg, zone, flags, null );
        }

        /// <summary>  Parses a SIF data element into a <c>SifElement</c>.</summary>
        /// <param name="msg">The content to parse</param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known</param>
        /// <param name="flags">The flags to use for parsing</param>
        /// <param name="version">The version of SIF that will be associated with the
        /// returned object.</param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)
        /// </returns>
        /// <remarks>
        /// <note type="note">In order to be SIF Compliant, the TextReader must be set to use UTF-8 encoding</note>
        ///  By default, SifParser uses the default version of
        /// SIF in effect for the agent when parsing messages that do not have
        /// a SIF_Message envelope. By specifying a value to this parameter, you
        /// can change the version of SIF associated with the returned object in
        /// the event there is no SIF_Message envelope present in the XML
        /// content. Note that when parsing XML content with a SIF_Message
        /// envelope, SifParser ignores this parameter and instead uses the
        /// version indicated by the <c>Version</c> and <c>xmlns</c> attributes
        /// </remarks>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        public SifElement Parse( TextReader msg,
                                 IZone zone,
                                 SifParserFlags flags,
                                 SifVersion version )
        {
            // TODO: Document the fact that encoding needs to be properly set on the TextReader for SIF Support
            XmlTextReader reader = new XmlTextReader( msg );
            reader.WhitespaceHandling = WhitespaceHandling.None;
            return Parse( reader, zone, flags, version );
        }

        /// <summary>
        /// Parses a SIF object from the binary data stream
        /// </summary>
        /// <param name="msg">The stream containing the Xml content to parse</param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known</param>
        /// <param name="flags">The flags to use for parsing</param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)</returns>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        public SifElement Parse( Stream msg,
                                 IZone zone,
                                 SifParserFlags flags )
        {
           return Parse( msg, zone, flags, null );
        }

        /// <summary>
        /// Parses a SIF object from the binary data stream
        /// </summary>
        /// <param name="msg">The stream containing the Xml content to parse</param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known</param>
        /// <param name="flags">The flags to use for parsing</param>
        /// <param name="version">The SifVersion to use for this parsing operation</param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)</returns>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
         public SifElement Parse( Stream msg, IZone zone, SifParserFlags flags, SifVersion version )
        {
            XmlTextReader reader =
                new XmlTextReader( new StreamReader( msg, SifIOFormatter.ENCODING ) );
            reader.WhitespaceHandling = WhitespaceHandling.None;
            return Parse( reader, zone, flags, version );
        }

        /// <summary>
        /// Parses a SIF object from the binary data stream
        /// </summary>
        /// <param name="msg">The stream containing the Xml content to parse</param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known</param>
        /// <returns>A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)</returns>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        public SifElement Parse( Stream msg,
                                 IZone zone )
        {
            return Parse( msg, zone, SifParserFlags.None );
        }

        /// <summary>
        /// Parses the source of the XmlReader into a SifElement object.
        /// </summary>
        /// <param name="reader">The reader containing the Xml data to be parsed</param>
        /// <param name="zone">The Zone from which the message was received, or null if
        /// not applicable or not known</param>
        /// <param name="flags">The flags to use for parsing</param>
        /// <param name="version">The version of SIF that will be associated with the
        /// returned object.</param>
        /// <returns> A SifElement object encapsulating the message payload (e.g.
        /// a OpenADK.Library.us.Student.StudentPersonal object)</returns>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        public SifElement Parse( XmlReader reader,
                                 IZone zone,
                                 SifParserFlags flags,
                                 SifVersion version )
        {
            try
            {
                reader.MoveToContent();
                if ( reader.LocalName == "SIF_Message" )
                {
                    SifElement element = ReadSIFMessageElement( reader, Adk.Dtd, zone, flags, version );
                    return element.GetChildList()[0];
                }
                else
                {
                    version = ParseVersion( reader, Adk.Dtd, zone, flags, version );
                    return ParseElementStream( reader, version, Adk.Dtd, zone, flags );
                }
            } 
            catch( XmlException xmle )
            {
                throw new AdkParsingException( xmle.Message, zone, xmle );
            }
        }


        /// <summary>
        /// Reads a SIF_Message element, which sets the version and namespace scope for the rest of the 
        /// xml parsing
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="dtd"></param>
        /// <param name="zone"></param>
        /// <param name="flags"></param>
        /// <param name="defaultVersion"></param>
        /// <returns></returns>
        /// <exception cref="OpenADK.Library.AdkParsingException">AdkParsingException is thrown if unable to 
        /// parse the message</exception>
        /// <exception cref="System.IO.IOException"> IOException is thrown if an error is reported while reading 
        /// the message content</exception>
        private SifElement ReadSIFMessageElement(
            XmlReader reader,
            IDtd dtd,
            IZone zone,
            SifParserFlags flags,
            SifVersion defaultVersion )
        {
            SifVersion version = ParseVersion( reader, dtd, zone, flags, defaultVersion );


            SIF_Message message = new SIF_Message();
            // Set the namespace from our working version
            message.SetXmlns( version.Xmlns );
            if ( version.CompareTo( SifVersion.SIF11 ) >= 0 )
            {
                // If we are at SifVersion 1.1 or greater, set the version attribute
                message.SetVersionAttribute( version.ToString() );
            }

            // Advance to the next element
            if ( reader.Read() )
            {
                while ( reader.NodeType != XmlNodeType.Element )
                {
                    if ( !reader.Read() )
                    {
                        break;
                    }
                }
                if ( reader.NodeType == XmlNodeType.Element )
                {
                    SifElement element = ParseElementStream( reader, version, dtd, zone, flags );
                    message.AddChild( element );
                }
            }

            return message;
        }


        /// <summary>
        /// Parses the SIF Version from the version attribute or namespace. If not able
        /// to parse the version, the default version is returned.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="dtd"></param>
        /// <param name="zone"></param>
        /// <param name="flags"></param>
        /// <param name="defaultVersion"></param>
        /// <returns></returns>
        private SifVersion ParseVersion(
            XmlReader reader,
            IDtd dtd,
            IZone zone,
            SifParserFlags flags,
            SifVersion defaultVersion)
        {
            SifVersion version;
            String verAttr = reader.GetAttribute( "Version" );


            // Order of precedence:
            // 1) Version attribute of message
            // 2) The version passed in (if not null)
            // 3) The namespace version (if able to parse)
            // 4) The ADK SIF Version

            if ( verAttr != null )
            {
                version = SifVersion.Parse( verAttr );
            }
            else if ( defaultVersion != null )
            {
                version = defaultVersion;
            }
            else
            {
                String namespc = reader.NamespaceURI;
                version = SifVersion.ParseXmlns( namespc );
                if ( version == null )
                {
                    version = Adk.SifVersion;
                }
            }

            // Do validation on the version
            if ( !Adk.IsSIFVersionSupported( version ) )
            {
                throw new SifException(
                    SifErrorCategoryCode.Generic,
                    SifErrorCodes.GENERIC_VERSION_NOT_SUPPORTED_3,
                    string.Format( "SIF {0} not supported", version.ToString() ), reader.NamespaceURI, zone );
            }
            else if ( zone != null && zone.Properties.StrictVersioning )
            {
                if ( version.CompareTo( Adk.SifVersion ) != 0 )
                {
                    throw new SifException(
                        SifErrorCategoryCode.Generic,
                        SifErrorCodes.GENERIC_VERSION_NOT_SUPPORTED_3,
                        "SIF " + version.ToString() + " message support disabled by this agent",
                        string.Format( "This agent is running in strict SIF {0} mode", Adk.SifVersion.ToString() ), zone );
                }
            }
            return version;
        }


        private SifElement ParseElementStream( XmlReader reader,
                                               SifVersion version,
                                               IDtd dtd,
                                               IZone zone,
                                               SifParserFlags flags )
        {
            bool legacyParse = version.CompareTo( SifVersion.SIF20 ) < 0;

            // The current SIFElement being parsed
            SifElement currentElement = null;
            // The actual tag name of the current element
            SifFormatter formatter = Adk.Dtd.GetFormatter( version );
            reader.MoveToContent();
            bool doneParsing = false;
            while ( !(reader.EOF || doneParsing) )
            {
                switch ( reader.NodeType )
                {
                    case XmlNodeType.Element:
                        if ( reader.LocalName == "SIF_Message" )
                        {
                            // Special case for embedded SIF_Message envelopes
                            if ( (flags & SifParserFlags.ExpectInnerEnvelope) != 0 )
                            {
                                SifElement msgElement =
                                    ReadSIFMessageElement( reader, dtd, zone, SifParserFlags.None, version );
                                currentElement.AddChild( msgElement );
                                currentElement = msgElement;
                            }
                            else
                            {
                                throw new AdkParsingException
                                    ( "Unexpected SIF_Message encountered in parsing", zone );
                            }
                        }
                        else
                        {
                            String xmlName = reader.LocalName;
                            if( xmlName == "Teacher" )
                            {
                                Console.WriteLine( "Ready to break" );
                            }
                
                            IElementDef foundDef = LookupElementDef( currentElement, reader, dtd, version, zone );
                            if (foundDef == null)
                            {
                                if (legacyParse)
                                {
                                    ParseLegacyXML(reader, version, zone, currentElement, formatter, xmlName);
                                    continue;
                                }
                                else if (currentElement != null && currentElement.ElementDef.Name.Equals("XMLData"))
                                {
                                    // Parse this into a DOM and set on the XMLData
                                    // element
                                    XmlReader nestedReader = reader.ReadSubtree();
                                    XmlDocument doc = new XmlDocument();
                                    doc.Load( nestedReader );
                                    ((XMLData)currentElement).Xml = doc;
                                    continue;
                                }
                                else
                                {
                                    String _tag = currentElement != null ? currentElement.ElementDef.Name
                                            + "/" + xmlName
                                            : xmlName;
                                    throw new SifException( SifErrorCategoryCode.Xml, SifErrorCodes.XML_GENERIC_VALIDATION_3, "Unknown element or attribute", _tag
                                            + " is not a recognized element of SIF "
                                            + version.ToString(), zone);
                                }
                            }
                      
                            if ( legacyParse )
                            {
                                IElementVersionInfo evi = foundDef.GetVersionInfo( version );
                                if (evi != null)
                                {
                                    IRenderSurrogate rs = evi.GetSurrogate();
                                    if (rs != null)
                                    {

                                        using (XmlReader subtreeReader = reader.ReadSubtree())
                                        {
                                            bool shouldContinue = true;
                                            subtreeReader.Read();
                                            try
                                            {
                                                shouldContinue = rs.ReadRaw( subtreeReader, version, currentElement, formatter );
                                            }
                                            catch ( AdkTypeParseException atpe )
                                            {
                                                HandleTypeParseException( "Unable to parse value: " + atpe.Message, atpe,
                                                                          zone );
                                            }
                                            subtreeReader.Close();
                                            // advance to the next tag
                                            reader.Read();
                                            if ( shouldContinue )
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                throw new SifException( SifErrorCategoryCode.Xml,
                                                                        SifErrorCodes.XML_GENERIC_VALIDATION_3,
                                                                        "Unknown element or attribute", reader.LocalName
                                                                                                        +
                                                                                                        " was not able to be parsed by "
                                                                                                        + rs, zone );
                                            }
                                        }
                                    }
                                }
                            }


                            if ( foundDef.Field )
                            {
                                SetFieldValueFromElement
                                    ( foundDef, currentElement, reader, version, formatter, zone );
                                // Advance to the next tag
                                do
                                {
                                    reader.Read();
                                } while (
                                    !(reader.EOF || reader.NodeType == XmlNodeType.Element ||
                                      reader.NodeType == XmlNodeType.EndElement) );
                                continue;
                            }
                            else if ( reader.IsEmptyElement )
                            {
                                // The .Net XmlReader does not return an EndElement event for
                                // tags with empty content. Therefore, this region of the code is
                                // slightly different from Java
                                ReadSifElementFromElementNode( foundDef, reader, dtd, currentElement, formatter,
                                                               version, zone );
                            }
                            else
                            {
                                currentElement =
                                    ReadSifElementFromElementNode( foundDef, reader, dtd, currentElement, formatter,
                                                                   version, zone );
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        if ( currentElement.ElementDef.HasSimpleContent )
                        {
                            SetFieldValueFromElement( currentElement.ElementDef, currentElement, reader, version,
                                                      formatter, zone );
                            // The XML Reader cursor is automatically advanced by this method, so we
                            // need to continue on without calling read()
                            continue;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ( currentElement.Parent != null )
                        {
                            currentElement = (SifElement) currentElement.Parent;
                            while ( legacyParse && currentElement.ElementDef.IsCollapsed( version ) )
                            {
                                currentElement = (SifElement) currentElement.Parent;
                            }
                        }
                        if ( reader.LocalName == "SIF_Message" )
                        {
                            // We need to return here. If we let the reader keep reading, and we are reading an embedded 
                            // SIF_Message, it will keep parsing the end tags and not let the stack of SifElement objects
                            // propertly unwind. We're done anyway.
                            doneParsing = true;
                        }
                        break;
                }

                // Advance the cursor
                reader.Read();
            }


            if ( currentElement == null )
            {
                return null;
            }
            else
            {
                // Now, unwind and pop off the top element parsed
                Element top = currentElement;
                Element current;
                while ( (current = top.Parent) != null )
                {
                    top = current;
                }
                return (SifElement) top;
            }
        }


        private SifElement ReadSifElementFromElementNode(
            IElementDef def,
            XmlReader reader,
            IDtd dtd,
            SifElement parent,
            SifFormatter formatter,
            SifVersion version,
            IZone zone )
        {
            SifElement element;
            try
            {
                element = SifElement.Create( parent, def );
            }
            catch ( TypeLoadException tle )
            {
                throw new AdkParsingException
                    ( "Could not create an instance of " + def.FQClassName + " to wrap a " +
                      reader.LocalName + " element because that class doesn't exist", zone, tle );
            }
            catch ( Exception ex )
            {
                throw new AdkParsingException
                    ( "Could not create an instance of " + def.FQClassName, zone, ex );
            }

            element.ElementDef = def;
            element.SifVersion = version;

            if ( parent != null )
            {
                element = formatter.AddChild( parent, element, version );
            }

            // Set the attributes to fields of the SifElement
            while ( reader.MoveToNextAttribute() )
            {
                SetFieldValueFromAttribute( element, reader, dtd, version, formatter, zone );
            }

            return element;
        }

        private IElementDef LookupElementDef( SifElement parent,
                                              XmlReader reader,
                                              IDtd dtd,
                                              SifVersion version,
                                              IZone zone )
        {
            //  Lookup the ElementDef metadata in the SifDtd object for the
            //  version of SIF we are parsing. First try looking up a ElementDef
            //  for a field or complex object that is a child of another element,
            //  such as StudentPersonal_Name, SIF_Ack_SIF_Header, etc. If none
            //  found then look for a root-level element such as StudentPersonal,
            //  SIF_Ack, etc. If still nothing is found we don't know how to
            //  parse this element -- it is neither a top-level object element
            //  nor a child field element for this version of SIF.
            String elementName = reader.LocalName;
            IElementDef def = null;
            if ( parent != null )
            {
                def = dtd.LookupElementDef( parent.ElementDef, elementName );
            }

            if ( def == null )
            {
                def = dtd.LookupElementDef( elementName );
            }

            //	Beginning with SIF 1.5 *any* object can have a SIF_ExtendedElements
            //	child, so we need to check for that case since the Adk metadata 
            //	does not add SIF_ExtendedElements to all object types
            if ( def == null && elementName.Equals( "SIF_ExtendedElements" ) )
            {
                def = GlobalDTD.SIF_EXTENDEDELEMENTS;
            }

            //	Beginning with SIF 2.0 *any* object can have a SIF_ExtendedElements
            //	child, so we need to check for that case since the Adk metadata 
            //	does not add SIF_ExtendedElements to all object types
            if ( def == null && elementName.Equals( "SIF_Metadata" ) )
            {
                // TODO: Add support for SIF_Metadata back in to the .NET ADK
                def = null; // DatamodelDTD.SIF_METADATA;
            }

            // Note: def returned can be null.
            return def;
        }

        private void SetFieldValueFromAttribute(
            SifElement element,
            XmlReader reader,
            IDtd dtd,
            SifVersion version,
            SifFormatter formatter,
            IZone zone )
        {

            IElementDef elementDef = element.ElementDef;
            IElementDef field = dtd.LookupElementDef( element.ElementDef, reader.LocalName );
            if ( field == null && reader.Prefix != null )
            {
                if(reader.LocalName == SifWriter.NIL && reader.NamespaceURI == XmlSchema.InstanceNamespace )
                {
                    TypeConverter converter = elementDef.TypeConverter;
                    if ( converter != null )
                    {
                        SifSimpleType sst = converter.GetSifSimpleType( null );
                        element.SetField(elementDef, sst);
                    }
                    return;
                }
                else if( reader.Name.StartsWith( "xmlns" ) )
                {
                    return;
                }
                else
                {
                    field = dtd.LookupElementDef(elementDef, reader.Prefix + ":" + reader.LocalName);
                }
            }

            if( field != null )
            {
                string strVal = reader.Value.Trim();
                SifSimpleType val = ParseValue( field, strVal, version, formatter, zone );
                element.SetField( field, val );
            }
            else if (element.ElementDef.EarliestVersion >= SifVersion.SIF20 && version < SifVersion.SIF20)
            {
                Adk.Log.Warn("Field " + element.ElementDef.ClassName + "." + (reader.Prefix == null ? reader.LocalName : reader.Prefix + ":" + reader.LocalName ) + " does not exist in the sif 2.0 specification onwards.  It may or may not be valid in sif 1.5r1.  It will be ignored."  );
            }
            else
            {
                // TODO: Log and gracefully ignore, depending on whether the ADK is set to strict or loose parsing
                throw new SifException
                    (SifErrorCategoryCode.Xml, SifErrorCodes.XML_GENERIC_VALIDATION_3,
                      "Unknown element or attribute",
                      reader.LocalName + " is not a recognized attribute of the " +
                      elementDef.Name + " element (SIF " +
                      element.EffectiveSIFVersion.ToString() + ")", zone);
            }
            
        }


        private void SetFieldValueFromElement( IElementDef def,
                                               SifElement element,
                                               XmlReader reader,
                                               SifVersion version,
                                               SifFormatter formatter,
                                               IZone zone )
        {
            // Check for xsi:nill
            if ( reader.IsEmptyElement )
            {
                // no data to set
                return;
            }

            // Look for the xsi:nill attribute that signals a null value
            while ( reader.MoveToNextAttribute() )
            {
                if (reader.LocalName == SifWriter.NIL && reader.NamespaceURI == XmlSchema.InstanceNamespace)
                {
                    SifSimpleType val = def.TypeConverter.GetSifSimpleType( null );
                    element.SetField( def, val );
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        reader.Read();
                    }
                    return;
                }
                else
                {
                    // TODO: This is an unknown attribute. Log and continue
                }
            }

            while ( reader.NodeType == XmlNodeType.Element )
            {
                reader.Read();
            }

            if ( reader.NodeType == XmlNodeType.Text )
            {
                SifSimpleType val = ParseValue( def, reader.Value.Trim(), version, formatter, zone );
                element.SetField( def, val );
            }
            // TODO: Not sure if this will always advance as expected.
            while ( reader.NodeType != XmlNodeType.EndElement )
            {
                reader.Read();
            }
        }

        private SifSimpleType ParseValue(
            IElementDef def,
            String value,
            SifVersion version,
            SifFormatter formatter,
            IZone zone )
        {
            try
            {
                TypeConverter converter = def.TypeConverter;
                if (converter == null)
                {
                    // TODO: Should we not allow this in "STRICT" mode?
                    converter = SifTypeConverters.STRING;
                }
                return converter.Parse(formatter, value);
            }
            catch (AdkTypeParseException pe)
            {
                String errorMessage = "Unable to parse element or attribute '"
                                      + def.Name + "'" + pe.Message + " (SIF "
                                      + version.ToString() + ")";
                HandleTypeParseException( errorMessage, pe, zone );
                return null;
            }
        }

        private void ParseLegacyXML(
                XmlReader reader, 
                SifVersion version,
			    IZone zone, 
                SifElement currentElement, 
                SifFormatter formatter,
			    String xmlName )
        {
            bool handled = false;

            // Determine if any surrogate formatters that are defined as children
            // of the current element can resolve it
            // NOTE: Until we fix this in the ADK, elements from the common package loose their
            // metadata information that was originally defined.
            IElementDef currentDef = currentElement.ElementDef;
            IList<IElementDef> children = currentDef.Children;
            if ( children == null || children.Count == 0 )
            {
                // try to get the actual element def
                // WARNING! this is somewhat of a hack until
                // we get support for what we need in the ADK metadata

                try
                {
                    SifElement copy =
                        (SifElement) ClassFactory.CreateInstance( currentDef.FQClassName );
                    children = copy.ElementDef.Children;
                }
                catch ( Exception cnfe )
                {
                    throw new SifException(
                        SifErrorCategoryCode.Xml,
                        SifErrorCodes.XML_GENERIC_VALIDATION_3,
                        "Unable to parse" + xmlName + "  " + version.ToString() + cnfe.Message, zone );
                }
            }
            using (XmlReader subtreeReader = reader.ReadSubtree())
            {
                subtreeReader.Read();
                foreach ( IElementDef candidate in children )
                {
                    if ( candidate.EarliestVersion.CompareTo( version ) > 0 )
                    {
                        continue;
                    }
                    IElementVersionInfo evi = candidate.GetVersionInfo( version );
                    if ( evi != null )
                    {
                        IRenderSurrogate rs = evi.GetSurrogate();
                        if ( rs != null )
                        {
                            try
                            {
                                bool surrogateHandled = rs.ReadRaw( subtreeReader, version, currentElement, formatter );
                                if ( surrogateHandled )
                                {
                                    handled = true;
                                    break;
                                }
                            }
                            catch ( AdkTypeParseException e )
                            {
                                HandleTypeParseException( "Unable to parse element or attribute value: " + e.Message, e,
                                                          zone );
                                handled = true;
                                break;
                            }
                            catch ( AdkParsingException e )
                            {
                                throw new SifException( SifErrorCategoryCode.Xml,
                                                        SifErrorCodes.XML_GENERIC_VALIDATION_3,
                                                        "unable to parse xml: " + e.Message
                                                        + version.ToString(), zone );
                            }
                        }
                    }
                }
    
                subtreeReader.Close();
            }
            // advance to the next tag
            reader.Read();


            if ( !handled )
            {
                String _tag = currentElement != null
                                  ? currentElement.ElementDef.Name + "/" + xmlName
                                  : xmlName;
                throw new SifException(
                    SifErrorCategoryCode.Xml,
                    SifErrorCodes.XML_GENERIC_VALIDATION_3, "Unknown element or attribute",
                    _tag + " is not a recognized element of SIF " + version.ToString(), zone );
            }
        }
    



        /// <summary>
        ///Evaluates the ADK StrictTypeParsing property to determine if a
        /// SIFException should be thrown for a failed parse
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="pe"></param>
        /// <param name="zone"></param>
        private void HandleTypeParseException(
            string errorMessage,
            AdkTypeParseException pe,
            IZone zone)
        {
            ILog log = Adk.Log;
            if ( zone != null )
            {
                log = zone.Log;
                if ( zone.Properties.StrictTypeParsing )
                {
                    throw new SifException( SifErrorCategoryCode.Xml, SifErrorCodes.XML_INVALID_VALUE_4, errorMessage,
                                            zone, pe );
                }
            }
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) > 0 )
            {
                log.Warn( errorMessage, pe );
            }
        }

 
    }
}

// Synchronized with Branch_Library-ADK-1.5.0.Version_4.SIFParser.java
