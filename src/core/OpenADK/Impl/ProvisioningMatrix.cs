//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenADK.Library.us;

namespace OpenADK.Library.Impl
{




    ///<summary>
    /// Provides a matrix of provisioning options for an agent, zone, or topic, divided by context. Any provisioning
    ///that is done is either done in the default context or in a specific context. This class
    ///enables all the provisioning registrations to be handled easily by the Zone, Agent, or Topic classes.

    ///To register a generic handler for any object type, the key that is used is SIFDTD.SIF_MESSAGE

    ///</summary>

    public class ProvisioningMatrix : IProvisioner
    {

        private List<ContextMatrix> fAllContexts = new List<ContextMatrix>();

        public void SetPublisher(IPublisher publisher)
        {
            SetPublisher(publisher, null, null );
        }


        ///<summary>
        /// Register a Publisher message handler to process SIF_Request messages for all object types
        ///</summary>
        ///<param name="publisher">An object that implements the Publisher interface to respond to SIF_Request queries received by the agent, where the SIF object type referenced by the request matches the specified objectType. This Publisher will be called whenever a SIF_Request is received on this zone and no other object in the message dispatching chain has processed the message.</param>
        ///<param name="objectType">An ElementDef constant from the SIFDTD class that identifies a SIF Data Object type. E.g. SIFDTD.STUDENTPERSONAL</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetPublisher(IPublisher publisher, IElementDef objectType )
        {
            SetPublisher(publisher, objectType, null);

        }


        ///<summary>
        /// Register a Publisher message handler to process SIF_Request messages for all object types
        ///</summary>
        ///<param name="publisher">An object that implements the Publisher interface to respond to SIF_Request queries received by the agent, where the SIF object type referenced by the request matches the specified objectType. This Publisher will be called whenever a SIF_Request is received on this zone and no other object in the message dispatching chain has processed the message.</param>
        ///<param name="objectType">An ElementDef constant from the SIFDTD class that identifies a SIF Data Object type. E.g. SIFDTD.STUDENTPERSONAL</param>
        ///<param name="options">Specify options about which SIF Contexts to join and whether SIF_Provide messagees will be sent when the agent is running in SIF 1.5r1 or lower </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetPublisher(IPublisher publisher, IElementDef objectType, PublishingOptions options)
        {

            if (options == null)
            {
                options = new PublishingOptions();
            }
            foreach (SifContext context in options.SupportedContexts)
            {
                GetOrCreateContextMatrix(context).setPublisher(publisher, objectType, options);
            }

        }




        ///<summary>
        /// <see cref="OpenADK.Library.IProvisioner.SetSubscriber(ISubscriber, IElementDef)"/>
        ///</summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetSubscriber(ISubscriber subscriber, IElementDef objectType)
        {

           SetSubscriber( subscriber, objectType, null );
        }



        ///<summary>
        /// <see cref="OpenADK.Library.IProvisioner.SetSubscriber(ISubscriber, IElementDef, SubscriptionOptions)"/>
        ///</summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetSubscriber(ISubscriber subscriber, IElementDef objectType, SubscriptionOptions options)
        {

            if (options == null)
            {
                options = new SubscriptionOptions();
            }
            foreach (SifContext context in options.SupportedContexts)
            {
                GetOrCreateContextMatrix(context).setSubscriber(subscriber, objectType, options);
            }
        }

        ///<summary>
        /// <see cref="OpenADK.Library.IProvisioner.SetQueryResults(IQueryResults)"/>
        ///</summary>
        public void SetQueryResults(IQueryResults queryResults)
        {
            SetQueryResults(queryResults, null);
        }

        ///<summary>
        /// <see cref="OpenADK.Library.IProvisioner.SetQueryResults(IQueryResults, IElementDef )"/>
        ///</summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetQueryResults(IQueryResults queryResults, IElementDef objectType)
        {
           SetQueryResults( queryResults, objectType, null );
        }



        ///<summary>
        /// <see cref="OpenADK.Library.IProvisioner.SetQueryResults(IQueryResults, IElementDef , QueryResultsOptions)"/>
        ///</summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetQueryResults(IQueryResults queryResults, IElementDef objectType, QueryResultsOptions options)
        {
            if (options == null)
            {
                options = new QueryResultsOptions();
            }
            foreach (SifContext context in options.SupportedContexts)
            {
                GetOrCreateContextMatrix(context).setQueryResults(queryResults, objectType, options);
            }
        }

  


