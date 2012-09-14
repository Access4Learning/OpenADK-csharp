//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Tools.Policy
{
    /// <summary>
    /// Creates Policy objects for Zones. The factory pattern enables the default
    /// ADK policy management to be overriden by specific agents, frameworks, or
    /// implementations
    /// </summary>
    /// <remarks>
    /// <para>Policy objects control behavior of the ADK as it is configured in a specific
    /// zone or implementation. Examples of policy behaviors are requesting data in a 
    /// specific version of SIF, requesting data in a specific SIF Context, or sending
    /// events using a specific version of SIF in a specific zone.
    /// </para>
    /// <para>Policy can usually be applied external to agent code. For example, request policy
    /// is, for the most part, transparent from the agent's code. 
    /// </para>
    /// </remarks>
    public abstract class PolicyFactory
    {
        /// <summary>
        /// Returns an implementation of the PolicyFactory class that returns
        /// an implementation-specific set of policies for managing the agent
        /// </summary>
        /// <param name="agent">The Agent instance to retrieve policy information for</param>
        /// <returns>an instance of PolicyFactory</returns>
        /// <exception cref="AdkException">If the PolicyFactory instance cannot be created</exception>
        public static PolicyFactory GetInstance( Agent agent )
        {
            return new AdkDefaultPolicy( agent );
        }


        /// <summary>
        /// Returns the ObjectRequestyPolicy for the specified SIF Data Object
        /// </summary>
        /// <param name="zone">The zone to get policy information for</param>
        /// <param name="objectType">The name of the SIF Data Object for
        /// which to return request policy</param>
        /// <returns> An instance of ObjectRequestPolicy that has been initialized
        /// to prescribe policy for requesting data of that type or <c>null</c> if
        /// no policy is defined</returns>
        public abstract ObjectRequestPolicy GetRequestPolicy( IZone zone, string objectType );
    }
}
