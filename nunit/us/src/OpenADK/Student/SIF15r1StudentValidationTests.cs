using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library;
using NUnit.Framework;
using Library.Nunit.US.Library.Tools;

namespace Library.Nunit.US.Library.Student
{
    [TestFixture]
    public class SIF15r1StudentValidationTests : ValidationTest
    {
        
	public SIF15r1StudentValidationTests() : base( SifVersion.SIF15r1 ) {
	}

    [Test]
	public void testStudentSchoolEnrollment010() {
		
		SifElement se = readElementFromFile( "data/SIF15r1/StudentSchoolEnrollment/StudentSchoolEnrollment.xml", SifVersion.SIF15r1 );
		testSchemaElement( se );
		
	}
    }
}
