//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Specialized;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Impl
{
    /// <summary>  Abstract base class for all Transport implementations.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public abstract class TransportImpl : ITransport
    {
        protected TransportProperties fProps;

        #region abstract methods

        /// <summary>
        /// The name of this transport
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// A string representation of the protocol being used
        /// </summary>
        public abstract string Protocol { get; }

        /// <summary>
        /// Whether or not this Transport is secure
        /// </summary>
        public abstract bool Secure { get; }

    
        /// <summary>
        /// Activate the transport for this agent. This mehods is called
        /// when the agent is being initialized
        /// </summary>
        /// <param name="agent"></param>
        public abstract void Activate( Agent agent );

        /// <summary>  Creates an IProtocolHandler for the zone associated with this Transport instance.</summary>
        public abstract IProtocolHandler CreateProtocolHandler(AgentMessagingMode mode);
    

        /// <summary>
        /// Create a copy of this object
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>  Activate this Transport for a zone</summary>
        public abstract void Activate( IZone zone );

        /// <summary>  Is this Transport activated?</summary>
        public abstract bool IsActive( IZone zone );

        /// <summary>  Shutdown this Transport</summary>
        public abstract void Shutdown();

        #endregion

        /// <summary>
        /// The properties being used for this transport
        /// </summary>
        public virtual TransportProperties Properties
        {
            get { return fProps; }
        }


        #region Protected Members

        /// <summary>
        /// Creates in instance of the transport
        /// </summary>
        /// <param name="props"></param>
        protected internal TransportImpl( TransportProperties props )
        {
            fProps = props;
        }



        #endregion

 
    }
}
