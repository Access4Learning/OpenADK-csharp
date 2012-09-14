//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Impl;

namespace OpenADK.Library
{
   /// <summary>Encapsulates a SIF Event</summary>
   /// <remarks>
   /// <para>
   /// An Event is a notification to subscribing agents that a data object has been
   /// added, changed, or deleted in the reporting application's database. In the
   /// Schools Interoperability Framework, any agent may report SIF_Event messages for 
   /// any object, even if that agent is not the authoritative publisher of the object.
   /// </para>
   /// <para>Reporting SIF Events</para>
   /// <para>
   /// To report a SIF_Event to a zone, construct an Event instance and call the
   /// <see cref="OpenADK.Library.IZone.ReportEvent"/> method. Supply an array
   ///	of SifDataObject instances and an action code to the Event constructor. You may
   ///	also call alternative forms of the <c>reportEvent</c> method that accept 
   ///	SIFDataObjects and an action code as parameters. Note Events cannot be reported 
   ///	to a Topic because the ADK cannot determine which of its zones should receive 
   ///	the event. If you're using Topics, you must report events to the specific Zone 
   ///	or Zones to which the data applies.
   /// </para>
   /// <para>Subscribing to SIF Events</para>
   /// To subscribe to SIF_Events for a specific object type, register an <see cref="OpenADK.Library.ISubscriber"/>
   /// instance with IZone or ITopic instances. Call the <c>setSubscriber</c>
   ///	method repeatedly for each object type you wish to subscribe to. When the ADK
   ///	connects to a zone, it sends SIF_Subscribe messages for each object type. When a 
   ///	SIF_Event message is received by the ADK, it is dispatched to your 
   ///	<see cref="OpenADK.Library.ISubscriber.OnEvent"/>
   ///	method for processing. Refer to that method for more information.
   ///	</remarks>
   /// <author>Eric Petersen</author>
   /// <since>Adk 1.0</since>
   public class Event
   {
      /// <summary>  Gets the SIF Data Objects in the Event payload
      /// 
      /// </summary>
      /// <value>An input stream from which the agent can read the individual
      /// SifDataObjects contained in the event
      /// </value>
      public virtual IDataObjectInputStream Data
      {
         get { return fData; }
      }

      /// <summary>Identifies the type of SIF Data Object contained in the Event payload</summary>
      /// <value> An ElementDef constant from the SifDtd class that identifies the
      /// type of SIF Data Object contained in the event. All objects in an event
      /// must be of the same class type.
      /// </value>
      public virtual IElementDef ObjectType
      {
         get { return fObjType; }
      }

      /// <summary>Gets the action code identifying how all of the data objects in the
      /// Event payload have changed
      /// </summary>
      /// <value> One of the enum values defined by the <see cref="EventAction"/> Enum.
      /// If <see cref="EventAction.Undefined"/>
      /// is returned, call getActionString to inspect the actual action string
      /// received from the SIF Agent that generated this event data.
      /// 
      /// </value>
      /// <remarks>
      /// <seealso cref="EventAction">
      /// </seealso>
      /// </remarks>
      public virtual EventAction Action
      {
         get { return fEventAction; }
      }

      /// <summary>  Gets the action string that was received from the SIF Agent that
      /// generated this event data.
      /// 
      /// </summary>
      /// <returns> An action string (e.g. "Add", "Change", or "Delete")
      /// 
      /// </returns>
      /// <seealso cref="EventAction">
      /// </seealso>
      public virtual string ActionString
      {
         get { return fAction; }
      }

      /// <summary>Used by the Adk to track the zone from which this event originated.
      /// Calling this method for Events created by an agent has no effect.
      /// </summary>
      public virtual IZone Zone
      {
         get { return fZone; }

         set { fZone = value; }
      }


      /// <summary>  The data that has changed as described by the action</summary>
      private IDataObjectInputStream fData;

      /// <summary>  Identifies the type of SIF Data Object contained in the event</summary>
      private IElementDef fObjType;


      private EventAction fEventAction;

      private string fAction;

      /// <summary>  The zone from which this event originated</summary>
      private IZone fZone;

      /**
   * The SIF Contexts to which this event applies 
   */
      private SifContext[] fContexts = new SifContext[] { SifContext.DEFAULT };

