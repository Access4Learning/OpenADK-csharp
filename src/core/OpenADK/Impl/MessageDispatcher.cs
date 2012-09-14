//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using OpenADK.Library.Infra;
using OpenADK.Library.Tools.Policy;

namespace OpenADK.Library.Impl
{
    /// <summary>  Handles message dispatching within the class framework</summary>
    /// <remarks>
    /// There is a MessageDispatcher object for each zone. It is protocol-independent:
    /// the MessageDispatcher works in conjunction with either the zone's protocol
    /// handler (IProtocolHandler) or its Agent Local Queue (IAgentQueue) to produce
    /// and consume messages, but never participates in the actual sending or
    /// receiving of messages on the wire. Conversely, the protocol handler and
    /// queue never dispatch messages; they are only concerned with sending and
    /// receiving them.
    /// 
    /// 
    /// Message dispatching is at the heart of the class framework and the most
    /// complex implementation class because of the multiple ways in which an agent
    /// can exchange messages with a ZIS -- namely, Push and Pull modes, Selective
    /// Message Blocking, and optional Agent Local Queue (with negates the need for
    /// SMB when enabled). Further complicating matters is the fact that an agent
    /// may be connected to multiple zones, some of which may be using Push mode
    /// while others are using Pull mode.
    /// 
    /// 
    /// <b>Message Acknowledgement</b>
    /// 
    /// SIF guarantees the delivery (and hopefully the processing) of a message by
    /// requiring that it remain in its queue until acknowledged by the recipient.
    /// An agent should only acknowledge a message after it has processed it, so the
    /// Adk does not send acknowledgements when a message is received but rather
    /// after it has been successfully dispatched without exception. In other words,
    /// a message is not acknowledged simply because it was received successfully;
    /// it must be processed by the agent first. This is an important distinction
    /// to keep in mind and critical to the operation of MessageDispatcher.
    /// 
    /// 
    /// <b>Message Consumption</b>
    /// 
    /// Throughout the rest of this commentary, ALQ refers to "Agent Local Queue"
    /// and PH refers to "Protocol Handler".
    /// 
    /// MessageDispatcher consumes messages from one of two sources: either the ALQ
    /// or a zone's PH. When the ALQ is enabled, the PH stores messages in the queue
    /// as they are received from the network. It immediately acknowledges them
    /// because they are safely persisted in the ALQ and will remain there even if
    /// the agent goes down. Further, Selective Message Blocking is not needed when
    /// the ALQ is enabled, so there is no reason to ever send anything but an
    /// Immediate acknowledgement to the originating zone.
    /// 
    /// When the ALQ is disabled, however, messages must be dispatched and processed
    /// before the PH can return an acknowledgement to the zone. This means a full
    /// cycle through the class framework from the time a message is received by
    /// the PH until the time it has been processed and a SIF_Ack is returned. To
    /// complicate matters, an agent may need to invoke Selective Message Blocking
    /// because the ALQ is not available, and in this case the framework may need
    /// to return an Intermediate ack before the agent has had a chance to fully
    /// process the message.
    /// 
    /// Thus, when ALQ is enabled MessageDispatcher runs in a thread to consume
    /// messages from the queue as follows:
    /// 
    /// <ul>
    /// <li>Waits for the next message to become available (blocks)</li>
    /// <li>Dispatches the message to the appropriate Subscriber, Publisher,
    /// or QueryResults object</li>
    /// <li>If an exception is thrown during the dispatch call, the message is
    /// left in the ALQ and the process repeats</li>
    /// <li>If the dispatch call succeeds, the message is permanently removed
    /// from the ALQ</li>
    /// </ul>
    /// 
    /// Remember, each message in the ALQ was acknowledged at the time it was placed
    /// into the queue by the PH so the above algorithm never sends SIF_Ack messages.
    /// It is purely concerned with dispatching messages waiting in the queue.
    /// 
    /// When ALQ is disabled MessageDispatcher does not run in a thread. Rather
    /// than <i>consume</i> messages, it is handed them by the PH for synchronous
    /// processing as they're received from the network. This process works as
    /// follows:
    /// 
    /// <ul>
    /// <li>The PH receives a message and calls MessageDispatcher.dispatch
    /// to dispatch it</li>
    /// <li>The message is dispatched to the appropriate Subscriber, Publisher,
    /// or QueryResults object</li>
    /// <li>If an exception is thrown during the dispatch call, the exception
    /// is propagated up the call stack to the PH, which will respond by
    /// simply not sending an acknowledgement. Thus, the message is left in
    /// the agent queue on the zone server.</li>
    /// <li>If the dispatch call succeeds, it returns a status code 1, 2, or 3
    /// to the PH and the PH sends an acknowledgement with that status code.
    /// These codes corresponding to the Immediate, Intermediate, and Final
    /// acknowledgement types. Status code 2 (Intermediate) is only returned
    /// if the message was dispatched to a Subscriber. Status code 3 (Final)
    /// is only returned if the message was dispatched to a QueryResults
    /// object in response to an earlier SIF_Event that invoked Selective
    /// Message Blocking. Status code 1 (Immediate) is returned in all other
    /// cases. Note MessageDispatcher <b>does not</b> keep track of SMB
    /// state; this is the job of the dispatch recipient.</li>
    /// </ul>
    /// 
    /// <b>Message Production</b>
    /// 
    /// MessageDispatcher also "produces" messages on behalf of Topic and Zone
    /// objects when posting outgoing SIF_Event, SIF_Request, and SIF_Response
    /// messages (these are the only outgoing infrastructure messages that pass
    /// through MessageDispatcher; all others are immediately handed to the PH for
    /// synchronous delivery). Like message consumption, the process of sending
    /// messages depends on whether or not the ALQ is enabled as well as the type of
    /// message being sent.
    /// 
    /// When the ALQ is enabled, SIF_Event messages are immediately stored in the
    /// queue and will eventually be sent to the appropriate PH by a worker thread
    /// that is built into the ALQ. This ensures that regardless of whether the
    /// agent goes down or not, the events it has generated are guaranteed to make
    /// their way to the ZIS eventually. SIF_Request and SIF_Response messages are
    /// not handled in this way; rather, they are sent synchronously in the same
    /// way as SIF_Events are sent when the ALQ is disabled.</p>
    /// 
    /// 
    /// When the ALQ is disabled, SIF_Event, SIF_Request, and SIF_Response messages
    /// are immediately handed to the PH of each destination zone. If an exception
    /// occurs before the message is acknowledged by the ZIS, it will propagate up
    /// the call stack to the agent code that originally initiated the message (e.g.
    /// to a Topic.publishEvent call). An agent can either retry the operation by
    /// calling the same method a second time, or can abandon the transaction
    /// altogether.</p>
    /// 
    /// <b>Push vs. Pull Mode</b></p>
    /// 
    /// Push and Pull mode have no effect on MessageDispatcher, its interfaces, or
    /// its logic. When Push mode is active for a zone, the PH will receive incoming
    /// messages as they're pushed by the ZIS. Those messages will then be handled
    /// as described in "Message Consumption" above. When Pull mode is active for a
    /// zone, the PH runs as a thread to periodically get messages from the ZIS. In
    /// short, MessageDispatcher does not care how messages were obtained. It works
    /// consistently in both modes.
    /// 
    /// <b>Selective Message Blocking</b>
    /// 
    /// The Adk addresses Selective Message Blocking by forcing agents to use the
    /// TrackQueryResults class to perform queries while processing SIF_Events. (If
    /// an agent attempts to call a query method while a SIF_Event is being
    /// dispatched for a given zone, an exception is thrown.)  TrackQueryResults
    /// houses all of the logic for Selective Message Blocking. When the ALQ is
    /// enabled, TrackQueryResults does not invoke SMB; instead it draws upon the
    /// ALQ for SIF_Responses. When the ALQ is disabled, TrackQueryResults keeps
    /// a tab of which messages it has sent Intermediate acknowledgements for and
    /// will eventually need to send Final acknowledgements for. To accomplish this
    /// it works closely with MessageDispatcher so that when the dispatching of a
    /// given SIF_Event has ended the TrackQueryResults object is asked to send its
    /// pending Final acknowledgements. For more details on this consult that
    /// class.
    /// </remarks>
    public class MessageDispatcher
    {
        private Object fRunning;
        private IAgentQueue fQueue;
        private ZoneImpl fZone;
        private string fSourceId;
        private SifParser fParser;
        private bool fKeepMsg;
        private bool fAckAckOnPull;
        private Object fPullCanStart = new object();
        private Hashtable fEvDispCache;

        private RequestCache fRequestCache;

        /// <summary>
        /// Returns the RequestCache used to map Responses to the original Request
        /// </summary>
        internal RequestCache RequestCache
        {
            get { return fRequestCache; }
        }

        /// <summary>  Constructs a MessageDispatcher for a zone</summary>
        public MessageDispatcher(ZoneImpl zone)
        {
            fRequestCache = RequestCache.GetInstance(zone.Agent);

            fZone = zone;
            fQueue = zone.fQueue;
            if (fQueue != null)
            {
                if (!fQueue.Ready)
                {
                    throw new AdkQueueException
                        (
                        "Agent Queue is not ready for agent \"" + zone.Agent.Id + "\" zone \"" +
                        zone.ZoneId + "\"",
                        fZone);
                }
                Thread thread = new Thread(new ThreadStart(this.Run));
                thread.Name = zone.Agent.Id + "@" + zone.ZoneId + ".MessageDispatcher";
                thread.Start();
            }

            fSourceId = zone.Agent.Id;
            fKeepMsg = zone.Agent.Properties.KeepMessageContent;
            fAckAckOnPull = zone.Agent.Properties.PullAckAck;

            try
            {
                fParser = SifParser.NewInstance();
            }
            catch (AdkException adke)
            {
                throw new ApplicationException(adke.ToString());
            }
        }

