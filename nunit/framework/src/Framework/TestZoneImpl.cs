using OpenADK.Library;
using OpenADK.Library.Impl;
using Library.UnitTesting.Framework;
//using Library.Nunit.US;
//import com.OpenADK.Library.ADKTransportException;
//import com.OpenADK.Library.Agent;
//import com.OpenADK.Library.AgentProperties;
//import com.OpenADK.Library.impl.*;
namespace Library.UnitTesting.Framework
{
   public class TestZoneImpl : ZoneImpl
   {

      public TestZoneImpl(string zoneId, string zoneUrl, Agent agent, AgentProperties props)
         : base(zoneId, zoneUrl, agent, props)
      {
      }

      public TestZoneImpl(string zoneId, string zoneUrl, Agent agent, AgentProperties props, MessageDispatcher dispatcher, IProtocolHandler proto)
         : base(zoneId, zoneUrl, agent, props)
      {

         base.Dispatcher = dispatcher;
         this.fProtocolHandler = proto;
      }

       public void SetDispatcher( MessageDispatcher dispatcher )
       {
           this.fDispatcher = dispatcher;
       }


      public IProtocolHandler Proto
      {
         get
         {
            return this.fProtocolHandler;
         }
         set
         {
            this.fProtocolHandler = value;
         }
      }
   }//end class
}//end namespace

