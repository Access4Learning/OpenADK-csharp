using System;
using System.IO;
using OpenADK.Library;
using NUnit.Framework;
using Library.UnitTesting.Framework;
using Library.UnitTesting.Framework.Validation;

namespace Library.Nunit.US.Library.Tools
{
    public class ValidationTest : AdkTest
    {
        protected SchemaValidator fSchemaValidator;

	private SifVersion fVersion;

	private TextWriter fOutput;

	/**
	 * Creates an instance of a schema validation test using the specified SIF
	 * Version
	 * 
	 * @param version
	 * @throws Exception
	 */
	public ValidationTest(SifVersion version)  {
		fVersion = version;
		fSchemaValidator = USSchemaValidator.NewInstance( version );
		fOutput = Console.Out;
	}
	
	/**
	 * Runs a schema validation test on a single object using the ADK
	 * @param sdo
	 * @throws Exception
	 */
	protected void testSchemaElement(SifElement se)  {
		bool validated = fSchemaValidator.Validate( se, fVersion );

		// 4) If validation failed, write the object out for tracing purposes
		if ( !validated ) {
			SifWriter outWriter = new SifWriter( fOutput );
			outWriter.Write( se, fVersion );
			outWriter.Flush();
			fSchemaValidator.PrintProblems( fOutput );
			Assertion.Assert( "Errors validating...", false );
		}
		

	}




        /// <summary>
        /// Reads a source SIF Data object from a file, using the specified SIF
        /// Version
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected SifElement readElementFromFile(String fileName, SifVersion version)
        {
            // NOTE: This method expects the source files to be located in the project
            // the unit tests were compiled from and makes an assumption about where that
            // project is relative to where the unit test assembly is located. That
            // makes any test based on this method less portable and may need to be adjusted in the future.

            fileName = "..\\..\\" + fileName;

            SifElement se;
            SifParser parser = SifParser.NewInstance();

            using ( StreamReader reader = new StreamReader( fileName ) )
            {
                se = parser.Parse( reader, null, 0, version );
                reader.Close();
            }
            return se;
        }
    }
}
