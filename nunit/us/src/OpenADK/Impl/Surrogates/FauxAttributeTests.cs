using System.Xml.XPath;
using OpenADK.Library;
using OpenADK.Library.Impl.Surrogates;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.XPath;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Library.Impl.Surrogates
{
    [TestFixture]
    public class FauxAttributeTests : AdkTest
    {
        [Test]
        public void testFauxAttribute010()
        {

            SifElementPointer sep = new SifElementPointer(null, new StudentPersonal(), SifVersion.SIF15r1);
            FauxAttribute fa = new FauxAttribute(sep, "Type", "Projected");


            // Assert base functionality
            Assert.AreEqual( "Type", fa.Name );
            Assert.AreEqual("Projected", fa.Value);
            Assert.AreEqual(sep, fa.Parent );

            // Assert XPath functionality
            Assert.AreEqual( XPathNodeType.Attribute, fa.NodeType );


        }
    }
}
