using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Xml;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Updates Edustructures Metadata from a schema or other source
    /// </summary>
    public class MetadataUpdater
    {
        /// <summary>
        /// Constructs a MetadataUpdater
        /// </summary>
        /// <param name="sourceLocation">The directory where the metadatafiles are located</param>
        public MetadataUpdater( string sourceLocation )
        {
            fsrcLocation = sourceLocation;
        }

        public void AddUndefinedObjects( ISchemaDefinition src,
                                         ISchemaDefinition edustructuresSchema )
        {
            IDictionary srcHash = src.GetAllEnums();
            foreach ( string enumKey in srcHash.Keys ) {
                if ( edustructuresSchema.GetEnum( enumKey ) == null ) {

                    // Exclude YesNo enums
                    EnumDef srcDef = src.GetEnum( enumKey );
                    if( srcDef.Values.Count < 4 && srcDef.ContainsValue( "Yes") && srcDef.ContainsValue( "No" ) )
                    {
                        continue;
                    }


                    AddUndefinedEnum( srcHash[enumKey] as EnumDef, edustructuresSchema.Version );
                }
            }

            srcHash = src.GetAllObjects();
            foreach ( string key in srcHash.Keys ) {
                if ( key.StartsWith( "State" ) || key.StartsWith( "Country" ) ) {
                    continue;
                }
                System.Diagnostics.Debug.Assert(key != "LAInfo");
                if ( edustructuresSchema.GetObject( key ) == null ) {

                    
                    AddUndefinedObject( srcHash[key] as ObjectDef, edustructuresSchema.Version );
                }
            }


            saveChanges();
        }

        private void AddUndefinedObject( ObjectDef def,
                                         SifVersion version )
        {
            XmlDocument doc = getDocument( def.LocalPackage, version );
            DefinitionFile.WriteObjectToDom( def, doc );
        }

        private void AddUndefinedEnum( EnumDef def,
                                       SifVersion version )
        {
            XmlDocument doc = getDocument( def.LocalPackage, version );
            DefinitionFile.WriteEnumToDom( def, doc );
        }

        private XmlDocument getDocument( string localPackage,
                                         SifVersion version )
        {
            string path = Path.Combine( fsrcLocation, "autogen." + localPackage + ".xml" );

            XmlDocument doc = packageFiles[path] as XmlDocument;
            if ( doc == null ) {
                doc = new XmlDocument();
                if ( File.Exists( path ) ) {
                    doc.Load( path );
                }
                else {
                    // TODO: generate a more concise xml string, based on version
                    string xml = "<adk package=\"" + localPackage + "\" version=\"" +
                                 version.ToString() + "\" namespace=\"" + version.Xmlns + "\"/>";
                    doc.PreserveWhitespace = false;
                    doc.LoadXml( xml );
                }
                packageFiles[path] = doc;
            }
            return doc;
        }

        private void saveChanges()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.IndentChars = "  ";
            //settings.NewLineHandling = NewLineHandling.Replace;
            settings.NewLineChars = "\r\n";
            settings.CloseOutput = true;
            foreach ( DictionaryEntry entry in packageFiles ) {
                //using( XmlWriter writer = XmlWriter.Create( (string)entry.Key, settings ) )
                //{
                //    XmlDocument doc = ((XmlDocument)entry.Value);
                //    doc.PreserveWhitespace = false;
                //    doc.Save( writer );
                //}

                using (
                    XmlTextWriter writer = new XmlTextWriter( (string) entry.Key, Encoding.UTF8 ) ) {
                    writer.Formatting = Formatting.Indented;
                    XmlDocument doc = ((XmlDocument) entry.Value);
                    doc.PreserveWhitespace = false;
                    doc.DocumentElement.WriteTo( writer );
                }
            }
        }

        private IDictionary packageFiles = new ListDictionary();
        private string fsrcLocation;
    }
}