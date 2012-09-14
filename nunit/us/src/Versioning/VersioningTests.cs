using System;
using System.IO;
using System.Text;
using System.Xml.Schema;
using OpenADK.Library;
using NUnit.Framework;
using Library.Nunit.US;
using Library.Nunit.US.Library.Tools;
using Library.UnitTesting.Framework;
using Library.UnitTesting.Framework.Validation;

namespace OpenADK.Library.Nunit.US.Validation
{
    [TestFixture, Explicit]
    public class VersioningTests : AdkTest
    {
        private const bool VERBOSE = true;

        /*
         * Reads all supported SIF US 1.5r1 objects - Writes them IN SIF US 1.1 - Validates
         * them with the SIF US 1.1 Schema - Parses them back into Adk Objects
         */
        [Test]
        public void ReadSIF15r1WriteSIF11()
        {
            RunVersioningTests(SifVersion.SIF15r1, SifVersion.SIF11, false);
        }


        /*
         * Reads all supported SIF US 1.5r1 objects - Writes them IN SIF US 1.5r1 - Validates
         * them with the SIF US 1.5r1 Schema - Parses them back into Adk Objects
         */
        [Test]
        public void ReadSIF15r1WriteSIF15r1()
        {
            RunVersioningTests(SifVersion.SIF15r1, SifVersion.SIF15r1, false);
        }

        /*
         * Reads all supported SIF US 1.5r1 objects - Writes them IN SIF US 2.0 - Validates
         * them with the SIF US 2.0 Schema - Parses them back into Adk Objects
         */
        [Test]
        public void ReadSIF15r1WriteSIF20()
        {
            RunVersioningTests(SifVersion.SIF15r1, SifVersion.SIF20, true);
        }

        /*
         * Reads all supported SIF US 2.0r1 objects - Writes them IN SIF US 1.1 - Validates
         * them with the SIF US 1.1 Schema - Parses them back into Adk Objects
         */
        [Test]
        public void ReadSIF20r1WriteSIF11()
        {
            RunVersioningTests(SifVersion.SIF20r1, SifVersion.SIF11, true);
        }

        
        [Test]
        public void ReadSIF20r1WriteSIF15r1()
        {
            RunVersioningTests(SifVersion.SIF20r1, SifVersion.SIF15r1, true);
        }


               
        [Test]
        public void ReadSIF20r1Write20r1()
        {
            RunVersioningTests(SifVersion.SIF20r1, SifVersion.SIF20r1, false);
        }

        
        [Test]
        public void ReadSIF21Write21()
        {
            RunVersioningTests(SifVersion.SIF21, SifVersion.SIF21, false);
        }

        
        [Test]
        public void ReadSIF21Write20r1()
        {
            RunVersioningTests(SifVersion.SIF21, SifVersion.SIF20r1, false);
        }

        
        [Test]
        public void ReadSIF22Write22()
        {
            RunVersioningTests(SifVersion.SIF22, SifVersion.SIF22, false);
        }

        
        [Test]
        public void ReadSIF22Write21()
        {
            RunVersioningTests(SifVersion.SIF22, SifVersion.SIF21, false);
        }

        
        [Test]
        public void ReadSIF22Write20r1()
        {
            RunVersioningTests(SifVersion.SIF22, SifVersion.SIF20r1, false);
        }
        


        private void RunVersioningTests(SifVersion dataVersion, SifVersion schemaVersion, bool ignoreEnumerationErrors)
        {
            // Tests assume that the schema files are embedded in the test assembly
            DirectoryInfo workingDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            SchemaValidator sv = USSchemaValidator.NewInstance( schemaVersion );
            sv.IgnoreEnumerationErrors = ignoreEnumerationErrors;

            String dataVersionStr = getShortenedVersion(dataVersion);
            // Warning, slight hack. Move up two directories to the project root 
            // directory (assumes the project directory is still there)
            // This assumption will need to be changed if the tests need to become more portable
            workingDirectory = workingDirectory.Parent.Parent;
            DirectoryInfo dataDir = new DirectoryInfo(workingDirectory.FullName + "\\data\\" + dataVersionStr);

            int errorCount = RunDirectoryTest(dataVersion, schemaVersion, dataDir, Console.Out, sv);

            Assert.AreEqual(0, errorCount, "Tests Failed. See System.out for details");

        }

