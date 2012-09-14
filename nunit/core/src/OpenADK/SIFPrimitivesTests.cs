using System;
using OpenADK.Library;
using OpenADK.Library.Infra;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.NUnit.Core.Library
{
    [TestFixture]
    public class SIFPrimitivesTests : InMemoryProtocolTest, ISubscriber
    {
        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            // Clean up any properties that have been set and reset the ADK version
            Adk.SifVersion = SifVersion.LATEST;
            Agent.Properties.Clear();
        }

        [Test]
        public void testRegisterSIF20()
        {
            Adk.SifVersion = (SifVersion.SIF20);
            String iconURL = "http://acme.foo.bar/ico";
            AgentProperties props = Agent.Properties;
            props.AgentIconUrl = iconURL;

            Agent.Name = "acmeAgent";
            props.AgentVendor = "acmeVendor";
            props.AgentVersion = "2.6.5.8";

            props.ApplicationName = "acmeApp";
            props.ApplicationVendor = "acme<>AppVendor";
            props.ApplicationVersion = "10.2";

            Zone.Connect( ProvisioningFlags.Register );
            InMemoryProtocolHandler handler = (InMemoryProtocolHandler) Zone.ProtocolHandler;

            SIF_Register sr = (SIF_Register) handler.readMsg();

            Assert.AreEqual( Agent.Id, sr.SourceId, "SourceID" );
            Assert.AreEqual( "acmeAgent", sr.SIF_Name, "Name" );
            Assert.AreEqual( "acmeVendor", sr.SIF_NodeVendor, "Agent Vendor" );
            Assert.AreEqual( "2.6.5.8", sr.SIF_NodeVersion, "Agent Version" );
            SIF_Application appInfo = sr.SIF_Application;
            Assert.IsNotNull( appInfo );
            Assert.AreEqual( "acmeApp", appInfo.SIF_Product, "App Name" );
            Assert.AreEqual( "acme<>AppVendor", appInfo.SIF_Vendor, "App Vendor" );
            Assert.AreEqual( "10.2", appInfo.SIF_Version, "App Version" );
            Assert.AreEqual( iconURL, sr.SIF_Icon, "Icon" );
        }

        [Test]
        public void testRegisterOverrideZISVersion()
        {
            Adk.SifVersion = (SifVersion.SIF20);
            AgentProperties props = Agent.Properties;
            props.OverrideSifVersions = "1.1, 2.5";
            
            Zone.Connect(ProvisioningFlags.Register);
            InMemoryProtocolHandler handler = (InMemoryProtocolHandler)Zone.ProtocolHandler;

            SIF_Register sr = (SIF_Register)handler.readMsg();
            SIF_Version[] versions = sr.GetSIF_Versions();
            Assert.IsNotNull( versions );
            Assert.AreEqual( 2, versions.Length );
            Assert.AreEqual( "1.1", versions[0].Value );
            Assert.AreEqual("2.5", versions[1].Value);
        }


        [Test]
        public void testRegisterSIF15r1()
        {
            Adk.SifVersion = (SifVersion.SIF15r1);
            String iconURL = "http://acme.foo.bar/ico";
            AgentProperties props = Agent.Properties;
            props.AgentIconUrl = iconURL;

            Agent.Name = "acmeAgent";
            props.AgentVendor = "acmeVendor";
            props.AgentVersion = "2.6.5.8";
            props.ApplicationName = "acmeApp";
            props.ApplicationVendor = "acme<>AppVendor";
            props.ApplicationVersion = "10.2";


            Zone.Connect( ProvisioningFlags.Register );
            InMemoryProtocolHandler handler = (InMemoryProtocolHandler) Zone.ProtocolHandler;

            SIF_Register sr = (SIF_Register) handler.readMsg();

            Assert.AreEqual( Agent.Id, sr.SourceId, "SourceID" );
            Assert.AreEqual( "acmeAgent", sr.SIF_Name, "Name" );
            Assert.IsNull( sr.SIF_NodeVendor, "Agent Vendor" );
            Assert.IsNull( sr.SIF_NodeVersion, "Agent Version" );
            SIF_Application appInfo = sr.SIF_Application;
            Assert.IsNull( appInfo );
            Assert.IsNull( sr.SIF_Icon, "Icon" );


            // Assert the versions in the message. If the ADK is initialized to
            // SIF 1.5r1, it should not be sending any versions that start with a
            // "2"
            SifVersion messageVersion = sr.SifVersion;
            Assert.AreEqual( SifVersion.SIF15r1, messageVersion, "Should be version 1.5r1" );
            foreach ( SIF_Version version in sr.GetSIF_Versions() )
            {
                String versionString = version.TextValue;
                Assert.IsTrue( versionString.StartsWith( "1" ), "Should start with 1 but was " + versionString );
            }
        }

        /**
	 * 
	 */
        [Test]
        public void testSIFPingDifferentVersions()
        {
            Adk.SifVersion = (SifVersion.LATEST);
            Zone.Connect( ProvisioningFlags.None );
            InMemoryProtocolHandler handler = (InMemoryProtocolHandler) Zone.ProtocolHandler;
            handler.clear();
            Zone.SifPing();
            SIF_SystemControl ssc = (SIF_SystemControl) handler.readMsg();

            Assert.AreEqual( SifVersion.LATEST, ssc.SifVersion, "SifVersion" );
            Assert.AreEqual( SifVersion.LATEST.Xmlns, ssc.GetXmlns(), "SifVersion->Xmlns" );

            foreach ( SifVersion version in Adk.SupportedSIFVersions )
            {
                // This may seem strange, but the ADK sometimes has a SIF version in the list of 
                // supported versions that is not fully supported yet (e.g. preparing the ADK for 
                // the next version. Because of that, only test SIF_Ping with versions if they
                // are equal to or less than SifVersion.LATEST
                if ( version.CompareTo( SifVersion.LATEST ) <= 0 )
                {
                    testSIFPingWithZISVersion( handler, version );
                }
            }
        }

        /**
	 * 
	 */
        [Test]
        public void testSynchronousGetZoneStatus()
        {
            Adk.SifVersion = (SifVersion.LATEST);
            Zone.Connect( ProvisioningFlags.None );
            InMemoryProtocolHandler handler = (InMemoryProtocolHandler) Zone.ProtocolHandler;
            Zone.Properties.UseZoneStatusSystemControl = true;
            Zone.Properties.ZisVersion = SifVersion.SIF15r1.ToString();
            handler.clear();

            SIF_ZoneStatus szs = Zone.GetZoneStatus();

            SIF_SystemControl ssc = (SIF_SystemControl) handler.readMsg();

            Assert.AreEqual( SifVersion.SIF15r1, ssc.SifVersion, "SifVersion" );
            Assert.AreEqual( SifVersion.SIF15r1.Xmlns, ssc.GetXmlns(), "SifVersion->Xmlns" );
            SifElement element = ssc.SIF_SystemControlData.GetChildList()[0];
            Assert.IsNotNull( element, "SIF_SystemControlData\\Child" );
            Assert.IsTrue( element is SIF_GetZoneStatus, "is instanceof SIF_GetZoneStatus" );
        }

        /**
	 * 
	 */
        [Test]
        public void testAsynchronousGetZoneStatus()
        {
            Adk.SifVersion = (SifVersion.LATEST);
            Zone.Connect( ProvisioningFlags.None );
            InMemoryProtocolHandler handler = (InMemoryProtocolHandler) Zone.ProtocolHandler;
            Zone.Properties.UseZoneStatusSystemControl = false;
            Zone.Properties.ZisVersion = SifVersion.SIF15r1.ToString();
            handler.clear();

            try
            {
                // We expect a SIF XML Error exception in this case because
                // our handler doesn't return a valid response back to a pull message
                Zone.GetZoneStatus();
            }
            catch ( SifException sifEx )
            {
                Assert.AreEqual( SifErrorCategoryCode.Xml, sifEx.ErrorCategory );
            }

            SIF_Request sr = (SIF_Request) handler.readMsg();

            Assert.AreEqual( SifVersion.SIF15r1, sr.SifVersion, "SifVersion" );
            Assert.AreEqual( SifVersion.SIF15r1.Xmlns, sr.GetXmlns(), "SifVersion->Xmlns" );
        }


        private void testSIFPingWithZISVersion( InMemoryProtocolHandler handler, SifVersion testVersion )
        {
            SIF_SystemControl ssc;
            Zone.Properties.ZisVersion = testVersion.ToString();
            Zone.SifPing();
            ssc = (SIF_SystemControl) handler.readMsg();

            Assert.AreEqual( testVersion, ssc.SifVersion, "SifVersion" );
            Assert.AreEqual( testVersion.Xmlns, ssc.GetXmlns(), "SifVersion->Xmlns" );
        }


        /**
	 * Tests registering with the ADK version set to 2.0 or greater, but the 
	 * AgentProperties.getZISVersion() property set to 1.5r1. This should result 
	 * in the SIF_Register message being sent in 1.5r1
	 * @throws Exception
	 */
        [Test]
        public void testSIFRegisterZISVersion15r1()
        {
            Adk.SifVersion = (SifVersion.LATEST);
            String iconURL = "http://acme.foo.bar/ico";
            AgentProperties props = Agent.Properties;
            // Set the ZIS Version to 1.5r1
            props.ZisVersion = SifVersion.SIF15r1.ToString();

            props.AgentIconUrl = iconURL;

            Agent.Name = "acmeAgent";
            props.AgentVendor = "acmeVendor";
            props.AgentVersion = "2.6.5.8";
            props.ApplicationName = "acmeApp";
            props.ApplicationVendor = "acme<>AppVendor";
            props.ApplicationVersion = "10.2";


            Zone.Connect( ProvisioningFlags.Register );
            InMemoryProtocolHandler handler = (InMemoryProtocolHandler) Zone.ProtocolHandler;

            SIF_Register sr = (SIF_Register) handler.readMsg();

            Assert.AreEqual( SifVersion.SIF15r1, sr.SifVersion, "SifVersion" );
            Assert.AreEqual( SifVersion.SIF15r1.Xmlns, sr.GetXmlns(), "SifVersion->Xmlns" );

            Assert.AreEqual( Agent.Id, sr.SourceId, "SourceID" );
            Assert.AreEqual( "acmeAgent", sr.SIF_Name, "Name" );
            Assert.IsNull( sr.SIF_NodeVendor, "Agent Vendor" );
            Assert.IsNull( sr.SIF_NodeVersion, "Agent Version" );
            SIF_Application appInfo = sr.SIF_Application;
            Assert.IsNull( appInfo );
            Assert.IsNull( sr.SIF_Icon, "Icon" );


            // Assert the versions in the message. If the ADK is initialized to
            // SIF 1.5r1, it should not be sending any versions that start with a
            // "2"
            SifVersion messageVersion = sr.SifVersion;
            Assert.AreEqual( SifVersion.SIF15r1, messageVersion, "Should be version 1.5r1" );
            foreach ( SIF_Version version in sr.GetSIF_Versions() )
            {
                String versionString = version.TextValue;
                Assert.IsTrue( versionString.StartsWith( "1" ), "Should start with 1 but was " + versionString );
            }
        }

        /**
	 * 
	 */
        [Test]
        public void testProvisioningSIF20()
        {
            String[] expectedMessages =
                new String[] {"SIF_Register", "SIF_SystemControl", "SIF_SystemControl", "SIF_Provision"};
            Adk.SifVersion = (SifVersion.LATEST);
            Zone.SetSubscriber( this, InfraDTD.SIF_AGENTACL, null );
            assertMessagesInVersion( SifVersion.LATEST, expectedMessages );
        }

        /**
	 * 
	 */
        [Test]
        public void testProvisioningSIF15r1()
        {
            String[] expectedMessages = new String[] {"SIF_Register", "SIF_SystemControl", "SIF_Subscribe"};
            Adk.SifVersion = (SifVersion.SIF15r1);
            Zone.SetSubscriber(this, InfraDTD.SIF_AGENTACL, null);
            assertMessagesInVersion( SifVersion.SIF15r1, expectedMessages );
        }

        /**
	 * 
	 */
        [Test]
        public void testProvisioningZIS15r1()
        {
            String[] expectedMessages = new String[] {"SIF_Register", "SIF_SystemControl", "SIF_Subscribe"};
            Adk.SifVersion = (SifVersion.LATEST);
            Zone.Properties.ZisVersion = SifVersion.SIF15r1.ToString();
            Zone.SetSubscriber(this, InfraDTD.SIF_AGENTACL, null);
            assertMessagesInVersion( SifVersion.SIF15r1, expectedMessages );
        }


        private void assertMessagesInVersion( SifVersion version, String[] expectedMessages )
        {
            Zone.Connect( ProvisioningFlags.Register );
            InMemoryProtocolHandler handler = (InMemoryProtocolHandler) Zone.ProtocolHandler;

            for ( int a = 0; a < expectedMessages.Length; a++ )
            {
                SifMessagePayload smp = (SifMessagePayload) handler.readMsg();
                Assert.AreEqual( expectedMessages[a], smp.Tag, "Should be a " + expectedMessages[a] );
                Assert.AreEqual( version, smp.SifVersion, "Version should be " + version );
            }

            Assert.IsNull( handler.readMsg(), " Should have no more messages " );
        }

        #region ISubscriber Members

        /// <summary>  Respond to a SIF_Event received from a zone.</summary>
        /// <param name="evnt">The event data</param>
        /// <param name="zone">The zone from which this event originated</param>
        /// <param name="info">Information about the SIF_Event message</param>
        public void OnEvent( Event evnt, IZone zone, IMessageInfo info )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}