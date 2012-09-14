//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using OpenADK.Library.Global;
using OpenADK.Library.Tools.Mapping;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library
{
    /// <summary>  The abstract base class for all root-level SIF Data Object classes.
    /// SifDataObject encapsulates top-level data objects defined by SIF Working
    /// Groups, including <c>&lt;StudentPersonal&gt;</c>,
    /// <c>&lt;LibraryPatronStatus&gt;</c>, <c>&lt;BusInfo&gt;</c>,
    /// and so on.
    /// 
    /// 
    /// <b>Setting Elements &amp; Attributes of a SIF Data Object</b>
    /// 
    /// There are two general approaches to getting and setting the element/attribute
    /// values of a SifDataObject. First, you can call the getXxx and setXxx methods
    /// of the subclass to manipulate the elements and attributes in an object-oriented
    /// fashion. For example, to assign a first and last name to a StudentPersonal
    /// object, create a Name object and attach it to the StudentPersonal with the
    /// setName method:
    /// 
    /// <c>&nbsp;&nbsp;&nbsp;&nbsp;// Build a StudentPersonal object<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;StudentPersonal sp = new StudentPersonal();<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;sp.setRefId( Adk.makeGUID() );<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;sp.setName( new Name( "Davis", "Alan" ) );</br>
    /// </c>
    /// 
    /// The second approach to getting and setting element/attribute values is to
    /// call the <c>setElementOrAttribute</c> and <c>getElementOrAttribute</c>
    /// methods, which accept an XPath-like query string that identifies a specific
    /// SIF element or attribute relative to the SifDataObject. (See also the
    /// Mappings class for a higher-level mechanism that performs much of the work
    /// involved in dynamically mapping application fields to SIF elements and
    /// attributes).
    /// 
    /// <c>&nbsp;&nbsp;&nbsp;&nbsp;// Build a StudentPersonal object<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;StudentPersonal sp = new StudentPersonal();<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;sp.setRefId( Adk.makeGUID() );<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;sp.setElementOrAttribute( "Name[@Type='02']/LastName", "Davis", null );<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;sp.setElementOrAttribute( "Name[@Type='02']/FirstName", "Brian", null );<br/>
    /// </c>
    /// 
    /// XPath-like query strings can include substitution tokens and can even call
    /// static .Net methods. For example, the following uses name/value pairs defined
    /// in a Map to select the first and last name. The static <c>capitalize</c>
    /// method of the "MyFunctions" class is called to capitalize the last name:
    /// 
    /// 
    /// <c>&nbsp;&nbsp;&nbsp;&nbsp;// Prepare a table with field values<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;HashMap values = new HashMap();<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;values.put( "LASTNAME", "Davis" );<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;values.put( "FIRSTNAME", "Brian" );<br/><br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;// Build a StudentPersonal object<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;StudentPersonal sp = new StudentPersonal();<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;sp.setRefId( Adk.makeGUID() );<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;sp.setElementOrAttribute( "Name[@Type='02']/LastName=@MyFunctions.capitalize( $(LASTNAME) )", null, values );<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;sp.setElementOrAttribute( "Name[@Type='02']/FirstName=$(FIRSTNAME)", null, values );<br/>
    /// </c>
    /// 
    /// <b>Object Type</b>
    /// 
    /// The <c>getObjectType</c> method returns the an ElementDef constant
    /// from the SifDtd class that identifies the SIF Data Object. The the
    /// <c>getObjectTag</c> convenience method returns the element tag name of
    /// the object for the version of SIF associated with the instance. For example,
    /// 
    /// 
    /// <c>
    /// &nbsp;&nbsp;&nbsp;&nbsp;// Lookup a Topic instance<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;SifDataObject data = new SifDataObject( Adk.Dtd().STUDENTPERSONAL );<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;TopicFactory factory = myAgent.getTopicFactory();<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;Topic t = factory.getInstance( data.getObjectType() );<br/>
    /// </c>
    /// 
    /// 
    /// <b>SIF Version</b>
    /// 
    /// Each SifDataObject is associated with a SifVersion instance. The version
    /// is used by the SifWriter class when rendering the object as XML. By default,
    /// it is assumed to be the version of SIF in effect for this agent; that is,
    /// the value passed to the <c>Adk.initialize</c> method. However, to
    /// support mixed environments where an agent may send and receive objects
    /// using different versions of SIF, the version may be changed by the Adk
    /// during message processing:
    /// 
    /// <ul>
    /// <li>
    /// When constructing a SifDataObject instance from parsed XML content,
    /// the SifParser class sets its SifVersion to the version identified by
    /// the <i>xmlns</i> attribute of the <c>&lt;SIF_Message&gt;</c>
    /// envelope.<br/><br/>
    /// </li>
    /// <li>
    /// The version may be changed by the Adk prior to rendering a
    /// SifDataObject as XML. For example, when your agent responds to a
    /// &lt;SIF_Request&gt; message that specifically identifies a version
    /// to use for the results, the Adk will change the version of the
    /// SifDataObject when generating &lt;SIF_Response&gt; messages. Once
    /// messages have been generated, it restores the SifVersion to its
    /// original setting.
    /// <br/><br/>
    /// </li>
    /// <li>
    /// An agent may manually change the SifVersion associated with a
    /// SifDataObject by calling the <c>setSIFVersion</c> method.
    /// </li>
    /// </ul>
    /// 
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public abstract class SifDataObject : SifKeyedElement //, ISerializable
    {
        /// <summary>  Constructs a SifDataObject</summary>
        /// <param name="version">The version of SIF that should be used to render this
        /// SifDataObject and its child elements. If this SifDataObject is the
        /// result of parsing a SIF message, this is the version of SIF
        /// identified by the message envelope.
        /// 
        /// </param>
        /// <param name="def">The IElementDef that provides metadata for this element
        /// </param>
        public SifDataObject( SifVersion version,
                              IElementDef def )
            : base( def )
        {
            fVersion = version;
        }


        /// <summary>
        /// This object does not define a key field
        /// </summary>
        /// <returns></returns>
        public override IElementDef[] KeyFields
        {
            get { return new IElementDef[0]; }
        }

        /// <value> A SifVersion
        /// </value>
        /// <summary>  Changes the version of SIF that should be used to render this SifDataObject
        /// and its children. The calling thread may change the way a SifDataObject
        /// is rendered by calling this method. It is recommended the version be
        /// restored to its original value after rendering is completed.
        /// </summary>
        public override SifVersion SifVersion
        {
            get { return fVersion; }

            set { fVersion = value; }
        }

        /// <summary>  Gets the ElementDef that identifies this SIF Data Object type</summary>
        /// <returns> An ElementDef constant defined by the SifDtd class
        /// </returns>
        public virtual IElementDef ObjectType
        {
            get { return ElementDef; }
        }

        /// <summary>  Gets the element tag name of this object</summary>
        /// <returns> The element tag for the version of SIF associated with the object
        /// </returns>
        public virtual string ObjectTag
        {
            get { return ElementDef.Tag( fVersion ); }
        }


        /// <summary>  Returns the XML representation of this SIF Data Object</summary>
        public virtual string ToXml()
        {
            using ( StringWriter buffer = new StringWriter() )
            {
                SifWriter w = new SifWriter( buffer );
                w.Write( this );
                w.Flush();
                return buffer.ToString();
            }
        }

        ///// <summary>Gets or Sets all SIF_ExtendedElements/SIF_ExtendedElement children of this object.</summary>
        ///// <value>An array of SIF_ExtendedElement instances. If no SIF_ExtendedElements child element was
        ///// found, an empty array is returned. Setting this property replaces all items in the list
        ///// </value>
        ///// <remarks>
        ///// Since Adk 1.5
        ///// </remarks>
        //public virtual SIF_ExtendedElement[] SIFExtendedElements
        //{
        //    get
        //    {
        //        if ( SifDtd.SIF_EXTENDEDELEMENTS == null )
        //        {
        //            return new SIF_ExtendedElement[0];
        //        }

        //        SIF_ExtendedElements container = ( SIF_ExtendedElements ) GetChild( SifDtd.SIF_EXTENDEDELEMENTS );
        //        if ( container == null )
        //        {
        //            return new SIF_ExtendedElement[0];
        //        }

        //        SifElement[] ch = container.GetChildren();
        //        SIF_ExtendedElement[] arr = new SIF_ExtendedElement[ch.Length];
        //        for ( int i = 0; i < arr.Length; i++ )
        //        {
        //            arr[ i ] = ( SIF_ExtendedElement ) ch[ i ];
        //        }

        //        return arr;
        //    }
        //    set
        //    {
        //        SIFExtendedElementsContainer.SetSIF_ExtendedElements( value );
        //    }

        //}

        /// <summary>  The version of SIF that should be used to render this SifDataObject and
        /// its child elements. If this SifDataObject is the result of parsing a SIF
        /// message, this is the version of SIF identified by the message envelope.
        /// The version is initially set by the constructor but may be changed at
        /// any time by the <c>setVersion</c> method.
        /// </summary>
        protected internal SifVersion fVersion;


        /// <summary>  Gets this object's <i>RefId</i>.
        /// 
        /// Most SIF Data Object elements define a RefId value to uniquely identify
        /// the object. However, some objects such as SIF_ZoneStatus and StudentMeal
        /// do not have a RefId. For these, a blank string will be returned.
        /// 
        /// 
        /// </summary>
        /// <returns> The value of this object's <i>RefId</i> element
        /// </returns>
        public virtual string RefId
        {
            get { return ""; }
            set
            {
                throw new AdkNotSupportedException
                    ( "Cannot set RefID on object of type " + GetType().FullName );
            }
        }

        /// <summary>Sets an element or attribute value identified by an XPath-like query string.</summary>
        /// <remarks>
        ///   NOTE: This method makes calls to SIFXPathContext. If multiple calls to
        ///  <c>setElementOrAttribute</c> are being done, it is much more efficient to create
        ///  a new <c>SIFXPathContext</c> by calling <c>SIFXPathContext.newInstance(sdo)</c> and then
        /// calling <c>.setElementorAttributeon</c> on that SifXPathContext instance
        /// </remarks>
        /// <param name="xpath">An XPath-like query string that identifies identifies
        /// the element or attribute to set. The string must reference elements
        /// and attributes by their <i>version-independent</i> names.
        /// </param>
        /// <param name="valu">The value of the element or attribute
        /// </param>
        public virtual void SetElementOrAttribute( string xpath,
                                                   string valu )
        {
            SifVersion = Adk.SifVersion;
            SifXPathContext spc = SifXPathContext.NewSIFContext( this );
            spc.SetElementOrAttribute( xpath, valu );
        }

        /// <summary>Sets an element or attribute value identified by an XPath-like query string.</summary>
        /// <remarks>
        ///   NOTE: This method makes calls to SIFXPathContext. If multiple calls to
        ///  <c>setElementOrAttribute</c> are being done, it is much more efficient to create
        ///  a new <c>SifXPathContext</c> by calling <c>SifXPathContext.NewInstance(sdo)</c> and then
        /// calling <c>.SetElementorAttributeon</c> on that SifXPathContext instance
        /// </remarks>
        /// <param name="xpath">An XPath-like query string that identifies the element or
        /// attribute to set. The string must reference elements and attributes
        /// by their <i>version-independent</i> names.
        /// </param>
        /// <param name="valu">The value to assign to the element or attribute if the
        /// query string does not set a value; may be null
        /// </param>
        /// <param name="adaptor"> A data source may be used for variable
        /// substitutions within the query string
        /// </param>
        public virtual void SetElementOrAttribute( string xpath,
                                                   string valu,
                                                   IFieldAdaptor adaptor )
        {
            SifVersion = Adk.SifVersion;
            SifXPathContext spc = SifXPathContext.NewSIFContext( this );
            if ( adaptor is IXPathVariableLibrary )
            {
                spc.AddVariables( "", (IXPathVariableLibrary) adaptor );
            }
            spc.SetElementOrAttribute( xpath, valu );
        }

        /// <summary>Sets an element or attribute value identified by an XPath-like query string.</summary>
        /// <remarks>
        ///   NOTE: This method makes calls to SIFXPathContext. If multiple calls to
        ///  <c>setElementOrAttribute</c> are being done, it is much more efficient to create
        ///  a new <c>SifXPathContext</c> by calling <c>SifXPathContext.NewInstance(sdo)</c> and then
        /// calling <c>.SetElementorAttributeon</c> on that SifXPathContext instance
        /// </remarks>
        /// <param name="xpath">An XPath-like query string that identifies the element or
        /// attribute to set. The string must reference elements and attributes
        /// by their <i>version-independent</i> names.
        /// </param>
        /// <param name="valu">The value to assign to the element or attribute if the
        /// query string does not set a value; may be null
        /// </param>
        /// <param name="valueBuilder">a ValueBuilder implementation that evaluates
        /// expressions in XPath-like query strings using name/value pairs in
        /// the <i>variables</i> map
        /// </param>
        public virtual void SetElementOrAttribute( string xpath,
                                                   string valu,
                                                   IValueBuilder valueBuilder )
        {
            valu = valueBuilder == null ? valu : valueBuilder.Evaluate( valu );
            SetElementOrAttribute( xpath, valu );
        }

        /// <summary>Gets an element or attribute value identified by an XPath-like query string.</summary>
        ///   NOTE: This method makes calls to SIFXPathContext. If multiple calls to
        ///  <c>GetElementOrAttribute</c> are being done, it is much more efficient to create
        ///  a new <c>SifXPathContext</c> by calling <c>SifXPathContext.NewInstance(sdo)</c> and then
        /// calling <c>.GetElementorAttributeon</c> on that SifXPathContext instance
        /// </remarks>
        /// <param name="xpath">An XPath-like query string that identifies the element or
        /// attribute to get. The string must reference elements and attributes
        /// by their <i>version-independent</i> names.
        /// </param>
        /// <returns> An Element instance encapsulating the element or attribute if
        /// found. If not found, <c>null</c> is returned. To retrieve the
        /// value of the Element, call its <c>getTextValue</c> method.
        /// </returns>
        public virtual Element GetElementOrAttribute( string xpath )
        {
            // XPath Navigation using this API causes the object to have to 
            // remember the SIF Version being evaluated
            if (fVersion == null)
            {
                fVersion = Adk.SifVersion;
            }
            SifXPathContext spc = SifXPathContext.NewSIFContext( this );
            return spc.GetElementOrAttribute( xpath );
        }


        //* Sets the SIF_ExtendedElements container for this object.<P>
        //* Normally, agents can just call {@link #addSIFExtendedElement(String, String)},
        //* which automatically creates a SIF_ExtendedElements container, if necessary and 
        //* allows for easy addition of SIF_ExtendedElements.<p>
        //* This method is provided as a convenience to agents that need more control or
        //* wish to set or completely replace the existing SIF_ExtendedElements container. 

        public void AddSifExtendedElementsContainer( SIF_ExtendedElements container )
        {
            RemoveChild( GlobalDTD.SIF_EXTENDEDELEMENTS );
            AddChild( container );
        }

        /**
       * Sets an array of <code>SIF_ExtendedElement</code> objects. All existing 
       * <code>SIF_ExtendedElement</code> instances 
       * are removed and replaced with this list. Calling this method with the 
       * parameter value set to null removes all <code>SIF_ExtendedElements</code>.
       * @param elements The SIF_Extended elements instances to set on this object
       *
       *  @since ADK 1.5
       */

        //public void SetSifExtendedElements(SIF_ExtendedElement[] elements)
        //{
        //   SIF_ExtendedElements = elements;
        //}


        /// <summary> 	Sets a SIF_ExtendedElement.</summary>
        /// <param name="name">The element name
        /// </param>
        /// <param name="valu">The element value
        /// 
        /// @since Adk 1.5
        /// </param>
        public virtual void AddSIFExtendedElement( string name, string valu )
        {
            if ( GlobalDTD.SIF_EXTENDEDELEMENTS == null || name == null || valu == null )
            {
                return;
            }

            SIF_ExtendedElement ele = null;

            //	Lookup existing SIF_ExtendedElements container
            SIF_ExtendedElements see = (SIF_ExtendedElements) GetChild( GlobalDTD.SIF_EXTENDEDELEMENTS );
            if ( see == null )
            {
                //	Create a new SIF_ExtendedElements container
                see = new SIF_ExtendedElements();
                AddChild( see );
            }
            else
            {
                //	Lookup existing SIF_ExtendedElement with this name
                ele = see[name];
            }

            //	Create/update SIF_ExtendedElement
            if ( ele == null )
            {
                ele = new SIF_ExtendedElement( name, valu );
                see.AddChild( ele );
            }
            else
            {
                ele.TextValue = valu;
            }
        }

        /// <summary> 	Gets the SIF_ExtendedElement with the specified Name attribute.</summary>
        /// <param name="name">The value of the SIF_ExtendedElement/@Name attribute to search for
        /// </param>
        /// <returns> The SIF_ExtendedElement that has a Name attribute matching the
        /// <c>name</c> parameter, or null if no such element exists
        /// <since>ADK 1.5</since>
        /// </returns>
        public virtual SIF_ExtendedElement GetSIFExtendedElement( string name )
        {
            if ( GlobalDTD.SIF_EXTENDEDELEMENTS == null || name == null )
            {
                return null;
            }

            SIF_ExtendedElements container = (SIF_ExtendedElements) GetChild( GlobalDTD.SIF_EXTENDEDELEMENTS );
            if ( container == null )
            {
                return null;
            }

            return container[name];
        }

        /**
          * 	Gets all SIF_ExtendedElements/SIF_ExtendedElement children of this object.<p>
          * 	@return An array of SIF_ExtendedElement instances. If no SIF_ExtendedElements 
          * 		child element was found, an empty array is returned
          * 
          * 	@since ADK 1.5
          */

        public SIF_ExtendedElement[] SIFExtendedElements
        {
            get
            {
                if ( GlobalDTD.SIF_EXTENDEDELEMENTS == null )
                    return new SIF_ExtendedElement[0];

                SIF_ExtendedElements container = (SIF_ExtendedElements) GetChild( GlobalDTD.SIF_EXTENDEDELEMENTS );
                if ( container == null )
                {
                    return new SIF_ExtendedElement[0];
                }
                return container.ToArray();
            }
            set { SIFExtendedElementsContainer.SetChildren( value ); }
        }

        /// <summary>
        /// Gets the SIF_ExtendedElements container in which all child SIF_ExtendedElement
        /// elements are placed by the {@link #setSIFExtendedElement(String, String)}
        /// method. Note if there is currently no container element, one is created and 
        /// added as a child of the SIFDataObject.
        /// </summary>
        /// <remarks>
        /// This method is provided as a convenience to agents that wish to obtain the 
        /// SIF_ExtendedElements container element in order to manually add extended
        /// elements to it. This is useful, for example, if you need to call methods on
        /// the extended element before adding it to the container (e.g. the <c>DoNotEncode</c>
        /// method). 
        /// </remarks>
        /// <example>
        /// The equivalent functionality is possible by making this call:
        /// <code>
        /// SIF_ExtendedElements container = (SIF_ExtendedElements)GetChild( SifDtd.SIF_EXTENDEDELEMENTS );
        /// </code>
        /// </example>
        /// <returns>The SIF_ExtendedElements container element, which is created and
        /// added as a child to this SIFDataObject if it does not currently exist.</returns>
        public SIF_ExtendedElements SIFExtendedElementsContainer
        {
            get
            {
                SIF_ExtendedElements container = (SIF_ExtendedElements) GetChild( GlobalDTD.SIF_EXTENDEDELEMENTS );
                if ( container == null )
                {
                    container = new SIF_ExtendedElements();
                    AddChild( container );
                }

                return container;
            }
            set
            {
                RemoveChild( GlobalDTD.SIF_EXTENDEDELEMENTS );
                AddChild( GlobalDTD.SIF_EXTENDEDELEMENTS, value );
            }
        }

        /// <summary>
        /// Changes the default behavior of SIFElement so that
        /// the root element and attributes of a SIFData object are always
        /// written, even if setChanged(false) is called. Callers can
        /// call this method to ensure that the root elements and attibutes
        /// are always rendered even if a previous call to setChanged(false) was done.
        /// </summary>
        /// <remarks>
        /// The reason for this is that the ADK uses the change tracking to determine
        /// whether an element should be written out or not when a Query is received with
        /// conditions. No matter what, even if the object does not have element specified as
        /// a query condition, the root SIFDataObjectElement and its mandatory attributes
        /// should still be rendered.
        /// </remarks>
        public void EnsureRootElementRendered()
        {
            // Make sure that the root element and attributes are written out
            this.SetChildChanged();
            lock ( fSyncLock )
            {
                if ( fFields != null )
                {
                    foreach ( SimpleField field in fFields.Values )
                    {
                        if ( field.ElementDef.IsAttribute( this.SifVersion ) )
                        {
                            field.SetChanged( true );
                        }
                    }
                }
            }
        }

        #region Serialization

        // TODO: Andy E Serialization still does not work in the .Net ADK. We need to modify
        // adkgen so that the protected serialization constructor is added to each
        // subclass of Element
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
        protected SifDataObject( SerializationInfo info,
                                 StreamingContext context )
            : base( info, context )
        {
            string versionString = info.GetString( "fVersion" );
            fVersion = SifVersion.Parse( versionString );
        }

        //protected override void OnGetObjectData(SerializationInfo info,
        //                                         StreamingContext context)
        //{
        //   info.AddValue("fVersion", this.SifVersion.ToString());
        //}

        #endregion
    }
}
