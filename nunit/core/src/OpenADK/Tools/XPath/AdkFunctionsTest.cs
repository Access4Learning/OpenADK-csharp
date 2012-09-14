using OpenADK.Library.Tools.XPath;
using NUnit.Framework;

namespace Library.Nunit.Core.Tools.XPath
{
    [TestFixture]
    public class AdkFunctionsTest
    {
        [Test]
        public void testToUpper()
        {
            Assertion.AssertEquals("ABC DEFG", AdkFunctions.toUpperCase("abc defg"));
        }

        [Test]
        public void testToLower()
        {
            Assertion.AssertEquals("abc defg", AdkFunctions.toLowerCase("aBc dEfg"));
        }

        [Test]
        public void testPad()
        {
            Assertion.AssertEquals("Hello World", AdkFunctions.padEnd("Hello World", "*", 11));
            Assertion.AssertEquals("Hello World", AdkFunctions.padEnd("Hello World", "*", 5));
            Assertion.AssertEquals("Hello World****", AdkFunctions.padEnd("Hello World", "*", 15));

            Assertion.AssertEquals("Hello World", AdkFunctions.padBegin("Hello World", "*", 11));
            Assertion.AssertEquals("Hello World", AdkFunctions.padBegin("Hello World", "*", 5));
            Assertion.AssertEquals("****Hello World", AdkFunctions.padBegin("Hello World", "*", 15));
        }

        [Test]
        public void testToProperCase()
        {
            Assertion.AssertEquals("Hello World", AdkFunctions.toProperCase("hello World"));
            Assertion.AssertEquals("Hello O'Reilly", AdkFunctions.toProperCase("HELLO o'reilly"));
            Assertion.AssertEquals("Old O'Leary", AdkFunctions.toProperCase("old o'leary"));
            Assertion.AssertEquals("Old\tO'Leary", AdkFunctions.toProperCase("old\to'LEARY"));
        }
    }
}