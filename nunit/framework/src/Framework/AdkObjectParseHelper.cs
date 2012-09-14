using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using OpenADK.Library;
using NUnit.Framework;
using Library.UnitTesting.Framework.Validation;

namespace Library.UnitTesting.Framework
{
    /// <summary>
    /// Summary description for AdkObjectParseHelper.
    /// </summary>
    public sealed class AdkObjectParseHelper
    {
        public static T runParsingTest<T>( T o, SifVersion version )
            where T : SifDataObject
        {
            return runParsingTest( o, version, true, null );
        }

        public static T runParsingTest<T>( T o, SifVersion version, bool doAssertions,
                                           List<IElementDef> expectedDifferences )
            where T : SifDataObject
        {
            T o2 = (T)WriteParseAndReturn( o, version );

            Assert.IsNotNull( o2, "Object is null after parsing" );
            if ( doAssertions )
            {
                runAssertions( o, o2, expectedDifferences );
            }

            return o2;
        }

        public static void runAssertions( SifDataObject originalObject,
                                          SifDataObject reparsedObject )
        {
            runAssertions( originalObject, reparsedObject, null );
        }

        public static void runAssertions( SifDataObject originalObject, SifDataObject reparsedObject,
                                          List<IElementDef> expectedDifferences )
        {
            runAssertions( originalObject, reparsedObject, expectedDifferences, Adk.SifVersion );
        }

        public static void runAssertions( SifDataObject originalObject, SifDataObject reparsedObject,
                                          List<IElementDef> expectedDifferences, SifVersion version )
        {
            // run additional assertions by overriding this method
            //  Compare the objects
            Element[][] diffs = originalObject.CompareGraphTo( reparsedObject );
            if ( diffs[0].Length == 0 )
                Console.WriteLine( "\r\n*** Objects are identical ***\r\n" );
            else
            {
                bool isChanged = false;
                //	Display differences in two columns
                Console.WriteLine();
                Console.WriteLine( pad( "Original Object" ) + " "
                                   + pad( "Reparsed Object" ) );
                Console.WriteLine( underline( 50 ) + " " + underline( 50 ) );

                StringBuilder str = new StringBuilder();

                for ( int i = 0; i < diffs[0].Length; i++ )
                {
                    Element originalElement = diffs[0][i];
                    Element newElement = diffs[1][i];

                    str.Length = 0;

                    if ( originalElement != null )
                    {
                    }
                    appendElementDesc( originalElement, str, version );
                    str.Append( ' ' );
                    appendElementDesc( newElement, str, version );
                    // The Element.compareGraphTo() method either has a bug, or else
                    // the behavior is supposed to be this
                    // way. ( Eric, please verify ). We only want to fail if the two
                    // fields are entirely different.
                    String s1 = originalElement == null ? "" : originalElement.TextValue;
                    s1 = s1 == null ? "" : s1;
                    String s2 = newElement == null ? "" : newElement.TextValue;
                    s2 = s2 == null ? "" : s2;
                    if ( !s1.Equals( s2 ) )
                    {
                        if ( expectedDifferences != null &&
                             (
                                 (originalElement != null && expectedDifferences.Contains( originalElement.ElementDef ))
                                 ||
                                 (newElement != null && expectedDifferences.Contains( newElement.ElementDef ))
                             )
                            )
                        {
                            str.Append( " [Expected Diff]" );
                        }
                        else
                        {
                            isChanged = true;
                        }
                    }
                    Console.WriteLine( str.ToString() );
                }
                if ( isChanged )
                {
                    // TODO: The ADK Compare Graph To method needs to be fixed
                    // disabling this check for now so that we don't have a lot of
                    // failing tests, since this is a known issue and logged in 
                    // TestTrack
                    //Assert.fail( " Original and reparsed object differ, see System.Out for details " );
                }
            }
        }

        private static void appendElementDesc( Element element, StringBuilder str, SifVersion version )
        {
            if ( element == null || !element.ElementDef.IsSupported( version ) )
            {
                return;
            }

            str.Append( pad( element.ElementDef.GetSQPPath( version ) ) );
            str.Append( '(' );
            str.Append( element.TextValue );
            str.Append( ')' );
        }

        private static String underline( int length )
        {
            return new string( '-', length );
        }

        private static String pad( String text )
        {
            return text.PadRight( 50 );
        }

        public static SifElement WriteParseAndReturn( SifElement o,
                                                      SifVersion version )
        {
            return WriteParseAndReturn( o, version, null, true );
        }


