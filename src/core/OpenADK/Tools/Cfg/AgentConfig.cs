//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.IO;
using System.Xml;
using OpenADK.Library.Tools.Mapping;
using OpenADK.Util;

namespace OpenADK.Library.Tools.Cfg
{
    /// <summary>  Implements an XML-based configuration file for agents.
    /// 
    /// To read the configuration file into memory, call the <c>read</c>
    /// method and specify whether to validate the XML document against a DTD. The
    /// methods of the AgentConfig class do not perform validation of required
    /// elements and attributes or their values, so it is recommended that you
    /// validate against a DTD. A default DTD is located in the <c>docs</c>
    /// directory.
    /// 
    /// 
    /// <b>Applying the Configuration</b>
    /// 
    /// Once the configuration file is parsed, you can inspect the elements and
    /// attributes by calling the various methods of this class. Or, you can call
    /// the <c>apply</c> method prior to agent initialization to automatically
    /// apply the configuration settings to your agent as follows:
    /// 
    /// 
    /// <ul>
    /// <li>
    /// The root <c>&lt;agent&gt;</c> attributes are inspected to set
    /// the SourceId and default SIF Version for the agent.
    /// </li>
    /// <li>
    /// For each <c>&lt;property&gt;</c> element, the associated
    /// property is set in the agent's AgentProperties object.
    /// </li>
    /// <li>
    /// The agent's transports are configured according to the properties
    /// of the <c>&lt;transport&gt;</c> elements
    /// <li>
    /// A zone is created for each <c>&lt;zone&gt;</c> element
    /// </li>
    /// </ul>
    /// 
    /// Most agents will extend or modify this class to add additional configuration
    /// settings specific to the agent. The source code can be found in the Adk's
    /// <c>extras</c> directory.
    /// 
    /// <b>Properties</b>
    /// 
    /// Agent and Zone properties can be defined at three levels:
    /// 
    /// <ul>
    /// <li>
    /// <c>&lt;property&gt;</c> elements of the root <c>&lt;agent&gt;</c>
    /// node are intended to serve as global defaults. When then applyProperties
    /// method is called, these property values are assigned to the Agent's
    /// AgentProperties object.
    /// </li>
    /// <li>
    /// <c>&lt;property&gt;</c> elements of a zone <c>&lt;template&gt;</c>
    /// are inherited by zones that reference that template. When the applyZones
    /// method is called, these property values are assigned to the Zone's
    /// AgentProperties object unless specifically overridden by <c>&lt;property&gt;</c>
    /// elements defined by each <c>&lt;zone&gt;</c>
    /// </li>
    /// <li>
    /// <c>&lt;property&gt;</c> elements of a <c>&lt;zone&gt;</c>
    /// are specific to that zone and override any property defined in the
    /// template referenced by the zone
    /// </li>
    /// </ul>
    /// 
    /// <b>Updating the Configuration Programmatically</b>
    /// 
    /// The elements and attributes of the configuration file are represented as DOM
    /// Nodes that can be programmatically updated using the standard DOM interface.
    /// Use the convenience methods GetZoneNode, GetZoneTemplateNode, getTransportNode,
    /// and so on to obtain a reference to a DOM XmlElement instance for a group of
    /// configuration settings. The <c>GetProperty</c> and <c>SetProperty</c>
    /// methods can also be used to manipulate the <c>&lt;property&gt;</c>
    /// children of an element. Static helper routines from the
    /// com.OpenADK.util.XMLUtils class may also be used to get and set
    /// element attributes and text values.
    /// 
    /// 
    /// Call the <c>save</c> method to write the configuration to a file.
    /// 
    /// </summary>
    /// <author> Data Solutions
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class AgentConfig
    {
        /// <summary>  Determines if the configuration file has been loaded</summary>
        /// <returns> true if the <c>read</c> method has been successfully called
        /// </returns>
        public virtual bool Loaded
        {
            get { return fDoc != null; }
        }

        /// <summary>  Gets the DOM document</summary>
        /// <returns> The loaded configuration file
        /// </returns>
        public virtual XmlDocument Document
        {
            get { return fDoc; }
        }

        /// <summary>  Gets the version of SIF that should be used by the agent</summary>
        /// <returns> A SifVersion object for the value of the <c>sifVersion</c>
        /// attribute specified by the <c>&lt;agent&gt;</c> element, or
        /// the Adk's default SifVersion if this attribute was not set
        /// </returns>
        public virtual SifVersion Version
        {
            get
            {
                XmlElement agent = RootNode;
                if ( agent != null ) {
                    string version = agent.GetAttribute( "sifVersion" );
                    if ( version.Length > 0 ) {
                        return SifVersion.Parse( version );
                    }
                }

                return SifVersion.LATEST;
            }
        }

        /// <summary>  Gets the Mappings object</summary>
        /// <returns> The <c>&lt;agent&gt;</c> node if defined
        /// </returns>
        public virtual Mappings Mappings
        {
            get { return fMappings; }
        }

        /// <summary>  Gets the root <c>&lt;agent&gt;</c> element</summary>
        /// <returns> The root node
        /// </returns>
        public virtual XmlElement RootNode
        {
            get { return Document[XmlConstants.ROOT_ELEMENT]; }
        }

        /// <summary>  Gets the SourceId that should be used by the agent</summary>
        public virtual string SourceId
        {
            get
            {
                XmlElement agent = RootNode;
                return agent != null ? agent.GetAttribute( "id" ) : null;
            }
        }

        /// <summary>  Gets all <c>&lt;property&gt;</c> values defined for the root
        /// <c>&lt;agent&gt;</c> node.
        /// </summary>
        public virtual XmlElement [] AgentPropertyNodes
        {
            get { return GetPropertyNodes( RootNode ); }
        }

        /// <summary>  Gets an array of all zones defined by the configuration file</summary>
        /// <returns> An array of DOM Nodes representing the <c>&lt;zone&gt;</c> elements
        /// defined by the configuration file
        /// </returns>
        public virtual XmlElement [] ZoneNodes
        {
            get
            {
                ArrayList list = new ArrayList();
                XmlElement root = RootNode;
                if ( root != null ) {
                    XmlNodeList nlist = root.SelectNodes( "zone" );
                    foreach ( XmlElement element in nlist ) {
                        list.Add( element );
                    }
                }
                XmlElement [] n = new XmlElement[list.Count];
                list.CopyTo( n );
                return n;
            }
        }

        /// <summary>  The configuration file</summary>
        /// <seealso cref="Read">
        /// </seealso>
        private FileInfo fSrc;

        /// <summary>  The DOM document produced by reading the configuration file</summary>
        /// <seealso cref="Read">
        /// </seealso>
        private XmlDocument fDoc;

        /// <summary>  The root <c>&lt;mappings&gt;</c> object defines rules for mapping
        /// application database fields to SIF Data Object elements and attributes.
        /// Refer to the Mappings class for details.
        /// </summary>
        private Mappings fMappings;


        /// <summary>  Constructor</summary>
        public AgentConfig() {}

        /// <summary>  Read a configuration file into memory.
        /// 
        /// </summary>
        /// <param name="file">The path to the configuration file
        /// </param>
        /// <param name="validate">true to validate the configuration file
        /// 
        /// </param>
        /// <returns> A DOM Document encapsulating the configuration file. This
        /// object may also be obtained by calling <c>getDocument</c>
        /// </returns>
        /// <exception cref="IOException">  thrown if an error occurs reading the file
        /// </exception>
        /// <exception cref="AdkException">  thrown if the Adk is not initialized
        /// </exception>
        /// <exception cref="AdkMappingException">  thrown if an error occurs parsing any
        /// <c>&lt;mappings&gt;</c> elements
        /// </exception>
        /// <exception cref="AdkConfigException">  thrown if the configuration file fails
        /// to parse or is not valid when validation is turned on
        /// 
        /// </exception>
        /// <seealso cref="Document">
        /// </seealso>
        public virtual XmlDocument Read( string file,
                                         bool validate )
        {
            if ( !Adk.Initialized ) {
                throw new AdkException( "Adk is not initialized", null );
            }

            try {
                fSrc = new FileInfo( file );
                // TODO: Implement validation if the validate flag is true
                if ( validate ) {
                    throw new NotImplementedException
                        ( "Document validation is not yet implemented" );
                }
                else {
                    fDoc = new XmlDocument();
                    fDoc.Load( fSrc.FullName );
                }
            }
            catch ( Exception ex ) {
                throw new AdkConfigException( ex.Message, ex );
            }

            //  Build the Mappings object
            fMappings = new Mappings();
            fMappings.Populate( fDoc, RootNode );
            return fDoc;
        }

        /// <summary>  Saves the XML document back to the file from which it was read.</summary>
        /// <exception cref="IOException">  thrown if an error occurs writing the document
        /// </exception>
        public virtual void Save()
        {
            Document.Save( fSrc.FullName );
        }

        /// <summary>  Saves the XML document to the specified output stream.</summary>
        /// <param name="outStream">The OutputStream to which the XML document is written
        /// </param>
        /// <exception cref="IOException">  thrown if an error occurs writing the document
        /// </exception>
        public virtual void Save( Stream outStream )
        {
            try {
                Document.Save( outStream );
            }
            catch ( Exception cnfe ) {
                throw new AdkConfigException( cnfe.Message, cnfe );
            }
        }

        /// <summary>  Saves the XML document to the specified Writer.</summary>
        /// <param name="outStream">The Writer to which the XML document is written
        /// </param>
        /// <exception cref="IOException">  thrown if an error occurs writing the document
        /// </exception>
        public virtual void Save( StreamWriter outStream )
        {
            try {
                Document.Save( outStream );
            }
            catch ( Exception cnfe ) {
                throw new AdkConfigException( cnfe.Message, cnfe );
            }
        }


        /// <summary>  Applies the settings in the configuration to the Agent. This method
        /// should be called at most once during agent startup, usually from
        /// <coce>Agent.initialize</c>
        /// 
        /// <ul>
        /// <li>
        /// For each <c>&lt;property&gt;</c> element, the associated
        /// property is set in the agent properties.
        /// </li>
        /// <li>
        /// A Zone instance is created for each <c>&lt;zone&gt;</c>
        /// element. Any <c>&lt;property&gt</c> elements defined for
        /// the zone are set in the zone's AgentProperties object. An array
        /// of all zones created is returned. The caller can then join those
        /// zones to topics.
        /// </li>
        /// </ul>
        /// 
        /// </summary>
        /// <param name="agent">The Agent to apply the configuration settings to
        /// 
        /// </param>
        /// <param name="overwrite">When true, properties defined in the configuration file
        /// replace properties of the same name that are already defined in the
        /// agent properties
        /// </param>
        public virtual IZone [] Apply( Agent agent,
                                       bool overwrite )
        {
            ApplyProperties( agent, overwrite );
            ApplyTransports( agent, overwrite );

            agent.ConfigurationSource = this;

            return ApplyZones( agent );
        }

        /// <summary>  Applies <c>&lt;property&gt;</c> elements to the Agent
        /// 
        /// </summary>
        /// <param name="agent">The Agent to apply properties to
        /// </param>
        /// <param name="overwrite">When true, properties defined in the configuration file
        /// replace properties of the same name that are already defined in the
        /// agent properties
        /// </param>
        public virtual void ApplyProperties( Agent agent,
                                             bool overwrite )
        {
            //  Set the agent's ID if specified in the configuration
            string sourceId = SourceId;
            if ( (Object) sourceId != null ) {
                agent.Id = sourceId;
            }

            //  Apply root-level properties to the agent's AgentProperties
            GetAgentProperties( agent.Properties );
        }

        /// <summary>  Applies <c>&lt;transport&gt;</c> elements to the Agent.
        /// 
        /// This method selects the <c>&lt;transport&gt;</c> child of the root
        /// <c>&lt;agent&gt;</c> with the <i>enabled</i> attribute set to true.
        /// More than one transport may be enabled; if more than one is enabled the last
        /// definition is considered the agent's default transport protocol. The
        /// <c>&lt;property&gt;</c> elements are then assigned to the agent's
        /// default transport properties for this protocol.
        /// 
        /// 
        /// </summary>
        /// <param name="agent">The Agent to apply the transport properties to
        /// </param>
        /// <param name="overwrite">When true, properties defined in the configuration file
        /// replace properties of the same name that are already defined in the
        /// agent's default transport properties
        /// </param>
        public virtual void ApplyTransports( Agent agent,
                                             bool overwrite )
        {
            //
            //  Enumerate all <transport> elements of the root; ignore elements
            //  where the 'enabled' attribute is set to false
            //
            if ( RootNode != null ) {
                foreach ( XmlElement n in RootNode.SelectNodes( XmlConstants.TRANSPORT_ELEMENT ) ) {
                    string proto = n.GetAttribute( XmlConstants.PROTOCOL );

                    //  Get default properties for this transport protocol
                    TransportProperties tp = agent.GetDefaultTransportProperties( proto );
                    if ( tp == null ) {
                        throw new AdkConfigException( "Transport protocol not supported: " + proto );
                    }
                    Boolean isEnabled = XmlUtils.IsElementEnabled( n );
                    if (isEnabled)
                    {
                        String prev = agent.Properties.GetProperty( AgentProperties.PROP_MESSAGING_TRANSPORT );
                        if (prev == null)
                        {
                            if ((Adk.Debug & AdkDebugFlags.Properties) != 0)
                            {
                                Agent.Log.Info("Configuration file selecting " + proto.ToUpperInvariant() + " as the default transport protocol");
                            }

                            agent.Properties.TransportProtocol = proto;
                        }
                    }
                    tp.Enabled = isEnabled;

                    //
                    //  Enumerate all <property> elements of this <transport> ...
                    //
                    foreach ( XmlElement prop in n.SelectNodes( AdkXmlConstants.Property.ELEMENT ) ) {
                        string nam = prop.GetAttribute( AdkXmlConstants.Property.NAME );
                        string val = prop.GetAttribute( AdkXmlConstants.Property.VALUE );

                        if ( tp.Contains( nam ) ) {
                            if ( overwrite ) {
                                if ( (Adk.Debug & AdkDebugFlags.Properties) != 0 ) {
                                    Agent.Log.Debug
                                        ( "Configuration file overwriting " + proto.ToUpper() +
                                          " transport property: " + nam + " = " + val );
                                }
                                tp[nam] = val;
                            }
                        }
                        else {
                            if ( (Adk.Debug & AdkDebugFlags.Properties) != 0 ) {
                                Agent.Log.Debug
                                    ( "Setting " + proto.ToUpper() +
                                      " transport property from configuration file: " + nam + " = " +
                                      val );
                            }

                            tp[nam] = val;
                        }
                    }
                }
            }
        }

        /// <summary>  Applies <c>&lt;zone&gt;</c> elements to create new Zone instances.
        /// The caller is responsible for setting up topics, connecting to zones,
        /// and joining zones to topics.
        /// </summary>
        public virtual IZone [] ApplyZones( Agent agent )
        {
            IZoneFactory zf = agent.ZoneFactory;

            XmlElement [] zones = ZoneNodes;

            for ( int i = 0; i < zones.Length; i++ ) {
                //  Get any properties defined for this zone. Zone properties
                //  should inherit from the agent's AgentProperties object, so
                //  pass that object as the parent
                AgentProperties props = new AgentProperties( agent.Properties );
                GetZoneProperties( props, zones[i] );

                string zoneId = zones[i].GetAttribute( XmlConstants.ID );
                if ( zoneId.Trim().Length == 0 ) {
                    throw new AdkConfigException( "<zone> cannot have an empty id attribute" );
                }
                string zoneUrl = zones[i].GetAttribute( XmlConstants.URL );
                if ( zoneUrl.Trim().Length == 0 ) {
                    throw new AdkConfigException( "<zone> cannot have an empty url attribute" );
                }

                //  Ask ZoneFactory to create a Zone instance
                zf.GetInstance( zoneId, zoneUrl, props );
            }

            return agent.ZoneFactory.GetAllZones();
        }

        /// <summary>  Adds a new zone</summary>
        /// <param name="zoneId">The zone ID
        /// </param>
        /// <param name="zoneUrl">The zone URL
        /// </param>
        /// <param name="templateId">The ID of the zone template
        /// </param>
        public virtual XmlElement AddZone( string zoneId,
                                           string zoneUrl,
                                           string templateId )
        {
            if ( zoneId == null || zoneId.Trim().Length == 0 ) {
                throw new AdkConfigException( "Zone ID cannot be blank" );
            }

            XmlElement zone = GetZoneNode( zoneId );
            if ( zone != null ) {
                throw new AdkConfigException( "Zone already defined" );
            }

            //  Create a new <zone> element
            zone = Document.CreateElement( XmlConstants.ZONE_ELEMENT );
            RootNode.AppendChild( zone );
            zone.SetAttribute( XmlConstants.ID, zoneId );
            if ( zoneUrl != null ) {
                zone.SetAttribute( XmlConstants.URL, zoneUrl );
            }
            zone.SetAttribute( XmlConstants.TEMPLATE, templateId == null ? "Default" : templateId );

            return zone;
        }

        /// <summary>  Deletes a zone</summary>
        /// <param name="zoneId">The zone ID
        /// </param>
        public virtual void DeleteZone( string zoneId )
        {
            if ( zoneId == null ) {
                return;
            }

            XmlElement zone = GetZoneNode( zoneId );
            if ( zone != null ) {
                zone.ParentNode.RemoveChild( zone );
            }
        }

        /// <summary>  Populates an AgentProperties object with all <c>&lt;property&gt;</c>
        /// values defined for the root <c>&lt;agent&gt;</c> element.
        /// 
        /// Properties defined at the root level are applied to the AgentProperties
        /// object of the Agent class. These serve as global defaults to all zones.
        /// Use the getTemplateProperties method to obtain properties for a zone
        /// template, which apply to all zones that reference that template, or the
        /// GetZoneProperties method to obtain properties specific to a zone.
        /// 
        /// 
        /// </summary>
        /// <returns> An AgentProperties object populated with all <c>&lt;property&gt;</c>
        /// values defined for the root <c>&lt;agent&gt;</c> element
        /// </returns>
        public virtual void GetAgentProperties( AgentProperties props )
        {
            PopulateProperties( props, RootNode );
        }

        /// <summary>  Sets the value of a <c>&lt;property&gt;</c> child of the root
        /// <c>&lt;agent&gt;</c> node. If a <c>&lt;property&gt;</c>
        /// element already exists, its value is updated; otherwise a new element is
        /// appended to the root <c>&lt;agent&gt;</c> node.
        /// 
        /// </summary>
        /// <param name="property">The name of the property
        /// </param>
        /// <param name="val">The property value
        /// </param>
        public virtual void SetAgentProperty( string property,
                                              string val )
        {
            SetProperty( RootNode, property, val );
        }

        /// <summary>  Populates a Properties object with all <c>&lt;property&gt;</c>
        /// values defined by a <c>&lt;zone&gt;</c> node as well as all
        /// properties defined by the referenced zone template. Properties defined
        /// by the <c>&lt;zone&gt;</c> override properties defined by the
        /// template.
        /// </summary>
        public virtual void GetZoneProperties( IPropertyCollection props,
                                               string zone )
        {
            GetZoneProperties( props, GetZoneNode( zone ) );
        }

        /// <summary>  Populates a Properties object with all <c>&lt;property&gt;</c>
        /// values defined by a <c>&lt;zone&gt;</c> node as well as all
        /// properties defined by the referenced zone template. Properties defined
        /// by the <c>&lt;zone&gt;</c> override properties defined by the
        /// template.
        /// </summary>
        public virtual void GetZoneProperties( IPropertyCollection props,
                                               XmlElement zone )
        {
            PopulateProperties( props, zone );

            string template = zone.GetAttribute( XmlConstants.TEMPLATE );
            if ( template.Length > 0 ) {
                GetZoneTemplateProperties( props, template );
            }
        }

        /// <summary>  Gets all <c>&lt;property&gt;</c> values defined for a
        /// <c>&lt;zone&gt;</c> node.
        /// </summary>
        public virtual XmlElement [] GetZonePropertyNodes( string zoneId )
        {
            return GetPropertyNodes( GetZoneNode( zoneId ) );
        }

        /// <summary>  Gets all <c>&lt;property&gt;</c> values defined for a
        /// <c>&lt;zone&gt;</c> node.
        /// </summary>
        public virtual XmlElement [] GetZonePropertyNodes( XmlElement zone )
        {
            return GetPropertyNodes( zone );
        }

        /// <summary>  Sets the value of a <c>&lt;property&gt;</c> child of the specified
        /// <c>&lt;zone&gt;</c> node. If a <c>&lt;property&gt;</c> child
        /// already exists with the same name, its value is updated; otherwise a new
        /// element is appended to the <c>&lt;zone&gt;</c> node.
        /// 
        /// </summary>
        /// <param name="zoneId">The zone ID
        /// </param>
        /// <param name="property">The name of the property
        /// </param>
        /// <param name="val">The property value
        /// </param>
        public virtual void SetZoneProperty( string zoneId,
                                             string property,
                                             string val )
        {
            XmlElement zone = GetZoneNode( zoneId );
            if ( zone != null ) {
                SetProperty( zone, property, val );
            }
        }

        /// <summary>  Gets the value of a <c>&lt;property&gt;</c> child of the specified
        /// <c>&lt;zone&gt;</c> node.
        /// 
        /// </summary>
        /// <param name="zoneId">The zone ID
        /// </param>
        /// <param name="property">The name of the property
        /// </param>
        /// <param name="defaultValue">The value that will be returned if the property is
        /// not defined
        /// </param>
        public virtual string GetZoneProperty( string zoneId,
                                               string property,
                                               string defaultValue )
        {
            XmlElement n = GetZoneNode( zoneId );
            if ( n != null ) {
                return GetProperty( n, property, defaultValue );
            }

            return defaultValue;
        }

        /// <summary>  Sets the value of a <c>&lt;zone&gt;</c> node attribute.
        /// 
        /// </summary>
        /// <param name="zoneId">The zone ID
        /// </param>
        /// <param name="attr">The name of the attribute
        /// </param>
        /// <param name="val">The attribute value
        /// </param>
        public virtual void SetZoneAttribute( string zoneId,
                                              string attr,
                                              string val )
        {
            XmlElement zone = GetZoneNode( zoneId );
            if ( zone != null ) {
                zone.SetAttribute( attr, val );
            }
        }

        /// <summary>  Gets the value of a <c>&lt;zone&gt;</c> node attribute.
        /// 
        /// </summary>
        /// <param name="zoneId">The zone ID
        /// </param>
        /// <param name="attr">The name of the attribute
        /// </param>
        /// <param name="defaultValue">The value that will be returned if the attribute is
        /// not defined
        /// </param>
        public virtual string GetZoneAttribute( string zoneId,
                                                string attr,
                                                string defaultValue )
        {
            string s = null;
            XmlElement zone = GetZoneNode( zoneId );
            if ( zone != null ) {
                s = zone.GetAttribute( attr );
            }
            return s.Length == 0 ? defaultValue : s;
        }

        /// <summary>  Populates a Properties object with all <c>&lt;property&gt;</c>
        /// values defined for a <c>&lt;template&gt;</c> node.
        /// </summary>
        public virtual void GetZoneTemplateProperties( IPropertyCollection props,
                                                       string template )
        {
            PopulateProperties( props, GetZoneTemplateNode( template ) );
        }

        /// <summary>  Populates a Properties object with all <c>&lt;property&gt;</c>
        /// values defined for a <c>&lt;template&gt;</c> node.
        /// </summary>
        public virtual void GetZoneTemplateProperties( IPropertyCollection props,
                                                       XmlElement template )
        {
            PopulateProperties( props, template );
        }

        /// <summary>  Gets a zone template property.</summary>
        /// <param name="templateId">The ID of the <c>&lt;template&gt;</c>
        /// </param>
        /// <param name="property">The name of the <c>&lt;property&gt;</c>
        /// </param>
        /// <param name="defValue">The default value to return if the property is not defined
        /// </param>
        public virtual string GetZoneTemplateProperty( string templateId,
                                                       string property,
                                                       string defValue )
        {
            XmlElement n = GetZoneTemplateNode( templateId );
            if ( n != null ) {
                return GetProperty( n, property, defValue );
            }

            return defValue;
        }

        /// <summary>  Gets a zone template property node.</summary>
        /// <param name="templateId">The ID of the <c>&lt;template&gt;</c>
        /// </param>
        /// <param name="property">The name of the <c>&lt;property&gt;</c>
        /// </param>
        /// <returns> the DOM XmlElement if found
        /// </returns>
        public virtual XmlElement GetZoneTemplatePropertyNode( string templateId,
                                                               string property )
        {
            XmlElement n = GetZoneTemplateNode( templateId );
            if ( n != null ) {
                return GetPropertyNode( n, property );
            }

            return null;
        }

        /// <summary>  Gets all <c>&lt;property&gt;</c> values defined for a <c>&lt;zone&gt;</c> node.</summary>
        public virtual XmlElement [] GetZoneTemplatePropertyNodes( string templateId )
        {
            XmlElement [] templates = GetZoneTemplates( false );

            for ( int i = 0; i < templates.Length; i++ ) {
                string id = templates[i].GetAttribute( XmlConstants.ID );
                if ( id == templateId ) {
                    return GetPropertyNodes( templates[i] );
                }
            }
            return null;
        }

        /// <summary>  Gets all <c>&lt;property&gt;</c> values defined for a <c>&lt;zone&gt;</c> node.</summary>
        public virtual XmlElement [] GetZoneTemplatePropertyNodes( XmlElement template )
        {
            return GetPropertyNodes( template );
        }

        /// <summary>  Sets the value of a <c>&lt;property&gt;</c> child of the specified
        /// <c>&lt;zone&gt;</c> node. If a <c>&lt;property&gt;</c> child
        /// already exists with the same name, its value is updated; otherwise a new
        /// element is appended to the <c>&lt;template&gt;</c> node.
        /// 
        /// </summary>
        /// <param name="template">The zone template ID
        /// </param>
        /// <param name="property">The name of the property
        /// </param>
        /// <param name="val">The property value
        /// </param>
        public virtual void SetZoneTemplateProperty( string template,
                                                     string property,
                                                     string val )
        {
            XmlElement temp = GetZoneTemplateNode( template );
            if ( temp == null ) {
                temp = AddZoneTemplateNode( template );
            }
            SetProperty( temp, property, val );
        }

        /// <summary>  Deletes a property.</summary>
        /// <param name="property">The name of the <c>&lt;property&gt;</c>
        /// </param>
        public virtual void DeleteProperty( string property )
        {
            XmlElement prn = GetPropertyNode( RootNode, property );
            if ( prn != null ) {
                prn.ParentNode.RemoveChild( prn );
            }
        }

        /// <summary>  Deletes a zone property.</summary>
        /// <param name="zoneId">The ID of the <c>&lt;zone&gt;</c>
        /// </param>
        /// <param name="property">The name of the <c>&lt;property&gt;</c>
        /// </param>
        public virtual void DeleteZoneProperty( string zoneId,
                                                string property )
        {
            XmlElement n = GetZoneNode( zoneId );
            if ( n != null ) {
                XmlElement prn = GetPropertyNode( n, property );
                if ( prn != null ) {
                    prn.ParentNode.RemoveChild( prn );
                }
            }
        }

        /// <summary>  Deletes a zone template property.</summary>
        /// <param name="templateId">The ID of the <c>&lt;template&gt;</c>
        /// </param>
        /// <param name="property">The name of the <c>&lt;property&gt;</c>
        /// </param>
        public virtual void DeleteZoneTemplateProperty( string templateId,
                                                        string property )
        {
            XmlElement n = GetZoneTemplateNode( templateId );
            if ( n != null ) {
                XmlElement prn = GetPropertyNode( n, property );
                if ( prn != null ) {
                    prn.ParentNode.RemoveChild( prn );
                }
            }
        }


        /// <summary>  Populates a Properties object with all <c>&lt;property&gt;</c>
        /// values defined as children of the specified node. Properties that already
        /// exist in the Properties object are not overwritten by properties defined
        /// by the node.
        /// </summary>
        public virtual void PopulateProperties( IPropertyCollection props,
                                                XmlElement node )
        {
            PopulateProperties( props, node, false );
        }

        /// <summary>  Populates a Properties object with all <c>&lt;property&gt;</c>
        /// values defined as children of the specified node. Properties that already
        /// exist in the Properties object are optionally overwritten with properties
        /// defined by the node.
        /// 
        /// </summary>
        /// <param name="props">The Properties object to populate
        /// </param>
        /// <param name="node">The XmlElement to search for child <c>&lt;property&gt;</c> elements
        /// </param>
        /// <param name="replace">true to replace the values of properties already defined
        /// in the <c>props</c> object
        /// </param>
        public virtual void PopulateProperties( IPropertyCollection props,
                                                XmlElement node,
                                                bool replace )
        {
            if ( node != null ) {
                XmlNodeList propertyList = node.SelectNodes( AdkXmlConstants.Property.ELEMENT );
                // Return only the "enabled" nodes
                foreach ( XmlElement n in new XmlUtils.FilteredElementList( propertyList ) ) {
                    string nam = n.GetAttribute( AdkXmlConstants.Property.NAME );
                    string val = n.GetAttribute( AdkXmlConstants.Property.VALUE );
                    if ( nam.Length > 0 && val.Length > 0 && (replace || !props.Contains( nam )) ) {
                        props[nam] = val;
                    }
                }
            }
        }

        /// <summary>  Sets the value of a <c>&lt;property&gt;</c> child of the specified
        /// node. If a <c>&lt;property&gt;</c> element already exists, its value
        /// is updated; otherwise a new element is appended to the node.
        /// 
        /// </summary>
        /// <param name="node">The parent node of the property
        /// </param>
        /// <param name="property">The name of the property
        /// </param>
        /// <param name="val">The property value
        /// </param>
        public virtual void SetProperty( XmlElement node,
                                         string property,
                                         string val )
        {
            XmlUtils.SetProperty( node, property, val );
        }

        public virtual void SetProperty( XmlElement node,
                                         string elementName,
                                         string property,
                                         string val )
        {
            XmlElement element = node[elementName];
            if ( element == null ) {
                element = node.OwnerDocument.CreateElement( elementName );
                node.AppendChild( element );
            }
            SetProperty( element, property, val );
        }

        /// <summary>Gets the value of a <c>&lt;property&gt;</c> element defined by a node.</summary>
        /// <param name="node">The parent node to search</param>
        /// <param name="property">The name of the property</param>
        /// <param name="defaultValue">The value to return if the property is not found</param>
        /// <returns>The property value</returns>
        public virtual string GetProperty( XmlElement node,
                                           string property,
                                           string defaultValue )
        {
            string val = defaultValue;
            // Both node lists need to be filtered for the "enabled" attribute
            // so we will use the XmlFilteredElementEnumerator;
            foreach (
                XmlElement ch in
                    new XmlUtils.FilteredElementList
                        ( node.SelectNodes( AdkXmlConstants.Property.ELEMENT ) ) ) {
                if ( ch.GetAttribute( AdkXmlConstants.Property.NAME ) == property ) {
                    val = ch.GetAttribute( AdkXmlConstants.Property.VALUE );
                    break;
                }
            }

            return val;
        }


        /// <summary>
        /// Gets the bolean value of a <c>&lt;property&gt;</c> element defined by a node. A value of
        /// "True" or "Yes" will return <c>true</c>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual bool GetProperty( XmlElement node,
                                         string propertyName,
                                         bool defaultValue )
        {
            string propValue = GetProperty( node, propertyName, null );
            if ( propValue == null ) {
                return defaultValue;
            }
            else {
                return (string.Compare( propValue, "true", true ) == 0 ||
                        string.Compare( propValue, "yes", true ) == 0);
            }
        }

