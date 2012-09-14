//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using OpenADK.Library.Global;
using OpenADK.Library.Infra;
using OpenADK.Util;
using OpenADK.Web;
using OpenADK.Web.Http;
using log4net;
using Org.Mentalis.Security.Certificates;
using Org.Mentalis.Security.Ssl;
using StoreLocation=Org.Mentalis.Security.Certificates.StoreLocation;

namespace OpenADK.Library.Impl
{
    /// <summary>  Transport class for the HTTP and HTTPS protocols.
    /// 
    /// One instance of HttpTransport is shared among all zones that use the HTTP or
    /// HTTPS protocols. When activated, it creates an internal  HTTP/HTTPS web
    /// server for use by all zones the agent is connected to using these protocols.
    /// Each zone then instantiates its own HttpProtocolHandler, which is responsible
    /// for sending and receiving messages. (The HttpTransport class does not send
    /// or receive messages.)
    /// 
    /// Because many SIF Agents may use the Adk, no default HTTP or HTTPS port is
    /// assigned to push mode agents by the class framework. It is the developer's
    /// responsibility to assign a default port (pull-mode agents do not require a
    /// port be assigned.) To do so, use one of the following methods:
    /// 
    /// <ul>
    /// <li>
    /// Set the <c>adk.transport.http.port</c> system property prior
    /// to creating your agent's Zones and/or Topics. This property can be
    /// set programmatically.
    /// </li>
    /// <li>
    /// Call the setPort method on the default HttpProperties and/or
    /// HttpsProperties objects prior to creating your agent's Zones and/or
    /// Topics. The following block of code demonstrates:<br/>
    /// <br/>
    /// <c>
    /// 
    /// //  Set transport properties for HTTP<br/>
    /// Agent myAgent = ...<br/>
    /// HttpProperties http = agent.getDefaultHttpProperties();<br/>
    /// http.setPort( 7081 );<br/>
    /// <br/>
    /// //  Set transport properties for HTTPS<br/>
    /// HttpsProperties https = agent.getDefaultHttpsProperties();<br/>
    /// https.setPort( 7082 );<br/>
    /// https.setKeystorePassword( "changeit" );<br/>
    /// ...<br/>
    /// <br/>
    /// </li>
    /// </ul>
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class HttpTransport : TransportImpl, ICertificatePolicy
    {
        private const string OID_SERVER_AUTHENTICATION = "1.3.6.1.5.5.7.3.1";
        private const string OID_CLIENT_AUTHENTICATION = "1.3.6.1.5.5.7.3.2";

        /// <summary>  Gets the name of this transport protocol</summary>
        /// <returns> The unique name of the transport protocol, used to identify this
        /// transport among all transports that may be active in the agent
        /// </returns>
        /// <seealso cref="Protocol">
        /// </seealso>
        public override String Name
        {
            get { return fProps.Protocol; }
        }

        /// <summary>  Gets the protocol name</summary>
        /// <returns> The protocol name used in constructing URLs (e.g. "http", "https", etc.)
        /// </returns>
        public override String Protocol
        {
            get { return fProps.Protocol; }
        }

        /// <summary>  Determines if this transport is secure or not</summary>
        public override bool Secure
        {
            get { return Protocol.Equals( "https" ); }
        }

        /// <summary>  Gets or sets the local port this transport protocol will use when listening for
        /// incoming traffic from the ZIS.
        /// </summary>
        /// <value> The port number returned by the TransportProperties object
        /// passed to the constructor
        /// </value>
        public virtual int Port
        {
            get { return ((HttpProperties) fProps).Port; }

            set { ((HttpProperties) fProps).Port = value; }
        }


        /// <summary>
        /// Gets or sets string that contains the host name or an IP address in dotted-quad notation for IPv4 
        /// and in colon-hexadecimal notation for IPv6. Unlike the Java ADK, the .NET ADK will return NULL for this property
        /// if it is not set. If it is not set, the .NET ADK will bind to all local IP addresses and return the first
        /// one to the ZIS in the SIF_Register list
        /// </summary>
        public virtual String Host
        {
            get
            {
                return ((HttpProperties)fProps).Host;
            }
            set
            {
                ((HttpProperties)fProps).Host = value;
            }
        }


        /// <summary> Gets or sets the local IPAddress this transport bind to. If the Host property is set, 
        /// this property will attempt to find the IPAddress assigned to the host. Otherwise, it will default
        /// to IPAddress.Any
        /// </summary>
        /// <value> The hostname returned by the TransportProperties object passed
        /// to the constructor
        /// </value>
        private IPAddress getPushHostIP()
        {
            String host = this.Host;
            if( string.IsNullOrEmpty( host ) )
            {
                // First option is to return IPAddress.Any
                return IPAddress.Any;
            }

            // Try to convert it to an IP Address, one of two ways:
            IPAddress returnValue = null;
            if( IPAddress.TryParse( host, out returnValue ) )
            {
                // Second option is to return the IP Address, if specified in that format
                return returnValue;
            }
           
            // try doing a reverse DNS lookup
            IPAddress[] hostAddresses = Dns.GetHostAddresses( host );
            if (hostAddresses.Length > 0)
            {
                // Debug all resolutions
                foreach (IPAddress addr in hostAddresses)
                {
                    DebugTransport( "Host {0} resolved to {1}", host, addr );
                }
                DebugTransport("Using {0}", hostAddresses[0] );
                // Third option is to return the resolved IP Address from a host name
                return hostAddresses[0];
            }
           
            DebugTransport("Unable to resolve host {0}: using {1}", host, IPAddress.Any );

            // Fourth option is to return IPAddress.Any
            return IPAddress.Any;
                
        }

        private String getPushHostName()
        {
            String host = ((HttpProperties)fProps).PushHost;
            if (String.IsNullOrEmpty(host))
            {
                // Since the Host property is not set, the ADK has bound the ADKSocketBinding 
                // to IPAddress.Any. However, to the ZIS we need to return the most appropriate
                // public IP Address we can find.
                IList<IPAddress> addresses = NetworkUtils.GetLocalAddresses(false);
                if (addresses.Count > 0)
                {
                    // Return the first valid address (they are already pre-validated)
                    return addresses[0].ToString();
                }
                else
                {
                    return IPAddress.Loopback.ToString();
                }
            }
            return host;
        }



        /// <summary>The SIF Authentication Level to use for client connections</summary>
        public virtual int ClientAuthLevel
        {
            get
            {
                if ( string.Compare( fProps.Protocol, "https", true ) == 0 )
                {
                    return ((HttpsProperties) fProps).ClientAuthLevel;
                }

                return 0;
            }

            set
            {
                if ( string.Compare( fProps.Protocol, "https", true ) == 0 )
                {
                    ((HttpsProperties) fProps).ClientAuthLevel = value;
                }
            }
        }

        /// <summary>  The root log Category. Subcategories exist for each zone, where the
        /// subcategory name is "Agent.<i>zoneId</i>". The Adk uses the root
        /// Category when writing log events that are not associated with a
        /// specific zone. Your agent may also use this Category to post log
        /// events.
        /// </summary>
        private ILog log = null;

        /// <summary>  Constructs an HttpTransport for HTTP or HTTPS</summary>
        /// <param name="props">Transport properties (usually an instance of HttpProperties
        /// or HttpsProperties)
        /// </param>
        public HttpTransport( HttpProperties props )
            : base( props )
        {
            Construct( props );
        }

        private HttpTransport( HttpProperties props,
                               IHttpServer server )
            : base( props )
        {
            Construct( props );
            sServer = server;
        }

        private void Construct( HttpProperties props )
        {
            fProps = props;
            log = LogManager.GetLogger( Adk.LOG_IDENTIFIER + ".Agent.transport$" + fProps.Protocol );
        }

        /// <summary>  Clone this HttpTransport.
        /// 
        /// Cloning a transport results in a new HttpTransport instance with
        /// HttpProperties that inherit from this object's properties. However, the
        ///  web server owned by this transport will be shared with the cloned
        /// instance.
        /// </summary>
        public ITransport CloneTransport()
        {
            HttpProperties props;
            if ( fProps is HttpsProperties )
            {
                props = new HttpsProperties( (HttpsProperties) fProps.Parent );
            }
            else
            {
                props = new HttpProperties( (HttpProperties) fProps.Parent );
            }
            //  This object and the clone share the  server
            HttpTransport t = new HttpTransport( props, sServer );
            return t;
        }


        /// <summary>
        /// Returns a copy of this object
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return CloneTransport();
        }

