//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using OpenADK.Library.Impl;

namespace OpenADK.Library
{
    /// <summary>  The abstract base class for all element and attribute classes in the SIF
    /// Data Objects library.</summary>
    /// <remarks><para>
    /// Agent developers do not generally work with this class directly.
    /// </para>
    /// <para>
    /// Each Element instance is associated with an IElementDef that both identifies
    /// and describes the element or attribute encapsulated by this class. IElementDef
    /// constants are defined by the <see cref="SifDtd"/> class for each element and
    /// attribute defined in the SIF Specification. These objects provide
    /// information about how to render elements in a version-dependent way,
    /// including the tag name and sequence number (which may vary from one version
    /// of SIF to the next), the Sdo implementation class name, the earliest version
    /// of SIF the element or attribute appeared in, and the latest version of SIF
    /// that supports the element or attribute.
    /// </para>
    /// An IElementDef must be provided to the constructor.
    /// </remarks>
    /// <author>Eric Petersen</author>
    /// <version>Adk 1.0</version>
    [Serializable]
    public abstract class Element :  ICloneable //ISerializable,
        // TODO:  ISerializable,     IDeserializationCallback
    {
        /// <summary>
        /// The current version of this object
        /// </summary>
        private const int CURRENT_SERIALIZE_VERSION = 2;


        [Flags]
        protected enum ElementFlags
        {
            /// <summary>  State flag indicating this Element has a dirty value. This state is
            /// enabled at the time of constuction (i.e. all elements are dirty by
            /// default) and must be explicitly disabled by an agent as necessary.
            /// </summary>
            Dirty = 0x01,
            /// <summary>  State flag indicating this Element has an empty value. This state is
            /// disabled at the time of construction, but may be set or cleared by
            /// methods that set this element's value.
            /// </summary>
            Empty = 0x02,
            /// <summary>
            ///  Option flag indicating XML Encoding of character entities should not
            ///  be performed on this Element's text content when rendered by the ADK.
            /// </summary>
            DoNotEncode = 0x04
        }


        /// <summary>  Identifies this field by its IElementDef constant defined in SifDtd</summary>
        [NonSerialized()] internal IElementDef fElementDef;

        /// <summary>  State flags keep track of whether this Element is changed or has an
        /// empty value. By default the Adk does not track the state of an Element when the 
        /// <see cref="TextValue"/> property is called to change its value. The programmer must
        /// explicitly call the <see cref="IsChanged"/> and <see cref="IsEmpty"/> methods
        /// to mark an element as changed or empty
        /// </summary>
        [NonSerialized()] protected ElementFlags fFlags;

        /// <summary>
        /// The parent Element of null if there is no parent
        /// </summary>
        [NonSerialized()] private Element fParent;


        /// <summary>  Gets the metadata for this Element</summary>
        /// <value> an IElementDef that describes this Element
        /// </value>
        /// <remarks>
        /// Sets the metadata for this Element.
        /// <note type="note">
        /// this method should not generally be called by agents because the
        /// IElementDef metadata is established in the constructor. It is provided
        /// in order to support the dynamic creation of Element instances by clients 
        /// that do not (or cannot) use reflection to call the default constructor. 
        /// These clients can call the <c>Class.newInstance</c> method followed
        /// by <c>setElementDef</c> to construct an Element dynamically.
        /// </note>
        /// </remarks>
        public virtual IElementDef ElementDef
        {
            get { return fElementDef; }

            set
            {
                if ( fElementDef != null ) {
                    // The IElementDef for this element is being re-assigned. This is only valid
                    // in cases where a SifElement is being added as a child to another SifElement
                    // and more specific IElementDef information if present for that child instance.
                    // Only allow this if the names match so that this method is not used incorrectly
                    if ( !fElementDef.Name.Equals( value.Name ) ) {
                        {
                            throw new InvalidOperationException
                                ( string.Format
                                      ( "{0} is not a valid IElementDef to replace {1} for {2}",
                                        fElementDef, value, this.GetType().Name ) );
                        }
                    }
                }

                fElementDef = value;
            }
        }


        /// <summary> Gets or sets the text value of this element if applicable</summary>
        public abstract string TextValue { get; set; }

        /// <summary> Gets or sets the SIF strongly-typed value of this element if applicable</summary>
        public abstract SifSimpleType SifValue { get; set; }

        /// <summary>
        /// Gets or sets the parent of this Element.
        /// </summary>
        public Element Parent
        {
            get { return fParent; }

            set { fParent = value; }
        }

        /// <summary>  Enumerating the ancestry of this object to return the root Element</summary>
        public virtual Element Root
        {
            get
            {
                if ( fParent == null ) {
                    return this;
                }
                Element p = fParent;
                while ( p.fParent != null ) {
                    p = p.fParent;
                }
                return p;
            }
        }


        /// <summary>  Constructor</summary>
        /// <param name="def">The metadata that describes this Element
        /// </param>
        protected Element( IElementDef def )
        {
            if ( def == null ) {
                throw new ArgumentNullException
                    ( "SIF " + Adk.SifVersion +
                      " does not support this element or attribute, or the required Sdo library is not loaded (" +
                      GetType().ToString() + ")" );
            }

            fElementDef = def;
            fFlags = ElementFlags.Dirty;
        }

        /// <summary>  Constructor</summary>
        /// <param name="def">The metadata that describes this Element</param>
        /// <param name="parent">The Parent of this element</param>
        protected Element( IElementDef def,
                           Element parent )
            : this( def )
        {
            fParent = parent;
        }

        /// <summary>  Sets this DataObject and each of its children to the dirty state. An
        /// object in the dirty state will not be written to an XML stream when a
        /// SIF message is rendered.
        /// </summary>
        public virtual void SetChanged()
        {
            SetChanged( true );
        }

        /// <summary>  Sets this DataObject and each of its children to the specified dirty
        /// state. An object in the dirty state will not be written to an XML stream
        /// when a SIF message is rendered.
        /// </summary>
        /// <param name="changed">true to set the dirty state, false to clear it
        /// </param>
        public virtual void SetChanged( bool changed )
        {
            if ( changed ) {
                fFlags |= ElementFlags.Dirty;
                if (fParent != null)
                {
                    fParent.SetChildChanged();
                }

            }
            else {
                fFlags &= ~ElementFlags.Dirty;
            }
        }



        /// <summary>
        /// Called by children when they are set to a "changed" state. The
        /// parent of any child element that is changed should also be marked as
        /// changed (but not any other children of the parent).
        /// </summary>
        protected void SetChildChanged()
        {
            // don't want to recurse children. This just a call
            // to recurse up the chain
            if (!IsChanged())
            {
                fFlags |= ElementFlags.Dirty;
                if (fParent != null)
                {
                    fParent.SetChildChanged();
                }
            }

        }


        /// <summary>  Sets this DataObject and each of its children to the empty state. An
        /// object in the empty state will be written to an XML stream as an empty
        /// element with no attributes and no child elements.
        /// </summary>
        public virtual void SetEmpty()
        {
            SetEmpty( true );
        }

        /// <summary>  Sets this DataObject and each of its children to the specified empty
        /// state.  An object in the empty state will be written to an XML stream as
        /// an empty element with no attributes and no child elements.
        /// </summary>
        /// <param name="empty">true to set the empty state, false to clear it
        /// </param>
        public virtual void SetEmpty( bool empty )
        {
            if ( empty ) {
                fFlags |= ElementFlags.Empty;
            }
            else {
                fFlags &= ~ElementFlags.Empty;
            }
        }

        /// <summary>  Determines if this object is in the changed state.</summary>
        /// <returns> true if this object has been marked changed. The return value
        /// assumes all children are in the same state.
        /// </returns>
        public virtual bool IsChanged()
        {
            return (fFlags & ElementFlags.Dirty) != 0;
        }

        /// <summary>  Determines if this object is in the empty state</summary>
        /// <returns> true if this object has explicitly been marked empty. The return
        /// value assumes all children are in the same state.
        /// </returns>
        public virtual bool IsEmpty()
        {
            return (fFlags & ElementFlags.Empty) != 0;
        }


        /**
			 * 	Determines if automatic XML Encoding of character entities should be 
			 * 	performed on this element when rendered by the ADK. By default, all
			 * 	elements and attributes are encoded. Use the #setDoNotEncode method to
			 * 	turn off automatic encoding for an element if you will be assigning
			 * 	XML content to its text value (e.g. if you are using SIF_ExtendedElement
			 * 	to exchange raw XML content with another agent).
			 * 
			 * 	@return <code>true</code> if automatic XML Encoding is disabled for this
			 * 		element; <code>false</code> if enabled (the default)
			 */


        /// <summary>
        /// Determines if automatic XML Encoding of character entities should be
        /// performed on this element when rendered by the ADK.
        /// </summary>
        /// <remarks>
        /// By default, all elements and attributes are encoded. Use this property to
        /// turn off automatic encoding for an element if you will be assigning
        /// XML content to its text value (e.g. if you are using SIF_ExtendedElement
        /// to exchange raw XML content with another agent).
        /// </remarks>
        /// <value><c>True</c> if automatic XML Encoding is disabled for this
        /// element; <c>False</c> if enabled (the default)</value>
        public virtual bool DoNotEncode
        {
            get
            {
                if ( (fFlags & ElementFlags.DoNotEncode) != 0 ) {
                    return true;
                }
                if ( fElementDef != null ) {
                    return fElementDef.DoNotEncode;
                }

                return false;
            }
            set
            {
                if ( value ) {
                    fFlags |= ElementFlags.DoNotEncode;
                }
                else {
                    fFlags &= ~ ElementFlags.DoNotEncode;
                }
            }
        }


        /// <summary>Compare the text value of this Element to another Element</summary>
        /// <param name="target">The Element to be compared</param>
        /// <returns> the value <c>0</c> if the argument's text value is
        /// lexicographically equal to this Element's text value; a value less
        /// than <c>0</c> if the argument's value is lexicographically
        /// greater than this Element's text value; and a value greater than
        /// <c>0</c> if the argument's text value is lexicographically
        /// less than this Element's text value. If one Element's text value is
        /// null and the others is not, a negative value is returned.
        /// </returns>
        public virtual int CompareTo( Element target )
        {
            string s1 = TextValue;
            string s2 = target.TextValue;

            if ( s1 != null && s2 != null ) {
                return s1.CompareTo( s2 );
            }
            if ( s1 == null && s2 == null ) {
                return 0;
            }

            return - 1;
        }

        /// <summary>  Returns the value of <c>getTextValue</c></summary>
        /// <returns> The text value of this element if applicable
        /// </returns>
        public override string ToString()
        {
            return TextValue;
        }

        #region Serialization

        // TODO: Andy E Serialization still does not work in the .Net ADK. We need to modify
        // adkgen so that the protected serialization constructor is added to each
        // subclass of Element
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected Element( SerializationInfo info,
                           StreamingContext context )
        {
            fFlags = (ElementFlags) info.GetInt32( "fFlags" );
            fParent = (Element) info.GetValue( "fParent", typeof ( Element ) );
            IElementDef foundElementDef = null;
            string path = info.GetString( "fElementDef.SDOPath" );
            if ( path.Length > 0 ) {
                foundElementDef = Adk.Dtd.LookupElementDef( path );
            }
            if ( foundElementDef == null ) {
                // TODO:  MLW - I consider this a hack.  On deserialization, the no-arguments constructor is 
                // not called.  Also, SIFElements that were serialized without a parent but normally do have a parent
                // are not returned by the lookupElementDef() call above.  To fix this, I instantiate
                // a new object of this type, and then see what the elementdef of that object is.
                SifElement instanceOfThisType =
                    (SifElement) Activator.CreateInstance( this.GetType() );
                foundElementDef = instanceOfThisType.ElementDef;
            }
            fElementDef = foundElementDef;
        }

        //[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        //void ISerializable.GetObjectData( SerializationInfo info,
        //                                  StreamingContext context )
        //{
        //    info.AddValue( "fFlags", fFlags );
        //    info.AddValue( "fParent", fParent );
        //    info.AddValue( "fElementDef.SDOPath", fElementDef.SDOPath );
        //    OnGetObjectData( info, context );
        //}

        //protected abstract void OnGetObjectData( SerializationInfo info,
        //                                         StreamingContext context );

        #endregion

        #region ICloneable Members

        public abstract object Clone();

        #endregion
    }
}
