//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using OpenADK.Library.Impl;

namespace OpenADK.Library
{
    /// <summary>
    /// Manages the state of all open transports used by an agent instance
    /// </summary>
    public interface ITransportManager
    {
        /// <summary>
        /// Gets the Transport instance that has been instantiated for the specified protocol
        /// </summary>
        /// <param name="protocol">The protocol to retrieve the transport instance for (e.g. "http")</param>
        /// <returns>The transport object for the specified protocol</returns>
        /// <exception cref="AdkTransportException">If the protocol is not supported by the ADK</exception>
        ITransport GetTransport( string protocol );
    }
}