      /// <summary>  Constructs an Event object to encapsulate in inbound SIF Event.
      /// This form of the contructor is called internally by the ADK when it receives
      /// a SIF Event Message and is not intended to be used by ADK developers.
      /// 
      /// </summary>
      /// <param name="data">An IDataObjectInputStream that returns SifDataObjects, all of
      /// which must be of the same class type
      /// </param>
      /// <param name="action">One of the EventAction enum values</c>
      /// </param>
      /// <param name="objectType">An ElementDef constant from the <see cref="OpenADK.Library.SifDtd"/> class that
      /// identifies the type of SIF Data Object contained in the event
      /// </param>
      public Event(IDataObjectInputStream data,
                    EventAction action,
                    IElementDef objectType)
      {
         fData = data;
         fObjType = objectType;
         SetAction(action);
      }

      /// <summary>  Constructs an Event object to encapsulate
      /// an inbound SIF Event. This form of the constructor is called internally by the
      /// ADK when it receives a SIF Event message and is not intended to be used by ADK developers.
      /// </summary>
      /// <param name="data">An IDataObjectInputStream that returns SifDataObjects, all of
      /// which must be of the same class type
      /// </param>
      /// <param name="action">Describes how the data has changed
      /// </param>
      /// <param name="objectType">An ElementDef constant from the SifDtd class that
      /// identifies the type of SIF Data Object contained in the event
      /// </param>
      public Event(IDataObjectInputStream data,
                    string action,
                    IElementDef objectType)
      {
         fData = data;
         fObjType = objectType;
         SetAction(action);
      }

      /// <summary>  Constructs an Event object to encapsulate
      /// an outbound SIF Event, which describes
      /// one or more SifDataObjects that have been added, changed,
      /// or deleted by the local application. This form of the constructor is called
      /// by agents when reporting a SIF_Event message to a zone
      /// </summary>
      /// <param name="data">An array of SifDataObjects, all of which must be of the same class type
      /// </param>
      /// <param name="action">Describes how the data has changed.</param>
      /// <remarks>
      /// <seealso cref="OpenADK.Library.IZone.ReportEvent"/>
      /// </remarks>
      public Event(SifDataObject[] data,
                    EventAction action)
      {
         try
         {
            fData = DataObjectInputStreamImpl.newInstance();
            if (data != null)
            {
               ((DataObjectInputStreamImpl)fData).Data = data;
            }
         }
         catch (AdkException adke)
         {
            throw new SystemException(adke.ToString());
         }

         fObjType = data != null && data.Length > 0 && data[0] != null
                        ? data[0].ElementDef
                        : null;
         SetAction(action);
      }

      /// <summary>  Constructs an Event object to encapsulate
      /// an outbound SIF Event, which describes
      /// one or more SifDataObjects that have been added, changed,
      /// or deleted by the local application. This form of the constructor is called
      /// by agents when reporting a SIF_Event message to a zone
      /// </summary>
      /// <param name="data">An array of SIFDataObjects, all of which must be of the same class
      /// </param>
      /// <param name="action">Describes how the data has changed: in SIF, this string
      /// must be "Add", "Change", or "Delete"
      /// </param>
      /// <remarks>
      /// <seealso cref="OpenADK.Library.IZone.ReportEvent"/>
      /// </remarks>
      public Event(SifDataObject[] data,
                    string action)
      {
         try
         {
            fData = DataObjectInputStreamImpl.newInstance();
            if (data != null)
            {
               ((DataObjectInputStreamImpl)fData).Data = data;
            }
         }
         catch (AdkException adke)
         {
            throw new SystemException(adke.ToString());
         }

         fObjType = data != null && data.Length > 0 && data[0] != null
                        ? data[0].ElementDef
                        : null;

         SetAction(action);
      }

      ///<summary>
      /// Gets/Sets the SIF Contexts that this event applies to. If the context has not been set,
      /// it defaults to the SIF Default context
      ///return An array of SIF contexts to which this event applies
      /// <see cref="SiFContext.DEFAULT"></see>
      ///</summary>
      public SifContext[] Contexts
      {
         get
         {
            return fContexts;
         }
         set
         {
            if (value == null || value.Length == 0)
            {
               throw new ArgumentException(
                     "Event must apply to one or more SIF Contexsts. SIFContext cannot be null");
            }
            this.fContexts = value;
         }
      }

      #region Private Members

      private void SetAction(string action)
      {
         try
         {
            SetAction((EventAction)Enum.Parse(typeof(EventAction), action, true));
         }
         catch
         {
            fEventAction = EventAction.Undefined;
            fAction = action;
         }
      }

      private void SetAction(EventAction action)
      {
         if (action == EventAction.Undefined)
         {
            throw new ArgumentException("Event action code " + action + " is not valid");
         }
         fEventAction = action;
         fAction = action.ToString();
      }

      #endregion
   }
}

// Synchronized with Event.java Branch Library-ADK-1.5.0 Version 3
