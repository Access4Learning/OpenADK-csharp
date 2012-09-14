using System;
using OpenADK.Library;
using OpenADK.Library.Impl;
using Library.UnitTesting;

//import com.OpenADK.Library.ADKTransportException;
//import com.OpenADK.Library.TransportProperties;
//import com.OpenADK.Library.impl.Transport;
//import com.OpenADK.Library.impl.TransportPlugin;

/**
 * 
 * Class used by the ADK testing framework to test ADK messaging
 * @author Andrew
 *
 */
namespace Library.UnitTesting.Framework
{
   public class InMemoryTransportPlugin : TransportPlugin
   {

      public override string Protocol
      {
         get
         {
            return "AdkInMemory";
         }
      }

      public override ITransport NewInstance(TransportProperties props)
      {
         return new InMemoryTransport(props);
      }

      public override Boolean Internal
      {
         get
         {
            return false;
         }
      }//end property isInternal
      public override TransportProperties CreateProperties()
      {
         
         return new InMemoryProperties();
      }

   }//end class
}//end namespace