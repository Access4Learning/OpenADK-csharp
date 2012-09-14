//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  Creates Zone objects and provides access to the Zones created by an agent.
    /// 
    /// Agents exchange data with SIF zones via Zone and Topic objects, which can
    /// be obtained with the ZoneFactory and TopicFactory classes. These factories
    /// are available by calling the Agent.getZoneFactory and Agent.getTopicFactory
    /// methods.
    /// 
    /// An Agent may have one Zone object per SIF zone to which it is connected.
    /// 
    /// Zone instances are returned by calling the getInstance method. When no
    /// properties are specified for a zone, the agent's defaults are used. Default
    /// properties are returned by the Agent.getProperties method. For a explanation
    /// of the various properties that affect zones, refer to the AgentProperties
    /// class description.
    /// 
    /// The getInstance method returns the same Zone object for a given zone
    /// identifier for as long as it remains in the factory's cache. getAllZones
    /// returns all zones in the cache. A Zone remains in the cache until its
    /// disconnect method is called, at which point it is removed and can no longer
    /// be used by agent. (Calling the Zone.connect method on a disconnected zone
    /// throws an exception.) To reconnect to a zone after calling its disconnect
    /// method, obtain a fresh Zone instance from the ZoneFactory.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public interface IZoneFactory
    {
        /// <summary>  Gets a Zone instance with default properties</summary>
        /// <param name="zoneId">The zone identifier
        /// </param>
        /// <param name="zoneUrl">The URL of the Zone Integration Server that manages this
        /// zone. The format of the URL is specific to the ZIS product and the
        /// transport protocol used to communicate with the ZIS
        /// </param>
        /// <returns> A new or cached Zone instance
        /// </returns>
        IZone GetInstance( string zoneId,
                           string zoneUrl );

        /// <summary>  Gets a Zone instance with custom properties</summary>
        /// <param name="zoneId">The zone identifier
        /// </param>
        /// <param name="zoneUrl">The URL of the Zone Integration Server that manages this
        /// zone. The format of the URL is specific to the ZIS product and the
        /// transport protocol used to communicate with the ZIS
        /// </param>
        /// <param name="props">Properties for the factory to use when creating a new Zone
        /// object
        /// </param>
        /// <returns> A new or cached Zone instance
        /// </returns>
        IZone GetInstance( string zoneId,
                           string zoneUrl,
                           AgentProperties props );

        /// <summary>  Gets a Zone previously created by a call to getInstance</summary>
        /// <param name="zoneId">The zone identifier
        /// </param>
        /// <returns> The Zone instance
        /// </returns>
        IZone GetZone( string zoneId );

        /// <summary>  Gets all zones in the factory cache</summary>
        /// <returns> An array of Zones
        /// </returns>
        IZone [] GetAllZones();

        /// <summary>  Remove a zone from the ZoneFactory
        /// This method can only be called on a zone that is in the disconnected state.
        /// </summary>
        /// <param name="zone">The zone to remove
        /// </param>
        void RemoveZone( IZone zone );
    }
}
