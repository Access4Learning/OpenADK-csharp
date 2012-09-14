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
    /// Summary description for AdkDefaultConnectSocket.
    /// </summary>
    public class AdkDefaultConnectSocket : IConnectedSocket
    {
        private Socket fSocket;

        public AdkDefaultConnectSocket( Socket wrappedSocket )
        {
            fSocket = wrappedSocket;
        }

        public bool Connected
        {
            get { return fSocket.Connected; }
        }

        public void SetSocketOption( SocketOptionLevel level,
                                     SocketOptionName name,
                                     int val )
        {
            fSocket.SetSocketOption( level, name, val );
        }

        public void Close()
        {
            fSocket.Close();
        }

        public void Shutdown( SocketShutdown shutDownType )
        {
            fSocket.Shutdown( shutDownType );
        }

        public EndPoint LocalEndPoint
        {
            get { return fSocket.RemoteEndPoint; }
        }


        public EndPoint RemoteEndPoint
        {
            get { return fSocket.RemoteEndPoint; }
        }

        public Stream CreateStream( FileAccess access,
                                    bool ownsSocket )
        {
            return new NetworkStream( fSocket, access, ownsSocket );
        }
    }
}
