//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library
{


   ////////////////////////////////////////////////////////////////////////////////
   //
   //  Copyright (c)2011 Pearson Education, Inc., or associates.
   //  All rights reserved.
   //
   //  This software is the confidential and proprietary information of
   //  Data Solutions ("Confidential Information").  You shall not disclose
   //  such Confidential Information and shall use it only in accordance with the
   //  terms of the license agreement you entered into with Data Solutions.
   //

   /**
    * Encapsulates the set of Subscription options supported by a given subscriber
    * @author Andrew
    *
    */
   public class SubscriptionOptions : ProvisioningOptions
   {

      private Boolean fSendSIFSubscribe = true;

      ///<summary>
      /// Creates an instance of SubscriptionOptions that supports the
      /// default SIF Context
      ///</summary>
      public SubscriptionOptions() : base() { }



      ///<summary>
      /// Creates an instance of SubscriptionOptions that only supports
      /// the given set of SIFContexts. If the set of contexts given does not
      /// include the default SIF context, the default context will not be supported
      /// by this Subscriber
      ///</summary>
      ///<param name="contexts"></param>
      public SubscriptionOptions(params SifContext[] contexts) : base(contexts) { }

      ///<summary>
      ///If ADK managed provisioining is in effect, this flag controls whether
      /// a SIF_Subscribe message is sent when connecting to the ZIS in legacy mode.<p>
      /// 
      /// The default value of this property is  <code>True</code>
      /// 
      ///<see cref="AgentProperties.ProvisionInLegacyMode()"></see> 
      ///</summary>
      public Boolean SendSIFSubscribe
      {
         get
         {
            return fSendSIFSubscribe;
         }
         set
         {
            fSendSIFSubscribe = value;
         }
      }

   }//end class
}//end namespace
