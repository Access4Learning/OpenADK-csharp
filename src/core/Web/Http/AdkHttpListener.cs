//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Specialized;
using OpenADK.Library;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for AdkHttpListener.
    /// </summary>
    public class AdkHttpListener
    {
        private bool fKeepAlivesEnabled = true;
        private IDictionary fHandlerContexts = new HybridDictionary( true );
        private AdkHttpServer fServer;
        private IAdkHttpHandler fAnonymousHandler;

        public AdkHttpListener( AdkHttpServer server )
        {
            fServer = server;
        }

        public void Attach( AdkSocketBinding server )
        {
            server.DataReceived += new AdkSocketMessageHandler( HandleSocketMessage );
        }

        public void Detach( AdkSocketBinding server )
        {
            server.DataReceived -= new AdkSocketMessageHandler( HandleSocketMessage );
        }

        protected void HandleSocketMessage( AdkSocketConnection socketConnection )
        {
            AdkHttpConnection.ProcessRequest( socketConnection, this );
        }

        internal IAdkHttpHandler GetHandlerForContext( AdkHttpRequest request )
        {
            IAdkHttpHandlerFactory factory = this.SearchForContextHandler( request.Url );
            if ( factory != null ) {
                return factory.CreateHandler( request );
            }
            else {
                return this.AnonymousHandler;
            }
        }


        public AdkHttpServer Server
        {
            get { return fServer; }
        }

        /// <summary>
        /// Adds a handler to the list of handlers associated with this listener
        /// </summary>
        /// <param name="hostName">The host name to use for request resolution( not currently used )</param>
        /// <param name="virtualPath">The virtual path e.g. "/virdir1"</param>
        /// <param name="factory">The object responsible for creating new handlers for requests</param>
        /// <param name="force">If set to true, the handler will replace any other handlers defined at the path, if false, an exception will
        /// be thrown if any handlers are already defined at the path</param>
        public void AddHandlerContext( string hostName,
                                       string virtualPath,
                                       IAdkHttpHandlerFactory factory,
                                       bool force )
        {
            // We don't support virtual hosts in this version. Look up the context by the virtual path only
            string contextPath = BuildContextString( virtualPath );
            if ( fHandlerContexts.Contains( contextPath ) && !force ) {
                throw new AdkException( "Handler is already defined for " + virtualPath, null );
            }
            fHandlerContexts[contextPath] = factory;
        }

        public IAdkHttpHandler AnonymousHandler
        {
            get
            {
                if ( fAnonymousHandler == null ) {
                    fAnonymousHandler = new AnonymousPathHandler();
                }
                return fAnonymousHandler;
            }

            set { fAnonymousHandler = value; }
        }

        public void RemoveHandlerContext( string hostName,
                                          string virtualPath )
        {
            fHandlerContexts.Remove( BuildContextString( virtualPath ) );
        }

        public IAdkHttpHandlerFactory SearchForContextHandler( Uri requestUri )
        {
            string searchPath = requestUri.AbsolutePath;
            if ( searchPath.IndexOf( '.' ) == -1 && !searchPath.EndsWith( "/" ) ) {
                searchPath = searchPath + "/";
            }
            while ( searchPath.Length > 0 ) {
                searchPath = searchPath.Substring( 0, searchPath.LastIndexOf( '/' ) );
                IAdkHttpHandlerFactory factory =
                    (IAdkHttpHandlerFactory) fHandlerContexts[searchPath];
                if ( factory != null ) {
                    return factory;
                }
            }
            return null;
        }

        public bool HasHandlerContext( string hostName,
                                       String virtualPath )
        {
            string contextPath = BuildContextString( virtualPath );
            return fHandlerContexts.Contains( contextPath );
        }

        public bool KeepAlivesEnabled
        {
            get { return fKeepAlivesEnabled; }
            set { fKeepAlivesEnabled = value; }
        }

        private string BuildContextString( string virtualPath )
        {
            while ( virtualPath.EndsWith( "/" ) ) {
                virtualPath = virtualPath.Substring( 0, virtualPath.Length - 1 );
            }
            return virtualPath;
        }


        private class AnonymousPathHandler : IAdkHttpHandler
        {
            #region IAdkHttpHandler Members

            public void ProcessRequest( AdkHttpRequestContext context )
            {
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ClientError_403_Forbidden,
                      "No application configured at this URL (" + context.Request.Url.ToString() +
                      ")" );
            }

            #endregion
        }
    }
}
