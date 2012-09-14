using System;
using OpenADK.Library;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Reporting;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Reporting
{
    /// <summary>
    /// Summary description for TestReportManifest.
    /// </summary>
    [TestFixture]
    public class TestReportManifest : AdkTest
    {
        [Test]
        public void SDOParse()
        {
            ReportManifest rm = new ReportManifest();
            rm.RefId = "C234516384746B387459000F84723A00";
            rm.ReportAuthorityInfoRefId = "84756373645746363738484848484832";
            SIF_Version version = new SIF_Version();
            version.SifVersion = SifVersion.LATEST;
            rm.SIF_Version = version.ToString();

            rm.SetReceivingAuthority("84756373645746363738484848484812", "");
            rm.ReportName = "December 1 IDEA Students";
            rm.Description = "A report of all IDEA-eligible students receiving services on December 1";

            ReportingPeriod period = new ReportingPeriod();
            period.BeginReportDate = new DateTime?(new DateTime(2003, 12, 01));
            period.EndReportDate = new DateTime?(new DateTime(2003, 12, 01));
            period.BeginSubmitDate = new DateTime?(new DateTime(2003, 12, 01));
            period.EndSubmitDate = new DateTime?(new DateTime(2003, 12, 01));
            period.DueDate = new DateTime?(new DateTime(2003, 12, 01));
            rm.ReportingPeriod = period;

            rm.SetReportDefinitionSource(ReportDefinitionSourceType.URL, "http://www.state.edu/IDEAEligible.html");
            SIF_QueryGroup group = new SIF_QueryGroup();
            rm.SIF_QueryGroup = group;

            SIF_Query query = new SIF_Query();
            query.SetSIF_QueryObject("StudentPersonal");
            group.Add(query);

            query = new SIF_Query();
            group.Add(query);
            query.SetSIF_QueryObject("StudentSchoolEnrollment");

            SIF_ConditionGroup condGroup = new SIF_ConditionGroup();
            SIF_Conditions conds = new SIF_Conditions();
            conds.AddSIF_Condition("EntryDate", Operators.EQ, "20031201");
            condGroup.AddChild(conds);
            query.SIF_ConditionGroup = condGroup;

            // NOTE: This will currently fail every time, due to a bug in 
            // CompareGraphTo
            AdkObjectParseHelper.runParsingTest(rm, SifVersion.LATEST );
        }
    }
}