using System;
using OpenADK.Library;
using OpenADK.Library.Impl;

namespace Library.Nunit.US.Impl
{
   /// <summary>
   /// Summary description for TestZoneImpl.
   /// </summary>
   /// 
   public class TestZoneImpl : ZoneImpl
   {
      public TestZoneImpl(String zoneId, String zoneUrl, Agent agent, AgentProperties props)
         : base(zoneId, zoneUrl, agent, props)
      {
      }

      public TestZoneImpl(String zoneId, String zoneUrl, Agent agent, AgentProperties props,
                          MessageDispatcher dispatcher, IProtocolHandler proto)
         : base(zoneId, zoneUrl, agent, props)
      {
         this.Dispatcher = dispatcher;
         fProtocolHandler = proto;
      }
      //private bool connected;

      //public override bool Connected
      //{
      //   get
      //   {
      //      return connected;
      //   }

      //}

      //public override void Connect(ProvisioningFlags provOptions)
      //{
      //   if (Connected)
      //   {
      //      throw new InvalidOperationException("Zone already connected");
      //   }

      //   fImpl = Adk.Primitives;
      //   connected = true;
      //   fState = 0;
      //}

      protected void SetDispatcher(MessageDispatcher dispatcher)
      {
         this.Dispatcher = dispatcher;
      }

      protected void SetProto(IProtocolHandler proto)
      {
         fProtocolHandler = this.fProtocolHandler;
      }
      public IProtocolHandler getProto()
      {
         return this.fProtocolHandler;
      }
   }
}