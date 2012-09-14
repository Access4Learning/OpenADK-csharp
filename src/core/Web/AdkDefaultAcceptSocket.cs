//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace OpenADK.Web
{
    /// <summary>
    /// Wraps the standard .Net Socket class to suit the ISocketDefinition interface
    /// </summary>
    public class AdkDefaultAcceptSocket : IAcceptSocket
    {
        private Socket fAcceptSocket;

        public AdkDefaultAcceptSocket()
        {
           
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Bind(IPEndPoint endPoint)
        {
            if (fAcceptSocket != null)
            {
                throw new InvalidOperationException("Socket is already bound");
            }
            fAcceptSocket =
               new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            fAcceptSocket.Bind( endPoint );
            fAcceptSocket.Listen( 10 );
        }

       
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
            return new AdkDefaultConnectSocket( fAcceptSocket.EndAccept( result ) );
        }
    }
}
