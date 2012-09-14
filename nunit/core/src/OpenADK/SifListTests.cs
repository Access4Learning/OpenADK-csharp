using OpenADK.Library;
using OpenADK.Library.us.Common;
using NUnit.Framework;
using OpenADK.Library.us;

namespace Library.Nunit.Core
{
    [TestFixture]
    public class SifListTests
    {
        [SetUp]
        public void Setup()
        {
            Adk.Initialize(SifVersion.LATEST, SIFVariant.SIF_US, (int)SdoLibraryType.None);
        }

        [Test]
        public void testList010()
        {
            EmailList el = new EmailList();

            // Using the generic "Wrap" API so that we can use this
            // test against any internationalized version of the ADK
            Email email1 = new Email(EmailType.Wrap("foo"), "email1@OpenADK.com");
            Email email2 = new Email(EmailType.Wrap("foo"), "email2@OpenADK.com");

            el.Add(email1);
            Assert.AreEqual(1, el.ChildCount, "Should have 1 email");

            el.Add(email2);
            Assert.AreEqual(2, el.ChildCount, "Should have 2 emails");

            Email[] children = el.ToArray();
            Assert.AreEqual(2, children.Length, "Should have 2 array elements");

            el.RemoveChild(email2);
            Assert.AreEqual(1, el.ChildCount, "Should have 1 email");

            el.RemoveChild(email1);
            Assert.AreEqual(0, el.ChildCount, "Should have 0 emails");

            children = el.ToArray();
            Assert.AreEqual(0, children.Length, "Should have 0 array elements");
        }


        [Test]
        public void testList020()
        {
            EmailList el = new EmailList();

            // Using the generic "Wrap" API so that we can use this
            // test against any internationalized version of the ADK
            Email email1 = new Email(EmailType.Wrap("foo"), "email1@OpenADK.com");
            Email email2 = new Email(EmailType.Wrap("foo"), "email2@OpenADK.com");


            el.Add(email1);
            el.Add(email2);

            Assert.IsNotNull(email1.Parent, "Parent should not be null");
            Assert.IsNotNull(email2.Parent, "Parent should not be null");

            el.Clear();
            Assert.AreEqual(0, el.ChildCount, "Should have 0 emails");
            Assert.IsNull(email1.Parent, "Parent should be null");
            Assert.IsNull(email2.Parent, "Parent should be null");
        }

        [Test]
        public void testList030()
        {
            EmailList el = new EmailList();

            // Using the generic "Wrap" API so that we can use this
            // test against any internationalized version of the ADK
            Email email1 = new Email(EmailType.Wrap("foo"), "email1@OpenADK.com");
            Email email2 = new Email(EmailType.Wrap("bar"), "email2@OpenADK.com");

            el.Add(email1);
            el.Add(email2);

            // test the iterator
            int count = 0;
            foreach (Email e in el)
            {
                Assert.IsNotNull(e, "Email should not be null");
                Assert.AreEqual(24, e.TextValue.Length, "Should have email address");
                count++;
            }

            Assert.AreEqual(2, count, "Should have iterated 2 emails");
        }
    }
}