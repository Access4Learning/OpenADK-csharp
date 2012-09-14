//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;
using OpenADK.Library.Impl;
using OpenADK.Util;

namespace OpenADK.Library
{
   /// <summary>  Properties describing the operational settings of the agent or a zone.</summary>
   /// <remarks>
   /// <para>
   /// The constructor will initialize each property to its default value if the
   /// property is defined in the application configuration file (more precisely, any
   /// appSetting property that begins with the string "adk.")  Otherwise the Adk's
   /// factory defaults are used. Because of this it is possible to adjust an
   /// agent's properties at runtime by modifying the application config file.
   /// </para>
   /// <para> 
   /// The properties below are currently defined. Most properties can be set in both the
   /// agent or zone scope so that configurable options can be defined on a zone-by-zone
   /// basis. Properties set in the agent scope are global to the agent and inherited
   /// by all zones that have not explicitly overridden the property. To set a
   /// property in the agent scope, call <c>Agent.getProperties</c> method to
   /// obtain the agent's properties object, then call its setter methods. To set a
   /// property in the zone scope, call a zone's <c>Zone.getProperties</c>
   /// method to obtain the zone's AgentProperties object, then call its setter
   /// methods.
   /// </para>
   /// <list type="table">
   /// <listheader><term>Property</term><description>Description</description></listheader>
   /// <item>
   /// <term>adk.messaging.mode</term>
   /// <description>Indicates whether the agent should operate in Push or Pull mode.
   /// Possible values include <see cref="AgentMessagingMode"/>.<see cref="AgentMessagingMode.Push"/> and
   /// <see cref="AgentMessagingMode"/>.<see cref="AgentMessagingMode.Pull"/>. When Push mode is selected,
   /// the agent establishes a local socket to listen for incoming
   /// messages sent by the ZIS. (The Transport object associated with
   /// a zone dictates the type and parameters of the local socket.
   /// Transport properties are set independently in Transport objects.)
   /// When Pull mode is selected, the agent periodically polls the ZIS
   /// to check its queue for new messages. The default polling frequency
   /// can adjusted with the <see cref="PullFrequency"/>
   /// property. Per SIF specification, an agent must use the same
   /// messaging mode from the time it is registered on the ZIS until
   /// it is unregistered. Default Value: <see cref="AgentMessagingMode"/>.<see cref="AgentMessagingMode.Pull"/>
   /// </description>
   /// </item>
   /// <item>
   ///	<term>adk.messaging.transport</term>
   ///	<description>The transport protocol to use (e.g. "http", "https"). Default Value: http</description>
   /// </item>
   /// <item>
   ///	<term>adk.messaging.pullFrequency</term>
   ///	<description>For pull agents: the polling frequency in milliseconds. Default Value: 30 seconds</description>
   /// </item>
   /// <item>
   ///	<term>adk.messaging.pullDelayOnError</term>
   ///	<description> For pull agents, gets the amount of time in milliseconds the agent
   /// will delay when it encounters a transport error or disconnected
   /// zone when attempting to pull the next message from its queue. Default Value: 3 minutes</description>
   /// </item>
   /// <item>
   ///	<term>adk.messaging.maxBufferSize</term>
   ///	<description>The maximum size of messages (in bytes) that can be processed by
   /// this agent. This setting is used whenever the SIF_MaxBufferSize
   /// element is required in a SIF_Register or SIF_Request message.
   /// Default Value: 393216</description>
   /// </item>
   /// <item>
   ///	<term>adk.messaging.effectiveBufferSize</term>
   ///	<description>The maximum size of messages (in bytes) that will be processed
   /// in-memory before the Adk off-loads the message to the local file
   /// system for processing. This setting is used internally to
   /// influence memory management but is not used in any SIF messages. Default Value: 32000</description>
   /// </item>
   /// <item>
   ///	<term>adk.keepMessageContent</term>
   ///	<description>Determines if the Adk will retain SIF_Message XML content after
   /// it processes a message. When enabled, the SifMessageInfo.getMessage
   /// method returns a non-null value (otherwise it returns null). A
   /// SifMessageInfo object is passed to all message handlers such as
   /// <see cref="ISubscriber.OnEvent"/>, <see cref="IPublisher.OnRequest"/>, and
   /// <see cref="IQueryResults.OnQueryResults"/>. Default Value: <c>False</c> </description>
   /// </item>
   /// <item>
    ///	<term>adk.messaging.strictTypeParsing</term>
   ///	<description>When set to <c>true</c>, the agent will throw an exception while
   ///	parsing a SIF_Message if it cannot parse the value of a strongly-typed
   ///	element or attribute. When <c>false</c>,
    ///	the agent ignores the values it cannot parse. Default Value: <c>False</c> 
   /// </description>
   /// </item>
    /// <item>
    ///	<term>adk.messaging.strictVersioning</term>
    ///	<description>When set to <c>True</c>, the agent will only parse messages
    /// received from agents that are using the same version of SIF as
    /// this agent. An agent declares the version of SIF it will use
    /// when initializing the Adk's class framework. When <c>False</c>,
    /// the agent parses messages from all versions of SIF supported by
    /// the Adk's SIF Data Objects (Sdo) library. Default Value: <c>False</c></description>
    /// </item>
   /// <item>
   ///	<term>adk.messaging.oneObjectPerResponse</term>
   ///	<description>Instructs the Adk to return one SIF Data Object per SIF_Response
   /// packet. When disabled (the default), the Adk fits as many SIF
   /// Data Objects in a single response packet as allowed by the
   /// requestor's SIF_MaxBufferSize. Default Value: <c>False</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.messaging.processEventsFromSelf</term>
   ///	<description>Instructs the Adk to process SIF_Event messages that were reported
   /// by this agent (i.e. the SourceId of the SIF_Event matches the
   /// SourceId of this agent). By default, such events are ignored by
   /// the class framework and automatically acknowledged as successfully
   /// received. Default Value: <c>False</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.messaging.noRequestIndividualElements</term>
   ///	<description>A compatibility option that instructs the Adk to render SIF_Request
   /// messages without SIF_Element field restrictions. When this property
   /// is enabled, the class framework will not include SIF_Element elements
   /// in SIF_Request messages even if you have called the
   /// Query.addFieldRestriction method. This property can be used when
   /// requesting data from agents that do not work well if SIF_Elements
   /// are present in the SIF_Request. Default Value: <c>False</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.messaging.disableDispatcher</term>
   ///	<description>Advanced - Disables the Adk's message fDispatcher, causing all
   /// messages received by the agent to be ignored and disposed of
   /// immediately without dispatching to the agent's message handlers.
   /// This property is only useful in rare situations when an agent
   /// sends messages to the zone integration server but does not want
   /// to process any messages in its queue. Default Value: <c>False</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.compatibility.useZoneStatusSystemControl</term>
   ///	<description>SIF 1.5 Experimental - When this property is <c>true</c>
   /// and the Adk is initialized for SIF 1.5 or later, it will issue a
   /// synchronous SIF_SystemControl/SIF_GetZoneStatus message when the 
   /// <c>Zone.GetZoneStatus</c> method is called instead of
   /// using asynchronous SIF_Request messages. This mechanism of obtaining 
   /// SIF_ZoneStatus is preferred over the traditional SIF_Request method, 
   /// but is not officially supported in SIF as of 1.5. It is considered 
   /// experimental. When this property is <c>false</c> (the default), 
   /// the Adk issues SIF_Requests to obtain the SIF_ZoneStatus object. Default Value: <c>False</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.provisioning.zisVersion</term>
   ///	<description>Defines the latest version of the SIF Specification supported by
   /// the Zone Integration Server to which the agent is connecting. This
   /// property defaults to the latest version of the SIF Specification
   /// supported by the Adk. Currently, it affects how SIF_Register
   /// messages are sent: if the ZIS supports SIF 1.1 or later, the Adk
   /// will send a SIF_Register with multiple SIF_Version elements, one
   /// for each version of the SIF Specification supported by the Adk.
   /// The first SIF_Version will be the version passed to the <c>Adk.initialize</c>
   /// method. If the ZIS does not support SIF 1.1 or later, the Adk will send
   /// a SIF_Register with a single SIF_Version element where the value
   /// is equal to the version passed to the <c>Adk.initialize</c> method. Default Value: <see cref="SifVersion"/>.<see cref="SifVersion.LATEST"/></description>
   /// </item>
   /// <item>
   ///	<term>adk.provisioning.overrideSifVersions</term>
   ///	<description>Overrides the way the Adk prepares SIF_Register/SIF_Version elements
   /// to include only the list of versions specified in the comma-delimited
   /// list. When connecting to a SIF 1.1 or later zone integration server,
   /// the class framework will include a SIF_Version element for the
   /// version of SIF used to initialize the Adk, followed by one additional
   /// SIF_Version element for each version specified by this property. Default value: null</description>
   /// </item>
   /// <item>
   ///	<term>adk.provisioning.batch</term>
   ///	<description> Controls how the Adk prepares SIF_Provide and SIF_Subscribe
   /// provisioning messages. When this property is set to false (the
   /// default), the Adk sends an individual SIF_Provide and SIF_Subscribe
   /// message to the zone for each SIF Data Object. If any of the
   /// messages fail with an Access Control error (Category 4), the
   /// error is recorded as a warning and subsequently returned by the
   /// <c>Zone.getConnectWarnings</c> method. All other SIF Errors
   /// result in an exception thrown by the <c>Zone.connect</c>
   /// method. When this property is set to true, SIF Data Objects are
   /// batched into a single message that will be accepted or rejected
   /// as a group by the zone integration server. Default Value: <c>False</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.provisioning.mode</term>
   ///	<description>The provisioning mode in effect for this zone. Possible values:
   /// "adk" for Adk-managed provisioning; "zis" for ZIS-managed
   /// provisioning; or "agent" for Agent-managed provisioning. Refer
   /// to the Adk Developer Guide for an explanation of the three
   /// provisioning modes. Default value: <c>"adk"</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.provisioning.ignoreErrors</term>
   ///	<description>A compatibility option that determines if the Adk will throw
   /// exceptions when a SIF_Error is received by the ZIS during
   /// Adk-managed provisioning. This property should be enabled when
   /// connecting to the OpenSIF ZIS (0.9.x) because that it incorrectly
   /// treats attempt to re-provide or re-subscribe as errors instead of
   /// successful statuses. Default Value: <c>False</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.queue.disable</term>
   ///	<description>Disables the Agent Local Queue. Default Value: <c>False</c></description>
   /// </item>
   /// <item>
   ///	<term>adk.security.authenticationLevel</term>
   ///	<description>Authentication level to use for all communications with this zone.
   /// This value is specified in the header of all SIF messages to
   /// direct the Zone Integration Server to protect sending of the
   /// agent's messages to another agent with a lower authentication level. Default Value: 0</description>
   /// </item>
   /// <item>
   ///	<term>adk.security.encryptionLevel</term>
   ///	<description>Encryption level to use for all communications with this zone.
   /// This value is specified in the header of all SIF messages to
   /// direct the Zone Integration Server to protect sending of the
   /// agent's messages to another agent with a lower encryption level. Default Value: 0</description>
   /// </item>
   /// <item>
   ///	<term>adk.encryption.algorithm</term>
   ///	<description>The default algorithm used for writing passwords</description>
   /// </item>
   /// <item>
   ///	<term>adk.encryption.key</term>
   ///	<description>The name of the default key used for writing passwords</description>
   /// </item>
   /// <item>
   ///	<term>adk.encryption.keys.[KeyName]</term>
   ///	<description>The actual key to use for encryption or decryption where “keyname” matches the @KeyName attribute of the AuthenticationInfoPassword object</description>
   /// </item>
   /// </list>
   /// 
   /// </remarks>
   [Serializable]
   public class AgentProperties : AdkProperties
   {
      ///<summary> Gets/Sets the URL to the agent's icon. The icon must meet the requirements for the SIF_Icon
      /// element. If this property is set, the agent will send a &lt;SIF_Icon&gt; element during
      /// agent registration
      ///</summary>

