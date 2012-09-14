//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using log4net;
using System.Runtime.CompilerServices;


namespace OpenADK.Library.Impl
{
    /// <summary>  Default implementation of the Topic interface.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    internal class TopicImpl : ITopic
    {

        /// <summary>  logging framework logging category for this topic</summary>
        public ILog log;

        /// <summary>  The Subscriber registered with this topic.</summary>
        internal ISubscriber fSub;

        /// <summary>  The Publisher registered with this topic.</summary>
        internal IPublisher fPub;


        /// <summary>  The QueryResults registered with this topic.</summary>
        internal IQueryResults fQueryResults;

        ///<summary> The SIF data object type associated with this topic
        ///</summary>  
        protected IElementDef fObjType;

        /// <summary>
        /// The options for publishing
        /// </summary>
        internal PublishingOptions fPubOpts;
       
        /// <summary>
        /// The options for subscriptions
        /// </summary>
        internal SubscriptionOptions fSubOpts;

        ///<summary>The SIF Context that this topic is joined to</summary>
        private SifContext fContext;

        /// <summary>  The Zones joined with this topic</summary>
        internal List<IZone> fZones = new List<IZone>();



        ///<summary>The options for QueryResults handling</summary>
        public QueryResultsOptions fQueryResultsOptions;

        internal TopicImpl(IElementDef objType, SifContext context)
        {
            fObjType = objType;
            fContext = context;
            log = LogManager.GetLogger(Agent.LOG_IDENTIFIER + ".Topic$" + objType.Name);
        }


        /// <summary>  Adds a zone to this topic</summary>
        /// <param name="zone">The Zone to join with this topic
        /// </param>
        /// <exception cref="OpenADK.Library.AdkException"> AdkException is thrown if the zone is already joined to a
        /// topic or if there is a SIF error during agent registration.
        /// </exception>
        public  void Join(IZone zone)
        {
            lock (this)
            {
                //  Check that zone is not already joined with this topic
                if (fZones.Contains(zone))
                {
                    AdkUtils._throw
                        (new SystemException
                             ("Zone already joined with topic \"" + fObjType + "\""),
                         ((ZoneImpl) zone).Log);
                }

                //  Check that topic has a Provider, Subscriber, or QueryResults object
                if (fSub == null && fPub == null && fQueryResults == null)
                {
                    AdkUtils._throw
                        (
                        new SystemException
                            (
                            "Agent has not registered a Subscriber, Publisher, or QueryResults object with this topic"),
                        ((ZoneImpl) zone).Log);
                }

                fZones.Add(zone);

                if (zone.Connected)
                {
                    ((ZoneImpl) zone).Provision();
                }
            }
        }



        /// <summary>  Gets the name of the SIF data object type associated with this topic</summary>
        /// <returns> The name of a root level SIF data object such as "StudentPersonal",
        /// "BusInfo", or "LibraryPatronStatus"
        /// </returns>
        public  string ObjectType
        {
            get { return fObjType.Name; }
        }

        /// <summary>
        /// see com.OpenADK.Library.Topic#getObjectDef()
        /// </summary>
        public IElementDef ObjectDef
        {
            get { return fObjType; }
        }

        /// <summary>
        /// see com.OpenADK.Library.Topic#getSIFContext()
        /// </summary>
        public SifContext SifContext
        {
            get { return fContext; }
        }

        /// <summary>  Gets the zones to which this topic is bound</summary>
        /// <returns> The zone that created this topic instance
        /// </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  IZone[] GetZones()
        {
            return fZones.ToArray();
        }

        /// <summary>  Checks that at least one zone is joined with the topic</summary>
        /// <exception cref="AdkException">  is thrown if no zones are joined with this topc
        /// </exception>
        private void _checkZones()
        {
            if (fZones.Count == 0)
            {
                throw new AdkException
                    ("No zones are joined with the \"" + fObjType + "\" topic", null);
            }
        }

        /// <summary>  Register a publisher of this topic.
        /// 
        /// Provisioning messages are sent as follows:
        /// 
        /// <ul>
        /// <li>
        /// If the agent is using Adk-managed provisioning, a <c>&lt;
        /// SIF_Provide&gt;</c> message is sent to the ZIS when the
        /// AdkFlags.PROV_PROVIDE flag is specified. When
        /// Adk-managed provisioning is disabled, no messages are sent to
        /// the zone.
        /// </li>
        /// <li>
        /// If Agent-managed provisioning is enabled, the ProvisioningOptions
        /// flags have no affect. The agent must explicitly call the
        /// sifProvide method to manually send those message to the zone.
        /// </li>
        /// <li>
        /// If ZIS-managed provisioning is enabled, no provisioning messages
        /// are sent by the agent regardless of the ProvisioningOptions
        /// used and the methods are called.
        /// </li>
        /// </ul>
        /// 
        /// 
        /// </summary>
        /// <param name="publisher">An object that implements the <c>Publisher</c>
        /// interface to publish change events and to evaluate SIF queries
        /// received by the agent
        /// </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetPublisher(IPublisher publisher)
        {
            SetPublisher( publisher, null );
        }


        /// <summary>  Register a publisher of this topic.
        /// 
        /// Provisioning messages are sent as follows:
        /// 
        /// <ul>
        /// <li>
        /// If the agent is using Adk-managed provisioning, a <c>&lt;
        /// SIF_Provide&gt;</c> message is sent to the ZIS when the
        /// AdkFlags.PROV_PROVIDE flag is specified. When
        /// Adk-managed provisioning is disabled, no messages are sent to
        /// the zone.
        /// </li>
        /// <li>
        /// If Agent-managed provisioning is enabled, the ProvisioningOptions
        /// flags have no affect. The agent must explicitly call the
        /// sifProvide method to manually send those message to the zone.
        /// </li>
        /// <li>
        /// If ZIS-managed provisioning is enabled, no provisioning messages
        /// are sent by the agent regardless of the ProvisioningOptions
        /// used and the methods are called.
        /// </li>
        /// </ul>
        /// 
        /// 
        /// </summary>
        /// <param name="publisher">An object that implements the <c>Publisher</c>
        /// interface to publish change events and to evaluate SIF queries
        /// received by the agent
        /// </param>
        /// <param name="provisioningOptions">Allows options to be set, such as whether to register
        /// as the default provider of the object type in the zone, and which SIF_Contexts are supported</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void SetPublisher(IPublisher publisher,
                                         PublishingOptions provisioningOptions)
        {
            assertProvisioningOptions(provisioningOptions);

            if (publisher == null)
            {
                fPub = null;
                fPubOpts = null;
            }
            else
            {
                fPub = publisher;
                if (provisioningOptions == null)
                {
                    provisioningOptions = new PublishingOptions();
                }
                fPubOpts = provisioningOptions;
            }
        }




