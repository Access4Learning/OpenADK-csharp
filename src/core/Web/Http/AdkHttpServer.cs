//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Net;
using log4net;
using Org.Mentalis.Security.Ssl;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpHost.
    /// </summary>
    public class AdkHttpServer
    {
        private ILog fLog;
        private string fServerName;
        private ArrayList fBindings = new ArrayList();
        private AdkHttpListener fListener;
        private bool fIsStarted = false;

        protected AdkHttpServer()
        {
            Type type = this.GetType();
            fServerName = "OpenADK Library ADK(r); Version " +
                          type.Assembly.GetName().Version.ToString();
            // Default the log
            fLog = LogManager.GetLogger( this.GetType() );
            fListener = new AdkHttpListener( this );
        }

        public virtual string Name
        {
            get { return fServerName; }
        }

        public virtual AdkHttpRequestContext CreateContext( AdkHttpConnection connection,
                                                            AdkHttpRequest request,
                                                            AdkHttpResponse response )
        {
            return new AdkHttpRequestContext( connection, request, response, this );
        }

        /// <summary>
        /// Adds a handler for a virtual directory
        /// </summary>
        /// <param name="hostName">The host name to use for this handler ( currently only "" is supported )</param>
        /// <param name="virtualPath">The virtual path to respond to requests on</param>
        /// <param name="contextHandler">The handler that should respond to requests in this path</param>
        /// <param name="force">True if an existing handler should be replaced by this one</param>
        public void AddHandlerContext( string hostName,
                                       string virtualPath,
                                       IAdkHttpHandlerFactory contextHandler,
                                       bool force )
        {
            fListener.AddHandlerContext( hostName, virtualPath, contextHandler, force );
        }

        /// <summary>
        /// Removes a context handler for the specified virtual path
        /// </summary>
        /// <param name="hostname">The host name to use for this handler ( currently only "" is supported )</param>
        /// <param name="virtualPath">The virtual path</param>
        public void RemoveHandlerContext( string hostname,
                                          string virtualPath )
        {
            fListener.RemoveHandlerContext( hostname, virtualPath );
        }

        public virtual void AddListener( AdkSocketBinding binding )
        {
            lock ( fBindings.SyncRoot ) {
                if ( GetListener( binding.Port ) != null ) {
                    throw new ArgumentException
                        ( string.Format( "Port {0} is already in use", binding.Port ) );
                }

                fListener.Attach( binding );
                fBindings.Add( binding );
            }
        }

        public virtual AdkSocketBinding CreateHttpListener()
        {
            AdkSocketBinding binding =
                new AdkSocketBinding( new AdkDefaultAcceptSocket(), this.Log );
            return binding;
        }

        /// <summary>
        /// Creates an HTTPS socket binding to the specified port
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual AdkSocketBinding CreateHttpsListener( SecurityOptions options )
        {
            AdkSSLAcceptSocket socket = new AdkSSLAcceptSocket( options );
            AdkSocketBinding binding = new AdkSocketBinding( socket );
            return binding;
        }


        public AdkSocketBinding GetListener( int port )
        {
            lock ( fBindings.SyncRoot ) {
                foreach ( AdkSocketBinding binding in fBindings ) {
                    if ( binding.Port == port ) {
                        return binding;
                    }
                }
            }
            return null;
        }

        protected void RemoveBinding( int port )
        {
            lock ( fBindings.SyncRoot ) {
                AdkSocketBinding server = GetListener( port );
                if ( server != null ) {
                    server.Stop();
                    fBindings.Remove( server );
                }
            }
        }

        protected AdkHttpListener Listener
        {
            get { return fListener; }
        }


        protected AdkSocketBinding [] GetPortBindings()
        {
            lock ( fBindings.SyncRoot ) {
                AdkSocketBinding [] bindings = new AdkSocketBinding[fBindings.Count];
                fBindings.CopyTo( bindings );
                return bindings;
            }
        }

        protected void StartServer()
        {
            lock ( fBindings.SyncRoot ) {
                if ( !IsStarted ) {
                    foreach ( AdkSocketBinding server in fBindings ) {
                        try {
                            server.Start();
                        }
                        catch ( Exception ex ) {
                            this.Error( ex.Message, ex );
                        }
                    }
                    fIsStarted = true;
                }
            }
        }

        public bool IsStarted
        {
            get { return fIsStarted; }
        }

        protected void StopServer( bool clearAllListeners )
        {
            lock ( fBindings.SyncRoot ) {
                if ( IsStarted ) {
                    foreach ( AdkSocketBinding server in fBindings ) {
                        try {
                            server.Stop();
                        }
                        catch ( Exception ex ) {
                            this.Error( ex.Message, ex );
                        }
                    }
                    if( clearAllListeners )
                    {
                        fBindings.Clear();
                    }

                    fIsStarted = false;
                }
            }
        }

        public ILog Log
        {
            get { return fLog; }
            set { fLog = value; }
        }

        public void Error( string message,
                           Exception ex )
        {
            if ( fLog != null && fLog.IsErrorEnabled ) {
                fLog.Error( message, ex );
            }
        }
    }
}
