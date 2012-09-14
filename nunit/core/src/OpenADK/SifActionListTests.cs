using OpenADK.Library;
using OpenADK.Library.us.Common;
using NUnit.Framework;
using OpenADK.Library.us;

namespace Library.Nunit.Core
{
    [TestFixture]
    public class SifActionListTests
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
            Email email2 = new Email(EmailType.Wrap("bar"), "email2@OpenADK.com");

            el.Add(email1);
            Assert.AreEqual(1, el.ChildCount, "Should have 1 email");

            el.Add(email2);
            Assert.AreEqual(2, el.ChildCount, "Should have 2 emails");

            Email[] children = el.ToArray();
            Assert.AreEqual(2, children.Length, "Should have 2 array elements");


            Assert.IsTrue( el.Remove( EmailType.Wrap("foo") ), "Should have removed the email") ;
            Assert.AreEqual(1, el.ChildCount, "Should have 1 email");

            el.RemoveChild(CommonDTD.EMAILLIST_EMAIL, email2.Key);
            Assert.AreEqual(0, el.ChildCount, "Should have 0 emails");
        }

        [Test]
        public void testList020()
        {
            EmailList el = new EmailList();

            // Using the generic "Wrap" API so that we can use this
            // test against any internationalized version of the ADK
            Email email1 = new Email( EmailType.Wrap( "asdfasdf" ), "email1@OpenADK.com" );
            Email email2 = new Email( EmailType.Wrap( "Primary" ), "email2@OpenADK.com" );

            

            el.Add(email1);
            Assert.AreEqual(1, el.ChildCount, "Should have 1 email");

            el.Add(email2);
            Assert.AreEqual(2, el.ChildCount, "Should have 2 emails");


            Email email3 = new Email();
            email3.Type = "Alternate1";
            el.Add(email3);
            Assert.AreEqual(3, el.ChildCount, "Should have 3 emails");



            Email primary = el[EmailType.PRIMARY];
            Assert.IsNotNull( primary );

            primary = el["Primary"];
            Assert.IsNotNull(primary);


            Email secondary = el[EmailType.ALT1];
            Assert.IsNotNull(secondary);

            secondary = el["Alternate1"];
            Assert.IsNotNull(secondary);

        }
    }
}