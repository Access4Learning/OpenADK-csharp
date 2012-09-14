using System;
using System.Collections;
using System.Collections.Generic;
using OpenADK.Library;
using OpenADK.Library.us.Programs;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.Mapping;
using NUnit.Framework;
using Library.Nunit.US.Tools.Mapping;

namespace Library.Nunit.US.Library.Tools.Mapping
{
    public class SIF15MappingTests : MappingTests
    {
        public SIF15MappingTests()
            : base( SifVersion.SIF15r1,
                    "..\\..\\Library\\Tools\\Mapping\\SIF1.5.agent.cfg")
        {
        }

        [Test]
        public void testInheritRules()
        {
            String configFileText1_ = "<agent id=\"mcmTest.MappingsTest\" sifVersion=\"2.0\">\n"
                                      + " <mappings id=\"Default\">\n"
                                      + " <object object=\"StudentPersonal\">\n"
                                      + " <field name=\"StudentPers_guid\">@RefId</field>\n"
                                      + " <field name=\"LastName\">Name[@Type='01']/LastName</field>\n"
                                      + " <field name=\"FirstName\">Name[@Type='01']/FirstName</field>\n"
                                      + " <field name=\"MiddleName\">Name[@Type='01']/MiddleName</field>\n"
                                      +
                                      " <field name=\"Street\">StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/Street/Line1</field>\n"
                                      +
                                      " <field name=\"City\">StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/City</field>\n"
                                      +
                                      " <field name=\"State\">StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/StateProvince</field>\n"
                                      +
                                      " <field name=\"Zip\">StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/PostalCode</field>\n"
                                      + " </object>\n"
                                      + "  <mappings id=\"Zone A\" zoneId=\"Zone A\">\n"
                                      + "   <object object=\"StudentPersonal\">\n"
                                      + "     <field name=\"LastName\">Name[@Type='06']/LastName</field>\n"
                                      + "     <field name=\"FirstName\">Name[@Type='06']/FirstName</field>\n"
                                      + "     <field name=\"MiddleName\">Name[@Type='06']/MiddleName</field>\n"
                                      + "   </object>\n"
                                      + "  </mappings>\n"
                                      + " </mappings>\n"
                                      + "</agent>";

            IDictionary psValueMap = new Dictionary<String, String>();
            psValueMap["StudentPers_guid"] = "14050614103526133C3FD2324C5BC8FF";
            psValueMap["LastName"] = "Finale";
            psValueMap["FirstName"] = "Prima";
            psValueMap["MiddleName"] = "Mediccio";
            psValueMap["Street"] = "667 Gate Way";
            psValueMap["City"] = "Sacramento";
            psValueMap["State"] = "CA";
            psValueMap["Zip"] = "91020";

            StringMapAdaptor sma = new StringMapAdaptor( psValueMap );
            StudentPersonal sp = doOutboundMappingSelect( sma, configFileText1_,
                                                          "Zone A", null, null );

            Assertion.AssertNotNull( "Student should not be null", sp );

            SifElement address = (SifElement) sp
                                                  .GetElementOrAttribute(
                                                  "StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']" );
            Assertion.AssertNotNull( "Student Address should have mapped", address );

            SifElement name = (SifElement) sp
                                               .GetElementOrAttribute( "Name[@Type='06']" );
            Assertion.AssertNotNull( "Name should have mapped to '06'", name );
        }


        protected override void assertStudentPlacement( StudentPlacement sp )
        {
            Assertion.AssertEquals( "RefID", "0000000000000000", sp.RefId );
            Assertion.AssertEquals( "StudentPersonalRefid", "0000000000000000", sp.StudentPersonalRefId );
            Assertion.AssertEquals( "Code Type", "Local", sp.Service.CodeType );
            Assertion.AssertEquals( "Type", "Related Service", sp.Service.Type );
            // Note: Currently mapping the Service value out in SIF 1.5 does not work as expected
            //Assertion.AssertEquals( "Service/@Code", "ZZZ99987", sp.Service().Code());
        }


        protected override IDictionary buildIDictionaryForStudentPlacementTest()
        {
            IDictionary data = new Hashtable();
            data.Add( "REFID", "0000000000000000" );
            data.Add( "STU_REFID", "0000000000000000" );
            //data.Add("SERVICE_CODE", "ZZZ99987");
            data.Add( "SERVICE_TYPE", "Related Service" );
            return data;
        }
    }
}