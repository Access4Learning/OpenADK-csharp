using System;
using System.Collections;
using System.Collections.Generic;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.Mapping;
using NUnit.Framework;
using OpenADK.Library.us;

namespace Library.Nunit.US.Tools.Mapping
{
    [TestFixture]
    public class AdvancedMappingsTests : BaseMappingsTest
    {
        public AdvancedMappingsTests()
        {
            Adk.Debug = AdkDebugFlags.All;
            Adk.Initialize(SifVersion.SIF20, SIFVariant.SIF_US, (int)SdoLibraryType.All);
        }

        public static String flattenDate( IValueBuilder vb, String dateString_in )
        {
            ((TestValueBuilder) vb).WasCalled = true;
            return dateString_in.Replace( "-", "" ).Trim();
        }

        [Test]
        public void TestCustomFunctionOutbound()
        {
            String flattenDateFunctionCall = "<agent id=\"Repro\" sifVersion=\"2.0\">"
                                             + "    <mappings id=\"Default\">"
                                             + "        <object object=\"StudentPersonal\">"
                                             + "          <field name=\"StudentPers_guid\">@RefId</field>"
                                             +
                                             "          <field name=\"DOB\">Demographics/BirthDate=@flattenDate( $(DOB) )</field>"
                                             + "        </object>"
                                             + "    </mappings>"
                                             + "</agent>";

            IDictionary map = new Dictionary<String, String>();
            map.Add( "StudentPers_guid", "1234" );
            map.Add( "DOB", "19900904" );
            StringMapAdaptor sma = new StringMapAdaptor( map );
            TestValueBuilder tvb = new TestValueBuilder( sma );
            DefaultValueBuilder.AddAlias( "flattenDate", GetType().AssemblyQualifiedName );
            Adk.SifVersion = SifVersion.SIF20;
            StudentPersonal sp = mapToStudentPersonal( sma, flattenDateFunctionCall, tvb );
            Assertion.Assert( "flattenDate should have been called", tvb.WasCalled );
            Assertion.AssertNotNull( "Student should not be null", sp );
            Assertion.AssertNotNull( "BirthDate should not be null", sp.Demographics.BirthDate );
        }

        [Test]
        public void TestVariableOutbound()
        {
            String flattenDateFunctionCall = "<agent id=\"Repro\" sifVersion=\"2.0\">"
                                             + "    <mappings id=\"Default\">"
                                             + "        <object object=\"StudentPersonal\">"
                                             + "            <field name=\"StudentPers_guid\">@RefId</field>"
                                             + "            <field name=\"DOB\">Demographics/BirthDate=$(DOB)</field>"
                                             + "        </object>" + "    </mappings>" + "</agent>";

            IDictionary map = new Dictionary<String, String>();
            map.Add( "StudentPers_guid", "1234" );
            map.Add( "DOB", "19900904" );
            StringMapAdaptor sma = new StringMapAdaptor( map );
            StudentPersonal sp = mapToStudentPersonal( sma, flattenDateFunctionCall, null );
            Assertion.AssertNotNull( "Student should not be null", sp );
            Assertion.AssertNotNull( "BirthDate should not be null", sp.Demographics.BirthDate );
        }

        [Test]
        public void TestCustomFunctionInbound()
        {
            //		String customMappings = 
            //		  "<agent id='Repro' sifVersion='2.0'>"
            //		+ "   <mappings id='Default'>"
            //		+ "     <object object='StudentPersonal'>"
            //		+ "       <field name='FIRSTNAME'>testcase:ProperCase(//FirstName)</field>"
            //		+ "</object></mappings></agent>";
            //		
            //		
            //		StudentPersonal sp = new StudentPersonal();
            //		sp.SetElementOrAttribute( "Name/FirstName", "JIMMY" );
            //		
            //		
            //		TestValueBuilder tvb = new TestValueBuilder(sma);
            //		IDictionary map = doInboundMapping( customMappings, sp );
            //		assertTrue("ProperCase should have been called", tvb.getWasCalled());
            //		
            //		Assertion.AssertNotNull("Student should not be null", sp);
            //		Assertion.AssertNotNull("BirthDate should not be null", sp.getDemographics()
            //				.getBirthDate());
        }

