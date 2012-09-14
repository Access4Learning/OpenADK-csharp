using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library;
using NUnit.Framework;
using OpenADK.Library.us;

namespace Library.NUnit.Core.Library
{
    [TestFixture]
    public class SifParserTests
    {

        [SetUp]
        public void setUp()
        {
            Adk.Initialize(SifVersion.LATEST, SIFVariant.SIF_US, (int)SdoLibraryType.Infra);
        }


        /**
	 * Asserts that the SIFParser returns an object using the same SifVersion
	 * that was passed in. If a version was not passed in, the SIFParser
	 * should try to guess the version
	 * @throws Exception
	 */
	    [Test]
        public void testReturningCorrectSifVersionSIF2x()
	{
		String test =
			"<SIF_Request xmlns=\"http://www.sifinfo.org/infrastructure/2.x\">" 	+
			"	<SIF_Query >" 																+
			"      <SIF_QueryObject ObjectName=\"SchoolInfo\" />" 						+
			"	</SIF_Query>"																+
			"</SIF_Request>";

		SifParser parser = SifParser.NewInstance();
		SifElement element = parser.Parse( test, null );
		// Since the version was not passed in, the latest supported
		// SIF 2.x Version should be returned
		Assertion.AssertEquals( SifVersion.GetLatest( 2 ), element.SifVersion );

		element = parser.Parse( test, null, 0, SifVersion.SIF21 );
		// Since the version was passed in, 2.1 should be returned
		Assertion.AssertEquals( SifVersion.SIF21, element.SifVersion );

		test =
			"<SIF_Message Version=\"2.1\" xmlns=\"http://www.sifinfo.org/infrastructure/2.x\">" +
			"<SIF_Request>" 	+
			"	<SIF_Query >" +
			"      <SIF_QueryObject ObjectName=\"SchoolInfo\" />" +
			"	</SIF_Query>" +
			"</SIF_Request>" +
			"</SIF_Message>";

		element = parser.Parse( test, null );
		// The version attribute is specified, use it.
		Assertion.AssertEquals( SifVersion.SIF21, element.SifVersion );

		element = parser.Parse( test, null, 0, SifVersion.SIF22 );
		// The version attribute is specified and should override the
		// version passed in
		Assertion.AssertEquals( SifVersion.SIF21, element.SifVersion );

	}


	/**
	 * Asserts that the SIFParser returns an object using the same SifVersion
	 * that was passed in. If a version was not passed in, the SIFParser
	 * should try to guess the version
	 * @throws Exception
	 */
	    [Test]
        public void testReturningCorrectSifVersionSIF1x()
	{
		String test =
			"<SIF_Request xmlns=\"http://www.sifinfo.org/infrastructure/1.x\">" 	+
			"	<SIF_Query >" 																+
			"      <SIF_QueryObject ObjectName=\"SchoolInfo\" />" 						+
			"	</SIF_Query>"																+
			"</SIF_Request>";

		SifParser parser = SifParser.NewInstance();
		SifElement element = parser.Parse( test, null );
		// Since the version was not passed in, the latest supported
		// SIF 2.x Version should be returned
		Assertion.AssertEquals( SifVersion.GetLatest( 1 ), element.SifVersion );

		element = parser.Parse( test, null, 0, SifVersion.SIF11 );
		// Since the version was passed in, 1.1 should be returned
		Assertion.AssertEquals( SifVersion.SIF11, element.SifVersion );

		test =
            "<SIF_Message Version=\"1.1\" xmlns=\"http://www.sifinfo.org/infrastructure/1.x\">" +
            "<SIF_Request>" +
            "	<SIF_Query >" +
            "      <SIF_QueryObject ObjectName=\"SchoolInfo\" />" +
            "	</SIF_Query>" +
            "</SIF_Request>" +
            "</SIF_Message>";

		element = parser.Parse( test, null );
		// The version attribute is specified, use it.
		Assertion.AssertEquals( SifVersion.SIF11, element.SifVersion );

		element = parser.Parse( test, null, 0, SifVersion.SIF15r1 );
		// The version attribute is specified and should override the
		// version passed in
		Assertion.AssertEquals( SifVersion.SIF11, element.SifVersion );

	}

	/**
	 * Asserts that the SIFParser returns an object using the default
	 * ADK Version if it cannot deduce a version from the namespace
	 * or version attribute
	 * @throws Exception
	 */
        [Test]
        public void testReturningCorrectSifVersionDefault()
	{
		String test =
			"<SIF_Request>" 	+
			"	<SIF_Query >" 																+
			"      <SIF_QueryObject ObjectName=\"SchoolInfo\" />" 						+
			"	</SIF_Query>"																+
			"</SIF_Request>";

		SifParser parser = SifParser.NewInstance();
		SifElement element = parser.Parse( test, null );
		// Since the version was not passed in, the ADK SIF Version should be returned
		Assertion.AssertEquals( Adk.SifVersion, element.SifVersion );


	}


    }
}
