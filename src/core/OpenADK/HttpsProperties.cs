//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  Properties for the HTTPS transport protocol.</summary>
    /// <remarks>
    /// <para>
    /// To set default HTTPS properties, call the Agent.DefaultHttpsProperties
    /// method to obtain the agent's default properties for this transport protocol.
    /// The defaults are used by all zones that do not explicitly set their own
    /// transport properties. Alternatively, you may set the default value of a
    /// property by modifying the appropriate setting in the "appSettings" section of the 
    /// application config file. Property names follow the naming convention
    /// <c>adk.transport.https.</c><i>property</i> (e.g. <c>adk.transport.https.port</c>).
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
    /// <item><term>Use an agent configuration file that can be read by the <see cref="OpenADK.Library.Tools.Cfg.AgentConfig"/> class </term></item>
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
    public class HttpsProperties : HttpProperties
    {
        /// <summary>  Constructor</summary>
        public HttpsProperties()
            : base() {}

        /// <summary>  Constructs an HttpsProperties object that inherits values from a parent</summary>
        /// <param name="parent">The Http or HttpsProperties object from which properties
        /// will be inherited if not explicitly defined by this object
        /// </param>
        public HttpsProperties( HttpProperties parent )
            : base( parent ) {}

        /// <summary>  Gets the name of the transport protocol associated with these properties</summary>
        /// <returns> The protocol name ("https")
        /// </returns>
        public override string Protocol
        {
            get { return "https"; }
        }


        /// <summary>
        /// Gets the filename of a certificate to use for SSL Authentication. The certificate must be
        /// in the PFX/P12 format and have a ".pfx" extension.
        /// </summary>
        /// <remarks>
        /// <para>
        ///  If this property is
        /// not set, the ADK will attempt to find a certificate using the <see cref="CertStore"/>,
        /// <see cref="CertStoreLocation"/>, and <see cref="SSLCertName"/> values.
        /// </para>
        /// <para>
        /// This property is represented by the <c>adk.transport.https.sslCertFile</c> configuration property
        /// </para>
        /// </remarks>
        public string SSLCertFile
        {
            get { return this.GetProperty( "sslCertFile" ); }
            set { this.SetProperty( "sslCertFile", value ); }
        }

        /// <summary>
        /// Gets and sets the password to use for the PFX/P12  certificate, if necessary
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is represented by the <c>adk.transport.https.sslCertFilePassword</c> configuration property
        /// </para>
        /// </remarks>
        public string SSLCertFilePassword
        {
            get { return this.GetProperty( "sslCertFilePassword" ); }
            set { this.SetProperty( "sslCertFilePassword", value ); }
        }

        /// <summary>
        /// Gets the certificate store location. Defaults to CurrentUser
        /// </summary>
        /// <remarks>Possible values include:
        /// <list type="table">
        /// <listheader><term>Value</term><description>Description</description></listheader>
        /// <item><term>CurrentService
        /// </term><description>The certificate store for the current service.
        /// </description></item>
        /// <item><term>CurrentUser
        /// </term><description>The certificate store for the currently logged-on user. This is the default value for this property if it is not explicitly specified.
        /// </description></item>
        /// <item><term>CurrentUserGroupPolicy
        /// </term><description>The certificate store for the currently logged-on group.
        /// </description></item>
        /// <item><term>LocalMachine
        /// </term><description>The certificate store for the local computer.
        /// </description></item>
        /// <item><term>LocalMachineEnterprise
        /// </term><description>The certificate store for the local machine enterprise downloaded from a network setting.
        /// </description></item>
        /// <item><term>LocalMachineGroupPolicy
        /// </term><description>The certificate store for the local machine group policy downloaded from a network setting.
        /// </description></item>
        /// </list>
        /// <para>
        /// This property is represented by the <c>adk.transport.https.certStoreLocation</c> configuration property
        /// </para>
        /// </remarks>
        public string CertStoreLocation
        {
            get { return this.GetProperty( "certStoreLocation" ); }

            set { this.SetProperty( "certStoreLocation", value ); }
        }

        /// <summary>
        /// Gets the certificate store to open. Defaults to "My", which represents the "Personal" folder
        /// in the Windows certificate manager
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is represented by the <c>adk.transport.https.certStore</c> configuration property
        /// </para>
        /// </remarks>
        public string CertStore
        {
            get { return this.GetProperty( "certStore" ); }

            set { this.SetProperty( "certStore", value ); }
        }

        /// <summary>
        /// Gets the certificate that should be used for SSL when the agent is running as a server
        /// (Https PUSH mode). If not specified, the first applicable certificate in the store 
        /// that is valid for server authentication and not out of date will be used
        /// </summary>
        /// <remarks>
        /// <para>
        /// The Adk looks for the certificate using first a distinguished name search using the value
        /// of this property. If a certificate is not found, it then does a search by subject string, which
        /// looks for the value of this property in any part of the subject of the certificate. The
        /// search is case-insensitive.
        /// </para>
        /// <para>
        /// Examples of the value of this property would be. "localhost" or 
        /// "CN=localhost,OU=Software Development,O=The KPD-Team,L=Leuven,S=Brabant".
        /// The complete Distinguished name of a certificate can be derived by looking 
        /// at the Subject in the Certificate properties. Note that the
        /// distinguished name must be specified in X.500 format, which is reverse order from 
        /// what it appears in the Subject field</para>
        /// <para>
        /// This property is represented by the <c>adk.transport.https.sslCertName</c> configuration property
        /// </para>
        /// </remarks>
        public string SSLCertName
        {
            get { return this.GetProperty( "sslCertName" ); }

            set { this.SetProperty( "sslCertName", value ); }
        }


        /// <summary>
        /// Gets the certificate that the agent should use for client authentication when connecting
        /// to the ZIS. This is only done if the agent is already connecting using SSL If not specified, 
        /// the first applicable certificate in the store 
        /// that is valid for client authentication and not out of date will be used
        /// </summary>
        /// <remarks>
        /// <para>
        /// The Adk looks for the certificate using first a distinguished name search using the value
        /// of this property. If a certificate is not found, it then does a search by subject string, which
        /// looks for the value of this property in any part of the subject of the certificate. The
        /// search is case-insensitive.
        /// </para>
        /// <para>
        /// Examples of the value of this property would be. "localhost" or 
        /// "CN=localhost,OU=Software Development,O=The KPD-Team,L=Leuven,S=Brabant".
        /// The complete Distinguished name of a certificate can be derived by looking 
        /// at the Subject in the Certificate properties. Note that the
        /// distinguished name must be specified in X.500 format, which is reverse order from 
        /// what it appears in the Subject field</para>
        /// <para>
        /// This property is represented by the <c>adk.transport.https.clientCertName</c> configuration property
        /// </para>
        /// </remarks>
        public string ClientCertName
        {
            get { return this.GetProperty( "clientCertName" ); }

            set { this.SetProperty( "clientCertName", value ); }
        }


        /// <summary>
        /// Returns the SIF Authentication Level required by the agent for incoming https connections
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property equates to the SIF AUthentication level and can have one of the following values
        /// </para>
        /// <list type="table">
        /// <listheader><term>Value</term><description>Description</description></listheader>
        /// <item><term>0</term><description>SIF Authentication level 0: No client authentication is required</description></item>
        /// <item><term>1</term><description>SIF Authentication level 1: A valid certificate must be presented.</description></item>
        /// <item><term>3</term><description>SIF Authentication level 2: A valid certificate from a trusted certificate authority must be presented.</description></item>
        /// <item><term>4</term><description>SIF Authentication level 3: A valid certificate from a trusted certificate authority must be presented and the CN field of the certificate's
        /// Subject entry must match the host sending the certificate.</description></item>
        /// </list>
        /// <para>
        /// This property is represented by the <c>adk.transport.https.clientAuthLevel</c> configuration property
        /// </para>
        /// </remarks>
        public int ClientAuthLevel
        {
            get { return GetProperty( "clientAuthLevel", 0 ); }

            set { SetProperty( "clientAuthLevel", value ); }
        }
    }
}