        /// <summary>  Gets a <c>&lt;property&gt;</c> DOM XmlElement
        /// 
        /// </summary>
        /// <param name="parentNode">The parent node to search
        /// </param>
        /// <param name="property">The name of the property
        /// 
        /// </param>
        /// <returns> The DOM XmlElement if the property is found, otherwise <c>null</c>
        /// </returns>
        public virtual XmlElement GetPropertyNode( XmlElement parentNode,
                                                   string property )
        {
            return
                XmlUtils.GetElementByAttribute
                    ( parentNode, AdkXmlConstants.Property.ELEMENT, AdkXmlConstants.Property.NAME,
                      property, true );
        }

        /// <summary>  A convenience function to get the value of a <c>&lt;property&gt;</c>
        /// defined by the first child element of the specified node that has a
        /// matching tag name. This routine is often used to obtain properties that
        /// are organized into named groups (e.g. <c>&lt;database-settings&gt;</c>,
        /// <c>&lt;transport-settings&gt;</c>, etc.)
        /// 
        /// For example, to lookup a property named "user" of a top-level
        /// <c>&lt;database-settings&gt;</c> element,
        /// 
        /// <c>GetProperty( getRootNode(), "database-settings", "user", "sa" );</c>
        /// 
        /// 
        /// </summary>
        /// <param name="node">The parent node to search
        /// </param>
        /// <param name="element">The tag name of the element to search for (it must be a
        /// non-repeating child of the parent node)
        /// </param>
        /// <param name="property">The name of the property
        /// </param>
        /// <param name="defaultValue">The value to return if the property is not found
        /// 
        /// </param>
        /// <returns> The property value
        /// </returns>
        public virtual string GetProperty( XmlElement node,
                                           string element,
                                           string property,
                                           string defaultValue )
        {
            XmlElement propertyParent = node[element];
            if ( propertyParent == null ) {
                return defaultValue;
            }
            else {
                return GetProperty( propertyParent, property, defaultValue );
            }
        }