        /// <summary>  Activate this Transport for an agent. This method is called when the agent is initialized</summary>
        /// <param name="agent">The Agent</param>
        public override void Activate( Agent agent )
        {
            ActivateServer( agent, agent.Properties, false );
        }

        /// <summary>
        /// Activate the embedded HTTP server if the agent is configured to run in push mode or
        /// support for servlets is enabled
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="agentProps"></param>
        /// <param name="isPushModeZone"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.Synchronized )]
        private bool ActivateServer( Agent agent, AgentProperties agentProps, bool isPushModeZone )
        {
            HttpProperties props = (HttpProperties) fProps;
            bool webAppEnabled = props.ServletEnabled;

            // Create the http server if it doesn't exist. All HttpTransport
            // objects share once instance of the server.
            if ( (sServer == null || !sServer.IsStarted)
                 && (webAppEnabled || isPushModeZone) )
            {
                if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 && log.IsInfoEnabled )
                {
                    log.Info( "Activating " + fProps.Protocol.ToUpperInvariant() + " transport..." );
                }

                if ( sServer == null )
                {
                    // TODO: This should be using some type of a factory metaphor, based on a config setting
                    // We may be going to support, at the minimum, three different HttpPushProtocol configurations
                    // 1) Using the embedded AdkHttpSifServer()
                    // 2) a Wrapper arround HTTP.SYS on Windows 2003
                    // 3) an implementation that runs inside IIS
                    sServer = new AdkHttpApplicationServer( this );
                }
                sServer.Start();
            }


