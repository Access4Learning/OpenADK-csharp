using System;
using System.IO;
using System.Text;
using OpenADK.Library;
using NUnit.Framework;
using Library.UnitTesting.Framework;
using Library.UnitTesting.Framework.Validation;

namespace OpenADK.Library.Nunit.UK.Validation
{
    [TestFixture]
    public class VersioningTests : AdkTest
    {
        private const bool VERBOSE = false;

        /*
         * Reads all supported SIF UK 1.0 objects - Writes them IN SIF UK 1.0 - Validates
         * them with the SIF UK 1.0 Schema - Parses them back into ADK Objects
         */
        [Test]
        public void ReadSIFUK10Write10()
        {
            RunVersioningTests(SifVersion.SIF20r1, SifVersion.SIF20r1, false);
        }

        /*
         * Reads all supported SIF UK 1.1 objects - Writes them IN SIF UK 1.1 - Validates
         * them with the SIF UK 1.1 Schema - Parses them back into ADK Objects
         */
        [Test]
        public void ReadSIFUK11Write11()
        {
            RunVersioningTests(SifVersion.SIF21, SifVersion.SIF21, false);
        }

        /*
         * Reads all supported SIF UK 1.1 objects - Writes them IN SIF UK 1.0 - Validates
         * them with the SIF UK 1.0 Schema - Parses them back into ADK Objects
         */
        [Test]
        public void ReadSIFUK11Write10()
        {
            RunVersioningTests(SifVersion.SIF21, SifVersion.SIF20r1, false);
        }

        private void RunVersioningTests(SifVersion dataVersion, SifVersion testVersion, bool ignoreEnumerationErrors)
        {
            // Tests assume a working directory that has the schemas in subfolders
            DirectoryInfo workingDirectory = new DirectoryInfo(Environment.CurrentDirectory);
            String testVersionStr = getShortenedVersion(testVersion);

            SchemaValidator sv = SchemaValidator.NewInstance(workingDirectory.FullName + "\\schemas\\" + testVersionStr + "\\SIF_Message.xsd");
            sv.IgnoreEnumerationErrors = ignoreEnumerationErrors;

            String dataVersionStr = getShortenedVersion(dataVersion);
            DirectoryInfo dataDir = new DirectoryInfo(workingDirectory.FullName + "\\data\\" + dataVersionStr);

            int errorCount = RunDirectoryTest(dataVersion, testVersion, dataDir, Console.Out, sv);

            Assert.AreEqual(0, errorCount, "Tests Failed. See System.out for details");

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
            SchemaValidator.WriteObject(writeVersion, tmpFileName, smp);

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
                output.WriteLine("Errors reading/writing " + fileName );
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
