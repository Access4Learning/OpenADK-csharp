using System;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;
using OpenADK.Library.us;

namespace Library.Nunit.US.Library.Impl.Surrogates
{
    [TestFixture]
    public class GradYearSurrogateTests
    {
        [SetUp]
        public void setUp()
        {
            Adk.Initialize( SifVersion.SIF15r1, SIFVariant.SIF_US, (int)SdoLibraryType.Student );
        }

        [Test]
        public void testParseOnTimeGradYear()
        {
            String sXML = "<StudentPersonal RefId='12345678901234567890'>"
                          + " <GradYear Type='Original'>1971</GradYear>"
                          + "</StudentPersonal>";

            StudentPersonal sp = (StudentPersonal) parseSIF15r1XML( sXML );
            sp = (StudentPersonal) AdkObjectParseHelper.WriteParseAndReturn( sp, SifVersion.SIF11 );
            Assertion.AssertNotNull( sp );
            Assertion.AssertNotNull( "On Time Grad Year", sp.OnTimeGraduationYear );
            Assertion.AssertEquals( "On Time Grad Year", 1971, (int) sp
                                                                         .OnTimeGraduationYear );

            Adk.SifVersion = SifVersion.SIF15r1;
            sp = new StudentPersonal();
            sp.SetElementOrAttribute( "GradYear[@Type='Original']", "8877" );
            Assertion.AssertNotNull( "On Time Grad Year", sp.OnTimeGraduationYear );
            Assertion.AssertEquals( "On Time Grad Year", 8877, (int) sp
                                                                         .OnTimeGraduationYear );

            Element gradValue = sp.GetElementOrAttribute( "GradYear[@Type='Original']" );
            Assertion.AssertNotNull( "On Time Grad Year", gradValue );
            int gradYear = (int) gradValue.SifValue.RawValue;
            Assertion.AssertNotNull( "On Time Grad Year", gradYear );
            Assertion.AssertEquals( "On Time Grad Year", 8877, gradYear );
        }

        public void testParseProjectedGradYear()
        {
            String sXML = "<StudentPersonal RefId='12345678901234567890'>"
                          + " <GradYear Type='Projected'>2012</GradYear>"
                          + "</StudentPersonal>";

            StudentPersonal sp = (StudentPersonal) parseSIF15r1XML( sXML );
            sp = (StudentPersonal) AdkObjectParseHelper.WriteParseAndReturn( sp, SifVersion.SIF11 );
            Assertion.AssertNotNull( sp );
            Assertion.AssertNotNull( "Projected Grad Year", sp.ProjectedGraduationYear );
            Assertion.AssertEquals( "Projected Grad Year", 2012, (int) sp
                                                                           .ProjectedGraduationYear );

            Adk.SifVersion = SifVersion.SIF15r1;
            sp = new StudentPersonal();
            sp.SetElementOrAttribute( "GradYear[@Type='Projected']", "2089" );
            Assertion.AssertNotNull( "Projected Grad Year", sp.ProjectedGraduationYear );
            Assertion.AssertEquals( "Projected Grad Year", 2089, (int) sp
                                                                           .ProjectedGraduationYear );

            Element gradValue = sp.GetElementOrAttribute( "GradYear[@Type='Projected']" );
            Assertion.AssertNotNull( "Projected Grad Year", gradValue );
            int gradYear = (int) gradValue.SifValue.RawValue;
            Assertion.AssertNotNull( "Projected Grad Year", gradYear );
            Assertion.AssertEquals( "Projected Grad Year", 2089, gradYear );
        }

        public void testParseGraduationDate()
        {
            String sXML = "<StudentPersonal RefId='12345678901234567890'>"
                          + " <GradYear Type='Actual'>2005</GradYear>"
                          + "</StudentPersonal>";

            StudentPersonal sp = (StudentPersonal) parseSIF15r1XML( sXML );
            sp = (StudentPersonal) AdkObjectParseHelper.WriteParseAndReturn( sp, SifVersion.SIF11 );
            Assertion.AssertNotNull( sp );
            PartialDateType gd = sp.GraduationDate;
            Assertion.AssertNotNull( "Actual Grad Year", gd );
            Assertion.AssertEquals( "Actual Grad Year", 2005, (int) gd.Year );

            Adk.SifVersion = SifVersion.SIF15r1;
            sp = new StudentPersonal();
            sp.SetElementOrAttribute( "GradYear[@Type='Actual']", "2054" );
            gd = sp.GraduationDate;
            Assertion.AssertNotNull( "Actual Grad Year", gd );
            Assertion.AssertNotNull( "GraduationDate/getYear()", gd.Year );
            Assertion.AssertEquals( "Actual Grad Year", 2054, gd.Year.Value );

            Element gradValue = sp.GetElementOrAttribute( "GradYear[@Type='Actual']" );
            Assertion.AssertNotNull( "Actual Grad Year", gradValue );
            PartialDateType pdt = (PartialDateType) gradValue;
            Assertion.AssertEquals( "Actual Grad Year", 2054, pdt.Year.Value );
        }

