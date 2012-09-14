using System;
using System.Xml.XPath;
using OpenADK.Library;
using OpenADK.Library.Infra;
using OpenADK.Library.Tools.XPath;
using NUnit.Framework;
using OpenADK.Library.us;

namespace Library.Nunit.Core.Tools.XPath
{
    [TestFixture]
    public class SifXPathContextTests
    {
        [SetUp]
        public void SetUp()
        {
            Adk.Initialize(SifVersion.LATEST, SIFVariant.SIF_US, (int)SdoLibraryType.Infra);
        }


        [Test]
        public void testGetValue()
        {
            SIF_ZoneStatus zoneStatus = createZoneStatus();
            SifXPathContext context = SifXPathContext.NewSIFContext(zoneStatus, SifVersion.SIF20);
            object value =
                context.GetValue(
                    "SIF_Providers/SIF_Provider[SIF_ObjectList/SIF_Object[@ObjectName='SchoolInfo']]/@SourceId");
            Assert.AreEqual("AcmeAgent", value.ToString(), "Value");
        }

        [Test]
        public void testGetValueSubstring()
        {
            SIF_ZoneStatus zoneStatus = createZoneStatus();
            Console.WriteLine(zoneStatus.ToXml());

            SifXPathContext context = SifXPathContext.NewSIFContext(zoneStatus, SifVersion.SIF20);
            Object value =
                context.GetValue(
                    "substring(SIF_Providers/SIF_Provider[SIF_ObjectList/SIF_Object[@ObjectName='SchoolInfo']]/@SourceId, 5)");
            Assert.AreEqual("Agent", value, "Value");
        }

        [Test]
        public void testIterate()
        {
            SIF_ZoneStatus zoneStatus = createZoneStatus();
            SifXPathContext context = SifXPathContext.NewSIFContext(zoneStatus, SifVersion.SIF20);
            // Select all of the objects that are provided in this zone
            XPathNodeIterator iterator = context.Select("//SIF_Provider/*/SIF_Object");
            int a = 0;
            foreach (XPathNavigator o in iterator)
            {
                Console.WriteLine(o.UnderlyingObject);
                a++;
            }
            Assert.AreEqual(5, a, "Should have iterated 5 objects");
        }

        [Test]
        public void testSelectSingleNode()
        {
            SIF_ZoneStatus zoneStatus = createZoneStatus();
            SifXPathContext context = SifXPathContext.NewSIFContext(zoneStatus, SifVersion.SIF20);
            // Select a single object provider
            XPathNodeIterator iterator =
                context.Select("SIF_Providers/SIF_Provider[SIF_ObjectList/SIF_Object[@ObjectName='StudentPersonal']]");

            Assert.AreEqual(1, iterator.Count);
            Assert.IsTrue(iterator.MoveNext());

            Element node = (Element) iterator.Current.UnderlyingObject;
            Assert.IsNotNull(node);
        }

        [Test]
        public void testSelectNodes()
        {
            SIF_ZoneStatus zoneStatus = createZoneStatus();
            SifXPathContext context = SifXPathContext.NewSIFContext(zoneStatus, SifVersion.SIF20);
            // Select all of the objects that are provided in this zone
            XPathNodeIterator iterator = context.Select("//SIF_Provider/*/SIF_Object");
            Assert.AreEqual(5, iterator.Count, "Should be 5 objects selected");
        }


        [Test]
        public void testCustomFunction()
        {
            SIF_ZoneStatus zoneStatus = createZoneStatus();
            SifXPathContext context = SifXPathContext.NewSIFContext(zoneStatus, SifVersion.SIF20);
            object value =
                context.GetValue(
                    "adk:toLowerCase(SIF_Providers/SIF_Provider[SIF_ObjectList/SIF_Object[@ObjectName='SchoolInfo']]/@SourceId)");
            Assert.AreEqual("acmeagent", value, "Value");
        }


        private SIF_ZoneStatus createZoneStatus()
        {
            SIF_ZoneStatus zoneStatus = new SIF_ZoneStatus();
            SIF_Providers providers = new SIF_Providers();
            zoneStatus.SIF_Providers = providers;

            SIF_Provider provider = new SIF_Provider();
            provider.SourceId = "SPProvider";
            provider.SIF_ObjectList = new SIF_ObjectList(new SIF_Object("StudentPersonal"));
            providers.Add(provider);


            provider = new SIF_Provider();
            provider.SourceId = "AcmeAgent";
            SIF_ObjectList objects = new SIF_ObjectList();
            objects.Add(new SIF_Object("Authentication"));
            objects.Add(new SIF_Object("SchoolInfo"));
            objects.Add(new SIF_Object("Acme"));
            objects.Add(new SIF_Object("SIF_AgentACL"));
            provider.SIF_ObjectList = objects;
            providers.Add(provider);

            return zoneStatus;
        }
    }
}