      public string AgentIconUrl
      {
         get
         {
            return this.GetProperty(PROP_PROVISIONING_ICON, null);
         }

         set
         {
            this.SetProperty(PROP_PROVISIONING_ICON, value);
         }
      }

      ///<summary>  The name of the vendor who developed this SIF agent. If set, this information will
      ///sent to the ZIS during agent registration.
      /// 
      /// return the name of the vendor that developed this agent
      ///</summary>
      public String AgentVendor
      {
         get
         {
            return this.GetProperty(PROP_PROVISIONING_AGENT_VENDOR, null);
         }
         set
         {
            SetProperty(PROP_PROVISIONING_AGENT_VENDOR, value);
         }
      }

      public String AgentVersion
      {
         get
         {
            return GetProperty(PROP_PROVISIONING_AGENT_VERSION, null);
         }
         set
         {
            SetProperty(PROP_PROVISIONING_AGENT_VERSION, value);
         }
      }

      ///<summary>
      ///The name of the application that this agent services.
      ///This information is available in SIF_Register and SIFZoneStatus
      ///
      ///returns the name of the application serviced by this agent
      ///</summary>
      public String ApplicationName
      {
         get
         {
            return GetProperty(PROP_PROVISIONING_APP_NAME, null);
         }
         set
         {
            SetProperty(PROP_PROVISIONING_APP_NAME, value);
         }
      }

