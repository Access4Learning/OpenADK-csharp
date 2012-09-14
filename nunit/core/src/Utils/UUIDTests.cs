using System;
using OpenADK.Library;
using NUnit.Framework;

namespace OpenADK.Utils
{
    [TestFixture]
    public class UUIDTests
    {
        public void setUp()
        {
            Adk.Initialize();
        }


        [Test]
        public void testAssertSIFGUIDFormat()
        {
            String refId = Adk.MakeGuid();
            Console.WriteLine(refId);
            assertRefId(refId);
        }

        [Test]
        public void testConvertUUIDToRefId()
        {
            String str = "f81d4fae-7dec-11d0-a765-00a0c91e6bf6";
            Guid guid = new Guid(str);
            String adkGuid = SifFormatter.GuidToSifRefID(guid);
            assertRefId(adkGuid);
            Assert.AreEqual("F81D4FAE7DEC11D0A76500A0C91E6BF6", adkGuid, "match");
        }

        [Test]
        public void testConvertRefIdtoUUID()
        {
            String adkGuid = "F81D4FAE7DEC11D0A76500A0C91E6BF6";
            Guid? guid = SifFormatter.SifRefIDToGuid(adkGuid);
            Assert.AreEqual(new Guid("f81d4fae-7dec-11d0-a765-00a0c91e6bf6"), guid.Value, "match");
        }

        /// <summary>
        /// Asserts that the refId is in the proper format
        /// </summary>
        /// <param name="refId"></param>
        private void assertRefId(String refId)
        {
            Assert.AreEqual(32, refId.Length, "Length");

            int pos = refId.IndexOf("-");
            Assert.AreEqual(-1, pos, "Dashes");

            // Assert case
            Assert.AreEqual(refId, refId.ToUpperInvariant(), "Case Compare");
        }
    }
}