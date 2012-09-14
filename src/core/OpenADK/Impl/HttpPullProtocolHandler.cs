//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Threading;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Impl
{
    /// <summary>  An protocol handler implementation for HTTP. Each zone that is registered
    /// with a ZIS using the HTTP or HTTPS protocol has an instance of this class
    /// as its protocol handler. It implements the HttpHandler interface to process
    /// SIF messages received by the agent's internal Jetty HTTP Server. When a
    /// message is received via that interface it is delegated to the zone's
    /// MessageDispatcher. HttpProtocolHandler also implements the IProtocolHandler
    /// interface so it can send outgoing messages received by the
    /// MessageDispatcher.
    /// 
    /// 
    /// An instance of this class runs in a separate thread only when the agent is
    /// registered with the ZIS in Pull mode. In this case it does not accept
    /// messages from the HttpHandler interface but instead periodically queries the
    /// ZIS for new messages waiting in the agent's queue. Messages are delegated to
    /// the MessageDispatcher for processing.
    /// 
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    internal class HttpPullProtocolHandler : BaseHttpProtocolHandler
    {
        private Thread fThread;
        private bool fRunning;

        public HttpPullProtocolHandler( HttpTransport transport ) : base( transport )
        {
        }

        #region overrides of BaseHttpTransportProtocol

        /// <summary>  Starts the handler's polling thread if running in Pull mode</summary>
        public override void Start()
        {
            lock ( this ) {
                if ( this.Zone.Properties.MessagingMode == AgentMessagingMode.Pull ) {
                    //  Polling thread already running?
                    if ( fThread != null && fThread.ThreadState == ThreadState.Running ) {
                        this.Zone.Log.Debug
                            ( "Polling thread (HTTP/HTTPS) already running for zone" );
                        return;
                    }

                    if ( (Adk.Debug & AdkDebugFlags.Transport) != 0 ) {
                        this.Zone.Log.Debug
                            ( "Starting polling thread (HTTP/HTTPS), zone connected in Pull mode..." );
                    }

                    //  Run in a thread for pull mode operation
                    fThread = new Thread( new ThreadStart( this.Run ) );
                    fThread.Name = Name;
                    fThread.Start();
                }
                else {
                    throw new AdkException
                        ( this.GetType().FullName + " is not able to Start in PUSH mode", this.Zone );
                }
            }
        }

        /// <summary>  Stops the handler's polling thread if running in Pull mode</summary>
        public override void Shutdown()
        {
            lock ( this ) {
                if ( fThread != null & fThread.ThreadState == ThreadState.Running ) {
                    fThread.Interrupt();
                }
            }
            
            fRunning = false;
        }


        /// <summary>  Close this ProtocolHandler for a zone</summary>
        public override void Close( IZone zone )
        {
            lock ( this ) {
                //  Stop the polling thread if we were registered in Pull mode
                if ( fThread != null ) {
                    fRunning = false;
                    fThread.Interrupt();
                    fThread = null;
                }
            }
        }

        #endregion

        /// <summary>  Thread periodically sends a SIF_GetMessage request to the ZIS to get
        /// the next message waiting in the agent queue
        /// </summary>
        private void Run()
        {
            fRunning = true;
            TimeSpan freq = this.Zone.Properties.PullFrequency;
            TimeSpan delay = this.Zone.Properties.PullDelayOnError;
            if ( (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0 ) {
                this.Zone.Log.Debug
                    ( "Polling thread (HTTP/HTTPS) started with frequency " + freq.ToString() +
                      " seconds" );
            }

            while ( fRunning ) {
                try {
                    if ( this.Zone.IsShutdown ) {
                        if ( (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0 ) {
                            this.Zone.Log.Debug
                                ( "Polling thread (HTTP/HTTPS) will stop, zone has shut down" );
                        }
                        break;
                    }
                    else if ( !this.Zone.Connected ) {
                        if ( (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0 ) {
                            this.Zone.Log.Debug
                                ( "Polling thread (HTTP/HTTPS) will delay " + delay.Seconds +
                                  " seconds, zone is no longer connected" );
                        }
                        Thread.Sleep( delay );
                    }
                    else {
                        int i = this.Zone.Dispatcher.Pull();
                        if ( i == - 1 ) {
                            //  The zone is sleeping
                            if ( (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0 ) {
                                this.Zone.Log.Debug
                                    ( "Polling thread (HTTP/HTTPS) will delay " + delay.Seconds +
                                      " seconds, zone is sleeping" );
                            }

                            Thread.Sleep( delay );
                        }
                        else {
                            Thread.Sleep( freq );
                        }
                    }
                }
                catch ( LifecycleException ) {
                    //  Agent was shutdown - exit gracefully
                    break;
                }
                catch ( ThreadInterruptedException ) {
                    if ( (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0 ) {
                        this.Zone.Log.Debug( "Polling thread (HTTP/HTTPS) interrupted" );
                    }
                    break;
                }
                catch ( Exception adke ) {
                    if ( (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0 ) {
                        this.Zone.Log.Debug
                            ( "Polling thread (HTTP/HTTPS) failed to retrieve message: " + adke );
                    }

                    //
                    //  Special Check: If registered in Push mode and we're still
                    //  pulling, stop the thread - there is no sense in continuing.
                    //  This should not happen, but just in case...
                    //
                    if ( adke is AdkException &&
                         ((AdkException) adke).HasSifError
                             ( SifErrorCategoryCode.Registration, SifErrorCodes.REG_PUSH_EXPECTED_9 )
                        ) {
                        if ( (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0 ) {
                            this.Zone.Log.Debug
                                ( "Polling thread (HTTP/HTTPS) will now stop because agent is registered in Push mode" );
                        }
                        break;
                    }

                    try {
                        //  Sleep for the regular frequency before pulling next message
                        Thread.Sleep( freq );
                    }
                    catch ( ThreadInterruptedException ) {
                        // App is shutting down
                        break;
                    }
                }
            }

            fRunning = false;

            if ( (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0 ) {
                this.Zone.Log.Debug( "Polling thread (HTTP/HTTPS) has ended" );
            }
        }

        /// <summary>
        /// Returns true if the protocol and underlying transport are currently active
        /// for this zone
        /// </summary>
        /// <param name="zone"></param>
        /// <returns>True if the protocol handler and transport are active</returns>
        public override bool IsActive( ZoneImpl zone )
        {
            return fRunning;
        }

        /// <summary>
        /// Creates the SIF_Protocol object that will be included with a SIF_Register
        /// message sent to the zone associated with this Transport.</Summary>
        /// <remarks>
        /// The base class implementation creates an empty SIF_Protocol with zero
        /// or more SIF_Property elements according to the parameters that have been
        /// defined by the client via setParameter. Derived classes should therefore
        /// call the superclass implementation first, then add to the resulting
        /// SIF_Protocol element as needed.
        /// </remarks>
        /// <param name="zone"></param>
        /// <returns></returns>
        public override SIF_Protocol MakeSIF_Protocol( IZone zone )
        {
            // No SIF_Protocol element is necessary in pull mode
            return null;
        }
    }
}
