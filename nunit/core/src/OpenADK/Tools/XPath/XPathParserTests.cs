using System;
using OpenADK.Library.Tools.XPath.Compiler;
using NUnit.Framework;

namespace Library.Nunit.Core.Tools.XPath
{
    [TestFixture]
    public class XPathParserTests
    {
        [Test]
        public void TestXPath010()
        {
            AdkXPathStep[] steps = XPathParser.Parse("Foo");

            Assert.AreEqual(1, steps.Length);
            Assert.AreEqual("Foo", ((AdkNodeNameTest) steps[0].NodeTest).NodeName);

            steps = XPathParser.Parse("/Foo");
            Assert.AreEqual(1, steps.Length);
            Assert.AreEqual("Foo", ((AdkNodeNameTest) steps[0].NodeTest).NodeName);

            Console.WriteLine(steps[0]);
        }

        [Test]
        public void TestXPath020()
        {
            AdkXPathStep[] steps = XPathParser.Parse("Foo/Bar/Win");

            Assert.AreEqual(3, steps.Length);
            Assert.AreEqual("Foo", ((AdkNodeNameTest) steps[0].NodeTest).NodeName);
            Assert.AreEqual("Bar", ((AdkNodeNameTest) steps[1].NodeTest).NodeName);
            Assert.AreEqual("Win", ((AdkNodeNameTest) steps[2].NodeTest).NodeName);

            steps = XPathParser.Parse("/Foo/Bar/Win");
            Assert.AreEqual(3, steps.Length);
            Assert.AreEqual("Foo", ((AdkNodeNameTest) steps[0].NodeTest).NodeName);
            Assert.AreEqual("Bar", ((AdkNodeNameTest) steps[1].NodeTest).NodeName);
            Assert.AreEqual("Win", ((AdkNodeNameTest) steps[2].NodeTest).NodeName);
        }

        [Test]
        public void TestXPath030()
        {
            String path = "Foo[@Win='2']/Bar[Type=5]/Win[Zone=\"yada\"]";
            AdkXPathStep[] steps = XPathParser.Parse(path);

            Assert.AreEqual(3, steps.Length);
            AssertStep(steps[0], "Foo", "Win", "2");
            AssertStep(steps[1], "Bar", "Type", 5);
            AssertStep(steps[2], "Win", "Zone", "yada");

            Console.WriteLine(steps[0] + "/" + steps[1] + "/" + steps[2]);


            steps = XPathParser.Parse("/" + path);
            Assert.AreEqual(3, steps.Length);
            AssertStep(steps[0], "Foo", "Win", "2");
            AssertStep(steps[1], "Bar", "Type", 5);
            AssertStep(steps[2], "Win", "Zone", "yada");
        }


        [Test]
        public void TestXPath040()
        {
            String path = "Foo[@Win='2']/Bar";
            AdkXPathStep[] steps = XPathParser.Parse(path);

            Assert.AreEqual(2, steps.Length);
            AssertStep(steps[0], "Foo", "Win", "2");
            Assert.AreEqual("Bar", ((AdkNodeNameTest) steps[1].NodeTest).NodeName);


            steps = XPathParser.Parse("/" + path);
            Assert.AreEqual(2, steps.Length);
            AssertStep(steps[0], "Foo", "Win", "2");
            Assert.AreEqual("Bar", ((AdkNodeNameTest) steps[1].NodeTest).NodeName);
        }

        private void AssertStep(AdkXPathStep step, String name, String singePredicateName, object singlePredicateValue)
        {
            Assert.AreEqual(name, ((AdkNodeNameTest) step.NodeTest).NodeName);
            Assert.IsNotNull(step.Predicates);
            Assert.AreEqual(1, step.Predicates.Length);
            Assert.IsInstanceOfType(typeof (AdkEqualOperation), step.Predicates[0]);

            AdkExpression[] components = ((AdkEqualOperation) step.Predicates[0]).Arguments;
            Assert.AreEqual(2, components.Length);
            Assert.IsInstanceOfType(typeof (AdkLocPath), components[0]);
            AdkLocPath lp = (AdkLocPath) components[0];

            AdkNodeNameTest attrName = (AdkNodeNameTest) lp.Steps[0].NodeTest;
            Assert.AreEqual(singePredicateName, attrName.NodeName);

            object value = components[1].ComputeValue(null);
            Assert.AreEqual(singlePredicateValue, value);
        }
    }
}