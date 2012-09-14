//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;

namespace OpenADK.Library
{
    /// <summary>  A class that interacts with the MessageDispatcher to perform protocol-specific
    /// messaging implements the IProtocolHandler interface. There must be a protocol
    /// handler for each communication protocol supported by the Adk. By default
    /// the HttpProtocolHandler exists to support HTTP and HTTPS.
    /// </summary>
    public interface IProtocolHandler
    {
        string Name { get; }
        void Open( ZoneImpl zone );

        void Close( IZone zone );

        void Start();

        void Shutdown();

        /// <summary>  Send a SIF infrastructure message</summary>
        IMessageInputStream Send( IMessageOutputStream msg );

        /// <summary>
        /// Returns true if the protocol and underlying transport are currently active
        /// for this zone
        /// </summary>
        /// <param name="zone"></param>
        /// <returns>True if the protocol handler and transport are active</returns>
        bool IsActive( ZoneImpl zone );

        /// <summary>
        /// Creates the SIF_Protocol object that will be included with a SIF_Register
        /// message sent to the zone associated with this Transport.</Summary>
        /// <remarks>
        /// The base class implementation creates an empty SIF_Protocol with zero
        /// or more SIF_Property elements according to the parameters that have been
        /// defined by the client via setParameter. Derived classes should therefore
        /// call the superclass implementation first, then add to the resulting
        /// SIF_Protocol element as needed.
        /// </remarks>
        /// <param name="zone"></param>
        /// <returns></returns>
        SIF_Protocol MakeSIF_Protocol( IZone zone );
    }
}
