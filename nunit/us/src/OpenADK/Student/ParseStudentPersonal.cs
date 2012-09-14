using System;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Student
{
    /// <summary>
    /// Summary description for ParseStudentPersonal.
    /// </summary>
    [TestFixture]
    public class ParseStudentPersonal : AdkTest
    {
       
        [Test]
        public void SDOParse() {
		// Create a StudentPersonal
		StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
		// Test changing the name
		sp.Name = new Name(NameType.BIRTH, "STUDENT", "JOE");

		sp = AdkObjectParseHelper.runParsingTest(sp, SifVersion.SIF15r1);

		// Test to ensure that Email is not a child of StudentPersonal
		Assertion.AssertEquals("No StudentPersonal/Email", 0, sp.GetChildList( CommonDTD.EMAIL).Count );
		Assertion.AssertNotNull("StudentPersonal/EmailList", sp.EmailList);
        Assertion.Assert("StudentPersonal/EmailList/Email", sp.EmailList.ChildCount > 0);

		sp = AdkObjectParseHelper.runParsingTest(sp, SifVersion.SIF20);

		// Test to ensure that Email is not a child of StudentPersonal
		Assertion.AssertEquals("No StudentPersonal/Email", 0, sp.GetChildList(CommonDTD.EMAIL).Count );
        Assertion.AssertNotNull("StudentPersonal/EmailList", sp.EmailList);
        Assertion.Assert("StudentPersonal/EmailList/Email", sp.EmailList.ChildCount > 0);
		sp = AdkObjectParseHelper.runParsingTest(sp, SifVersion.SIF11);

		// Test to ensure that Email is not a child of StudentPersonal
		Assertion.AssertEquals("No StudentPersonal/Email", 0, sp.GetChildList(CommonDTD.EMAIL).Count );
        Assertion.AssertNotNull("StudentPersonal/EmailList", sp.EmailList);
        Assertion.Assert("StudentPersonal/EmailList/Email", sp.EmailList.ChildCount > 0);

		sp = AdkObjectParseHelper.runParsingTest(sp, SifVersion.SIF22);

		// Test to ensure that Email is not a child of StudentPersonal
		Assertion.AssertEquals("No StudentPersonal/Email", 0, sp.GetChildList(CommonDTD.EMAIL).Count );
        Assertion.AssertNotNull("StudentPersonal/EmailList", sp.EmailList);
        Assertion.Assert("StudentPersonal/EmailList/Email", sp.EmailList.ChildCount > 0);

	}

    }
}