      ///<summary>
      /// The name of the vendor who developed the application serviced by this agent.
      /// If set, this information will sent to the ZIS during agent registration.
      /// 
      /// return the name of the vendor that developed the application serviced by this agent
      ///</summary>
      public String ApplicationVendor
      {
         get
         {
            return GetProperty(PROP_PROVISIONING_APP_VENDOR, null);
         }
         set
         {
            SetProperty(PROP_PROVISIONING_APP_VENDOR, value);
         }
      }

      ///<summary>
      /// The version of the application serviced by this agent, e.g. "2.0.0.11"
      /// 
      /// return the version of the application serviced by this agent
      ///</summary>
      public String ApplicationVersion
      {
         get
         {
            return GetProperty(PROP_PROVISIONING_APP_VERSION, null);
         }
         set
         {
            SetProperty(PROP_PROVISIONING_APP_VERSION, value);
         }
      }




      /// <summary>  Gets the preferred agent mode of operation (push or pull). The default
      /// value is <c>PULL_MODE</c>.
      /// </summary>
      /// <value> Either <c>PUSH_MODE</c> or <c>PULL_MODE</c>
      /// </value>
      public virtual AgentMessagingMode MessagingMode
      {
         get
         {
            string s = this.GetProperty(PROP_MESSAGING_MODE, "Pull");

            if (s.ToUpper().Equals("Push".ToUpper()))
            {
               return AgentMessagingMode.Push;
            }

            return AgentMessagingMode.Pull;
         }

         set
         {
            if (value != AgentMessagingMode.Push && value != AgentMessagingMode.Pull)
            {
               throw new ArgumentException
                   ("setMessagingMode accepts PUSH_MODE or PULL_MODE only");
            }

            this.SetProperty(PROP_MESSAGING_MODE, value.ToString());
         }
      }