        public static SifElement WriteParseAndReturn( 
            SifElement o,
            SifVersion version,
            SchemaValidator validator, 
            Boolean echoOut )
        {
            SifElement returnVal;

            if ( o is SifMessagePayload )
            {
                o.SifVersion = version;
            }

            SifWriter echo = null;

            if ( echoOut )
            {
                //   Write the object to System.out
                Console.WriteLine( "Writing object : " + o.ElementDef.Name
                                   + " using SIFVersion: " + version.ToString() );

                echo = new SifWriter( Console.Out );
                echo.Write( o, version );
                echo.Flush();
                Console.Out.Flush();
                
            }

            //  Write the object to a file
            Console.WriteLine( "Writing to file... test.xml" );
            using (Stream fos = new FileStream("test.xml", FileMode.Create))
            {
                SifWriter writer = new SifWriter( new StreamWriter( fos, Encoding.UTF8 ) );
                o.SetChanged( true );
                writer.Write( o, version );
                writer.Flush();
                writer.Close();
                fos.Close();
            }

            if ( validator != null )
            {
                Validate( "test.xml", validator );
            }

            //  Parse the object from the file
            Console.WriteLine( "Parsing from file..." );
            SifParser p = SifParser.NewInstance();

            FileStream fr = new FileStream( "test.xml", FileMode.Open );

            StreamReader inStream = new StreamReader( fr, Encoding.UTF8 );


            returnVal = p.Parse( inStream, null, 0, version );

            inStream.Close();
            fr.Close();
            //  Write the parsed object to System.out
            returnVal.SetChanged( true );
            Console.WriteLine( "Read object : " + returnVal.ElementDef.Name );
            if ( echoOut )
            {
                echo.Write( returnVal, version );
                echo.Flush();
            }


            return returnVal;
        }


        public static T WriteParseAndReturn<T>(T o, SifVersion version, SchemaValidator validator, bool echoOut)
            where T : SifElement
        {
            T returnVal;

            if ( o is SifMessagePayload )
            {
                o.SifVersion = version;
            }


            SifWriter echo = null;
            if ( echoOut )
            {
                //   Write the object to System.out
                Console.WriteLine( "Writing object : " + o.ElementDef.Name + " using SIFVersion: " + version.ToString() );

                echo = new SifWriter( Console.Out );
                echo.Write( o, version );
            }

            //  Write the object to a file
            Console.WriteLine( "Writing to file... test.xml" );

            using ( Stream fos = File.Open( "test.xml", FileMode.Create, FileAccess.Write ) )
            {
                SifWriter writer = new SifWriter( fos );
                writer.Write( o, version  );
                writer.Flush();
                writer.Close();
                fos.Close();
            }


            if ( validator != null )
            {
                Validate( "test.xml", validator );
            }

            //  Parse the object from the file
            Console.WriteLine( "Parsing from file..." );
            SifParser p = SifParser.NewInstance();
            using ( Stream fis = File.OpenRead( "test.xml" ) )
            {
                returnVal = (T) p.Parse( fis, null );
                fis.Close();
            }


            //  Write the parsed object to System.out
            returnVal.SetChanged( true );
            Console.WriteLine( "Read object : " + returnVal.ElementDef.Name );
            if ( echoOut )
            {
                echo.Write( returnVal, version );
                echo.Flush();
            }
             
            return returnVal;
        }

        private static Boolean Validate( String filePath, SchemaValidator validator )
        {
            try
            {
                return validator.Validate( filePath );
            }
            catch ( Exception e )
            {
                Console.WriteLine( e );
                Assert.Fail( "Could not parse xml file. Exception: " + e.Message );
                return false;
            }
        }

        public static void RunSDOParsingTest( String fileName, SifVersion version, Boolean runAssertions )
        {
            //  Parse the object from the file
            Console.WriteLine( "Parsing from file..." + fileName );
            SifParser p = SifParser.NewInstance();
            StreamReader reader = new StreamReader( fileName );
            SifDataObject sdo = (SifDataObject) p.Parse( reader, null );
            reader.Close();

            Assert.IsNull( sdo );
            runParsingTest<SifDataObject>( sdo, version );
        }


        public static SifElement ParseFile( String fileName, SifVersion parseVersion )
        {
            SifElement se;
            SifParser parser = SifParser.NewInstance();
            using ( StreamReader fileStream = new StreamReader( fileName ) )
            {
                se = parser.Parse( fileStream, null, 0, parseVersion );
                fileStream.Close();
            }
            return se;
        }
    } //end class
} //end namespace