        /// <summary>
        /// Returns True if the specified property node has text that equals "True" or "Yes", ignoring case
        /// </summary>
        /// <param name="node">The parent node</param>
        /// <param name="element">The element name</param>
        /// <param name="property">The property name</param>
        /// <param name="defaultValue">The default value to return if the property does not exist</param>
        /// <returns></returns>
        public virtual bool GetProperty( XmlElement node,
                                         string element,
                                         string property,
                                         bool defaultValue )
        {
            XmlElement propertyParent = node[element];
            if ( propertyParent == null ) {
                return defaultValue;
            }
            else {
                return GetProperty( propertyParent, property, defaultValue );
            }
        }

        /// <summary>  Gets all <c>&lt;property&gt;</c> child nodes of an element</summary>
        public virtual XmlElement [] GetPropertyNodes( XmlElement node )
        {
            return
                XmlUtils.ElementArrayFromNodeList
                    ( node.SelectNodes( AdkXmlConstants.Property.ELEMENT ), false );
        }

        /// <summary>  Gets the <c>&lt;zone&gt;</c> element with the specified ID</summary>
        public virtual XmlElement GetZoneNode( string zoneId )
        {
            return
                XmlUtils.GetElementByAttribute
                    ( RootNode, XmlConstants.ZONE_ELEMENT, XmlConstants.ID, zoneId, false );
        }

