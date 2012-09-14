//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using OpenADK.Library.Infra;
using OpenADK.Util;
using Org.Mentalis.Security.Certificates;

namespace OpenADK.Library.Impl
{
    /// <summary>
    /// Summary description for BaseHttpProtocolHandler.
    /// </summary>
    internal abstract class BaseHttpProtocolHandler : IProtocolHandler
    {

        #region Private Members

        private string fHttpUserAgent;
        private bool fKeepAliveOnSend;
        private Uri fZoneUrl;
        private ZoneImpl fZone;
        protected readonly HttpTransport fTransport;

        private X509Certificate fClientAuthCertificate;
        private bool fSSLInitialized;


        protected BaseHttpProtocolHandler(HttpTransport transport)
        {
            fTransport = transport;
        }


        #endregion


        #region IProtocolHandler Members

        public virtual string Name
        {
            get { return fZone.Agent.Id + "@" + fZone.ZoneId + ".HttpProtocolHandler"; }
        }


        /// <summary>  Initialize the protocol handler for a zone</summary>
        public virtual void Open( ZoneImpl zone )
        {
            fZone = zone;


            fKeepAliveOnSend = ((HttpProperties) fTransport.Properties).KeepAliveOnSend;

            try {
                //  Ensure the ZIS URL is http/https
                fZoneUrl = fZone.ZoneUrl;
                string check = fZoneUrl.Scheme.ToLower();
                if ( !check.Equals( "http" ) && !check.Equals( "https" ) ) {
                    throw new AdkException
                        ( "HttpProtocolHandler cannot handle URL: " + fZone.ZoneUrl, fZone );
                }

                //  Prepare headers later used to send messages
                fHttpUserAgent = fZone.Agent.Id + " (Adk/" + Adk.AdkVersion + ")";
            }
            catch ( Exception thr ) {
                throw new AdkException
                    ( "HttpProtocolHandler could not parse URL \"" + fZone.ZoneUrl + "\": " + thr,
                      fZone );
            }
        }


        public abstract void Close( IZone zone );

        public abstract void Start();

        public abstract void Shutdown();

        /// <summary>  Sends a SIF infrastructure message and returns the response.</summary>
        /// <remarks>
        /// The message content should consist of a complete <SIF_Message> element.
        /// This method sends whatever content is passed to it without any checking
        /// or validation of any kind.
        /// </remarks>
        /// <param name="msg">The message content</param>
        /// <returns> The response from the ZIS (expected to be a <SIF_Ack> message)
        /// </returns>
        /// <exception cref="AdkMessagingException"> is thrown if there is an error sending
        /// the message to the Zone Integration Server
        /// </exception>
        public IMessageInputStream Send( IMessageOutputStream msg )
        {
            try {
                return TrySend( msg );
            }
            catch ( WebException webEx ) {
                if ( webEx.Status == WebExceptionStatus.ConnectionClosed ) {
                    // Try one more time, the underlying keep-alive connection must have
                    // been closed by the ZIS. Trying again will start with a fresh,
                    // new connection
                    try {
                        return TrySend( msg );
                    }
                    catch ( AdkException ) {
                        throw;
                    }
                    catch ( Exception ex ) {
                        throw new AdkMessagingException
                            ( "HttpProtocolHandler: Unexpected error sending message retry: " + ex,
                              fZone );
                    }
                }
                else {
                    // This code should never be hit because TrySend() should never emit this exception
                    // Leaving the code here for defensive purposes
                    throw new AdkMessagingException
                        ( "HttpProtocolHandler: Unexpected error sending message: " + webEx, fZone );
                }
            }
        }

        /// <summary>
        /// Returns true if the protocol and underlying transport are currently active
        /// for this zone
        /// </summary>
        /// <param name="zone"></param>
        /// <returns>True if the protocol handler and transport are active</returns>
        public abstract bool IsActive( ZoneImpl zone );

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
        public abstract SIF_Protocol MakeSIF_Protocol( IZone zone );