        /// <summary>  Find the QueryResults object for a zone by searching up the message
        /// dispatching chain until a Zone, Topic, or Agent is found with a registered
        /// QueryResults implementation.
        /// 
        /// </summary>
        /// <param name="rsp">The SIF_Response message (if a SIF_Response was received).
        /// Either rsp or req must be specified, but not both.
        /// </param>
        /// <param name="req">The SIF_Request message (if a SIF_Request is being sent).
        /// Either rsp or req must be specified, but not both.
        /// </param>
        /// <param name="query">Only applicable when <i>req</i> is non-null: The Query
        /// associated with the SIF_Request
        /// </param>
        /// <param name="zone">The Zone to begin the search at
        /// </param>
        internal IQueryResults getQueryResultsTarget(SIF_Response rsp,
                                                      SIF_Request req,
                                                      IElementDef objType,
                                                      Query query,
                                                      IZone zone)
        {
            //
            //  - First check TrackQueryResults for a matching pending request
            //  - Next check the Topic, the Zone, and finally the Agent. The
            //    message is dispatched to the first one that results a
            //    QueryResults object
            //
            IQueryResults target = null;
            SifContext context = null;

            if (req != null)
            {
                //  First check TrackQueryResults
                string reqId = req.MsgId;

                // TODO: Implement SMB later
                //				TrackQueryResults tracker = (TrackQueryResults) TrackQueryResultsImpl.sRequestQueries[query];
                //				if (tracker != null)
                //				{
                //					TrackQueryResultsImpl.sRequestQueries.Remove( query );
                //					target = tracker;
                //				}
                //				else
                //				{
                SIF_Query q = req.SIF_Query;
                if (q == null)
                {
                    throw new SifException
                        (SifErrorCategoryCode.Xml, SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                          "SIF_Request message missing mandatory element", "SIF_Query is required",
                          fZone);
                }

                SIF_QueryObject qo = q.SIF_QueryObject;
                if (qo == null)
                {
                    throw new SifException
                        (SifErrorCategoryCode.Xml, SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                          "SIF_Request message missing mandatory element",
                          "SIF_QueryObject is required", fZone);
                }

                objType = Adk.Dtd.LookupElementDef(qo.ObjectName);
                if (objType == null)
                {
                    throw new SifException
                        (SifErrorCategoryCode.RequestResponse, SifErrorCodes.REQRSP_INVALID_OBJ_3,
                          "Agent does not support this object type", qo.ObjectName, fZone);
                }
                // Check to see if the Context is supported
                // TODO: Determine if a SIFException should be thrown at this point?
                try
                {

                    context = req.SifContexts[0];
                }
                catch (AdkNotSupportedException contextNotSupported)
                {
                    throw new SifException(
                            SifErrorCategoryCode.Generic,
                            SifErrorCodes.GENERIC_CONTEXT_NOT_SUPPORTED_4,
                            contextNotSupported.Message, fZone);
                }

            }
            else if (rsp != null)
            {
                // TODO Implement SMB
                //				//  First check TrackQueryResults object to see if it is expecting
                //				//  to be called for this SIF_Response
                //				string reqId = rsp.SIF_RequestMsgId;
                //				
                //				TrackQueryResults tracker = (TrackQueryResults) TrackQueryResultsImpl.sRequestMsgIds[reqId];
                //				if (tracker != null)
                //				{
                //					//  Dispatch to the TrackQueryResults object
                //					target = tracker;
                //				}

                // Check to see if the Context is supported
                // TODO: Determine if a SIFException should be thrown at this point?
                try
                {
                    context = rsp.SifContexts[0];
                }
                catch (AdkNotSupportedException contextNotSupported)
                {
                    throw new SifException(
                            SifErrorCategoryCode.Generic,
                            SifErrorCodes.GENERIC_CONTEXT_NOT_SUPPORTED_4,
                            contextNotSupported.Message, fZone);
                }


            }
            else
            {
                throw new ArgumentException
                    (
                    "A SIF_Request or SIF_Response object must be passed to getQueryResultsTarget");
            }

            if (target == null)
            {
                TopicImpl topic = (TopicImpl)fZone.Agent.TopicFactory.LookupInstance(objType, context);
                if (topic != null)
                {
                    target = topic.fQueryResults;
                }


                if (target == null)
                {
                    //  Next try the Zone...
                    target = fZone.GetQueryResults(context, objType);
                }
                if (target == null)
                {
                    //  Finally, try the Agent...
                    target = fZone.Agent.GetQueryResults(context, objType);
                }
            }

            return target;
        }


        /// <summary>  Find the MessagingListenerImpl objects to notify when a message is
        /// received or sent on this zone.
        /// </summary>
        internal static ICollection<IMessagingListener> GetMessagingListeners(ZoneImpl zone)
        {
            List<IMessagingListener> listeners = new List<IMessagingListener>();

            //	Contribute the Zone's listeners to the group
            listeners.AddRange(zone.fMessagingListeners);
            listeners.AddRange(zone.Agent.MessagingListeners);
            return listeners;
        }

        /// <summary>  Dispatch a message.
        /// 
        /// </summary>
        /// <param name="msg">The infrastructure message to dispatch
        /// </param>
        public virtual int dispatch(SifMessagePayload msg)
        {
            string errTyp = null;
            int status = 1;

            try
            {
                SifMessageType pload =
                    (SifMessageType)Adk.Dtd.GetElementType(msg.ElementDef.Name);

                if (pload == SifMessageType.SIF_SystemControl)
                {
                    IList<SifElement> ch =
                        ((SIF_SystemControl)msg).SIF_SystemControlData.GetChildList();

                    if (ch != null && ch.Count > 0)
                    {
                        if (ch[0].ElementDef == InfraDTD.SIF_SLEEP)
                        {
                            fZone.ExecSleep();
                        }
                        else if (ch[0].ElementDef == InfraDTD.SIF_WAKEUP)
                        {
                            fZone.ExecWakeup();
                        }
                        else if (ch[0].ElementDef == InfraDTD.SIF_PING)
                        {
                            //	Notify MessagingListeners...
                            SifMessageInfo msginfo = new SifMessageInfo(msg, fZone);
                            NotifyMessagingListeners_OnMessageProcessed
                                (SifMessageType.SIF_SystemControl, msginfo);

                            if (fZone.IsSleeping(AdkQueueLocation.QUEUE_LOCAL))
                            {
                                return 8;
                            }

                            return 1;
                        }
                        else
                        {
                            fZone.Log.Warn("Received unknown SIF_SystemControlData: " + ch[0].Tag);

                            throw new SifException
                                (SifErrorCategoryCode.Xml,
                                  SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                                  "SIF_SystemControlData must contain SIF_Ping, SIF_Wakeup, or SIF_Sleep",
                                  fZone);
                        }

                        //	Notify MessagingListeners...
                        SifMessageInfo msginfo2 = new SifMessageInfo(msg, fZone);
                        NotifyMessagingListeners_OnMessageProcessed
                            (SifMessageType.SIF_SystemControl, msginfo2);
                    }

                    return status;
                }

                //  If zone is asleep, return status code
                if (fZone.IsSleeping(AdkQueueLocation.QUEUE_LOCAL))
                {
                    return 8;
                }

                //  Some agents don't want to receive messages - for example, the
                //  SIFSend Adk Example agent. This is very rare but we offer a property
                //  to allow for it
                if (fZone.Properties.DisableMessageDispatcher)
                {
                    return status;
                }

                switch (pload)
                {
                    case SifMessageType.SIF_Event:
                        errTyp = "Subscriber.onEvent";
                        status = dispatchEvent((SIF_Event)msg);
                        break;

                    case SifMessageType.SIF_Request:
                        errTyp = "Publisher.onRequest";
                        dispatchRequest((SIF_Request)msg);
                        break;

                    case SifMessageType.SIF_Response:
                        errTyp = "QueryResults";
                        dispatchResponse((SIF_Response)msg);
                        break;

                    default:
                        fZone.Log.Warn
                            ("Agent does not know how to dispatch " + msg.ElementDef.Name +
                              " messages");
                        throw new SifException
                            (SifErrorCategoryCode.Generic,
                              SifErrorCodes.GENERIC_MESSAGE_NOT_SUPPORTED_2, "Message not supported",
                              msg.ElementDef.Name, fZone);
                }
            }
            catch (LifecycleException)
            {
                throw;
            }
            catch (SifException se)
            {
                //	Check if AdkException.setRetry() was called; use transport
                //	error category to force ZIS to resend message
                if (se.Retry)
                {
                    se.ErrorCategory = SifErrorCategoryCode.Transport;
                    se.ErrorCode = SifErrorCodes.WIRE_GENERIC_ERROR_1;
                }

                logAndRethrow("SIFException in " + errTyp + " message handler for " + msg.ElementDef.Name, se);
            }
            catch (AdkZoneNotConnectedException adkznce)
            {
                // Received a message while the zone was disconnected. Return a system transport
                // error so that the message is not removed from the queue
                SifException sifEx = new SifException(
                        SifErrorCategoryCode.Transport,
                        SifErrorCodes.WIRE_GENERIC_ERROR_1,
                        adkznce.Message, fZone);
                logAndRethrow("Message received while zone is not connected", sifEx);
            }

            catch (AdkException adke)
            {
                //	Check if ADKException.setRetry() was called; use transport
                //	error category to force ZIS to resent message
                if (adke.Retry)
                {
                    logAndThrowRetry(adke.Message, adke);
                }
                logAndThrowSIFException("ADKException in " + errTyp + " message handler for " + msg.ElementDef.Name, adke);
            }
            catch (Exception uncaught)
            {
                logAndThrowSIFException("Uncaught exception in " + errTyp + " message handler for " + msg.ElementDef.Name, uncaught);
            }

            return status;
        }

