using System;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Programs;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Instr
{
    /// <summary>
    /// Summary description for TestStudentPlacement.
    /// </summary>
    [TestFixture]
    public class TestStudentPlacement : AdkTest
    {
        [Test]
        public void SDOParse()
        {
            DateTime today = DateTime.Now;

            Demographics demo = new Demographics();
            demo.CountriesOfCitizenship = new CountriesOfCitizenship();
            demo.CountriesOfCitizenship.AddCountryOfCitizenship(CountryCode.US);
            demo.CountriesOfCitizenship.AddCountryOfCitizenship(CountryCode.Wrap("CA"));
            demo.CountriesOfResidency = new CountriesOfResidency(new Country(CountryCode.Wrap("CA")));
            demo.CountryOfBirth = CountryCode.US.ToString();

            //  Create a StudentPlacement
            StudentPlacement sp = new StudentPlacement();
            sp.RefId = Adk.MakeGuid();
            sp.StudentParticipationRefId = Adk.MakeGuid();
            sp.StudentPersonalRefId = Adk.MakeGuid();
            sp.SetService(ServiceCode.STAFF_PROFESSIONAL_DEVELOPMENT, "foo", "test");
            sp.ServiceProviderAgency = "ABSD";
            sp.ServiceProviderName = "John Smithfield";
            sp.SetServiceSetting("asdfasdf", ServiceSettingCode.REGULAR_SCHOOL_CAMPUS);
            sp.StartDate = today;
            sp.FrequencyTime = new FrequencyTime();
            sp.SetIndirectTime(DurationUnit.MINUTES, 10);
            sp.TotalServiceDuration = new TimeUnit(DurationUnit.MINUTES, 5);
            sp.SpecialNeedsTransportation = false;
            sp.AssistiveTechnology = true;
            sp.SetDirectTime(DurationUnit.HOURS, 5);

            AdkObjectParseHelper.runParsingTest(sp, SifVersion.LATEST );
        }
    }
}