using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenADK.Library;

namespace Library.NUnit.Core
{
    [TestFixture]
    public class AdkPropertiesTests
    {
        public void TestReturnDefaultValue()
        {
            AdkProperties props = new AdkProperties( null );
            props["TEST1"] = "Value1";

            Assert.AreEqual( "Value1", props["TEST1"] );
            Assert.AreEqual("Value1", props.GetProperty( "TEST1" ));
            Assert.AreEqual("Value1", props.GetProperty( "TEST1", "foo" ));
            Assert.AreEqual("foo", props.GetProperty( "bar", "foo" ));

        }

        public void TestReturnDefaultValueWithInheritance()
        {
            AdkProperties parent = new AdkProperties(null);
            parent["TEST1"] = "Value1";

            AdkProperties props = new AdkProperties( parent );
            props["TEST2"] = "Value2";

            Assert.AreEqual("Value1", props["TEST1"]);
            Assert.AreEqual("Value2", props["TEST2"]);
            Assert.AreEqual("Value1", props.GetProperty("TEST1"));
            Assert.AreEqual("Value1", props.GetProperty("TEST1", "foo"));
            Assert.AreEqual("foo", props.GetProperty("bar", "foo"));

            props["TEST1"] = "scooter";
            Assert.AreEqual("scooter", props["TEST1"]);

        }

    }
}
