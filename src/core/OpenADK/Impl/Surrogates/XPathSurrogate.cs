//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library.Impl.Surrogates
{
    /// <summary>
    /// Implements a flexible Rendering surrogate based on a proprietary XPath syntax
    /// </summary>
    internal class XPathSurrogate : AbstractRenderSurrogate, IRenderSurrogate
    {
        private String fLegacyXpath;
        private String fValueXpath;

        public XPathSurrogate( IElementDef def,
                               String xpathMacro )
            : base( def )

        {
            String[] macroParts = xpathMacro.Split( '=' );
            fLegacyXpath = macroParts[0];
            fValueXpath = macroParts[1];
        }

        public void RenderRaw(
            XmlWriter writer,
            SifVersion version,
            Element o,
            SifFormatter formatter )
        {
            SifSimpleType value;
            // Read the value out of the source object
            if ( fValueXpath.StartsWith( "." ) )
            {
                value = o.SifValue;
            }
            else
            {
                IElementDef valueDef = null;
                if ( o is SifElement )
                {
                    valueDef = Adk.Dtd.LookupElementDefBySQP( o.ElementDef, fValueXpath );
                }
                if ( valueDef == null )
                {
                    throw new InvalidOperationException
                        ( "Support for value path {" + fValueXpath +
                          "} is not supported by XPathSurrogate." );
                }
                SimpleField field = ((SifElement) o).GetField( valueDef );
                if ( field == null )
                {
                    return;
                }
                value = field.SifValue;
            }

            if ( value == null )
            {
                return;
            }

            String[] xPathParts = fLegacyXpath.Split( '/' );
            int currentSegment = 0;
            // Build the path
            while ( currentSegment < xPathParts.Length - 1 )
            {
                writer.WriteStartElement( xPathParts[currentSegment] );
                currentSegment++;
            }

            String finalSegment = xPathParts[currentSegment];
            if ( finalSegment.StartsWith( "@" ) )
            {
                writer.WriteAttributeString(
                    finalSegment.Substring( 1 ),
                    value.ToString( formatter ) );
            }
            else
            {
                // Note: finalSegment can be equal to ".", which 
                // signals to render the text only
                if ( finalSegment.Length > 1 )
                {
                    writer.WriteStartElement( finalSegment );
                    currentSegment++;
                }
                writer.WriteValue( value.ToString( formatter ) );
            }

            currentSegment--;
            // unwind the path
            while ( currentSegment > -1 )
            {
                writer.WriteEndElement();
                currentSegment--;
            }
        }


        public bool ReadRaw(
            XmlReader reader,
            SifVersion version,
            SifElement parent,
            SifFormatter formatter )
        {
            String value = null;
            // 
            // STEP 1
            // Determine if this surrogate can handle the parsing of this node.
            // Retrieve the node value as a string
            //

            String[] xPathParts = fLegacyXpath.Split( '/' );
            XmlNodeType eventType = reader.NodeType;
            String localName = reader.LocalName;
            if ( eventType == XmlNodeType.Element &&
                 localName.Equals( xPathParts[0] ) )
            {
                try
                {
                    int currentSegment = 0;
                    int lastSegment = xPathParts.Length - 1;
                    if ( xPathParts[lastSegment].StartsWith( "@" ) )
                    {
                        lastSegment--;
                    }
                    while ( currentSegment < lastSegment )
                    {
                        reader.Read();
                        currentSegment++;

                        if ( !reader.LocalName.Equals( xPathParts[currentSegment] ) )
                        {
                            ThrowParseException
                                ( "Element {" + reader.LocalName +
                                  "} is not supported by XPathSurrogate path " + fLegacyXpath,
                                  version );
                        }
                    }

                    // New we are at the last segment in the XPath, and the XMLStreamReader
                    // should be positioned on the proper node. The last segment is either
                    // an attribute or an element, which need to be read differently
                    String finalSegment = xPathParts[xPathParts.Length - 1];
                    if ( finalSegment.StartsWith( "@" ) )
                    {
                        value = reader.GetAttribute( finalSegment.Substring( 1 ) );
                    }
                    else
                    {
                        value = ReadElementTextValue( reader );
                    }

                    // Note: Unlike the Java ADK, Surrogates in the the .NET ADK do not have to worry about
                    // completely consuming the XMLElement and advancing to the next tag. The .NET 
                    // Surrogates are handed a reader that only allows reading the current node and 
                    // the parent reader is automatically advanced when the surrogate is done.
                }
                catch ( Exception xse )
                {
                    ThrowParseException( xse, reader.LocalName, version );
                }
            }
            else
            {
                // No match was found
                return false;
            }

            //
            // STEP 2
            // Find the actual field to set the value to
            //
            IElementDef fieldDef;
            SifElement targetElement = parent;
            if ( fValueXpath.Equals( "." ) && fElementDef.Field )
            {
                fieldDef = fElementDef;
            }
            else
            {
                //	This indicates a child SifElement that needs to be created
                try
                {
                    targetElement = SifElement.Create( parent, fElementDef );
                }
                catch ( AdkSchemaException adkse )
                {
                    ThrowParseException( adkse, reader.LocalName, version );
                }

                formatter.AddChild( parent, targetElement, version );

                if ( fValueXpath.Equals( "." ) )
                {
                    fieldDef = fElementDef;
                }
                else
                {
                    String fieldName = fValueXpath;
                    if ( fValueXpath.StartsWith( "@" ) )
                    {
                        fieldName = fValueXpath.Substring( 1 );
                    }
                    fieldDef = Adk.Dtd.LookupElementDef( fElementDef, fieldName );
                }
            }

            if ( fieldDef == null )
            {
                throw new InvalidOperationException
                    ( "Support for value path {" + fValueXpath +
                      "} is not supported by XPathSurrogate." );
            }


            //
            // STEP 3
            // Set the value to the field
            //
            TypeConverter converter = fieldDef.TypeConverter;
            if ( converter == null )
            {
                // TODO: Determine if we should be automatically creating a converter
                // for elementDefs that don't have one, or whether we should just throw the
                // spurious data away.
                converter = SifTypeConverters.STRING;
            }
            SifSimpleType data = converter.Parse( formatter, value );
            targetElement.SetField( fieldDef, data );

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
            // 1) Create an instance of the SimpleField with a null value (It's assigned later)

            //
            // STEP 2
            // Find the actual field to set the value to
            //
            SifElement parent = (SifElement) ((SifElementPointer) parentPointer).Element;
            SifElement targetElement = parent;

            if ( !fElementDef.Field )
            {
                //	This indicates a child SifElement that needs to be created
                targetElement = SifElement.Create( parent, fElementDef );

                formatter.AddChild( parent, targetElement, version );
            }

            IElementDef fieldDef = null;
            if ( fValueXpath.Equals( "." ) )
            {
                fieldDef = fElementDef;
            }
            else
            {
                String fieldName = fValueXpath;
                if ( fValueXpath.StartsWith( "@" ) )
                {
                    fieldName = fValueXpath.Substring( 1 );
                }
                fieldDef = Adk.Dtd.LookupElementDef( fElementDef, fieldName );
            }


            if ( fieldDef == null )
            {
                throw new ArgumentException( "Support for value path {" + fValueXpath +
                                             "} is not supported by XPathSurrogate." );
            }

            SifSimpleType ssf = fieldDef.TypeConverter.GetSifSimpleType( null );
            SimpleField sf = ssf.CreateField( targetElement, fieldDef );
            targetElement.SetField( sf );


            // 2) built out a fake set of node pointers representing the SIF 1.5r1 path and
            //    return the root pointer from that stack
            return BuildLegacyPointers( parentPointer, sf );
        }

        #region IRenderSurrogate Members

        /// <summary>
        /// Called by the ADK XPath traversal code when it is traversing the given element
        /// in a legacy version of SIF
        /// </summary>
        /// <param name="parentPointer">The parent element pointer</param>
        /// <param name="sourceElement">The Element to create a node pointer for</param>
        /// <param name="version">The SIFVersion in effect</param>
        /// <returns>A NodePointer representing the current element</returns>
        public INodePointer CreateNodePointer( INodePointer parentPointer, Element sourceElement, SifVersion version )
        {
            // 1) Find the field referenced by the XPathSurrogate expression
            //    If it doesn't exist, return null
            Element referencedField = FindReferencedElement( sourceElement );
            if ( referencedField == null )
            {
                return null;
            }
            // 2) If it does exist, build out a fake set of node pointers representing the
            //    SIF 1.5r1 path and return the root pointer.
            return BuildLegacyPointers( parentPointer, referencedField );
        }


        /**
         * Finds the element referenced by the expression after the '=' sign in
         * the constructor for XPathSurrogate. This path represents the XPath from
         * the element being wrapped to the actual field it represents.
         * @param startOfPath
         * @return
         */

        private Element FindReferencedElement( Element startOfPath )
        {
            //	Read the value out of the source object
            if ( fValueXpath.StartsWith( "." ) )
            {
                return startOfPath;
            }
            else
            {
                IElementDef valueDef = null;
                if ( startOfPath is SifElement )
                {
                    valueDef = Adk.Dtd.LookupElementDefBySQP( startOfPath.ElementDef, fValueXpath );
                }
                if ( valueDef == null )
                {
                    throw new NotSupportedException( "Support for value path {" + fValueXpath +
                                                     "} is not supported by XPathSurrogate." );
                }
                SimpleField field = ((SifElement) startOfPath).GetField( valueDef );
                return field;
            }
        }

        #endregion

        private INodePointer BuildLegacyPointers( INodePointer parent, Element referencedField )
        {
            String[] xPathParts = fLegacyXpath.Split( '/' );
            int currentSegment = 0;
            INodePointer root = null;
            INodePointer currentParent = parent;

            // Build the path
            while ( currentSegment < xPathParts.Length - 1 )
            {
                FauxSifElementPointer pointer = new FauxSifElementPointer( currentParent, xPathParts[currentSegment] );
                if ( currentParent != null && currentParent is FauxSifElementPointer )
                {
                    ((FauxSifElementPointer) currentParent).SetChild( pointer, xPathParts[currentSegment] );
                }
                currentParent = pointer;
                if ( root == null )
                {
                    root = pointer;
                }
                currentSegment++;
            }

            String finalSegment = xPathParts[currentSegment];
            bool isAttribute = false;
            if ( finalSegment.StartsWith( "@" ) )
            {
                // This is an attribute
                finalSegment = finalSegment.Substring( 1 );
                isAttribute = true;
            }

            SurrogateSimpleFieldPointer fieldPointer =
                new SurrogateSimpleFieldPointer( currentParent, finalSegment, referencedField, isAttribute );
            if ( currentParent != null && currentParent is FauxSifElementPointer )
            {
                ((FauxSifElementPointer) currentParent).SetChild( fieldPointer, finalSegment );
            }
            if ( root == null )
            {
                root = fieldPointer;
            }

            return root;
        }

        /// <summary>
        /// Gets the element name or path to the element in this version of SIF
        /// </summary>
        public string Path
        {
            get { return fLegacyXpath; }
        }


        public String toString()
        {
            return "XPathSurrogate{" + fLegacyXpath + "=" + fValueXpath + "}";
        }
    }
}