        /// <summary>  Gets an array of all zones templates.</summary>
        /// <returns> An array of DOM Nodes comprised of <c>&lt;template&gt;</c>
        /// elements defined as children of the root <c>&lt;agent&gt;</c> node
        /// </returns>
        public virtual XmlElement [] GetZoneTemplates( bool filtered )
        {
            XmlNodeList templatesList = RootNode.SelectNodes( XmlConstants.TEMPLATE );
            return XmlUtils.ElementArrayFromNodeList( templatesList, filtered );
        }

        /// <summary>  Gets a zone template by ID.</summary>
        /// <param name="templateId">The ID of the template to return
        /// </param>
        /// <returns> The DOM XmlElement encapsulating the <c>&lt;template&gt;</c>
        /// element with the specified ID
        /// </returns>
        public virtual XmlElement GetZoneTemplateNode( string templateId )
        {
            return
                XmlUtils.GetElementByAttribute
                    ( RootNode, XmlConstants.TEMPLATE, XmlConstants.ID, templateId, false );
        }

        /// <summary>  Add a &lt;template&gt; node for the specified zone template.</summary>
        /// <returns> The newly created node, or if a zone template node already
        /// exists with this templateId, a reference to it is returned
        /// </returns>
        public virtual XmlElement AddZoneTemplateNode( string templateId )
        {
            XmlElement n = GetZoneTemplateNode( templateId );
            if ( n == null ) {
                //  Create and append a new template node
                XmlElement root = RootNode;
                n = root.OwnerDocument.CreateElement( XmlConstants.TEMPLATE );
                n.SetAttribute( XmlConstants.ID, templateId );
                root.AppendChild( n );
            }
            return n;
        }

