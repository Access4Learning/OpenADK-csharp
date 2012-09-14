//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using OpenADK.Library;
using log4net;

namespace OpenADK.Web
{
    /// <summary>
    /// Summary description for AdkSocketBinding.
    /// </summary>
    public class AdkSocketBinding
    {
        //
        private const int DEFAULT_MAX_CONNECTIONS = 50;

        // The amount of time to keep connections open in seconds
        private const int KEEP_ALIVE_TIME = 180;

        private Timer fCleanupTimer;
        private IPAddress fIPAddress;
        private int fPort = 0;
        private bool fIsRunning;
        private bool fIsShuttingDown;

        private IAcceptSocket fAcceptSocket;

        ///<summary>A vector of AdkSocketConnection objects </summary>
        private ArrayList fSocketConnections;

        private int fMaxClientConnections = 50;
        private int fRawBufferSize = 2048;
        private Boolean fDisposed;

   
        private ILog fLog;

        /// <summary> Constructor </summary>
        public AdkSocketBinding( IAcceptSocket socket )
        {
            fAcceptSocket = socket;
            fSocketConnections = new ArrayList();
            TimeSpan cleanupInterval = TimeSpan.FromSeconds( (int) (KEEP_ALIVE_TIME/3) );
            fCleanupTimer =
                new Timer
                    ( new TimerCallback( Cleanup ), cleanupInterval, cleanupInterval,
                      cleanupInterval );
        }

        /// <summary> Constructor </summary>
        public AdkSocketBinding( IAcceptSocket socket,
                                 ILog log )
            : this( socket )
        {
            fLog = log;
        }

        //********************************************************************