        ///<summary>Looks up the publisher for the specified context and object type</summary>
        ///<param name="context"></param>
        ///<param name="objectType"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IPublisher LookupPublisher(SifContext context, IElementDef objectType)
        {

            ContextMatrix handler = LookupContextMatrix(context);
            if (handler != null)
            {
                return handler.lookupPublisher(objectType);
            }
            return null;
        }

        ///<summary>Returns all of the Publisher options for all contexts</summary>
        ///<param name="excludeWildcard"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<ProvisionedObject<IPublisher, PublishingOptions>> GetAllPublishers(Boolean excludeWildcard)
        {

            List<ProvisionedObject<IPublisher, PublishingOptions>> list = new List<ProvisionedObject<IPublisher, PublishingOptions>>();
            foreach (ContextMatrix context in fAllContexts)
            {
                GetAll(context.fPubs, list, excludeWildcard);
            }

            return list;
        }



        ///<summary>Returns all of the Subscriptions options for all contexts</summary>
        ///<param name="excludeWildcard"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IList<ProvisionedObject<ISubscriber, SubscriptionOptions>> GetAllSubscribers(Boolean excludeWildcard)
        {

            List<ProvisionedObject<ISubscriber, SubscriptionOptions>> list = new List<ProvisionedObject<ISubscriber, SubscriptionOptions>>();
            foreach (ContextMatrix context in fAllContexts)
            {
                GetAll(context.fSubs, list, excludeWildcard);
            }

            return list;
        }