        [Test]
        public void TestADKFunctionInbound()
        {
            String customMappings =
                "<agent id='Repro' sifVersion='2.0'>"
                + "   <mappings id='Default'>"
                + "     <object object='StudentPersonal'>"
                + "       <field name='FIRSTNAME'>//FirstName</field>"
                + "       <field name='LASTNAME'>//LastName</field>"
                + "       <field name='FULLNAME'>adk:toProperCase( concat( $FIRSTNAME, ' ', $LASTNAME ) )</field>"
                + "</object></mappings></agent>";


            StudentPersonal sp = new StudentPersonal();
            sp.SetElementOrAttribute( "Name/FirstName", "ahmad" );
            sp.SetElementOrAttribute( "Name/LastName", "O'TOOLE" );

            IDictionary map = doInboundMapping( customMappings, sp );

            Assertion.AssertEquals( "First Name", "ahmad", map["FIRSTNAME"] );
            Assertion.AssertEquals( "Last Name", "O'TOOLE", map["LASTNAME"] );
            Assertion.AssertEquals( "Full Name", "Ahmad O'Toole", map["FULLNAME"] );
        }


        /**
  * Asserts default value field mapping behavior
  * @
  */

        [Explicit]
        public void TestVariableMapping()
        {
            String customMappings =
                "<agent id='Repro' sifVersion='2.0'>"
                + "   <mappings id='Default'>"
                + "     <object object='StudentPersonal'>"
                + "       <field name='STUDENT_NUM'>OtherIdList/OtherId[@Type='06']</field>"
                + "       <field name='HOMEROOM'>$STUDENT_NUM</field>"
                + "</object></mappings></agent>";


            StudentPersonal sp = new StudentPersonal();
            sp.SetElementOrAttribute( "OtherIdList/OtherId[@Type='06']", "998" );

            IDictionary map = doInboundMapping( customMappings, sp );

            Assertion.AssertEquals( "STUDENT_NUM", "998", map["STUDENT_NUM"] );
            Assertion.AssertEquals( "HOMEROOM", "998", map["HOMEROOM"] );

            Console.WriteLine( "HomeRoom = " + map["HOMEROOM"] );
        }

        [Test]
        public void TestConcatenateFieldsInbound()
        {
            String customMappings =
                "<agent id='Repro' sifVersion='2.0'>"
                + "   <mappings id='Default'>"
                + "     <object object='StudentPersonal'>"
                + "       <field name='FIRSTNAME'>//FirstName</field>"
                + "       <field name='LASTNAME'>//LastName</field>"
                + "       <field name='FULLNAME'>concat( //FirstName, ' ' , //LastName )</field>"
                + "</object></mappings></agent>";


            StudentPersonal sp = new StudentPersonal();
            sp.SetElementOrAttribute( "Name/FirstName", "Jimmy" );
            sp.SetElementOrAttribute( "Name/LastName", "Johnson" );

            IDictionary map = doInboundMapping( customMappings, sp );

            Assertion.AssertEquals( "First Name", "Jimmy", map["FIRSTNAME"] );
            Assertion.AssertEquals( "Last Name", "Johnson", map["LASTNAME"] );
            Assertion.AssertEquals( "Full Name", "Jimmy Johnson", map["FULLNAME"] );
        }

        [Test]
        public void TestConcatenateFieldsInbound020()
        {
            String customMappings =
                "<agent id='Repro' sifVersion='2.0'>"
                + "   <mappings id='Default'>"
                + "     <object object='StudentPersonal'>"
                +
                "       <field name='CITY_STATE_ZIP'>concat( AddressList/Address/City, ', ', AddressList/Address/StateProvince, '  ', AddressList/Address/PostalCode)</field>"
                + "</object></mappings></agent>";


            StudentPersonal sp = new StudentPersonal();
            sp.SetElementOrAttribute( "AddressList/Address[@Type='04']/City", "Chicago" );
            sp.SetElementOrAttribute( "AddressList/Address[@Type='04']/StateProvince", "IL" );
            sp.SetElementOrAttribute( "AddressList/Address[@Type='04']/PostalCode", "50001" );
            sp.SetElementOrAttribute( "Name/LastName", "Johnson" );

            Console.WriteLine( "StudentPersonal=" + sp.ToXml() );

            IDictionary map = doInboundMapping( customMappings, sp );

            String csz = (String) map["CITY_STATE_ZIP"];
            Console.WriteLine( "City State Zip=" + csz );
            Assertion.AssertEquals( "CityStateZip", "Chicago, IL  50001", map["CITY_STATE_ZIP"] );
        }

