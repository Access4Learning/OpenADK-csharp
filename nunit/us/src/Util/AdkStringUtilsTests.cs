using System;
using NUnit.Framework;

namespace OpenADK.Util
{
    /// <summary>
    /// Summary description for AdkStringUtilsTests.
    /// </summary>
    [TestFixture]
    public class AdkStringUtilsTests
    {
        [Test]
        public void EncodeXML()
        {
            String xml = null;
            Assert.IsNull(AdkStringUtils.EncodeXml(xml), "String should be null");

            xml = "<Hello>";
            Assert.AreEqual("&lt;Hello&gt;", AdkStringUtils.EncodeXml(xml));
        }

        [Test]
        public void UnencodeXML()
        {
            String xml = null;
            Assert.IsNull(AdkStringUtils.UnencodeXml(xml), "String should be null");

            xml = "&lt;Hello&gt;";
            Assert.AreEqual("<Hello>", AdkStringUtils.UnencodeXml(xml));
        }

        [Test]
        public void ReplaceFirstTests()
        {
            AssertReplaceFirst("Select??", "?", " Hello", "Select Hello?");
            AssertReplaceFirst("Select[?]", "[?]", " Hello", "Select Hello");
            AssertReplaceFirst("Hel?lo", "?", "-", "Hel-lo");
            AssertReplaceFirst("Hel[?]lo", "[?]", "-", "Hel-lo");
            AssertReplaceFirst("?Hel?lo", "?", "-", "-Hel?lo");
            AssertReplaceFirst("[?]Hel[?]lo", "[?]", "-", "-Hel[?]lo");
            AssertReplaceFirst("Hello[?][?]", "[?]", "-", "Hello-[?]");
        }

        private void AssertReplaceFirst(string source, string search, string replace, string expectedResult)
        {
            string result = AdkStringUtils.ReplaceFirst(source, search, replace);
            Assert.AreEqual(expectedResult, result, "String Replace Failed");
        }
    }
}