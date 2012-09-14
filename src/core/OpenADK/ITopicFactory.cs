//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
namespace OpenADK.Library
{
   /// <summary>  Creates Topic objects and provides access to the topics that have been
   /// created during this session of the agent.
   /// 
   /// Agents exchange data with SIF zones via Zone and Topic objects, which can
   /// be obtained with the ZoneFactory and TopicFactory classes. These factories
   /// are available by calling the <code>Agent.getZoneFactory</code> and
   /// <code>Agent.getTopicFactory</code> methods.
   /// 
   /// Topics are used to aggregate publish, subscribe, and query activity from
   /// multiple zones. An Agent may have only one Topic instance per SIF Data Object
   /// type (e.g. "StudentPersonal", "BusInfo", or "LibraryPatronStatus"), but a
   /// Topic can be joined with any number of Zones.
   /// 
   /// Topic instances are returned by calling the getInstance method, which
   /// returns the same Topic object for a given object type. Topic instances are
   /// cached by the factory. getAllTopics returns all topics in the cache. A Topic
   /// remains in the cache for as long as the agent is running.
   /// 
   /// </summary>
   /// <author>  Eric Petersen
   /// </author>
   /// <version>  Adk 1.0
   /// </version>
   public interface ITopicFactory
   {
      /// <summary>  Gets a Topic instance for a SIF object type
      /// 
      /// </summary>
      /// <param name="objectType">The ElementDef identifying a top-level SIF Data Object
      /// defined by the SifDtd class (e.g. <code>Adk.Dtd().STUDENTPERSONAL</code>)
      /// </param>
      /// <returns> A new or cached Topic instance
      /// </returns>
      /// <exception cref="System.ArgumentException"> thrown if the object type is not
      /// a root-level object for the version of SIF in use by the agent
      /// </exception>
      ITopic GetInstance(IElementDef objectType);

      ///<summary>
      ///Gets all SIF Contexts that have topics created for them
      /// 
      ///Returns a collection of SifContext instances that have topics joined to them
      ///</summary>
      ICollection<SifContext> AllSupportedContexts
      { get;}

      ITopic GetInstance(IElementDef objectType, SifContext context);

      ///<summary>
      /// Looks up an existing Topic withing the given SIFContext for the given object 
      /// type. If the topic does not exist, null is returned
      ///</summary>
      ///<param name="objectType">The ElementDef identifying the top-level SIF Data Object</param>
      ///<param name="context">The SIF Context to look in</param>
      ITopic LookupInstance(IElementDef objectType, SifContext context);

      ///<summary>
      /// Gets all Topic instances in the factory cache for the specified context
      /// 
      /// Returns an array of Topics
      ///</summary>
      ///<param name="context"><A SIFContext to return all topics for/param>
      ICollection<ITopic> GetAllTopics(SifContext context);

      ///<summary>
      ///Gets all SIF Contexts that have topics created for them
      /// 
      /// Returs a collection of SifContext instances that have topics joined to them
      ///</summary>
      ICollection<SifContext> GetAllSupportedContexts();

   }
}