      /// <summary>  Determines if the Adk should send a SIF_Sleep message to a zone when
      /// disconnecting. The default is true.
      /// </summary>
      /// <value> true if the Adk should place the agent's ZIS queue in sleep
      /// mode when disconnecting.
      /// </value>
      /// <summary>  Determines if the Adk should send a SIF_Sleep message to a zone when
      /// disconnecting. The default is true.
      /// </summary>
      public virtual bool SleepOnDisconnect
      {
         get { return GetProperty(PROP_SLEEP_ON_DISC, true); }

         set { SetProperty(PROP_SLEEP_ON_DISC, value); }
      }

      /// <summary>  Determines if the Adk's message fDispatcher is disabled. When disabled,
      /// all messages received by the Adk are disposed of immediately without
      /// dispatching to the agent's message handlers. This property should only
      /// be set true in rare cases when the agent should send but not receive
      /// messages. The default is false.
      /// </summary>
      /// <value> true if the Adk's has disabled its message fDispatcher
      /// </value>
      public virtual bool DisableMessageDispatcher
      {
         get { return GetProperty(PROP_DISABLE_DISPATCHER, false); }

         set { SetProperty(PROP_DISABLE_DISPATCHER, value); }
      }

      /// <summary>  Determines if the Adk renders SIF_Response packets with one SIF Data
      /// Object per packet regardless of the requestor's SIF_MaxBufferSize value.
      /// By default, this property is disabled, causing the Adk to fit as many
      /// SIF Data Objects per SIF_Response packet as possible.
      /// </summary>
      /// <value> true if the Adk should render SIF_Response packets with one object per packet
      /// </value>
      public virtual bool OneObjectPerResponse
      {
         get { return GetProperty(PROP_ONEOBJECTPERRESPONSE, false); }

         set { SetProperty(PROP_ONEOBJECTPERRESPONSE, value); }
      }

      //
      /// <summary>  A compatibility option that determines if the Adk renders SIF_Request
      /// messages with SIF_Element field restrictions. When this property is
      /// enabled, the class framework will not include SIF_Element elements in
      /// SIF_Request messages even if you have called the Query.addFieldRestriction
      /// method. This property can be used when requesting data from agents that
      /// do not work well if SIF_Elements are present in the SIF_Request.
      /// </summary>
      /// <value> true if the Adk should not include SIF_Element elements in
      /// SIF_Request messages
      /// </value>
      public virtual bool NoRequestIndividualElements
      {
         get { return GetProperty(PROP_NOREQUESTINDIVIDUALELEMENTS, false); }

         set { SetProperty(PROP_NOREQUESTINDIVIDUALELEMENTS, value); }
      }

      /// <summary>  Gets the maximum size of SIF messages that can be processed by this agent
      /// when sending and receiving. The default value is 32K.
      /// </summary>
      /// <value> The maximum packet size in bytes
      /// </value>
      public virtual int MaxBufferSize
      {
         get { return GetProperty(PROP_MAX_BUFFER_SIZE, 393216); }

         set { SetProperty(PROP_MAX_BUFFER_SIZE, value); }
      }

      /// <summary>  Gets the maximum size of SIF messages that can be processed by this agent
      /// in-memory before off-loading the message to the local file system. Used
      /// internally to influence memory management.
      /// </summary>
      /// <value> The effective buffer size (in bytes)
      /// </value>
      public virtual int EffectiveBufferSize
      {
         get { return GetProperty(PROP_EFFECTIVE_BUFFER_SIZE, 32000); }

         set { SetProperty(PROP_EFFECTIVE_BUFFER_SIZE, value); }
      }

      /// <summary>  Gets the Pull frequency when the agent is registered in Pull mode.
      /// By default, a Pull agent will query the ZIS for new messages every 30
      /// seconds.
      /// </summary>
      /// <value> The number of milliseconds between Pull requests
      /// </value>
      public virtual TimeSpan PullFrequency
      {
         get { return TimeSpan.FromMilliseconds(GetProperty(PROP_PULL_FREQUENCY, 30000)); }

         set { SetProperty(PROP_PULL_FREQUENCY, (int)value.TotalMilliseconds); }
      }

      /// <summary>  For pull agents, gets the amount of time in milliseconds the agent will
      /// delay when it encounters a transport error or disconnected zone when
      /// attempting to pull the next message from its queue.
      /// 
      /// </summary>
      /// <returns> The pull delay (in milliseconds)
      /// </returns>
      /// <summary>  For pull agents, sets the amount of time in milliseconds the agent will
      /// delay when it encounters a transport error or disconnected zone when
      /// attempting to pull the next message from its queue.
      /// 
      /// </summary>
      /// <returns> The pull delay (in milliseconds)
      /// </returns>
      public virtual TimeSpan PullDelayOnError
      {
         get { return TimeSpan.FromMilliseconds(GetProperty(PROP_PULL_DELAY_ON_ERROR, 180000)); }

         set { SetProperty(PROP_PULL_DELAY_ON_ERROR, (int)value.TotalMilliseconds); }
      }

