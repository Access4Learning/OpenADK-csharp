//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using System.Net.Sockets;

namespace OpenADK.Web
{
    ///<summary> This class abstracts a socket </summary>
    public class AdkSocketConnection : IDisposable
    {
        #region Private Members

        ///<summary>A callback object for processing recieved socket data </summary>
        private AsyncCallback fReadCompleteCallback;

        ///<summary>A reference to a user supplied function to be called when a socket message arrives </summary>
        private AdkSocketMessageHandler fMessageHandler;

        ///<summary>A reference to a user supplied function to be called when a socket connection is closed </summary>
        private AdkSocketMessageHandler fCloseHandler;

        ///<summary>A reference to a user supplied function to be called when a socket error occurs </summary>
        private AdkSocketErrorHandler fErrorHandler;

        ///<summary>Flag to indicate if the class has been disposed </summary>
        private Boolean fDisposed;

        // The last time ( in ticks ) that this socket was messaged
        private long fLastSignal;
        private bool fIsBusy = false;
        private int fRawBufferSize;
        private int fBytesReceived;
        private byte [] fRawBuffer;
        private Stream fStream;

        ///<summary>The SocketServer for this socket object </summary>
        private AdkSocketBinding fSocketServer;

        /// <summary>
        /// A place to store state across socket reads
        /// </summary>
        private object fUserData;

        /// <summary>
        /// The raw socket that is currently open
        /// </summary>
        private IConnectedSocket fWrappedSocket;

        #endregion

        /// <summary>
        /// Constructor for SocketServer Supppor
        /// </summary>
        /// <param name="server">A Reference to the parent SocketServer</param>
        /// <param name="wrappedSocket">RetType: The Socket object we are encapsulating</param>
        /// <param name="sizeOfRawBuffer">The size of the raw buffer</param>
        /// <param name="messageHandler">Reference to the user defined message handler function</param>
        /// <param name="closeHandler">Reference to the user defined close handler function</param>
        /// <param name="errorHandler">Reference to the user defined error handler function</param>
        public AdkSocketConnection(
            AdkSocketBinding server,
            IConnectedSocket wrappedSocket,
            int sizeOfRawBuffer,
            AdkSocketMessageHandler messageHandler,
            AdkSocketMessageHandler closeHandler,
            AdkSocketErrorHandler errorHandler
            )
        {
            // Create the raw buffer
            fRawBufferSize = sizeOfRawBuffer;
            fRawBuffer = new Byte[sizeOfRawBuffer];

            // Set the handler functions
            fMessageHandler = messageHandler;
            fCloseHandler = closeHandler;
            fErrorHandler = errorHandler;

            // Set the async socket function handlers
            fReadCompleteCallback = new AsyncCallback( ReceiveComplete );

            // Set reference to SocketServer
            fSocketServer = server;

            // Init the socket references
            fWrappedSocket = wrappedSocket;

            // Init the NetworkStream reference
            fStream = fWrappedSocket.CreateStream( FileAccess.ReadWrite, false );

            // Set these socket options
            fWrappedSocket.SetSocketOption
                ( SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, sizeOfRawBuffer );
            fWrappedSocket.SetSocketOption
                ( SocketOptionLevel.Socket, SocketOptionName.SendBuffer, sizeOfRawBuffer );
            fWrappedSocket.SetSocketOption
                ( SocketOptionLevel.Socket, SocketOptionName.DontLinger, 1 );
            fWrappedSocket.SetSocketOption( SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1 );

            // Wait for a message
            Receive();
        }


        public AdkSocketBinding Binding
        {
            get { return fSocketServer; }
        }


        //********************************************************************
        /// <summary> Called when a message arrives </summary>
        /// <param name="ar">An async result interface </param>
        private void ReceiveComplete( IAsyncResult ar )
        {
            Signal();
            fIsBusy = true;
            try {
                
                // Is the Network Stream object valid
                if ( fStream != null && fStream.CanRead ) {
                    
                    // Read the current bytes from the stream buffer
                    fBytesReceived = fStream.EndRead( ar );

                    // If there are bytes to process else the connection is lost
                    if ( fBytesReceived > 0 ) {
                        // A message came in send it to the MessageHandler
                        CallMessageHandler( fMessageHandler );
                        // Wait for a new message
                        if ( this.Connected ) {
                            Receive();
                        }
                        else {
                            Dispose();
                        }
                    }
                    else {
                        Dispose();
                    }
                    
                }
            }
            catch ( Exception ) {
                // Dispose of the class
                Dispose();
            }
        }

        /// <summary> Function used to disconnect from the server </summary>
        public void Close()
        {
            if ( fSocketServer != null ) {
                fSocketServer.RemoveSocket( this );
            }
            try {
                // Notify any listeners that the socket has closed
                CallMessageHandler( fCloseHandler );
            }
            catch {}

            // Close down the connection
            if ( fStream != null ) {
                fStream.Close();
                fStream = null;
            }

            if ( fWrappedSocket != null ) {
                try {
                    fWrappedSocket.Shutdown( SocketShutdown.Both );
                }
                finally {
                    fWrappedSocket.Close();
                }
            }

            // Clean up the connection state
            fWrappedSocket = null;
            fStream = null;
        }


        /// <summary> Wait for a message to arrive </summary>
        public void Receive()
        {
            Signal();
            fIsBusy = false;
            if ( (fStream != null) && (fStream.CanRead) ) {
                // Issue an asynchronous read
                fStream.BeginRead( RawBuffer, 0, fRawBufferSize, fReadCompleteCallback, null );
            }
            else {
                // Do nothing, We're closed
            }
        }


        public void Dispose()
        {
            if ( !fDisposed ) {
                try {
                    fDisposed = true;

                    // Disconnect the client from the server
                    Close();
                }
                catch {}
            }
        }

        #region Public Properties

        public Stream GetOutputDataStream()
        {
            return fStream;
        }

        ///<summary>The socket for the client connection </summary>
        public IConnectedSocket Socket
        {
            get { return fWrappedSocket; }
        }

        public bool Connected
        {
            get { return fWrappedSocket != null && fWrappedSocket.Connected; }
        }


        ///<summary>A raw buffer to capture data comming off the socket </summary>
        public Byte [] RawBuffer
        {
            get { return fRawBuffer; }
        }

        ///<summary>The number of bytes received by the last read</summary>
        public int RawBufferLength
        {
            get { return fBytesReceived; }
        }

        /// <summary>
        /// A place to store state across socket reads and writes
        /// </summary>
        public object UserData
        {
            get { return fUserData; }
            set { fUserData = value; }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// The length of time that this socket has been idle
        /// </summary>
        internal TimeSpan IdleTime
        {
            get
            {
                if (fIsBusy)
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    return TimeSpan.FromTicks( DateTime.Now.Ticks - fLastSignal );
                }
            }
        }

        #endregion

        #region Private Methods

        private void CallMessageHandler( AdkSocketMessageHandler handler )
        {
            if ( handler != null ) {
                handler( this );
            }
        }

        private void CallErrorHandler( AdkSocketErrorHandler handler,
                                       Exception ex )
        {
            if ( handler != null ) {
                handler( this, ex );
            }
        }

        /// <summary>
        /// This socked has been used, reset the IdleTime
        /// </summary>
        private void Signal()
        {
            fLastSignal = DateTime.Now.Ticks;
        }

        #endregion
    }
}
