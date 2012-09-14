using System;
using OpenADK.Library;
using NUnit.Framework;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;

//import com.OpenADK.Library.ADKTransportException;
//import com.OpenADK.Library.TransportProperties;
//import com.OpenADK.Library.Zone;
//import com.OpenADK.Library.impl.IProtocolHandler;
//import com.OpenADK.Library.impl.Transport;
//import com.OpenADK.Library.impl.TransportImpl;
//import com.OpenADK.Library.infra.SIF_Protocol;

namespace Library.UnitTesting.Framework
{
   public class InMemoryTransport : TransportImpl
   {

      private Boolean fActive;

      public InMemoryTransport(TransportProperties props):base(props)
      {
         // TODO Auto-generated constructor stub
      }

      //  Definition changed. now takes mode arg. Not used in for InMemoryProtocolhandler
      public override IProtocolHandler CreateProtocolHandler(AgentMessagingMode mode)
      {
         return new InMemoryProtocolHandler();
      }
      




      public override object Clone()
      {
         // TODO Auto-generated method stub
         return null;
      }


      public override void Activate(IZone zone)
      {
         fActive = true;
      }

      public override bool IsActive(IZone zone)
      {
         return fActive;
      }

      public override void Shutdown()
      {
         fActive = false;
      }

      public override string Name
      {
         get
         {
            // TODO Auto-generated method stub
            return "ADK In Memory Tranport Handler";
         }
      }

      public override String Protocol
      {
         get
         {
            // TODO Auto-generated method stub
            return "AdkInMemory";
         }
      }

      public override Boolean Secure
      {//TODO Auto-generated method stub
         get
         {
            return false;
         }
      }


       /// <summary>
       /// Activate the transport for this agent. This mehods is called
       /// when the agent is being initialized
       /// </summary>
       /// <param name="agent"></param>
       public override void Activate( Agent agent )
       {
           
       }
   }//end class
}//end namespace