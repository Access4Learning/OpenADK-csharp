using System;
using System.Collections;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.Cfg;
using OpenADK.Library.Tools.Mapping;
using NUnit.Framework;
using Library.UnitTesting.Framework.Validation;

namespace Library.Nunit.US.Library.Tools.Mapping
{
    [TestFixture]
    public class SASIMappingsTests
    {
        private SifVersion fVersion = SifVersion.SIF15r1;

        private AgentConfig fCfg;
        [SetUp]
        public void setUp()
        {
            if( !Adk.Initialized )
            {
                Adk.Initialize( );
            }
            Adk.SifVersion = fVersion;
            fCfg = new AgentConfig();
            fCfg.Read("..\\..\\Library\\Tools\\Mapping\\SASI2.0.cfg", false);
        }

        [Test]
        public void testStudentPersonalSIF15r1()
        {
            IDictionary values = new Hashtable();
            values.Add( "PERMNUM", "9798" );
            values.Add( "SOCSECNUM", "845457898" );
            values.Add( "SCHOOLNUM", "999" );
            values.Add( "SCHOOLNUM2", "999" );
            values.Add( "GRADE", "09" );
            values.Add( "HOMEROOM", "5" );
            values.Add( "LASTNAME", "Doe" );
            values.Add( "FIRSTNAME", "Jane" );
            values.Add( "MIDDLENAME", null );
            values.Add( "NICKNAME", null );
            values.Add( "MAILADDR", "5845 Test Blvd" );
            values.Add( "CITY", "slc" );
            values.Add( "STATE", "Utah" );
            values.Add( "COUNTRY", "" );
            values.Add( "ZIPCODE", "84093" );
            values.Add( "RESADDR", null );
            values.Add( "RESCITY", null );
            values.Add( "RESSTATE", null );
            values.Add( "RESCOUNTRY", null );
            values.Add( "RESZIPCODE", null );
            values.Add( "BIRTHDATE", "20051209" );
            values.Add( "GENDER", "F" );
            values.Add( "ETHNICCODE", "W" );
            values.Add( "ENGPROF", "" );
            values.Add( "PRIMARYLNG", "" );
            values.Add( "TELEPHONE", null );

            StringMapAdaptor sma = new StringMapAdaptor( values );
            StudentPersonal sp = new StudentPersonal();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, sp, SifVersion.SIF15r1 );
            Console.WriteLine( sp.ToXml() );
            OtherIdList oil = sp.OtherIdList;
            bool gradeMappingFound = false;
            foreach ( OtherId oid in oil )
            {
                if ( "ZZ".Equals( oid.Type )
                     && oid.Value.StartsWith( "GRADE" ) )
                {
                    Assertion.AssertEquals( "OtherId[@Type=ZZ]GRADE: mapping", "GRADE:09", oid.Value );
                    gradeMappingFound = true;
                }
            }
            Assertion.Assert( "GradeMapping found", gradeMappingFound );
        }

        [Test]
        public void testStudentSnapshot15r1()
        {
            StringMapAdaptor sma = createStudentSnapshotFields();
            StudentSnapshot ss = new StudentSnapshot();
            ss.StudentPersonalRefId = Adk.MakeGuid();
            ss.SnapDate = DateTime.Now;

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m
                .MapOutbound( sma, ss, new DefaultValueBuilder( sma ),
                              SifVersion.SIF15r1 );
            Console.WriteLine( ss.ToXml() );

            int? onTimeGradYear = ss.OnTimeGraduationYear;
            Assertion.Assert( "onTimeGraduationYear is null", onTimeGradYear.HasValue );
            Assertion.AssertEquals( "OnTimeGraduationYear", 2000, onTimeGradYear.Value );

            SchemaValidator sv = USSchemaValidator.NewInstance( SifVersion.SIF15r1 );

            // 3) Validate the file
            bool validated = sv.Validate( ss, SifVersion.SIF15r1 );

            // 4) If validation failed, write the object out for tracing purposes
            if ( !validated )
            {
                sv.PrintProblems( Console.Out );
                Assertion.Fail( "Schema Validation Failed:" );
            }
        }

