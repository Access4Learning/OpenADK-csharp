//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OpenADK.Library.Impl
{
    internal class TransportManagerImpl : ITransportManager
    {
        private List<ITransport> fTransports = new List<ITransport>();
        private List<TransportProperties> fDefaultTransportProps;


        /// <summary>
        /// Activate the transport for this zone
        /// </summary>
        /// <param name="zone">the zone that is being activated</param>
        /// <returns>The transport object that was used to activate the zone</returns>
        /// <exception cref="AdkTransportException">If one of the configured protocols is not supported by the ADK</exception>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public IProtocolHandler Activate( ZoneImpl zone )
        {
            //  Activate the transport used by the zone
            ITransport trans = GetTransport( zone.Properties.TransportProtocol );
            trans.Activate( zone );
            return trans.CreateProtocolHandler( zone.Properties.MessagingMode );
        }


        /// <summary>
        /// Gets the Transport instance that has been instantiated for the specified protocol
        /// </summary>
        /// <param name="protocol">The protocol to retrieve the transport instance for (e.g. "http")</param>
        /// <returns>The transport object for the specified protocol</returns>
        /// <exception cref="AdkTransportException">if the protocol is not supported by the ADK</exception>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public ITransport GetTransport( String protocol )
        {
            if ( protocol == null )
            {
                throw new ArgumentException( "Protocol cannot be null" );
            }
            protocol = protocol.ToLower();

            foreach ( ITransport trans in fTransports )
            {
                if ( trans.Protocol.Equals( protocol ) )
                {
                    return trans;
                }
            }

            // No transport has been created for this protocol yet. Create
            // new one using the TransportPlugin
            TransportPlugin tp = Adk.GetTransportProtocol( protocol );
            TransportProperties defs = GetDefaultTransportProperties( protocol );

            ITransport transport = tp.NewInstance( defs );
            fTransports.Add( transport );
            return transport;
        }


        /// <summary>
        /// Gets the default properties for a transport protocol
        /// </summary>
        /// <remarks>
        ///  Each transport protocol supported by the ADK is represented by a class
        ///  that implements the Transport interface. Transports are identified by
        ///  a string such as "http" or "https". Like Zones, each Transport instance
        ///  is associated with a set of properties specific to the transport
        ///  protocol. Such properties may include IP address, port, SSL security
        ///  attributes, and so on. The default properties for a given transport
        ///  protocol may be obtained by calling this method.
        /// </remarks>
        /// <param name="protocol"></param>
        /// <returns>The default properties for the specified protocol</returns>
        /// <exception cref="AdkTransportException">is thrown if the protocol is not supported
        /// by the ADK</exception>
        public TransportProperties GetDefaultTransportProperties( String protocol )
        {
            //  Already initialized?
            if ( fDefaultTransportProps == null )
            {
                fDefaultTransportProps = new List<TransportProperties>();
            }
            else
            {
                foreach ( TransportProperties props in fDefaultTransportProps )
                {
                    if ( props.Protocol.Equals( protocol ) )
                    {
                        return props;
                    }
                }
            }

            // Didn't find the transport properties above. Create a new one and return it
            TransportPlugin tp = Adk.GetTransportProtocol( protocol );
            if ( tp == null )
            {
                throw new AdkTransportException( "The requested transport protocol: '" + protocol +
                                                 "'  is not supported by this instance of the ADK", null );
            }

            TransportProperties properties = tp.CreateProperties();
            properties.Defaults(null);
            fDefaultTransportProps.Add(properties);
            return properties;
        }


        /// <summary>
        /// Shuts down all transports managed by this TransportManager instance 
        /// </summary>
        /// <exception cref="AdkTransportException">If the ADK is configured improperly</exception>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public void Shutdown()
        {
            foreach ( ITransport t in fTransports )
            {
                t.Shutdown();
            }
            fTransports.Clear();
        }


        /// <summary>
        /// Initializes the enabled agent transports
        /// </summary>
        /// <param name="agent"></param>
        /// <exception cref="AdkTransportException">If the ADK is configured improperly</exception>
        public void Activate( Agent agent )
        {
            // Initialize each transport supported by the ADK
            foreach ( String protocol in Adk.TransportProtocols )
            {
                TransportProperties tp = GetDefaultTransportProperties( protocol );
                if ( tp.Enabled )
                {
                    ITransport transport = GetTransport( protocol );
                    transport.Activate( agent );
                }
            }
        }
    }
}
