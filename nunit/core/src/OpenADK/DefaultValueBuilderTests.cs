using OpenADK.Library;
using NUnit.Framework;

namespace Library.Nunit.Core
{
    /// <summary>
    /// Summary description for DefaultValueBuilderTests.
    /// </summary>
    [TestFixture]
    public class DefaultValueBuilderTests
    {
        [Test]
        public void ParseResultsSimple()
        {
            string starter = "@random() @strip(\"(801) 323-1131\")";
            int pos = 10;

            DefaultValueBuilder.ParseResults results = DefaultValueBuilder.ParseResults.parse(starter, pos);

            Assert.AreEqual(34, results.Position, "Position");
            Assert.AreEqual("strip", results.MethodName, "MethodName");
            Assert.AreEqual(1, results.Parameters.TokenCount, "Token Count");
            Assert.AreEqual("(801) 323-1131", results.Parameters.GetToken(0), "Parameter");
        }

        [Test]
        public void parseSyncMacro()
        {
            string starter = "@syncmatchname(first,middle,last)";
            int pos = 0;

            DefaultValueBuilder.ParseResults results = DefaultValueBuilder.ParseResults.parse(starter, pos);

            Assert.AreEqual("syncmatchname", results.MethodName, "MethodName");
            Assert.AreEqual(3, results.Parameters.TokenCount, "Token Count");
            Assert.AreEqual("first", results.Parameters.GetToken(0), "Parameter");
            Assert.AreEqual("middle", results.Parameters.GetToken(1), "Parameter");
            Assert.AreEqual("last", results.Parameters.GetToken(2), "Parameter");
        }
    }
}