      /// <summary>
      /// Set to True if the agent should Ack SIF_Acks received while doing a Pull request in Pull mode.
      /// This is for compatibility with some Zis's. Default to False.
      /// </summary>
      public virtual bool PullAckAck
      {
         get { return GetProperty(PROP_PULL_ACKACK, false); }

         set { SetProperty(PROP_PULL_ACKACK, value); }
      }

      /// <summary>
      /// Should the ADK use strict versioning?
      /// </summary>
      public virtual bool StrictVersioning
      {
         get { return GetProperty(PROP_STRICT_VERSIONING, false); }

         set { SetProperty(PROP_STRICT_VERSIONING, value); }
      }

      /// <summary>
      /// Determines whether the ADK returns an error message
      /// if it is unable to parse the value of a strongly-typed element
      /// or attribute (one that is defined as a numeric or date type)
      /// </summary>
      public virtual bool StrictTypeParsing
      {
          get { return GetProperty(PROP_STRICT_TYPEPARSING, false); }

          set { SetProperty(PROP_STRICT_TYPEPARSING, value); }
      }

      /// <summary>  Gets the latest version of the SIF Specification supported by the Zone
      /// Integration Server to which the agent is connecting.
      /// </summary>
      /// <value> A SIF Version number (e.g. "1.1", "1.0r1", etc.)
      /// </value>
      /// <remarks>
      /// <para>
      /// This property
      /// defaults to the latest version of the SIF Specification supported by the
      /// Adk.
      /// </para>
      /// <para>
      /// Currently, it affects how SIF_Register messages are sent: if the ZIS
      /// supports SIF 1.1 or later, the Adk will send a SIF_Register with multiple
      /// SIF_Version elements, one for each version of the SIF Specification
      /// supported by the Adk. The first SIF_Version will be the version passed
      /// to the <c>Adk.initialize</c> method. If the ZIS does not support
      /// SIF 1.1 or later, the Adk will send a SIF_Register with a single
      /// SIF_Version element where the value is equal to the version passed to
      /// the <c>Adk.initialize</c> method.
      /// </para>
      /// </remarks>
      public virtual string ZisVersion
      {
         get { return this.GetProperty(PROP_PROVISIONING_ZISVERSION, SifVersion.LATEST.ToString()); }

         set { this.SetProperty(PROP_PROVISIONING_ZISVERSION, value); }
      }

      /// <summary>Gets or sets the the versions of SIF supported by the agent when the class
      /// framework sends a SIF_Register message to the zone.
      /// </summary>
      /// <value>A comma-delimited list of SIF version or wildcard tags.
      /// When connecting to a SIF 1.1 or later zone integration server, the
      /// class framework will include a SIF_Version element for the version
      /// of SIF used to initialize the Adk, followed by one additional
      /// SIF_Version element for each version specified by this value.
      /// </value>
      /// <remarks>
      /// If this property is not defined, it will return null
      /// </remarks>
      public virtual string OverrideSifVersions
      {
         get { return this.GetProperty(PROP_PROVISIONING_OVERRIDESIFVERSIONS); }

         set { this.SetProperty(PROP_PROVISIONING_OVERRIDESIFVERSIONS, value); }
      }

      public virtual string OverrideSifMessageVersionForSifRequests
      {
          get { return this.GetProperty(PROP_PROVISIONING_OVERRIDE_REQUEST_VERSION); }

          set { this.SetProperty(PROP_PROVISIONING_OVERRIDE_REQUEST_VERSION, value); }
      }

      /// <summary>Gets or sets the provisioning mode for this zone</summary>
      public virtual AgentProvisioningMode ProvisioningMode
      {
         get
         {
            string s = GetProperty(PROP_PROVISIONING_MODE);
            if (s == null)
            {
               return AgentProvisioningMode.Adk;
            }

            try
            {
               AgentProvisioningMode mode =
                   (AgentProvisioningMode)
                   Enum.Parse(typeof(AgentProvisioningMode), s, true);
               return mode;
            }
            catch (ArgumentException)
            {
               return AgentProvisioningMode.Adk;
            }
         }

         set { this.SetProperty(PROP_PROVISIONING_MODE, value.ToString()); }
      }


      /// <summary>
      /// Gets /Sets whether the ADK will provision the agent using the legacy SIF_Subscribe and
      /// SIF_Provide messages even if it is running in SIF 2.0.
      /// 
      /// NOTE: The ADK will ALWAYs provision the agent in legacy mode if the ADK is initialized
      /// to SIF 1.5r1 or less
      ///
      /// return True if legacy provisioning should be used
      /// </summary>
      public Boolean ProvisionInLegacyMode
      {
         get
         {
            return GetProperty(PROP_PROVISIONING_LEGACY, false);
         }
         set
         {
            SetProperty(PROP_PROVISIONING_LEGACY, value);
         }
      }


      /// <summary>Gets or sets the name of the Transport Protocol to use for this agent or zone.</summary>
      /// <value> The name of the Transport Protocol (defaults to <i>http</i>)
      /// </value>
      public virtual string TransportProtocol
      {
         get { return this.GetProperty(PROP_MESSAGING_TRANSPORT, "http"); }

         set
         {
            string[] supported = Adk.TransportProtocols;
            for (int i = 0; i < supported.Length; i++)
            {
               if (supported[i].ToUpper().Equals(value.ToUpper()))
               {
                  this.SetProperty(PROP_MESSAGING_TRANSPORT, value);
                  return;
               }
            }

            throw new ArgumentException(value + " is not a supported protocol");
         }
      }

