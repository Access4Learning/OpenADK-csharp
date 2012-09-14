using Edustructures.SifWorks;
using Edustructures.SifWorks.Impl;
//import com.edustructures.sifworks.ADK;
//import com.edustructures.sifworks.impl.TransportPlugin;

using SIFWorks.UnitTesting.Framework;

namespace SIFWorks.UnitTesting.Framework
{

   public class InMemoryProtocolTest 
   {

      protected TestAgent fAgent;
      protected  TestZoneImpl fZone;

      public void Setup()
      {
         Adk.Initialize();
         TransportPlugin tp = new InMemoryTransportPlugin();
         Adk.InstallTransport(tp);
         fAgent = new TestAgent();
         fAgent.Initialize();
         fAgent.Properties.TransportProtocol=tp.Protocol;
         fZone = (TestZoneImpl)fAgent.ZoneFactory.GetInstance("test", "http://test");

      }//end method Setup

   }//end class
}//end namespace