        /// <summary> Dispose function to shutdown the AdkSocketConnection </summary>
        public void Dispose()
        {
            if ( !fDisposed )
            {
                try
                {
                    fDisposed = true;
                    Stop();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Closes any idle socket connections
        /// </summary>
        public void Cleanup( object timeInterval )
        {
            try
            {
                fCleanupTimer.Change( Timeout.Infinite, Timeout.Infinite );
                lock ( fSocketConnections.SyncRoot )
                {
                    for ( int i = fSocketConnections.Count - 1; i > -1; i-- )
                    {
                        AdkSocketConnection connection = (AdkSocketConnection) fSocketConnections[i];
                        if ( connection.IdleTime.TotalSeconds > KEEP_ALIVE_TIME )
                        {
                            connection.Dispose(); // will automatically remove itself
                        }
                    }
                }
            }
            finally
            {
                TimeSpan timerInterval = (TimeSpan) timeInterval;
                fCleanupTimer.Change( timerInterval, timerInterval );
            }
        }


        protected virtual void OnMaxConnectionsReached( IConnectedSocket connectedSocket )
        {
        }

        protected virtual void OnError( Exception ex )
        {
            Error( ex.Message, null, ex );
        }


        //********************************************************************
        /// <summary> Function to start the SocketServer </summary>
        public void Start()
        {
            if ( fIPAddress == null || fPort == 0 )
            {
                // TODO:
                throw new Exception( "Unable to start Socket server with specified values" );
            }
            // Is the Socket already listening?
            if ( !fIsRunning )
            {
                fIsShuttingDown = false;
                // Init the array of AdkSocketConnection references
                fSocketConnections = new ArrayList( fMaxClientConnections );
                IPEndPoint ipEndpoint = new IPEndPoint( fIPAddress, fPort );
                AsyncCallback handler = new AsyncCallback( AcceptNewConnection );

                // Call the derived class to begin accepting connections
                fAcceptSocket.Bind( ipEndpoint );
                fAcceptSocket.BeginAccept( handler, null );
                Debug( "AdkSocketServer Started. Listening on port : {0}", new object[] {fPort} );
                fIsRunning = true;
            }
        }


        //********************************************************************

        /// <summary> Function to stop the SocketServer.It can be restarted with Start </summary>
        public void Stop()
        {
            if ( fIsRunning )
            {
                fIsShuttingDown = true;
                fIsRunning = false;

                // Dispose of all of the socket connections
                for ( int i = fSocketConnections.Count - 1; i > -1; i -- )
                {
                    try
                    {
                        ((AdkSocketConnection) fSocketConnections[i]).Dispose();
                        // Automatically removes itself from the list
                    }
                    catch ( Exception ex )
                    {
                        Error( ex.Message, null, ex );
                    }
                }

                fAcceptSocket.Close();
            }
        }

        public void Debug( string message,
                           object[] mergeValues )
        {
            if ( fLog != null && fLog.IsDebugEnabled )
            {
                fLog.Debug( string.Format( message, mergeValues ) );
            }
        }

        public void Error( string message,
                           object[] mergeValues,
                           Exception ex )
        {
            if ( fLog != null && fLog.IsErrorEnabled )
            {
                fLog.Error( string.Format( message, mergeValues ), ex );
            }
        }

        #region Public Methods

        /// <summary> Function to remove a socket from the list of sockets </summary>
        /// <param name="connection">The index of the socket to remove </param>
        public void RemoveSocket( AdkSocketConnection connection )
        {
            lock ( fSocketConnections.SyncRoot )
            {
                try
                {
                    fSocketConnections.Remove( connection );
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// The port this server is listening on
        /// </summary>
        public int Port
        {
            get { return fPort; }
            set
            {
                if ( CanSetValues() )
                {
                    fPort = value;
                }
            }
        }

        public IPAddress HostAddress
        {
            get { return fIPAddress; }
            set
            {
                if ( CanSetValues() )
                {
                    fIPAddress = value;
                }
            }
        }

        public bool IsStarted
        {
            get { return fIsRunning; }
        }

        public int ConnectionCount
        {
            get { return fSocketConnections.Count; }
        }

        /// <summary>
        /// The size of memory that will be reserved for receiving data by this listener
        /// </summary>
        public int RawBufferSize
        {
            get { return fRawBufferSize; }
            set { fRawBufferSize = value; }
        }


        ///<summary>Maximum number of client connections that will be accepted by this listener </summary>
        public int MaxClientConnections
        {
            get { return fMaxClientConnections; }
            set
            {
                if ( value < 1 )
                {
                    fMaxClientConnections = DEFAULT_MAX_CONNECTIONS;
                }
                else
                {
                    fMaxClientConnections = value;
                }
            }
        }

        public event AdkSocketMessageHandler SocketAccepted;

        public event AdkSocketMessageHandler DataReceived;

        public event AdkSocketMessageHandler SocketClosed;

        public event AdkSocketErrorHandler SocketError;

        #endregion

        #region Private Methods

        /// <summary> Function to process and accept socket connection requests </summary>
        private void AcceptNewConnection( IAsyncResult ar )
        {
            if ( fIsShuttingDown )
            {
                return;
            }

            IConnectedSocket clientSocket = null;
            try
            {
                // Call the derived class's accept handler
                clientSocket = fAcceptSocket.EndAccept( ar );
                if ( !clientSocket.Connected )
                {
                    return;
                }

                lock ( fSocketConnections.SyncRoot )
                {
                    try
                    {
                        // If we have room to accept this connection
                        if ( fSocketConnections.Count <= MaxClientConnections )
                        {
                            // Create a AdkSocketConnection object
                            AdkSocketConnection adkSocket = new AdkSocketConnection
                                (
                                this, clientSocket, RawBufferSize,
                                DataReceived,
                                SocketClosed,
                                SocketError
                                );

                            // Call the Accept Handler
                            fSocketConnections.Add( adkSocket );
                            if ( SocketAccepted != null )
                            {
                                try
                                {
                                    SocketAccepted( adkSocket );
                                }
                                catch
                                {
                                }
                            }
                        }
                        else
                        {
                            OnMaxConnectionsReached( clientSocket );
                            // Close the socket connection
                            clientSocket.Shutdown( SocketShutdown.Both );
                            clientSocket.Close();
                        }
                    }
                    catch ( SocketException e )
                    {
                        // Did we stop the TCPListener
                        if ( e.ErrorCode == 10004 )
                        {
                            // The connection is being shut down, ignore the error
                        }
                        else
                        {
                            OnError( e );
                            // Close the socket down if it exists
                            if ( clientSocket != null && clientSocket.Connected )
                            {
                                clientSocket.Shutdown( SocketShutdown.Both );
                                clientSocket.Close();
                            }
                        }
                    }
                    catch ( Exception ex )
                    {
                        OnError( ex );
                        // Close the socket down if it exists
                        if ( clientSocket != null && clientSocket.Connected )
                        {
                            clientSocket.Shutdown( SocketShutdown.Both );
                            clientSocket.Close();
                        }
                    }
                }
            }
            catch ( SocketException e )
            {
                // Did we stop the TCPListener
                if ( e.ErrorCode == 10004 )
                {
                    // We're done
                    return;
                }
                else
                {
                    OnError( e );
                    // Close the socket down if it exists
                    if ( clientSocket != null && clientSocket.Connected )
                    {
                        clientSocket.Shutdown( SocketShutdown.Both );
                        clientSocket.Close();
                    }
                }
            }
            finally
            {
                if ( fIsRunning )
                {
                    fAcceptSocket.BeginAccept( new AsyncCallback( AcceptNewConnection ), null );
                }
            }
        }


        private bool CanSetValues()
        {
            lock ( fSocketConnections.SyncRoot )
            {
                if ( fIsRunning )
                {
                    throw new AdkException
                        ( "Cannot switch connection properties while server is running", null );
                }
                else
                {
                    return true;
                }
            }
        }

        #endregion
    }
}