      /// <summary>
      /// 
      /// </summary>
      public virtual bool DisableQueue
      {
         get { return GetProperty(PROP_QUEUE_DISABLE, true); }

         set { SetProperty(PROP_QUEUE_DISABLE, value); }
      }

      /// <summary>
      /// Keeps the text of the SIFMessage in memory after parsing into SDO objects.
      /// Defaults to false. Only set to true if your agent has a class that is implementing
      /// the <see cref="IMessagingListener"/> interface
      /// </summary>
      public virtual bool KeepMessageContent
      {
         get { return GetProperty(PROP_KEEP_MESSAGE_CONTENT, false); }

         set { SetProperty(PROP_KEEP_MESSAGE_CONTENT, value); }
      }

      /// <summary>
      /// Ignore any problems while provisioning objects. Defaults to false
      /// </summary>
      public virtual bool IgnoreProvisioningErrors
      {
         get { return GetProperty(PROP_IGNORE_PROVISIONING_ERRORS, false); }

         set { SetProperty(PROP_IGNORE_PROVISIONING_ERRORS, value); }
      }

      /// <summary>
      /// Use the SystemControl message to request SIF_ZoneStatus
      /// When this property is <code>true</code>
      /// and the ADK is initialized for SIF 1.5 or later, it will issue a
      /// synchronous SIF_SystemControl/SIF_GetZoneStatus message when the 
      /// <code>Zone.getZoneStatus</code> method is called instead of
      /// using asynchronous SIF_Request messages. This mechanism of obtaining 
      /// SIF_ZoneStatus is preferred over the traditional SIF_Request method, 
      /// but is not officially supported in SIF as of 1.5. It is considered 
      /// experimental. When this property is <code>false</code> (the default), 
      /// the ADK issues SIF_Requests to obtain the SIF_ZoneStatus object.
      /// </summary>
      public virtual bool UseZoneStatusSystemControl
      {
         get
         {
             String overrideZISVersion = GetProperty(PROP_PROVISIONING_ZISVERSION);
             SifVersion calculatedVersion = Adk.SifVersion;
             if (overrideZISVersion != null)
             {
                 try
                 {
                     calculatedVersion = SifVersion.Parse(overrideZISVersion);
                 }
                 catch (Exception iae)
                 {
                     Adk.Log.Warn("Unable to parse property 'adk.provisioning.zisVersion'", iae);
                     calculatedVersion = Adk.SifVersion;
                 }
             }
             
            Boolean useSystemControlDefault = calculatedVersion.CompareTo(SifVersion.SIF15r1) >= 0;
            return GetProperty(PROP_USE_ZONE_STATUS_SYSTEM_CONTROL, useSystemControlDefault);
         }

         set
         {
            SetProperty(PROP_USE_ZONE_STATUS_SYSTEM_CONTROL, value);
         }
      }

      /// <summary>
      /// The authentication level to use when connecting to the ZIS, defaults to 0
      /// </summary>
      public virtual int AuthenticationLevel
      {
         get { return GetProperty(PROP_AUTH_LEVEL, 0); }

         set { this.SetProperty(PROP_AUTH_LEVEL, value); }
      }

      /// <summary>
      /// The encryption level to use when communicating with the ZIS. defaults to 0
      /// </summary>
      public virtual int EncryptionLevel
      {
         get { return GetProperty(PROP_ENCRYPT_LEVEL, 0); }

         set { this.SetProperty(PROP_ENCRYPT_LEVEL, value); }
      }

      /// <summary>  Gets the default timeout value used by the Adk for functions that
      /// accept a timeout as a parameter.
      /// </summary>
      /// <returns> A timeout value as a timespan value (defaults to 30 seconds )
      /// </returns>
      /// <summary>  Sets the default timeout value used by the Adk for functions that
      /// accept a timeout as a parameter.
      /// </summary>
      public virtual TimeSpan DefaultTimeout
      {
         get { return TimeSpan.FromMilliseconds(GetProperty(PROP_DEFAULT_TIMEOUT, 30000)); }

         set { SetProperty(PROP_DEFAULT_TIMEOUT, (int)value.TotalMilliseconds); }
      }

      /// <summary>Determines how the Adk prepares SIF_Provide and SIF_Subscribe messages.</summary>
      /// <value> false if the Adk should send an individual SIF_Provide and
      /// SIF_Subscribe message to the zone for each SIF Data Object. If any
      /// of the messages fail with an Access Control error (Category 4),
      /// the error is recorded as a warning and subsequently returned by the
      /// <c>Zone.getConnectWarnings</c> method. All other SIF Errors
      /// result in an exception thrown by the <c>Zone.connect</c> method.
      /// When this property is set to true, SIF Data Objects are batched into
      /// a single message that will be accepted or rejected as a group by the
      /// zone integration server.
      /// </value>
      public virtual bool BatchProvisioning
      {
         get { return GetProperty(PROP_PROVISIONING_BATCH, false); }

         set { SetProperty(PROP_PROVISIONING_BATCH, value); }
      }