            return sServer != null;
        }


        /// <summary>
        /// Activate this Transport for a zone. This method is called for every zone
        /// when it is being connected
        /// </summary>
        /// <param name="zone"></param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public override void Activate( IZone zone )
        {
            bool isPushMode = zone.Properties.MessagingMode == AgentMessagingMode.Push;
            if ( ActivateServer( zone.Agent, zone.Properties, isPushMode ) && isPushMode )
            {
                // Configure the server specifically for this Transport
                ConfigureServer( zone );
            }
        }


        /// <summary>  Is this Transport activated?</summary>
        public override bool IsActive( IZone zone )
        {
            if ( zone.Properties.MessagingMode == AgentMessagingMode.Push )
            {
                if ( sServer != null )
                {
                    return sServer.IsStarted;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>  Shutdown this Transport</summary>
        public override void Shutdown()
        {
            lock ( this )
            {
                if ( sServer != null )
                {
                    sServer.Shutdown( true );
                }
            }
        }

        /// <summary>  Configure the  server for HTTP as needed based on the settings of
        /// this Transport object. If the server does not have a SocketListener on
        /// the port specified for this transport, one is created. 
        /// configuration is performed dynamically as HttpTransport objects are
        /// created, so listeners are added to the server the first time they are
        /// needed.
        /// </summary>
        protected void ConfigureServer( IZone zone )
        {
            AdkSocketBinding newListener;
            if ( String.Compare( Protocol, "http", true ) == 0 )
            {
                newListener = ConfigureHttp( zone );
            }
            else if ( String.Compare( Protocol, "https", true ) == 0 )
            {
                newListener = ConfigureHttps( zone );
            }
            else
            {
                throw new Exception( "HttpTransport object configured with properties for another protocol: " +
                                     fProps.Protocol );
            }

            if ( newListener != null )
            {
                sServer.AddListener( newListener );
                try
                {
                    newListener.Start();
                }
                catch ( Exception le )
                {
                    AdkUtils._throw(
                        new AdkTransportException( "Error starting SocketListener: " + le.Message, zone, le ), log );
                }
            }
        }


        protected internal virtual AdkSocketBinding ConfigureHttp( IZone zone )
        {
            int port = Port;
            if ( port == -1 )
            {
                throw new AdkTransportException
                    ( "The agent is not configured with a default HTTP port" );
            }

            IPAddress hostAddress = this.getPushHostIP();

            //  If there is no SocketListener on this port, create one
            AdkSocketBinding listener = sServer.GetListener( port );
            if ( listener == null )
            {
                if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 )
                {
                    if ( hostAddress != null )
                    {
                        log.Debug
                            ( "Creating HTTP listener for push mode on " + hostAddress + ":" + port );
                    }
                    else
                    {
                        log.Debug( "Creating HTTP listener for push mode on port " + port );
                    }
                }


                listener = sServer.CreateHttpListener();
                ConfigureSocketListener( listener, zone, port, hostAddress );
                return listener;
            }
            else
            {
                if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 )
                {
                    if ( hostAddress != null )
                    {
                        log.Debug( "Already a HTTP listener on " + hostAddress + ":" + port );
                    }
                    else
                    {
                        log.Debug( "Already a HTTP listener on port " + port );
                    }
                }
            }

            return null;
        }

        private void ConfigureSocketListener( AdkSocketBinding listener, IZone zone, int port, IPAddress address )
        {
            listener.HostAddress = address;
            listener.Port = port;
            listener.RawBufferSize = zone.Properties.MaxBufferSize + 512;


            // TT 1440 Add support for a "Max-Connections" feature in the ADK
            // This is currently an experimental, undocumented and untested feature.
            // See http://jetty.mortbay.org/jetty5/doc/optimization.html for more
            // information on optimization with Jetty
            HttpProperties httpProps = (HttpProperties) fProps;
            int maxRequestThreads = httpProps.MaxConnections;
            if ( maxRequestThreads > 0 )
            {
                listener.MaxClientConnections = maxRequestThreads;

                // No support in the .NET ADK yet for these features

                //			int minRequestThreads = httpProps.getMinConnections();
                //			if (minRequestThreads < 0) {
                //				minRequestThreads = (int) Math.ceil(maxRequestThreads / 5);
                //			}
                //			listener.setMinThreads(minRequestThreads);
                //
                //			int maxIdleTimeMs = httpProps.getMaxIdleTimeMs();
                //			if (maxIdleTimeMs > 0) {
                //				listener.setMaxIdleTimeMs(maxIdleTimeMs);
                //			}
                //			int lowResourcesPersistTimeMs = httpProps
                //					.getLowResourcesPersistTimeMs();
                //			if (lowResourcesPersistTimeMs > 0) {
                //				listener.setLowResourcePersistTimeMs(lowResourcesPersistTimeMs);
                //			}

                if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 && log.IsDebugEnabled )
                {
                    log.Debug( "Set HttpListener.maxThreads to " + maxRequestThreads );
                    //				if (minRequestThreads > 0) {
                    //					log.Debug("Set HttpListener.minThreads to " + minRequestThreads );
                    //				}
                    //				if (maxIdleTimeMs > 0) {
                    //					log.Debug("Set HttpListener.maxIdleTimeMs to " + maxIdleTimeMs );
                    //				}
                    //				if (lowResourcesPersistTimeMs > 0) {
                    //					log.Debug("Set HttpListener.lowResourcesPersistTimeMs to " + lowResourcesPersistTimeMs );
                    //				}
                }
            }
        }


        /// <summary>  Configure the  server for HTTPS as needed based on the settings of
        /// this Transport object. If the server does not have a JSSEListener on
        /// the port specified for this transport, one is created. 
        /// configuration is performed dynamically as HttpTransport and HttpsTransport
        /// objects are created, so listeners are added to the server the first time
        /// they are needed.
        /// </summary>
        protected internal virtual AdkSocketBinding ConfigureHttps( IZone zone )
        {
            int port = Port;
            if ( port == -1 )
            {
                throw new AdkTransportException
                    ( "The agent is not configured with a default HTTP port" );
            }

            IPAddress hostAddress = getPushHostIP();

            //  If there is no SocketListener on this port, create one
            AdkSocketBinding listener = sServer.GetListener( port );
            if ( listener == null )
            {
                if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 )
                {
                    if ( hostAddress != null )
                    {
                        log.Debug
                            ( "Creating HTTPS listener for push mode on " + hostAddress + ":" + port );
                    }
                    else
                    {
                        log.Debug( "Creating HTTPS listener for push mode on port " + port );
                    }
                }

                //  If there is no SSL listener on this port, create one
                try
                {
                    Certificate cert = GetServerAuthenticationCertificate();
                    if ( cert == null )
                    {
                        throw new AdkTransportException
                            ( "Unable to locate certificate for Server Authentication in the selected certificate store" );
                    }

                    DebugTransport( "Using {0} ", cert.ToString( true ) );


                    SecurityOptions options =
                        new SecurityOptions
                            ( SecureProtocol.Ssl3 | SecureProtocol.Tls1, cert, ConnectionEnd.Server );
                    int clientAuthLevel = ClientAuthLevel;
                    if ( clientAuthLevel > 0 )
                    {
                        options.Flags = SecurityFlags.MutualAuthentication;
                        options.VerificationType = CredentialVerification.Manual;
                        if ( clientAuthLevel > 3 )
                        {
                            clientAuthLevel = 3;
                        }
                        switch ( clientAuthLevel )
                        {
                            case 1:
                                // Use our own verifier to support SIF Level 1 Authentication
                                options.Verifier =
                                    new CertVerifyEventHandler( verifyLevel1Authentication );
                                break;
                            case 2:
                                // Use our own verifier to support SIF Level 2 Authentication
                                options.Verifier =
                                    new CertVerifyEventHandler( verifyLevel2Authentication );
                                break;
                            case 3:
                                // Use our own verifier to support SIF Level 3 Authentication
                                options.Verifier =
                                    new CertVerifyEventHandler( verifyLevel3Authentication );
                                break;
                        }
                    }

                    // TODO: Remove org.mentalis.security and switch to .NET
                    // Add support for setting the allowed types and ciphers


                    //            if( fProps.getProtocol().equalsIgnoreCase("https") )
                    //            {
                    //                String allowedCiphers = fProps.getProperty( "ciphers" );
                    //                if ( allowedCiphers != null && allowedCiphers.length() > 0 )
                    //                {
                    //                    log.debug( "Setting the set of allowed ciphers to " + allowedCiphers );
                    //                    String[] allowed = allowedCiphers.split( "," );
                    //
                    //                    SunJsseListener jsse = (SunJsseListener) newListener;
                    //                    SSLServerSocket socket = (SSLServerSocket) jsse.getServerSocket();
                    //
                    //                    List<String> ciphers = new ArrayList<String>();
                    //                    for ( String cipher : socket.getEnabledCipherSuites() )
                    //                    {
                    //                        if ( Arrays.binarySearch( allowed, cipher ) < 0 )
                    //                        {
                    //                            log.debug( "Disabling cipher: " + cipher );
                    //                        }
                    //                        else
                    //                        {
                    //                            log.debug( "Enabling cipher: " + cipher );
                    //                            ciphers.add( cipher );
                    //                        }
                    //                    }
                    //
                    //                    String[] enabled = new String[ciphers.size()];
                    //                    ciphers.toArray( enabled );
                    //                    socket.setEnabledCipherSuites( enabled );
                    //
                    //                    //				for( String pro : socket.getEnabledProtocols() ){
                    //                    //					System.out.println( pro );
                    //                    //				}
                    //                    //				
                    //                    for ( String cipher : socket.getEnabledCipherSuites() )
                    //                    {
                    //                        log.debug( cipher + " is enabled for this session." );
                    //                    }
                    //                }
                    //            }


                    listener = sServer.CreateHttpsListener( options );
                    ConfigureSocketListener( listener, zone, port, hostAddress );
                    return listener;
                }
                catch ( AdkTransportException )
                {
                    throw;
                }
                catch ( Exception ioe )
                {
                    throw new AdkTransportException
                        ( "Error configuring HTTPS transport: " + ioe );
                }
            }
            else
            {
                if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 )
                {
                    if ( hostAddress != null )
                    {
                        log.Debug( "Already a HTTPS listener on " + hostAddress + ":" + port );
                    }
                    else
                    {
                        log.Debug( "Already a HTTPS listener on port " + port );
                    }
                }
            }

            return null;
        }


        private void verifyLevel1Authentication( SecureSocket socket,
                                                 Certificate cert,
                                                 CertificateChain chain,
                                                 VerifyEventArgs e
            )
        {
            if ( cert == null )
            {
                if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 )
                {
                    log.Warn( "Client Certificate is missing and fails SIF Level 1 Authentication" );
                }
                e.Valid = false;
            }
            else if ( !cert.IsCurrent )
            {
                if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 )
                {
                    log.Warn( "Client Certificate is invalid and fails SIF Level 1 Authentication" );
                }
                e.Valid = false;
            }
            else
            {
                e.Valid = true;
            }
        }


        private void verifyLevel2Authentication( SecureSocket socket,
                                                 Certificate cert,
                                                 CertificateChain chain,
                                                 VerifyEventArgs e
            )
        {
            // Verify level 1 first
            verifyLevel1Authentication( socket, cert, chain, e );
            if ( !e.Valid )
            {
                return;
            }

            CertificateStatus certStatus =
                chain.VerifyChain( null, AuthType.Client, VerificationFlags.IgnoreInvalidName );
            if ( certStatus != CertificateStatus.ValidCertificate )
            {
                if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 )
                {
                    log.Warn
                        ( "Client Certificate is not trusted and fails SIF Level 2 Authentication: " +
                          certStatus.ToString() );
                }
                e.Valid = false;
            }
            else
            {
                e.Valid = true;
            }
        }

        private const string OID_CN = "2.5.4.3";

        private void verifyLevel3Authentication( SecureSocket socket,
                                                 Certificate cert,
                                                 CertificateChain chain,
                                                 VerifyEventArgs e
            )
        {
            try
            {
                // Verify level 2 first
                verifyLevel2Authentication( socket, cert, chain, e );
                if ( !e.Valid )
                {
                    return;
                }

                // Verify that the host name or IP matches the subject on the certificate
                // ( Level3 authentication )
                // First, get the "CN=" name from the certificate
                string commonName = null;
                DistinguishedName certificateName = cert.GetDistinguishedName();
                for ( int a = 0; a < certificateName.Count; a++ )
                {
                    NameAttribute part = certificateName[a];
                    if ( part.ObjectID == OID_CN )
                    {
                        commonName = part.Value;
                        break;
                    }
                }
                if ( commonName == null )
                {
                    if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 )
                    {
                        log.Warn
                            ( "Client Certificate fails SIF Level 3 Authentication: common name attribute not found." );
                    }
                    e.Valid = false;
                    return;
                }

                if( String.Compare( commonName, "localhost", true ) == 0 )
                {
                    commonName = "127.0.0.1";
                }

                // Does it match the IP Address?
                IPEndPoint remoteEndPoint = (IPEndPoint) socket.RemoteEndPoint;
                if ( remoteEndPoint.Address.ToString() == commonName )
                {
                    e.Valid = true;
                    return;
                }

                // Does it match the common name of the client machine?
                IPHostEntry entry = GetHostByAddress( remoteEndPoint.Address );
                if ( entry == null )
                {
                    if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 )
                    {
                        log.Warn
                            ( "Client Certificate fails SIF Level 3 Authentication: Host Name not found for Address " +
                              remoteEndPoint.Address.ToString() );
                    }
                    e.Valid = false;
                    return;
                }

                if ( string.Compare( commonName, entry.HostName, true ) == 0 )
                {
                    e.Valid = true;
                    return;
                }

                // No match was found
                e.Valid = false;
                if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 )
                {
                    log.Warn
                        ( "Client Certificate fails SIF Level 3 Authentication: Certificate Common Name=" +
                          commonName + ". Does not match client IP / Host: " +
                          remoteEndPoint.Address.ToString() + " / " + socket.CommonName );
                }
            }
            catch ( Exception ex )
            {
                if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 )
                {
                    log.Warn
                        ( "Client Certificate fails SIF Level 3 Authentication: " + ex.Message, ex );
                }
                e.Valid = false;
            }
        }


        private IPHostEntry GetHostByAddress( IPAddress address )
        {
            if ( fCachedLookup == null || !fCachedLookup.LookupAddress.Equals( address ) )
            {
                // If name resolution fails, this will throw an exception
                IPHostEntry entry = Dns.GetHostEntry( address );
                fCachedLookup = new CachedHostLookup( address, entry );
            }
            return fCachedLookup.Entry;
        }

        /// <summary>
        /// The last host entry retrieved by IP Address is cached here. It might be wise
        /// to implement a more complex cache if this becomes a performance point
        /// </summary>
        private CachedHostLookup fCachedLookup;

        private class CachedHostLookup
        {
            private IPAddress fLookupAddress;
            private IPHostEntry fEntry;

            public CachedHostLookup( IPAddress lookupAddress,
                                     IPHostEntry entry )
            {
                fLookupAddress = lookupAddress;
                fEntry = entry;
            }

            public IPAddress LookupAddress
            {
                get { return fLookupAddress; }
            }

            public IPHostEntry Entry
            {
                get { return fEntry; }
            }
        }


        public override IProtocolHandler CreateProtocolHandler( AgentMessagingMode mode )
        {
            if ( mode == AgentMessagingMode.Pull )
            {
                return new HttpPullProtocolHandler( this );
            }
            else
            {
                if ( sServer == null )
                {
                    throw new AdkException( "HttpTransport is not Activated", null );
                }
                return new AdkHttpPushProtocolHandler( (AdkHttpApplicationServer) sServer, this );
            }
        }


        /// <summary>
        /// Returns the class that will verify server certificates
        /// </summary>
        /// <returns></returns>
        public ICertificatePolicy GetServerCertificatePolicy()
        {
            return this;
        }

        #region ICertificatePolicy Members

        /// <summary>
        /// Called by the .Net framework when it is initiating an SSL connection. Allows the client
        /// to examine the certificate and verify whether it should be used or not.
        /// </summary>
        /// <param name="srvPoint">The service point representing the server</param>
        /// <param name="certificate">The certificate received from the server</param>
        /// <param name="request">The request that initiated the connection</param>
        /// <param name="certificateProblem">The problem, if any that the cryptography subsystem uncovered, or zero</param>
        /// <returns>True if the certificate is validated, otherwise false</returns>
        bool ICertificatePolicy.CheckValidationResult(
            ServicePoint srvPoint,
            X509Certificate certificate,
            WebRequest request,
            int certificateProblem )
        {
            // The .Net ADK uses the same validation as the default .Net framework certificate policy. If other
            // policy requirements become necessary for the SIF Specification or special situations, they can be
            // implemented here.
            if ( certificateProblem == 0 )
            {
                return true;
            }
            else
            {
                if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 )
                {
                    if ( log.IsDebugEnabled )
                    {
                        log.Debug
                            ( string.Format
                                  ( "Certificate is being rejected for reason {0} : {1}",
                                    certificateProblem, certificate.ToString( true ) ) );
                    }
                }
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Gets the certificate that should be used for server authentication ( SSL )
        /// </summary>
        /// <remarks>
        /// This method will throw an exception if the configuration properties used to
        /// look for the certificate are invalid. However, if the configuration is valid
        /// and no certificate is found, null will be returned and it is up to the caller
        /// to determine if an exception should be thrown in that case.
        /// </remarks>
        /// <exception cref="AdkTransportException">Thrown if the configuration properties used
        /// to look for the certificate are invalid</exception>
        /// <returns>The certificate found or <c>null</c></returns>
        public Certificate GetServerAuthenticationCertificate()
        {
            HttpsProperties props = (HttpsProperties) fProps;
            Certificate cert =
                GetCertificateFromStore( OID_SERVER_AUTHENTICATION, props.SSLCertName );
            return cert;
        }

        /// <summary>
        /// Gets the certificate that should be used for client authentication
        /// </summary>
        /// <remarks>
        /// This method will throw an exception if the configuration properties used to
        /// look for the certificate are invalid. However, if the configuration is valid
        /// and no certificate is found, null will be returned and it is up to the caller
        /// to determine if an exception should be thrown in that case.
        /// </remarks>
        /// <exception cref="AdkTransportException">Thrown if the configuration properties used
        /// to look for the certificate are invalid or a certificate is found but
        /// it's certificate chain is invalid</exception>
        /// <returns>The certificate found or <c>null</c></returns>
        public Certificate GetClientAuthenticationCertificate()
        {
            HttpsProperties props = (HttpsProperties) fProps;
            Certificate cert =
                GetCertificateFromStore( OID_CLIENT_AUTHENTICATION, props.ClientCertName );
            if ( cert == null )
            {
                return null;
            }

            DebugTransport
                ( "Using this certificate for Client Authentication: {0}", cert.ToString( true ) );

            // ANDY E 07/26/2005 Removed the following code that verifies the chain of the certificate.
            // The reason is that the certificate doesn't really need to be trusted on this machine,
            // only on the machine that is receiving the certificate. Enabling the trust check here
            // makes configuration more difficult, and doesn't really help anything. If this code is 
            // uncommented there will have to be more documentation added to the HTTPS documentation to
            // tell what all needs to be there for client certificates to be accepted by the ADK, especially
            // and specifically when an agent is running as a service.
            //			CertificateStatus status = cert.GetCertificateChain().VerifyChain( null, AuthType.Client );
            //
            //			if ( status != CertificateStatus.ValidCertificate )
            //			{
            //				log.Warn( "Certificate selected for client authentication is not valid: " + status.ToString() );
            //				return null;
            //			}

            return cert;
        }


        /// <summary>
        /// Returns the system Certificate Store to retrieve certificates from
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        private CertificateStore GetSystemStore( HttpsProperties props )
        {
            string certStoreLocation = props.CertStoreLocation;
            StoreLocation loc;
            if ( certStoreLocation == null )
            {
                loc = StoreLocation.CurrentUser;
            }
            else
            {
                try
                {
                    loc =
                        (StoreLocation)
                        Enum.Parse( typeof ( StoreLocation ), certStoreLocation, true );
                }
                catch
                {
                    throw new AdkTransportException
                        ( "Invalid CertificateStore location: " + certStoreLocation, null );
                }
            }

            string certStoreName = props.CertStore;
            if ( certStoreName == null )
            {
                certStoreName = CertificateStore.MyStore;
            }

            DebugTransport( "Using Certificate store {0} / {1}", loc, certStoreName );

            return new CertificateStore( loc, certStoreName );
        }

        private Certificate GetCertificateFromStore( string oid,
                                                     string certName )
        {
            HttpsProperties props = (HttpsProperties) fProps;

            // First, look for a file-based certificate, if specified in the props
            CertificateStore certStore = null;
            string certFile = props.SSLCertFile;
            if ( certFile != null )
            {
                FileInfo info = new FileInfo( certFile );
                if ( info.Exists )
                {
                    if ( info.Extension == ".pfx" )
                    {
                        string cfp = props.SSLCertFilePassword;
                        certStore = CertificateStore.CreateFromPfxFile( info.FullName, cfp );
                        DebugTransport( "Using certificate file '{0}'", info.FullName );
                    }
                    else
                    {
                        throw new AdkTransportException
                            ( "Certificate file must be in the .PFX format", null );
                    }
                }
                else
                {
                    throw new FileNotFoundException
                        ( "Unable to locate specified certificate file: " + certFile, certFile );
                }
            }

            if ( certStore == null )
            {
                certStore = GetSystemStore( props );
            }

            Certificate cert = null;
            if ( certName != null )
            {
                cert = certStore.FindCertificateBySubjectName( certName );
            }
            else
            {
                // Find the first applicable certificate
                foreach ( Certificate c in certStore.EnumCertificates() )
                {
                    if ( !c.HasPrivateKey() )
                    {
                        DebugTransport
                            ( "Ignoring Certificate {0} because it has no private key",
                              c.ToString( true ) );
                        continue;
                    }
                    if ( !c.SupportsDataEncryption )
                    {
                        DebugTransport
                            ( "Ignoring Certificate {0} because it doesn't support data encryption",
                              c.ToString( true ) );
                        continue;
                    }
                    if ( c.GetEffectiveDate() > DateTime.Now )
                    {
                        DebugTransport
                            ( "Ignoring Certificate {0} because the effective date is in the future.",
                              c.ToString( true ) );
                        continue;
                    }
                    if ( c.GetExpirationDate() < DateTime.Now )
                    {
                        DebugTransport
                            ( "Ignoring Certificate {0} because it has expired", c.ToString( true ) );
                        continue;
                    }
                    StringCollection enhancedUsages = c.GetEnhancedKeyUsage();
                    if ( enhancedUsages.Count > 0 && !enhancedUsages.Contains( oid ) )
                    {
                        DebugTransport
                            ( "Ignoring Certificate {0} because it has an enhanced key usage attribute, that doesn't include {1}",
                              c.ToString( true ), oid );
                        continue;
                    }
                    cert = c;
                    break;
                }
            }

            return cert;
        }

        public void DebugTransport( string message,
                                    params object[] args )
        {
            if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 && log.IsDebugEnabled )
            {
                log.Debug( string.Format( message, args ) );
            }
        }


        /// <summary>
        /// The HTtp server that is currently in scope for the push zone
        /// </summary>
        public IHttpServer Server
        {
            get { return sServer; }
        }

        #region Private Fields

        private static IHttpServer sServer;

        #endregion

        public void ConfigureSIF_Protocol( SIF_Protocol proto, IZone zone )
        {
            proto.Type = Protocol.ToUpperInvariant();
            proto.SetSecure( Secure ? YesNo.YES : YesNo.NO );

            String host = getPushHostName();

            int port = ((HttpProperties) fProps).PushPort;
            if ( port == -1 )
            {
                port = this.Port;
            }

            UriBuilder builder = new UriBuilder();
            builder.Scheme = this.Protocol;
            builder.Host = host.Trim();
            builder.Port = port;
            builder.Path = "/zone/" + zone.ZoneId + "/";
            proto.SIF_URL = builder.Uri.AbsoluteUri;
        }
    }
}
