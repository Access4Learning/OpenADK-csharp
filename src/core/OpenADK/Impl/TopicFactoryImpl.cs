//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using OpenADK.Util;
using System.Runtime.CompilerServices;
namespace OpenADK.Library.Impl
{
    /// <summary>  Default ZoneFactory implementation.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    internal class TopicFactoryImpl : ITopicFactory
    {
        ///<summary> Cache of Topics keyed by object type (e.g. "StudentPersonal")</summary>
        protected Dictionary<SifContext, Dictionary<IElementDef, ITopic>> fContexts =
              new Dictionary<SifContext, Dictionary<IElementDef, ITopic>>();


        /// <summary>  The agent that owns this TopicFactory. By associating factories with
        /// Agents (rather than having a static singleton) we can support multiple
        /// Agents per machine.
        /// </summary>
        protected internal Agent fAgent;

        /// <summary>  Constructs a TopicFactory</summary>
        /// <param name="agent">The Agent that owns this factory
        /// </param>
        public TopicFactoryImpl(Agent agent)
        {
            fAgent = agent;
        }

        /// <summary>  Gets a Topic instance for a SIF Data Object type.</summary>
        /// <param name="objectType">A SifDtd constant identifying a SIF Data Object type
        /// (e.g. <c>SifDtd.STUDENTPERSONAL</c>)
        /// </param>
        /// <returns> A new or cached Topic instance
        /// </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ITopic GetInstance(IElementDef objectType)
        {
            return GetInstance(objectType, SifContext.DEFAULT);
        }


        ///<summary>see com.OpenADK.Library.TopicFactory#getInstance(com.OpenADK.Library.ElementDef, com.OpenADK.Library.SIFContext)
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ITopic GetInstance(IElementDef objectType, SifContext context)
        {

            if (objectType == null)
            {

                throw new ArgumentNullException("The {objectType} parameter cannot be null");
            }

            if (context == null)
            {
                throw new ArgumentNullException("The {context} parameter cannot be null");
            }


            Dictionary<IElementDef, ITopic> map = GetTopicMap(context);
            ITopic topic = null;
            if(!map.TryGetValue( objectType, out topic ))
            {
                topic = new TopicImpl(objectType, context);
                map.Add(objectType, topic);
            }

            return topic;

        }


        ///<summary>Looks up an existing Topic withing the given SIFContext for the given object type. If the topic does not exist, null is returned </summary>
        ///<param name="objectType">The ElementDef identifying the top-level SIF Data Object</param>
        ///<param name="context">The SIF Context to look in </param>

        [MethodImpl(MethodImplOptions.Synchronized)]
        public ITopic LookupInstance(IElementDef objectType, SifContext context)
        {
            ITopic topic;
            if (GetTopicMap(context).TryGetValue(objectType, out topic))
            {
                return topic;
            }
            else
            {
                return null;
            }

        }


        public ICollection<SifContext> AllSupportedContexts
        {
            get
            {
                return fContexts.Keys;
            }
        }//end property AllSupportedContexts


        ///<summary>Gets all Topic instances in the factory cache for the specified context</summary>
        ///<param name="context"></param>
        public ICollection<ITopic> GetAllTopics(SifContext context)
        {
            return GetTopicMap(context).Values;
        }

        ///<summary>
        /// Gets the map of topics for the specified context
        /// </summary>
        ///<param name="context"></param>
        private Dictionary<IElementDef, ITopic> GetTopicMap(SifContext context)
        {
            Dictionary<IElementDef, ITopic> contextMap;
            if (!fContexts.TryGetValue(context, out contextMap))
            {
                contextMap = new Dictionary<IElementDef, ITopic>();
                fContexts[context] = contextMap;
            }
            return contextMap;
        }

        public ICollection<SifContext> GetAllSupportedContexts()
        {
            return fContexts.Keys;
        }

    }//end class
}//end namespace
