using System;
using System.IO;
using System.Text;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Summary description for Metadatagen.
    /// </summary>
    /// <remarks>
    /// This utility compares the SIF XML Schema with a set of Edustructures Metadata files
    /// </remarks>
    public class Metadatagen
    {

        // Set this to true to get detailed output
        private const bool DETAILED = false;

        public static void Main( string[] args )
        {
            Console.WriteLine();
            Console.WriteLine( "Edustructures SIFWorks ADK" );
            Console.WriteLine( "Metadata Generator" );
            Console.WriteLine( "Copyright (c) 2004-" + DateTime.Now.Year + " Data Solutions. All Rights Reserved." );
            Console.WriteLine( "Version 1.1" );
            Console.WriteLine();
            Console.WriteLine( "*** For Internal Use Only ***" );
            Console.WriteLine();

            if ( args.Length < 2 ) {
                PrintHelp();
                return;
            }

            new Metadatagen().RunCompare( args );
        }

        private static void PrintHelp()
        {
            Console.WriteLine( "Usage: Metadatagen xsd compare_dir [report_file] [/autogen]" );
            Console.WriteLine( "       xsd             Path to the root SIF XSD file" );
            Console.WriteLine( "       compare_dir     Path to metadata directory to compare against" );
            Console.WriteLine( "       report_file     Path to where comparison output is written" );
            Console.WriteLine( "                       if not specified, all output is written to the console" );
            Console.WriteLine( "       /autogen        If specified, missing objects are written to the compare" );
            Console.WriteLine( "                       directory ( with a .autogen.xml extension )" );
            Console.WriteLine( "Example: Metadatagen ..\\SIF15\\SIF_Message.xsd ..\\..\\datadef\\SIF15r1" );
            Console.WriteLine();
            Console.WriteLine( "Press enter to continue . . ." );
            Console.Read();
        }


        public void RunCompare( string[] args )
        {


            string xsdPath = args[0];
            string comparePath = args[1];
            TextWriter writer;
            if ( args.Length > 2 ) {
                string fileName;
                fileName = args[2];
                writer = new StreamWriter( fileName, false, Encoding.ASCII );
                if( DETAILED ) {
                    Console.SetOut( writer );
                }
            }
            else {
                writer = Console.Out;
            }

          

            MetaDataSchema metadataSchema = new MetaDataSchema( comparePath );
            SifSchema sifSchema = new SifSchema( xsdPath );
            //sifSchema.WriteExampleXml( "Results.xml" );
            
            new SchemaComparer().Compare
                ( sifSchema.GetSchemaDefinition(),
                  metadataSchema.GetSchemaDefinition( SifVersion.SIF22 ),
                  new CompareOutput( writer, DETAILED, false ), true );
            writer.Close();


            if ( args.Length > 3 && args[3].Trim().ToLower() == "/autogen" ) {
                // Automatically generate xml for missing objects
                new MetadataUpdater( comparePath ).AddUndefinedObjects
                    ( sifSchema.GetSchemaDefinition(), metadataSchema.GetSchemaDefinitions()[0] );
            }
        }
    }
}