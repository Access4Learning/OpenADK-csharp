//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library.Tools.Policy;

namespace OpenADK.Library.Impl
{
    /// <summary>
    /// The default Object Factory used by the ADK
    /// </summary>
    class ObjectFactoryImpl : ObjectFactory
    {
        /// <summary>
        /// Creates an instance of the object factory of a specified type
        /// </summary>
        /// <param name="factoryType">the type of Object factory to return</param>
        /// <param name="agentInstance">the running instance of Agent</param>
        /// <returns>the requested object factory</returns>
        /// <exception cref="OpenADK.Library.AdkException"></exception>
        public override object CreateInstance( ADKFactoryType factoryType, Agent agentInstance )
        {
            switch (factoryType)
            {
                case ADKFactoryType.ZONE:
                    return new ZoneFactoryImpl(agentInstance);
                case ADKFactoryType.TOPIC:
                    return new TopicFactoryImpl(agentInstance);
                case ADKFactoryType.POLICY_FACTORY:
                    return new AdkDefaultPolicy(agentInstance);
                case ADKFactoryType.POLICY_MANAGER:
                    return new PolicyManagerImpl(agentInstance);
            }
            return null;
        }
    }
}