        [Test]
        public void TestCustomVariablesInbound()
        {
            String customMappings =
                "<agent id='Repro' sifVersion='2.0'>"
                + "   <mappings id='Default'>"
                + "     <object object='StudentPersonal'>"
                + "       <field name='FIRSTNAME'>//FirstName</field>"
                + "       <field name='LASTNAME'>//LastName</field>"
                + "       <field name='FULLNAME'>concat( $FIRSTNAME, ' ', $LASTNAME )</field>"
                + "</object></mappings></agent>";


            StudentPersonal sp = new StudentPersonal();
            sp.SetElementOrAttribute( "Name/FirstName", "Jimmy" );
            sp.SetElementOrAttribute( "Name/LastName", "Johnson" );

            IDictionary map = doInboundMapping( customMappings, sp );

            Assertion.AssertEquals( "First Name", "Jimmy", map["FIRSTNAME"] );
            Assertion.AssertEquals( "Last Name", "Johnson", map["LASTNAME"] );
            Assertion.AssertEquals( "Full Name", "Jimmy Johnson", map["FULLNAME"] );
        }

        [Test]
        public void TestConcatenateFields()
        {
            String configFileText = "<agent id=\"Repro\" sifVersion=\"2.0\">"
                                    + "    <mappings id=\"Default\">"
                                    + "        <object object=\"StudentPersonal\">"
                                    + "            <field name=\"StudentPers_guid\">@RefId</field>"
                                    + "            <field name=\"LastName\">Name[@Type='01']/LastName</field>"
                                    + "            <field name=\"FirstName\">Name[@Type='01']/FirstName</field>"
                                    + "            <field name=\"MiddleName\">Name[@Type='01']/MiddleName</field>"
                                    +
                                    "            <field name=\"FullName\">Name[@Type='01']/FullName=$(LastName), $(FirstName) $(MiddleName)</field>"
                                    + "        </object>" + "    </mappings>" + "</agent>";

            IDictionary map = new Dictionary<String, String>();
            map.Add( "StudentPers_guid", "1234" );
            map.Add( "LastName", "Finale" );
            map.Add( "FirstName", "Prima" );
            map.Add( "MiddleName", "Mediccio" );

            map.Add( "FullName", "" );
            StringMapAdaptor sma = new StringMapAdaptor( map );
            TestValueBuilder tvb = new TestValueBuilder( sma );
            StudentPersonal sp = mapToStudentPersonal( sma, configFileText, tvb );

            Assertion.AssertNotNull( "Student should not be null", sp );

            SimpleField fullName = (SimpleField) sp
                                                     .GetElementOrAttribute( "Name/FullName" );
            Assertion.AssertNotNull( "FullName", fullName );
            Assertion.AssertEquals( "FullName", "Finale, Prima Mediccio", fullName.Value );
        }

        [Test]
        public void TestStringConcat()
        {
            String configFileText = "<agent id=\"Repro\" sifVersion=\"2.0\">"
                                    + "    <mappings id=\"Default\">"
                                    + "        <object object=\"StudentPersonal\">"
                                    +
                                    "            <field name=\"FullName\">Name[@Type='01']/FullName=NAME:$(LastName), $(FirstName) $(MiddleName)</field>"
                                    + "        </object>" + "    </mappings>" + "</agent>";

            IDictionary map = new Dictionary<String, String>();
            map.Add( "StudentPers_guid", "1234" );
            map.Add( "LastName", "Finale" );
            map.Add( "FirstName", "Prima" );
            map.Add( "MiddleName", "Mediccio" );
            map.Add( "FullName", "" );

            StringMapAdaptor sma = new StringMapAdaptor( map );
            TestValueBuilder tvb = new TestValueBuilder( sma );
            StudentPersonal sp = mapToStudentPersonal( sma, configFileText, tvb );

            Assertion.AssertNotNull( "Student should not be null", sp );

            SimpleField fullName = (SimpleField) sp
                                                     .GetElementOrAttribute( "Name/FullName" );
            Assertion.AssertNotNull( "FullName", fullName );
            Assertion.AssertEquals( "FullName", "NAME:Finale, Prima Mediccio", fullName.Value );
        }

