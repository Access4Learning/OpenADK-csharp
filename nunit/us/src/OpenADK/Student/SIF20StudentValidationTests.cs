using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.Nunit.US.Library.Tools;

namespace Library.Nunit.US.Library.Student
{
    [TestFixture]
    class SIF20StudentValidationTests : ValidationTest
    {
        public SIF20StudentValidationTests()
            : base(SifVersion.SIF20r1 )
        {
		
	}

    [Test]
	public void testStudentSchoolEnrollment010() {

		SifElement se = readElementFromFile( "data/SIF20r1/StudentSchoolEnrollment/SIF20StudentSchoolEnrollment.xml", SifVersion.SIF20r1 );
		testSchemaElement( se );

	}

    [Test]
	public void testStudentSchoolEnrollment020() {

        StudentSchoolEnrollment sse = new StudentSchoolEnrollment(Adk.MakeGuid(), Adk.MakeGuid(), Adk.MakeGuid(), MembershipType.HOME, TimeFrame.CURRENT);
		sse.SchoolYear = 2008;
		sse.SifVersion = SifVersion.SIF20r1;
	    DateTime entryDate = DateTime.Now;
		sse.EntryDate = entryDate;
		sse.computeTimeFrame( DateTime.Now );
		sse.Homeroom = new Homeroom( "RoomInfo", Adk.MakeGuid() );
		sse.SetGradeLevel( GradeLevelCode.KG );
		testSchemaElement( sse );

	}

    [Test]
	public void testStudentPersonal010() {

		SifElement se = readElementFromFile( "data/SIF20/StudentPersonal/SIF20StudentPersonal.xml", SifVersion.SIF20r1 );
		testSchemaElement( se );

	}

    }
}
