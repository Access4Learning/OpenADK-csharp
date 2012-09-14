//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using OpenADK.Library.Infra;
using OpenADK.Library.Log;
using OpenADK.Library.Tools.Policy;
using OpenADK.Util;
using log4net;

namespace OpenADK.Library.Impl
{
    /// <summary>  Implementation of the Zone interface.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class ZoneImpl : IZone
    {
        // Values for the fState variable
        private const int
            UNINIT = 0x00000000,
            // connect() not yet called
            CONNECTED = 0x00000001,
            // connect() called successfully
            CLOSED = 0x00000002,
            // disconnect() called; ZoneImpl object now invalid
            SHUTDOWN = 0x00000004,
            // shutdown() called
            SLEEPING = 0x00000020,
            // Zone is in sleep mode
            GETZONESTATUS = 0x00000040; // getZoneStatus is blocking on results


        /// <summary>  logging framework logging category for this zone</summary>
        private ILog fLog;

        /// <summary>  The Agent that owns this zone</summary>
        protected Agent fAgent;

        /// <summary>  The unique string identifier for this zone</summary>
        protected string fZoneId;

        /// <summary>  The URL of the Zone Integration Server that manages this zone</summary>
        protected Uri fZoneUrl;

        /// <summary>  Zone properties</summary>
        private AgentProperties fProps;

        /// <summary>  Connection state</summary>
        protected int fState = UNINIT;

        /// <summary>  The ProtocolHandler for this zone</summary>
        protected internal IProtocolHandler fProtocolHandler;

        /// <summary>  The MessageDispatcher for this zone</summary>
        protected MessageDispatcher fDispatcher;

        /// <summary>  ResponseDelivery thread for this zone that handles sending SIF_Response packets
        /// that have been stored on the local file system as a result of calling
        /// the Publisher.onQuery message handler.
        /// </summary>
        private ResponseDelivery fResponseDelivery;

        /// <summary>  ResponseDelivery thread for this zone that handles sending SIF_Response packets
        /// for SIF_ReportObject requests stored on the local file system as a result of 
        /// calling the ReportPublisher.onQuery message handler method.
        /// </summary>
        private ResponseDelivery frptResponseDelivery;

        /// <summary>  The IAgentQueue for this zone</summary>
        protected internal IAgentQueue fQueue;

        /// <summary> 	Manages the MessagingListeners</summary>
        protected internal List<IMessagingListener> fMessagingListeners = new List<IMessagingListener>();

        /// <summary>  The UndeliverableMessageHandler for this zone</summary>
        protected IUndeliverableMessageHandler fErrHandler;


        ///<summary>The matrix of all Publishers, Subsriber, etc. for each SIF Context running in this </summary>
        public ProvisioningMatrix fProvMatrix = new ProvisioningMatrix();


        /// <summary>  If a SIF Error is received in response to a SIF_Provide or SIF_Subscribe
        /// message sent by the provision() method, it is added to this list and
        /// returned when <c>getConnectWarnings</c> is called
        /// </summary>
        protected List<SifException> fProvWarnings = new List<SifException>();

        /// <summary>  Reference to the ISIFPrimitives object to use for SIF messaging</summary>
        internal ISIFPrimitives fPrimitives;


        /// <summary>  The last SIF_ZoneStatus object received by MessageDispatcher. When
        /// getZoneStatus is called, it blocks on fZSLock until setZoneStatus()
        /// is called by MessageDispatcher or the timeout period specified by the
        /// caller has elapsed.
        /// </summary>
        private SIF_ZoneStatus fZoneStatus = null;

        //  Objects used as semaphores
        private object fZSLock = new object();
        private object fConnLock = new object();

        /// <summary>
        /// Semaphore to ensure that MessageDispatcher does not attempt to process a
        /// SIF_Response message before the Zone.onQuery has completed. Zone.onQuery
        /// synchronizes on this object; MessageDispatcher must also synchronize on
        /// this object when it receives a SIF_Response.
        /// </summary>
        /// <remarks>
        /// <see cref="ZoneImpl.WaitForRequestsToComplete"/>
        /// </remarks>
        private object fReqLock = new object();

        /// <summary>  Optional user data set with the setUserData method</summary>
        private Object fUserData;

        #region Properties

        /// <summary>  Gets the ZoneProperties for this zone</summary>
        /// <value> The ZoneProperties object for this zone</value>
        public AgentProperties Properties
        {
            get { return fProps; }

            set
            {
                if (value == null)
                {
                    fProps = new AgentProperties(fAgent.Properties);
                }
                else
                {
                    fProps = value;
                }
            }
        }

        /// <summary>  Gets the Agent object</summary>
        public virtual Agent Agent
        {
            get { return fAgent; }
        }

        /// <summary>  Gets the zone name</summary>
        /// <returns>s The name of the zone
        /// </returns>
        public virtual string ZoneId
        {
            get { return fZoneId; }
        }

        /// <summary>  Gets the URL of the Zone Integration Server that manages this zone</summary>
        /// <returns>s The URL to the ZIS in whatever format is expected by the ZIS
        /// (e.g. "https://host:port/opensif/zis/zone-name" for the OpenSIF ZIS,
        /// "https://host:port/zone-name" for the Library ZIS, etc.)
        /// </returns>
        public virtual Uri ZoneUrl
        {
            get { return fZoneUrl; }
        }


        /// <summary> 	Gets the ResponseDelivery thread for generic SIF_Response processing
        /// on this zone, creating it if not already initialized. This method is
        /// normally called by the DataObjectOutputStream class to notify the
        /// delivery thread that new packets are waiting for delivery to the ZIS.
        /// </summary>
        /// <value>The ResponseDelivery object used for SIF_Response processing</value>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual ResponseDelivery GetResponseDelivery()
        {
            if (fResponseDelivery == null)
            {
                fResponseDelivery = new ResponseDelivery(this, ResponseDeliveryType.Generic);
            }

            return fResponseDelivery;
        }

        /// <summary> 	Gets the ResponseDelivery thread for SIF_ReportObject SIF_Response 
        /// processing on this zone, creating it if not already initialized. This 
        /// method is normally called by the ReportObjectOutputStream class to 
        /// notify the delivery thread that new packets are waiting for delivery 
        /// to the ZIS.
        /// </summary>
        /// /// <value>The ResponseDelivery object used for SIF_Response processing</value>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual ResponseDelivery GetReportResponseDelivery()
        {
            if (frptResponseDelivery == null)
            {
                frptResponseDelivery =
                    new ResponseDelivery(this, ResponseDeliveryType.SIFReportObject);
            }

            return frptResponseDelivery;
        }

        /// <summary>  Gets a read-only list of any SIF Errors that resulted from sending SIF_Provide and
        /// SIF_Subscribe provisioning messages to the zone. Currently, only access
        /// control errors (Category 4) are treated as warnings rather than errors.
        /// All other SIF Errors result in an exception thrown by the
        /// <c>connect</c> method.
        /// 
        /// </summary>
        /// <returns> An array of SIFExceptions
        /// </returns>
        public virtual IList<SifException> ConnectWarnings
        {
            get
            {
                lock (fConnLock)
                {
                    return fProvWarnings.AsReadOnly();
                }
            }
        }

        /// <summary>  Gets the connection state of this Zone</summary>
        /// <returns> true if the connect method has been called but the disconnect
        /// method has not; false if the connect method has not yet been called
        /// or the disconnect method has been called
        /// </returns>
        public virtual bool Connected
        {
            get { return (fState & CONNECTED) == CONNECTED; }
        }

        /// <summary>  Determines if this zone is shutting down</summary>
        /// <returns> true if the shutdown method has been called
        /// </returns>
        public virtual bool IsShutdown
        {
            get { return (fState & SHUTDOWN) == SHUTDOWN; }
        }

        public virtual IUndeliverableMessageHandler ErrorHandler
        {
            get
            {
                if (fErrHandler != null)
                {
                    return fErrHandler;
                }

                return fAgent.ErrorHandler;
            }

            set { fErrHandler = value; }
        }

        /// <summary>  Gets the application-supplied object for this Zone</summary>
        /// <value> The object that is stored</value>
        public virtual Object UserData
        {
            get { return fUserData; }

            set { fUserData = value; }
        }

        protected virtual void CheckConnect()
        {
            if (!Connected)
            {
                throw new AdkZoneNotConnectedException("Zone is not connected", this);
            }
        }

        /// <summary>  Gets the root logging framework Category for this agent.</summary>
        public virtual ILog Log
        {
            get { return Agent.GetLog(this); }
        }

        /// <summary> 	Gets the ServerLog for a specific zone.</summary>
        /// <value> The ServerLog instance for the zone
        /// </value>
        public virtual ServerLog ServerLog
        {
            get { return ServerLog.GetInstance(Adk.LOG_IDENTIFIER + ".Agent$" + ZoneId, this); }
        }

        public IProtocolHandler ProtocolHandler
        {
            get { return fProtocolHandler; }
        }

        /// <summary>  The MessageDispatcher for this zone</summary>
        protected internal MessageDispatcher Dispatcher
        {
            get { return fDispatcher; }
            set { fDispatcher = value; }
        }

        #endregion

        /// <summary>  Constructs a Zone instance</summary>
        /// <param name="zoneId">The name of the zone
        /// </param>
        /// <param name="zoneUrl">The URL of the Zone Integration Server that manages this zone
        /// </param>
        /// <param name="agent">The Agent object. Any Adk object such as a Topic can always
        /// find a reference to the Agent by asking a Zone
        /// </param>
        /// <param name="props">Zone properties
        /// </param>
        protected internal ZoneImpl(string zoneId,
                                     string zoneUrl,
                                     Agent agent,
                                     AgentProperties props)
        {
            if (zoneId == null || zoneId.Trim().Length < 1)
            {
                AdkUtils._throw
                    (new ArgumentException("Zone name cannot be null or blank"), fLog);
            }

            fLog = LogManager.GetLogger(Agent.LOG_IDENTIFIER + "$" + zoneId);

            if (zoneUrl == null || zoneUrl.Trim().Length < 1)
            {
                AdkUtils._throw(new ArgumentException("Zone URL cannot be null or blank"), fLog);
            }
            if (agent == null)
            {
                AdkUtils._throw(new ArgumentException("Agent cannot be null"), fLog);
            }

            fAgent = agent;
            fZoneId = zoneId;
            // Call the Properties Accessor because 
            Properties = props;

            try
            {
                fZoneUrl = new Uri(zoneUrl);
            }
            catch (UriFormatException mue)
            {
                throw new AdkTransportException
                    ("Zone URL is malformed: " + zoneUrl + mue.ToString(), this);
            }
        }

        /// <summary>  Shutdown the zone.</summary>
        public virtual void Shutdown()
        {
            if (fProtocolHandler == null)
            {
                return;
            }

            if ((Adk.Debug & AdkDebugFlags.Lifecycle) != 0)
            {
                fLog.Info("Shutting down zone...");
            }

            fState |= SHUTDOWN;

            if (fDispatcher != null)
            {
                try
                {
                    if ((Adk.Debug & AdkDebugFlags.Lifecycle) != 0)
                    {
                        fLog.Info("Shutting down Message Dispatcher");
                    }
                    fDispatcher.shutdown();
                }
                catch (Exception ignored)
                {
                    fLog.Error("Error shutting down Message Dispatcher: " + ignored, ignored);
                }
            }

            fDispatcher = null;

            if (fProtocolHandler != null)
            {
                try
                {
                    if ((Adk.Debug & AdkDebugFlags.Lifecycle) != 0)
                    {
                        fLog.Info("Shutting down Protocol Handler");
                    }
                    fProtocolHandler.Close(this);
                }
                catch (Exception ignored)
                {
                    fLog.Error("Error shutting down Protocol Handler: " + ignored, ignored);
                }
            }

            fProtocolHandler = null;

            if ((Adk.Debug & AdkDebugFlags.Lifecycle) != 0)
            {
                fLog.Info("Zone shutdown complete");
            }
        }


        /// <summary>
        /// Gets the Agent ACL list from the zone. This method invokes the zone 
        /// synchronously before returning.
        /// </summary>
        /// <returns>the SIF_AgentACL object returned from the ZIS</returns>
        /// <exception cref="AdkException">May be thrown if the ZIS does not support GetAgentACL</exception>
        public SIF_AgentACL GetAgentACL()
        {
            CheckConnect();
            SIF_Ack ack = fPrimitives.SifGetAgentACL(this);
            if (ack.HasError())
            {
                throw new SifException(ack, this);
            }

            //	The SIF_AgentACL object will be in the SIF_Ack/SIF_Data
            SIF_Status status = ack.SIF_Status;
            if (status != null)
            {
                SIF_Data data = status.SIF_Data;
                if (data != null)
                {
                    SifElementList childList = data.GetChildList(InfraDTD.SIF_AGENTACL);
                    if (childList != null && childList.Count > 0)
                    {
                        return (SIF_AgentACL)childList[0];
                    }
                }
            }
            return null;
        }


        public virtual void Connect(ProvisioningFlags provOptions)
        {
            lock ( fConnLock )
            {
                if ( Connected )
                {
                    throw new InvalidOperationException( "Zone already connected" );
                }

                fPrimitives = Adk.Primitives;

                fState = 0;


                //  Sleep on connect will cause any messages received by the fDispatcher
                //  to be turned away with a status code 8 ("Receiver sleeping")
                if ( (provOptions & ProvisioningFlags.SleepOnConnect) != 0 )
                {
                    fState |= SLEEPING;
                }

                //  Initialize the MessageDispatcher
                if ( fDispatcher == null )
                {
                    fDispatcher = new MessageDispatcher( this );
                }

                try
                {
                    //  Start the IProtocolHandler for this zone. The protocol handler
                    //  is transport-specific so it is created by the Transport object
                    //  associated with the zone. Starting the handler typically
                    //  establishes a socket to the ZIS and starts a thread to pull
                    //  messages when the zone is running is Pull mode.
                    fProtocolHandler = ((TransportManagerImpl) fAgent.TransportManager).Activate( this );
                    fProtocolHandler.Open( this );
                }
                catch ( AdkException adke )
                {
                    AdkUtils._throw( adke, fLog );
                }
                catch ( Exception ex )
                {
                    AdkUtils._throw
                        ( new AdkException( "Failed to start transport protocol: " + ex, this ),
                          fLog );
                }


                //	When running in Push mode, make sure protocol handler is 
                //	started right away because the ZIS may send a message to us 
                //	immediately after registration. The SIF Compliance test harness 
                //	does this, and if the protocol handler is not started a 404 
                //	error is returned (which causes the agent to fail one of the 
                //	certification tests.)  In Pull mode, the handler is started
                //	later in this function since starting it invokes the Pull
                //	thread (which should not be done until after SIF Register).
                //
                try
                {
                    if ( Properties.MessagingMode == AgentMessagingMode.Push )
                    {
                        fProtocolHandler.Start();

                        //  Transport protocol still active? With Jetty, the Acceptor thread may
                        //  have gone down by now and so we need to check again to see if the
                        //  Listeners are still active for https/http.
                        if ( !fProtocolHandler.IsActive( this ) )
                        {
                            AdkUtils._throw(
                                new AdkTransportException( "The transport protocol is not available for this zone", this ),
                                Log );
                        }
                    }
                }
                catch ( AdkException adke )
                {
                    AdkUtils._throw( adke, fLog );
                }
                catch ( Exception ex )
                {
                    AdkUtils._throw
                        ( new AdkException( "Failed to start transport protocol: " + ex, this ),
                          fLog );
                }

                //
                //  If any exception is thrown from this point on, catch-rethrow but
                //  set the state to Disconnected.
                //
                bool _connectFailed = true;

                SIF_AgentACL acl = null;
                try
                {
                    //  Send SIF_Register
                    if ( ((provOptions & ProvisioningFlags.Register) != 0) &&
                         Properties.ProvisioningMode == AgentProvisioningMode.Adk )
                    {
                        SIF_Ack ack = fPrimitives.SifRegister( this );
                        if ( ack.HasStatusCode( 8 ) )
                        {
                            fState |= SLEEPING;
                        }
                        if ( !ack.HasStatusCode( 0 ) )
                        {
                            fState &= ~CONNECTED;
                            AdkUtils._throw( new SifException( ack, this ), fLog );
                        }
                        else if ( (Adk.Debug & AdkDebugFlags.Provisioning) != 0 )
                        {
                            fLog.Info( "SIF_Register successful" );
                        }


                        SIF_Data payload = ack.SIF_Status.SIF_Data;
                        if ( payload != null && payload.ChildCount > 0 )
                        {
                            SifElement child = payload.GetChildList()[0];
                            if ( child.ElementDef == InfraDTD.SIF_AGENTACL )
                            {
                                acl = (SIF_AgentACL) child;
                            }
                        }
                    }
                    //  We're now connected
                    fState |= CONNECTED;

                    //  Send pending SIF_Responses if any leftover from prior session
                    if ( ResponseDelivery.HasPendingPackets( ResponseDeliveryType.Generic, this ) )
                    {
                        ResponseDelivery _rd = this.GetResponseDelivery();

                        ResponseDeliveryHandler handler = delegate( ResponseDelivery rd )
                                                              {
                                                                  try
                                                                  {
                                                                      rd.Process();
                                                                  }
                                                                  catch ( AdkException adke )
                                                                  {
                                                                      string objectType = "SIF_Response";
                                                                      if ( _rd.ProcessingType ==
                                                                           ResponseDeliveryType.SIFReportObject )
                                                                      {
                                                                          objectType = "SIF_ReportObject";
                                                                      }
                                                                      fLog.Debug
                                                                          ( "Failed to send " + objectType +
                                                                            " packets from a previous session", adke );
                                                                  }
                                                              };
                        
                        AsyncUtils.QueueTaskToThreadPool( handler, new object[] { _rd } );
                    }

                    //  Send pending SIF_Responses if any leftover from prior session
                    if ( ResponseDelivery.HasPendingPackets( ResponseDeliveryType.SIFReportObject, this ) )
                    {
                        if ( frptResponseDelivery == null )
                        {
                            frptResponseDelivery =
                                new ResponseDelivery( this, ResponseDeliveryType.SIFReportObject );
                        }
                        ResponseDeliveryHandler handler = delegate( ResponseDelivery rd2 )
                                                              {
                                                                  try
                                                                  {
                                                                      rd2.Process();
                                                                  }
                                                                  catch ( AdkException adke )
                                                                  {
                                                                      Log.Debug(
                                                                          "Failed to send SIF_ReportObject response packets from a previous session",
                                                                          adke );
                                                                  }
                                                              };
                           
                        AsyncUtils.QueueTaskToThreadPool
                            ( handler, new object[] {fResponseDelivery} );
                    }

                    if ( (provOptions & ProvisioningFlags.SleepOnConnect) != 0 )
                    {
                        try
                        {
                            //  Sleep immediately if requested by the caller
                            Sleep();
                            fState |= SLEEPING;
                        }
                        catch ( Exception ex )
                        {
                            Log.Warn( "Error attempting to sleep on connect:" + ex, ex );
                        }
                    }
                    else
                    {
                        //  SIF spec recommends sending SIF_Wakeup on startup; no need
                        //  to send this when SIF_Register is sent because it is implicit
                        //  in that case
                        SIF_Ack ack;
                        if ( (provOptions & ProvisioningFlags.Register) == 0 )
                        {
                            if ( Properties.ProvisioningMode == AgentProvisioningMode.Adk )
                            {
                                WakeUp();
                            }
                        }

                        //  Calling sifPing will determine the sleep mode of the zone
                        //  from the server's perspective
                        ack = SifPing();
                        if ( ack.HasError() )
                        {
                            throw new SifException( ack, this );
                        }
                    }

                    //  Send SIF_Provide and SIF_Subscribe
                    Provision( acl );

                    try
                    {
                        //  Protocol handler can start now that the zone is connected. If
                        //	running in Push mode, this was done earlier in this function.
                        if ( Properties.MessagingMode == AgentMessagingMode.Pull )
                        {
                            fProtocolHandler.Start();
                            fState |= CONNECTED;
                        }

                        //  Let MessageDispatcher know it can start sending SIF_GetMessage
                        //  if operating in pull mode
                        fDispatcher.notifyPullCanStart();

                        //  Success!
                        _connectFailed = false;
                    }
                    catch ( Exception ex )
                    {
                        throw new AdkException
                            ( "Failed to start " + fProtocolHandler.Name + " protocol handler: " + ex.Message,
                              this );
                    }
                }
                finally
                {
                    try
                    {
                        //	When running in Push mode, stop the protocol handler if
                        //	SIF_Register failed
                        if ( !((fState & CONNECTED) != 0) )
                        {
                            if ( Properties.MessagingMode == AgentMessagingMode.Push )
                            {
                                fProtocolHandler.Shutdown();
                            }
                        }
                    }
                    catch ( Exception ex )
                    {
                        fLog.Error( "Failed to stop transport protocol: " + ex );
                    }

                    if ( _connectFailed )
                    {
                        //  Put the agent to sleep and set the zone connection state to
                        //  Disconnected. The client should try and reconnect the zone again.
                        try
                        {
                            Sleep();
                        }
                        catch ( Exception ignored )
                        {
                            Log.Warn( "Error attempting to sleep:" + ignored, ignored );
                        }

                        fState &= ~CONNECTED;
                    }
                }
            }
        }


        private delegate void ResponseDeliveryHandler(ResponseDelivery rd);


        internal void Provision()
        {
            Provision(null);
        }

        ///<summary>Send SIF_Subscribe and SIF_Provide provisioning messages.</summary>
        private void ProvisionLegacy()
        {
            //  - Visit each Topic to which this zone is joined and send a message
            //    if the Topic has a publisher and/or subscriber and no message has
            //    yet been sent for the Topic's data object type.
            //
            //  - Send a message for each data object type for which a subscriber
            //    and/or publisher is registered with this zone (and no message has
            //    yet been sent for that object type.)
            //
            List<String> subSent = new List<String>();
            List<String> pubSent = new List<String>();

            //  Send provisioning messages for all topics to which this zone is
            //  currently joined...
            foreach (ITopic topic in fAgent.TopicFactory.GetAllTopics(SifContext.DEFAULT))
            {
                TopicImpl t = (TopicImpl)topic;
                if (t.fZones.Contains(this))
                {
                    //  This zone is joined to the topic. Send a SIF_Subscribe and/or
                    //  SIF_Provide for the topic's data type.
                    String objType = t.ObjectType;
                    if (t.fSub != null && (t.fSubOpts == null || t.fSubOpts.SendSIFSubscribe))
                    {
                        subSent.Add(objType);
                    }

                    if (t.fPub != null && (t.fPubOpts == null || t.fPubOpts.SendSIFProvide))
                    {
                        pubSent.Add(objType);
                    }
                }
            }


            // Add subscribers
            foreach (
                ProvisionedObject<ISubscriber, SubscriptionOptions> subOption in fProvMatrix.GetAllSubscribers(true))
            {
                SubscriptionOptions subOptions = subOption.ProvisioningOptions;
                if (subSent.Contains(subOption.ObjectType.Name) || !subOptions.SendSIFSubscribe)
                {
                    continue;
                }
                // For legacy provisioning, only add subscribers in the default context
                foreach (SifContext context in subOptions.SupportedContexts)
                {
                    if (context.Equals(SifContext.DEFAULT))
                    {
                        subSent.Add(subOption.ObjectType.Name);
                    }
                    else
                    {
                        Log.Debug(
                            String.Format("SIF_Subscribe will not be sent in legacy mode for {0} in SIF Context {1}",
                                           subOption.ObjectType.Name, context.Name));
                    }
                }
            }

            // Add publishers
            foreach (ProvisionedObject<IPublisher, PublishingOptions> pubOption in fProvMatrix.GetAllPublishers(true)
                )
            {
                PublishingOptions pubOptions = pubOption.ProvisioningOptions;
                if (pubSent.Contains(pubOption.ObjectType.Name) || !pubOptions.SendSIFProvide)
                {
                    continue;
                }
                // For legacy provisioning, only add subscribers in the default context
                foreach (SifContext context in pubOptions.SupportedContexts)
                {
                    if (context.Equals(SifContext.DEFAULT))
                    {
                        pubSent.Add(pubOption.ObjectType.Name);
                    }
                    else
                    {
                        Log.Debug(
                            String.Format("SIF_Provide will not be sent in legacy mode for {0} in SIF Context {1}",
                                           pubOption.ObjectType.Name, context.Name));
                    }
                }
            }

            
            if (subSent.Count > 0)
            {
                if (Properties.BatchProvisioning)
                {
                    //  Send a single SIF_Provide for the set of objects identified above
                    String[] subTypes = subSent.ToArray();
                    SIF_Ack ack = fPrimitives.SifSubscribe(this, subTypes);
                    if (!ack.HasStatusCode(SifStatusCodes.ALREADY_SUBSCRIBED_5) && ack.HasError())
                    {
                        if (!Properties.IgnoreProvisioningErrors)
                        {
                            SifException se = new SifException(ack, this);
                            if (se.ErrorCategory == SifErrorCategoryCode.AccessAndPermissions)
                            {
                                fProvWarnings.Add(se);
                            }
                            else
                            {
                                AdkUtils._throw(se, null);
                            }
                        }
                        else
                        {
                            if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                            {
                                Log.Info("SIF_Subscribe errors ignored for " + ArrayToStr(subTypes));
                            }
                        }
                    }
                    else
                    {
                        if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                        {
                            Log.Info("SIF_Subscribe successful for " + ArrayToStr(subTypes));
                        }
                    }
                }
                else
                {
                    //  Send individual SIF_Provide messages for the set of objects identified above.
                    for (int i = 0; i < subSent.Count; i++)
                    {
                        SIF_Ack ack = fPrimitives.SifSubscribe(this, new String[] { subSent[i] });
                        if (!ack.HasStatusCode(SifStatusCodes.ALREADY_SUBSCRIBED_5) && ack.HasError())
                        {
                            if (!Properties.IgnoreProvisioningErrors)
                            {
                                SifException se = new SifException(ack, this);
                                if (se.ErrorCategory == SifErrorCategoryCode.AccessAndPermissions)
                                    fProvWarnings.Add(se);
                                else
                                    AdkUtils._throw(se, null);
                            }
                            else if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                                Log.Info("SIF_Subscribe errors ignored for " + subSent[i]);
                        }
                        else if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                            Log.Info("SIF_Subscribe successful for " + subSent[i]);
                    }
                }
            }

            if (pubSent.Count > 0)
            {
                if (Properties.BatchProvisioning)
                {
                    //  Send a single SIF_Provide for the set of objects identified above
                    //String[] pubTypes = new String[ pubSent.Count ];
                    string[] pubTypes = pubSent.ToArray();
                    SIF_Ack ack = fPrimitives.SifProvide(this, pubTypes);
                    if (!ack.HasStatusCode(SifStatusCodes.ALREADY_PROVIDER_6) && ack.HasError())
                    {
                        if (!Properties.IgnoreProvisioningErrors)
                        {
                            SifException se = new SifException(ack, this);
                            if (se.ErrorCategory == SifErrorCategoryCode.AccessAndPermissions)
                                fProvWarnings.Add(se);
                            else
                                AdkUtils._throw(se, null);
                        }
                        else if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                            Log.Info("SIF_Provide errors ignored for " + ArrayToStr(pubTypes));
                    }
                    else if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                        Log.Info("SIF_Provide successful for " + ArrayToStr(pubTypes));
                }
                else
                {
                    //  Send individual SIF_Provide messages for the set of objects identified above.
                    String[] pubTypes = new String[pubSent.Count];
                    pubSent.ToArray().CopyTo(pubTypes, 0);
                    //pubSent.ToArray( pubTypes );
                    for (int i = 0; i < pubTypes.Length; i++)
                    {
                        SIF_Ack ack = fPrimitives.SifProvide(this, new String[] { pubTypes[i] });
                        if (!ack.HasStatusCode(SifStatusCodes.ALREADY_PROVIDER_6) && ack.HasError())
                        {
                            if (!Properties.IgnoreProvisioningErrors)
                            {
                                SifException se = new SifException(ack, this);
                                if (se.ErrorCategory == SifErrorCategoryCode.AccessAndPermissions)
                                    fProvWarnings.Add(se);
                                else
                                    AdkUtils._throw(se, null);
                            }
                            else if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                                Log.Info("SIF_Provide errors ignored for " + pubTypes[i]);
                        }
                        else if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                            Log.Info("SIF_Provide successful for " + pubTypes[i]);
                    }
                }
            }
        }

        ///<summary>
        /// Takes a SifElement such as SIF_SubscribeObjects or SIF_RequestObjects and adds an additional
        /// child nodes describing support for each of the objects in the specified provisioning list
        ///</summary>
        ///<param name="addTo">A SIF_SubscribeObjects or SIF_RequestObjects element</param>
        ///<param name="list">A list of objects and associated publishing options related to them</param>
        private void ProvisionHandlers<V, Y>(SifElement addTo, IList<ProvisionedObject<V, Y>> list)
            where Y : ProvisioningOptions
        {
            foreach (ProvisionedObject<V, Y> subOption in list)
            {
                ProvisioningOptions opts = subOption.ProvisioningOptions;
                SIF_Object qr = new SIF_Object(subOption.ObjectType.Name);
                SIF_Contexts qContexts = new SIF_Contexts();
                qr.SIF_Contexts = qContexts;

                if (opts != null)
                {
                    // Add the list of supported contexts for this object
                    foreach (SifContext context in opts.SupportedContexts)
                    {
                        qContexts.AddSIF_Context(context.Name);
                    }
                }
                addTo.AddChild(qr);
            }
        }

        ///<summary>Provisions the agent with the zone</summary>
        ///<param name="acl">The SIF_AgentACL object to use for provisioning, if available, otherwise null</param>
        protected void Provision(SIF_AgentACL acl)
        {
            if (!Connected)
            {
                return;
            }

            if (Properties.ProvisioningMode != AgentProvisioningMode.Adk)
            {
                if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                {
                    Log.Info("Zone not sending provisioning messages because ADK-managed provisioning is not in effect");
                }
                return;
            }

            fProvWarnings.Clear();

            SifVersion effectiveZisVersion = HighestEffectiveZISVersion;
            if ((effectiveZisVersion.CompareTo(SifVersion.SIF20)) < 0 ||
                 Properties.ProvisionInLegacyMode)
            {
                ProvisionLegacy();
            }
            else
            {
                ProvisionSIF20(acl);
            }
        }


        ///<summary> Takes a SifElement such as SIF_ProvideObjects or SIF_RespondObjects and adds an additional
        /// child node describing support for each of the objects in the specified provisioning list
        /// NOTE: the V parameter is unused, but Java is not allowing it to be wildcarded. Strange 
        /// </summary>
        ///<param name="addTo">A SIF_ProvideObjects or SIF_RespondObjects element</param> 
        ///<param name="list"> A list of objects and associated publishing options related to them</param>
        private void ProvisionPublisher<V, Y>(SifElement addTo, IList<ProvisionedObject<V, Y>> list)
            where Y : PublishingOptions
        {
            foreach (ProvisionedObject<V, Y> pubOption in list)
            {
                PublishingOptions pubOptions = pubOption.ProvisioningOptions;
                SIF_Object published = new SIF_Object(pubOption.ObjectType.Name);
                SIF_Contexts pContexts = new SIF_Contexts();
                published.AddChild(pContexts);
                if (pubOptions != null)
                {
                    published.SIF_ExtendedQuerySupport = pubOptions.SupportsExtendedQuery;

                    // Add the list of supported contexts for this object
                    foreach (SifContext context in pubOptions.SupportedContexts)
                    {
                        pContexts.AddSIF_Context(context.Name);
                    }
                }
                addTo.AddChild(published);
            }
        }

        ///<summary>Provisions the agent with the zone using the SIF_Provision message</summary>
        ///<param name="acl">The SIF_AgentACL object, if received from SIF_Register, or null</param>
        private void ProvisionSIF20(SIF_AgentACL acl)
        {
            SIF_SubscribeObjects subscribeObjects = new SIF_SubscribeObjects();
            ;
            SIF_RequestObjects requestObjects = new SIF_RequestObjects();
            SIF_ProvideObjects provideObjects = new SIF_ProvideObjects();
            SIF_RespondObjects respondObjects = new SIF_RespondObjects();

            //
            // Add subscribers
            //
            ProvisionHandlers(subscribeObjects, fProvMatrix.GetAllSubscribers(true));

            //
            // Add requestors
            //
            ProvisionHandlers(requestObjects, fProvMatrix.GetAllQueryResults(true));

            //
            // Add publishers
            //
            IList<ProvisionedObject<IPublisher, PublishingOptions>> publishers = fProvMatrix.GetAllPublishers(true);
            ProvisionPublisher(provideObjects, publishers);
            ProvisionPublisher(respondObjects, publishers);


            //  Send provisioning messages for all topics to which this zone is
            //  currently joined...
            ITopicFactory tFactory = fAgent.TopicFactory;
            foreach (SifContext context in tFactory.AllSupportedContexts)
            {
                foreach (ITopic topic in fAgent.TopicFactory.GetAllTopics(context))
                {
                    TopicImpl t = (TopicImpl)topic;
                    if (t.fZones.Contains(this))
                    {
                        //  This zone is joined to the topic. Send a SIF_Subscribe and/or
                        //  SIF_Provide for the topic's data type.
                        String objType = t.ObjectType;
                        if (t.fSub != null && (t.fSubOpts == null || t.fSubOpts.SendSIFSubscribe))
                        {
                            AddProvisionedObject(subscribeObjects, objType, context, false);
                        }

                        if (t.fQueryResults != null)
                        {
                            AddProvisionedObject(requestObjects, objType, context,
                                                  t.fQueryResultsOptions.getSupportsExtendedQuery());
                        }

                        if (t.fPub != null && (t.fPubOpts == null || t.fPubOpts.SendSIFProvide))
                        {
                            AddProvisionedObject(provideObjects, objType, context, t.fPubOpts.SupportsExtendedQuery);
                            AddProvisionedObject(respondObjects, objType, context, t.fPubOpts.SupportsExtendedQuery);
                        }
                    }
                }
            }

            // Get the SIF_ZoneStatus and SIF_AgentACL objects. Compare that matrix
            // with the list of provisioned objects in the agent. If there is a different provider
            // for an object, or an ACL restricts this agent from participating, log the anomaly and
            // remove the object from the list

            // The zone status check only needs to be done if this agent provides any objects
            if (provideObjects.ChildCount > 0)
            {
                SIF_ZoneStatus zoneStatus = GetZoneStatus();
                if (zoneStatus == null)
                {
                    // should never be null
                    Log.Warn("Unable to obtain SIF_ZoneStatus for provisioning.");
                }
                else
                {
                    // Remove any provided objects if they are provided by any other agent
                    SIF_Providers zoneProviders = zoneStatus.SIF_Providers;
                    if (zoneProviders != null)
                    {
                        foreach (SIF_Provider zoneProvider in zoneProviders)
                        {
                            //for( SIF_Provider zoneProvider : zoneProviders.getSIF_Providers() ){
                            if (!zoneProvider.SourceId.Equals(Agent.Id) && zoneProvider.SIF_ObjectList != null)
                            {
                                // determine if any objects being provided match ones this agent wants 
                                // to provide
                                foreach (SIF_Object zoneProvidedObject in zoneProvider.SIF_ObjectList)
                                {
                                    SIF_Object po =
                                        (SIF_Object)
                                        provideObjects.GetChild(InfraDTD.SIF_PROVIDEOBJECTS_SIF_OBJECT,
                                                                 new String[] { zoneProvidedObject.ObjectName.ToString() });

                                    //SIF_Object po = publishObjects.getSIF_Object(zoneProvidedObject.ObjectName);
                                    if (po != null)
                                    {
                                        SIF_Contexts publishContexts =
                                            (SIF_Contexts)po.GetChild(InfraDTD.SIF_OBJECT_SIF_CONTEXTS);
                                        // SIF_Contexts publishContexts = po.getSIF_Contexts();
                                        // Remove any contexts supported by this provider
                                        SIF_Contexts zoneObjectContexts =
                                            (SIF_Contexts)
                                            zoneProvidedObject.GetChild(InfraDTD.SIF_OBJECT_SIF_CONTEXTS);
                                        ;
                                        //SIF_Contexts zoneObjectContexts = zoneProvidedObject.getSIF_Contexts();
                                        if (zoneObjectContexts == null)
                                        {
                                            publishContexts.RemoveChild(InfraDTD.SIF_OBJECT_SIF_CONTEXTS,
                                                                         SifContext.DEFAULT.Name);
                                            // publishContexts.removeSIF_Context(SifContext.DEFAULT.Name);
                                            if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                                            {
                                                Log.Info("Unable to provide " + po.ObjectName +
                                                          " in SIF Context " + SifContext.DEFAULT.Name +
                                                          " because it is being provided by " + zoneProvider.SourceId);
                                            }
                                        }
                                        else
                                        {
                                            Array ar = zoneObjectContexts.ToArray();

                                            foreach (SIF_Context context in ar)
                                            {
                                                publishContexts.RemoveChild(InfraDTD.SIF_CONTEXTS_SIF_CONTEXT,
                                                                             new string[] { context.Value });
                                                //publishContexts.removeSIF_Context(context.Value);
                                                if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                                                {
                                                    Log.Info("Unable to provide " + po.ObjectName +
                                                              " in SIF Context " + context.Value +
                                                              " because it is being provided by " +
                                                              zoneProvider.SourceId);
                                                }
                                            }
                                        }
                                        // If there are not remaining contexts left, remove the
                                        // support for publishing this object
                                        if (publishContexts.ChildCount == 0)
                                        {
                                            provideObjects.RemoveChild(po);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Now check Agent ACL permissions
            if (acl == null)
            {
                acl = GetAgentACL();
            }
            if (acl == null)
            {
                // Should never be null
                Log.Warn("Unable to obtain SIF_AgentACL for provisioning.");
            }
            else
            {
                compareToACL(acl.SIF_ProvideAccess, provideObjects, "Publish");
                compareToACL(acl.SIF_RespondAccess, respondObjects, "Respond to");
                compareToACL(acl.SIF_SubscribeAccess, subscribeObjects, "Subscribe to");
                compareToACL(acl.SIF_RequestAccess, requestObjects, "Request");
            }

            SIF_Ack ack = fPrimitives.SifProvision(
                this, provideObjects,
                subscribeObjects,
                new SIF_PublishAddObjects(),
                new SIF_PublishChangeObjects(),
                new SIF_PublishDeleteObjects(),
                requestObjects,
                respondObjects);
            if (ack.HasError())
            {
                SifException se = new SifException(ack, this);
                AdkUtils._throw(se, null);
            }
            else
            {
                if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                {
                    Log.Info("SIF_Provision successful");
                }
            }
        }

        ///<summary>Checks to see if the list of Published objects already contains the instance. If so, a check
        /// is done to ensure that the specified context is included.
        /// </summary>
        /// <param name="context"></param>
        ///<param name="objectName"></param>
        ///<param name="provObjects"></param>
        ///<param name="supportsSIFExtendedQuery"></param>
        private void AddProvisionedObject(
            SifElement provObjects,
            String objectName,
            SifContext context,
            Boolean supportsSIFExtendedQuery)
        {
            SIF_Object lookedUp = (SIF_Object)provObjects.GetChild(InfraDTD.SIF_OBJECT.Name, objectName);
            if (lookedUp == null)
            {
                lookedUp = new SIF_Object(objectName);
                lookedUp.SIF_Contexts = new SIF_Contexts();
                provObjects.AddChild(lookedUp);
            }
            if (supportsSIFExtendedQuery)
            {
                lookedUp.SIF_ExtendedQuerySupport = true;
            }
            SIF_Contexts contexts = lookedUp.SIF_Contexts;
            // SIF_Context ctxt = contexts.getSIF_Context(context.Name);
            SIF_Context ctxt =
                (SIF_Context)contexts.GetChild(InfraDTD.SIF_CONTEXTS_SIF_CONTEXT, new string[] { context.Name });
            if (ctxt == null)
            {
                contexts.AddSIF_Context(context.Name);
            }
        }


        ///<summary> Looks at all of the object actions currently desired to be provisioned by the agent. If the
        /// corresponding ACL is not found in the ACL list, a warning is written to the log and the 
        /// corresponding provisioning option is removed from the desiredProvisionedObjects list
        ///</summary>
        ///<param name="aclPermissions"> A SifElement instance from SIF_AgentACL</param>
        ///<param name="desiredProvisionedObjects">a ProvisionedObjects or PublishedObjects instance for 
        /// the corresponding provisioning action</param>
        ///<param name="actionString"></param>
        private void compareToACL(SifElement aclPermissions, SifElement desiredProvisionedObjects, String actionString)
        {
            IList<SifElement> children = desiredProvisionedObjects.GetChildList();
            SifElement[] allProvisionedObjects = new SifElement[ children.Count ];
            children.CopyTo( allProvisionedObjects, 0 );
            foreach (SifElement provisionedObject in allProvisionedObjects)
            {
                // TODO: It would be nice to be able to better support inheritance. This method is called
                // with both ProvisionedObjects and PublishedObjects as its second parameter. They are similar,
                // but PublishedObjects has an additional element child

                String objectName = provisionedObject.GetFieldValue(InfraDTD.SIF_OBJECT_OBJECTNAME);
                SifElement aclObject = null;
                if (aclPermissions != null)
                {
                    aclObject = aclPermissions.GetChild(InfraDTD.SIF_OBJECT, objectName);
                }

                SIF_Contexts aclContexts = null;
                if (aclObject != null)
                {
                    aclContexts = (SIF_Contexts)aclObject.GetChild(InfraDTD.SIF_CONTEXTS.Name);
                }

                if (aclContexts == null)
                {
                    desiredProvisionedObjects.RemoveChild(provisionedObject);
                    if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                    {
                        Log.Info("Unable to " + actionString + " " + objectName +
                                  " because the agent does not have ACL permission to do so.");
                    }
                }
                else
                {
                    List<SIF_Context> contextsToRemove = new List<SIF_Context>();
                    SIF_Contexts provContexts = (SIF_Contexts)provisionedObject.GetChild(InfraDTD.SIF_CONTEXTS.Name);
                    foreach (SIF_Context desiredContext in provContexts)
                    {
                        SIF_Context aclContext =
                            (SIF_Context)
                            aclContexts.GetChild(InfraDTD.SIF_CONTEXTS_SIF_CONTEXT, new String[] { desiredContext.Value });
                        if (aclContext == null)
                        {
                            contextsToRemove.Add(desiredContext);
                            if ((Adk.Debug & AdkDebugFlags.Provisioning) != 0)
                            {
                                Log.Info("Continuing to " + actionString + " " + objectName +
                                          " in Context \"" + desiredContext.Value +
                                          "\" in spite of the fact that the agent does not have ACL permission to do so.");
                                //Log.Info("Unable to " + actionString + " " + objectName +
                                //          " in Context \"" + desiredContext.Value +
                                //          "\" because the agent does not have ACL permission to do so.");
                            }
                        }
                    }

                    //foreach ( SIF_Context contextToRemove in contextsToRemove ) {
                    //    provContexts.RemoveChild(contextToRemove);
                    //}

                    if (provContexts.ChildCount == 0) {
                        desiredProvisionedObjects.RemoveChild(provisionedObject);
                    }
                }
            }
        }


        protected virtual string ArrayToStr( string[] arr )
        {
            return string.Join(", ", arr);
        }

        /// <summary>  Disconnects the agent from this zone</summary>
        public virtual void Disconnect(ProvisioningFlags provOptions)
        {
            lock (fConnLock)
            {
                if (!Connected)
                {
                    return;
                }

                SifException exc = null;

                try
                {
                    if (fQueue != null)
                    {
                        fQueue.Shutdown();
                        fQueue = null;
                    }

                    if ((provOptions & ProvisioningFlags.Unregister) != 0)
                    {
                        if (Properties.ProvisioningMode == AgentProvisioningMode.Adk)
                        {
                            fPrimitives.SifUnregister(this);
                        }
                    }
                    else
                    {
                        if (Properties.SleepOnDisconnect &&
                             Properties.ProvisioningMode == AgentProvisioningMode.Adk)
                        {
                            try
                            {
                                Sleep();
                            }
                            catch (SifException se)
                            {
                                //	Ignore Category 4, Code 9 error (invalid SourceId, which
                                //	with Library would be the case if the admin manually
                                //	unregistered the agent but the agent still thinks it is
                                //	connected.
                                //
                                if (
                                    !
                                    (se.ErrorCategory == SifErrorCategoryCode.AccessAndPermissions &&
                                     se.ErrorCode == SifErrorCodes.ACCESS_UNKNOWN_SOURCEID_9))
                                {
                                    exc = se;
                                }
                            }
                        }
                    }

                    //  Shutdown the message fDispatcher and transport protocol
                    Shutdown();
                }
                finally
                {
                    fState |= CLOSED;
                    fState &= ~CONNECTED;
                }

                if (exc != null)
                {
                    throw exc;
                }
            }
        }

        /// <summary>  Puts this zone into sleep mode.
        /// 
        /// A SIF_Sleep message is sent to the Zone Integration Server to request
        /// that this agent's queue be put into sleep mode. If successful, the ZIS
        /// should not deliver further messages to this agent until it is receives
        /// a SIF_Register or SIF_Wakeup message from the agent. Note the Adk keeps
        /// an internal sleep flag for each zone, which is initialized when the
        /// <c>connect</c> method is called by sending a SIF_Ping to the ZIS.
        /// This flag is set so that the Adk will return a Status Code 8 ("Receiver
        /// is sleeping") in response to any message received by the ZIS for the
        /// duration of the session.
        /// 
        /// 
        /// If the SIF_Sleep message is not successful, an exception is thrown and
        /// the Adk's internal sleep flag for this zone is not changed.
        /// 
        /// 
        /// </summary>
        /// <exception cref="AdkException">  thrown if the SIF_Sleep message is unsuccessful
        /// </exception>
        public virtual void Sleep()
        {
            CheckConnect();

            fState |= SLEEPING;

            SIF_Ack ack = SifSleep();
            if (ack.HasError())
            {
                AdkUtils._throw(new SifException(ack, this), fLog);
            }
        }

        /// <summary>  Execute a SIF_Sleep received from the ZIS</summary>
        public virtual void ExecSleep()
        {
            //  TODO: Notify the zone's ResponseDelivery thread to pause.

            fState |= SLEEPING;
        }

        /// <summary>  Wakes up this zone if currently in sleep mode.</summary>
        public virtual void WakeUp()
        {
            CheckConnect();

            fState &= ~SLEEPING;

            SIF_Ack ack = SifWakeup();
            if (ack.HasError())
            {
                AdkUtils._throw(new SifException(ack, this), fLog);
            }
        }

        /// <summary>  Execute a SIF_Wakeup received from the ZIS</summary>
        public virtual void ExecWakeup()
        {
            //  TODO: Notify the zone's ResponseDelivery thread to continue
            //  sending any SIF_Responses that are pending delivery but were
            //  interrupted due to a SIF_Sleep ( see execSleep() )

            fState &= ~SLEEPING;
        }

        /// <summary>  Determines if the agent's queue for this zone is in sleep mode.
        /// 
        /// </summary>
        /// <param name="flags">When AdkFlags.LOCAL_QUEUE is specified, returns true if the
        /// Agent Local Queue is currently in sleep mode. False is returned if
        /// the Agent Local Queue is disabled. When AdkFlags.SERVER_QUEUE is
        /// specified, queries the sleep mode of the Zone Integration Server
        /// by sending a SIF_Ping message.
        /// </param>
        public virtual bool IsSleeping(AdkQueueLocation flags)
        {
            CheckConnect();

            if ((flags & AdkQueueLocation.QUEUE_SERVER) != 0)
            {
                //  Determine if agent's queue on server is sleeping
                SIF_Ack ack = SifPing();
                if (ack.HasError())
                {
                    AdkUtils._throw(new SifException(ack, this), fLog);
                }
                return (ack.HasStatusCode(SifStatusCodes.SLEEPING_8));
            }
            else if ((flags & AdkQueueLocation.QUEUE_LOCAL) != 0)
            {
                //  TODO: Move state into ALQ object when it exists
                return ((fState & SLEEPING) != 0);
            }
            else
            {
                throw new ArgumentException("Invalid flags (specify SERVER_QUEUE or LOCAL_QUEUE)");
            }
        }

        /// <summary>  Determines if this zone is awaiting a SIF_ZoneStatus result</summary>
        protected internal virtual bool AwaitingZoneStatus()
        {
            return (fState & GETZONESTATUS) != 0;
        }

        public virtual void ReportEvent(SifDataObject obj,
                                         EventAction actionCode)
        {
            ReportEvent(new Event(new SifDataObject[] { obj }, actionCode), null, null);
        }

        public virtual void ReportEvent(SifDataObject obj,
                                         EventAction actionCode,
                                         string destinationId)
        {
            ReportEvent(new Event(new SifDataObject[] { obj }, actionCode), destinationId, null);
        }

        public virtual void ReportEvent(Event evnt)
        {
            ReportEvent(evnt, null, null);
        }

        public virtual void ReportEvent(Event evnt,
                                         string destinationId)
        {
            ReportEvent(evnt, destinationId, null);
        }

        public void ReportEvent(Event evnt,
                                 String destinationId,
                                 string sifMsgId)
        {
            CheckConnect();

            SIF_Ack ack = fPrimitives.SifEvent(this, evnt, destinationId, sifMsgId);
            if (ack.HasError())
            {
                AdkUtils._throw(new SifException(ack, this), fLog);
            }
        }

        /// <summary> 	Report an informative message to the zone in the form of a SIF_LogEntry object.
        /// 
        /// </summary>
        /// <param name="desc">A textual description
        /// </param>
        public virtual void ReportInfoLogEntry(string desc)
        {
            ServerLog.Log(desc);
        }

        /// <summary> 	Report an informative message to the zone in the form of a SIF_LogEntry object.
        /// 
        /// </summary>
        /// <param name="desc">A textual description
        /// </param>
        /// <param name="extDesc">An optional extended description, or null if not applicable
        /// </param>
        /// <param name="appCode">An optional application-defined code
        /// </param>
        /// <param name="objects">An optional array of one or more SIFDataObjects to be
        /// included with the SIF_LogEntry, or null if not applicable
        /// </param>
        public virtual void ReportInfoLogEntry(string desc,
                                                string extDesc,
                                                string appCode,
                                                SifDataObject[] objects)
        {
            ServerLog.Log(LogLevel.INFO, desc, extDesc, appCode, null, objects);
        }

        /// <summary> 	Report a warning message to the zone in the form of a SIF_LogEntry object.
        /// 
        /// </summary>
        /// <param name="desc">A textual description
        /// </param>
        /// <param name="extDesc">An optional extended description, or null if not applicable
        /// </param>
        /// <param name="category">The SIF_LogEntry category
        /// </param>
        /// <param name="code">The SIF_LogEntry code
        /// </param>
        /// <param name="appCode">An optional application-defined code
        /// </param>
        /// <param name="objects">An optional array of one or more SIFDataObjects to be
        /// included with the SIF_LogEntry, or null if not applicable
        /// </param>
        public virtual void ReportWarningLogEntry(string desc,
                                                   string extDesc,
                                                   int category,
                                                   int code,
                                                   string appCode,
                                                   SifDataObject[] objects)
        {
            ServerLog.Log(LogLevel.WARNING, desc, extDesc, appCode, category, code, null, objects);
        }

        /// <summary> 	Report a warning message to the zone in the form of a SIF_LogEntry object.
        /// Use this form of the <c>reportWarningLogEntry</c> method if the warning
        /// references a SIF_Message received by the agent.
        /// 
        /// </summary>
        /// <param name="msgInfo">A SifMessageInfo instance describing the SIF_Message that
        /// to which this warning relates. A SifMessageInfo value is passed to all
        /// message handler functions by the Adk.
        /// </param>
        /// <param name="desc">A textual description
        /// </param>
        /// <param name="extDesc">An optional extended description, or null if not applicable
        /// </param>
        /// <param name="category">The SIF_LogEntry category
        /// </param>
        /// <param name="code">The SIF_LogEntry code
        /// </param>
        /// <param name="appCode">An optional application-defined code
        /// </param>
        /// <param name="objects">An optional array of one or more SIFDataObjects to be
        /// included with the SIF_LogEntry, or null if not applicable
        /// </param>
        public virtual void ReportWarningLogEntry(SifMessageInfo msgInfo,
                                                   string desc,
                                                   string extDesc,
                                                   int category,
                                                   int code,
                                                   string appCode,
                                                   SifDataObject[] objects)
        {
            ServerLog.Log(LogLevel.WARNING, desc, extDesc, appCode, category, code, msgInfo, objects);
        }

        /// <summary> 	Report an error message to the zone in the form of a SIF_LogEntry object.
        /// 
        /// </summary>
        /// <param name="desc">A textual description
        /// </param>
        /// <param name="extDesc">An optional extended description, or null if not applicable
        /// </param>
        /// <param name="category">The SIF_LogEntry category
        /// </param>
        /// <param name="code">The SIF_LogEntry code
        /// </param>
        /// <param name="appCode">An optional application-defined code
        /// </param>
        /// <param name="objects">An optional array of one or more SIFDataObjects to be
        /// included with the SIF_LogEntry, or null if not applicable
        /// </param>
        public virtual void ReportErrorLogEntry(string desc,
                                                 string extDesc,
                                                 int category,
                                                 int code,
                                                 string appCode,
                                                 SifDataObject[] objects)
        {
            ServerLog.Log(LogLevel.ERROR, desc, extDesc, appCode, category, code, null, objects);
        }

        /// <summary> 	Report an error message to the zone in the form of a SIF_LogEntry object.
        /// Use this form of the <c>reportErrorLogEntry</c> method if the error
        /// references a SIF_Message received by the agent.
        /// 
        /// </summary>
        /// <param name="msgInfo">A SifMessageInfo instance describing the SIF_Message that
        /// to which this error relates. A SifMessageInfo value is passed to all
        /// message handler functions by the Adk.
        /// </param>
        /// <param name="desc">A textual description
        /// </param>
        /// <param name="extDesc">An optional extended description, or null if not applicable
        /// </param>
        /// <param name="category">The SIF_LogEntry category
        /// </param>
        /// <param name="code">The SIF_LogEntry code
        /// </param>
        /// <param name="appCode">An optional application-defined code
        /// </param>
        /// <param name="objects">An optional array of one or more SIFDataObjects to be
        /// included with the SIF_LogEntry, or null if not applicable
        /// </param>
        public virtual void ReportErrorLogEntry(SifMessageInfo msgInfo,
                                                 string desc,
                                                 string extDesc,
                                                 int category,
                                                 int code,
                                                 string appCode,
                                                 SifDataObject[] objects)
        {
            ServerLog.Log(LogLevel.ERROR, desc, extDesc, appCode, category, code, msgInfo, objects);
        }


        /// <summary>  Register a Publisher with this zone for all SIF object types.
        /// 
        /// The agent is expected to be registered with the zone as a Provider of
        /// one or more object types. This method does not send SIF_Provide messages
        /// to the zone.
        /// 
        /// </summary>
        /// <param name="publisher">An object that implements the <c>Publisher</c>
        /// interface to respond to SIF_Request queries received by the agent.
        /// This object will be called whenever a SIF_Request is received on
        /// this zone and no other object in the message dispatching chain has
        /// processed the message.
        /// </param>
        public virtual void SetPublisher(IPublisher publisher)
        {
            fProvMatrix.SetPublisher(publisher);
        }

        /// <summary>  Register a Publisher with this zone for the specified SIF object type.
        /// 
        /// </summary>
        /// <param name="publisher">An object that implements the <c>Publisher</c>
        /// interface to respond to SIF_Request queries received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This Publisher will be called whenever a
        /// SIF_Request is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        /// 
        /// </param>
        /// <param name="objectType">The object type this Publisher will respond to.
        /// Object types are defined by classes that implement the SifDtd
        /// interface (e.g. OpenADK.Library.SIF10r1.STUDENTPERSONAL).
        /// Only SIF_Request messages that reference this object type will be
        /// forwarded to the Publisher for processing.
        /// 
        /// </param>
        public virtual void SetPublisher(IPublisher publisher,
                                          IElementDef objectType)
        {
            fProvMatrix.SetPublisher(publisher, objectType);
            Provision();
        }


        /// <summary>  Register a Publisher with this zone for the specified SIF object type.
        /// 
        /// </summary>
        /// <param name="publisher">An object that implements the <c>Publisher</c>
        /// interface to respond to SIF_Request queries received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This Publisher will be called whenever a
        /// SIF_Request is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        /// 
        /// </param>
        /// <param name="objectType">The object type this Publisher will respond to.
        /// Object types are defined by classes that implement the SifDtd
        /// interface (e.g. OpenADK.Library.SIF10r1.STUDENTPERSONAL).
        /// Only SIF_Request messages that reference this object type will be
        /// forwarded to the Publisher for processing.
        /// 
        /// </param>
        /// <param name="options">Provisioning flags. Specify <c>AdkFlags.PROV_PROVIDE</c>
        /// to send a SIF_Provide message to the zone. If the zone is currently
        /// connected, the message is sent immediately; otherwise it is sent
        /// when the <c>connect</c> method is called
        /// </param>
        public virtual void SetPublisher(IPublisher publisher,
                                          IElementDef objectType,
                                          PublishingOptions options)
        {
            fProvMatrix.SetPublisher(publisher, objectType, options);
            Provision();
        }






        /// <summary>  Register a Subscriber with this zone for the specified SIF object type.
        /// 
        /// </summary>
        /// <param name="subscriber">An object that implements the <c>Subscriber</c>
        /// interface to respond to SIF_Event notifications received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This Subscriber will be called whenever a
        /// SIF_Event is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        /// 
        /// </param>
        /// <param name="objectType">The object type this Subscriber will respond to.
        /// Object types are defined by classes that implement the SifDtd
        /// interface (e.g. OpenADK.Library.SIF10r1.STUDENTPERSONAL).
        /// Only SIF_Event messages that reference this object type will be
        /// forwarded to the Subscriber for processing.
        /// 
        /// </param>
        public void SetSubscriber(ISubscriber subscriber, IElementDef objectType)
        {
            fProvMatrix.SetSubscriber(subscriber, objectType);
            Provision();
        }

        /// <summary>  Register a Subscriber with this zone for the specified SIF object type.
        /// 
        /// </summary>
        /// <param name="subscriber">An object that implements the <c>Subscriber</c>
        /// interface to respond to SIF_Event notifications received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This Subscriber will be called whenever a
        /// SIF_Event is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        /// 
        /// </param>
        /// <param name="objectType">The object type this Subscriber will respond to.
        /// Object types are defined by classes that implement the SifDtd
        /// interface (e.g. OpenADK.Library.SIF10r1.STUDENTPERSONAL).
        /// Only SIF_Event messages that reference this object type will be
        /// forwarded to the Subscriber for processing.
        /// 
        /// </param>
        /// <param name="options">Provisioning flags. Specify <c>AdkFlags.PROV_SUBSCRIBE</c>
        /// to send a SIF_Subscribe message to the zone. If the zone is currently
        /// connected, the message is sent immediately; otherwise it is sent
        /// when the <c>connect</c> method is called
        /// </param>
        public void SetSubscriber(ISubscriber subscriber, IElementDef objectType, SubscriptionOptions options)
        {
            fProvMatrix.SetSubscriber(subscriber, objectType, options);
            Provision();
        }


        public void SetQueryResults(IQueryResults queryResults)
        {
            fProvMatrix.SetQueryResults(queryResults, null, null);
        }


        public void SetQueryResults(IQueryResults queryResults, IElementDef objectType)
        {
            fProvMatrix.SetQueryResults(queryResults, objectType);
        }


        public void SetQueryResults(IQueryResults queryResults, IElementDef objectType, QueryResultsOptions flags)
        {
            fProvMatrix.SetQueryResults(queryResults, objectType, flags);
        }

        ///<summary> Gets the Publisher for a SIF object type
        ///</summary>
        ///<param name="context">The SIFContext to obtain the publisher for
        ///</param>
        ///<param name="objectType">A SIFDTD constant identifying a SIF Data Object type
        ///(e.g. <code>SIFDTD.STUDENTPERSONAL</code>)</param>
        ///<returns>The Publisher registered for this object type by the agent when
        ///it called the setPublisher method, or null if no Publisher has been
        ///registered
        ///</returns>
        public IPublisher GetPublisher(SifContext context, IElementDef objectType)
        {
            return fProvMatrix.LookupPublisher(context, objectType);
        }

        /// <summary>  Gets the Subscriber for a SIF object type</summary>
        /// <param name="context">The SIF Context that the subscriber is provisioned in</param>
        /// <param name="objectType">A SifDtd constant identifying a SIF Data Object type
        /// (e.g. <c>SifDtd.STUDENTPERSONAL</c>)
        /// </param>
        /// <returns> The Subscriber registered for this object type by the agent when
        /// it called the setSubscriber method, or null if no Subscriber has
        /// been registered
        /// </returns>
        public virtual ISubscriber GetSubscriber(SifContext context, IElementDef objectType)
        {
            return fProvMatrix.LookupSubscriber(context, objectType);
        }


        /// <summary>
        /// Gets the QueryResults object for a SIF object type
        /// </summary>
        /// <param name="context">The SIF Context to use for looking up the handler</param>
        /// <param name="objectType">A SIFDTD constant identifying a SIF Data Object type
        /// (e.g. <code>SIFDTD.STUDENTPERSONAL</code>)</param>
        /// <returns>The QueryResults object registered for this object type by the
        /// agent when it called the setQueryResults method, or null if no
        /// QueryResults object has been registered</returns>
        public virtual IQueryResults GetQueryResults(SifContext context, IElementDef objectType)
        {
            return fProvMatrix.LookupQueryResults(context, objectType);
        }


        public virtual string Query(Query query)
        {
            return Query(query, null, null, 0, null);
        }

        public virtual string Query(Query query,
                                     IMessagingListener listener)
        {
            return Query(query, listener, null, 0, null);
        }

        public virtual string Query(Query query,
                                     AdkQueryOptions queryOptions)
        {
            return Query(query, null, null, queryOptions, null);
        }

        public virtual string Query(Query query,
                                     IMessagingListener listener,
                                     AdkQueryOptions queryOptions)
        {
            return Query(query, listener, null, queryOptions, null);
        }

        public virtual string Query(Query query,
                                     string destinationId,
                                     AdkQueryOptions queryOptions)
        {
            return Query(query, null, destinationId, queryOptions, null);
        }

        public virtual string Query(Query query,
                                     IMessagingListener listener,
                                     string destinationId,
                                     AdkQueryOptions queryOptions)
        {
            return Query(query, listener, destinationId, queryOptions, null);
        }


        /// <summary>
        /// Undocumented version of the <code>query</code> method that allows the caller to 
        /// specify the SIF_MsgId that will be used in the SIF_Message/SIF_Header.
        /// </summary>
        /// <remarks>
        /// This method is not part of the public Zone interface. It is intended to be used 
        /// by agents that must decide upon the SIF_MsgId of a query before the ADK is called, 
        /// or that want to choose the SIF_MsgId that will be used for a query for debugging 
        /// purposes or to satisfy some other special requirement. (Library Student Locator 
        /// agent is an example of an agent that requires this method.)
        /// </remarks>
        /// <param name="query">A Query object describing the parameters of the query,
        /// including optional conditions and field restrictions</param>
        /// <param name="listener">A MessagingListener that will be notified when the
        /// SIF_Request message is sent to the zone. Any other MessagingListeners 
        /// registered with the zone will also be called. </param>
        /// <param name="destinationId">The SourceId of the agent to which the SIF Request
        /// will be routed by the zone integration server</param>
        /// <param name="queryOptions">[Reserved for future use]</param>
        /// <param name="sifMsgId">The value to assign to the SIF_Message/SIF_Header/SIF_MsgId
        /// element, or <code>null</code> to let the framework assign its own value</param>
        /// <returns>The SIF_MsgId of the SIF_Request that was sent to the zone.</returns>
        /// <exception cref="AdkException">If the query cannot be sent</exception>
        public String Query(Query query,
                             IMessagingListener listener,
                             String destinationId,
                             AdkQueryOptions queryOptions,
                             String sifMsgId)
        {
            //	Synchronized to prevent MessageDispatcher from processing a SIF_Response (on the 
            //	zone thread) while this thread is issuing a SIF_Request. This is required to ensure 
            //	that the query() method completes before any SIF_Response processing is performed.
            //	It can lead to situations where the RequestCache is not updated in time, and/or
            //	the QueryResults.onQueryResults method is called before the onQueryPending method
            //	is called to notify the agent that a request was issued.
            //
            lock (fReqLock)
            {
                CheckConnect();

                SIF_Request req;

                try
                {
                    SIF_Ack ack = fPrimitives.SifRequest(this, query, destinationId, sifMsgId);
                    if (ack.HasError())
                    {
                        AdkUtils._throw(new SifException(ack, this), fLog);
                    }

                    req = (SIF_Request)ack.message;

                    //  Record the request in the agent's RequestCache
                    IRequestInfo reqInfo =
                        fDispatcher.RequestCache.StoreRequestInfo(req, query, this);

                    //  Call the onQueryResults.onQueryPending method...
                    IQueryResults target =
                        fDispatcher.getQueryResultsTarget(null, req, null, query, this);
                    if (target != null)
                    {
                        SifMessageInfo inf = new SifMessageInfo(ack.message, this);
                        inf.SIFRequestMsgId = inf.MsgId;
                        inf.SIFRequestVersions = query.SifVersions;
                        inf.SIFRequestObjectType = query.ObjectType;
                        inf.SIFRequestInfo = reqInfo;
#if PROFILED
						try
						{
							ProfilerUtils.profileStart(  com.OpenADK.sifprofiler.api.OIDs.ADK_SIFREQUEST_REQUESTOR_MESSAGING.ToString(), query.ObjectType, inf.MsgId );

#endif
                        target.OnQueryPending(inf, this);
#if PROFILED
						}
						finally
						{
								ProfilerUtils.profileStop();
						}
#endif
                    }
                }
                catch (AdkException)
                {
                    throw;
                }

                //	New to 1.5: Return the SIF_Request/SIF_MsgId to the caller
                return req.Header.SIF_MsgId;
            }
        }

        /// <summary>
        /// Blocks the calling thread until Zone.onQuery has completed.
        /// </summary>
        /// <remarks>
        /// <seealso cref="IZone.Query(Library.Query)"/>
        /// <seealso cref="IZone.Query(Library.Query,AdkQueryOptions)"/>
        /// <seealso cref="IZone.Query(Library.Query,IMessagingListener)"/>
        /// </remarks>
        internal void WaitForRequestsToComplete()
        {
            lock (fReqLock)
            {
                //	do nothing - synchronization on fReqLock hidden within this method 
                //	so we can shield the caller from knowledge of our protected data
                //	members and have the room to expand on this functionality later on.
            }
        }


        /// <summary>  Gets the SIF_ZoneStatus object from the ZIS managing this zone. The
        /// method blocks until a result is received.
        /// </summary>
        public virtual SIF_ZoneStatus GetZoneStatus()
        {
            return GetZoneStatus(Properties.DefaultTimeout);
        }

        /// <summary>  Gets the SIF_ZoneStatus object from the ZIS managing this zone. The
        /// method blocks for the specified timeout period.
        /// 
        /// <b>Note</b> getZoneStatus cannot be called from the Subscriber.onEvent
        /// method when the agent is operating in Pull mode or a deadlock situation
        /// may arise. If you must obtain a SIF_ZoneStatus while processing a
        /// SIF Event, use the TrackQueryResults object to first issue a query for
        /// the object, then call this method to wait for the reply to be
        /// received. TrackQueryObjects is the only safe way to perform queries
        /// while processing SIF Events because it can invoke Selective Message
        /// Blocking if necessary.
        /// 
        /// </summary>
        /// <param name="timeout">The amount of time to wait for a SIF_ZoneStatus object to
        /// be received by the agent (or 0 to wait infinitely)
        /// </param>
        /// <returns>s The SIF_ZoneStatus object, or null if no response was received
        /// within the specified timeout period
        /// 
        /// </returns>
        /// <exception cref="AdkException">  is thrown if an error occurs
        /// </exception>
        public virtual SIF_ZoneStatus GetZoneStatus(TimeSpan timeout)
        {
            CheckConnect();

            lock (fZSLock)
            {
                try
                {
                    fState |= GETZONESTATUS;

                    if (Properties.UseZoneStatusSystemControl)
                    {
                        //
                        //	SIF 1.5 EXPERIMENTAL / SIF 2.0 Official - Use a SIF_SystemControl message to
                        //	immediately retrieve the SIF_ZoneStatus
                        //
                        SIF_Ack ack = fPrimitives.SifZoneStatus(this);
                        if (ack.HasError())
                        {
                            throw new SifException(ack, this);
                        }

                        //	The SIF_AgentACL object will be in the SIF_Ack/SIF_Data
                        SIF_Status status = ack.SIF_Status;
                        if (status != null)
                        {
                            SIF_Data data = status.SIF_Data;
                            if (data != null)
                            {
                                SifElementList childList = data.GetChildList(InfraDTD.SIF_ZONESTATUS);
                                if (childList != null && childList.Count > 0)
                                {
                                    return (SIF_ZoneStatus)childList[0];
                                }
                            }
                        }
                        return null;
                    }
                    else
                    {
                        fZoneStatus = null;

                        SIF_Ack ack = fPrimitives.SifRequest
                            (
                            this,
                            new Query(InfraDTD.SIF_ZONESTATUS),
                            null,
                            null);
                        if (ack.HasError())
                        {
                            throw new SifException(ack, this);
                        }

                        //  ** When in pull mode: force a pull immediately regardless of
                        //     the usual pull frequency (e.g. if it is 22 seconds until the
                        //     next pull we don't want to sit and wait here for the
                        //     SIF_ZoneStatus to be returned in 22 seconds.) **
                        if (Properties.MessagingMode == AgentMessagingMode.Pull)
                        {
                            fDispatcher.Pull();
                        }

                        //  Wait for MessageDispatcher to hand us the response
                        if (fZoneStatus == null)
                        {
                            try
                            {
                                if ((Adk.Debug & AdkDebugFlags.Messaging) != 0)
                                {
                                    fLog.Debug
                                        ("Waiting " + timeout + "ms for SIF_ZoneStatus reply...");
                                }

                                bool received = Monitor.Wait(fZSLock, timeout);

                                if ((Adk.Debug & AdkDebugFlags.Messaging) != 0)
                                {
                                    if (received)
                                    {
                                        fLog.Debug("Received SIF_ZoneStatus reply...");
                                    }
                                    else
                                    {
                                        fLog.Debug
                                            (
                                            "Did not receive SIF_ZoneStatus reply within the expected timeout period.");
                                    }
                                }
                            }
                            catch (ThreadInterruptedException)
                            {
                                fZoneStatus = null;
                            }
                        }
                    }
                }
                finally
                {
                    fState &= ~GETZONESTATUS;
                }
            }

            return fZoneStatus;
        }

        /// <summary>  Called by MessageDispatcher to inform this Zone of a SIF_Response
        /// received for the SIF_ZoneStatus object type.
        /// </summary>
        protected internal virtual void SetZoneStatus(SIF_ZoneStatus stat)
        {
            lock (fZSLock)
            {
                fZoneStatus = stat;
                Monitor.PulseAll(fZSLock);
            }
        }


        /// <summary>
        /// Gets the latest version of SIF that should be used for messaging in this zone.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The implementation checks the AgentProperties-ZisVersion property and the
        /// ADK-SIFVersion property. The lowest version between the two properties is returned.
        /// </para>
        /// <para>For example, if the ADK-SIFVersion property is set to 1.5r1, SIFVersion.15r1 will
        /// be returned. If, however, the ADK-SIFVersion property is set to 2.1, but the
        /// AgentProperties-ZisVersion property is set to "1.1", SIFVersion.11 will be returned.</para>
        /// This behavior allows the ADK to be configured in one of two ways to support older versions
        /// of SIF
        /// <list type="List">
        /// <item>Support an older version across the board. If this is the desired behavior, setting
        /// the ADK-SIFVersion property is the proper way to achieve this.</item>
        /// <item>Run using the latest version of SIF, but connect to specific zones using an older 
        /// version of SIF. In this case, setting the AgentProperties-ZisVersion is the proper way
        /// to achieve this.</item>
        /// </list>
        /// <para>
        /// <b>NOTE:</b> This property is primarily for internal use only and is meant for use with 
        /// messages that are meant to be sent directly to the ZIS, which include the provisioning 
        /// messages (SIF_Register, SIF_Subscribe, SIF_Provide, etc.), and the system messages 
        /// (SIF_SystemControl).
        /// </para>
        /// <para>To override behavior of messages that are sent to another agent (SIF_Event, SIF_Request),
        /// the ADK supports a more granular approach for dealing with versions down to the object
        /// level. To accomplish this, use ADK object versioning polic</para>
        /// </remarks>
        public SifVersion HighestEffectiveZISVersion
        {
            get
            {
                SifVersion zisVersion = SifVersion.LATEST;
                try
                {
                    zisVersion = SifVersion.Parse(fProps.ZisVersion);
                }
                catch (Exception iae)
                {
                    // Unable to parse the ZIS Version from props
                    Log.Warn(
                        "Unable to parse adk.provisioning.zisVersion value:'" +
                        fProps.ZisVersion +
                        "' into a SIFVersion instance. Using " +
                        SifVersion.LATEST, iae);
                }
                SifVersion adkVersion = Adk.SifVersion;

                if (zisVersion.CompareTo(adkVersion) < 0)
                {
                    return zisVersion;
                }
                else
                {
                    return adkVersion;
                }
            }
        }





        /// <summary> 	Register a <i>MessagingListener</i> to listen to messages received by the
        /// message handlers of this class.
        /// 
        /// NOTE: Agents may register a MessagingListener with the Agent or Zone
        /// classes. When a listener is registered with both classes, it will be 
        /// called twice. Consequently, it is recommended that most implementations 
        /// choose to register MessagingListeners with only one of these classes 
        /// depending on whether the agent is interested in receiving global
        /// notifications or notifications on only a subset of zones.
        /// 
        /// </summary>
        /// <param name="listener">a MessagingListener implementation
        /// </param>
        public virtual void AddMessagingListener(IMessagingListener listener)
        {
            fMessagingListeners.Add(listener);
        }

        /// <summary> 	Remove a <i>MessagingListener</i> previously registered with the
        /// <c>addMessagingListener</c> method.
        /// 
        /// </summary>
        /// <param name="listener">a MessagingListener implementation
        /// </param>
        public virtual void RemoveMessagingListener(IMessagingListener listener)
        {
            fMessagingListeners.Remove(listener);
        }

        public virtual SIF_Ack SifSend(string xml)
        {
            CheckConnect();

            SIF_Ack ack;

            try
            {
                if ((Adk.Debug & AdkDebugFlags.Message_Content) != 0)
                {
                    fLog.Debug("Sending user message (" + xml.Length + " characters): ");
                    fLog.Debug(xml);
                }

                using (IMessageOutputStream stream = new MessageStreamImpl(xml))
                //  Send the message
                {
                    using (IMessageInputStream ackStream = fProtocolHandler.Send(stream))
                    {
                        SifParser parser = SifParser.NewInstance();
                        //  Parse the results into a SIF_Ack
                        ack =
                            (SIF_Ack)
                            parser.Parse(ackStream.GetInputStream(), this, SifParserFlags.None);
                    }
                }

                if (ack != null)
                {
                    ack.message = null;
                    if ((Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0)
                    {
                        ack.LogRecv(fLog);
                    }
                }
            }
            catch (Exception e)
            {
                throw new AdkException(e.ToString(), this);
            }

            return ack;
        }

        /// <summary>  Sends a SIF_Register message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        public virtual SIF_Ack SifRegister()
        {
            CheckConnect();

            if (Properties.ProvisioningMode == AgentProvisioningMode.Agent)
            {
                return fPrimitives.SifRegister(this);
            }

            return null;
        }

        /// <summary>  Sends a SIF_Unregister message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        public virtual SIF_Ack SifUnregister()
        {
            CheckConnect();

            if (Properties.ProvisioningMode == AgentProvisioningMode.Agent)
            {
                return fPrimitives.SifUnregister(this);
            }

            return null;
        }

        /// <summary>  Sends a SIF_Subscribe message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        public virtual SIF_Ack SifSubscribe(string[] objectType)
        {
            CheckConnect();

            if (Properties.ProvisioningMode == AgentProvisioningMode.Agent)
            {
                return fPrimitives.SifSubscribe(this, objectType);
            }

            return null;
        }

        /// <summary>  Sends a SIF_Unsubscribe message to the ZIS.</summary>
        public virtual SIF_Ack SifUnsubscribe(string[] objectType)
        {
            CheckConnect();

            if (Properties.ProvisioningMode != AgentProvisioningMode.Zis)
            {
                return fPrimitives.SifUnsubscribe(this, objectType);
            }

            return null;
        }

        /// <summary>  Sends a SIF_Provide message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        public virtual SIF_Ack SifProvide(string[] objectType)
        {
            CheckConnect();

            if (Properties.ProvisioningMode == AgentProvisioningMode.Agent)
            {
                return fPrimitives.SifProvide(this, objectType);
            }

            return null;
        }

        /// <summary>  Sends a SIF_Unprovide message to the ZIS.</summary>
        public virtual SIF_Ack SifUnprovide(string[] objectType)
        {
            CheckConnect();

            if (Properties.ProvisioningMode != AgentProvisioningMode.Zis)
            {
                fPrimitives.SifUnprovide(this, objectType);
            }

            return null;
        }

        /// <summary>  Sends a SIF_Ping message to the ZIS that manages this zone.</summary>
        public virtual SIF_Ack SifPing()
        {
            CheckConnect();

            return fPrimitives.SifPing(this);
        }

        public virtual SIF_Ack SifSleep()
        {
            CheckConnect();

            return fPrimitives.SifSleep(this);
        }

        public virtual SIF_Ack SifWakeup()
        {
            CheckConnect();

            return fPrimitives.SifWakeup(this);
        }

        public virtual void PurgeQueue(bool incoming,
                                        bool outgoing)
        {
        }

        /// <summary>  Returns the string representation of this zone as "zoneId@zoneUrl"</summary>
        public override string ToString()
        {
            StringBuilder b = new StringBuilder(fZoneId);
            b.Append('@');
            b.Append(fZoneUrl.AbsoluteUri);

            return b.ToString();
        }

    }
}

