using OpenADK.Library;
using OpenADK.Library.Impl;
using NUnit.Framework;
using OpenADK.Library.us;
//import com.OpenADK.Library.ADK;
//import com.OpenADK.Library.impl.TransportPlugin;

namespace Library.UnitTesting.Framework
{
    public class InMemoryProtocolTest
    {
        protected Agent fAgent;
        protected TestZoneImpl fZone;

        protected const string TEST_URL = "http://localhost:7003?%20%34%"; 

        [SetUp]
        public virtual void Setup()
        {
            Adk.Initialize(SifVersion.LATEST, SIFVariant.SIF_US, (int)SdoLibraryType.All );
            //uses transportplugin interface , and factory method Createthat
            //returns new instance of class we're looking for 
            TransportPlugin tp = new InMemoryTransportPlugin();
            Adk.Install( tp );
            fAgent = new TestAgent();
            fAgent.Initialize();
            fAgent.Properties.TransportProtocol = tp.Protocol;

            //createzone added To ZoneFactoryImpl
            fZone =  (TestZoneImpl)fAgent.ZoneFactory.GetInstance( "test", TEST_URL );
        } //end method Setup


        [TearDown]
        public virtual void TearDown()
        {
            
        }


        public Agent Agent
        {
            get { return fAgent; }
        }

        public TestZoneImpl Zone
        {
            get { return fZone; }
        }
    } //end class
} //end namespace