        /// <summary>  Dispatch a SIF_Event.
        /// 
        /// <b>When ALQ Disabled:</b> Dispatching of this event is handled in a
        /// separate EvDisp thread in case SMB is invoked. This makes it possible
        /// to asynchronously return a SIF_Ack code to the dispatchEvent() caller
        /// before the handling of the event by the Subscriber is completed. The
        /// EvDisp also tracks the internal dispatch state of this particular message.
        /// The Topic matching the object type is then notified via its Subscriber's
        /// onEvent method. If a TrackQueryResults object is created within that
        /// method, its constructor will wakeup the EvDisp thread, instructing it to
        /// return a value of 2 (Intermediate). If no TrackQueryResults object is
        /// instantiated during the Subscriber.onEvent method, the EvDisp thread
        /// returns a 1 (Immediate) status code upon completion.
        /// 
        /// <b>When ALQ Enabled:</b> Dispatching is immediate. The Topic matching
        /// the object type is then notified via its Subscriber's onEvent method,
        /// then processing ends. No EvDisp thread is needed because if a
        /// TrackQueryResults is used it will draw upon the ALQ instead of invoking
        /// SMB on the zone server.
        /// 
        /// Note if an exception is thrown at any time during the processing of a
        /// SIF_Event, it is propagated up the call stack. The PH must not return a
        /// SIF_Ack for the message; the ALQ must not delete the message from its
        /// queue.
        /// 
        /// </summary>
        private int dispatchEvent(SIF_Event sifEvent)
        {
            if ((Adk.Debug & AdkDebugFlags.Messaging_Event_Dispatching) != 0)
            {
                fZone.Log.Debug("Dispatching SIF_Event (" + sifEvent.MsgId + ")...");
            }

            //  Was this event reported by this agent?
            if (!fZone.Properties.ProcessEventsFromSelf &&
                 sifEvent.Header.SIF_SourceId.Equals(fZone.Agent.Id))
            {
                if ((Adk.Debug & AdkDebugFlags.Messaging_Event_Dispatching) != 0)
                {
                    fZone.Log.Debug
                        (
                        "SIF_Event ignored because it was originally reported by this agent (see the adk.messaging.processEventsFromSelf property)");
                }

                return 1;
            }

            SIF_ObjectData odata = sifEvent.SIF_ObjectData;
            if (odata == null)
            {
                throw new SifException
                    (SifErrorCategoryCode.Xml, SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                      "SIF_Event message missing mandatory element",
                      "SIF_ObjectData is a required element", fZone);
            }

            //
            //  Loop through all SIF_EventObjects inside this SIF_Event and dispatch
            //  to corresponding topics
            //
            SIF_EventObject eventObj = odata.SIF_EventObject;
            if (eventObj == null)
            {
                throw new SifException
                    (
                    SifErrorCategoryCode.Xml,
                    SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                    "SIF_Event message missing mandatory element",
                    "SIF_ObjectData/SIF_EventObject is a required element", fZone);
            }

            int ackCode = 1;
            int thisCode;

            SifMessageInfo msgInfo = new SifMessageInfo(sifEvent, fZone);

            IElementDef typ = Adk.Dtd.LookupElementDef(eventObj.ObjectName);
            if (typ == null)
            {
                //  SIF Data Object type not supported
                throw new SifException
                    (
                    SifErrorCategoryCode.EventReportingAndProcessing,
                    SifErrorCodes.EVENT_INVALID_EVENT_3,
                    "Agent does not support this object type", eventObj.ObjectName, fZone);
            }

            // TODO: For now, the ADK only routes SIF Events to the first context
            // in the event. This needs to be implemented to support
            // events in multiple contexts
            SifContext eventContext = msgInfo.SIFContexts[0];
            ISubscriber target = null;
            ITopic topic = null;
            //
            //  Lookup the Topic for this SIF object type
            // Topics are only used for the SIF Default context
            //
            topic = fZone.Agent.TopicFactory.LookupInstance(typ, eventContext);
            if (topic != null)
            {
                target = topic.GetSubscriber();
            }

            if (target == null)
            {
                //  Is a Subscriber registered with the Zone?
                target = fZone.GetSubscriber(eventContext, typ);
                if (target == null)
                {
                    //  Is a Subscriber registered with the Agent?
                    target = fZone.GetSubscriber(eventContext, typ);
                    if (target == null)
                    {
                        //
                        //  No Subscriber message handler found. Try calling the Undeliverable-
                        //  MessageHandler for the zone or agent. If none is registered,
                        //  return an error SIF_Ack indicating the object type is not
                        //  supported.
                        //
                        Boolean handled = false;
                        IUndeliverableMessageHandler errHandler = fZone.ErrorHandler;
                        if (errHandler != null)
                        {
                            handled = errHandler.OnDispatchError(sifEvent, fZone, msgInfo);

                            //	Notify MessagingListeners...
                            IEnumerable<IMessagingListener> mList = GetMessagingListeners(fZone);
                            foreach (IMessagingListener ml in mList)
                            {
                                ml.OnMessageProcessed(SifMessageType.SIF_Event, msgInfo);
                            }
                        }

                        if (!handled)
                        {
                            fZone.Log.Warn("Received a SIF_Event (" + sifEvent.MsgId + "), but no Subscriber object is registered to handle it");

                            throw new SifException(
                               SifErrorCategoryCode.EventReportingAndProcessing,
                               SifErrorCodes.EVENT_INVALID_EVENT_3,
                               "Agent does not support this object type",
                               eventObj.ObjectName, fZone);
                        }

                        return 1;
                    }
                }
            }


            //
            //  Call Subscriber.onEvent with the event data
            //
            IList<SifElement> sel = eventObj.GetChildList();
            SifDataObject[] data = new SifDataObject[sel.Count];
            for (int x = 0; x < sel.Count; x++)
            {
                data[x] = (SifDataObject)sel[x];
            }

            //  Wrap in an Event object
            DataObjectInputStreamImpl dataStr = DataObjectInputStreamImpl.newInstance();
            dataStr.Data = data;
            Event adkEvent =
                new Event(dataStr, eventObj.Action, eventObj.GetChildList()[0].ElementDef);
            adkEvent.Zone = fZone;
            if ((Adk.Debug & AdkDebugFlags.Messaging_Event_Dispatching) != 0)
            {
                fZone.Log.Debug
                    (
                    "SIF_Event contains " + data.Length + " " + eventObj.ObjectName +
                    " objects (" + eventObj.Action + ")");
            }

            if (fQueue == null)
            {
                if ((Adk.Debug & AdkDebugFlags.Messaging_Event_Dispatching) != 0)
                {
                    fZone.Log.Debug
                        ("Dispatching SIF_Event to Subscriber message handler via EvDisp");
                }

                //
                //  -- No ALQ available --
                //  Dispatch in a separate EvDisp thread. Block until an ack
                //  status code is available, then return it
                //
                EvDisp disp;
                try
                {
                    disp = checkoutEvDisp(adkEvent);
                    thisCode = disp.dispatch(target, adkEvent, fZone, topic, msgInfo);
                }
                finally
                {
                    checkinEvDisp(adkEvent);
                }
            }
            else
            {
                if ((Adk.Debug & AdkDebugFlags.Messaging_Event_Dispatching) != 0)
                {
                    fZone.Log.Debug("Dispatching SIF_Event to Subscriber message handler");
                }

                //
                //  -- ALQ is available --
                //  Dispatch immediately.
                //
                try
                {
                    target.OnEvent(adkEvent, fZone, msgInfo);
                }
                catch (SifException)
                {
                    throw;
                }
                catch (Exception thr)
                {
                    throw new SifException
                        (SifErrorCategoryCode.EventReportingAndProcessing,
                          SifErrorCodes.EVENT_GENERIC_ERROR_1, "Error processing SIF_Event",
                          "Exception in Subscriber.onEvent message handler: " + thr, fZone);
                }

                thisCode = 1;
            }

            if (thisCode > ackCode)
            {
                ackCode = thisCode;
            }


            if ((Adk.Debug & AdkDebugFlags.Messaging) != 0)
            {
                fZone.Log.Debug
                    ("SIF_Event (" + sifEvent.MsgId + ") dispatching returning SIF_Ack status " +
                      ackCode);
            }

            return ackCode;
        }