        ///<summary>Returns all of the QueryResults options for all contexts</summary>
        ///<param name="excludeWildcard"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<ProvisionedObject<IQueryResults, QueryResultsOptions>> GetAllQueryResults(Boolean excludeWildcard)
        {

            List<ProvisionedObject<IQueryResults, QueryResultsOptions>> list = new List<ProvisionedObject<IQueryResults, QueryResultsOptions>>();
            foreach (ContextMatrix context in fAllContexts)
            {
                GetAll(context.fQueryResults, list, excludeWildcard);
            }

            return list;

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void GetAll<K, Y>(
              IDictionary<IElementDef, ProvisionedObject<K, Y>> items,
              IList<ProvisionedObject<K, Y>> list,
              Boolean excludeWildcard) where Y : ProvisioningOptions
        {

            foreach (KeyValuePair<IElementDef, ProvisionedObject<K, Y>> entry in items)
            {
                if (excludeWildcard && entry.Key == SifDtd.SIF_MESSAGE)
                {
                    continue;
                }
                list.Add(entry.Value);
            }


        }

        ///<summary>Looks up the subscriber for the specified context and object type</summary>
        ///<param name="context"></param>
        ///<param name="objectType"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IQueryResults LookupQueryResults(SifContext context, IElementDef objectType)
        {

            ContextMatrix handler = LookupContextMatrix(context);
            if (handler != null)
            {
                return handler.lookupQueryResults(objectType);
            }

            return null;
        }

        ///<summary>Looks up the subscriber for the specified context and object type</summary>
        ///<param name="context"></param>
        ///<param name="objectType"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ISubscriber LookupSubscriber(SifContext context, IElementDef objectType)
        {


            ContextMatrix handler = LookupContextMatrix(context);
            if (handler != null)
            {
                return handler.lookupSubscriber(objectType);
            }

            return null;
        }

        private ContextMatrix LookupContextMatrix(SifContext context)
        {
            foreach (ContextMatrix handler in fAllContexts)
            {
                if (handler.fContext.Equals(context))
                {
                    return handler;
                }
            }
            return null;
        }

        ///<summary>Looks up the ContextMatrix for the specified SifContext</summary>
        ///<param name="context"
        private ContextMatrix GetOrCreateContextMatrix(SifContext context)
        {
            ContextMatrix handler = LookupContextMatrix(context);
            if (handler == null)
            {
                handler = new ContextMatrix(context);
                fAllContexts.Add(handler);
            }
            return handler;
        }

        ///<summary>Contains references to all the message handlers supported for a specific SIF Context</summary>
        private class ContextMatrix //removed static
        {
            private ContextMatrix() { }//make default class constructor non-creatable, hidden

            /// <summary>
            /// The Subscribers registered with this context. The map consists of Handler
            ///  objects keyed by SIF data object names (e.g. "StudentPersonal"). If a
            ///  Subscriber is registered for all object types, it is keyed by the
            /// string "*".
            /// </summary>
            internal IDictionary<IElementDef, ProvisionedObject<ISubscriber, SubscriptionOptions>> fSubs =
                       new Dictionary<IElementDef, ProvisionedObject<ISubscriber, SubscriptionOptions>>();


            /// <summary>The Publishers registered with this context. The map consists of Handler
            /// objects keyed by SIF data object names (e.g. "StudentPersonal"). If a
            /// Publisher is registered for all object types, it is keyed by the string "*".
            /// </summary>
            internal IDictionary<IElementDef, ProvisionedObject<IPublisher, PublishingOptions>> fPubs =
                        new Dictionary<IElementDef, ProvisionedObject<IPublisher, PublishingOptions>>();




            ///<summary> The QueryResults objects registered in this context. The map is keyed by
            /// SIF data object names (e.g. "StudentPersonal"). If a QueryResults object
            /// is registered for all object types, it is keyed by the string "*".
            /// </summary>
            internal IDictionary<IElementDef, ProvisionedObject<IQueryResults, QueryResultsOptions>> fQueryResults =
                        new Dictionary<IElementDef, ProvisionedObject<IQueryResults, QueryResultsOptions>>();

            ///<summary>The context that this context handler covers </summary>
            internal SifContext fContext;



            public ContextMatrix(SifContext context)
            {
                fContext = context;
            }



            ///<summary>Sets the publisher for this context and object type</summary>
            ///<param name="publisher"></param>
            ///<param name="objectType"></param>
            ///<param name="options"></param>
            public void setPublisher(
                  IPublisher publisher,
                  IElementDef objectType,
                  PublishingOptions options)
            {
                SetHandler(fPubs, publisher, objectType, options);
            }

            ///<summary>Sets the subscriber for this context and object type</summary>
            ///<param name="subscriber"></param>
            ///<param name="objectType"></param>
            ///<param name="options"></param>
            public void setSubscriber(
                  ISubscriber subscriber,
                  IElementDef objectType,
                  SubscriptionOptions options)
            {
                SetHandler(fSubs, subscriber, objectType, options);
            }



            ///<summary>Sets the Query results handler for this context and object type</summary>
            ///<param name="qr"></param>
            ///<param name="objectType"></param>
            ///<param name="options"></param>
            public void setQueryResults(
                  IQueryResults qr,
                  IElementDef objectType,
                  QueryResultsOptions options)
            {
                SetHandler(fQueryResults, qr, objectType, options);
            }

            ///<summary>Looks up the publisher for the specified object type</summary>
            ///<param name="objectType"></param>
            public IPublisher lookupPublisher(IElementDef objectType)
            {
                return lookupHandler(objectType, fPubs);
            }

            ///<summary>Looks up the subscriber for the specified object type</summary>
            ///<param name="objectType"></param>
            public ISubscriber lookupSubscriber(IElementDef objectType)
            {
                return lookupHandler(objectType, fSubs);
            }


            ///<summary> Looks up the subscriber for the specified object type</summary>
            ///<param name="objectType"></param>
            public IQueryResults lookupQueryResults(IElementDef objectType)
            {
                return lookupHandler(objectType, fQueryResults);
            }

            ///<summary>Sets a handler of a specific type</summary>
            ///<param name="map"></param>
            ///<param name="handler"></param>
            ///<param name="objType"></param>
            ///<param name="options"></param>
            public void SetHandler<T, V>(IDictionary<IElementDef, ProvisionedObject<T, V>> map,
                           T handler,
                           IElementDef objType,
                           V options) where V : ProvisioningOptions
            {
                if (objType == null)
                {
                    objType = SifDtd.SIF_MESSAGE;
                }
                ProvisionedObject<T, V> item = new ProvisionedObject<T, V>(objType, handler, options);
                map.Add(objType, item);
            }

            ///<summary>Looks up a handler of a specific type</summary>
            ///<param name="def"></param>
            ///<param name="map"></param>
            public T lookupHandler<T, V>(IElementDef def, IDictionary<IElementDef, ProvisionedObject<T, V>> map)
               where V : ProvisioningOptions
            {
                ProvisionedObject<T, V> item;
                if (!map.TryGetValue(def, out item))
                {
                    // Look for a default handler 
                    if (!map.TryGetValue(SifDtd.SIF_MESSAGE, out item))
                    {
                        return default(T);
                    }
                }
                if (item != null)
                {
                    return item.Handler;
                }
                // Given a variable (map) of a parameterized type T, the statement map = null 
                // is only valid if T is a reference type and t = 0 will only work for numeric value types but not for structs.
                // The solution is to use the default keyword,
                // which will return null for reference types and zero for numeric 
                return default(T);
            }
        }//end class ContextMatrix

    }//end class ProvisioningMatrix
}//end namespace
