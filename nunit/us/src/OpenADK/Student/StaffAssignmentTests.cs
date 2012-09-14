using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Library.Student
{
    [TestFixture]
    public class StaffAssignmentTests
    {
        	public void testReadWriteStaffAssignmentSIF1x() {
		StaffAssignment sa = ObjectCreator.CreateStaffAssignment();
		Console.WriteLine(sa.GetContent().Count );

		sa = (StaffAssignment) AdkObjectParseHelper.WriteParseAndReturn(sa,
				SifVersion.SIF15r1);
		Assertion.AssertNull("Primary Assignment", sa.PrimaryAssignment);
	}

    [Test]
	public void testReadWriteStaffAssignmentSIF2x() {
		StaffAssignment sa = ObjectCreator.CreateStaffAssignment();
		sa = (StaffAssignment) AdkObjectParseHelper.WriteParseAndReturn(sa,
				SifVersion.SIF20r1);

		Assertion.AssertEquals("Primary Assignment", "Yes", sa.PrimaryAssignment );

	}
    }
}
