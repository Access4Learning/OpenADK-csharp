//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  Aggregates publish, subscribe, and query activity across multiple zones.
    /// 
    /// An agent typically registers with its zones at startup, creates Topic
    /// instances for the data types it is interested in, and then joins zones to
    /// each topic by calling <c>Topic.addZone</c>. A given Zone can only be
    /// joined to one Topic. If an agent wishes to manage publish and subscribe
    /// activity on a per-zone basis, it can do so by creating one Topic object for
    /// each zone instead of aggregating zones on a single Topic instance.
    /// 
    /// The Adk applies publish, subscribe, and query functionality equally to all
    /// zones joined to a topic. Thus, if the topic provides authoritative data to
    /// its zones (that is, it is registered with the ZIS as a Provider), it will
    /// do so for all zones by delegating incoming queries to the Publisher
    /// registered with this topic. Similarly, the topic will send change events
    /// to all of its zones when the <c>publish</c> method is called, and will
    /// query all zones for data objects when the <c>query</c> method is
    /// called. (An exception to this is directed queries, which require that the
    /// caller specify a zone and agent name to direct the query.)
    /// 
    /// Consult the Adk Developer Guide for more information about working with
    /// multiple zones.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public interface ITopic
    {
        /// <summary>  Gets the name of the SIF Data Object associated with this topic</summary>
        /// <returns> A SIF Data Object type such as "StudentPersonal"
        /// </returns>
        string ObjectType { get; }

        /// <summary>  Joins a zone with this topic.
        /// 
        /// All SIF messaging performed on this topic will be propagated to all
        /// zones joined with the topic. For example, calling the <c>query</c>
        /// method queries all of the topic's zones, and SIF_Responses received by
        /// each zone are first dispatched to the topic's <i>QueryResults</i> object.
        /// 
        /// </summary>
        /// <param name="zone">Zone created by the agent ZoneFactory
        /// </param>
        /// <exception cref="OpenADK.Library.AdkException"> AdkException is thrown if the zone is already joined to a
        /// topic or if there is a SIF error during agent registration.
        /// </exception>
        void Join(IZone zone);



        ///<Summary>
        /// Gets the ElementDef representing the SIF Data Object associated with this topic
        ///returns The ElementDef used to create this Topic instance, such as StudentDTD.STUDENTPERSONAL
        ///</Summary>
        IElementDef ObjectDef { get; }


        ///<Summary>
        /// Gets the SIFContext that this Topic is bound to. A single Topic can only be bound to a single
        /// SIF Context.
        ///  returns the SiFContext that this Topic is bound to.
        ///</Summary>
        SifContext SifContext { get;}

        /// <summary>  Gets all zones joined to this topic</summary>
        /// <returns> An array of Zones
        /// </returns>
        IZone[] GetZones();


        /// <summary>
        /// Register a Publisher message handler to process SIF_Request messages
        /// received on the zones joined with this topic. The message handler will 
        /// be called whenever a SIF_Request is received for the SIF Data Object type 
        /// associated with the topic.
        /// </summary>
        /// <remarks>
        /// For Topics created to represent the SIF_ReportObject object type, register 
        /// a publisher message handler with the <see cref="ITopic.SetReportPublisher"/> 
        /// method instead
        /// </remarks>
        /// <param name="publisher">An object that implements the <see cref="IPublisher"/> interface</param>
        void SetPublisher(IPublisher publisher );


        /// <summary>
        /// Register a Publisher message handler to process SIF_Request messages
        /// received on the zones joined with this topic. The message handler will 
        /// be called whenever a SIF_Request is received for the SIF Data Object type 
        /// associated with the topic.
        /// </summary>
        /// <remarks>
        /// For Topics created to represent the SIF_ReportObject object type, register 
        /// a publisher message handler with the <see cref="ITopic.SetReportPublisher"/> 
        /// method instead
        /// </remarks>
        /// <param name="publisher">An object that implements the <see cref="IPublisher"/> interface</param>
        /// <param name="flags">Controls whether the ADK registers this agent as the default provider
        /// in the zone or not, as well as, optionally, the SIF_Contexts.</param>
        void SetPublisher(IPublisher publisher, PublishingOptions flags);




        /// <summary>  Gets the <i>Publisher</i> registered with this topic</summary>
        /// <returns> The object passed to the <c>setPublisher</c> method
        /// </returns>
        IPublisher GetPublisher();

        /// <summary>
        ///   Register a Subscriber message handler to process SIF_Event messages 
        ///	received on the zones joined with this topic. The message handler will 
        ///	be called whenever a SIF_Event is received for the SIF Data Object type 
        ///	associated with the topic.
        /// </summary>
        /// <param name="subscriber">An object that implements the <code>Subscriber</code> interface</param>
        /// agent as a subscriber of the object type. The ADK will send a
        /// SIF_Subscribe message to each zone joined with the topic.
        /// </param>
        void SetSubscriber(ISubscriber subscriber);

        /// <summary>
        ///   Register a Subscriber message handler to process SIF_Event messages 
        ///	received on the zones joined with this topic. The message handler will 
        ///	be called whenever a SIF_Event is received for the SIF Data Object type 
        ///	associated with the topic.
        /// </summary>
        /// <param name="subscriber">An object that implements the <code>Subscriber</code> interface</param>
        /// agent as a subscriber of the object type. The ADK will send a
        /// SIF_Subscribe message to each zone joined with the topic.
        /// <param name="flags">Specify options associated with this object subscription  </param>
        void SetSubscriber(ISubscriber subscriber,
                            SubscriptionOptions flags);




        /// <summary>  Gets the <i>Subscriber</i> registered with this topic</summary>
        /// <returns> The object passed to the <c>setSubscriber</c> method
        /// </returns>
        ISubscriber GetSubscriber();


        /// <summary>  Sets the <i>QueryResults</i> message handler registered with this topic.
        /// This object will be called whenever a SIF_Response message is received
        /// by one of the zones joined with this topic and the response contains
        /// data associated with the topic. Note that SIF_Response messages are
        /// dispatched to this handler only if the initial request was issued by
        /// calling one of the <c>Topic.query</c> methods.
        /// 
        /// </summary>
        /// <param name="queryResultsObject">An <c>IQueryResults</c> message handler</param>
        /// <seealso cref="Query(Library.Query)">
        /// </seealso>
        /// <seealso cref="IQueryResults">
        /// </seealso>
        void SetQueryResults(IQueryResults queryResultsObject);


        /// <summary>  Sets the <i>QueryResults</i> message handler registered with this topic.
        /// This object will be called whenever a SIF_Response message is received
        /// by one of the zones joined with this topic and the response contains
        /// data associated with the topic. Note that SIF_Response messages are
        /// dispatched to this handler only if the initial request was issued by
        /// calling one of the <c>Topic.query</c> methods.
        /// 
        /// </summary>
        /// <param name="queryResultsObject">An <c>IQueryResults</c> message handler</param>
        /// <param name="flags"> The QueryResultsOptions that should be used for this agent, or <code>Null</code>
        /// to accept defaults</param>
        /// <seealso cref="Query(Library.Query)">
        /// </seealso>
        /// <seealso cref="IQueryResults">
        /// </seealso>
        void SetQueryResults(IQueryResults queryResultsObject, QueryResultsOptions flags);

        /// <summary>  Gets the <i>QueryResults</i> object that is registered with this topic.
        /// 
        /// </summary>
        /// <returns> The message handler instance passed to the <c>setQueryResults</c>
        /// method, or null if no message handler is registered with this topic.
        /// </returns>
        IQueryResults GetQueryResultsObject();

        /// <summary>  Query the topic by sending a SIF_Request message to all zones joined
        /// with the topic
        /// 
        /// </summary>
        /// <param name="query">A Query object that encapsulates the elements to query and
        /// the optional field restrictions placed on the results
        /// </param>
        void Query(Query query);

        /// <summary>  Query the topic by sending a SIF_Request message to all zones joined
        /// with the topic. This form of the <c>query</c> method also notifies a 
        /// MessagingListener of any SIF_Request messaging that takes place.
        /// 
        /// </summary>
        /// <param name="query">A Query object that encapsulates the elements to query and
        /// the optional field restrictions placed on the results
        /// </param>
        /// <param name="listener">A MessagingListener that will be notified when the
        /// SIF_Request message is sent to the zone. Any other MessagingListeners 
        /// registered with the zone will also be called.
        /// 
        /// @since Adk 1.5 
        /// </param>
        void Query(Query query,
                    IMessagingListener listener);

        /// <summary>  Query the topic by sending a SIF_Request message to all zones joined
        /// with the topic
        /// 
        /// </summary>
        /// <param name="query">A Query object that encapsulates the elements to query and
        /// the optional field restrictions placed on the results
        /// </param>
        /// <param name="queryOptions">Reserved for future use
        /// </param>
        void Query(Query query,
                    AdkQueryOptions queryOptions);

        /// <summary>  Query the topic by sending a SIF_Request message to all zones joined
        /// with the topic. This form of the <c>query</c> method also notifies a 
        /// MessagingListener of any SIF_Request activity that takes place.
        /// 
        /// </summary>
        /// <param name="query">A Query object that encapsulates the elements to query and
        /// the optional field restrictions placed on the results
        /// </param>
        /// <param name="queryOptions">Reserved for future use
        /// </param>
        /// <param name="listener">A MessagingListener that will be notified when the
        /// SIF_Request message is sent to the zone. Any other MessagingListeners 
        /// registered with the zone will also be called. 
        /// 
        /// @since Adk 1.5
        /// </param>
        void Query(Query query,
                    IMessagingListener listener,
                    AdkQueryOptions queryOptions);

        /// <summary>  Query the topic by sending a SIF_Request message to all zones joined
        /// with the topic
        /// 
        /// </summary>
        /// <param name="query">A Query object that encapsulates the elements to query and
        /// the optional field restrictions placed on the results
        /// </param>
        /// <param name="destinationId">The SourceId of the agent to which the SIF_Request
        /// message should be delivered. When null, the message is delivered to
        /// the object provider as defined by the SIF Zone
        /// </param>
        /// <param name="queryOptions">Reserved for future use
        /// </param>
        void Query(Query query,
                    String destinationId,
                    AdkQueryOptions queryOptions);

        /// <summary>  Query the topic by sending a SIF_Request message to all zones joined
        /// with the topic. This form of the <c>query</c> method also notifies 
        /// a MessagingListener of any SIF_Request messaging that takes place.
        /// 
        /// </summary>
        /// <param name="query">A Query object that encapsulates the elements to query and
        /// the optional field restrictions placed on the results.
        /// </param>
        /// <param name="listener">A MessagingListener that will be notified when the
        /// SIF_Request message is sent to the zone. Any other MessagingListeners 
        /// registered with the zone will also be called. 
        /// </param>
        /// <param name="destinationId">The SourceId of the agent to which the SIF_Request
        /// message should be delivered. When null, the message is delivered to
        /// the object provider as defined by the SIF Zone.
        /// </param>
        /// <param name="queryOptions">Reserved for future use.
        /// 
        /// @since Adk 1.5
        /// </param>
        void Query(Query query,
                    IMessagingListener listener,
                    String destinationId,
                    AdkQueryOptions queryOptions);

        /// <summary>  Purge all pending incoming and/or outgoing messages from this agent's
        /// queue. Only messages destined to and originating from the zones joined
        /// to this topic are affected. See also the Agent.purgeQueue and
        /// Zone.purgeQueue methods to purge the queues of all zones to which the
        /// agent is connected or a specific zone, respectively.
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
        void PurgeQueue(bool incoming,
                         bool outgoing);
    }
}
