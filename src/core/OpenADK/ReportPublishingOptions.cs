//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library
{

   ///<summary>
   /// Encapsulates all of the optional behavior that a publisher is able to support. 
   /// Flags in this class are used to control behavior of the agent during provisioning
   ///</summary>
   public class ReportPublishingOptions : PublishingOptions
   {
      ///<summary>
      ///Creates an instance of PublishingOptions that supports the
      /// default SIF Context.
      /// 
      /// SendSiFProvide <code>True</code> if the ADK should provision this 
      /// agent as the default provider in the zone for the object.
      ///</summary>

      public ReportPublishingOptions() : this(true) { }

      ///<summary>
      /// Creates an instance of PublishingOptions that supports the
      /// default SIF Context.
      /// 
      /// sendSIFProvide <code>True</code> if the ADK should provision this 
      /// agent as the default provider in the zone for the object.
      ///</summary>
      public ReportPublishingOptions(Boolean sendSIFProvide) : base(sendSIFProvide) { }

      ///<summary>
      /// Creates an instance of PublishingOptions that only supports
      ///the given set of SIFContexts. 
      ///</summary>
      ///<param name="contexts"></param>
      public ReportPublishingOptions(params SifContext[] contexts) : base(contexts) { }

   }//end class
}//end namespace
