//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library
{

    ///<summary>
    /// a common interface implemented by the IZone interface
    /// classes. It provides common APIs for setting Subscribers, Publisher, and QueryResults
    /// handlers
    /// </summary>
    public interface IProvisioner
    {
        /// <summary>
        /// Register a Publisher message handler to process SIF_Request
        /// messages for all object types
        /// </summary>
        /// <param name="publisher">
        /// An object that implements the <code>Publisher</code>
        /// nterface to respond to SIF_Request queries received by the agent.
        /// This object will be called whenever a SIF_Request is received and 
        /// no other object in the message dispatching chain has
        /// processed the message.
        /// </param>
        void SetPublisher(IPublisher publisher);


        /// <summary>
        /// Register a Publisher message handler with this zone to process SIF_Requests
        /// for the specified object type. This method may be called repeatedly for
        /// each SIF Data Object type the agent will publish on this zone.
        /// </summary>
        /// <param name="publisher">
        /// An object that implements the <code>Publisher</code>
        /// interface to respond to SIF_Request queries received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This Publisher will be called whenever a
        /// SIF_Request is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        /// </param>
        /// <param name="objectType">An ElementDef constant from the SIFDTD class that 
        /// identifies a SIF Data Object type. E.g. SIFDTD.STUDENTPERSONAL
        /// </param>
        void SetPublisher(IPublisher publisher, IElementDef objectType);

        /// <summary>
        /// Register a Publisher message handler with this zone to process SIF_Requests
        /// for the specified object type. This method may be called repeatedly for
        /// each SIF Data Object type the agent will publish on this zone.
        /// </summary>
        /// <param name="publisher">
        /// An object that implements the <code>Publisher</code>
        /// interface to respond to SIF_Request queries received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This Publisher will be called whenever a
        /// SIF_Request is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        /// </param>
        /// <param name="objectType">An ElementDef constant from the SIFDTD class that 
        /// identifies a SIF Data Object type. E.g. SIFDTD.STUDENTPERSONAL
        /// </param>
        /// <param name="options"> 
        /// Specify options about which SIF Contexts to join and whether
        /// SIF_Provide messagees will be sent when the agent is running in SIF 1.5r1 or lower
        /// </param>
        void SetPublisher(IPublisher publisher, IElementDef objectType, PublishingOptions options);



        ///<summary>
        ///Register a Subscriber message handler with this zone to process SIF_Event
        /// messages for the specified object type. This method may be called 
        /// repeatedly for each SIF Data Object type the agent subscribes to on 
        /// this zone.
        /// </summary>
        /// <param name="subscriber">
        /// An object that implements the <code>Subscriber</code>
        /// interface to respond to SIF_Event notifications received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This Subscriber will be called whenever a
        /// SIF_Event is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        /// </param>
        /// <param name="objectType">
        /// A constant from the SIFDTD class that identifies a
        /// SIF Data Object type.
        ///</param>
        void SetSubscriber(ISubscriber subscriber, IElementDef objectType);


        ///<summary>
        ///Register a Subscriber message handler with this zone to process SIF_Event
        /// messages for the specified object type. This method may be called 
        /// repeatedly for each SIF Data Object type the agent subscribes to on 
        /// this zone.
        /// </summary>
        /// <param name="subscriber">
        /// An object that implements the <code>Subscriber</code>
        /// interface to respond to SIF_Event notifications received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This Subscriber will be called whenever a
        /// SIF_Event is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        /// </param>
        /// <param name="objectType">
        /// A constant from the SIFDTD class that identifies a
        /// SIF Data Object type.
        ///</param>
        ///<param name="options">
        /// Specify which contexts to join and whether SIF_Subscribe
        /// messages will be sent when the agent is running in SIF 1.5r1 or lower
        ///</param>
        void SetSubscriber(ISubscriber subscriber, IElementDef objectType, SubscriptionOptions options);

        ///<summary>
        /// Register a QueryResults message handler with this zone to process
        /// SIF_Response messages for all object types.
        ///</summary>
        ///<param name="queryResults">
        /// An object that implements the <code>QueryResults</code>
        /// interface to respond to SIF_Response query results received by the
        /// agent. This object will be called whenever a SIF_Response is received
        /// and no other object in the message dispatching chain has processed the message.
        ///</param>
        void SetQueryResults(IQueryResults queryResults);

        /// <summary>
        /// Register a QueryResults object with this zone for the specified SIF object type.
        /// </summary>
        /// <param name="queryResults">
        /// An object that implements the <code>QueryResults</code>
        /// interface to respond to SIF_Response query results received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This QueryResults object will be called whenever
        /// a SIF_Response is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        ///</param>
        ///<param name="objectType">
        /// A constant from the SIFDTD class that identifies a
        /// SIF Data Object type.
        /// </param>
        void SetQueryResults(IQueryResults queryResults, IElementDef objectType);

        /// <summary>
        /// Register a QueryResults object with this zone for the specified SIF object type.
        /// </summary>
        /// <param name="queryResults">
        /// An object that implements the <code>QueryResults</code>
        /// interface to respond to SIF_Response query results received by the agent,
        /// where the SIF object type referenced by the request matches the
        /// specified objectType. This QueryResults object will be called whenever
        /// a SIF_Response is received on this zone and no other object in the
        /// message dispatching chain has processed the message.
        ///</param>
        ///<param name="objectType">
        /// A constant from the SIFDTD class that identifies a
        /// SIF Data Object type.
        /// </param>
        /// <param name="options">
        /// Specify which contexts to join
        /// </param>
        void SetQueryResults(IQueryResults queryResults, IElementDef objectType, QueryResultsOptions options);
    } //end class
} //end namespace
