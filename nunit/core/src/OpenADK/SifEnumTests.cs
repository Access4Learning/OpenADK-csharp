using System;
using OpenADK.Library.Global;
using OpenADK.Library.us.Common;
using NUnit.Framework;

namespace Library.Nunit.Core
{
    /// <summary>
    /// Summary description for EnumTests.
    /// </summary>
    public class SifEnumTests
    {
        /**
	 * Test that overriding equals works as expected
	 */

        public void testEqualsOverride()
        {
            String assertedValue = "FOO";

            AddressType testEnum = AddressType.Wrap(assertedValue);
            // objects should be reflexively equal to themselves
            Assert.IsTrue(testEnum.Equals(testEnum), "Reflexive comparison should pass");

            // test with null comparison
            Assert.IsFalse(testEnum.Equals(null), "Null comparison should fail");

            AddressType testEnum2 = AddressType.Wrap(assertedValue);
            Assert.IsTrue(testEnum.Equals(testEnum2));
            Assert.IsTrue(testEnum2.Equals(testEnum));

            // Test with a different enum value
            AddressType testEnum3 = AddressType.MAILING;
            Assert.IsFalse(testEnum3.Equals(testEnum));
            Assert.IsFalse(testEnum.Equals(testEnum3));

            // Test with a different enum type, but same value
            EmailType differentEnum = EmailType.Wrap(assertedValue);
            Assert.IsFalse(differentEnum.Equals(testEnum));
            Assert.IsFalse(testEnum.Equals(differentEnum));

            // Test with two null values
            AddressType nullEnum = AddressType.Wrap(null);
            AddressType nullEnum2 = AddressType.Wrap(null);
            Assert.IsTrue(nullEnum.Equals(nullEnum2), "Two Enums with null values should match");
            Assert.IsFalse(nullEnum.Equals(testEnum));
            Assert.IsFalse(testEnum.Equals(null));
        }

        /**
		 *  Test that overriding hashCode works as expected
		 */

        public void testHashCodeOverride()
        {
            String assertedValue = "FOO";

            AddressType testEnum = AddressType.Wrap(assertedValue);
            AddressType testEnum2 = AddressType.Wrap(assertedValue);
            Assert.AreEqual(testEnum.GetHashCode(), testEnum2.GetHashCode(), "hashcodes should match");
        }
    }
}