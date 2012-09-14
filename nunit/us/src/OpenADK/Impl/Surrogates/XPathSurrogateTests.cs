using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;
using OpenADK.Library.us;

namespace Library.Nunit.US.Library.Impl.Surrogates
{
    [TestFixture]
    public class XPathSurrogateTests
    {
        [SetUp]
	public void setUp() {
		Adk.Initialize(SifVersion.SIF15r1, SIFVariant.SIF_US, (int)SdoLibraryType.Student );
	}

	/**
	 * This test asserts that SIF 1.x elements that are rendered using the XPathSurrogate
	 * in the StudentSchoolEnrollment class return the proper and expected values.
	 * @throws Exception
	 */
	[Test]
        public void testSSE_Get_XPathSurrogate_SIF15r1() {

		Adk.SifVersion=SifVersion.SIF15r1;

		String sXML = "<StudentSchoolEnrollment RefId=\"A8C3D3E34B359D75101D00AA001A1652\""
					+ "  StudentPersonalRefId=\"D3E34B359D75101A8C3D00AA001A1652\""
					+ "		SchoolInfoRefId=\"D3E34B359D75101A8C3D00AA001A1651\""
					+ "	MembershipType=\"Home\""
					+ "	TimeFrame=\"Current\">"
					+ "	<SchoolYear>2004</SchoolYear>"
					+ "	<EntryDate>20040129</EntryDate>"
					+ "	<EntryType Code=\"D05\"/>"
					+ "	<GradeLevel Code=\"10\"/>"
					+ "	<Homeroom RoomInfoRefId=\"D7510D3E34B3591A8C3D00AA001A1651\"/>"
					+ "	<StaffAssigned Type=\"Advisor\" StaffPersonalRefId=\"B359D3E34D75101A8C3D00AA001A1652\"/>"
					+ "	<FTE>1.00</FTE>"
					+ "	<FTPTStatus>FullTime</FTPTStatus>"
					+ "	<ResidencyStatus>1653</ResidencyStatus>"
					+ "	<NonResidentAttendReason>1658</NonResidentAttendReason>"
					+ "	</StudentSchoolEnrollment> ";



		StudentSchoolEnrollment sse = (StudentSchoolEnrollment) parseSIF15r1XML(sXML);
		sse = (StudentSchoolEnrollment)AdkObjectParseHelper.WriteParseAndReturn( sse, SifVersion.SIF15r1 );
		Assertion.AssertNotNull( sse );

		// Check getting Homeroom and Residency status using APIs
		Assertion.AssertNotNull( "Homeroom", sse.Homeroom );
		Assertion.AssertEquals( "Homeroom", "D7510D3E34B3591A8C3D00AA001A1651", sse.Homeroom.Value );


		ResidencyStatus rs = sse.ResidencyStatus;
		Assertion.AssertNotNull( "ResidencyStatus", rs );
		Assertion.AssertEquals( "ResidencyStatus", "1653", rs.Code );

		//	Check getting Homeroom and Residency status using SIF 1.5 xpaths
		// Homeroom
		Element value = sse.GetElementOrAttribute( "Homeroom/@RoomInfoRefId" );
		Assertion.AssertNotNull( "Homeroom", value );
		Assertion.AssertEquals( "Homeroom", "D7510D3E34B3591A8C3D00AA001A1651", value.TextValue );


		// ResidencyStatus
		value = sse.GetElementOrAttribute( "ResidencyStatus" );
		Assertion.AssertNotNull( "ResidencyStatus", value );
		Assertion.AssertEquals( "ResidencyStatus", "1653", value.TextValue );


	}


	private SifElement parseSIF15r1XML(String xml) {
		SifParser parser = SifParser.NewInstance();
		return parser.Parse(xml, null, 0, SifVersion.SIF15r1);
	}

    }
}
