//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>The Subscriber message handler interface is implemented by classes that wish to 
    /// SIF_Event messages received from a zone.
    /// process SIF_Event messages received from a zone. Consult the <i>ADK Developer
    /// Guide</i> for more information about message handler interfaces.
    /// </summary>
    /// <remarks>
    /// 	<b>About SIF Events</b>
    /// <para>  
    /// An Event is a notification to subscribing agents that a data object has been
    /// added, changed, or deleted in the reporting application's database. In the
    /// Schools Interoperability Framework, any agent may report SIF_Event messages for 
    /// any object, even if that agent is not the authoritative publisher of the object.</para>
    ///  <para>
    /// Subscribing to SIF Events</para>
    ///  <para>
    /// Agents that wish to receive SIF_Event messages must register with the zone
    /// integration server as a Subscriber of one or more SIF Data Object types. To 
    /// subscribe to events for a specific object type, register a <i>Subscriber</i> 
    /// message handler with Zone or Topic instances. When registering Subscribers with zones, repeatedly call the 
    /// {@linkplain com.OpenADK.Library.Zone#setSubscriber(Subscriber, ElementDef, int)}
    /// method for each SIF Data Object type you wish to subscribe to. When registering
    /// message handlers with Topics, call the {@linkplain com.OpenADK.Library.Topic#setSubscriber(Subscriber, int)}
    /// method once to register with all zones bound to the topic (your <i>Subscriber</i>
    /// implementation will be called whenever a SIF_Event is received on any of the
    /// zones bound to that topic.) Be sure to specify the <c>ADKFlags.PROV_SUBSCRIBE</c> 
    /// flag as the last parameter to these methods if you wish the ADK to send a 
    /// SIF_Subscribe message to the zone when it connects.</para>
    /// 
    ///  	When a SIF_Event message is received, it is dispatched to the
    /// appropriate Subscriber message handler's <c>onEvent</c> method for 
    /// processing. Obtain a DataObjectInputStream from the Event instance
    /// by calling the Event parameter's {@linkplain com.OpenADK.Library.Event#getData()}
    /// method, then repeatedly call the stream's {@linkplain com.OpenADK.Library.DataObjectInputStream#readDataObject()}
    /// method to get the next SIFDataObject in the event payload.
    /// </remarks>
    /// <example>
    /// For example,
    ///  
    /// <code>
    /// public void OnEvent( Event evnt, IZone zone, IMessageInfo info )
    /// {
    ///    IDataObjectInputStream payload = evnt.Data;
    ///    while( payload.Available ) 
    ///    {
    ///        StudentPersonal sp = (StudentPersonal)payload.ReadDataObject();
    ///        switch( event.Action ) 
    ///	    {
    ///            case EventAction.Add:
    ///              // Add student...
    ///              break;
    ///            case EventActon.Change:
    ///              // Change student...
    ///              break;
    ///            case EventAction.Delete:
    ///              // Delete student...
    ///              break;
    ///        }
    ///     }
    /// }
    /// </code>
    /// <para>
    /// If <c>OnEvent</c> returns successfully, the ADK acknowledges the SIF_Event
    /// message with a success SIF_Ack. If a SifException or other AdkException is 
    /// thrown, the ADK acknowledges the SIF_Event with an error SIF_Ack using the error
    /// category, code, and description from the exception.</para> 
    /// </example> 
    /// <author>Eric Petersen</author>
    /// <since>ADK 1.0</since>
    public interface ISubscriber
    {
        /// <summary>  Respond to a SIF_Event received from a zone.</summary>
        /// <param name="evnt">The event data</param>
        /// <param name="zone">The zone from which this event originated</param>
        /// <param name="info">Information about the SIF_Event message</param>
        void OnEvent( Event evnt,
                      IZone zone,
                      IMessageInfo info );
    }
}
