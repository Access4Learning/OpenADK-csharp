//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Net;
using System.Collections;
using OpenADK.Util;
namespace OpenADK.Library
{
   /// <summary>  Properties for the HTTP transport protocol.</summary>
   /// <remarks>
   /// <para>
   /// To set default HTTP properties, call the Agent.DefaultHttpProperties
   /// method to obtain the agent's default properties for this transport protocol.
   /// The defaults are used by all zones that do not explicitly set their own
   /// transport properties. Alternatively, you may set the default value of a
   /// property by modifying the appropriate setting in the "appSettings" section of the 
   /// application config file. Property names follow the naming convention
   /// <c>adk.transport.http.</c><i>property</i> (e.g. <c>adk.transport.http.port</c>).
   /// </para>
   /// <para>
   /// No default HTTP or HTTPS port is
   /// assigned to push mode agents by the class framework. It is the developer's
   /// responsibility to assign a default port. To do so, use one of the following
   /// methods:
   /// </para>
   /// <list type="bullet">
   /// <listheader><term>Items</term><description>Descriptions</description></listheader>
   /// <item><term>Set the <c>adk.transport.https.port</c> system property prior to creating your agent's Zones and/or Topics. This property can be set programmatically by adding it to the "appSettings" node in the application's configuration file.</term></item>
   /// <item><term>Set the Port property on the default HttpProperties and/or HttpsProperties objects prior to creating and your agent's Zones instances. </term></item>
   /// <item><term>Use an agent configuration file that can be read by the <see cref="OpenADK.Library.Tool.Cfg.AgentConfig"/> class </term></item>
   /// </list>
   /// </remarks>
   /// <example>
   /// Set the Port property on the default HttpProperties and/or
   /// HttpsProperties objects prior to creating and your agent's Zones
   /// instances. The following block of code demonstrates:
   /// <code>
   /// //  Set transport properties for HTTP
   /// Agent myAgent = ...
   /// HttpProperties http = agent.DefaultHttpProperties;
   /// http.Port = 7081;
   /// //  Set transport properties for HTTPS
   /// HttpsProperties https = agent.DefaultHttpsProperties;
   /// https.setPort( 7082 );
   /// ...
   /// </code>
   /// </example>
   /// <author>  Eric Petersen
   /// </author>
   /// <version>  Adk 1.0
   /// </version>
   [Serializable]
   public class HttpProperties : TransportProperties
   {

      /// <summary>  Gets the name of the transport protocol associated with these properties</summary>
      /// <returns> The string "http"
      /// </returns>
      public override string Protocol
      {
         get { return "http"; }
      }

      /// <summary>Gets or sets the port that push-mode agents will use when establishing a local socket.</summary>
      /// <value> The port number or -1 if uninitialized
      /// </value>
      /// <summary>  Sets the port that push-mode agents will use when establishing a local socket.</summary>
      public virtual int Port
      {
         get { return GetProperty("port", -1); }

         set { SetProperty("port", value); }
      }

      /// <summary>Gets or Sets the port that push-mode agents will use when sending a SIF_Register
      /// message to the zone integration server, if different than the port the
      /// agent binds on when establishing the local socket. If set this value
      /// will be used in preparing the SIF_Register/SIF_Protocol/SIF_URL. If not
      /// set, the Adk will use the Port property to prepare this element.
      /// </summary>
      /// <value> The port to be used in the SIF_Register/SIF_Protocol/SIF_URL element
      /// or -1 if not initialized
      /// 
      /// @since Adk 1.5
      /// </value>
      public virtual int PushPort
      {
         get { return GetProperty("pushPort", -1); }

         set { SetProperty("pushPort", value); }
      }

      /// <summary> Gets or sets string that contains the host name or an IP address in dotted-quad notation for IPv4 
      /// and in colon-hexadecimal notation for IPv6. Unlike the Java ADK, the .NET ADK will return NULL for this property
      /// if it is not set. If it is not set, the .NET ADK will bind to all local IP addresses and return the first
      /// one to the ZIS in the SIF_Register list
      /// </summary>
      /// <value> The host name or IP address passed to the setHost method.
      /// </value>
      public virtual String Host
      {
         get
         {
             return GetProperty( "host" );
         }

         set
         {
             this.SetProperty("host", value );
         }
      }


      /// <summary>Gets or sets the hostname that push-mode agents will use when sending a SIF_Register
      /// message to the zone integration server, if different than the hostname the
      /// agent binds on when establishing the local socket. If set this value
      /// will be used in preparing the SIF_Register/SIF_Protocol/SIF_URL. If not
      /// set, the Adk will use the Host property to prepare this element.
      /// 
      /// Most agents do not call this method. It is only needed when the network
      /// configuration demands the hostname and port the ZIS uses to contact the
      /// agent is different than the local socket the agent binds on.
      /// 
      /// </summary>
      /// <value>The hostname to be used in the SIF_Register/SIF_Protocol/SIF_URL element
      /// or null if not initialized
      /// 
      /// @since Adk 1.5
      /// </value>
      public virtual string PushHost
      {
         get { return this.GetProperty("pushHost"); }

         set { this.SetProperty("pushHost", value); }
      }

      /// <summary>
      /// This property determines whether keep-alives are turned on on connections initiated from the 
      /// agent to the ZIS. The default value is TRUE.
      /// </summary>
      public virtual bool KeepAliveOnSend
      {
         get { return this.GetProperty("keepAliveOnSend", true); }
         set { this.SetProperty("keepAliveOnSend", value); }
      }
      /// <summary>
      /// This property gets or sets the IPVersion (IPV6 by default)
      /// </summary>
      public virtual bool UseIPV6
      {
         get { return GetProperty("useIPV6", false); }

         set { SetProperty("useIPV6", value); }
      }
      /// <summary>
      /// This property determines whether keep-alives are turned on on connections initiated from the 
      /// ZIS to the agent ( for "Push" agents ). The default value is TRUE.
      /// </summary>
      public virtual bool KeepAliveOnReceive
      {
         get { return this.GetProperty("keepAliveOnReceive", true); }
         set { this.SetProperty("keepAliveOnReceive", value); }
      }

       /// <summary>
       /// The maximum number of HTTP connections that will be allowe for this transport
       /// </summary>
       public int MaxConnections
       {
           get { return GetProperty( "maxConnections", 0 ); }
           set{ SetProperty( "maxConnections", value );}
       }

       /// <summary>
       /// Gets or sets whether the ADK is configured to support running a web application.
       /// </summary>
       public bool ServletEnabled
       {
           get { return GetProperty( "servletEnabled", false ); }
           set { SetProperty( "servletEnabled", value );}
       }

       /// <summary>
      /// Constructs an empty HttpProperties object
      /// </summary>
      public HttpProperties() { }

      /// <summary>  Constructs an HttpProperties object that inherits values from a parent</summary>
      /// <param name="parent">The HttpProperties object from which properties
      /// will be inherited if not explicitly defined by this object
      /// </param>
      public HttpProperties(HttpProperties parent)
         : base(parent) { }
   }
}
