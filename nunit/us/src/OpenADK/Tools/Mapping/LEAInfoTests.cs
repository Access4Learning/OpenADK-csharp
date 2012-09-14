using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Edustructures.SifWorks;
using Edustructures.SifWorks.Common;
using Edustructures.SifWorks.Student;
using Edustructures.SifWorks.Tools.Cfg;
using Edustructures.SifWorks.Tools.Mapping;
using NUnit.Framework;
using System.Collections;

namespace SIFWorks.Nunit.US.SifWorks.Student
{
    [TestFixture]
    public class LEAInfoTests
    {

        [SetUp]
        public void setUp()
        {
            Adk.Initialize(SifVersion.SIF15r1, SIFVariant.SIF_US, SdoLibraryType.Student);
        }

        public void testLeaInfoParseFrom15r1() {
		Adk.SifVersion = SifVersion.SIF15r1;
		String leaInfoXML = "	<LEAInfo RefId='1234' xmlns='http://www.sifinfo.org/infrastructure/1.x'>"
				+ "    <LocalId>1234</LocalId>"
				+ "    <StatePrId>4567</StatePrId>"
				+ "    <LEAName>Tom District</LEAName>"
				+ "    <PhoneNumber Format='NA' Type='TE'>814.455.4658</PhoneNumber>"
				+ "    <Address Type='07'>"
				+ "      <Street>"
				+ "        <Line1>1232 Bateman Point Drive</Line1>"
				+ "        <Line2></Line2>"
				+ "        <Line3></Line3>"
				+ "      </Street>"
				+ "      <City>West Jordan</City>"
				+ "      <StatePr Code='Utah' />"
				+ "      <PostalCode>84084</PostalCode>"
				+ "    </Address>"
				+ "    <LEAContact>"
				+ "      <ContactInfo>"
				+ "        <Name Type='04'>"
				+ "          <LastName>Ngo</LastName>"
				+ "          <FirstName>Tom</FirstName>"
				+ "          <MiddleName>C.</MiddleName>"
				+ "        </Name>"
				+ "        <PositionTitle>Principal</PositionTitle>"
				+ "        <PhoneNumber Format='NA' Type='TE'></PhoneNumber>"
				+ "        <Email Type='Primary'>tngo@edustructures.com</Email>"
				+ "      </ContactInfo>" + "    </LEAContact>" + "  </LEAInfo>";

        String agentCFG = "<agent id='Repro' sifVersion='2.0'>"
                        + "   <mappings id='Default'>"
                        + "     <object object='LEAInfo'>"
                        + "     <field name='LOCALID'>LocalId</field>"
                        + "     <field name='STATEPRID' sifVersion='+2.0'>StateProvinceId</field>"
                        + "     <field name='STATEPRID' sifVersion='-1.5r1'>StatePrId</field>"
                        + "     <field name='NAME'>LEAName</field>"
                        + "     <field name='DISTRICT_PHONE' sifVersion='+2.0'>PhonenumberList/PhoneNumber[@Type='0096']/Number</field>"
                        + "     <field name='DISTRICT_PHONE' sifVersion='-1.5r1'>PhoneNumber[@Format='NA',@Type='TE']</field>"
                        + "     <field name='DISTRICT_ADDR1' sifVersion='+2.0'>AddressList/Address[@Type='2382']/Street/Line1</field>"
                        + "     <field name='DISTRICT_ADDR1' sifVersion='-1.5r1'>Address[@Type='07']/Street/Line1</field>"
                        + "     <field name='DISTRICT_ADDR2' sifVersion='+2.0'>AddressList/Address[@Type='2382']/Street/Line2</field>"
                        + "     <field name='DISTRICT_ADDR2' sifVersion='-1.5r1'>Address[@Type='07']/Street/Line2</field>"
                        + "     <field name='DISTRICT_ADDR3' sifVersion='+2.0'>AddressList/Address[@Type='2382']/Street/Line3</field>"
                        + "     <field name='DISTRICT_ADDR3' sifVersion='-1.5r1'>Address[@Type='07']/Street/Line3</field>"
                        + "     <field name='DISTRICT_CITY' sifVersion='+2.0'>AddressList/Address[@Type='0123']/City</field>"
                        + "     <field name='DISTRICT_CITY' sifVersion='-1.5r1'>Address[@Type='07']/City</field>"
                        + "     <field name='DISTRICT_STATE' sifVersion='+2.0'>AddressList/Address[@Type='0123']/StateProvince</field>"
                        + "     <field name='DISTRICT_STATE' sifVersion='-1.5r1'>Address[@Type='07']/StatePr/@Code</field>"
                        + "     <field name='DISTRICT_COUNTRY' sifVersion='+2.0'>AddressList/Address[@Type='0123']/Country=US</field>"
                        + "     <field name='DISTRICT_COUNTRY' sifVersion='-1.5r1'>Address[@Type='07']/Country[@Code='US']</field>"
                        + "     <field name='DISTRICT_ZIPCODE' sifVersion='+2.0'>AddressList/Address[@Type='0123']/PostalCode</field>"
                        + "     <field name='DISTRICT_ZIPCODE' sifVersion='-1.5r1'>Address[@Type='07']/PostalCode</field>"
                        + "     <field name='CONTACT_POSITION' sifVersion='+2.0'>LEAContactList/LEAContact/ContactInfo/PositionTitle</field>"
                        + "     <field name='CONTACT_POSITION' sifVersion='-1.5r1'>LEAContact/ContactInfo/PositionTitle</field>"
                        + "     <field name='CONTACT_PHONE' sifVersion='+2.0'>LEAContactList/LEAContact/ContactInfo/PhonenumberList/PhoneNumber[@Type='0096']/Number</field>"
                        + "     <field name='CONTACT_PHONE' sifVersion='-1.5r1'>LEAContact/ContactInfo/PhoneNumber[@Format='NA',@Type='TE']</field>"
                        + "     <field name='CONTACT_EMAIL' sifVersion='+2.0'>LEAContactList/LEAContact/ContactInfo/EmailList/Email[@Type='Primary']</field>"
                        + "     <field name='CONTACT_EMAIL' sifVersion='-1.5r1'>LEAContact/ContactInfo/Email[@Type='Primary']</field>"
                        + "     <field name='CONTACT_FIRSTNAME' sifVersion='+2.0'>LEAContactList/LEAContact/ContactInfo/Name[@Type='04']/FirstName</field>"
                        + "     <field name='CONTACT_FIRSTNAME' sifVersion='-1.5r1'>LEAContact/ContactInfo/Name[@Type='04']/FirstName</field>"
                        + "     <field name='CONTACT_MIDDLENAME' sifVersion='+2.0'>LEAContactList/LEAContact/ContactInfo/Name[@Type='04']/MiddleName</field>"
                        + "     <field name='CONTACT_MIDDLENAME' sifVersion='-1.5r1'>LEAContact/ContactInfo/Name[@Type='04']/MiddleName</field>"
                        + "     <field name='CONTACT_LASTNAME' sifVersion='+2.0'>LEAContactList/LEAContact/ContactInfo/Name[@Type='04']/LastName</field>"
                        + "     <field name='CONTACT_LASTNAME' sifVersion='-1.5r1'>LEAContact/ContactInfo/Name[@Type='04']/LastName</field>"
                        + "</object></mappings></agent>";


		SifParser p = SifParser.NewInstance();
		LEAInfo leaObject = (LEAInfo) p.Parse(leaInfoXML, null, 0, SifVersion.SIF15r1);

		PhoneNumber phone = leaObject.PhoneNumberList.ItemAt( 0 );
		Assertion.AssertEquals( "Format", "NA", phone.Format);
		Assertion.AssertEquals("Type", "TE", phone.Type);
		Assertion.AssertEquals("District Phone", "814.455.4658", phone.Number);

            phone = leaObject.LEAContactList.ItemAt( 0 ).ContactInfo.PhoneNumberList.ItemAt( 0 );
		Assertion.AssertEquals("Format", "NA", phone.Format);
		Assertion.AssertEquals("Type", "TE", phone.Type);
		Assertion.AssertEquals("Contact Phone", "", phone.Number);

		AgentConfig cfg = createConfig(agentCFG);
		Mappings m = cfg.Mappings.GetMappings("Default").Select(null, null, null);
		IDictionary target = new Hashtable();
		m.MapInbound(leaObject, new StringMapAdaptor(target));

		Console.WriteLine( leaObject.SifVersion );

		Assertion.AssertEquals("District Phone", "814.455.4658", target["DISTRICT_PHONE"]);
		Assertion.AssertEquals("District Phone", "", target["CONTACT_PHONE"]);

	}

        private AgentConfig createConfig(String cfg)
        {
            String fileName = "AdvancedMappings.cfg";
            writeConfig(cfg, fileName);
            AgentConfig config = new AgentConfig();
            config.Read(fileName, false);
            return config;
        }

        private void writeConfig(String configFileText_in, String fileName)
        {
            using( StreamWriter writer = new StreamWriter( fileName, false, Encoding.UTF8 ))
            {
                writer.Write( configFileText_in );
                writer.Close();
            }
        }
    }
}
