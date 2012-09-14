//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace OpenADK.Web
{
    /// <summary>
    /// Summary description for IConnectedSocket.
    /// </summary>
    public interface IConnectedSocket
    {
        /// <summary>
        /// Returns true if the socket is still connected
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Sets socket options ( See the Winsock API for more information )
        /// </summary>
        /// <param name="level"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        void SetSocketOption( SocketOptionLevel level,
                              SocketOptionName name,
                              int val );

        /// <summary>
        /// Closes the current socket
        /// </summary>
        void Close();

        /// <summary>
        /// Shuts the socket down
        /// </summary>
        /// <param name="shutDownType"></param>
        void Shutdown( SocketShutdown shutDownType );

        /// <summary>
        /// Returns the local endpoint that the socket is connected to
        /// </summary>
        EndPoint LocalEndPoint { get; }

        /// <summary>
        /// Returns the remote endpoint that the socket is connected to
        /// </summary>
        EndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Create a stream for communication over the socket
        /// </summary>
        /// <param name="access"></param>
        /// <param name="ownsSocket"></param>
        /// <returns></returns>
        Stream CreateStream( FileAccess access,
                             bool ownsSocket );
    }
}