        [Explicit]
        public void TestInheritRules()
        {
            String configFileText1_ = "<agent id=\"mcmTest.MappingsTest\" sifVersion=\"2.0\">\n"
                                      + " <mappings id=\"Default\">\n"
                                      + " <object object=\"StudentPersonal\">\n"
                                      + " <field name=\"StudentPers_guid\">@RefId</field>\n"
                                      + " <field name=\"LastName\">Name[@Type='01']/LastName</field>\n"
                                      + " <field name=\"FirstName\">Name[@Type='01']/FirstName</field>\n"
                                      + " <field name=\"MiddleName\">Name[@Type='01']/MiddleName</field>\n"
                                      +
                                      " <field name=\"Street\">AddressList[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/Street/Line1</field>\n"
                                      +
                                      " <field name=\"City\">AddressList[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/City</field>\n"
                                      +
                                      " <field name=\"State\">AddressList[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/StateProvince</field>\n"
                                      +
                                      " <field name=\"Zip\">AddressList[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/PostalCode</field>\n"
                                      + " </object>\n"
                                      + "   <mappings id=\"Child\" zoneId=\"Zone A\">\n"
                                      + "     <object object=\"StudentPersonal\">\n"
                                      + "       <field name=\"LastName\">Name[@Type='06']/LastName</field>\n"
                                      + "       <field name=\"FirstName\">Name[@Type='06']/FirstName</field>\n"
                                      + "       <field name=\"MiddleName\">Name[@Type='06']/MiddleName</field>\n"
                                      + "     </object>\n"
                                      + "   </mappings>\n"
                                      + " </mappings>\n"
                                      + "</agent>";

            IDictionary psValueMap = new Dictionary<String, String>();
            psValueMap.Add( "StudentPers_guid", "14050614103526133C3FD2324C5BC8FF" );
            psValueMap.Add( "LastName", "Finale" );
            psValueMap.Add( "FirstName", "Prima" );
            psValueMap.Add( "MiddleName", "Mediccio" );
            psValueMap.Add( "Street", "667 Gate Way" );
            psValueMap.Add( "City", "Sacramento" );
            psValueMap.Add( "State", "CA" );
            psValueMap.Add( "Zip", "91020" );

            StringMapAdaptor sma = new StringMapAdaptor( psValueMap );
            StudentPersonal sp = doOutboundMappingSelect( sma, configFileText1_, "Zone A", null, null );

            Assertion.AssertNotNull( "Student should not be null", sp );

            SifElement address = (SifElement) sp
                                                  .GetElementOrAttribute(
                                                  "AddressList[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']" );
            Assertion.AssertNotNull( "Student Address should have mapped", address );

            SifElement name = (SifElement) sp
                                               .GetElementOrAttribute( "Name[@Type='06']" );
            Assertion.AssertNotNull( "Name should have mapped to '06'", name );
        }


        /**
  * Tests doing a double mapping to the same element, the second one by position
  * @
  */

        [Test]
        public void TestDoubleOutboundMapping()
        {
            String mapping = "<agent id=\"Repro\" sifVersion=\"2.0\">"
                             + "    <mappings id=\"Default\">"
                             + "        <object object=\"StudentPersonal\">"
                             + "           <field name='PHONE_TYPE'>PhoneNumberList/PhoneNumber/@Type</field>"
                             + "           <field name='PHONE'>PhoneNumberList/PhoneNumber[1]/Number</field>"
                             + "        </object>" + "    </mappings>" + "</agent>";

            IDictionary map = new Dictionary<String, String>();
            map.Add( "PHONE_TYPE", "1234" );
            map.Add( "PHONE", "715-555-5555" );
            StringMapAdaptor sma = new StringMapAdaptor( map );
            StudentPersonal sp = mapToStudentPersonal( sma, mapping, null );
            Assertion.AssertNotNull( "Student should not be null", sp );
            PhoneNumberList phoneList = sp.PhoneNumberList;
            Assertion.AssertEquals( "One Phone", 1, phoneList.ChildCount );

            PhoneNumber phone = (PhoneNumber) phoneList.GetChildList()[0];
            Assertion.AssertNotNull( "Phone should not be null", phone );
            Assertion.AssertEquals( "Phone type", "1234", phone.Type );
            Assertion.AssertEquals( "PhoneNumber", "715-555-5555", phone.Number );
        }
    }
}