        /// <summary>  Gets a &lt;transport&gt; node.</summary>
        /// <param name="protocol">The protocol type (e.g. "http", "https", etc.)
        /// </param>
        /// <returns> The DOM XmlElement encapsulating the <c>&lt;transport&gt;</c> element
        /// </returns>
        public virtual XmlElement GetTransportNode( string protocol )
        {
            return
                XmlUtils.GetElementByAttribute
                    ( RootNode, XmlConstants.TRANSPORT_ELEMENT, XmlConstants.PROTOCOL, protocol,
                      false );
        }

        /// <summary>  Add a &lt;transport&gt; node for the specified transport protocol.</summary>
        /// <returns> The newly created node, or if a transport node already exists
        /// for this protocol, a reference to it is returned
        /// </returns>
        public virtual XmlElement AddTransportNode( string protocol )
        {
            XmlElement n = GetTransportNode( protocol );

            if ( n == null ) {
                //  Create and append a new protocol node
                XmlElement root = RootNode;
                n = root.OwnerDocument.CreateElement( XmlConstants.TRANSPORT_ELEMENT );
                n.SetAttribute( XmlConstants.PROTOCOL, protocol );
                root.AppendChild( n );
            }

            return n;
        }

        private sealed class XmlConstants
        {
            public const string ROOT_ELEMENT = "agent";
            public const string TRANSPORT_ELEMENT = "transport";
            public const string ZONE_ELEMENT = "zone";

            public const string PROTOCOL = "protocol";
            public const string ID = "id";
            public const string URL = "url";
            public const string TEMPLATE = "template";
        }
    }
}