      /// <summary>  Determines if the Adk processes or ignores SIF_Event messages that were
      /// reported by this agent (i.e. the SourceId of the SIF_Event matches the
      /// SourceId of this agent). By default, such events are ignored by the
      /// class framework and automatically acknowledged as successfully received.
      /// </summary>
      /// <value> true if the class framework will process SIF_Event messages that
      /// were reported by this agent; false if it will ignore them
      /// </value>
      public virtual bool ProcessEventsFromSelf
      {
         get { return GetProperty(PROP_PROCESSEVENTSFROMSELF, false); }

         set { SetProperty(PROP_PROCESSEVENTSFROMSELF, value); }
      }

      /// <summary>
      /// Returns the default encryption algorithm used for writing passwords
      /// in the <c>Authentication</c> object
      /// </summary>
      public string DefaultEncryptionAlgorithm
      {
         get { return GetProperty(PROP_ENCRYPT_ALGORITHM); }
         set { SetProperty(PROP_ENCRYPT_ALGORITHM, value); }
      }

      /// <summary>
      /// Returns the default encryption keyname used for writing passwords
      /// in the <c>Authentication</c> object
      /// </summary>
      public string DefaultEncryptionKeyName
      {
         get { return GetProperty(PROP_ENCRYPT_KEY); }
         set { SetProperty(PROP_ENCRYPT_KEY, value); }
      }

      /// <summary>
      /// Returns the encryption key with the specifiec name.
      /// </summary>
      /// <param name="keyName">The name that the key is saved under in the properties</param>
      /// <returns>The encryption key or null if not found</returns>
      public byte[] GetEncryptionKey(string keyName)
      {
         string key = GetProperty(PROP_ENCRYPT_KEYS_BASE + keyName);
         if (key != null)
         {
            return Convert.FromBase64String(key);
         }
         else
         {
            return null;
         }
      }

      /// <summary>
      /// Adds the encryption key with the specified name to the properties
      /// </summary>
      /// <param name="keyName">The name to save the key under</param>
      /// <param name="key">The encryption key</param>
      public void SetEncryptionKey(string keyName,
                                    byte[] key)
      {
         SetProperty(PROP_ENCRYPT_KEYS_BASE + keyName, Convert.ToBase64String(key));
      }


      /// <summary>  <c>adk.defaultTimeout</c></summary>
      public const string PROP_DEFAULT_TIMEOUT = "adk.defaultTimeout";


      /// <summary>  The <c>adk.messaging.mode</c> property</summary>
      public const string PROP_MESSAGING_MODE = "adk.messaging.mode";

      /// <summary>  The <c>adk.messaging.sleepOnDisconnect</c> property</summary>
      public const string PROP_SLEEP_ON_DISC = "adk.messaging.sleepOnDisconnect";

      /// <summary>  The <c>adk.messaging.disableDispatcher</c> property</summary>
      public const string PROP_DISABLE_DISPATCHER = "adk.messaging.disableDispatcher";

      /// <summary>  The <c>adk.messaging.transport</c> property</summary>
      public const string PROP_MESSAGING_TRANSPORT = "adk.messaging.transport";

      /// <summary>  The <c>adk.messaging.maxBufferSize</c> property</summary>
      public const string PROP_MAX_BUFFER_SIZE = "adk.messaging.maxBufferSize";

      /// <summary>  The <c>adk.messaging.effectiveBufferSize</c> property</summary>
      public const string PROP_EFFECTIVE_BUFFER_SIZE = "adk.messaging.effectiveBufferSize";

      /// <summary>  <c>adk.messaging.pullFrequency</c></summary>
      public const string PROP_PULL_FREQUENCY = "adk.messaging.pullFrequency";

      /// <summary>  <c>adk.messaging.pullFrequency</c></summary>
      public const string PROP_PULL_DELAY_ON_ERROR = "adk.messaging.pullDelayOnError";

      /// <summary>  <c>adk.messaging.pullAckAck</c></summary>
      public const string PROP_PULL_ACKACK = "adk.messaging.pullAckAck";

      /// <summary>  <c>adk.messaging.strictVersioning</c></summary>
      public const string PROP_STRICT_VERSIONING = "adk.messaging.strictVersioning";

      /// <summary>  <c>adk.messaging.strictTypeParsing</c></summary>
       public const string PROP_STRICT_TYPEPARSING = "adk.messaging.strictTypeParsing";

      /// <summary>  <c>adk.messaging.oneResponsePerPacket</c></summary>
      public const string PROP_ONEOBJECTPERRESPONSE = "adk.messaging.oneObjectPerResponse";

      /// <summary>  <c>adk.messaging.processEventsFromSelf</c></summary>
      public const string PROP_PROCESSEVENTSFROMSELF = "adk.messaging.processEventsFromSelf";

      /// <summary>  <c>adk.messaging.noRequestIndividualElements</c></summary>
      public const string PROP_NOREQUESTINDIVIDUALELEMENTS =
          "adk.messaging.noRequestIndividualElements";


