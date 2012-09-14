using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Summary description for SifSchema.
    /// </summary>
    public class SifSchema : XsdSchema
    {
        public SifSchema( string xsdPath )
            : base( xsdPath )
        {
            fDB =
                new DB
                    ( SifVersion.ParseXmlns( this.Schema.TargetNamespace ),
                      this.Schema.TargetNamespace, "SIF XmlSchema(XSD)" );
            BuildDb();
        }

        public ISchemaDefinition GetSchemaDefinition()
        {
            return fDB;
        }

        public void WriteExampleXml( string path )
        {
            if ( File.Exists( path ) ) {
                File.Delete( path );
            }
            XmlTextWriter writer = new XmlTextWriter( path, Encoding.UTF8 );
            writer.Formatting = Formatting.Indented;
            //WriteExample( writer );
            WriteExampleSifMessages( writer );

            writer.Flush();
            writer.Close();
        }

        private void WriteExampleSifMessages( XmlWriter writer )
        {
            XmlSchemaElement sifMessageElement = GetSifMessageElement();
            XmlSchemaChoice items = GetInfraMessageElements();
            foreach ( XmlSchemaParticle p in items.Items ) {
                writer.WriteStartElement
                    ( sifMessageElement.QualifiedName.Name,
                      sifMessageElement.QualifiedName.Namespace );
                XmlSchemaComplexType type =
                    (XmlSchemaComplexType) sifMessageElement.ElementSchemaType;
                WriteExampleAttributes( type.Attributes, writer );
                if ( p != null ) {
                    WriteExampleParticle( p, writer );
                }
                writer.WriteEndElement();
            }
        }

        private XmlSchemaElement GetSifMessageElement()
        {
            XmlQualifiedName sifMessage =
                new XmlQualifiedName( "SIF_Message", Schema.TargetNamespace );
            XmlSchemaElement sifMessageElement = ((XmlSchemaElement) Schema.Elements[sifMessage]);
            return sifMessageElement;
        }

        private XmlSchemaChoice GetInfraMessageElements()
        {
            XmlSchemaComplexType sifMessageContent =
                GetSifMessageElement().ElementSchemaType as XmlSchemaComplexType;
            if( sifMessageContent != null ) {
                return sifMessageContent.Particle as XmlSchemaChoice;
            }
            return null;
        }

        private XmlSchemaObjectCollection GetTopicMessageElements()
        {
            XmlQualifiedName name =
                new XmlQualifiedName( "SIF_ProvideObjectNamesType", Schema.TargetNamespace );
            XmlSchemaSimpleType provideObjectNames = Schema.SchemaTypes[name] as XmlSchemaSimpleType;
            XmlSchemaSimpleTypeRestriction restriction =
                provideObjectNames.Content as XmlSchemaSimpleTypeRestriction;
            return restriction.Facets;
        }


        protected override void WriteExampleElement( XmlSchemaElement element,
                                                     XmlWriter writer )
        {
            if ( element.QualifiedName.Name == "SIF_Data" ) {
                writer.WriteStartElement( element.QualifiedName.Name );
                writer.WriteCData( "Example Data" );
                writer.WriteEndElement();
            }
            else {
                base.WriteExampleElement( element, writer );
            }
        }

        private void BuildDb()
        {
            foreach ( XmlSchemaElement element in Schema.Elements.Values ) {
                BuildObjectFromElement( element, null, true );
            }

            // Set the Infra flag on these items
            foreach ( XmlSchemaParticle p in GetInfraMessageElements().Items ) {
                ObjectDef def =
                    (ObjectDef) fDB.GetObject( ((XmlSchemaElement) p).QualifiedName.Name );
                def.setInfra();
            }

            // Special Case for SIF_Header
            ObjectDef def2;
            def2 = this.fDB.GetObject( "SIF_Header" );
            def2.setInfra();

            // Build up the topic items
            foreach ( XmlSchemaEnumerationFacet p in GetTopicMessageElements() ) {
                ObjectDef def = (ObjectDef) fDB.GetObject( p.Value );
                if ( def.Name != "SIF_EventObject" ) {
                    def.Topic = true;
                }
            }
        }

        private AbstractDef BuildObjectFromElement( XmlSchemaElement element,
                                                    ObjectDef parent,
                                                    bool attachToParent )
        {
            //System.Diagnostics.Debug.Assert( !( element.QualifiedName.Name == "FineInfo" ) );
            AbstractDef returnVal = null;
            if ( element.ElementSchemaType is XmlSchemaComplexType ) {
                ObjectDef def = fDB.GetObject( element.QualifiedName.Name );
                if ( def != null ) {
                    if ( parent != null && attachToParent ) {
                        returnVal = parent.DefineElement( def.Name, def.Name );
                        if ( parent.Infra ) {
                            def.setInfra();
                        }
                        returnVal.SetFlags( getFlags( element ) );
                        setAnnotation( returnVal, element.Annotation );
                    }
                    if ( parent == null ) {
                        // Reset the source location
                        def.SourceLocation = element.SourceUri + ";line:" + element.LineNumber;
                        def.LocalPackage = getLocalPackage( element.SourceUri );
                        setAnnotation( def, element.Annotation );
                    }
                    // Console.WriteLine( element.QualifiedName.Name + " is already defined, ignoring." );
                    return def;
                }
                //System.Diagnostics.Debug.Assert( !( element.QualifiedName.Name == "StaffPersonal" ) );
                def =
                    fDB.defineObject
                        ( 0, element.QualifiedName.Name, getLocalPackage( element.SourceUri ),
                          element.SourceUri + ";line:" + element.LineNumber );
                returnVal = def;
                XmlSchemaComplexType type = (XmlSchemaComplexType) element.ElementSchemaType;
                BuildAttributes( type.Attributes, def );
                BuildContentModel( type, def );
                if ( parent != null && attachToParent ) {
                    returnVal = parent.DefineElement( def.Name, def.Name );
                    if ( parent.Infra ) {
                        def.setInfra();
                    }
                }
            }
            else {
                if ( parent == null ) {
                    Console.WriteLine
                        ( "Unexpected Root Simple element found: " + element.QualifiedName.Name );
                }
                else {
                    bool isEnum;
                    string dataType =
                        GetClassTypeFromDataType
                            ( element.ElementSchemaType, element.QualifiedName.Name, parent.Name,
                              out isEnum );
                    if ( isEnum ) {
                        Debug.Assert( element.QualifiedName.Name != "SchoolYear" );

                        returnVal = parent.DefineElement( element.QualifiedName.Name, null );
                        ((FieldDef) returnVal).SetEnum( dataType );
                    }
                    else {
                        returnVal = parent.DefineElement( element.QualifiedName.Name, dataType );
                    }
                }
            }
            if ( returnVal != null ) {
                returnVal.SetFlags( getFlags( element ) );
                setAnnotation( returnVal, element.Annotation );
            }
            return returnVal;
        }

        private void BuildContentModel( XmlSchemaComplexType type,
                                        ObjectDef def )
        {
            if ( type.ContentModel != null ) {
                if ( type.ContentModel is XmlSchemaSimpleContent ) {
                    XmlSchemaSimpleContent content = type.ContentModel as XmlSchemaSimpleContent;
                    XmlSchemaSimpleContentRestriction restriction =
                        content.Content as XmlSchemaSimpleContentRestriction;
                    if ( restriction != null ) {
                        FieldDef field;
                        bool isEnum;
                        string dataType =
                            GetClassTypeFromDataType
                                ( restriction.BaseType, type.QualifiedName.Name, def.Name,
                                  out isEnum );
                        if ( isEnum ) {
                            field = def.DefineElement( type.QualifiedName.Name, null );
                            field.SetEnum( dataType );
                        }
                        else {
                            field = def.DefineElement( type.QualifiedName.Name, dataType );
                        }
                        if( ( type.Particle != null &&  type.Particle.MinOccurs > 0 ) ||
                            ( type.ContentTypeParticle != null && type.ContentTypeParticle.MinOccurs > 0 ) ) {
                            field.SetFlags( "M" );
                        }
                    }
                    else {
                        XmlSchemaSimpleContentExtension extension =
                            content.Content as XmlSchemaSimpleContentExtension;
                        BuildAttributes( extension.Attributes, def );
                        //FieldDef field = def.defineElement( type.QualifiedName.Name, extension.BaseTypeName.Name );
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
                            BuildContentModel( baseType, def );
                        }
                    }
                    if ( ext != null && ext.Particle != null ) {
                        if ( ext.BaseTypeName != null ) {
                            object ao = Schema.Elements[ext.BaseTypeName];
                            //string data = ao.ToString();
                        }
                        BuildParticle( ext.Particle, def, true );
                    }
                }
            }
            else if ( type.Particle != null ) {
                try {
                    BuildParticle( type.Particle, def, true );
                }
                catch ( SchemaNotImplementedException xse ) {
                    Console.WriteLine
                        ( "Type Not Implemented: " + type.QualifiedName + " : " +
                          xse.Particle.ToString() + xse.Particle.SourceUri + ":" +
                          xse.Particle.LineNumber );
                }
            }
        }

        private void BuildParticle( XmlSchemaParticle particle,
                                    ObjectDef parent,
                                    bool attachToParent )
        {
            //System.Diagnostics.Debug.Assert(!particle.SourceUri.EndsWith("TransacationList"));
            if ( particle is XmlSchemaElement ) {
                BuildObjectFromElement( (XmlSchemaElement) particle, parent, attachToParent );
            }
            else if ( particle is XmlSchemaSequence ) {
                foreach ( XmlSchemaParticle particle1 in ((XmlSchemaSequence) particle).Items ) {
                    BuildParticle( particle1, parent, true );
                }
            }
            else if ( particle is XmlSchemaChoice ) {
                foreach ( XmlSchemaParticle particle1 in ((XmlSchemaChoice) particle).Items ) {
                    // TODO: This element is a choice element, we won't define it on the parent, but we may need to add an element with some type of name ( research );
                    BuildParticle( particle1, parent, false );
                }
            }
            else if ( particle is XmlSchemaAny ) {
                // New in SIF 2.0. XML Schema Any just means that the data type of the parent object
                // is Set to represent XML Any
                parent.SetDataType( "any" );
            }
            else {
                throw new SchemaNotImplementedException( particle );
            }
        }

        private void BuildAttributes( XmlSchemaObjectCollection attributes,
                                      ObjectDef objectDef )
        {
            foreach ( XmlSchemaObject o in attributes ) {
                if ( o is XmlSchemaAttribute ) {
                    BuildAttribute( (XmlSchemaAttribute) o, objectDef );
                }
                else {
                    XmlSchemaAttributeGroup group =
                        (XmlSchemaAttributeGroup)
                        Schema.Groups[((XmlSchemaAttributeGroupRef) o).RefName];
                    BuildAttributes( group.Attributes, objectDef );
                }
            }
        }

        protected void BuildAttribute( XmlSchemaAttribute attribute,
                                       ObjectDef objectDef )
        {
            FieldDef def = null;
            bool isEnum;
            string dataType =
                GetClassTypeFromDataType
                    ( attribute.AttributeSchemaType, attribute.QualifiedName.Name, objectDef.Name,
                      out isEnum );
            if ( isEnum ) {
                def = objectDef.DefineAttr( attribute.QualifiedName.Name, null );
                def.SetEnum( dataType );
            }
            else {
                def = objectDef.DefineAttr( attribute.QualifiedName.Name, dataType );
            }

            if ( attribute.Use == XmlSchemaUse.Required ) {
                def.SetFlags( "R" );
            }
            else {
                def.SetFlags( "O" );
            }
            setAnnotation( def, attribute.Annotation );
            // TODO: See if the data type is an enumeration
            //System.Diagnostics.Debug.Assert( ! ( attribute.AttributeType is xmlschemdata
        }

        private string GetClassTypeFromDataType( XmlSchemaType dataType,
                                                 string enclosingName,
                                                 string parentName,
                                                 out bool isEnum )
        {
            //System.Diagnostics.Debug.Assert( dataType.TypeCode != XmlTypeCode.AnyAtomicType );
            //System.Diagnostics.Debug.Assert( dataType.TypeCode != XmlTypeCode.AnyUri );
            Debug.Assert( dataType.TypeCode != XmlTypeCode.UntypedAtomic );


            isEnum = false;
            XmlSchemaSimpleType simpleType = dataType as XmlSchemaSimpleType;
            if ( simpleType != null ) {
                XmlSchemaSimpleTypeRestriction restriction =
                    simpleType.Content as XmlSchemaSimpleTypeRestriction;
                if ( restriction != null ) {
                    IList<DataItem> enumItems = new List<DataItem>();
                    foreach ( XmlSchemaFacet facet in restriction.Facets ) {
                        if ( facet is XmlSchemaEnumerationFacet ) {
                            // LOok for a facet description
                            DataItem item = new DataItem();
                            item.Name = ((XmlSchemaEnumerationFacet) facet).Value;
                            item.Description = item.Name;
                            if ( facet.Annotation != null && facet.Annotation.Items.Count == 1 ) {
                                XmlSchemaDocumentation doc =
                                    facet.Annotation.Items[0] as XmlSchemaDocumentation;
                                if ( doc != null ) {
                                    item.Description = doc.Markup[0].OuterXml;
                                }
                            }
                            enumItems.Add( item );
                        }
                    }
                    if ( enumItems.Count > 0 ) {
                        // TODO: Pull out common enums, such as Yes, No, Unknown types (SIF should do this in the schema)
                        Debug.Assert( !parentName.EndsWith( "SchoolYearType" ) );
                        isEnum = true;
                        if ( simpleType.Name != null ) {
                            enclosingName = simpleType.Name;
                        }
                        // TODO: Determine what the final algorithm should be for local element names
                        /*else
                        {
                            enclosingName = parentName + enclosingName;
                        }
                        */
                        if ( enclosingName == "Type" || enclosingName == "Code" ||
                             enclosingName == "CodeType" || enclosingName == "TypeCode" ||
                             fDB.GetEnum( enclosingName ) != null ) {
                            enclosingName = parentName + enclosingName;
                        }
                        EnumDef enumDef =
                            new EnumDef
                                ( enclosingName,
                                  restriction.SourceUri + ":" + restriction.LineNumber );
                        enumDef.LocalPackage = getLocalPackage( restriction.SourceUri );
                        foreach ( DataItem item in enumItems ) {
                            if ( item.Name != null ) {
                                enumDef.DefineValue( item.Name, item.Name, item.Description );
                            }
                        }
                        fDB.defineEnum( enclosingName, enumDef );
                        return enclosingName;
                    }
                }
            }
            if ( dataType != null ) {
                return dataType.TypeCode.ToString();
            }
            else {
                return null;
            }
        }

        private string getLocalPackage( string srcUri )
        {
            // Example srcUri: file:///d:/Projects/SIF/1.1/XSD/XSD/CommonElements.xsd
            // file:///d:/Projects/SIF/1.5/XSD/XSD/InstructionalServices/AssessmentSection.xsd
            // We won't be able to automatically derive the correct package name, but the goal is just
            // to come up with something that seperates the various object types
            int packageEnd = srcUri.LastIndexOf( "/" );
            int packageStart = srcUri.LastIndexOf( "/", packageEnd - 1 );

            string package = srcUri.Substring( packageStart + 1, packageEnd - packageStart - 1 );
            switch ( package ) {
                case "XSD":
                case "Infrastructure":
                    if ( srcUri.EndsWith( "CommonElements.xsd" ) ||
                         srcUri.EndsWith( "CommonTypes.xsd" ) ) {
                        return "common";
                    }
                    else {
                        return "infra";
                    }
                case "GradeBook":
                    return "gradebook";
                case "FoodServices":
                    return "food";
                case "HRFinancials":
                case "HumanResourcesFinancials":
                    return "hr";
                case "InstructionalServices":
                    return "instr";
                case "LibraryAutomation":
                    return "library";
                case "StudentInformation":
                    return "student";
                case "Transportation":
                case "TransportationAndGeographicInformation":
                    return "trans";
                case "VerticalReporting":
                    return "reporting";
                case "DataWarehouse":
                    return "dw";
                case "ETranscripts":
                    return "transcripts";
                default:
                    return package;
            }
        }

        private string getFlags( XmlSchemaParticle particle )
        {
            string flags = "O";
            if ( particle.MinOccurs > 0 ) {
                flags = "M";
            }
            if ( particle.MaxOccursString == "unbounded" ) {
                flags += "R";
            }

            return flags;
        }

        private void setAnnotation( AbstractDef def,
                                    XmlSchemaAnnotation annotation )
        {
            if ( annotation != null ) {
                string desc = string.Empty;
                foreach ( XmlSchemaObject o in annotation.Items ) {
                    XmlSchemaDocumentation description = o as XmlSchemaDocumentation;
                    if ( description != null ) {
                        foreach ( XmlNode node in description.Markup ) {
                            desc += node.OuterXml;
                        }
                    }
                    else {
                        XmlSchemaAppInfo appinfo = o as XmlSchemaAppInfo;
                        if ( o != null ) {
                            def.SetFlags( appinfo.Markup[0].InnerText );
                        }
                    }
                }
                def.Desc = desc;
            }
        }

        private DB fDB;
    }
}