//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Impl
{
    /// <summary>  Transport protocol classes implement this interface.
    /// 
    /// The Schools Interoperability Framework supports multiple transport
    /// protocols, of which HTTP and HTTPS are required by all agents and zone
    /// integration servers. The transport protocol used by an agent is indicated at
    /// the time it connects to a zone. The Adk was designed to take advantage of
    /// this capability by supporting not only HTTP and HTTPS but additional
    /// transports that may be introduced in the future. An agent can specify the
    /// transport and its settings for each zone to which it connects.
    /// 
    /// Transports are represented by classes that implements the Transport
    /// interface. Presently there are two such implementations: HttpProtocol and
    /// HttpsProtocol.
    /// 
    /// <b>Working with Transport Objects</b>
    /// 
    /// The Agent class maintains a list of all available Transport objects. This
    /// list will always include a HttpTransport and HttpsTransport entry, and may
    /// include additional entries depending on the version of the Adk and the
    /// features granted by the Library license key. The Agent.getTransports()
    /// method returns an array of all Transport objects while getTransport(String)
    /// returns a transport by name (e.g. "http")
    /// 
    /// A Transport object is associated with a zone via its AgentProperties. The
    /// default is HttpProtocol (HTTP is easier to work with than HTTPS during
    /// development). An agent may change the default Transport protocol inherited
    /// by all zones by calling the AgentProperties.setTransport method and passing
    /// one of the global Transport instances. For example,
    /// 
    /// <c>
    /// &nbsp;&nbsp;&nbsp;&nbsp;// Change the default transport to HTTPS<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;AgentProperties props = myAgent.getProperties();<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;props.setTransport( myAgent.getTransport("https") );<br/>
    /// </c>
    /// 
    /// 
    /// To specify configurable settings for a transport protocol, call the methods
    /// offered by each Transport class. For example, to change the port that will
    /// be used by a ZIS to contact the agent in Push mode, call the setPort(int)
    /// method of the HttpTransport class:
    /// 
    /// <c>
    /// &nbsp;&nbsp;&nbsp;&nbsp;// Change the port of the default HttpTransport object<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;HttpTransport http = myAgent.getTransport("http");<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;http.setPort(8090);<br/>
    /// </c>
    /// 
    /// 
    /// The Transport objects maintained by the Agent class are global defaults
    /// that will be used whenever the agent connects to a zone. However, it is often
    /// necessary to customize the transport protocol or settings on a per-zone
    /// basis. To do this, create a new instance of the desired Transport by
    /// passing the global default to the constructor. This creates a new Transport
    /// object that will inherit its settings from the object passed to the
    /// constructor. Next, assign that Transport to the zone properties prior to
    /// calling its connect method. Note the Agent.defaultHttpTransport and
    /// Agent.defaultHttpsTransport convenience methods can be used in place of
    /// Agent.getTransport(String), which requires that you cast the return value
    /// to the appropriate type.
    /// 
    /// <c>
    /// &nbsp;&nbsp;&nbsp;&nbsp;// Create a new HttpsTransport for this zone<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;HttpsTransport https = new HttpsTransport(myAgent.defaultHttpsTransport());<br/>
    /// <br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;// Override default settings as necessary<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;https.setPort(10348);<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;https.setKeyStore("zone_1_keystore.jks");<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;https.setKeyStorePassword("changeit");<br/>
    /// <br/>
    /// // Assign the new HttpsTransport object to the zone properties<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;zone1.getProperties().setTransport(https);<br/>
    /// </c>
    /// 
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public interface ITransport : ICloneable
    {
        /// <summary>  Gets the name of this transport protocol</summary>
        string Name { get; }

        /// <summary>  Gets the protocol name (e.g. "http", "https", etc.)</summary>
        string Protocol { get; }

        /// <summary>  Gets the transport properties</summary>
        TransportProperties Properties { get; }

        /// <summary>  Determines if this transport is secure or not</summary>
        bool Secure { get; }

        /// <summary>
        /// Activate the transport for this agent. This mehods is called
        /// when the agent is being initialized
        /// </summary>
        /// <param name="agent"></param>
        void Activate( Agent agent );

        /// <summary>  Activate this Transport for a zone. This method is called
        /// once for each zone when it is being connected to the ZIS</summary>
        /// <param name="zone">The zone
        /// </param>
        void Activate( IZone zone );

        /// <summary>  Is this Transport activated?</summary>
        bool IsActive( IZone zone );

        /// <summary>  Shutdown this Transport</summary>
        void Shutdown();

        /// <summary>
        /// returns the Protocol handler for this transport
        /// </summary>
        /// <param name="mode">The requested messaging mode</param>
        /// <returns>An IProtocolHandler instance running on this transport in the specified mode</returns>
        IProtocolHandler CreateProtocolHandler( AgentMessagingMode mode );


    }
}
