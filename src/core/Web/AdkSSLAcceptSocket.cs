//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Org.Mentalis.Security.Ssl;

namespace OpenADK.Web
{
    /// <summary>
    /// Summary description for AdkSSLAcceptSocket.
    /// </summary>
    public class AdkSSLAcceptSocket : IAcceptSocket
    {
        private SecureSocket fAcceptSocket;
        private SecurityOptions fOptions;

        public AdkSSLAcceptSocket( SecurityOptions options )
        {
            fOptions = options;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Bind( IPEndPoint endPoint )
        {
            if( fAcceptSocket != null )
            {
                throw new InvalidOperationException( "Socket is already bound" );
            }
            fAcceptSocket = new SecureSocket
                (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, fOptions );
            fAcceptSocket.Bind( endPoint );
            fAcceptSocket.Listen( 10 );
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Close()
        {
            if (fAcceptSocket != null)
            {
                fAcceptSocket.Close();
                fAcceptSocket = null;
            }
        }

        public void BeginAccept( AsyncCallback callback,
                                 object state )
        {
            fAcceptSocket.BeginAccept( callback, state );
        }

        public IConnectedSocket EndAccept( IAsyncResult result )
        {
            return new AdkSSLConnectedSocket( (SecureSocket) fAcceptSocket.EndAccept( result ) );
        }
    }
}
