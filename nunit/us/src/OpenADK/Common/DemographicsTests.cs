using System;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Common
{
    /// <summary>
    /// Summary description for DemographicsTests.
    /// </summary>
    [TestFixture]
    public class DemographicsTests : AdkTest
    {
        /**
	 * Tests that accessors that accept a SIFDate can also accept a null value
	 */

        [Test]
        public void testSettingNullCountryArrivalDate()
        {
            StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
            Demographics d = sp.Demographics;
            d.CountryArrivalDate = new DateTime(1997, 5, 1);

            Assert.AreEqual(new DateTime(1997, 5, 1), d.CountryArrivalDate);
            d.CountryArrivalDate = null;
            Assert.IsNull(d.CountryArrivalDate, "CountryArrivalDate was set to null");

            sp = (StudentPersonal) AdkObjectParseHelper.WriteParseAndReturn(sp, Adk.SifVersion);
            d = sp.Demographics;
            Assert.IsNull(d.CountryArrivalDate, "After reparsing it should still be null");
        }

        /**
		 * Asserts that ADKGen creates overloads to methods, such as setStatePr( string Code ) that
		 * also take the object. e.g. setStatePr( StatePr )
		 */

        [Test]
        public void testSettingStatePrAndCountry()
        {
            StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
            Demographics d = sp.Demographics;

            d.SetCountryOfBirth(CountryCode.US);
            d.SetStateOfBirth(StatePrCode.AR);


            Assert.AreEqual(CountryCode.US.Value, d.CountryOfBirth);
            Assert.AreEqual(StatePrCode.AR.Value, d.StateOfBirth);
        }
    }
}