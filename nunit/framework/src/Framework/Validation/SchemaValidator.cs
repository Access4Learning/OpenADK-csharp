using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using OpenADK.Library;
using OpenADK.Library.Global;
using OpenADK.Library.Infra;

namespace Library.UnitTesting.Framework.Validation
{
    public class SchemaValidator
    {
        private XmlReaderSettings fValidationReaderSettings;

        private IList<ValidationEventArgs> fErrors = new List<ValidationEventArgs>();
        private IList<ValidationEventArgs> fWarnings = new List<ValidationEventArgs>();
        private bool fIgnoreEnumerationErrors;

        private SchemaValidator()
        {
        }

        private void SetSchema( XmlSchema schema, XmlResolver resolver )
        {
            //schema.Namespaces.Add("", schema.TargetNamespace);

            XmlSchemaSet xss = new XmlSchemaSet();
            if ( resolver != null )
            {
                xss.XmlResolver = resolver;
            }

            // Compile the schema and resolve all external references
            ValidationEventHandler validateHandler = new ValidationEventHandler( OnValidationError );
            xss.ValidationEventHandler += validateHandler;
            xss.Add( schema );
            xss.Compile();


            fValidationReaderSettings = new XmlReaderSettings();
            fValidationReaderSettings.ValidationType = ValidationType.Schema;
            fValidationReaderSettings.ValidationFlags =
                XmlSchemaValidationFlags.ReportValidationWarnings |
                XmlSchemaValidationFlags.AllowXmlAttributes;

            fValidationReaderSettings.Schemas = xss;
            fValidationReaderSettings.ValidationEventHandler += validateHandler;
        }

        public bool HasProblems
        {
            get { return fErrors.Count > 0 || fWarnings.Count > 0; }
        }

        public bool IgnoreEnumerationErrors
        {
            get { return fIgnoreEnumerationErrors; }
            set { fIgnoreEnumerationErrors = value; }
        }

        public void Clear()
        {
            fWarnings.Clear();
            fErrors.Clear();
        }


        /// <summary>
        /// Creates a new instance of a SchemaValidator
        /// </summary>
        /// <param name="rootSchema">A reader that will read an XML Schema</param>
        /// <param name="xmlResolver">A resolver that can be used for resolving external imports or includes that are included in the schema</param>
        /// <returns></returns>
        public static SchemaValidator NewInstance( TextReader rootSchema, XmlResolver xmlResolver )
        {
            SchemaValidator sv = new SchemaValidator();
            XmlSchema xs = XmlSchema.Read( rootSchema, new ValidationEventHandler( sv.OnValidationError ) );
            sv.SetSchema( xs, xmlResolver );


            if ( sv.HasProblems )
            {
                sv.PrintProblems( Console.Out );
                sv.Clear();
                throw new Exception( "Unable to perform validation due to errors in XML Schema" );
            }
            return sv;
        }


        private void OnValidationError( object sender, ValidationEventArgs e )
        {
            if ( fIgnoreEnumerationErrors && e.Message.IndexOf( "Enumeration" ) > 0 )
            {
                // ignore
                return;
            }

            switch ( e.Severity )
            {
                case XmlSeverityType.Error:
                    fErrors.Add( e );
                    break;
                default:
                    fWarnings.Add( e );
                    break;
            }
        }

        public void PrintProblems( TextWriter writer )
        {
            if ( fErrors.Count > 0 )
            {
                writer.WriteLine( "Schema Validation Errors: " );
                Print( fErrors, writer );
                writer.WriteLine();
            }

            if ( fWarnings.Count > 0 )
            {
                writer.WriteLine( "Schema Validation Warnings: " );
                Print( fWarnings, writer );
                writer.WriteLine();
            }
            writer.Flush();
        }

        private void Print( ICollection<ValidationEventArgs> errorList, TextWriter writer )
        {
            foreach ( ValidationEventArgs error in errorList )
            {
                writer.WriteLine( error.Exception.SourceUri + ": " + error.Exception.LineNumber + "\r\n" + error.Message );
            }
        }

        public static SifMessagePayload MakeSIFMessagePayload( SifElement payload )
        {
            if ( payload is SIF_Response )
            {
                return (SIF_Response) payload;
            }

            SIF_Response rsp = new SIF_Response();
            rsp.SetSIF_MorePackets( YesNo.NO );
            rsp.SIF_RequestMsgId = Adk.MakeGuid();
            rsp.SIF_PacketNumber = 1;

            SIF_Header hdr = rsp.Header;
            hdr.SIF_Timestamp = DateTime.Now;
            hdr.SIF_MsgId = Adk.MakeGuid();
            hdr.SIF_SourceId = "ADK Unit Tests";
            hdr.SetSIF_Security( new SIF_SecureChannel( AuthenticationLevel.ZERO,
                                                        EncryptionLevel.ZERO ) );
            hdr.SIF_DestinationId = "Schema Validator";
            if ( payload is SifDataObject )
            {
                SIF_ObjectData data = new SIF_ObjectData();
                data.AddChild( payload );
                rsp.SIF_ObjectData = data;
            }
            else if ( payload is SIF_ObjectData )
            {
                rsp.SIF_ObjectData = (SIF_ObjectData) payload;
            }
            else
            {
                throw new ArgumentException( "Unable to use payload: "
                                             + payload.ElementDef.Name );
            }

            return rsp;
        }

        public static void WriteObject( SifVersion writeVersion, string fileName, SifMessagePayload smp )
        {
            using ( FileStream outputStream = new FileStream( fileName, FileMode.Create ) )
            {
                SifWriter writer = new SifWriter( outputStream );
                smp.SetChanged( true );
                smp.SifVersion = writeVersion;
                writer.Write( smp );
                writer.Flush();
                writer.Close();
                outputStream.Close();
            }
        }

        public bool Validate( string fileName )
        {
            using ( TextReader textReader = new StreamReader( fileName, Encoding.UTF8 ) )
            using ( XmlReader xmlReader = XmlReader.Create( textReader, fValidationReaderSettings ) )
            {
                while ( xmlReader.Read() )
                {
                }

                xmlReader.Close();
                textReader.Close();
            }
            return !HasProblems;
        }

        public bool Validate( SifElement se, SifVersion testedVerson )
        {
            // Before we can validate with the schema, we need to ensure that the
            // data object is wrapped in a SIF_Message elements, because the SIF
            // Schema makes that assumption
            SifMessagePayload smp = MakeSIFMessagePayload( se );

            String tmpFileName = se.ElementDef.Name + ".SchemaValidator."
                                 + testedVerson.ToString() + ".adk";

            // 2) Write the message out to a file
            WriteObject( testedVerson, tmpFileName, smp );

            return Validate( tmpFileName );
        }
    }
}