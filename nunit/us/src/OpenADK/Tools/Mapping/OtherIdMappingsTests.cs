using System;
using System.Collections;
using System.Collections.Generic;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.Mapping;
using NUnit.Framework;

namespace Library.Nunit.US.Tools.Mapping
{
    [TestFixture]
    public class OtherIdMappingsTests : BaseMappingsTest
    {
        [Test]
        public void TestOtherIdMappings()
        {
            String otherIdMapping = "<agent id=\"Repro\" sifVersion=\"2.0\">"
                                    + "    <mappings id=\"Default\">"
                                    + "        <object object='StudentPersonal'>"
                                    +
                                    "            <field direction='inbound' name='FIELD1'><otherid type='ZZ' prefix='FIELD1:'/></field>"
                                    +
                                    "            <field direction='outbound' name='FIELD1'>OtherIdList/OtherId[@Type='ZZ'+]=FIELD1:$(FIELD1)</field>"
                                    +
                                    "            <field direction='inbound' name='FIELD2'><otherid type='ZZ' prefix='FIELD2:'/></field>"
                                    +
                                    "            <field direction='outbound' name='FIELD2'>OtherIdList/OtherId[@Type='ZZ'+]=FIELD2:$(FIELD2)</field>"
                                    + "        </object>" + "    </mappings>" + "</agent>";

            Dictionary<String, String> sourceMap = new Dictionary<String, String>();
            sourceMap.Add( "FIELD1", "1234" );
            sourceMap.Add( "FIELD2", "5678" );
            StringMapAdaptor sma = new StringMapAdaptor( sourceMap );
            StudentPersonal sp = mapToStudentPersonal( sma, otherIdMapping, null );
            Assertion.AssertNotNull( "Student should not be null", sp );

            IDictionary destinationMap = doInboundMapping( otherIdMapping, sp );
            assertMapsAreEqual( sourceMap, destinationMap );
        }
    }
}