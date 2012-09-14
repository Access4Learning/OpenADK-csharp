//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Impl
{
    public abstract class TransportPlugin
    {
        /// <summary>
        /// The protocol supported by this transport plugin (e.g. "http")
        /// </summary>
        public abstract string Protocol { get; }

        /// <summary>
        /// Returns true if this protocol is internal to the ADK
        /// </summary>
        public abstract bool Internal { get; }

        /// <summary>
        /// Creates a new instance of this transport
        /// </summary>
        /// <param name="props">The Transport properties to use for this transport</param>
        /// <returns></returns>
        public abstract ITransport NewInstance( TransportProperties props );

        /// CreateProperties method is used to explicitly instantiate 
        /// a new TransportProperties object and not rely on the NewInstance
        /// method which is limited to http and https Protocols
        public abstract TransportProperties CreateProperties();
    }
}
