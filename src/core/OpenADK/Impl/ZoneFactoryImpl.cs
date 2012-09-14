//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Specialized;

namespace OpenADK.Library.Impl
{
   /// <summary>  Default ZoneFactory implementation.
   /// 
   /// </summary>
   /// <author>  Eric Petersen
   /// </author>
   /// <version>  Adk 1.0
   /// </version>
   public class ZoneFactoryImpl : IZoneFactory
   {
      /// <summary>  Cache of Zones keyed by zoneId</summary>
      protected internal IDictionary fZones = new HybridDictionary();

      /// <summary>  Zone vector in the order created by the agent</summary>
      protected internal ArrayList fZoneList = new ArrayList();

      /// <summary>  The agent that owns this ZoneFactory. By associating factories with
      /// Agents (rather than having a static singleton) we can support multiple
      /// Agents per virtual machine.
      /// </summary>
      protected internal Agent fAgent;

      /// <summary>  Constructs a ZoneFactory for an Agent</summary>
      /// <param name="agent">The Agent that owns this factory
      /// </param>
      public ZoneFactoryImpl(Agent agent)
      {
         fAgent = agent;
      }

      public virtual IZone GetInstance(string zoneId,
                                        string zoneUrl)
      {
         lock (this)
         {
            return GetInstance(zoneId, zoneUrl, null);
         }
      }

      public virtual IZone GetInstance(string zoneId,
                                        string zoneUrl,
                                        AgentProperties props)
      {
         lock (this)
         {
            if (zoneId == null)
            {
               throw new ArgumentException("Zone ID cannot be null");
            }
            if (zoneId == null)
            {
               throw new ArgumentException("Zone URL cannot be null");
            }

            IZone zone = null;

            lock (fZones.SyncRoot)
            {
               //  Lookup zone by zoneId
               zone = (ZoneImpl)fZones[zoneId];

               if (zone == null)
               {
                  //  Not found so create new instance
                  zone = CreateZone(zoneId, zoneUrl,  props);
                  fZones[zoneId] = zone;
                  fZoneList.Add(zone);
               }
               else
               {
                  //  Reassign properties in case they're different
                  zone.Properties = props;
               }
            }

            return zone;
         }
      }

      /// <summary>  Gets a Zone previously created by a call to getInstance</summary>
      /// <param name="zoneId">The zone identifier
      /// </param>
      /// <returns> The Zone instance
      /// </returns>
      public virtual IZone GetZone(string zoneId)
      {
         if (zoneId == null)
         {
            throw new ArgumentException("Zone ID cannot be null");
         }

         return (IZone)fZones[zoneId];
      }

      public virtual IZone[] GetAllZones()
      {
         lock (this)
         {
            IZone[] arr = null;
            arr = new IZone[fZoneList.Count];
            fZoneList.CopyTo(arr);
            return arr;
         }
      }
      /// <summary>  Creates a new ZoneImpl object</summary>
      /// <param name="zoneId">The zone identifier
      /// </param>
      /// <param name="zoneUrl">The zone Url
      /// </param>
      ///<param name="props">The Agent Properties object
      /// </param>
      /// <returns> The ZoneImpl
      /// </returns>
      protected virtual IZone CreateZone(String zoneId, String zoneUrl, AgentProperties props)
      {
         return new ZoneImpl(zoneId, zoneUrl, fAgent, props);
      }

      /// <summary>  Remove a zone from the cache</summary>
      /// <param name="zone">The Zone
      /// </param>
      public virtual void RemoveZone(IZone zone)
      {
         lock (this)
         {
            if (zone == null)
            {
               throw new ArgumentException("Zone cannot be null");
            }
            if (zone.Connected)
            {
               throw new SystemException("Zone is connected");
            }

            fZones.Remove(zone.ZoneId);
            Object temp_object;
            Boolean temp_boolean;
            temp_object = zone;
            temp_boolean = fZoneList.Contains(temp_object);
            fZoneList.Remove(temp_object);
         }
      }
   }
}
