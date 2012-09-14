using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Reporting;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Reporting
{
    /// <summary>
    /// Summary description for TestReportAuthority.
    /// </summary>
    [TestFixture]
    public class TestReportAuthority : AdkTest
    {
        [Test]
        public void SDOParse()
        {
            ReportAuthorityInfo info = new ReportAuthorityInfo();
            info.AuthorityName = "XX State Department of Education";
            info.AuthorityId = "StateDOEDataWarehouse";
            info.AuthorityDepartment = "Bureau of Special Education";
            info.SetAuthorityLevel(AuthorityLevel.STATE);

            ContactInfo contact = new ContactInfo();
            contact.SetName(NameType.NAME_OF_RECORD, "Theodore", "Geisel");
            contact.PositionTitle = "State Superintendent";
            contact.EmailList = new EmailList(new Email(EmailType.PRIMARY, "drseuss@state.xx.us"));
            contact.PhoneNumberList =
                new PhoneNumberList(new PhoneNumber(PhoneNumberType.SIF1x_WORK_PHONE, "(555) 555-0000"));

            info.ContactInfo = contact;

            AdkObjectParseHelper.runParsingTest(info, SifVersion.LATEST );
        }
    }
}