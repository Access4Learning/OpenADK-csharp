using System.Collections;
using OpenADK.Library;
using OpenADK.Library.us.Programs;
using NUnit.Framework;
using Library.Nunit.US.Tools.Mapping;

namespace Library.Nunit.US.Library.Tools.Mapping
{
    [TestFixture]
    public class SIF20MappingTests : MappingTests
    {
        public SIF20MappingTests() :
            base( SifVersion.SIF20,
                  "..\\..\\Library\\Tools\\Mapping\\SIF2.0.agent.cfg")
        {
        }


        protected override void assertStudentPlacement( StudentPlacement sp )
        {
            Assertion.AssertEquals( "RefID", "0000000000000000", sp.RefId );
            Assertion.AssertEquals( "StudentPersonalRefid", "0000000000000000", sp.StudentPersonalRefId );
            Assertion.AssertEquals( "Service/@Code", "ZZZ99987", sp.Service.Code );
        }


        protected override IDictionary buildIDictionaryForStudentPlacementTest()
        {
            IDictionary data = new Hashtable();
            data.Add( "REFID", "0000000000000000" );
            data.Add( "STU_REFID", "0000000000000000" );
            data.Add( "SERVICE_CODE", "ZZZ99987" );
            return data;
        }
    }
}