        private IMessageInputStream TrySend( IMessageOutputStream msg )
        {
            MessageStreamImpl returnStream;
            Stream reqStream;

            HttpWebRequest conn = GetConnection( fZoneUrl );
            conn.ContentLength = msg.Length;

            try {
                reqStream = conn.GetRequestStream();
            }
            catch ( WebException webEx ) {
                if ( webEx.Status == WebExceptionStatus.ConnectionClosed ) {
                    // This could be a keep-alive connection that was closed unexpectedly
                    // rethrow so that retry handling can take affect
                    throw;
                }
                else {
                    throw new AdkTransportException
                        ( "Could not establish a connection to the ZIS (" + fZoneUrl.AbsoluteUri +
                          "): " + webEx, fZone, webEx );
                }
            }
            catch ( Exception thr ) {
                throw new AdkTransportException
                    ( "Could not establish a connection to the ZIS (" + fZoneUrl.AbsoluteUri + "): " +
                      thr, fZone, thr );
            }

            try {
                if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 ) {
                    fZone.Log.Debug( "Sending message (" + msg.Length + " bytes)" );
                }
                if ( (Adk.Debug & AdkDebugFlags.Message_Content) != 0 ) {
                    fZone.Log.Debug( msg.Decode() );
                }
                try {
                    msg.CopyTo( reqStream );
                    reqStream.Flush();
                    reqStream.Close();
                }
                catch ( Exception thr ) {
                    throw new AdkMessagingException
                        ( "HttpProtocolHandler: Unexpected error sending message: " + thr, fZone );
                }

                try {
                    using ( WebResponse response = conn.GetResponse() ) {
                        if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 ) {
                            fZone.Log.Debug
                                ( "Expecting reply (" + response.ContentLength + " bytes)" );
                        }

                        returnStream = new MessageStreamImpl( response.GetResponseStream() );

                        response.Close();

                        if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 ) {
                            fZone.Log.Debug( "Received reply (" + returnStream.Length + " bytes)" );
                        }
                        if ( (Adk.Debug & AdkDebugFlags.Message_Content) != 0 ) {
                            fZone.Log.Debug( returnStream.Decode() );
                        }
                    }
                }
                catch ( Exception thr ) {
                    throw new AdkTransportException
                        ( "An unexpected error occurred while receiving data from the ZIS: " + thr,
                          fZone );
                }
            }
            catch ( AdkException ) {
                // rethrow anything that's already wrapped in an AdkException
                throw;
            }
            catch ( Exception thr ) {
                throw new AdkMessagingException
                    ( "HttpProtocolHandler: Error receiving response to sent message: " + thr, fZone );
            }

            return returnStream;
        }

        #endregion

        #region Protected Members

        protected internal ZoneImpl Zone
        {
            get { return fZone; }
        }


        /// <summary>  Get an outbound connection to the ZIS</summary>
        /// <returns> Either an HttpsURLConnection or an HttpURLConnection depending
        /// on whether the associated transport protocol is secure or not
        /// </returns>
        protected HttpWebRequest GetConnection( Uri uri )
        {
            try {
                HttpWebRequest conn = (HttpWebRequest) WebRequest.Create( uri );
                conn.Method = "POST";
                conn.ContentType = SifIOFormatter.CONTENTTYPE;
                conn.UserAgent = fHttpUserAgent;
                conn.KeepAlive = fKeepAliveOnSend;

                // If the transport is an HTTPS transport, attempt to set an SSL
                // client certificate
                if ( fTransport.Secure ) {
                    ApplySSLAttributes( conn );
                }

                return conn;
            }
            catch ( WebException webEx ) {
                throw new AdkTransportException
                    ( "Failed to create HttpWebRequest " + fZoneUrl.AbsoluteUri + ": " + webEx, fZone,
                      webEx );
            }
        }


        /// <summary>
        /// Retrieves a certificate to use for client authentication, if available and 
        /// adds it to the client certificate collection of the HttpWebRequest.
        /// </summary>
        /// <param name="conn"></param>
        protected void ApplySSLAttributes( HttpWebRequest conn )
        {
            if ( !fSSLInitialized ) {
                fSSLInitialized = true;
                    Certificate cert = fTransport.GetClientAuthenticationCertificate();

                    if ( cert == null )
                    {
                        fTransport.DebugTransport
                            ( "No certificate found for client authentication", new object[0] );
                    }
                    else
                    {
                        fClientAuthCertificate = cert.ToX509();
                        ServicePointManager.CertificatePolicy = fTransport.GetServerCertificatePolicy();
                    }
            }

            if ( fClientAuthCertificate != null ) {
                conn.ClientCertificates.Add( fClientAuthCertificate );
            }
        }


        protected SifParser CreateParser()
        {
            return SifParser.NewInstance();
        }

        #endregion

    }
}

// Synchronized with Library-ADK-1.5.0.Version_5.SIFPrimitives.java