        /// <summary>  Dispatch a SIF_Response.
        /// 
        /// SIF_Response messages are dispatched as follows:
        /// 
        /// <ul>
        /// <li>
        /// If a TrackQueryResults object issued the original SIF_Request
        /// during this agent session (i.e. the agent process has not
        /// terminated since the SIF_Request was issued), the response is
        /// dispatched to that TrackQueryResults object via its QueryResults
        /// interface.
        /// </li>
        /// <li>
        /// If a Topic exists for the data type associated with the
        /// SIF_Response, it is dispatched to the QueryResults object
        /// registered with that Topic.
        /// </li>
        /// <li>
        /// If no Topic exists for the data type associated with the
        /// SIF_Response, it is dispatched to the QueryResults object
        /// registered with the Zone from which the SIF_Response was
        /// received.
        /// </li>
        /// </ul>
        /// 
        /// <b>SIF_ZoneStatus</b> is handled specially. When Zone.awaitingZoneStatus
        /// returns true, the agent is blocking on a call to Zone.getZoneStatus().
        /// In this case, the SIF_ZoneStatus object is routed directly to the Zone
        /// object instead of being dispatched via the usual QueryResults mechanism.
        /// 
        /// </summary>
        private void dispatchResponse(SIF_Response rsp)
        {
            //	block thread until Zone.Query() has completed in case it is in the
            //	midst of a SIF_Request. This is done to ensure that we don't receive
            //	the SIF_Response from the zone before the ADK and agent have finished
            //	with the SIF_Request in Zone.Query()
            fZone.WaitForRequestsToComplete();
            bool retry = false;
            IRequestInfo reqInfo = null;
            AdkException cacheErr = null;

            try
            {
                try
                {
                    reqInfo = fRequestCache.LookupRequestInfo(rsp.SIF_RequestMsgId, fZone);
                }
                catch (AdkException adke)
                {
                    cacheErr = adke;
                }

#if PROFILED
				if( objType != null ) 
				{
					ProfilerUtils.profileStart( com.OpenADK.sifprofiler.api.OIDs.ADK_SIFRESPONSE_REQUESTOR_MESSAGING.ToString(), Adk.Dtd.LookupElementDef(objType), rsp.MsgId );
				}
#endif

                SIF_Error error = null;
                SIF_ObjectData od = rsp.SIF_ObjectData;
                if (od == null)
                {
                    throw new SifException
                        (
                        SifErrorCategoryCode.Xml,
                        SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                        "SIF_Response missing mandatory element",
                        "SIF_ObjectData is a required element of SIF_Response",
                        fZone);
                }

                IList<SifElement> sel = od.GetChildList();
                if (sel.Count < 1)
                {
                    error = rsp.SIF_Error;

                    //
                    //  If the SIF_Response has no SIF_ObjectData elements but does
                    //  have a SIF_Error child, the associated object type can
                    //  only be gotten from the RequestCache, but that had
                    //  failed so try and call the UndeliverableMessageHandler.
                    //
                    if (cacheErr != null || reqInfo == null)
                    {
                        bool handled = false;

                        IUndeliverableMessageHandler errHandler = fZone.ErrorHandler;
                        if (errHandler != null)
                        {
                            SifMessageInfo msginfo = new SifMessageInfo(rsp, fZone);
                            msginfo.SIFRequestInfo = reqInfo;

                            handled = errHandler.OnDispatchError(rsp, fZone, msginfo);

                            //	Notify MessagingListeners...
                            NotifyMessagingListeners_OnMessageProcessed
                                (SifMessageType.SIF_Response, msginfo);
                        }

                        if (!handled)
                        {
                            fZone.Log.Warn
                                (
                                "Received a SIF_Response message with MsgId " + rsp.MsgId +
                                " (for SIF_Request with MsgId " + rsp.SIF_RequestMsgId + ") " +
                                " containing an empty result set or a SIF_Error, but failed to obtain the SIF Data Object" +
                                " type from the RequestCache" +
                                (cacheErr != null ? (" due to an error: " + cacheErr.Message) : ""));
                        }

                        return;
                    }
                }

                string objectType = reqInfo != null ? reqInfo.ObjectType : null;

                if (objectType == null && sel.Count > 0)
                {
                    objectType = sel[0].ElementDef.Tag(rsp.SifVersion);
                }

                if (objectType != null &&
                     (string.Compare(objectType, "SIF_ZoneStatus", true) == 0))
                {
                    //  SIF_ZoneStatus is a special case
                    if (fZone.AwaitingZoneStatus())
                    {
                        fZone.SetZoneStatus((SIF_ZoneStatus)sel[0]);
                        return;
                    }
                }

                if (reqInfo == null)
                {
                    reqInfo = new UnknownRequestInfo(rsp.SIF_RequestMsgId, objectType);
                }

                //  Decide where to send this response
                IQueryResults target =
                    getQueryResultsTarget
                        (rsp, null, Adk.Dtd.LookupElementDef(objectType), null, fZone);
                if (target == null)
                {
                    bool handled = false;
                    IUndeliverableMessageHandler errHandler = fZone.ErrorHandler;
                    if (errHandler != null)
                    {
                        SifMessageInfo msginfo = new SifMessageInfo(rsp, fZone);
                        if (reqInfo != null)
                        {
                            msginfo.SIFRequestInfo = reqInfo;
                        }

                        handled = errHandler.OnDispatchError(rsp, fZone, msginfo);

                        //	Notify MessagingListeners...
                        NotifyMessagingListeners_OnMessageProcessed
                            (SifMessageType.SIF_Response, msginfo);
                    }

                    if (!handled)
                    {
                        fZone.Log.Warn
                            ("Received a SIF_Response message with MsgId " + rsp.MsgId +
                              " (for SIF_Request with MsgId " + rsp.SIF_RequestMsgId +
                              "), but no QueryResults object is registered to handle it or the request was issued by a TrackQueryResults that has timed out");
                    }

                    return;
                }

                //
                //  Dispatch the message...
                //

                IElementDef sifRequestObjectDef = Adk.Dtd.LookupElementDef(objectType);
                DataObjectInputStreamImpl dataStr = DataObjectInputStreamImpl.newInstance();
                dataStr.fObjType = sifRequestObjectDef;

                if (error == null)
                {
                    //  Convert to a SifDataObject array
                    SifDataObject[] data = new SifDataObject[sel.Count];
                    for (int i = 0; i < sel.Count; i++)
                    {
                        data[i] = (SifDataObject)sel[i];
                    }

                    //  Let the QueryResults object process the message
                    dataStr.Data = data;
                }

                SifMessageInfo msgInf = new SifMessageInfo(rsp, fZone);
                msgInf.SIFRequestInfo = reqInfo;
                msgInf.SIFRequestObjectType = sifRequestObjectDef;
                target.OnQueryResults(dataStr, error, fZone, msgInf);

                //	Notify MessagingListeners...
                NotifyMessagingListeners_OnMessageProcessed(SifMessageType.SIF_Response, msgInf);
            }
            catch (AdkException adkEx)
            {
                retry = adkEx.Retry;
                throw;
            }
            catch (Exception thr)
            {
                throw new SifException
                    (
                    SifErrorCategoryCode.Generic,
                    SifErrorCodes.GENERIC_GENERIC_ERROR_1,
                    "Error processing SIF_Response",
                    "Exception in QueryResults message handler: " + thr.ToString(),
                    fZone);
            }
            finally
            {
                // If the reqInfo variable came from the cache, and retry is set to false,
                // remove it from the cache if this is the last packet
                if (!(reqInfo is UnknownRequestInfo) && !retry)
                {
                    String morePackets = rsp.SIF_MorePackets;
                    if (!(morePackets != null && morePackets.ToLower().Equals("yes")))
                    {
                        // remove from the cache
                        fRequestCache.GetRequestInfo(rsp.SIF_RequestMsgId, fZone);
                    }
                }
#if PROFILED
			if( BuildOptions.PROFILED ) {
				if( reqInfo != null ) {
					ProfilerUtils.profileStop();
				}
#endif
            }
        }

        /// <summary>  Dispatch a SIF_Request.
        /// 
        /// <b>When ALQ Disabled:</b> The SIF_Request is immediately dispatched to
        /// the Publisher of the associated topic. Only after the Publisher has
        /// returned a result does this method return, causing the SIF_Request to
        /// be acknowledged. The result data returned by the Publisher is handed to
        /// the zone's ResponseDelivery thread, which sends SIF_Response messages to
        /// the ZIS until all of the result data has been sent, potentially with
        /// multiple SIF_Response packets. Note without the ALQ, there is the
        /// potential for the agent to terminate before all data has been sent,
        /// causing some results to be lost. In this case the SIF_Request will have
        /// never been ack'd and will be processed again the next time the agent
        /// is started.
        /// 
        /// 
        /// <b>When ALQ Enabled:</b> The SIF_Request is placed in the ALQ where it
        /// will be consumed by the zone's ResponseDelivery thread at a later time.
        /// This method returns immediately, causing the SIF_Request to be
        /// acknowledged. The ResponseDelivery handles dispatching the request to
        /// the Publisher of the associated topic, and also handles returning
        /// SIF_Response packets to the ZIS. With the ALQ, the processing of the
        /// SIF_Request and the returning of all SIF_Response data is guaranteed
        /// because the original SIF_Request will not be removed from the ALQ until
        /// both of these activities have completed successfully (even over multiple
        /// agent sessions).
        /// 
        /// 
        /// Note that any error that occurs during a SIF_Request should result in a
        /// successful SIF_Ack (because the SIF_Request was received successfully),
        /// and a single SIF_Response with a SIF_Error payload. The SIF Compliance
        /// harness checks for this.
        /// 
        /// 
        /// </summary>
        /// <param name="req">The SIF_Request to process
        /// </param>
        private void dispatchRequest(SIF_Request req)
        {
            SifVersion renderAsVer = null;
            SIF_Query q = null;
            SIF_QueryObject qo = null;
            IElementDef typ = null;
            int maxBufSize = 0;
            bool rethrow = false;

            try
            {
                //	block thread until Zone.query() has completed in case it is in the
                //	midst of a SIF_Request and the destination of that request is this 
                //	agent (i.e. a request of self). This is done to ensure that we don't 
                //	receive the SIF_Request from the zone before the ADK and agent have 
                //	finished issuing it in Zone.query()
                fZone.WaitForRequestsToComplete();

                //
                //  Check SIF_Version. If the version is not supported by the Adk,
                //  fail the SIF_Request with an error SIF_Ack. If the version is
                //  supported, continue on; the agent may not support this version,
                //  but that will be determined later and will result in a SIF_Response
                //  with a SIF_Error payload.
                //
                SIF_Version[] versions = req.GetSIF_Versions();
                if (versions == null || versions.Length == 0)
                {
                    rethrow = true;
                    throw new SifException
                        (
                        SifErrorCategoryCode.Xml,
                        SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                        "SIF_Request/SIF_Version is a mandatory element",
                        fZone);
                }

                //  SIF_Version specifies the version of SIF that will be used to render
                //  the SIF_Responses
                // TODO: Add support for multiple SIF_Request versions
                renderAsVer = SifVersion.Parse(versions[0].Value);
                if (!Adk.IsSIFVersionSupported(renderAsVer))
                {
                    rethrow = true;
                    throw new SifException
                        (
                        SifErrorCategoryCode.RequestResponse,
                        SifErrorCodes.REQRSP_UNSUPPORTED_SIFVERSION_7,
                        "SIF_Version " + renderAsVer + " is not supported by this agent",
                        fZone);
                }

                //  Check max buffer size
                int? maximumBufferSize = req.SIF_MaxBufferSize;
                if (!maximumBufferSize.HasValue )
                {
                    rethrow = true;
                    throw new SifException
                        (
                        SifErrorCategoryCode.Xml,
                        SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                        "SIF_Request/SIF_MaxBufferSize is a mandatory element",
                        fZone);
                }
                maxBufSize = maximumBufferSize.Value;


                if (maxBufSize < 4096 || maxBufSize > Int32.MaxValue)
                {
                    throw new SifException
                        (
                        SifErrorCategoryCode.RequestResponse,
                        SifErrorCodes.REQRSP_UNSUPPORTED_MAXBUFFERSIZE_8,
                        "Invalid SIF_MaxBufferSize value (" + maxBufSize + ")",
                        "Acceptable range is 4096 to " + Int32.MaxValue,
                        fZone);
                }

                // Check to see if the Context is supported
                try
                {
                    IList<SifContext> contexts = req.SifContexts;
                }
                catch (AdkNotSupportedException contextNotSupported)
                {
                    throw new SifException(
                            SifErrorCategoryCode.Generic,
                            SifErrorCodes.GENERIC_CONTEXT_NOT_SUPPORTED_4,
                            contextNotSupported.Message, fZone);
                }


                //  Lookup the SIF_QueryObject
                q = req.SIF_Query;
                if (q == null)
                {
                    // If it's a SIF_ExtendedQuery or SIF_Example, throw the appropriate error
                    if (req.SIF_ExtendedQuery != null)
                    {
                        throw new SifException(
                            SifErrorCategoryCode.RequestResponse,
                            SifErrorCodes.REQRSP_NO_SUPPORT_FOR_SIF_EXT_QUERY,
                            "SIF_ExtendedQuery is not supported", fZone);
                    }
                    else
                    {
                        throw new SifException
                            (
                            SifErrorCategoryCode.Xml,
                            SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                            "SIF_Request/SIF_Query is a mandatory element",
                            fZone);
                    }
                }

                qo = q.SIF_QueryObject;
                if (qo == null)
                {
                    rethrow = true;
                    throw new SifException
                        (
                        SifErrorCategoryCode.Xml,
                        SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                        "SIF_Query/SIF_QueryObject is a mandatory element",
                        fZone);
                }

                //  Lookup the ElementDef for the requested object type
                typ = Adk.Dtd.LookupElementDef(qo.ObjectName);
                if (typ == null)
                {
                    throw new SifException
                        (
                        SifErrorCategoryCode.RequestResponse,
                        SifErrorCodes.REQRSP_INVALID_OBJ_3,
                        "Agent does not support this object type: " + qo.ObjectName,
                        fZone);
                }
            }
            catch (SifException se)
            {
                if (!rethrow)
                {
                    sendErrorResponse(req, se, renderAsVer, maxBufSize);
                }

                //	rethrow all errors at this point
                throw se;


                //                //  Capture the SifException so it can be written to the output stream
                //                //  and thus returned as the payload of the SIF_Response message later
                //                //  in this function.
                //                error = se;
                //                fZone.Log.Error("Error in dispatchRequest that will be put into the SIF_Response", se);
            }


            // For now, SIFContext is not repeatable in SIF Requests

            SifContext requestContext = req.SifContexts[0];
            Object target = null;

            //
            //  Lookup the Publisher for this object type using Topics, 
            // but only if the context is the Default context
            //


            if (typ != null && SifContext.DEFAULT.Equals(requestContext))
            {
                ITopic topic = null;
                topic = fZone.Agent.TopicFactory.LookupInstance(typ, requestContext);
                if (topic != null)
                {
                    target = topic.GetPublisher();
                }
            }


            if (target == null)
            {
                target = fZone.GetPublisher(requestContext, typ);
                

                if (target == null)
                {
                    //
                    //  No Publisher message handler found. Try calling the Undeliverable-
                    //  MessageHandler for the zone or agent. If none is registered,
                    //  return an error SIF_Ack indicating the object type is not
                    //  supported.
                    //
                    Boolean handled = false;
                    IUndeliverableMessageHandler errHandler = fZone.ErrorHandler;
                    if (errHandler != null)
                    {
                        SifMessageInfo msginfo = new SifMessageInfo(req, fZone);

                        handled = errHandler.OnDispatchError(req, fZone, msginfo);

                        //	Notify MessagingListeners...
                        foreach (IMessagingListener ml in GetMessagingListeners(fZone))
                        {
                            ml.OnMessageProcessed(SifMessageType.SIF_Request, msginfo);
                        }


                    }
                    if (!handled)
                    {
                        fZone.Log.Warn("Received a SIF_Request for " + qo.ObjectName + " (MsgId=" + req.MsgId + "), but no Publisher object is registered to handle it");

                        SifException sifEx = new SifException(
                           SifErrorCategoryCode.RequestResponse,
                           SifErrorCodes.REQRSP_INVALID_OBJ_3,
                           "Agent does not support this object type",
                           qo.ObjectName, fZone);
                        sendErrorResponse(req, sifEx, renderAsVer, maxBufSize);
                        throw sifEx;
                    }
                    else
                    {
#if PROFILED 
                                          ( BuildOptions.PROFILED )
							                        ProfilerUtils.profileStop();
#endif

                        return;
                    }
                }
            }


            //bool success;
            DataObjectOutputStreamImpl outStream = null;
            SifMessageInfo msgInfo = new SifMessageInfo(req, fZone);
            Query query = null;

            try
            {
                //  Convert SIF_Request/SIF_Query into a Query object
                if (q != null)
                {
                    query = new Query(q);
                }

                msgInfo.SIFRequestObjectType = typ;
            }
            catch (Exception thr)
            {
                fZone.Log.Debug(thr.ToString());
                SifException sifEx =
                    new SifException
                        (SifErrorCategoryCode.Xml, SifErrorCodes.XML_MALFORMED_2,
                          "Could not parse SIF_Query element", thr.Message, fZone, thr);
                sendErrorResponse(req, sifEx, renderAsVer, maxBufSize);
                throw sifEx;
            }

            try
            {

                outStream = DataObjectOutputStreamImpl.NewInstance();

                outStream.Initialize
                    (
                    fZone,
                    query, 
                    req.SourceId,
                    req.MsgId,
                    renderAsVer,
                    maxBufSize );

                //  Call the agent-supplied Publisher, or if we have an error, write
                //  that error to the output stream instead

               
                ((IPublisher)target).OnRequest(outStream, query, fZone, msgInfo);
                

                //	Notify MessagingListeners...
                NotifyMessagingListeners_OnMessageProcessed
                    (SifMessageType.SIF_Request, msgInfo);

            }
            catch (SifException se)
            {
                //  For a SIF_Request, a SifException (other than a Transport Error)
                //  does not mean to return an error ack but instead to return a
                //  valid SIF_Response with a SIF_Error payload (see the SIF
                //  Specification). Transport Errors must be returned to the ZIS so
                //  that the message will be retried later.
                //
                if (se.Retry || se.ErrorCategory == SifErrorCategoryCode.Transport)
                {
                    //success = false;
                    //retry was requested, so we have to tell the output stream to not send an empty response
                    outStream.DeferResponse();
                    throw;
                }

                outStream.SetError(se.Error);
            }
            catch (AdkException adke)
            {
                //	If retry requested, throw a Transport Error back to the ZIS
                //	instead of returning a SIF_Error in the SIF_Response payload
                if (adke.Retry)
                {
                    //success = false;
                    //retry was requested, so we have to tell the output stream to not send an empty response
                    outStream.DeferResponse();
                    throw;
                }

                //	Return SIF_Error payload in SIF_Response
                SIF_Error err = new SIF_Error();
                err.SIF_Category = (int)SifErrorCategoryCode.Generic;
                err.SIF_Code = SifErrorCodes.GENERIC_GENERIC_ERROR_1;
                err.SIF_Desc = adke.Message;

                outStream.SetError(err);
            }
            catch (Exception thr)
            {
                SIF_Error err = new SIF_Error();
                err.SIF_Category = (int)SifErrorCategoryCode.Generic;
                err.SIF_Code = SifErrorCodes.GENERIC_GENERIC_ERROR_1;
                err.SIF_Desc = "Agent could not process the SIF_Request at this time";

                err.SIF_ExtendedDesc = "Exception in " +
                                        "Publisher.onRequest" +
                                       " message handler: " + thr.ToString();

                outStream.SetError(err);
            }
            finally
            {
                try
                {
                    outStream.Close();
                }
                catch
                {
                    // Do Nothing
                }

                try
                {
                    outStream.Commit();
                }
                catch
                {
                    /* Do Nothing */
                }
#if PROFILED
				ProfilerUtils.profileStop();
#endif
                outStream.Dispose();
            }
        }


        private void sendErrorResponse(SIF_Request req, SifException se, SifVersion renderAsVer, int maxBufSize)
        {

            DataObjectOutputStreamImpl outStream = DataObjectOutputStreamImpl.NewInstance();
            outStream.Initialize(fZone, (IElementDef[])null, req.SourceId, req.MsgId, renderAsVer, maxBufSize);

            SIF_Error err = new SIF_Error(
                (int)se.ErrorCategory,
                se.ErrorCode,
                se.ErrorDesc);
            err.SIF_ExtendedDesc = se.ErrorExtDesc;

            outStream.SetError(err);
            try
            {
                outStream.Close();
            }
            catch (Exception ignored)
            {
                fZone.Log.Warn("Ignoring exception in out.close()", ignored);
            }

            try
            {
                outStream.Commit();
            }
            catch (Exception ignored)
            {
                fZone.Log.Warn("Ignoring exception in out.commit()", ignored);
            }
        }



        private void NotifyMessagingListeners_OnMessageProcessed(SifMessageType messageType,
                                                                  SifMessageInfo info)
        {
            IEnumerable<IMessagingListener> msgList = GetMessagingListeners(fZone);
            foreach (IMessagingListener listener in msgList)
            {
                listener.OnMessageProcessed(messageType, info);
            }
        }

        /// <summary>  Checks out an EvDisp thread for the dispatchEvent method</summary>
        private EvDisp checkoutEvDisp(Event forEvent)
        {
            EvDisp d = new EvDisp(this);

            if (fEvDispCache == null)
            {
                fEvDispCache = new Hashtable();
            }
            fEvDispCache[forEvent] = d;

            return d;
        }

        /// <summary>  Checks in an EvDisp thread previously obtained with checkoutEvDisp</summary>
        private void checkinEvDisp(Event forEvent)
        {
            if (fEvDispCache != null)
            {
                fEvDispCache.Remove(forEvent);
            }
        }

        /// <summary>  Sends a message.</summary>
        /// <remarks>
        /// <para>
        /// If the message is a SIF_Event or SIF_Response message it is persisted to
        /// the Agent Local Queue (if enabled) to ensure reliable delivery. The
        /// message is then sent. Upon successful delivery to the ZIS, the message
        /// is removed from the queue. For all other message types the message is
        /// sent immediately without being posted to the queue.</para>
        /// <para>
        /// If an exception occurs and a message has been persisted to the queue,
        /// it is removed from the queue if possible (the queue is operational) and
        /// the send operation fails.</para>
        /// </remarks>
        public virtual SIF_Ack send(SifMessagePayload msg)
        {
            return send(msg, false);
        }

        /// <summary>
        /// Sends the message to the SIF Zone and returns the SIFAck that was received
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="isPullMessage"></param>
        /// <returns></returns>
        public virtual SIF_Ack send(SifMessagePayload msg,
                                     bool isPullMessage)
        {
            if (fZone.ProtocolHandler == null)
            {
                throw new AdkTransportException("Zone is not connected", fZone);
            }

            try
            {
                PolicyManager policyMan = PolicyManager.GetInstance(fZone);
                if (policyMan != null)
                {
                    policyMan.ApplyOutboundPolicy(msg, fZone);
                }
            }
            catch (AdkException adkex)
            {
                throw new AdkMessagingException("Unable to apply outbound message policy: " + adkex, fZone, adkex);
            }

            SIF_Ack ack = null;
            SifWriter w = null;
            byte stage = 1;
            bool queued = false;
            SifMessageType pload = 0;
            bool cancelled = false;
            ICollection<IMessagingListener> msgList = null;

            try
            {
                //  Assign values to message header
                SIF_Header hdr = msg.Header;
                hdr.SIF_Timestamp = DateTime.Now;
                hdr.SIF_MsgId = SifFormatter.GuidToSifRefID(Guid.NewGuid());
                hdr.SIF_SourceId = fSourceId;
                hdr.SIF_Security = secureChannel();

                //	Adk 1.5+: SIF_LogEntry requires that we *duplicate* the
                //	header within the object payload. This is really the only
                //	place we can do that and ensure that the SIF_Header and
                //	the SIF_LogEntry/SIF_LogEntryHeader are identical.

                if (msg is SIF_Event)
                {
                    // TODO: Should this be done at a higher level, such as zone.ReportEvent()?
                    SIF_Event ev = ((SIF_Event)msg);
                    SIF_ObjectData od =
                        (SIF_ObjectData)ev.GetChild(InfraDTD.SIF_EVENT_SIF_OBJECTDATA);
                    SIF_EventObject eo = od == null ? null : od.SIF_EventObject;
                    if (eo != null)
                    {
                        SIF_LogEntry logentry = (SIF_LogEntry)eo.GetChild(InfraDTD.SIF_LOGENTRY);
                        if (logentry != null)
                        {
                            SIF_LogEntryHeader sleh = new SIF_LogEntryHeader();
                            sleh.SIF_Header = (SIF_Header)hdr.Clone();
                            logentry.SIF_LogEntryHeader = sleh;
                        }
                    }
                }

                if (!isPullMessage || (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0)
                {
                    msg.LogSend(fZone.Log);
                }
            }
            catch (Exception thr)
            {
                throw new AdkMessagingException
                    ("MessageDispatcher could not assign outgoing message header: " + thr,
                      fZone, thr);
            }

            try
            {
                //  Convert message to a stream
                using (MessageStreamImpl memBuf = new MessageStreamImpl())
                {
                    w = new SifWriter(memBuf.GetInputStream());
                    w.Write(msg);
                    w.Flush();

                    stage = 2;

                    //  SIF_Event and SIF_Response are posted to the agent local queue
                    //  if enabled, otherwise sent immediately. All other message types
                    //  are sent immediately even when the queue is enabled.
                    //
                    if (fQueue != null &&
                         (msg is SIF_Event ||
                          msg is SIF_Response))
                    {
                        fQueue.PostMessage(msg);
                        queued = true;
                    }

                    //	Notify MessagingListeners...
                    pload = (SifMessageType)Adk.Dtd.GetElementType(msg.ElementDef.Name);
                    if (pload != SifMessageType.SIF_Ack)
                    {
                        msgList = GetMessagingListeners(fZone);
                        if (msgList.Count > 0)
                        {
                            StringBuilder stringBuffer = new StringBuilder(memBuf.Decode());
                            SifMessageInfo msgInfo = new SifMessageInfo(msg, fZone);
                            foreach (IMessagingListener listener in msgList)
                            {
                                try
                                {
                                    if (!listener.OnSendingMessage(pload, msgInfo, stringBuffer)
                                        )
                                    {
                                        cancelled = true;
                                    }
                                }
                                catch { } // Do Nothing
                            }
                        }
                    }

                    if (!cancelled)
                    {
                        //  Send the message
                        IMessageInputStream ackStream = fZone.fProtocolHandler.Send(memBuf);
                        {
                            try
                            {
                                //  Parse the results into a SIF_Ack
                                ack =
                                    (SIF_Ack)
                                    fParser.Parse
                                        ( ackStream.GetInputStream(), fZone,
                                          isPullMessage
                                              ? SifParserFlags.ExpectInnerEnvelope
                                              : SifParserFlags.None );
                            }
                            catch (Exception parseEx)
                            {
                                if (isPullMessage && (parseEx is AdkParsingException || parseEx is SifException || parseEx is System.Xml.XmlException))
                                {
                                    String ackStr = ackStream.ToString();
                                    if ((Adk.Debug & AdkDebugFlags.Message_Content ) != 0)
                                    {
                                        fZone.Log.Info( ackStr );
                                    }
                                    // The SIFParse was unable to parse this message. Try to create an appropriate
                                    // SIF_Ack, if SIFMessageInfo is able to parse enough of the message
                                    throw new PullMessageParseException(parseEx, ackStr, fZone);

                                }

                                throw new AdkMessagingException(parseEx.Message, fZone);
                            }
                        }

                        if (ack != null)
                        {
                            ack.message = msg;
                            if (!isPullMessage || (Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0)
                            {
                                ack.LogRecv(fZone.Log);
                            }
                        }

                        //	Notify MessagingListeners...
                        if (msgList != null && msgList.Count > 0)
                        {
                            SifMessageInfo msgInfo = new SifMessageInfo(msg, fZone);

                            foreach (IMessagingListener listener in msgList)
                            {
                                try
                                {
                                    listener.OnMessageSent(pload, msgInfo, ack);
                                }
                                catch { } // Do Nothing
                            }
                        }
                    }
                    else
                    {
                        //	Prepare a success SIF_Ack to return
                        ack = msg.AckImmediate();
                    }
                }
            }
            catch (AdkMessagingException)
            {
                throw;
            }
            catch (AdkTransportException)
            {
                throw;
            }
            catch (Exception thr)
            {
                if (stage == 1)
                {
                    throw new AdkMessagingException
                        (
                        "MessageDispatcher could not convert outgoing infrastructure message to a string: " +
                        thr, fZone);
                }
                if (stage == 2)
                {
                    throw new AdkMessagingException
                        (
                        "MessageDispatcher could not convert SIF_Ack response to an object: " + thr,
                        fZone);
                }
            }
            finally
            {
                //  Removed queued message from queue
                if (queued)
                {
                    try
                    {
                        fQueue.RemoveMessage(msg.MsgId);
                    }
                    catch
                    {
                        // Do nothing
                    }
                }
            }

            return ack;
        }

        /// <summary>
        /// Locks until the pull thread can start again
        /// </summary>
        public virtual void notifyPullCanStart()
        {
            if (fPullCanStart != null)
            {
                lock (fPullCanStart)
                {
                    Monitor.PulseAll(fPullCanStart);
                    fPullCanStart = null;
                }
            }
        }

        /// <summary>  Poll the ZIS for messages pending in the agent's queue.
        /// 
        /// This method is typically called by a ProtocolHandler thread to perform a
        /// periodic pull when the agent is running in pull mode. It may also be
        /// called by the framework to force a pull if the framework requires an
        /// immediate response to a message it has sent to the ZIS, such as a
        /// request for SIF_ZoneStatus.
        /// 
        /// 
        /// If a message is retrieved from the ZIS, it is dispatched through the
        /// Adk's usual message routing mechanism just as pushed messages are. Thus,
        /// there is no difference between push and pull mode once a message has
        /// been obtained from the ZIS. Because message routing is asynchronous (i.e.
        /// the MessageDispatcher will forward the message to the appropriate
        /// framework or agent code), this method does not return a value. If an
        /// error is returned in the SIF_Ack, the agent's FaultListener will be
        /// notified if one is registered. If an exception occurs, it is thrown to
        /// the caller.
        /// 
        /// 
        /// Each time this method is invoked it repeatedly sends SIF_GetMessage to
        /// the ZIS until no more messages are available, effectively emptying out
        /// the agent's queue.
        /// 
        /// </summary>
        /// <returns> zero if no messages were waiting in the agent's queue; 1 if a
        /// message was pulled from the agent's queue; -1 if the zone is sleeping
        /// </returns>
        public virtual int Pull()
        {
            //  Wait for fPullCanStart to be set to true by the ZoneImpl class once
            //  the zone is connected
            if (fPullCanStart != null)
            {
                lock (fPullCanStart)
                {
                    try
                    {
                        Monitor.Wait(fPullCanStart);
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }
            }

            while (true)
            {
                if ((Adk.Debug & AdkDebugFlags.Messaging_Pull) != 0)
                {
                    fZone.Log.Debug("Polling for next message...");
                }

                //  Send a SIF_GetMessage, get a SIF_Ack
                SIF_SystemControl sys = new SIF_SystemControl();
                SIF_SystemControlData cmd = new SIF_SystemControlData();
                cmd.AddChild(new SIF_GetMessage());
                sys.SIF_SystemControlData = cmd;
                SIF_Ack ack = null;
                try
                {
                    ack = send(sys, true);
                }
                catch (PullMessageParseException pmpe)
                {
                    // Unable to parse the pulled message. Try sending the proper
                    // Error SIF_Ack to remove the message from the queue
                    if (pmpe.fSourceMessage != null)
                    {
                        fZone.Log.Debug("Handling exception by creating a SIF_Error", pmpe.fParseException);
                        // Try parsing out the SIF_OriginalMsgId so that we can remove the message
                        // from the queue.
                        // Ack either the SIF_Ack or the internal, embedded message, based on our setting
                        int startIndex = fAckAckOnPull ? 0 : 10;
                        int messageStart = pmpe.fSourceMessage.IndexOf("<SIF_Message", startIndex);
                        SifException sourceException = null;
                        if (pmpe.fParseException is SifException)
                        {
                            sourceException = (SifException)pmpe.fParseException;
                        }
                        else
                        {
                            sourceException = new SifException(
                                SifErrorCategoryCode.Xml,
                                SifErrorCodes.XML_GENERIC_ERROR_1,
                                "Unable to parse pulled SIF_Message",
                                pmpe.fParseException.Message,
                                fZone, pmpe.fParseException );
                        }
                        SIF_Ack errorAck = SIFPrimitives.ackError(pmpe.fSourceMessage.Substring(messageStart), sourceException, fZone);
                        errorAck.SifVersion = sys.SifVersion;
                        send( errorAck );
                        continue;
                    }
                }


                //
                //  Process the response. If status code 9 (no message), no
                //  action is taken. If status code 0 (success), the content of
                //  the SIF_Status / SIF_Data element is parsed and dispatched.
                //  If an error is reported in the ack, the agent's fault
                //  handler is called with a SifException describing the error
                //  if a fault handler has been registered.
                //
                if (ack.HasStatusCode(SifStatusCodes.NO_MESSAGES_9))
                {
                    if ((Adk.Debug & AdkDebugFlags.Messaging_Pull ) != 0)
                        fZone.Log.Debug( "No messages waiting in agent queue" );

                    return 0;
                }

                if(ack.HasError())
                {
                    SifException se = new SifException(ack, fZone);
                    fZone.Log.Debug("Unable to pull the next message from the queue: " + se.ToString());
                    AdkUtils._throw(se, fZone.Log);
                }

                if (ack.HasStatusCode(SifStatusCodes.SUCCESS_0))
                {
                    AdkException parseEx = null;
                    SifMessagePayload payload = getPullMessagePayload(ack);
                    if ((Adk.Debug & ( AdkDebugFlags.Messaging | AdkDebugFlags.Messaging_Pull ) ) != 0)
                    {
                        fZone.Log.Debug("Pulled a " + payload.ElementDef.Tag(payload.SifVersion) + " message (SIF " + payload.SifVersion + ")");
                    }

                    #region	Notify MessagingListeners...

                    bool cancelled = false;
                    ICollection<IMessagingListener> msgList = GetMessagingListeners(fZone);
                    if (msgList.Count > 0)
                    {
                        StringWriter tmp = new StringWriter();
                        SifWriter sifwriter = new SifWriter(tmp);
                        sifwriter.Write(payload);
                        sifwriter.Flush();
                        tmp.Flush();

                        StringBuilder xml = new StringBuilder();
                        xml.Append(tmp.ToString());

                        //	Determine message type before parsing
                        foreach (IMessagingListener listener in msgList)
                        {
                            try
                            {
                                SifMessageType pload =
                                    (SifMessageType)
                                    Adk.Dtd.GetElementType(payload.ElementDef.Name);
                                MessagingReturnCode code = listener.OnMessageReceived(pload, xml);
                                switch (code)
                                {
                                    case MessagingReturnCode.Discard:
                                        cancelled = true;
                                        break;

                                    case MessagingReturnCode.Reparse:
                                        {
                                            try
                                            {
                                                //	Reparse the XML into a new message
                                                payload =
                                                    (SifMessagePayload)
                                                    fParser.Parse(xml.ToString(), fZone);
                                            }
                                            catch (IOException ioe)
                                            {
                                                parseEx =
                                                    new AdkException
                                                        (
                                                        "Failed to reparse message that was modified by MessagingListener: " +
                                                        ioe, fZone);
                                            }
                                        }
                                        break;
                                }
                            }
                            catch (AdkException adke)
                            {
                                parseEx = adke;
                            }
                        }
                    }

                    #endregion

                    if (fQueue != null)
                    {
                        //  TODO: put message on agent local queue
                    }
                    else
                    {
                        if (parseEx != null)
                        {
                            throw parseEx;
                        }

                        int ackStatus = SifStatusCodes.IMMEDIATE_ACK_1;
                        SifException err = null;
                        bool acknowledge = true;

                        try
                        {
                            //  Dispatch the message
                            if (!cancelled)
                            {
                                ackStatus = dispatch(payload);
                            }
                        }
                        catch (LifecycleException)
                        {
                            throw;
                        }
                        catch (SifException se)
                        {
                            err = se;
                        }
                        catch (AdkException adke)
                        {
                            //  TODO: This needs to generate proper category/code based on payload

                            if (adke.HasSifExceptions())
                            {
                                //  Return the first exception
                                err = adke.SIFExceptions[0];
                            }
                            else
                            {
                                //  Build a SifException to describe this AdkException
                                err =
                                    new SifException
                                        (SifErrorCategoryCode.Generic,
                                          SifErrorCodes.GENERIC_GENERIC_ERROR_1, adke.Message, fZone);
                            }
                        }
                        catch (Exception thr)
                        {
                            //  Uncaught exception (probably an Adk internal error)
                            string txt =
                                "An unexpected error occurred while processing a pulled message: " +
                                thr;
                            fZone.Log.Debug(txt);

                            //  Build a SifException to describe this Throwable
                            err =
                                new SifException
                                    (SifErrorCategoryCode.System, SifErrorCodes.SYS_GENERIC_ERROR_1,
                                      thr.Message, thr.ToString(), fZone, thr);

                            fZone.Log.Debug(err);
                        }

                        if (acknowledge)
                        {
                            sendPushAck(ack, payload, ackStatus, err);
                        }
                        else
                            return 1;
                    }
                }
                else
                {
                    // We only get to here if there is no error and no success code
                    if (ack.HasStatusCode(SifStatusCodes.SLEEPING_8))
                    {
                        //  Zone is sleeping
                        return -1;
                    }
                    else
                    {
                        // Unknown condition
                        AdkUtils._throw(new SifException(ack, fZone), fZone.Log );
                    }
                }
            }
        }


        /**
	 * Sends a SIF_Ack in response to a pulled message
	 * @param sifGetMessageAck The original SIF_Ack from the SIF_GetMessage. This is sometimes null, when
	 * 						parsing fails
	 * @param pulledMEssage The message delivered inside of the above ack. NOTE that even if parsing fails,
	 *          the SIFParser tries to return what it can, and will return this message payload (in getParsed()),
	 *          instead of the above container message.
	 * @param ackStatus The status to ack (NOTE: This is ignored if the err property is set)
	 * @param err The error to set in the SIF_Ack
	 */
	private void sendPushAck(SIF_Ack sifGetMessageAck,
			SifMessagePayload pulledMEssage, int ackStatus, SifException err) {
		try
		{
			SIF_Ack ack2 = null;
			if( fAckAckOnPull && sifGetMessageAck != null){
				ack2 = sifGetMessageAck.ackStatus( ackStatus );
			} else {
				ack2 = pulledMEssage.ackStatus(ackStatus);
			}


			//  If an error occurred processing the message, return
			//  the details in the SIF_Ack
			if( err != null )
			{
				fZone.Log.Debug( "Handling exception by creating a SIF_Error", err );

				SIF_Error newErr = new SIF_Error();
				newErr.SIF_Category =  (int)err.ErrorCategory;
				newErr.SIF_Code = err.ErrorCode;
				newErr.SIF_Desc = err.ErrorDesc;
				newErr.SIF_ExtendedDesc = err.ErrorExtDesc;
				ack2.SIF_Error = newErr;

				//  Get rid of the <SIF_Status>
				SIF_Status status = ack2.SIF_Status;
				if( status != null ) {
					ack2.RemoveChild( status );
				}
			}

			//  Send the ack
			send(ack2);
		}
		catch( Exception ackEx )
		{
			fZone.Log.Debug( "Failed to send acknowledgement to pulled message: " + ackEx, ackEx );
		}
	}

	private SifMessagePayload getPullMessagePayload( SIF_Ack sifPullAck )
	{
		 try
         {
				//  Get the next message
				SifElement msg = sifPullAck.SIF_Status.SIF_Data.GetChildList()[0];
				return (SifMessagePayload)msg.GetChildList()[0];
         }
         catch( Exception ex )
         {
             // TT 139 Andy E
             //An Exception occurred while trying to read the contents of the SIF_Ack
             throw new SifException(
             		SifErrorCategoryCode.Xml , SifErrorCodes.XML_MISSING_MANDATORY_ELEMENT_6,
                 "Unable to parse SIF_Ack", fZone, ex );
         }
	}


        /// <summary>
        /// Returns the SIF_Security object that represents the zone's security settings
        /// </summary>
        /// <returns></returns>
        protected internal SIF_Security secureChannel()
        {
            AgentProperties props = fZone.Properties;
            SIF_SecureChannel sec =
                new SIF_SecureChannel
                    (AuthenticationLevel.Wrap(props.AuthenticationLevel.ToString()),
                      EncryptionLevel.Wrap(props.EncryptionLevel.ToString()));

            return new SIF_Security(sec);
        }

        /// <summary>
        /// Shuts the MessageDispatcher down
        /// </summary>
        public virtual void shutdown()
        {
            fRunning = null;
        }

        private void Run()
        {
            fRunning = new Object();

            SifMessageInfo[] messages = null;

            while (fRunning != null)
            {
                try
                {
                    //  Wait for the next incoming message from any zone and of any type
                    messages = fQueue.nextMessage(SifMessageType.Any, MessageDirection.Incoming);

                    //  Dispatch all incoming messages...
                    for (int i = 0; i < messages.Length; i++)
                    {
                        try
                        {
                            SifMessagePayload msg = null;
                            dispatch(msg);
                            fQueue.RemoveMessage(messages[i].MsgId);
                        }
                        catch (Exception adke)
                        {
                            Console.Out.WriteLine
                                (
                                "Messenger could not dispatch message from Agent Local Queue (agent: \"" +
                                fZone.Agent.Id + "\", zone: \"" + fZone.ZoneId + "\": " + adke);
                        }
                    }
                }
                catch (Exception adke)
                {
                    Console.Out.WriteLine
                        (
                        "MessageDispatcher could not get next message from Agent Local Queue (agent: \"" +
                        fZone.Agent.Id + "\", zone: \"" + fZone.ZoneId + "\": " + adke);
                }
            }
        }

        /// <summary>  EvDisp thread handles a single dispatch request. Used when ALQ is disabled.</summary>
        internal class EvDisp
        {
            internal EvState _state; // state of dispatch
            internal Object _idle; // signaled when a request received
            internal bool _alive = false; // flag thread is alive
            internal ISubscriber _target;
            internal Event _event;
            internal ZoneImpl _zone;
            internal ITopic _topic;
            internal IMessageInfo _msgInfo;
            private MessageDispatcher fDispatcher;
            // TODO: Implement SMB
            //internal SMBHelper _smb;

            /// <summary>  Constructs an EvDisp and starts its thread</summary>
            public EvDisp(MessageDispatcher dispatcher)
            {
                fDispatcher = dispatcher;
                _state = new EvState();
                _idle = new Object();
            }

            /// <summary>  Signals the waitForAckCode method that the processing of the
            /// dispatch request is ready for MessageDispatcher to return a
            /// SIF_Ack with this code.
            /// </summary>
            public virtual void notifyAckCode(int code)
            {
                lock (_state)
                {
                    _state._ack = code;
                    _state._exception = null;
                }
                lock (_idle)
                {
                    Monitor.PulseAll(_idle);
                }
            }

            /// <summary>  Signals the waitForAckCode method that an exception occurred
            /// during the processing of the dispatch request. It should stop
            /// waiting for a code and instead throw the exception.
            /// </summary>
            public virtual void notifyException(Exception thr)
            {
                lock (_state)
                {
                    _state._ack = -1;
                    _state._exception = thr;
                }
                lock (_idle)
                {
                    Monitor.PulseAll(_idle);
                }
            }

            /// <summary>  Waits for the EvDisp thread to either signal that an Intermediate
            /// acknowledgement should be returned to the ZIS, signal that processing
            /// has been completed and an Immediate acknowledgement should be
            /// returned, or signal that an exception occurred. If an exception
            /// occurred, it is rethrown by this method.
            /// 
            /// </summary>
            /// <returns> The SIF_Ack code (either 1 or 2) that should be returned to
            /// the ZIS in response to the processing of the SIF_Event
            /// </returns>
            private int waitForAckCode()
            {
                int result = -1;

                try
                {
                    lock (_state)
                    {
                        //  Was it changed because of an exception? If so throw it
                        if (_state._exception != null)
                        {
                            if ((Adk.Debug & AdkDebugFlags.Messaging) != 0)
                            {
                                fDispatcher.fZone.Log.Debug
                                    (
                                    "EvDisp received an exception instead of an acknowledgement code");
                            }
                            if (_state._exception is AdkMessagingException)
                            {
                                AdkUtils._throw
                                    ((AdkMessagingException)_state._exception,
                                      fDispatcher.fZone.Log);
                            }
                            else
                            {
                                AdkMessagingException adkme = new AdkMessagingException
                                    (
                                    "Dispatching SIF_Event: " + _state._exception,
                                    fDispatcher.fZone, _state._exception);
                                if (_state._exception is AdkException)
                                {
                                    // ensure that retry support is always enabled
                                    adkme.Retry = ((AdkException)_state._exception).Retry;
                                }

                                AdkUtils._throw(adkme, fDispatcher.fZone.Log);
                            }
                        }

                        //  Return the SIF_Ack code the caller is waiting for
                        if ((Adk.Debug & AdkDebugFlags.Messaging) != 0)
                        {
                            fDispatcher.fZone.Log.Debug
                                ("EvDisp received acknowledgement code " + _state._ack);
                        }

                        result = _state._ack;
                    }
                }
                catch (ThreadInterruptedException ie)
                {
                    if ((Adk.Debug & AdkDebugFlags.Messaging_Event_Dispatching) != 0)
                    {
                        fDispatcher.fZone.Log.Debug("EvDisp interrupted waiting for ack code");
                    }

                    AdkUtils._throw
                        (new AdkMessagingException(ie.ToString(), fDispatcher.fZone),
                          fDispatcher.fZone.Log);
                }

                return result;
            }

            /// <summary>  Wakes up the thread, dispatches the event to Subscriber.onEvent,
            /// then returns the appropriate SIF_Ack code. If onEvent() completes
            /// processing without invoking SMB, this method returns once onEvent()
            /// is finished. If, however, onEvent() invokes SMB by instantiating a
            /// TrackQueryResults object, this method returns at the time the
            /// TrackQueryResults constructor is called.
            /// </summary>
            internal virtual int dispatch(ISubscriber target,
                                           Event evnt,
                                        ZoneImpl zone,
                                        ITopic topic,
                                        IMessageInfo msgInfo)
            {
                _target = target;
                _event = evnt;
                _zone = zone;
                _topic = topic;
                _msgInfo = msgInfo;
                // TODO: Implement SMB
                //_smb = null;

                _state._ack = -1;
                _state._exception = null;

                this.Run();

                return waitForAckCode();
            }

            internal void Run()
            {
                try
                {
                    if ((Adk.Debug & AdkDebugFlags.Messaging) != 0)
                    {
                        fDispatcher.fZone.Log.Debug("EvDisp received dispatch request");
                    }

                    //  Dispatch event to subscriber...
                    _target.OnEvent(_event, _zone, _msgInfo);

                    //	Notify MessagingListeners...
                    ICollection<IMessagingListener> msgList = GetMessagingListeners(fDispatcher.fZone);
                    if (msgList.Count > 0)
                    {
                        foreach (IMessagingListener listener in msgList)
                        {
                            listener.OnMessageProcessed(SifMessageType.SIF_Event, _msgInfo);
                        }
                    }

                    //  The event was successfully processed, so either send an
                    //  immediate SIF_Ack with status code 1, *or* if SMB was
                    //  invoked by a TrackQueryResults object, ask the SMBHelper
                    //  to send a final SIF_Ack with status code 3.
                    //

                    // TODO: Implement SMB
                    //					if (_smb == null)
                    //					{
                    notifyAckCode(1); // send SIF_Ack(1)
                    //					}
                    //					else
                    //					{
                    //						_smb.endSMB(); // send SIF_Ack(3)
                    //					}
                }
                catch (SifException se)
                {
                    //Thread.CurrentThread.Interrupt();
                    notifyException(se);
                }
                catch (AdkException adke)
                {
                    //Thread.CurrentThread.Interrupt();
                    notifyException(adke);
                }
                catch (Exception thr)
                {
                    //Thread.CurrentThread.Interrupt();
                    fDispatcher.fZone.Log.Error("Uncaught exception in onEvent: " + thr);
                    notifyException(thr);
                }
            }
        }

        private void logAndThrowSIFException(String shortMessage, Exception exception)
        {
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 )
            {
                fZone.Log.Error( shortMessage, exception );
            }


            SifException exToThrow = new SifException(
                SifErrorCategoryCode.Generic,
                SifErrorCodes.GENERIC_GENERIC_ERROR_1,
                shortMessage,
                exception.StackTrace,
                fZone, exception );
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 )
            {
                fZone.Log.Error( "Translated to a SIFException", exToThrow );
            }
            throw exToThrow;
        }

        private void logAndRethrow(String shortMessage, AdkException exception)
        {
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 )
            {
                fZone.Log.Error( shortMessage, exception );
            }
            throw exception;
        }

        private void logAndThrowRetry(String shortMessage, Exception exception)
        {
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 )
            {
                fZone.Log.Error( shortMessage, exception );
            }
            SifException exToThrow = new SifException(
                SifErrorCategoryCode.Transport,
                SifErrorCodes.WIRE_GENERIC_ERROR_1,
                shortMessage,
                exception.StackTrace,
                fZone );
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 )
            {
                fZone.Log.Error( "Translated to a SIFException that will force a retry", exToThrow );
            }
            throw exToThrow;
        }