        [Test]
        public void testStudentSnapshot15r1_Adk20r1()
        {
            Adk.SifVersion = SifVersion.SIF15r1;
            StringMapAdaptor sma = createStudentSnapshotFields();
            StudentSnapshot ss = new StudentSnapshot();
            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, ss, SifVersion.SIF20r1 );
            Console.WriteLine( ss.ToXml() );

            int? onTimeGradYear = ss.OnTimeGraduationYear;
            Assertion.Assert( "onTimeGraduationYear is null", onTimeGradYear.HasValue );
            Assertion.AssertEquals( "OnTimeGraduationYear", 2000, onTimeGradYear.Value );
        }


        [Test]
        public void testStudentSnapshot15r1_EmptyGradYear()
        {
            Adk.SifVersion = SifVersion.SIF15r1;
            StringMapAdaptor sma = createStudentSnapshotFields();

            // SASI Expects that an empty string in a grad year
            // field will not produce an error, but rather an empty element
            sma.Dictionary[ "ORIGYRGRAD"] = "" ;
            StudentSnapshot ss = new StudentSnapshot();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, ss, SifVersion.SIF15r1 );
            Console.WriteLine( ss.ToXml() );

            int? onTimeGradYear = ss.OnTimeGraduationYear;
            Assertion.Assert( "onTimeGraduationYear is not null", !onTimeGradYear.HasValue );
        }


        [Test]
        public void testStudentSnapshot20_EmptyGradYear()
        {
            Adk.SifVersion = SifVersion.LATEST;
            StringMapAdaptor sma = createStudentSnapshotFields();

            // SASI Expects that an empty string in a grad year
            // field will not produce an error, but rather an empty element
            sma.Dictionary[ "ORIGYRGRAD" ] = "" ;
            StudentSnapshot ss = new StudentSnapshot();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, ss, SifVersion.LATEST );
            Console.WriteLine( ss.ToXml() );

            int? onTimeGradYear = ss.OnTimeGraduationYear;
            Assertion.Assert( "onTimeGraduationYear is not null", !onTimeGradYear.HasValue );
        }


        [Test]
        public void testStudentSnapshot15r1_BlankGradYear()
        {
            Adk.SifVersion = SifVersion.SIF15r1;
            StringMapAdaptor sma = createStudentSnapshotFields();

            // SASI Expects that a single space character in a grad year
            // field will not produce an error, but rather an empty element
            sma.Dictionary[ "ORIGYRGRAD" ] = " " ;
            StudentSnapshot ss = new StudentSnapshot();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, ss, SifVersion.SIF15r1 );
            Console.WriteLine( ss.ToXml() );

            int? onTimeGradYear = ss.OnTimeGraduationYear;
            Assertion.Assert( "onTimeGraduationYear is not null", !onTimeGradYear.HasValue );
        }


        [Test]
        public void testStudentSnapshot20_BlankGradYear()
        {
            Adk.SifVersion = SifVersion.LATEST;
            StringMapAdaptor sma = createStudentSnapshotFields();

            // SASI Expects that a single space character in a grad year
            // field will not produce an error, but rather an empty element
            sma.Dictionary[ "ORIGYRGRAD"] = " ";
            StudentSnapshot ss = new StudentSnapshot();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, ss, SifVersion.LATEST );
            Console.WriteLine( ss.ToXml() );

            int? onTimeGradYear = ss.OnTimeGraduationYear;
            Assertion.Assert( "onTimeGraduationYear is not null", !onTimeGradYear.HasValue );
        }


        [Test]
        public void testStudentSnapshot15r1_NullLastName()
        {
            Adk.SifVersion = SifVersion.SIF15r1;
            StringMapAdaptor sma = createStudentSnapshotFields();

            // SASI Expects that a null string will result in
            // the field not being mapped. Checking that here
            sma.Dictionary[ "LASTNAME" ] = null;
            StudentSnapshot ss = new StudentSnapshot();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null, null, null );
            m.MapOutbound( sma, ss, SifVersion.SIF15r1 );
            String value = ss.ToXml();
            Console.WriteLine( value );

            Assertion.Assert( "Last Name should be null", value.IndexOf( "LastName" ) == -1 );
        }


        [Test]
        public void testStudentSnapshot20_NullLastName()
        {
            Adk.SifVersion = SifVersion.LATEST;
            StringMapAdaptor sma = createStudentSnapshotFields();

            // SASI Expects that a null string will result in
            // the field not being mapped. Checking that here
            sma.Dictionary["LASTNAME"] = null ;
            StudentSnapshot ss = new StudentSnapshot();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, ss, SifVersion.LATEST );
            String value = ss.ToXml();
            Console.WriteLine( value );

            Assertion.Assert( "Last Name should be null", value.IndexOf( "LastName" ) == -1 );
        }


        [Test]
        public void testStudentSnapshot20r1()
        {
            StringMapAdaptor sma = createStudentSnapshotFields();
            StudentSnapshot ss = new StudentSnapshot();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, ss, SifVersion.SIF20r1 );

            int? onTimeGradYear = ss.OnTimeGraduationYear;
            Assertion.Assert( "onTimeGraduationYear is null", onTimeGradYear.HasValue );
            Assertion.AssertEquals( "OnTimeGraduationYear", 2000, onTimeGradYear.Value );

            Console.WriteLine( ss.ToXml() );
        }

        private StringMapAdaptor createStudentSnapshotFields()
        {
            IDictionary values = new Hashtable();
            values.Add( "PERMNUM", "9798" );
            values.Add( "SOCSECNUM", "845457898" );
            values.Add( "SCHOOLNUM", "999" );
            values.Add( "SCHOOLNUM2", "999" );
            values.Add( "GRADE", "09" );
            values.Add( "HOMEROOM", "5" );
            values.Add( "LASTNAME", "Doe" );
            values.Add( "FIRSTNAME", "Jane" );
            values.Add( "MIDDLENAME", null );
            values.Add( "NICKNAME", null );
            values.Add( "MAILADDR", "5845 Test Blvd" );
            values.Add( "CITY", "slc" );
            values.Add( "STATE", "Utah" );
            values.Add( "COUNTRY", "" );
            values.Add( "ZIPCODE", "84093" );
            values.Add( "RESADDR", null );
            values.Add( "RESCITY", null );
            values.Add( "RESSTATE", null );
            values.Add( "RESCOUNTRY", null );
            values.Add( "RESZIPCODE", null );
            values.Add( "BIRTHDATE", "20051209" );
            values.Add( "GENDER", "F" );
            values.Add( "ETHNICCODE", "W" );
            values.Add( "ENGPROF", "" );
            values.Add( "PRIMARYLNG", "" );
            values.Add( "TELEPHONE", null );
            values.Add( "ORIGYRGRAD", "2000" );

            StringMapAdaptor sma = new StringMapAdaptor( values );
            return sma;
        }

        [Test]
        public void testStudentPersonalNullHomeroom()
        {
            IDictionary values = new Hashtable();
            values.Add( "PERMNUM", "9798" );
            values.Add( "SOCSECNUM", "845457898" );
            values.Add( "SCHOOLNUM", "999" );
            values.Add( "SCHOOLNUM2", "999" );
            values.Add( "GRADE", "09" );
            values.Add( "HOMEROOM", null );
            values.Add( "LASTNAME", "Doe" );
            values.Add( "FIRSTNAME", "Jane" );
            values.Add( "MIDDLENAME", null );
            values.Add( "NICKNAME", null );
            values.Add( "MAILADDR", "5845 Test Blvd" );
            values.Add( "CITY", "slc" );
            values.Add( "STATE", "Utah" );
            values.Add( "COUNTRY", "" );
            values.Add( "ZIPCODE", "84093" );
            values.Add( "RESADDR", null );
            values.Add( "RESCITY", null );
            values.Add( "RESSTATE", null );
            values.Add( "RESCOUNTRY", null );
            values.Add( "RESZIPCODE", null );
            values.Add( "BIRTHDATE", "20051209" );
            values.Add( "GENDER", "F" );
            values.Add( "ETHNICCODE", "W" );
            values.Add( "ENGPROF", "" );
            values.Add( "PRIMARYLNG", "" );
            values.Add( "TELEPHONE", null );

            StringMapAdaptor sma = new StringMapAdaptor( values );
            StudentPersonal sp = new StudentPersonal();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, sp, SifVersion.SIF15r1 );

            Console.WriteLine( sp.ToXml() );

            Element e = sp
                .GetElementOrAttribute( "OtherId[@Type='ZZ' and starts-with(., 'HOMEROOM') ]" );
            Assertion.AssertNull( "HOMEROOM should not have been mapped", e );
        }

        [Test]
        public void testStudentPersonal2Addresses15r1()
        {
            IDictionary values = new Hashtable();
            values.Add( "PERMNUM", "9798" );
            values.Add( "LASTNAME", "Doe" );
            values.Add( "FIRSTNAME", "Jane" );
            values.Add( "MIDDLENAME", null );
            values.Add( "MAILADDR", "PO Box 80077" );
            values.Add( "CITY", "Salt Lake City" );
            values.Add( "STATE", "Utah" );
            values.Add( "COUNTRY", "US" );
            values.Add( "ZIPCODE", "84093" );
            values.Add( "RESADDR", "528 Big CottonWood Rd" );
            values.Add( "RESCITY", "Sandy" );
            values.Add( "RESSTATE", "UT" );
            values.Add( "RESCOUNTRY", "US" );
            values.Add( "RESZIPCODE", "84095" );

            StringMapAdaptor sma = new StringMapAdaptor( values );
            StudentPersonal sp = new StudentPersonal();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, sp, SifVersion.SIF15r1 );

            Console.WriteLine( sp.ToXml() );

            Element e = sp
                .GetElementOrAttribute(
                "StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='M']/Street/Line1" );
            Assertion.AssertNotNull( "Mailing Address was not mapped ", e );
            Assertion.AssertEquals( "Mailing Address", "PO Box 80077", e.TextValue );

            e = sp
                .GetElementOrAttribute(
                "StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='P']/Street/Line1" );
            Assertion.AssertNotNull( "Residential Address was not mapped ", e );
            Assertion.AssertEquals( "Mailing Address", "528 Big CottonWood Rd", e
                                                                                    .TextValue );
            StudentAddressList children = sp.AddressList;
            Assertion.AssertEquals( "Should have two StudentAddress elements", 2,
                                    children.Count );
        }

        [Test]
        public void testStudentPersonal2Addresses20r1()
        {
            IDictionary values = new Hashtable();
            values.Add( "PERMNUM", "9798" );
            values.Add( "LASTNAME", "Doe" );
            values.Add( "FIRSTNAME", "Jane" );
            values.Add( "MIDDLENAME", null );
            values.Add( "MAILADDR", "PO Box 80077" );
            values.Add( "CITY", "Salt Lake City" );
            values.Add( "STATE", "Utah" );
            values.Add( "COUNTRY", "US" );
            values.Add( "ZIPCODE", "84093" );
            values.Add( "RESADDR", "528 Big CottonWood Rd" );
            values.Add( "RESCITY", "Sandy" );
            values.Add( "RESSTATE", "UT" );
            values.Add( "RESCOUNTRY", "US" );
            values.Add( "RESZIPCODE", "84095" );

            StringMapAdaptor sma = new StringMapAdaptor( values );
            StudentPersonal sp = new StudentPersonal();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, sp, SifVersion.SIF20r1 );

            Console.WriteLine( sp.ToXml() );

            Element e = sp
                .GetElementOrAttribute( "AddressList/Address[@Type='0123']/Street/Line1" );
            Assertion.AssertNotNull( "Mailing Address was not mapped ", e );
            Assertion.AssertEquals( "Mailing Address", "PO Box 80077", e.TextValue );

            e = sp
                .GetElementOrAttribute( "AddressList/Address[@Type='0765']/Street/Line1" );
            Assertion.AssertNotNull( "Residential Address was not mapped ", e );
            Assertion.AssertEquals( "Mailing Address", "528 Big CottonWood Rd", e
                                                                                    .TextValue );

            StudentAddressList[] list = sp.AddressLists;
            SifElementList children = sp.GetChildList( CommonDTD.ADDRESSLIST );
            Assertion.AssertEquals( "Should have one StudentAddress elements", 1,
                                    children.Count );
            Assertion.AssertEquals( "Should have two address elements", 2, children[0].ChildCount );
        }

        [Test]
        public void testStaffPersonalATCH()
        {
            IDictionary values = new Hashtable();
            values.Add( "ATCH.TCHNUM", "98" );
            values.Add( "ATCH.SOCSECNUM", "455128888" );
            values.Add( "ATCH.SCHOOLNUM", "999" );
            values.Add( "ATCH.SCHOOLNUM2", "999" );
            values.Add( "ATCH.HOMEROOM", "5" );
            values.Add( "ATCH.LASTNAME", "Ngo" );
            values.Add( "ATCH.FIRSTNAME", "Van" );
            values.Add( "ATCH.MIDDLENAME", null );
            values.Add( "ATCH.TELEPHONE", "8011234567" );
            values.Add( "ATCH.TELEXTN", null );
            values.Add( "ATCH.EMAILADDR", null );
            values.Add( "ATCH.ETHNIC", "W" );
            StringMapAdaptor sma = new StringMapAdaptor( values );
            StaffPersonal s = new StaffPersonal();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, s, SifVersion.SIF15r1 );

            Console.WriteLine( s.ToXml() );
        }

        [Test]
        public void testStaffPersonalATCHNullPhone()
        {
            IDictionary values = new Hashtable();
            values.Add( "ATCH.TCHNUM", "98" );
            values.Add( "ATCH.SOCSECNUM", "455128888" );
            values.Add( "ATCH.SCHOOLNUM", "999" );
            values.Add( "ATCH.SCHOOLNUM2", "999" );
            values.Add( "ATCH.HOMEROOM", "5" );
            values.Add( "ATCH.LASTNAME", "Ngo" );
            values.Add( "ATCH.FIRSTNAME", "Van" );
            values.Add( "ATCH.MIDDLENAME", null );
            values.Add( "ATCH.TELEPHONE", null );
            values.Add( "ATCH.TELEXTN", null );
            values.Add( "ATCH.EMAILADDR", null );
            values.Add( "ATCH.ETHNIC", "W" );
            StringMapAdaptor sma = new StringMapAdaptor( values );
            StaffPersonal s = new StaffPersonal();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, s, SifVersion.SIF15r1 );

            Console.WriteLine( s.ToXml() );

            Element e = s
                .GetElementOrAttribute( "PhoneNumber[@Format='NA' and @Type='WP']" );
            Assertion.AssertNull( "PhoneNumber should be null", e );
        }

        [Test]
        public void testStaffPersonalASTF()
        {
            IDictionary values = new Hashtable();
            values.Add( "ASTF.STAFFNUM", "9847" );
            values.Add( "ASTF.SOCSECNUM", "123456789" );
            values.Add( "ASTF.SCHOOLNUM", "999" );
            values.Add( "ASTF.SCHOOLNUM2", "999" );
            values.Add( "ASTF.HOMEROOM", "8" );
            values.Add( "ASTF.EMAILADDR", null );
            values.Add( "ASTF.LASTNAME", "Ngo" );
            values.Add( "ASTF.FIRSTNAME", "Tom" );
            values.Add( "ASTF.MIDDLENAME", "C" );
            values.Add( "ASTF.ADDRESS", "1232 Bateman Point Drive" );
            values.Add( "ASTF.CITY", "West Jordan" );
            values.Add( "ASTF.STATE", "Utah" );
            values.Add( "ASTF.COUNTRY", "" );
            values.Add( "ASTF.ZIPCODE", "84084" );
            values.Add( "ASTF.SCHOOLTEL", "1234567890" );
            values.Add( "ASTF.TELEXTN", null );
            values.Add( "ASTF.HOMETEL", null );
            values.Add( "ASTF.ETHNICCODE", "W" );
            StringMapAdaptor sma = new StringMapAdaptor( values );
            StaffPersonal s = new StaffPersonal();

            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, s, SifVersion.SIF15r1 );

            Console.WriteLine( s.ToXml() );

            Element e = s
                .GetElementOrAttribute( "PhoneNumber[@Format='NA' and @Type='HP']" );
            Assertion.AssertNull( "Home PhoneNumber should not be mapped", e );
            e = s.GetElementOrAttribute( "PhoneNumber[@Format='NA' and @Type='WP']" );
            Assertion.AssertNotNull( "School PhoneNumber should be mapped", e );
            Assertion.AssertEquals( "School phone", "1234567890", e.TextValue );
        }

        [Test]
        public void testStudentContactSIF15r1()
        {
            StringMapAdaptor sma = createStudentContactFields();
            StudentContact sc = new StudentContact();
            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, sc, SifVersion.SIF15r1 );
            String value = sc.ToXml();
            Console.WriteLine( value );

            //	Verify that the phone number is properly escaped
            int loc = value.IndexOf( "<PhoneNumber Format=\"NA\" Type=\"EX\">M&amp;W</PhoneNumber>" );
            Assertion.Assert( "Phone number should be escaped", loc > -1 );

            Element e = sc
                .GetElementOrAttribute( "PhoneNumber[@Format='NA' and @Type='HP']" );
            Assertion.AssertNotNull( "School PhoneNumber should be mapped", e );
            Assertion.AssertEquals( "School phone", "8014504555", e.TextValue );

            e = sc
                .GetElementOrAttribute( "PhoneNumber[@Format='NA' and @Type='AP']" );
            Assertion.AssertNotNull( "School PhoneNumber should be mapped", e );
            // Note the " " Space at the end of the value. This should be there,
            // according to the mapping
            Assertion.AssertEquals( "School phone", "8014505555 ", e.TextValue );

            e = sc
                .GetElementOrAttribute( "PhoneNumber[@Format='NA' and @Type='WP']" );
            Assertion.AssertNull( "School PhoneNumber should not be mapped", e );

            AddressList al = sc.AddressList;
            if ( al != null )
            {
                foreach ( Address addr in al )
                {
                    Assertion.AssertNull( "Country should be null", addr.Country );
                    if ( addr.Type.Equals( "O" ) )
                    {
                        Assertion.AssertNull( "State should be null", addr.StateProvince );
                    }
                }
            }
        }

        [Test]
        public void testStudentSchoolEnrollmentGradeLevelMapping()
        {
            Adk.SifVersion = SifVersion.SIF15r1;
            IDictionary values = new Hashtable();
            values.Add( "GRADE", "00" );
            StringMapAdaptor sma = new StringMapAdaptor( values );
            StudentSchoolEnrollment sse = new StudentSchoolEnrollment();
            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, sse, SifVersion.SIF15r1 );
            sse.SetHomeroom( "RoomInfo", Adk.MakeGuid() );
            Console.WriteLine( sse.ToXml() );

            // This specific case tests what should happen when the grade level is
            // using an undefined value.
            // The valueset entries don't have a value for "00", so "00" should be
            // returned as-is
        }

        [Test]
        public void testStudentContactSIF20()
        {
            Adk.SifVersion = SifVersion.SIF20r1;
            StringMapAdaptor sma = createStudentContactFields();
            StudentContact sc = new StudentContact();
            sc.Type = "E4";
            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, sc, SifVersion.SIF20r1 );
            String value = sc.ToXml();
            Console.WriteLine( value );

            // Verify that the phone number is properly escaped
            int loc = value.IndexOf( "<Number>M&amp;W</Number>" );
            Assertion.Assert( "Phone number should be escaped", loc > -1 );

            // Verify that the @Type attribute is not rendered in SIF 2.0
            loc = value.IndexOf( "Type=\"E4\"" );
            Assertion.AssertEquals( "Type Attribute should not be rendered", -1, loc );

            sc.SifVersion = SifVersion.SIF20r1;
            Element e = sc
                .GetElementOrAttribute( "//PhoneNumber[@Type='0096']/Number" );
            Assertion.AssertNotNull( "School PhoneNumber should be mapped", e );
            Assertion.AssertEquals( "School phone", "8014504555", e.TextValue );

            e = sc.GetElementOrAttribute( "//PhoneNumber[@Type='0350'][1]/Number" );
            Assertion.AssertNotNull( "School PhoneNumber should be mapped", e );
            // Note the " " Space at the end of the value. This should be there,
            // according to the mapping
            Assertion.AssertEquals( "School phone", "8014505555 ", e.TextValue );

            e = sc.GetElementOrAttribute( "//PhoneNumber[@Type='0350'][2]/Number" );
            Assertion.AssertNull( "School PhoneNumber should not be mapped", e );

            AddressList al = sc.AddressList;
            if ( al != null )
            {
                foreach ( Address addr in al )
                {
                    Assertion.AssertNull( "Country should be null", addr.Country );
                    if ( addr.Type.Equals( "1075" ) )
                    {
                        Assertion.AssertNull( "State should be null", addr.StateProvince );
                    }
                }
            }
        }

        private StringMapAdaptor createStudentContactFields()
        {
            IDictionary values = new Hashtable();
            values.Add( "APRN.SOCSECNUM", "123456789" );
            values.Add( "APRN.SCHOOLNUM", "999" );
            values.Add( "APRN.SCHOOLNUM2", "999" );
            values.Add( "APRN.EMAILADDR", null );
            values.Add( "APRN.LASTNAME", "DOE" );
            values.Add( "APRN.FIRSTNAME", "JOHN" );
            values.Add( "APRN.MIDDLENAME", null );
            values.Add( "APRN.WRKADDR", null );
            values.Add( "APRN.WRKCITY", null );
            values.Add( "APRN.WRKSTATE", null );
            values.Add( "APRN.WRKCOUNTRY", null );
            values.Add( "APRN.WRKZIP", "54494" );
            values.Add( "APRN.ADDRESS", null );
            values.Add( "APRN.CITY", null );
            values.Add( "APRN.STATE", null );
            values.Add( "APRN.COUNTRY", null );
            values.Add( "APRN.ZIPCODE", null );
            values.Add( "APRN.TELEPHONE", "8014504555" );
            values.Add( "APRN.ALTTEL", "8014505555" );
            values.Add( "APRN.WRKTEL", null );
            // Test escaping of text values in the phone number field
            values.Add( "APRN.WRKEXTN", "M&W" );
            values.Add( "APRN.RELATION", "01" );
            // values.Add( "APRN.WRKSTATE", "" );
            // values.Add( "APRN.RELATION2", "01" );

            StringMapAdaptor sma = new StringMapAdaptor( values );
            return sma;
        }

        [Test]
        public void testSchoolCourseInfo()
        {
            IDictionary values = new Hashtable();
            values.Add( "CREDVALUE", "0" );
            values.Add( "MAXCREDITS", "1" );

            StringMapAdaptor sma = new StringMapAdaptor( values );
            SchoolCourseInfo sc = new SchoolCourseInfo();
            sc.SchoolYear = 1999;
            Mappings m = fCfg.Mappings.GetMappings( "Default" ).Select( null,
                                                                        null, null );
            m.MapOutbound( sma, sc, SifVersion.SIF15r1 );
            Console.WriteLine( sc.ToXml() );

            Element e = sc.GetElementOrAttribute( "CourseCredits[@Code='01']" );
            Assertion.AssertNotNull( "credits", e );
            Assertion.AssertEquals( "credits", "0", e.TextValue );

            e = sc.GetElementOrAttribute( "CourseCredits[@Code='02']" );
            Assertion.AssertNotNull( "maxcredits", e );
            Assertion.AssertEquals( "maxcredits", "1", e.TextValue );
        }
    }
}