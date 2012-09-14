//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Tools
{
    /// <summary>  This interface is implemented by classes that wish to be notified when a
    /// LoadBalancer's free pool increases from zero to greater than one, signaling
    /// Batons are once again available to the thread.
    /// 
    /// </summary>
    /// <author> Data Solutions
    /// </author>
    /// <version>  ADK 1.0
    /// </version>
    public interface ILoadBalancerListener
    {
        /// <summary>  Called when a LoadBalancer's free pool increases from zero to greater
        /// than one, signaling Batons are once again available to this thread.
        /// </summary>
        void OnBatonsAvailable( LoadBalancer balancer );
    }
}
