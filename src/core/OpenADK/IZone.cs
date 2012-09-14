//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;
using OpenADK.Library.Log;
using log4net;

namespace OpenADK.Library
{
    /// <summary>  A Zone is a logical grouping of applications that exchange data through the
    /// Schools Interoperability Framework. Each zone is managed by a Zone Integration
    /// Server (ZIS). Refer to the SIF Specifications for a more detailed definition.
    /// Agents developed with the Adk may connect to multiple zones concurrently.
    /// 
    /// Zones have the following characteristics:
    /// 
    /// 
    /// <ul>
    /// <li>
    /// Each zone connects to a Zone Integration Server, which provides a
    /// reliable and persistent queue for the storage of messages pending
    /// delivery to the agent. The "connection" is a logical one. The Adk
    /// handles retrieving messages from the queue and dispatching them to
    /// the <i>Publisher</i>, <i>Subscriber</i>, and <i>QueryResults</i>
    /// interfaces registered with your agent's Zone or Topic objects.
    /// <br/><br/>
    /// </li>
    /// <li>
    /// The Adk provides a persistent and reliable Agent Local Queue (ALQ)
    /// for each zone. Messages retrieved from the Zone Integration Server
    /// are stored in the local queue before being processed. A given
    /// message exists in the local queue or in the server queue but never
    /// in both. The Agent Local Queue provides for enhanced reliability
    /// and supports disconnected communications with the server.<br><br>
    /// </li>
    /// <li>
    /// Each zone maintains its own set of properties. By default, a zone
    /// inherits its default properties from the agent. To set properties
    /// on a per-zone basis, call the <c>Zone.getProperties</c> method
    /// to obtain the zone's AgentProperties object, then call its accessor
    /// methods. Properties must be set prior to calling <c>connect</c>.
    /// <br><br>
    /// </li>
    /// </ul>
    /// 
    /// In order to exchange messages with a zone, the agent must be <i>connected</i>
    /// to the zone. The act of connecting establishes local resources for transport
    /// protocols and queuing. Depending on the flags passed to the <c>connect</c>
    /// method, the agent may also send SIF registration messages to establish
    /// similar resources on the server.
    /// 
    /// An agent obtains a Zone instance by calling <c>Agent.getZoneFactory</c>.
    /// When finished interacting with a zone, call the <c>disconnect</c>
    /// method to release local resources held by the Adk as well as to optionally
    /// send a SIF_Unregister message to the server. Disconnecting all zones to
    /// which an agent is connected is easily done by calling the <c>Agent.shutdown</c>
    /// method when the agent exits.
    /// 
    /// Agents typically use <i>Topic</i> objects to aggregate publish, subscribe,
    /// and query activity across multiple zones. Refer to the Topic interface for
    /// details.
    /// 
    /// </summary>
    /// <author>  Eric Petersn
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public interface IZone : IProvisioner
    {
        /// <summary>Assigns a new properties object to this zone.</summary>
        /// </summary>
        /// <value>A new properties object to replace the existing object
        /// </value>
        /// <remarks>
        /// By default, a zone inherits the default agent properties. To change a
        /// property, first call the getProperties method to obtain the zone's
        /// properties object, then call any of the accessor methods available on
        /// that object. Because zones are constructed with a default AgentProperties
        /// object, it is typically not necessary to call this method.
        /// </remarks>
        AgentProperties Properties { get; set; }

        /// <summary>  Gets the Agent object</summary>
        /// <value> The Agent that created this Zone instance
        /// </value>
        Agent Agent { get; }

        /// <summary>  Gets the zone identifier</summary>
        /// <value>s The name of the zone as defined by the Zone Integration Server
        /// </value>
        string ZoneId { get; }

        /// <summary>  Gets the URL of the Zone Integration Server that manages this zone</summary>
        /// <value>s The URL to the ZIS (e.g. "http://host:port/zoneId" is the URL
        /// convention employed by the Library ZIS)
        /// </value>
        Uri ZoneUrl { get; }

        /// <summary>  Gets a list of any SIF Errors that resulted from the sending of provisioning
        /// messages to the zone. Only access control errors (Category 4) are treated as
        /// warnings rather than errors; all other SIF Errors result in an exception
        /// thrown by the <code>connect</code> method
        /// 
        /// </summary>
        /// <value> An array of SIFExceptions
        /// </value>
        IList<SifException> ConnectWarnings { get; }

        /// <summary>  Gets the connection state of this zone</summary>
        /// <value> true if the <i>connect</i> method has been called but the <i>
        /// disconnect</i> method has not
        /// </value>
        bool Connected { get; }


        /// <summary>  Sets the <i>UndeliverableMessageHandler</i> to be called when a dispatching
        /// error occurs on this zone. For more information, please refer to the
        /// UnderliverableMessageHandler class comments.
        /// </summary>
        /// <value>The handler to call when the Adk cannot dispatch an
        /// inbound message
        /// </value>
        IUndeliverableMessageHandler ErrorHandler { get; set; }

        /// <summary>Gets or Sets an application-supplied object to this Zone</summary>
        /// <value>Any object the application wishes to attach to this Zone instance
        /// </value>
        object UserData { get; set; }

        /// <summary>  Gets the root logging framework Category for this agent.</summary>
        ILog Log { get; }

        /// <summary> 	Gets the ServerLog for this zone.</summary>
        /// <value> The ServerLog instance for the zone
        /// @since Adk 1.5
        /// </value>
        ServerLog ServerLog { get; }

        IProtocolHandler ProtocolHandler { get; }

        /// <summary>Connects the agent with this zone.</summary>
        /// <remarks>
        /// An agent must connect to a zone in order to perform messaging within the
        /// context of the Adk Class Framework. A typical agent calls this method at
        /// startup for each zone it will connect to, then optionally joins the Zone
        /// with one or more Topics.
        /// 
        /// The Adk's Provisioning Mode affects the messages sent by this method.
        /// Refer to the AgentProperties class for more information.
        /// 
        /// The Adk will send <code>&lt;SIF_Subscribe&gt;</code> and <code>&lt;SIF_Provide&gt;</code>
        /// messages to the zone to provision the SIF Data Objects for which the agent has registered a
        /// <i>Subscriber</i> and <i>Publisher</i> message handlers, respectively.
        /// The <code>adk.provisioning.batch</code> agent property (refer to the
        /// AgentProperties class) determines whether the Adk sends a single message
        /// or individual messages for each SIF Data Object. When individual messages
        /// are sent (the default), Category 4 Access Control errors are treated as
        /// warnings. Connect warnings can be retrieved by calling the
        /// <code>getConnectWarnings</code> method. When a single message is sent,
        /// any SIF Error will result in the raising of a SifException.
        /// 
        /// Note the SIF_Wakeup and SIF_Ping system control messages are sent to
        /// the server upon successful connection.
        /// 
        /// </remarks>
        /// <exception cref="InvalidOperationException">  is thrown if this zone is already in
        /// the connected state (i.e. <i>connect</i> has already been called
        /// without a corresponding call to <i>disconnect</i>)
        /// </exception>
        /// <exception cref="AdkException">  is thrown if there is a SIF Error acknowledgement
        /// to a <code>&lt;SIF_Register&gt;</code>, <code>&lt;SIF_Subscribe&gt;</code>,
        /// or <code>&lt;SIF_Provide&gt;</code> message as described above
        /// 
        /// </exception>
        /// <seealso cref="Disconnect">
        /// </seealso>
        void Connect( ProvisioningFlags provOptions );

        /// <summary>  Disconnects the agent from this zone.</summary>
        /// <remarks>
        /// Resources held by the Class Framework, including the Agent Local Queue,
        /// are closed. To ensure these resources are properly closed, agents should
        /// disconnect from zones even when not planning on sending a <code>&lt;
        /// SIF_Unregister&gt;</code> provisioning message.
        /// 
        /// Provisioning messages are sent as follows:
        /// 
        /// <ul>
        /// <li>
        /// If the agent is using Adk-managed provisioning, a <code>&lt;
        /// SIF_Unregister&gt;</code> message is sent to the ZIS when the
        /// AdkFlags.PROV_UNREGISTER flag is specified. When
        /// Adk-managed provisioning is disabled, no messages are sent to
        /// the zone.
        /// </li>
        /// <li>
        /// If Agent-managed provisioning is enabled, the flags
        /// flags have no affect. The agent must explicitly call the
        /// sifUnregister method to manually send those message to the zone.
        /// </li>
        /// <li>
        /// If ZIS-managed provisioning is enabled, no provisioning messages
        /// are sent by the agent regardless of the flags
        /// used and the methods are called.
        /// </li>
        /// </ul>
        /// 
        /// 
        /// Note that SIF Agent sessions are long-lived and therefore an agent may
        /// remain registered with a ZIS even when it is "disconnected" from the
        /// perspective of the Adk Class Framework.
        /// 
        /// Disconnecting a zone also places the agent's server queue in sleep mode.
        /// This functionality can be disabled via the zone properties.
        /// 
        /// </remarks>
        /// <exception cref="AdkException">  is thrown if there is an error sending a
        /// <code>&lt;SIF_Unregister&gt;</code> message
        /// 
        /// </exception>
        /// <seealso cref="Connect">
        /// </seealso>
        void Disconnect( ProvisioningFlags flags );

        /// <summary>  Report a SIF Event to the zone</summary>
        /// <param name="ev">An Event object describing the SIF Data Object that has
        /// changed and how it has changed (added, updated, or removed)
        /// </param>
        void ReportEvent( Event ev );

        /// <summary>  Report a SIF Event to the zone</summary>
        /// <param name="obj">The object that was added, changed, or deleted
        /// </param>
        /// <param name="action"><code>Event.ADD</code>, <code>Event.CHANGE</code>, or <code>Event.DELETE</code>
        /// </param>
        void ReportEvent( SifDataObject obj,
                          EventAction action );

        /// <summary>  Report a directed SIF Event to the agent in the zone identified by 
        /// <code>destinationId</code>. Note: Directed SIF Events may not be supported 
        /// by all zone integration servers.
        /// 
        /// </summary>
        /// <param name="obj">The object that was added, changed, or deleted
        /// </param>
        /// <param name="action"><code>Event.ADD</code>, <code>Event.CHANGE</code>, or <code>Event.DELETE</code>
        /// </param>
        /// <param name="destinationId">The SourceId of the agent to which the SIF Event
        /// will be routed by the zone integration server
        /// </param>
        void ReportEvent( SifDataObject obj,
                          EventAction action,
                          string destinationId );

        /// <summary>  Query the zone.
        /// 
        /// </summary>
        /// <param name="query">A Query object describing the parameters of the query,
        /// including optional conditions and field restrictions
        /// 
        /// </param>
        /// <returns> The SIF_MsgId of the SIF_Request that was sent to the zone.
        /// </returns>
       string Query(Query query);

        /// <summary>  Query the zone and notify a MessagingListener
        /// 
        /// </summary>
        /// <param name="query">A Query object describing the parameters of the query,
        /// including optional conditions and field restrictions
        /// </param>
        /// <param name="listener">A MessagingListener that will be notified when the
        /// SIF_Request message is sent to the zone. Any other MessagingListeners 
        /// registered with the zone will also be called. 
        /// 
        /// </param>
        /// <returns> The SIF_MsgId of the SIF_Request that was sent to the zone.
        /// 
        /// @since Adk 1.5
        /// </returns>
       string Query(Query query,
                      IMessagingListener listener );

        /// <summary>  Query the zone with options.
        /// 
        /// </summary>
        /// <param name="query">A Query object describing the parameters of the query,
        /// including optional conditions and field restrictions.
        /// </param>
        /// <param name="queryOptions">Reserved for future use.
        /// 
        /// </param>
        /// <returns> The SIF_MsgId of the SIF_Request that was sent to the zone
        /// </returns>
       string Query(Query query,
                      AdkQueryOptions queryOptions );

        /// <summary>  Query the zone with options and notify a MessagingListener
        /// 
        /// </summary>
        /// <param name="query">A Query object describing the parameters of the query,
        /// including optional conditions and field restrictions.
        /// </param>
        /// <param name="listener">A MessagingListener that will be notified when the
        /// SIF_Request message is sent to the zone. Any other MessagingListeners 
        /// registered with the zone will also be called. 
        /// </param>
        /// <param name="queryOptions">Reserved for future use.
        /// 
        /// </param>
        /// <returns> The SIF_MsgId of the SIF_Request that was sent to the zone
        /// 
        /// @since Adk 1.5
        /// </returns>
       string Query(Query query,
                      IMessagingListener listener,
                      AdkQueryOptions queryOptions );

        /// <summary>  Query a specific agent registered with this zone (a <i>directed query</i>).
        /// 
        /// Directed queries are used primarily when the source of data is known
        /// because of a message previously received from that agent. For example,
        /// if your agent receives a SIF_Event and you wish to query the author of
        /// that event for additional data, a directed query is appropriate.
        /// 
        /// In addition, some kinds of SIF Data Objects in SIF 1.5 and later may be 
        /// designed to require agents to send directed queries if more than one 
        /// agent in a zone typically offers support for the object. This is necessary 
        /// because only one agent can be the authoritative provider of a given object 
        /// type in each zone.
        /// 
        /// </summary>
        /// <param name="query">A Query object describing the parameters of the query,
        /// including optional conditions and field restrictions
        /// </param>
        /// <param name="destinationId">The SourceId of the agent to which the SIF Request
        /// will be routed by the zone integration server
        /// </param>
        /// <param name="queryOptions">Reserved for future use
        /// 
        /// </param>
        /// <returns> The SIF_MsgId of the SIF_Request that was sent to the zone.
        /// </returns>
       string Query(Query query,
                      string destinationId,
                      AdkQueryOptions queryOptions );


        /// <summary>  Query a specific agent registered with this zone (a <i>directed query</i>)
        /// and notify a MessagingListener.
        /// 
        /// Directed queries are used primarily when the source of data is known
        /// because of a message previously received from that agent. For example,
        /// if your agent receives a SIF_Event and you wish to query the author of
        /// that event for additional data, a directed query is appropriate.
        /// 
        /// In addition, some kinds of SIF Data Objects in SIF 1.5 and later may be 
        /// designed to require agents to send directed queries if more than one 
        /// agent in a zone typically offers support for the object. This is necessary 
        /// because only one agent can be the authoritative provider of a given object 
        /// type in each zone.
        /// 
        /// </summary>
        /// <param name="query">A Query object describing the parameters of the query,
        /// including optional conditions and field restrictions
        /// </param>
        /// <param name="listener">A MessagingListener that will be notified when the
        /// SIF_Request message is sent to the zone. Any other MessagingListeners 
        /// registered with the zone will also be called. 
        /// </param>
        /// <param name="destinationId">The SourceId of the agent to which the SIF Request
        /// will be routed by the zone integration server
        /// </param>
        /// <param name="queryOptions">Reserved for future use
        /// 
        /// </param>
        /// <returns> The SIF_MsgId of the SIF_Request that was sent to the zone.
        /// 
        /// @since Adk 1.5
        /// </returns>
       string Query(Query query,
                      IMessagingListener listener,
                      string destinationId,
                      AdkQueryOptions queryOptions );

        SIF_ZoneStatus GetZoneStatus();

        /// <summary>  Gets the SIF_ZoneStatus object from the ZIS managing this zone. The
        /// method blocks for the specified timeout period.
        /// </summary>
        /// <param name="timeout">The amount of time to wait for a SIF_ZoneStatus object to
        /// be received by the agent 
        /// </param>
        SIF_ZoneStatus GetZoneStatus( TimeSpan timeout );

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
        void AddMessagingListener( IMessagingListener listener );

        /// <summary> 	Remove a <i>MessagingListener</i> previously registered with the
        /// <code>addMessagingListener</code> method.
        /// 
        /// </summary>
        /// <param name="listener">a MessagingListener implementation
        /// </param>
        void RemoveMessagingListener( IMessagingListener listener );

        /// <summary>  Purge all pending incoming and/or outgoing messages from this agent's
        /// queue. Only messages associated with this zone are affected. See also
        /// the Agent.purgeQueue and Topic.purgeQueue methods to purge the queues of
        /// all zones with which the agent is connected, or all zones joined with a
        /// given topic, respectively.
        /// 
        /// <ul>
        /// <li>
        /// If the Agent Local Queue is enabled, messages are permanently
        /// and immediately removed from the queue. Any messages in transit
        /// are not affected.
        /// </li>
        /// <li>
        /// If the underlying messaging protocol offers a mechanism to clear
        /// the agent's queue, it is invoked. (SIF 1.0 does not have such a
        /// mechanism.)
        /// <li>
        /// Otherwise, all incoming messages received by the agent having a
        /// timestamp earlier than or equal to the time this method is called
        /// are discarded. This behavior persists until the agent is
        /// terminated or until a message is received having a later
        /// timestamp.
        /// </li>
        /// </ul>
        /// 
        /// </summary>
        /// <param name="incoming">true to purge incoming messages
        /// </param>
        /// <param name="outgoing">true to purge outgoing messages (e.g. pending SIF_Events)
        /// when the Agent Local Queue is enabled
        /// </param>
        void PurgeQueue( bool incoming,
                         bool outgoing );

        /// <summary>  Puts this zone into sleep mode.
        /// 
        /// A SIF_Sleep message is sent to the Zone Integration Server to request
        /// that this agent's queue be put into sleep mode. If successful, the ZIS
        /// should not deliver further messages to this agent until it is receives
        /// a SIF_Register or SIF_Wakeup message from the agent. Note the Adk keeps
        /// an internal sleep flag for each zone, which is initialized when the
        /// <code>connect</code> method is called by sending a SIF_Ping to the ZIS.
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
        void Sleep();

        /// <summary>  Wakes up this zone if currently in sleep mode.
        /// 
        /// A SIF_Wakeup message is sent to the Zone Integration Server to request
        /// that sleep mode be removed from this agent's queue. Note the Adk keeps
        /// an internal sleep flag for each zone, which is initialized when the
        /// <code>connect</code> method is called by sending a SIF_Ping to the ZIS.
        /// This flag is cleared so that the Adk will no longer return a Status Code
        /// 8 ("Receiver is sleeping") in response to messages received by the ZIS.
        /// 
        /// 
        /// If the SIF_Sleep message is not successful, an exception is thrown and
        /// the Adk's internal sleep flag for this zone is not changed.
        /// 
        /// 
        /// </summary>
        /// <exception cref="AdkException">  thrown if the SIF_Wakeup message is unsuccessful
        /// </exception>
        void WakeUp();

        /// <summary>  Determines if the agent's queue for this zone is in sleep mode.
        /// 
        /// </summary>
        /// <param name="flags">When AdkFlags.LOCAL_QUEUE is specified, returns true if the
        /// Agent Local Queue is currently in sleep mode. False is returned if
        /// the Agent Local Queue is disabled. When AdkFlags.SERVER_QUEUE is
        /// specified, queries the sleep mode of the Zone Integration Server
        /// by sending a SIF_Ping message.
        /// </param>
        bool IsSleeping( AdkQueueLocation flags );

        /// <summary>  Sends a SIF_Register message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        SIF_Ack SifRegister();

        /// <summary>  Sends a SIF_Unregister message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        SIF_Ack SifUnregister();

        /// <summary>  Sends a SIF_Subscribe message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        SIF_Ack SifSubscribe( string [] objectType );

        /// <summary>  Sends a SIF_Unsubscribe message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        SIF_Ack SifUnsubscribe( string [] objectType );

        /// <summary>  Sends a SIF_Provide message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        SIF_Ack SifProvide( string [] objectType );

        /// <summary>  Sends a SIF_Unprovide message to the ZIS. This method can be called by
        /// agents that have chosen to use Agent-managed provisioning. If ZIS-managed
        /// or Adk-managed provisioning is enabled for this zone, the method has no
        /// effect.
        /// </summary>
        SIF_Ack SifUnprovide( string [] objectType );

        /// <summary>  Sends a SIF_Ping message to the ZIS that manages this zone.</summary>
        SIF_Ack SifPing();

        /// <summary>  Sends arbitrary SIF_Message content to the zone. This method does not
        /// alter the message or wrap it in an envelope prior to sending it.
        /// 
        /// </summary>
        /// <param name="xml">A valid SIF_Message complete with a SIF_Header header and a
        /// payload such as SIF_Register, SIF_Request, SIF_Event, etc.
        /// </param>
        /// <returns> A SIF_Ack object encapsulating the SIF_Ack response that was
        /// returned from the Zone Integration Server
        /// </returns>
        SIF_Ack SifSend( string xml );

        /// <summary>  Returns the string representation of this zone as "zoneId@zoneUrl"</summary>
        string ToString();
    }
}