        [MethodImpl(MethodImplOptions.Synchronized)]
        public  IPublisher GetPublisher()
        {

                return fPub;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetSubscriber(ISubscriber subscriber)
        {
            SetSubscriber( subscriber, null );
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void SetSubscriber(ISubscriber subscriber,
                                          SubscriptionOptions flags)
        {
            assertProvisioningOptions(flags);
            if (subscriber == null)
            {
                fSub = null;
                fSubOpts = null;
            }
            else
            {
                fSub = subscriber;
                if (flags == null)
                {
                    flags = new SubscriptionOptions();
                }
                fSubOpts = flags;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public  ISubscriber GetSubscriber()
        {
                return fSub;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetQueryResults(IQueryResults results )
        {
            SetQueryResults( results, null );
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetQueryResults(IQueryResults results, QueryResultsOptions flags)
        {
            assertProvisioningOptions(flags);
            if (results == null)
            {
                fQueryResults = null;
                fQueryResultsOptions = null;
            }
            else
            {
                fQueryResults = results;
                if (flags == null){
                    flags = new QueryResultsOptions();
                }
                fQueryResultsOptions = flags;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IQueryResults GetQueryResultsObject()
        {
            return fQueryResults;
        }


        private void assertProvisioningOptions(ProvisioningOptions opts)
        {
            if (opts != null && opts.SupportedContexts.Count > 1)
            {
                throw new ArgumentException("Cannot provision a single topic for more than one SIF Context.\r\n" +
                        "To use Topics with multiple SIF contexts, call TopicFactory.getInstance( ElementDef, SIFContext ).");
            }
        }


        /// <summary>  Publishes a change in topic data by sending a SIF_Event to all zones
        /// joined with this topic
        /// 
        /// This method is useful for communicating a single change event. If an
        /// agent changes data that spans several object types, it should consider
        /// using the BatchEvent class to publish changes as a group. BatchEvent
        /// aggregates changes in multiple SIF data objects, then sends a single
        /// SIF_Event message to each zone. This is much more efficient than calling
        /// the publishChange method of each Topic, which results in a single
        /// SIF_Event message being sent for each object type. Another alternative
        /// is to call the publishChange method of each Zone directly. That method
        /// accepts an Event object, which can describe changes in multiple data
        /// objects.
        /// 
        /// </summary>
        /// <param name="data">The data that has changed. The objects in this array must all
        /// be of the same SIF object type (e.g. all <c>StudentPersonal</c>
        /// objects if this topic encapsulates the "StudentPersonal" object type),
        /// and must all communicate the same state change (i.e. all added,
        /// all changed, or all deleted).
        /// 
        /// </param>
       [MethodImpl(MethodImplOptions.Synchronized)]
        public  void PublishEvent(Event data)
        {
                AdkMessagingException err = null;

                _checkZones();

                foreach (ZoneImpl z in fZones)
                {
                    try
                    {
                        z.fPrimitives.SifEvent(z, data, null, null);
                    }
                    catch (Exception th)
                    {
                        if (err == null)
                        {
                            err =
                                new AdkMessagingException
                                    ("Error publishing event to topic \"" + fObjType + "\"", z);
                        }

                        if (th is AdkException)
                        {
                            err.Add(th);
                        }
                        else
                        {
                            err.Add(new AdkMessagingException(th.ToString(), z));
                        }
                    }
                }

                if (err != null)
                {
                    AdkUtils._throw(err, Agent.GetLog());
                }
            
        }

        public  void Query(Query query)
        {
            Query(query, null, null, 0);
        }

        public  void Query(Query query,
                                  IMessagingListener listener)
        {
            Query(query, listener, null, 0);
        }

        public  void Query(Query query,
                                  AdkQueryOptions queryOptions)
        {
            Query(query, null, null, queryOptions);
        }

        public  void Query(Query query,
                                  IMessagingListener listener,
                                  AdkQueryOptions queryOptions)
        {
            Query(query, listener, null, queryOptions);
        }

        public  void Query(Query query,
                                  string destinationId,
                                  AdkQueryOptions queryOptions)
        {
            Query(query, null, destinationId, queryOptions);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Query(Query query,
                                  IMessagingListener listener,
                                  string destinationId,
                                  AdkQueryOptions queryOptions)
        {

            if (query == null)
            {
                AdkUtils._throw
                    (new ArgumentException("Query object cannot be null"), Agent.GetLog());
            }

            // Validate that the query object type and SIF Context are valid for this Topic
            if (query.ObjectType != fObjType)
            {
                AdkUtils._throw(new ArgumentException("Query object type: {" + query.ObjectTag +
                        "} does not match Topic object type: " + fObjType + "}"), log);
            }

            if (!query.SifContext.Equals(fContext))
            {
                AdkUtils._throw(new ArgumentException("Query SIF_Context: {" + query.SifContext +
                        "} does not match Topic SIF_Context: " + fContext + "}"), log);
            }

            _checkZones();

            AdkMessagingException err = null;

            //  Send the SIF_Request to each zone
            foreach (ZoneImpl z in fZones)
            {
                try
                {
                    z.Query(query, listener, destinationId, queryOptions);
                }
                catch (Exception th)
                {
                    if (err == null)
                    {
                        err =
                            new AdkMessagingException
                                ("Error querying topic \"" + fObjType + "\"", z);
                    }

                    if (th is AdkException)
                    {
                        err.Add(th);
                    }
                    else
                    {
                        err.Add(new AdkMessagingException(th.ToString(), z));
                    }
                }
            }

            if (err != null)
            {
                AdkUtils._throw(err, Agent.GetLog());
            }

        }

        public  void PurgeQueue(bool incoming,
                                       bool outgoing)
        {
        }

        public override string ToString()
        {
            return "Topic:" + fObjType.Name;
        }
    }
}

// Synchronized with Branch_Library-ADK-2.1.0.Version_3.TopicImpl.java