      /// <summary> 	<c>adk.compatibility.useZoneStatusSystemControl</c></summary>
      public const string PROP_USE_ZONE_STATUS_SYSTEM_CONTROL =
          "adk.compatibility.useZoneStatusSystemControl";


      /// <summary>  <c>adk.provisioning.overrideSifVersions</c></summary>
      public const string PROP_PROVISIONING_OVERRIDESIFVERSIONS =
          "adk.provisioning.overrideSifVersions";

      /// <summary>  <c>adk.provisioning.batch</c></summary>
      public const string PROP_PROVISIONING_BATCH = "adk.provisioning.batch";

      /// <summary>  <c>adk.provisioning.mode</c></summary>
      public const string PROP_PROVISIONING_MODE = "adk.provisioning.mode";

      /// <summary>  <c>adk.provisioning.ignoreErrors</c></summary>
      public const string PROP_IGNORE_PROVISIONING_ERRORS = "adk.provisioning.ignoreErrors";

      /// <summary>  <c>adk.keepMessageContent</c></summary>
      public const string PROP_KEEP_MESSAGE_CONTENT = "adk.keepMessageContent";

      /// <summary>  <c>adk.queue.disable</c></summary>
      public const string PROP_QUEUE_DISABLE = "adk.queue.disable";

      /// <summary>  <c>adk.security.authenticationLevel</c></summary>
      public const string PROP_AUTH_LEVEL = "adk.security.authenticationLevel";

      /// <summary>  <c>adk.security.encryptionLevel</c></summary>
      public const string PROP_ENCRYPT_LEVEL = "adk.security.encryptionLevel";

      /// <summary>  <c>adk.encryption.algorithm</c></summary>
      public const string PROP_ENCRYPT_ALGORITHM = "adk.encryption.algorithm";

      /// <summary>  <c>adk.encryption.key</c></summary>
      public const string PROP_ENCRYPT_KEY = "adk.encryption.key";

      /// <summary>  <c>adk.encryption.keys.</c></summary>
      public const string PROP_ENCRYPT_KEYS_BASE = "adk.encryption.keys.";

      /// <summary>  <c>adk.provisioning.icon</c></summary>
      public const String PROP_PROVISIONING_ICON = "adk.provisioning.icon";


      /// <summary>  <c>adk.provisioning.agentVendor</c></summary>
      public const String PROP_PROVISIONING_AGENT_VENDOR = "adk.provisioning.agentVendor";

      /// <summary><c>adk.provisioning.overrideSifMessageVersionForSifRequests</c></summary>
      public const String PROP_PROVISIONING_OVERRIDE_REQUEST_VERSION = "adk.provisioning.overrideSifMessageVersionForSifRequests";

      /// <summary>  <c>adk.provisioning.agentVersion</c></summary>
      public const String PROP_PROVISIONING_AGENT_VERSION = "adk.provisioning.agentVersion";

      /// <summary>  <c>adk.provisioning.applicationName</c></summary>
      public const String PROP_PROVISIONING_APP_NAME = "adk.provisioning.applicationName";

      /// <summary>  <c>adk.provisioning.applicationVendor</c></summary>
      public const String PROP_PROVISIONING_APP_VENDOR = "adk.provisioning.applicationVendor";

      /// <summary>  <c>adk.provisioning.applicationVersion</c></summary>
      public const String PROP_PROVISIONING_APP_VERSION = "adk.provisioning.applicationVersion";

      /// <summary>  <c>adk.provisioning.zisVersion</c></summary>
      public const String PROP_PROVISIONING_ZISVERSION = "adk.provisioning.zisVersion";

      /// <summary>  <c>adk.provisioning.legacy</c></summary>
      public const String PROP_PROVISIONING_LEGACY = "adk.provisioning.legacy";


      /// <summary>  Constructor</summary>
      protected internal AgentProperties(Agent agent)
         : base(agent) { }

      /// <summary>  Constructor</summary>
      /// <param name="inherit">The parent AgentProperties from which properties will be
      /// inherited when not explicitly set on this object
      /// </param>
      public AgentProperties(AgentProperties inherit)
         : base(inherit) { }

      /// <summary>  Assigns default property values. Called by the constructor to import
      /// the value of all System properties beginning with the
      /// prefix <c>adk.</c>
      /// </summary>
      public override void Defaults(Object owner)
      {
         NameValueCollection sysprops = Properties.GetProperties();

         //  Get all System properties that begin with "adk."
         foreach (string k in sysprops.Keys)
         {
            if (k.StartsWith("adk.") && !k.StartsWith("adk.transport"))
            {
               string val = sysprops[k];

               if ((Adk.Debug & AdkDebugFlags.Properties) != 0)
               {
                  if (owner == null)
                  {
                     Adk.Log.Debug("Using System property " + k + " = " + val);
                  }
                  else if (owner is ZoneImpl)
                  {
                     ((ZoneImpl)owner).Log.Debug
                         ("Using System property " + k + " = " + val);
                  }
                  else if (owner is Agent)
                  {
                     Agent.Log.Debug("Using System property " + k + " = " + val);
                  }
               }
               this.SetProperty(k, val);
            }
         }
      }
   }
}
