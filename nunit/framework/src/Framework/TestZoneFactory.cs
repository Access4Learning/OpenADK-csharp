using System;
using OpenADK.Library;
using OpenADK.Library.Impl;
//using Library.Nunit.US;

namespace Library.UnitTesting.Framework
{
   public class TestZoneFactory : ZoneFactoryImpl
   {

      public TestZoneFactory(Agent agent)
         : base(agent)
      {
         // TODO Auto-generated constructor stub
      }

      protected override IZone CreateZone(String zoneId, String zoneUrl, AgentProperties props)
      {
         TestZoneImpl zone = new TestZoneImpl(zoneId, zoneUrl, fAgent, props);

         zone.Proto = new InMemoryProtocolHandler();
         return zone;
      }

   }//end class
}//end namespace
