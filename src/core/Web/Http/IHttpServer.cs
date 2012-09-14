//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Net;
using OpenADK.Library;
using log4net;
using Org.Mentalis.Security.Ssl;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// The generic interface for describing an Http server to the ADK framework
    /// </summary>
    public interface IHttpServer
    {
        /// <summary>
        /// Starts the server
        /// </summary>
        void Start();

        /// <summary>
        /// Shuts down the server
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Shuts down the server, optionally removing all socket listeners
        /// </summary>
        void Shutdown( bool clearAllListeners );

        /// <summary>
        /// Returns true if the server is started
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Gets a socket listener at the specified port, if one exists
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        AdkSocketBinding GetListener( int port );
        /// <summary>
        /// Creates a new socket listener for HTTP
        /// </summary>
        /// <returns></returns>
        AdkSocketBinding CreateHttpListener();
        /// <summary>
        /// Creates a new socket listener for HTTPS
        /// </summary>
        /// <param name="options">Options for the secure socket</param>
        /// <returns></returns>
        AdkSocketBinding CreateHttpsListener( SecurityOptions options );

        /// <summary>
        /// Adds a new Socket Listener
        /// </summary>
        /// <param name="listener"></param>
        void AddListener( AdkSocketBinding listener );
    }
}
