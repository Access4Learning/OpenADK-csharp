//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library
{

   public class ProvisioningOptions
   {

      private List<SifContext> fSupportedContexts = new List<SifContext>(1);

      ///<summary>
      /// Creates a ProvisioningOptions instance that supports the default SIF Context
      ///</summary>
      protected ProvisioningOptions()
      {
         fSupportedContexts.Add(SifContext.DEFAULT);
      }

      protected ProvisioningOptions(params SifContext[] contexts)
      {
         addSupportedContext(contexts);
      }

      ///<summary>
      /// Adds one or more SIFContexts to this ProvisioningOptions instance
      ///</summary>
      ///<param name="contexts">One or more supported SIFContext instances</param>
      
      public void addSupportedContext(params SifContext[] contexts)
      {
         foreach (SifContext ctxt in contexts)
         {
            if (!fSupportedContexts.Contains(ctxt))
            {
               fSupportedContexts.Add(ctxt);
            }
         }
      }


      ///<summary>
      ///Determines if this provisioning instance supporst
      ///</summary>
      ///<param name="contextName"></param>
      public Boolean supportsContext(String contextName)
      {
         foreach (SifContext ctxt in fSupportedContexts)
         {
            if (ctxt.Equals(contextName))
            {
               return true;
            }
         }
         return false;
      }

      public List<SifContext> SupportedContexts
      {
         get
         {
            return fSupportedContexts;
         }
      }

   }//end class
}//end namespace
