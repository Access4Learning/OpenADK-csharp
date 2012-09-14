//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Tools.Cfg;

namespace OpenADK.Library.Tools.Policy
{
    public class AdkDefaultPolicy : PolicyFactory
    {
        private AgentConfig fConfig;

        public AdkDefaultPolicy( Agent agent )
        {
            if ( agent != null )
            {
                Object source = agent.ConfigurationSource;
                if ( source != null && source is AgentConfig )
                {
                    fConfig = (AgentConfig) source;
                }
            }
        }


        /// <summary>
        /// Returns the ObjectRequestyPolicy for the specified SIF Data Object
        /// </summary>
        /// <param name="zone">The zone to get policy information for</param>
        /// <param name="objectType">The metadata definition of the SIF Data Object for
        /// which to return request policy</param>
        /// <returns> An instance of ObjectRequestPolicy that has been initialized
        /// to prescribe policy for requesting data of that type or <c>null</c> if
        /// no policy is defined</returns>
        public override ObjectRequestPolicy GetRequestPolicy( IZone zone, string objectType )
        {
            // TODO Implement this based on the Java ADK's default request policy
            return null;
        }
    }
}