        private void OnValidateRead( object sender, ValidationEventArgs e )
        {
            Console.WriteLine( e.Message );
        }

        private int RunDirectoryTest(SifVersion parseVersion, SifVersion writeVersion, DirectoryInfo dir, TextWriter output, SchemaValidator sv)
        {
            int errorCount = 0;
            foreach (DirectoryInfo childDir in dir.GetDirectories())
            {
                errorCount += RunDirectoryTest(parseVersion, writeVersion, childDir, output, sv);
            }

            foreach (FileInfo fileInfo in dir.GetFiles("*.xml"))
            {
                if (!RunSingleTest(parseVersion, writeVersion, fileInfo.FullName, output, sv))
                {
                    errorCount++;
                }
            }
            output.Flush();
            return errorCount;
        }

        private bool RunSingleTest(
            SifVersion parseVersion, 
            SifVersion writeVersion, 
            string fileName, 
            TextWriter output, 
            SchemaValidator sv)
        {
            sv.Clear();

            if (VERBOSE)
            {
                output.Write("Running test on " + fileName + "\r\n");
            }

            // 1) Read the object into memory
            SifElement se = null;
            try
            {
                se = AdkObjectParseHelper.ParseFile(fileName, parseVersion);
            }
            catch (AdkException adke)
            {
                // Parsing failed. However, since this unit test is a complete
                // test of all available objects, just emit the problem and allow
                // the test to continue (with a notification of false)
                output
                        .WriteLine("Error parsing file " + fileName + "\r\n  - "
                                + adke);
                output.WriteLine();
                return false;
            }
            catch (Exception re)
            {
                output.WriteLine("Error parsing file " + fileName + "\r\n  - " + re);
                output.WriteLine();
                return false;
            }

//            if (VERBOSE)
//            {
//                SifWriter writer = new SifWriter(output);
//                writer.Write(se,parseVersion);
//                output.Flush();
//            }

            // Before we can validate with the schema, we need to ensure that the
            // data object is wrapped in a SIF_Message elements, because the SIF
            // Schema makes that assumption
            SifMessagePayload smp = SchemaValidator.MakeSIFMessagePayload(se);

            String tmpFileName = fileName + "." + writeVersion.ToString() + ".adk";

            // 2) Write the message out to a file
            try
            {
                SchemaValidator.WriteObject( writeVersion, tmpFileName, smp );
            }
            catch( Exception ex )
            {
                Console.WriteLine( "Error running test on {0}. {1}", tmpFileName, ex );
                return false;
            }

            // 3) Validate the file
            bool validated = sv.Validate(tmpFileName);

            // 4) If validation failed, write the object out for tracing purposes
            if (!validated)
            {
                if (VERBOSE)
                {
                    SifWriter outWriter = new SifWriter(output);
                    outWriter.Write(se, writeVersion );
                    outWriter.Flush();
                }
                output.WriteLine("Validation failed on " + tmpFileName );
                sv.PrintProblems(output);
                return false;
            }

            // 5) Read the object again into memory
            try
            {
                se = AdkObjectParseHelper.ParseFile(fileName, parseVersion);
            }
            catch (AdkException adke)
            {
                // Parsing failed. However, since this unit test is a complete
                // test of all available objects, just emit the problem and allow
                // the test to continue (with a notification of false)
                output.WriteLine("Error parsing file " + fileName + ": "
                        + adke.Message );
                return false;
            }
            catch (Exception re)
            {
                output.Write("Error parsing file " + fileName + ": "
                        + re.Message + "\r\n");
                return false;
            }

            return validated;
        }


        private String getShortenedVersion(SifVersion version)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SIF");
            builder.Append(version.Major);
            builder.Append(version.Minor);
            if (version.Revision > 0)
            {
                builder.Append('r');
                builder.Append(version.Revision);
            }
            return builder.ToString();
        }


    }
}