        sealed class PullMessageParseException : AdkMessagingException
        {

            public Exception fParseException;
            public String fSourceMessage;

            public PullMessageParseException( Exception parseException, String sourceMessage, IZone zone)
                : base(parseException.Message, zone)
            {

                fSourceMessage = sourceMessage;
                fParseException = parseException;

            }

        }

        internal sealed class EvState
        {
            internal int _ack = -1;
            internal Exception _exception = null;
        }


        ///<summary> Implements RequestInfo for cases where a cache lookup fails. The ADK
        /// always guarantees that SIFMessageInfo.getSIFRequestInfo() will be a non-null
        /// value in the onQueryResults and onQueryPending message handlers
        /// </summary>
        internal sealed class UnknownRequestInfo : IRequestInfo
        {
            private String fMsgId;
            private String fObjectType;

            public UnknownRequestInfo(String msgId,
                                       String objectType)
            {
                fMsgId = msgId;
                fObjectType = objectType;
            }

            public String ObjectType
            {
                get { return fObjectType; }
            }

            public String MessageId
            {
                get { return fMsgId; }
            }

            public DateTime RequestTime
            {
                get { return DateTime.MinValue; }
            }

            public bool IsActive
            {
                get { return false; }
            }

            public object UserData
            {
                get { return null; }
            }
        }
    }
}
