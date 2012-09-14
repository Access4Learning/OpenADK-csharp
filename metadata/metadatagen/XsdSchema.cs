using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Summary description for Schema.
    /// </summary>
    public class XsdSchema
    {
        protected XsdSchema( string xsdPath )
        {
            ValidationEventHandler handler = new ValidationEventHandler( HandleValidationError );
            XmlReader reader = new XmlTextReader( xsdPath );
            fSchema = XmlSchema.Read( reader, handler );
            reader.Close();
            fSchema.Compile( handler );
        }


        private void HandleValidationError( object src,
                                            ValidationEventArgs e )
        {
            fHasErrors = true;
            Console.WriteLine
                ( "Schema Error: " + e.Message + e.Exception.SourceUri + ":Line " +
                  e.Exception.LineNumber );
        }

        public XmlSchema Schema
        {
            get { return fSchema; }
        }

        public bool HasErrors
        {
            get { return fHasErrors; }
        }

        // Write out the example of the XSD usage
        protected void WriteExample( XmlWriter writer )
        {
            foreach ( XmlSchemaElement element in fSchema.Elements.Values ) {
                WriteExampleElement( element, writer );
            }
        }

        // Write some example elements
        protected virtual void WriteExampleElement( XmlSchemaElement element,
                                                    XmlWriter writer )
        {
            writer.WriteStartElement( element.QualifiedName.Name, element.QualifiedName.Namespace );
            if ( element.ElementSchemaType is XmlSchemaComplexType ) {
                XmlSchemaComplexType type = (XmlSchemaComplexType) element.ElementSchemaType;
                WriteExampleAttributes( type.Attributes, writer );

                WriteContentModel( type, writer );
            }
            else {
                WriteExampleValue( element.QualifiedName.Name, element.ElementSchemaType, writer );
            }

            writer.WriteEndElement();
        }

        protected virtual void WriteContentModel( XmlSchemaComplexType type,
                                                  XmlWriter writer )
        {
            if ( type.ContentModel != null ) {
                if ( type.ContentModel is XmlSchemaSimpleContent ) {
                    XmlSchemaSimpleContent content = type.ContentModel as XmlSchemaSimpleContent;
                    XmlSchemaSimpleContentRestriction restriction =
                        content.Content as XmlSchemaSimpleContentRestriction;
                    if ( restriction != null ) {
                        WriteExampleValue( type.QualifiedName.Name, restriction.BaseType, writer );
                    }
                    else {
                        XmlSchemaSimpleContentExtension extension =
                            content.Content as XmlSchemaSimpleContentExtension;
                    }
                }
                else if ( type.Particle == null && type.ContentModel is XmlSchemaComplexContent ) {
                    XmlSchemaComplexContent complexContent =
                        type.ContentModel as XmlSchemaComplexContent;
                    XmlSchemaComplexContentExtension ext =
                        type.ContentModel.Content as XmlSchemaComplexContentExtension;
                    if ( type.BaseXmlSchemaType != null ) {
                        XmlSchemaComplexType baseType =
                            type.BaseXmlSchemaType as XmlSchemaComplexType;
                        if ( baseType != null ) {
                            WriteContentModel( baseType, writer );
                        }
                    }
                    if ( ext != null && ext.Particle != null ) {
                        if ( ext.BaseTypeName != null ) {
                            object ao = Schema.Elements[ext.BaseTypeName];
                            //string data = ao.ToString();
                        }
                        WriteExampleParticle( ext.Particle, writer );
                    }
                }
            }
            if ( type.Particle != null ) {
                WriteExampleParticle( type.Particle, writer );
            }
        }

        // Write some example attributes
        protected void WriteExampleAttributes( XmlSchemaObjectCollection attributes,
                                               XmlWriter writer )
        {
            foreach ( object o in attributes ) {
                if ( o is XmlSchemaAttribute ) {
                    WriteExampleAttribute( (XmlSchemaAttribute) o, writer );
                }
                else {
                    XmlSchemaAttributeGroup group =
                        (XmlSchemaAttributeGroup)
                        fSchema.Groups[((XmlSchemaAttributeGroupRef) o).RefName];
                    WriteExampleAttributes( group.Attributes, writer );
                }
            }
        }

        // Write a single example attribute
        protected void WriteExampleAttribute( XmlSchemaAttribute attribute,
                                              XmlWriter writer )
        {
            writer.WriteStartAttribute
                ( attribute.QualifiedName.Name, attribute.QualifiedName.Namespace );
            // The examples value

            WriteExampleValue( attribute.Name, attribute.AttributeSchemaType, writer );
            writer.WriteEndAttribute();
        }

        // Write example particles
        protected virtual void WriteExampleParticle( XmlSchemaParticle particle,
                                                     XmlWriter writer )
        {
            Decimal max;

            if ( particle.MaxOccurs == -1 || particle.MaxOccurs > 10000 ) {
                max = 2;
            }
            else {
                max = particle.MaxOccurs;
            }

            for ( int i = 0; i < max; i ++ ) {
                if ( particle is XmlSchemaElement ) {
                    WriteExampleElement( (XmlSchemaElement) particle, writer );
                }
                else if ( particle is XmlSchemaSequence ) {
                    foreach ( XmlSchemaParticle particle1 in ((XmlSchemaSequence) particle).Items ) {
                        WriteExampleParticle( particle1, writer );
                    }
                }
                else if ( particle is XmlSchemaChoice ) {
                    WriteExampleParticle
                        ( (XmlSchemaParticle) ((XmlSchemaChoice) particle).Items[0], writer );
                }
                else {
                    Console.WriteLine( "Not Implemented for this type: {0}", particle.ToString() );
                }
            }
        }

        // Write the examples text values
        protected void WriteExampleValue( string appliesTo,
                                          object schemaType,
                                          XmlWriter writer )
        {
            XmlSchemaDatatype datatype = null;
            XmlSchemaSimpleType simpleType = schemaType as XmlSchemaSimpleType;
            if ( simpleType != null ) {
                datatype = ((XmlSchemaSimpleType) schemaType).Datatype;

                if ( simpleType.Content is XmlSchemaSimpleTypeRestriction ) {
                    XmlSchemaSimpleTypeRestriction restriction =
                        (XmlSchemaSimpleTypeRestriction) simpleType.Content;
                    ArrayList enumElements = new ArrayList();
                    foreach ( XmlSchemaFacet facet in restriction.Facets ) {
                        if ( facet is XmlSchemaEnumerationFacet ) {
                            enumElements.Add( ((XmlSchemaEnumerationFacet) facet).Value );
                        }
                    }
                    if ( enumElements.Count > 0 ) {
                        writer.WriteString
                            ( (string) enumElements[fRandom.Next( enumElements.Count - 1 )] );
                        return;
                    }
                }
            }
            else {
                datatype = (XmlSchemaDatatype) schemaType;
            }

            // Consult the XSD to CLR conversion table for the correct type mappings
            Type type = datatype.ValueType;
            if ( type == typeof ( bool ) ) {
                writer.WriteString( "true" );
            }
            else if ( type == typeof ( int ) || type == typeof ( long ) ) {
                writer.WriteString( fRandom.Next( 10 ).ToString() );
            }
            else if ( type == typeof ( float ) || type == typeof ( decimal ) ) {
                writer.WriteString( ((float) (fRandom.Next( 10000 )/100f)).ToString( "#.00" ) );
            }
            else if ( type == typeof ( XmlQualifiedName ) ) {
                writer.WriteString( "qualified_name" + fRandom.Next( 100 ).ToString() );
            }
            else if ( type == typeof ( DateTime ) ) {
                writer.WriteString( "12-12-2001" );
            }
            else if ( type == typeof ( string ) ) {
                if ( appliesTo.StartsWith( "SIF_" ) ) {
                    appliesTo = appliesTo.Substring( 4 );
                }
                writer.WriteString( "Example " + appliesTo + fRandom.Next( 100 ).ToString() );
            }
                // Handle the 'xsd:positiveInteger' XSD type in the SOMsample.xsd
            else if ( type == typeof ( UInt64 ) ) {
                //positiveInteger
                writer.WriteString( fRandom.Next( 100 ).ToString() );
            }
            else {
                writer.WriteString( "Not Implemented for this datatype: " + datatype.ToString() );
            }
        }

        private Random fRandom = new Random( Environment.TickCount );
        private XmlSchema fSchema;
        private bool fHasErrors = false;
    }
}