//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Impl
{
   /// <summary>  The ISIFPrimitives interface is implemented by internal Adk classes that
   /// implement primitive SIF messaging functionality such as SIF_Register, SIF_Event,
   /// and SIF_SystemControl. Internal Adk classes always delegate low-level SIF
   /// functions to the global ISIFPrimitives object provided by the static <c>
   /// Adk.getPrimitives</c> method.
   /// 
   /// </summary>
   /// <author>  Eric Petersen
   /// </author>
   /// <version>  1.0
   /// </version>
   public interface ISIFPrimitives
   {

      /// <summary>  SIF_Register</summary>
      SIF_Ack SifRegister(IZone zone);

      /// <summary>  SIF_Unregister</summary>
      SIF_Ack SifUnregister(IZone zone);

      /// <summary>  SIF_Subscribe</summary>
      SIF_Ack SifSubscribe(IZone zone,
                            string[] objectType);

      /// <summary>  SIF_Unsubscribe</summary>
      SIF_Ack SifUnsubscribe(IZone zone,
                              string[] objectType);

      /// <summary>  SIF_Provide</summary>
      SIF_Ack SifProvide(IZone zone,
                          string[] objectType);

      /// <summary>  SIF_Unprovide</summary>
      SIF_Ack SifUnprovide(IZone zone,
                            string[] objectType);

      /// <summary>  SIF_Ping</summary>
      SIF_Ack SifPing(IZone inZone);

      /// <summary>  SIF_ZoneStatus</summary>
      SIF_Ack SifZoneStatus(IZone zone);

      /// <summary>
      /// SifGetAgentACL
      /// </summary>
      /// <param name="zone"></param>
      /// <returns></returns>
      SIF_Ack SifGetAgentACL(IZone zone);

      ///<summary>
      /// SIF_Provision
      ///</summary>
      SIF_Ack SifProvision(
           IZone zone,
           SIF_ProvideObjects providedObjects,
           SIF_SubscribeObjects subscribeObjects,
           SIF_PublishAddObjects publishAddObjects,
           SIF_PublishChangeObjects publishChangeObjects,
           SIF_PublishDeleteObjects publishDeleteObjects,
           SIF_RequestObjects requestObjects,
           SIF_RespondObjects respondObjects);

      /// <summary>  SIF_Sleep</summary>
      SIF_Ack SifSleep(IZone zone);

      /// <summary>  SIF_Wakeup</summary>
      SIF_Ack SifWakeup(IZone zone);

      /// <summary>  Sends a SIF_Event</summary>
      SIF_Ack SifEvent(IZone zone,
                        Event inEvent,
                        string destinationId,
                        string sifMsgId);

      /// <summary>  SIF_Request</summary>
      SIF_Ack SifRequest(IZone zone,
                          Query query,
                          string destinationId,
                          string sifMsgId);
   }
}
