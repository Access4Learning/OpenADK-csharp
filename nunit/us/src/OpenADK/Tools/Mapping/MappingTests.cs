using System;
using System.Collections;
using System.Collections.Specialized;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Food;
using OpenADK.Library.us.Programs;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.Cfg;
using OpenADK.Library.Tools.Mapping;
using OpenADK.Library.Tools.XPath;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Tools.Mapping
{
    /// <summary>
    /// Summary description for MappingTests.
    /// </summary>
    [TestFixture]
    public abstract class MappingTests : BaseMappingsTest
    {
        private SifVersion fVersion;

        private String fFileName;

        private AgentConfig fCfg;

        protected MappingTests( SifVersion testedVersion, String configFileName )
        {
            fVersion = testedVersion;
            fFileName = configFileName;
        }

        [SetUp]
        public override void setUp()
        {
            base.setUp();
            Adk.SifVersion = fVersion;
            fCfg = new AgentConfig();
            fCfg.Read( fFileName, false );
        }

        /**
	 * Asserts default value field mapping behavior
	 *
	 * @
	 */

        [Test]
        public void testFieldMapping010()
        {
            StudentPersonal sp = new StudentPersonal();
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPersonalTest();

            // Add a "default" to the middle name rule and assert that it gets
            // created
            ObjectMapping om = mappings.GetObjectMapping( "StudentPersonal", false );
            FieldMapping middleNameRule = om.GetRule( 3 );
            middleNameRule.DefaultValue = "Jerry";
            map.Remove( "MIDDLE_NAME" );

            StringMapAdaptor adaptor = new StringMapAdaptor( map );

            mappings.MapOutbound( adaptor, sp );
            SifWriter writer = new SifWriter( Console.Out );
            writer.Write( sp );
            writer.Flush();

            // For the purposes of this test, all we care about is the Ethnicity
            // mapping.
            // It should have the default outbound value we specified, which is "7"
            Assertion.AssertEquals( "Middle Name should be Jerry", "Jerry", sp
                                                                                .Name.MiddleName );

            // Now, remap the student back into application fields
            IDictionary restoredData = new Hashtable();
            adaptor.Dictionary = restoredData;
            mappings.MapInbound( sp, adaptor );

            Assertion.AssertEquals( "Middle Name should be Jerry", "Jerry",
                                    restoredData["MIDDLE_NAME"] );

            sp.Name.LastName = null;
            // Now, remap the student back into application fields
            restoredData = new Hashtable();
            adaptor.Dictionary = restoredData;
            mappings.MapInbound( sp, adaptor );

            Object lastName = restoredData["LAST_NAME"];
            Console.WriteLine( sp.ToXml() );
            Assertion.AssertNull( "Last Name should be null", lastName );
        }

        /**
	 * Assert that defaults work even with valueset mapping
	 *
	 * @
	 */

        [Test]
        public void testFieldMapping020()
        {
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPersonalTest();

            // Find the Ethnicity rule
            ObjectMapping om = mappings.GetObjectMapping( "StudentPersonal", false );
            FieldMapping inboundEthnicityRule = null;
            FieldMapping outboundEthnicityRule = null;
            for ( int a = 0; a < om.RuleCount; a++ )
            {
                FieldMapping fm = om.GetRule( a );
                if ( fm.FieldName.Equals( "ETHNICITY" ) )
                {
                    if ( fm.Filter.Direction == MappingDirection.Inbound )
                    {
                        inboundEthnicityRule = fm;
                    }
                    else
                    {
                        outboundEthnicityRule = fm;
                    }
                }
            }

            // Put a value into the map that won't match the valueset
            map[ "ETHNICITY" ] = "abcdefg" ;
            StudentPersonal sp = new StudentPersonal();
            MapOutbound( sp, mappings, map );

            // There's no default value defined, so we expect that what get's put in
            // is what we get out
            Assertion.AssertEquals( "Outbound Ethnicity", "abcdefg", sp.Demographics.RaceList.ItemAt( 0 ).Code );

            // Now, remap the student back into application fields
            IDictionary restoredData = new Hashtable();

            StringMapAdaptor adaptor = new StringMapAdaptor( restoredData );
            mappings.MapInbound( sp, adaptor );

            // The value "abcdefg" does not have a match in the value set
            // It should be passed back through
            Assertion.AssertEquals( "Inbound Ethnicity", "abcdefg", restoredData["ETHNICITY"] );

            inboundEthnicityRule.DefaultValue = "11111";
            outboundEthnicityRule.DefaultValue = "99999";
            sp = new StudentPersonal();
            MapOutbound( sp, mappings, map );

            // It should have the default value we specified, which is "99999"
            Assertion.AssertEquals( "Outbound Ethnicity", "99999", sp.Demographics.RaceList.ItemAt( 0 ).Code );

            // Now, remap the student back into application fields
            restoredData = new Hashtable();
            adaptor.Dictionary = restoredData;
            mappings.MapInbound( sp, adaptor );

            // The value "9999" does not have a match in the value set
            // we want it to take on the default value, which is "111111"
            Assertion.AssertEquals( "Inbound Ethnicity", "11111", restoredData["ETHNICITY"] );

            // Now, do the mapping again, this time with the Ethnicity value
            // completely missing
            sp = new StudentPersonal();
            map.Remove( "ETHNICITY" );
            MapOutbound( sp, mappings, map );

            // Ethnicity should be set to "99999" in this case because we didn't set
            // the "ifNull" behavior and the
            // default behavior is "NULL_DEFAULT"
            Assertion.AssertEquals( "Outbound Ethnicity", "99999", sp.Demographics
                                                                       .RaceList.ItemAt( 0 ).Code );

            // Now, do the mapping again, this time with the NULL_DEFALT behavior.
            // The result should be the same
            outboundEthnicityRule.NullBehavior = MappingBehavior.IfNullDefault;
            sp = new StudentPersonal();
            MapOutbound( sp, mappings, map );

            // Ethnicity should be set to "99999" in this case because we set the
            // ifnull behavior to "NULL_DEFAULT"
            Assertion.AssertEquals( "Outbound Ethnicity", "99999", sp.Demographics.RaceList.ItemAt( 0 ).Code );

            outboundEthnicityRule.NullBehavior = MappingBehavior.IfNullSuppress;
            sp = new StudentPersonal();
            MapOutbound( sp, mappings, map );

            // Ethnicity should be null in this case because we told it to suppress
            // set the "ifNull" behavior
            Assertion.AssertNull( "Ethnicity should not be set", sp.Demographics.RaceList );
        }

        /**
	 * Asserts default value field mapping behavior
	 *
	 * @
	 */

        [Test]
        public void testStudentContactMapping010()
        {
            StudentContact sc = new StudentContact();
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IFieldAdaptor adaptor = createStudentContactFields();

            mappings.MapOutbound( adaptor, sc );
            SifWriter writer = new SifWriter( Console.Out );
            writer.Write( sc );
            writer.Flush();

            Assertion.AssertEquals( "Testing Pickup Rights", "Yes", sc.ContactFlags.PickupRights );
        }

        /**
	 * @param sp
	 * @param mappings
	 * @param map
	 * @throws AdkMappingException
	 */

        private void MapOutbound( StudentPersonal sp, Mappings mappings, IDictionary map )
        {
            StringMapAdaptor adaptor = new StringMapAdaptor( map );
            mappings.MapOutbound( adaptor, sp );

            SifWriter writer = new SifWriter( Console.Out );
            writer.Write( sp );
            writer.Flush();
            writer.Close();
        }

        [Test]
        public void testValueSetMapping010()
        {
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            ValueSet vs = mappings.GetValueSet( "Ethnicity", false );

            // Normal Translation
            Assertion.AssertEquals( "Translate", "A", vs.Translate( "1" ) );
            Assertion.AssertEquals( "TranslateReverse", "1", vs.TranslateReverse( "A" ) );
            // Default Values
            Assertion.AssertEquals( "Translate", "ZZZ", vs.Translate( "foo", "ZZZ" ) );
            Assertion.AssertEquals( "TranslateReverse", "AAA", vs.TranslateReverse( "foo",
                                                                                    "AAA" ) );

            // Test Null behavior
            Assertion.AssertNull( "TranslateNull", vs.Translate( null ) );
            Assertion.AssertNull( "TranslateReverseNull", vs.TranslateReverse( null ) );

            // No Match (this time should return what we pass in)
            Assertion.AssertEquals( "TranslateNoMatch", "QQQQ", vs.Translate( "QQQQ" ) );
            Assertion.AssertEquals( "TranslateReverseNoMath", "QQQQ", vs
                                                                          .TranslateReverse( "QQQQ" ) );

            // //////////////////////////////////
            //
            // Add an app default
            //
            // //////////////////////////////////
            vs.SetAppDefault( "6", false );

            // Normal Translation
            Assertion.AssertEquals( "Translate", "A", vs.Translate( "1" ) );
            Assertion.AssertEquals( "TranslateReverse", "1", vs.TranslateReverse( "A" ) );
            // Default Values
            Assertion.AssertEquals( "Translate", "ZZZ", vs.Translate( "foo", "ZZZ" ) );
            Assertion.AssertEquals( "TranslateReverse", "AAA", vs.TranslateReverse( "foo",
                                                                                    "AAA" ) );

            // Test Null behavior
            Assertion.AssertNull( "TranslateNull", vs.Translate( null ) );
            Assertion.AssertNull( "TranslateReverseNull", vs.TranslateReverse( null ) );

            // No Match (this time should return a default for app value)
            Assertion.AssertEquals( "TranslateNoMatch", "QQQQ", vs.Translate( "QQQQ" ) );
            Assertion.AssertEquals( "TranslateReverseNoMath", "6", vs.TranslateReverse( "QQQQ" ) );

            // ////////////////////////////////
            //
            // Add a SIF default
            //
            // //////////////////////////////////
            vs.SetSifDefault( "H", false );

            // Normal Translation
            Assertion.AssertEquals( "Translate", "A", vs.Translate( "1" ) );
            Assertion.AssertEquals( "TranslateReverse", "1", vs.TranslateReverse( "A" ) );
            // Default Values
            Assertion.AssertEquals( "Translate", "ZZZ", vs.Translate( "foo", "ZZZ" ) );
            Assertion.AssertEquals( "TranslateReverse", "AAA", vs.TranslateReverse( "foo",
                                                                                    "AAA" ) );

            // Test Null behavior
            Assertion.AssertNull( "TranslateNull", vs.Translate( null ) );
            Assertion.AssertNull( "TranslateReverseNull", vs.TranslateReverse( null ) );

            // No Match (this time should return a default for app value and sif
            // value)
            Assertion.AssertEquals( "TranslateNoMatch", "H", vs.Translate( "QQQQ" ) );
            Assertion.AssertEquals( "TranslateReverseNoMath", "6", vs.TranslateReverse( "QQQQ" ) );

            // //////////////////////////////////
            //
            // Change the App and SIF Value and set it to render if null
            //
            // //////////////////////////////////
            vs.SetSifDefault( "C", true );
            vs.SetAppDefault( "7", true );

            // Normal Translation
            Assertion.AssertEquals( "Translate", "A", vs.Translate( "1" ) );
            Assertion.AssertEquals( "TranslateReverse", "1", vs.TranslateReverse( "A" ) );
            // Default Values
            Assertion.AssertEquals( "Translate", "ZZZ", vs.Translate( "foo", "ZZZ" ) );
            Assertion.AssertEquals( "TranslateReverse", "AAA", vs.TranslateReverse( "foo",
                                                                                    "AAA" ) );

            // Test Null behavior
            Assertion.AssertEquals( "TranslateNullReturnsDefault", "C", vs.Translate( null ) );
            Assertion.AssertEquals( "TranslateReverseNullReturnsDefault", "7", vs
                                                                                   .TranslateReverse( null ) );

            // No Match (this time should return a default for app value and sif
            // value)
            Assertion.AssertEquals( "TranslateNoMatch", "C", vs.Translate( "QQQQ" ) );
            Assertion.AssertEquals( "TranslateReverseNoMath", "7", vs.TranslateReverse( "QQQQ" ) );
        }

        /**
	 * This test creates a StudentPersonal object and then builds it out using
	 * outbound mappings.
	 *
	 * Then, it uses the mappings again to rebuild a new Hashtable. The
	 * Hashtables are then compared to ensure that the resulting data is
	 * identical
	 *
	 * @
	 */

        [Test]
        public void testStudentMappingAdk15Mappings()
        {
            StudentPersonal sp = new StudentPersonal();
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPersonalTest();

            StringMapAdaptor adaptor = new StringMapAdaptor( map );
            mappings.MapOutbound( adaptor, sp );

            SifWriter writer = new SifWriter( Console.Out );
            writer.Write( sp );
            writer.Flush();

            // Assert that the StudentPersonal object was mapped correctly
            assertStudentPersonal( sp );

            // Now, map the student personal back to a hashmap and assert it
            IDictionary restoredData = new Hashtable();
            adaptor.Dictionary = restoredData;
            mappings.MapInbound( sp, adaptor );
            assertMapsAreEqual( map, restoredData, "ALT_PHONE_TYPE" );
        }

        /**
	 * This test creates a StudentPlacement object and then builds it out using
	 * outbound mappings.
	 *
	 * Then, it uses the mappings again to rebuild a new Hashtable. The
	 * hashtables are then compared.
	 *
	 * @
	 */

        [Test]
        public void testStudentPlacementMapping()
        {
            StudentPlacement sp = new StudentPlacement();
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPlacementTest();
            StringMapAdaptor sma = new StringMapAdaptor( map );
            mappings.MapOutbound( sma, sp );

            sp = (StudentPlacement) AdkObjectParseHelper.WriteParseAndReturn( sp, fVersion );

            // Assert that the StudentPlacement object was mapped correctly
            assertStudentPlacement( sp );

            // Now, map the StudentPlacement back to a hashmap and assert it
            IDictionary restoredData = new Hashtable();
            sma = new StringMapAdaptor( restoredData );
            mappings.MapInbound( sp, sma );
            assertMapsAreEqual( map, restoredData );
        }

        /**
	 * This test creates a StudentMeal object and then builds it out using
	 * outbound mappings.
	 *
	 * Then, it uses the mappings again to rebuild a new Hashtable. The
	 * hashtables are then compared.
	 *
	 * @
	 */

        [Test]
        public void testStudentMealMappings()
        {
            StudentMeal sm = new StudentMeal();
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = new Hashtable();
            map.Add( "Balance", "10.55" );

            StringMapAdaptor sma = new StringMapAdaptor( map );
            mappings.MapOutbound( sma, sm );

            sm = (StudentMeal) AdkObjectParseHelper.WriteParseAndReturn( sm, fVersion );

            // Assert that the object was mapped correctly
            FSAmounts amounts = sm.Amounts;
            Assertion.AssertNotNull( amounts );
            FSAmount amount = amounts.ItemAt( 0 );
            Assertion.Assert( amount.Value.HasValue );
            Assertion.AssertEquals( 10.55, amount.Value.Value );


            // Now, map the object back to a hashmap and assert it
            IDictionary restoredData = new Hashtable();
            sma = new StringMapAdaptor( restoredData );
            mappings.MapInbound( sm, sma );
            assertMapsAreEqual( map, restoredData );
        }

        /**
	 * This test creates a SectionInfo object and then builds it out using
	 * outbound mappings.
	 *
	 * Then, it uses the mappings again to rebuild a new Hashtable. The
	 * hashtables are then compared.
	 *
	 * @
	 */

        [Test]
        public void testSectionInfoMappings()
        {
            SectionInfo si = new SectionInfo();
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = new Hashtable();
            map.Add( "STAFF_REFID", "123456789ABCDEF" );

            StringMapAdaptor sma = new StringMapAdaptor( map );
            mappings.MapOutbound( sma, si );

            si = (SectionInfo) AdkObjectParseHelper.WriteParseAndReturn( si, fVersion );

            // Assert that the object was mapped correctly
            ScheduleInfoList sil = si.ScheduleInfoList;
            Assertion.AssertNotNull( sil );
            ScheduleInfo schedule = sil.ItemAt( 0 );
            Assertion.AssertNotNull( schedule );
            TeacherList tl = schedule.TeacherList;
            Assertion.AssertNotNull( tl );
            StaffPersonalRefId refId = tl.ItemAt( 0 );
            Assertion.AssertNotNull( refId );
            Assertion.AssertEquals( "123456789ABCDEF", refId.Value );


            // Now, map the object back to a hashmap and assert it
            IDictionary restoredData = new Hashtable();
            sma = new StringMapAdaptor( restoredData );
            mappings.MapInbound( si, sma );
            assertMapsAreEqual( map, restoredData );
        }


        protected abstract IDictionary buildIDictionaryForStudentPlacementTest();


        protected abstract void assertStudentPlacement( StudentPlacement sp );


        private void assertByXPath( SifXPathContext context, String xPath,
                                    String assertedValue )
        {
            Element e = (Element) context.GetValue( xPath );
            Assertion.AssertNotNull( "Field is null for path " + xPath, e );
            SifSimpleType value = e.SifValue;
            Assertion.AssertNotNull( "Value is null for path " + xPath, value );
            Assertion.AssertEquals( xPath, assertedValue, value.ToString() );
        }

        protected StringMapAdaptor createStudentContactFields()
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
            values.Add( "APRN.WRKEXTN", null );
            values.Add( "APRN.RELATION", "01" );
            values.Add( "APRN.PICKUPRIGHTS", "Yes" );

            StringMapAdaptor sma = new StringMapAdaptor( values );
            return sma;
        }

        [Test]
        public void StudentMapping()
        {
            StudentPersonal sp = new StudentPersonal();
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPersonalTest();
            StringMapAdaptor sma = new StringMapAdaptor( map );
            mappings.MapOutbound( sma, sp );

            SifWriter writer = new SifWriter( Console.Out );
            writer.Write( sp );
            writer.Flush();

            // Assert that the StudentPersonal object was mapped correctly
            assertStudentPersonal( sp );

            // Now, map the student personal back to a hashmap and assert it
            IDictionary restoredData = new HybridDictionary();
            StringMapAdaptor restorer = new StringMapAdaptor( restoredData );
            mappings.MapInbound( sp, restorer );
            assertDictionariesAreEqual( restoredData, map );
        }

        [Test]
        public void StudentPlacementMapping()
        {
            StudentPlacement sp = new StudentPlacement();
            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPlacementTest();
            StringMapAdaptor sma = new StringMapAdaptor( map );
            mappings.MapOutbound( sma, sp );

            SifWriter writer = new SifWriter( Console.Out );
            writer.Write( sp );
            writer.Flush();

            // Assert that the StudentPersonal object was mapped correctly
            assertStudentParticipation( sp );

            // Now, map the student personal back to a hashmap and assert it
            IDictionary restoredData = new HybridDictionary();
            StringMapAdaptor restorer = new StringMapAdaptor( restoredData );
            mappings.MapInbound( sp, restorer );
            assertDictionariesAreEqual( map, restoredData );
        }


        private IDictionary buildIDictionaryForStudentPersonalTest()
        {
            IDictionary data = new Hashtable();
            data.Add( "STUDENT_NUM", "998" );
            data.Add( "LAST_NAME", "Johnson" );
            data.Add( "MIDDLE_NAME", "George" );
            data.Add( "FIRST_NAME", "Betty" );
            data.Add( "BIRTHDATE", "19900101" );
            data.Add( "ETHNICITY", "4" );
            data.Add( "HOME_PHONE", "202-358-6687" );
            data.Add( "CELL_PHONE", "202-502-4856" );
            data.Add( "ALT_PHONE", "201-668-1245" );
            data.Add( "ALT_PHONE_TYPE", "TE" );

            data.Add( "ACTUALGRADYEAR", "2007" );
            data.Add( "ORIGINALGRADYEAR", "2005" );
            data.Add( "PROJECTEDGRADYEAR", "2007" );

            data.Add( "ADDR1", "321 Oak St" );
            data.Add( "ADDR2", "APT 11" );
            data.Add( "CITY", "Metropolis" );
            data.Add( "STATE", "IL" );
            data.Add( "COUNTRY", "US" );
            data.Add( "ZIPCODE", "321546" );

            return data;
        }


        private void assertStudentParticipation( StudentPlacement sp )
        {
            Assert.AreEqual( "0000000000000000", sp.RefId, "RefID" );
            Assert.AreEqual("0000000000000000", sp.StudentPersonalRefId, "StudentPersonalRefid");
            Assert.AreEqual("Local", sp.Service.CodeType, "Code Type");
            Assert.AreEqual("Related Service", sp.Service.Type, "Type");
            Assert.AreEqual("ZZZ99987", sp.Service.TextValue, "Service");
        }


        private void assertStudentPersonal( StudentPersonal sp )
        {
            DateTime birthDate = new DateTime( 1990, 1, 1 );

            Assertion.AssertEquals( "First Name", "Betty", sp.Name.FirstName );
            Assertion.AssertEquals( "Middle Name", "George", sp.Name.MiddleName );
            Assertion.AssertEquals( "Last Name", "Johnson", sp.Name.LastName );
            Assertion.AssertEquals( "Student Number", "998", sp.OtherIdList.ItemAt( 0 ).TextValue );
            Assertion.AssertEquals( "Birthdate", birthDate, sp.Demographics.BirthDate.Value );
            Assertion.AssertEquals( "Ethnicity", "H", sp.Demographics.RaceList.ItemAt( 0 ).Code );

            PhoneNumberList pnl = sp.PhoneNumberList;
            Assertion.AssertNotNull( "PhoneNumberList", pnl );

            PhoneNumber homePhone = pnl[PhoneNumberType.SIF1x_HOME_PHONE];
            Assertion.AssertNotNull( "Home Phone is null", homePhone );
            Assertion.AssertEquals( "Home Phone", "202-358-6687", homePhone
                                                                      .Number );

            PhoneNumber cellPhone = pnl
                [PhoneNumberType.SIF1x_PERSONAL_CELL];
            Assertion.AssertNotNull( "cellPhone Phone is null", cellPhone );
            Assertion.AssertEquals( "Cell Phone", "202-502-4856", cellPhone.Number );

            SifXPathContext xpathContext = SifXPathContext.NewSIFContext( sp, SifVersion.SIF20r1 );
            assertByXPath( xpathContext, "AddressList/Address/Street/Line1",
                           "321 Oak St" );
            assertByXPath( xpathContext, "AddressList/Address/Street/Line1",
                           "321 Oak St" );
            assertByXPath( xpathContext, "AddressList/Address/Street/Line2",
                           "APT 11" );
            assertByXPath( xpathContext, "AddressList/Address/City", "Metropolis" );
            assertByXPath( xpathContext, "AddressList/Address/StateProvince", "IL" );
            assertByXPath( xpathContext, "AddressList/Address/Country", "US" );
            assertByXPath( xpathContext, "AddressList/Address/PostalCode", "321546" );

            /*
             * These assertions are currently commented out because the Adk does not
             * currently support Repeatable elements that have wildcard attributes
             *
             * PhoneNumber number = sp.PhoneNumber( PhoneNumberType.PHONE );
             * Assertion.AssertNotNull( "Alternate Phone Element is null", number );
             * Assertion.AssertEquals( "Alternate Phone", "201-668-1245",
             * number.ToString() );
             */

            Assertion.AssertEquals( "OriginalGradYear", 2005, sp.OnTimeGraduationYear.Value );
            Assertion.AssertEquals( "Projected", 2007, sp.ProjectedGraduationYear.Value );
            Assertion.AssertNotNull( "Actual Grad Year", sp.GraduationDate.Value );
            Assertion.AssertEquals( "OriginalGradYear", 2007, sp.GraduationDate.Year );
        }


        private void assertDictionariesAreEqual( IDictionary map1, IDictionary map2 )
        {
            foreach ( Object key in map1.Keys )
            {
                Assert.AreEqual( map1[key], map2[key], key.ToString() );
            }
        }
    }
}