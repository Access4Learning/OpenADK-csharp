//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Net;

namespace OpenADK.Web
{
    /// <summary>
    /// A standard interface definition for a socket. This allows standard sockets and SSL Sockets to be used
    /// transparently.
    /// </summary>
    public interface IAcceptSocket
    {
        /// <summary>
        /// Closes the socket
        /// </summary>
        void Close();

        /// <summary>
        /// Accepts the current socket request and creates a socket connection asynchronously
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        void BeginAccept( AsyncCallback callback,
                          object state );

        /// <summary>
        /// The end of the asynchronous <see cref="IAcceptSocket.BeginAccept"/> call
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        IConnectedSocket EndAccept( IAsyncResult result );

        /// <summary>
        /// Starts listening for socket requests on the IPEndpoint
        /// </summary>
        /// <param name="endPoint"></param>
        void Bind( IPEndPoint endPoint );
    }
}
