//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Net;
using OpenADK.Library;
using OpenADK.Library.Impl;
using log4net;
using Org.Mentalis.Security.Certificates;
using Org.Mentalis.Security.Ssl;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpServer.
    /// </summary>
    public class AdkHttpApplicationServer : AdkHttpServer, IHttpServer
    {


        private HttpTransport fTransport;

        /// <summary>
        /// Creates an instance of an AdkHttpApplicationServer
        /// </summary>
        /// <param name="transport">The Http transport</param>
        public AdkHttpApplicationServer( HttpTransport transport )
        {
            fTransport = transport;
        }

        /// <summary>
        /// Adds a handler for a specific virtual path
        /// </summary>
        /// <param name="hostName">The host name to use for request resolution (Virtual hosts are not currently supported)</param>
        /// <param name="virtualPath">The virtual path that will be routed to this handler ( e.g "/virdir1"</param>
        /// <param name="handler">The handler that will handle requests on this path</param>
        /// <param name="force">If set to true, the handler will replace any other handlers defined at the path, if false, an exception will
        /// be thrown if any handlers are already defined at the path</param>
        public void AddHandlerContext( string hostName,
                                       string virtualPath,
                                       IAdkHttpHandler handler,
                                       bool force )
        {
            AdkHttpHandlerListeningContext contextHandler =
                new AdkHttpHandlerListeningContext( handler );
            this.AddHandlerContext( hostName, virtualPath, contextHandler, force );
        }

        public void RemoveHandlerContext( string virtualPath )
        {
            this.RemoveHandlerContext( "", virtualPath );
        }

        public void SetAnonymousHandler( IAdkHttpHandler handler )
        {
            this.Listener.AnonymousHandler = handler;
        }

        private class AdkHttpHandlerListeningContext : IAdkHttpHandlerFactory
        {
            public AdkHttpHandlerListeningContext( IAdkHttpHandler handler )
            {
                fHandler = handler;
            }

            IAdkHttpHandler IAdkHttpHandlerFactory.CreateHandler( AdkHttpRequest request )
            {
                return fHandler;
            }

            private IAdkHttpHandler fHandler;
        }

        #region IHttpServer Members

        void IHttpServer.Start()
        {
            lock ( this ) {
                if ( !this.IsStarted ) {

                    base.StartServer();
                }
            }
        }

        public void Shutdown()
        {
            base.StopServer( false );
        }

        public void Shutdown( bool clearAllListeners )
        {
            if( clearAllListeners )
            base.StopServer( clearAllListeners );
        }



        #endregion

    }
}