        public void testParseOnTimeGradYearSS()
        {
            String sXML = "<StudentSnapshot StudentPersonalRefId='12345678901234567890'>"
                          + " <GradYear Type='Original'>1971</GradYear>"
                          + "</StudentSnapshot>";

            StudentSnapshot sp = (StudentSnapshot) parseSIF15r1XML( sXML );
            sp = (StudentSnapshot) AdkObjectParseHelper.WriteParseAndReturn( sp, SifVersion.SIF15r1 );
            Assertion.AssertNotNull( sp );
            Assertion.AssertNotNull( "On Time Grad Year", sp.OnTimeGraduationYear );
            Assertion.AssertEquals( "On Time Grad Year", 1971, (int) sp
                                                                         .OnTimeGraduationYear );

            Adk.SifVersion = SifVersion.SIF15r1;
            sp = new StudentSnapshot();
            sp.SetElementOrAttribute( "GradYear[@Type='Original']", "8877" );
            Assertion.AssertNotNull( "On Time Grad Year", sp.OnTimeGraduationYear );
            Assertion.AssertEquals( "On Time Grad Year", 8877, (int) sp.OnTimeGraduationYear );

            Element gradValue = sp.GetElementOrAttribute( "GradYear[@Type='Original']" );
            Assertion.AssertNotNull( "On Time Grad Year is null", gradValue );
            SifInt intValue = (SifInt) gradValue.SifValue;
            Assertion.AssertEquals( "On Time Grad Year", 8877, intValue.RawValue );
        }

        public void testParseProjectedGradYearSS()
        {
            String sXML = "<StudentSnapshot StudentPersonalRefId='12345678901234567890'>"
                          + " <GradYear Type='Projected'>2012</GradYear>"
                          + "</StudentSnapshot>";

            StudentSnapshot sp = (StudentSnapshot) parseSIF15r1XML( sXML );
            sp = (StudentSnapshot) AdkObjectParseHelper.WriteParseAndReturn( sp, SifVersion.SIF15r1 );
            Assertion.AssertNotNull( sp );
            Assertion.AssertNotNull( "Projected Grad Year", sp.ProjectedGraduationYear );
            Assertion.AssertEquals( "Projected Grad Year", 2012, (int) sp
                                                                           .ProjectedGraduationYear );

            Adk.SifVersion = SifVersion.SIF15r1;
            sp = new StudentSnapshot();
            sp.SetElementOrAttribute( "GradYear[@Type='Projected']", "2089" );
            Assertion.AssertNotNull( "Projected Grad Year", sp.ProjectedGraduationYear );
            Assertion.AssertEquals( "Projected Grad Year", 2089, (int) sp.ProjectedGraduationYear );

            Element gradValue = sp.GetElementOrAttribute( "GradYear[@Type='Projected']" );
            Assertion.AssertNotNull( "Projected Grad Year", gradValue );
            SifInt intValue = (SifInt) gradValue.SifValue;
            Assertion.AssertEquals( "Projected Grad Year", 2089, intValue.Value.Value );
        }

        public void testParseGraduationDateSS()
        {
            String sXML = "<StudentSnapshot StudentPersonalRefId='12345678901234567890'>"
                          + " <GradYear Type='Actual'>2005</GradYear>"
                          + "</StudentSnapshot>";

            StudentSnapshot sp = (StudentSnapshot) parseSIF15r1XML( sXML );
            sp = (StudentSnapshot) AdkObjectParseHelper.WriteParseAndReturn( sp, SifVersion.SIF15r1 );
            Assertion.AssertNotNull( sp );
            PartialDateType gd = sp.GraduationDate;
            Assertion.AssertNotNull( "Actual Grad Year", gd );
            Assertion.AssertEquals( "Actual Grad Year", 2005, (int) gd.Year );

            Adk.SifVersion = SifVersion.SIF15r1;
            sp = new StudentSnapshot();
            sp.SetElementOrAttribute( "GradYear[@Type='Actual']", "2054" );
            gd = sp.GraduationDate;
            Assertion.AssertNotNull( "Actual Grad Year", gd );
            Assertion.AssertEquals( "Actual Grad Year", 2054, (int) gd.Year );

            Element gradValue = sp.GetElementOrAttribute( "GradYear[@Type='Actual']" );
            Assertion.AssertNotNull( "Actual Grad Year", gradValue );
            Assert.IsTrue( gradValue is PartialDateType, "Should be a partial date type" );
            PartialDateType gradYear = (PartialDateType) gradValue;
            Assertion.AssertEquals( "Actual Grad Year", 2054, gradYear.Year.Value );
        }

        private SifElement parseSIF15r1XML( String xml )
        {
            SifParser parser = SifParser.NewInstance();
            return parser.Parse( xml, null, 0, SifVersion.SIF15r1 );
        }
    }
}