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

   ///<summary>
   ///Encapsulates a set of provisioning options for a QueryResults handler
   ///</summary>
   public class QueryResultsOptions : ProvisioningOptions
   {
      ///<summary>
      /// Flag the indicates whether or not this QueryResults instance supports SIF_ExtendedQueryResults 
      /// 
      /// If <code>false</code>, the ADK will automatically send an error packet response
      /// back for any SIF_ExtendedQueryResults received.<p>
      ///  
      /// If <code>true</code>, the ADK will notify the zone of SIF_ExtendedQuery support during
      /// agent provisioning.
      ///</summary>
      private Boolean fSupportsExtendedQuery;

      ///<summary>
      /// Creates an instance of QueryResultsOptions that supports the
      /// default SIF Context
      ///</summary>
      public QueryResultsOptions() : base() { }

      ///<summary>
      /// Creates an instance of QueryResultsOptions that only supports
      /// the given set of SIFContexts. If the set of contexts given does not
      /// include the default SIF context, the default context will not be supported
      /// by this QueryResults instance
      ///</summary>
      ///<param name="contexts"></param>
      public QueryResultsOptions(params SifContext[] contexts) : base(contexts) { }

      ///<summary>
      /// Sets a flag the indicates whether or not this publisher supports SIF_ExtendedQueries
      ///</summary>
      ///<param name="fSupportsExtendedQuery">
      /// If <code>false</code>, the ADK will automatically
      /// send an error packet response back for any SIF_ExtendedQueryResults received.If 
      /// <code>true</code>, the ADK will notify the zone of SIF_ExtendedQuery support during
      ///  agent provisioning.
      ///</param>
      public void setSupportsExtendedQuery(Boolean fSupportsExtendedQuery)
      {
         this.fSupportsExtendedQuery = fSupportsExtendedQuery;
      }

      ///<summary>
      ///Sets a flag the indicates whether or not this publisher supports SIF_ExtendedQueries
      /// 
      ///  If <code>false</code>, the ADK will automatically
      /// send an error packet response back for any SIF_ExtendedQueryResults received.If 
      /// <code>true</code>, the ADK will notify the zone of SIF_ExtendedQuery support during
      ///  agent provisioning.
      ///</summary>
      public Boolean getSupportsExtendedQuery()
      {
         return fSupportsExtendedQuery;
      }
   }//end class
}//end namespace
