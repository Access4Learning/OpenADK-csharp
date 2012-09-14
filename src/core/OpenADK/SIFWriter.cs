//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Schema;
using OpenADK.Library.Global;
using OpenADK.Library.Impl;
using OpenADK.Library.Impl.Surrogates;

namespace OpenADK.Library
{
    /// <summary>  Renders a <c>SifElement</c> to an XML stream in SIF format.
    /// 
    /// Agents do not typically use the SifWriter class directly, but may do so to
    /// render a SIF Data Object or a SIF Message to an output stream. The following
    /// code demonstrates how to Write a SifDataObject to System.out:
    /// 
    /// 
    /// <c>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;StudentPersonal sp = ...<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SifWriter out = new SifWriter( System.out );<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;out.write( sp );<br/>
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public class SifWriter
    {
        private XmlWriter fWriter;
        private XmlWriterSettings fSettings;

        private SifFormatter fFormatter;

        public const string XML_NAMESPACE = "http://www.w3.org/XML/1998/namespace";
        public const string XSI_PREFIX = "xsi";
        public const string NIL = "nil";
        

        private bool fSerializeIds = false;


        /// <summary>  Elements that should not be included in the output</summary>
        protected internal IDictionary<string, IElementDef> fFilter = null;

        /// <summary>  The version of SIF to use when rendering XML</summary>
        protected internal SifVersion fVersion;

        private bool fRootAttributesWritten;


        private SifWriter()
        {
            fSettings = new XmlWriterSettings();

            fSettings.OmitXmlDeclaration = true;
            // Allow multiple fragments (objects) to be written without a root node
            fSettings.ConformanceLevel = ConformanceLevel.Fragment;
            fSettings.CheckCharacters = true;
            fSettings.Indent = true;
            fSettings.IndentChars = "  ";
            fSettings.Encoding = SifIOFormatter.ENCODING;
            SetSifVersion( Adk.SifVersion );
        }

        private void SetSifVersion( SifVersion version )
        {
            fVersion = version;
            fFormatter = Adk.Dtd.GetFormatter( version );
        }

        /// <summary>  Constructor</summary>
        /// <param name="outStream">The OutputStream to Write to
        /// </param>
        public SifWriter( Stream outStream )
            : this()
        {
            fWriter = XmlWriter.Create( outStream, fSettings );
        }

        /// <summary>
        /// Creates an instance of a SifWriter using a TextWriter
        /// </summary>
        /// <param name="writer">The writer to write to. The writer needs to be using the proper encoding for the purpose in which it is used</param>
        public SifWriter( TextWriter writer )
            : this()
        {
            fWriter = XmlWriter.Create( writer, fSettings );
        }



        public SifWriter(XmlWriter writer) : this()
        {
            fWriter = XmlWriter.Create(writer, fSettings);
        }


        /// <summary>
        /// Gets or Sets whether SifWriter will get and set xml:id values, using the SifElement <see cref="SifElement.XmlId"/> property.s
        /// </summary>
        public bool SerializeIds
        {
            get { return fSerializeIds; }
            set { fSerializeIds = value; }
        }

        /// <summary>  Places a filter on this SifWriter such that only elements (and their
        /// children) identified in the array will be included in the output. Note
        /// that attributes are always included even if not specified in the filter
        /// list, as are top-level SIF Data Objects like StudentPersonal.
        /// 
        /// The filter remains in effect until the <c>clearFilter</c> method
        /// is called or a null array is passed to this method.
        /// 
        /// </summary>
        /// <value>An array of ElementDef constants from the SifDtd class
        /// that identify elements to include in the output, or <c>null</c>
        /// to clear the current filter</value>
        public virtual IElementDef[] Filter
        {
            set
            {
                if ( value == null )
                {
                    clearFilter();
                }
                else
                {
                    if ( fFilter == null )
                    {
                        fFilter = new Dictionary<string, IElementDef>();
                    }
                    else
                    {
                        fFilter.Clear();
                    }

                    for ( int i = 0; i < value.Length; i++ )
                    {
                        if ( value[i] != null )
                        {
                            fFilter[value[i].Name] = value[i];
                        }
                    }
                }
            }
        }

        /// <summary>  Clears the filter previously set with the setFilter method</summary>
        public virtual void clearFilter()
        {
            if ( fFilter != null )
            {
                fFilter.Clear();
                fFilter = null;
            }
        }


        private const int EMPTY = 0;
        private const int OPEN = 1;
        //private const int CLOSE = 2;

        private Boolean HasContent( SifElement o,
                                    SifVersion version )
        {
            if ( o.ChildCount > 0 )
            {
                return true;
            }
            ICollection<SimpleField> fields = o.GetFields();
            foreach ( SimpleField f in fields )
            {
                // TODO: This is a perfect place to optimize. Version-specific lookups
                // should be optimized

                if ( f.ElementDef.IsSupported( version ) && !f.ElementDef.IsAttribute( version ) )
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>  Write a SIF Message in the version of SIF in effect for that object.
        /// To change the version of SIF that is used, call the
        /// <c>SifMessagePayload.setSIFVersion</c> method prior to calling
        /// this function.
        /// 
        /// </summary>
        /// <param name="o">The SIF Message to Write to the output stream
        /// </param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public virtual void Write( SifMessagePayload o )
        {
            SetSifVersion( o.SifVersion );
            fWriter.WriteStartElement( "SIF_Message", o.GetXmlns() );
            writeRootAttributes( true );
           
            Write( (SifElement) o );
            fWriter.WriteEndElement();
        }

        /// <summary>  Write a SIF Data Object in the version of SIF in effect for that object.
        /// To change the version of SIF that is used, call the
        /// <c>SifDataObject.setSIFVersion</c> method prior to calling
        /// this function.
        /// 
        /// </summary>
        /// <param name="o">The SifDataObject instance to Write to the output stream
        /// </param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public virtual void Write( SifDataObject o )
        {
            SetSifVersion( o.SifVersion );

            if ( o is SifDataObjectXml )
            {
                fWriter.WriteRaw( o.ToXml() );
            }
            else
            {
                WriteElement( o );
            }
        }

        /// <summary>  Write a SIF Data Object to the output stream using whatever XML content
        /// is currently defined for that object.
        /// 
        /// </summary>
        /// <param name="o">The SifDataObjectXml instance to Write to the output stream
        /// </param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public virtual void Write( SifDataObjectXml o )
        {
            SetSifVersion( o.SifVersion );
            fWriter.WriteRaw( o.ToXml() );
        }

        /// <summary>  Write a SIF element in the version of SIF specified.
        /// 
        /// </summary>
        /// <param name="version">The version of SIF to use when rendering the SIF element
        /// </param>
        /// <param name="o">The SIF Element instance to Write to the output stream
        /// </param>
        [MethodImpl( MethodImplOptions.Synchronized )]
        public virtual void Write( SifElement o,
                                   SifVersion version )
        {
            SetSifVersion( version );
            WriteElement( o );
        }

        /// <summary>
        /// Write a SIF element in the version of SIF currently in effect for this
        /// SIFWriter
        /// </summary>
        /// <param name="o">The SIF Element instance to write to the output stream</param>
        public virtual void Write( SifElement o )
        {
            WriteElement( o );
        }


        /// <summary>  Write a SIF element in the version of SIF currently in effect for this
        /// SifWriter.
        /// </summary>
        /// <param name="element">The SIF Element instance to Write to the output stream
        /// <param name="isLegacy">if true, this method assumes that it needs to do more work,
        /// such as looking for rendering surrogates for the specific version of SIF</param>
        /// </param>
        private void WriteElement( SifElement element, bool isLegacy )
        {
            IElementDef def = element.ElementDef;
            if ( !(Include( element ) && def.IsSupported( fVersion ) )  )
            {
                return;
            }

            if (isLegacy)
            {
                IRenderSurrogate surrogate = def.GetVersionInfo(fVersion).GetSurrogate();
                if (surrogate != null)
                {
                    surrogate.RenderRaw(fWriter, fVersion, element, fFormatter);
                    return;
                }
            }

            if ( element.IsEmpty() || !HasContent( element, fVersion ) )
            {
                if (element is XMLData)
                {
                    XmlDocument doc = ((XMLData) element).Xml;
                    doc.Save( fWriter );
                }
                else
                {
                    Write( element, EMPTY, isLegacy );
                }
            }
            else
            {
                Write( element, OPEN, isLegacy );

                ICollection<Element> elements = fFormatter.GetContent( element, fVersion );
                foreach ( Element childElement in elements )
                {
                    if ( childElement is SifElement )
                    {
                        WriteElement( (SifElement) childElement, isLegacy );
                    }
                    else
                    {
                        Write( (SimpleField) childElement, isLegacy );
                    }
                }
                fWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Write a SIF element in the version of SIF currently in effect for this
        /// SIFWriter.
        /// </summary>
        /// <param name="o"></param>
        private void WriteElement( SifElement o )
        {
            WriteElement( o, fVersion.CompareTo( SifVersion.SIF20 ) < 0 );
        }

        private void Write( SimpleField f, bool isLegacy )
        {
            if ( !Include( f ) )
            {
                return;
            }

            if ( isLegacy )
            {
                IRenderSurrogate surrogate = f.ElementDef.GetVersionInfo( fVersion ).GetSurrogate();
                if ( surrogate != null )
                {
                    surrogate.RenderRaw( fWriter, fVersion, f, fFormatter );
                    return;
                }
            }


            //  "<tag [attr...]>[text]" or "<tag [attr...]/>"

            String fieldValue = null;
            SifSimpleType simpleValue = f.SifValue;
            if ( simpleValue != null )
            {
                fieldValue = simpleValue.ToString( fFormatter );
            }
            if ( fieldValue == null )
            {
                if ( !isLegacy )
                {
                    fWriter.WriteStartElement( f.ElementDef.Tag( fVersion ) );
                    fWriter.WriteAttributeString(NIL, XmlSchema.InstanceNamespace, "true");
                    //fWriter.WriteElementString( f.ElementDef.Tag( fVersion ),  null );
                    fWriter.WriteFullEndElement();
                }
                else
                {
                    // The specified version of SIF doesn't support
                    // the xsi:nil attribute. Set the value to an empty
                    // string
                    fWriter.WriteStartElement( f.ElementDef.Tag( fVersion ) );
                    fWriter.WriteFullEndElement();
                }
            }
            else
            {
                fWriter.WriteStartElement( f.ElementDef.Tag( fVersion ) );

                if ( f.DoNotEncode )
                {
                    fWriter.WriteRaw( fieldValue );
                }
                else
                {
                    fWriter.WriteString( fieldValue );
                }
                fWriter.WriteEndElement();
            }
        }

        private void Write( SifElement o, int mode, Boolean isLegacy )
        {
            if ( !Include( o ) )
            {
                return;
            }


            //  "<tag [attr...]>[text]" or "<tag [attr...]/>"
            string tag = o.ElementDef.Tag( fVersion );
            fWriter.WriteStartElement( tag );
            if (!fRootAttributesWritten)
            {
                writeRootAttributes(false);
            }
            WriteAttributes( o );
            if ( mode == EMPTY )
            {
                fWriter.WriteEndElement();
            }
            else
            {
                // Check for a text value (or an xs:nil value)
                SimpleField elementValue = o.GetField( o.ElementDef );
                if ( elementValue != null )
                {
                    SifSimpleType sst = elementValue.SifValue;
                    if ( sst == null || sst.RawValue == null )
                    {
                        // The value of this element has been set and it is
                        // null. This should be rendered as 'xs:nil' in SIF 2.x and greater
                        if ( !isLegacy )
                        {
                            fWriter.WriteAttributeString(NIL, XmlSchema.InstanceNamespace, "true");
                        }
                    }
                    else
                    {
                        if ( o.DoNotEncode )
                        {
                            fWriter.WriteRaw( o.TextValue );
                        }
                        else
                        {
                            String xmlValue = sst.ToString( fFormatter );
                            fWriter.WriteString( xmlValue );
                        }
                    }
                }
            }
        }

        /// <summary>  Write the attributes of a SifElement to the output stream</summary>
        /// <param name="o">The SifElement whose attributes are to be written
        /// </param>
        private void WriteAttributes( SifElement o )
        {
            // TODO: We need to make sure the GetFields() API returns a usable collection
            ICollection<SimpleField> fields = fFormatter.GetFields( o, fVersion );
            foreach (SimpleField f in fields )
            {
                IElementVersionInfo evi = f.ElementDef.GetVersionInfo( fVersion );
                if ( evi != null && evi.IsAttribute )
                {
                    // Null attribute values are not supported in SIF, unlike 
                    // element values, which can be represented with xs:nil
                    SifSimpleType sst = f.SifValue;
                    if (sst.RawValue != null)
                    {
                        String tag = evi.Tag;
                        Boolean handled = false;
                        if ( tag.StartsWith( "x" ) )
                        {
                            if ( evi.Tag.Equals( "xml:lang" ) )
                            {
                                fWriter.WriteAttributeString("xml", "lang", null, sst.ToString(fFormatter));
                            }
                            else if ( evi.Tag.Equals( "xsi:type" ) )
                            {
                                fWriter.WriteAttributeString("type", XmlSchema.InstanceNamespace, sst.ToString(fFormatter));
                            }
                            handled = true;
                        }

                        if ( !handled )
                        {
                            fWriter.WriteStartAttribute( evi.Tag, string.Empty );
                            fWriter.WriteString( sst.ToString( fFormatter ) );
                            fWriter.WriteEndAttribute();
                        }
                    }
                }
            }


            if ( fSerializeIds && o.XmlId != null )
            {
                fWriter.WriteAttributeString( "id", XML_NAMESPACE, o.XmlId );
            }
        }

        private bool Include( Element o )
        {
            if ( o.ElementDef.IsSupported( fVersion ) && o.IsChanged() )
            {
                if ( fFilter == null || o.ElementDef.Object )
                {
                    return true;
                }

                //  If the element is in the filter list, include it
                if ( fFilter.ContainsKey( o.ElementDef.Name ) )
                {
                    return true;
                }

                //  If any of the element's parents are in the filter list, include it
                Element parent = o.Parent;
                Element cur = o;
                while ( parent != null )
                {
                    IElementDef tst = Adk.Dtd.LookupElementDef( parent.ElementDef, cur.ElementDef.Name );
                    if ( tst != null && fFilter.ContainsKey( tst.Name ) )
                    {
                        return true;
                    }
                    cur = parent;
                    parent = parent.Parent;
                }

                IElementDef parentDef = o.ElementDef.Parent;
                while ( parentDef != null )
                {
                    if ( fFilter.ContainsKey( parentDef.Name ) )
                    {
                        return true;
                    }
                    parentDef = parentDef.Parent;
                }

                //  At this point the element should not be included *unless* it is
                //  the parent of one of the elements in the filter list. In this
                //  case it has to be included or else that child will not be.
                foreach ( IElementDef def in  fFilter.Values )
                {
                    parentDef = def.Parent;
                    if ( parentDef != null && parentDef.Name.Equals( o.ElementDef.Name ) )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream
        /// </summary>
        public void Flush()
        {
            fWriter.Flush();
        }

        /// <summary>
        /// Closes this stream and the underlying stream
        /// </summary>
        public void Close()
        {
            fWriter.Close();
        }

        /// <summary>
        /// By Default, SIFWriter writes an XML Namespace when it starts
        /// writing to an element stream. If this is not desirable, it can
        /// be suppressed with this call. However, if suppressed, the ADK may
        /// not be able to parse the resulting XML, as it relies on XML Namespace
        /// parsing for some features.
        /// </summary>
        /// <param name="suppress"></param>
        public void SuppressNamespace( bool suppress )
        {
            if (suppress)
            {
                fRootAttributesWritten = suppress;
                // Trick the .NET XMLWriter by telling it that the XSI namespace is already 
                // declared in the current scope (WARNING, EXTREME HACK. May fail on future builds of .NET)
                try
                {
                    Type xmlWriterType = fWriter.GetType();
                    if ( xmlWriterType.Name == "XmlWellFormedWriter" )
                    {
                        MethodInfo mi =
                            xmlWriterType.GetMethod( "PushNamespace", BindingFlags.Instance | BindingFlags.NonPublic );
                        if ( mi != null )
                        {
                            mi.Invoke(fWriter, new object[] { XSI_PREFIX, XmlSchema.InstanceNamespace, true });
                        }
                    }
                } catch( Exception ex )
                {
                    Adk.Log.Error( "Unable to suppress namespace support on XmlWellFormedWriter: " + ex.Message, ex );
                }
            }
        }

        private void writeRootAttributes( bool includeVersion)
        {
            if (!fRootAttributesWritten)
            {
                if(includeVersion)
                {
                    fWriter.WriteAttributeString( "Version", fVersion.ToString() );
                }

                if (fFormatter.SupportsNamespaces)
                {
                    fWriter.WriteAttributeString("xmlns", XSI_PREFIX, null, XmlSchema.InstanceNamespace);
                }
            }
            fRootAttributesWritten = true;
        }


    }
}
