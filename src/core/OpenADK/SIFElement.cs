//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using OpenADK.Library.Impl;


namespace OpenADK.Library
{
    /// <summary>  The abstract base class for all SIF Elements.</summary>
    /// <remarks><para>
    /// Each object type and complex field element defined in the SIF Specification
    /// is encapsulated by a subclass of SifElement. Objects include &lt;StudentPersonal&gt;,
    /// &lt;StaffPersonal&gt;, &lt;BusInfo&gt;, etc. while complex field elements
    /// include &lt;Address&gt;, &lt;OtherId&gt;, &lt;PhoneNumber&gt; and so on.
    /// Simple fields which have only a string value but which have no child elements
    /// are encapsulated by the SimpleField class instead of by SifElement. An
    /// example of such a field is the &lt;FirstName&gt; child of the &lt;Name&gt;
    /// element.</para>
    /// 
    /// <para>
    /// SifElements may have a single parent and zero or more children. Complex
    /// fields are always represented as child objects. The AddChild, GetChildren,
    /// RemoveChild, countChildren, and removeAllChildren methods are provided to
    /// manipulate the child list. Simple fields are stored in a dictionary keyed by
    /// the field's <c>IElementDef</c> constant as defined in the SifDtd class.
    /// The value of a simple field is encapsulated by a SimpleField object, which
    /// stores not only the current string value of the field but also its change
    /// state and a reference to its IElementDef.</para>
    /// 
    /// <para>
    /// <b>Comparing SifElement Graphs</b></para>
    /// <para>
    /// Agent developers do not typically work with SifElement objects directly. The
    /// <c>compareGraphTo</c> method is one exception. Agents can use this
    /// method to compare the contents of two SifElements in order to determine which
    /// elements and attributes are different. For an example of how this method can
    /// be used to assist in SIF_Event reporting, refer to the SchoolInfoProvider
    /// Adk Example program. Similarly, the SIFDiff example program demonstrates
    /// using the <c>compareGraphTo</c> method to display the differences
    /// between two SIF Data Objects read from disk.</para>
    /// 
    /// <para>
    /// <b>SifVersion</b></para>
    /// <para>
    /// The abstract <c>getSIFVersion</c> method returns the SifVersion that
    /// is currently in effect for this object. This is often used to determine the
    /// element tag name and sequence number when rendering XML because these may
    /// change from one version of SIF to the next. However, the SifElement class
    /// does not itself keep track of version; it is up to the derived class to do
    /// so. Both SifDataObject and SifMessagePayload store the SIF version
    /// associated with their objects. By working up the object ancestry, it is
    /// possible to determine the SifVersion currently associated with a SifElement.
    /// The <c>effectiveSIFVersion</c> method performs this task.
    /// </para>
    /// </remarks>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    [Serializable]
    public abstract class SifElement : Element
    {
        /// <summary>  Field values. Simple fields (i.e. attributes and child elements that have
        /// only a text value and no attributes) are stored in the hashtable as
        /// SimpleField objects keyed by their associated IElementDef tag. IElementDef
        /// constants are defined by the SifDtd class. Complex fields are stored in
        /// the fChildren vector.
        /// </summary>
        protected IDictionary<String, SimpleField> fFields;

        /// <summary>
        /// Used for storing a unique identifier for this element. Used in the Id property
        /// </summary>
        private String fXmlId;

        /// <summary>  Child elements.</summary>
        private IList<SifElement> fChildren;

        /// <summary>
        /// The object that should by synchronized on for any add or remove operations
        /// of Children elements
        /// </summary>
        protected object fSyncLock = new object();

        /// <summary>  Constructs a SifElement</summary>
        /// <param name="def">The IElementDef constant from the <c>SifDtd</c> class
        /// that provides metadata for this element
        /// </param>
        public SifElement(IElementDef def)
            : base(def)
        {
        }


        // TODO: This API may be removed If not, revert back to the original API
        public virtual string Key
        {
            get { return null; }
        }


        /// <summary>  Gets or Sets the SifVersion associated with this element, if applicable.
        /// The base class implementation of this method always returns null.
        /// </summary>
        /// <returns> A SifVersion object that identifies the version of SIF that
        /// should be used to render this object (or that was used to parse it).
        /// Not all implementation classes store a SifVersion, so null may be
        /// returned.
        /// </returns>
        public virtual SifVersion SifVersion
        {
            get { return null; }

            set
            {
                // The base implementation does nothing
            }
        }

        /// <summary>Returns the number of children elements of this SIFElement</summary>
        public virtual int ChildCount
        {
            get
            {
                if (fChildren == null)
                {
                    return 0;
                }
                lock (fChildren)
                {
                    return fChildren.Count;
                }
            }
        }

        /// <summary>
        /// Gest the TypeConverter to use for text values for this element.
        /// </summary>
        /// <returns></returns>
        private TypeConverter getTextTypeConverter()
        {
            TypeConverter converter = ElementDef.TypeConverter;
            if (converter == null)
            {
                // TODO: Should we not allows this in "Strict" mode?
                converter = SifTypeConverters.STRING;
            }
            return converter;
        }


        /// <summary>  Gets the text value of this element, if applicable. The
        /// text value will be parsed into the native datatype of the element.</summary>
        /// <remarks>
        /// The formatter user for parsing, by default, is the SIF 1.x formatter,
        /// which means that this value must be albe to be parsed using SIF 1.x
        /// formatting rules. The change the format used for getting and setting text
        /// values, set the <see cref="Adk.TextFormatter"/> property.
        /// </remarks>
        /// <value> The text value of this element (e.g. &lt;element&gt;text&lt;element&gt;)</value>
        public override string TextValue
        {
            get { return GetFieldValue(ElementDef); }

            set
            {
                if (value == null)
                {
                    RemoveField(ElementDef);
                    return;
                }

                TypeConverter converter = getTextTypeConverter();
                SifSimpleType typedValue = converter.Parse(Adk.TextFormatter, value);
                SetField(typedValue.CreateField(this, ElementDef));
            }
        }

        /// <summary>
        /// Returns the underlying SIF data type that is stored in this field
        /// </summary>
        public override SifSimpleType SifValue
        {
            get
            {
                SimpleField fieldValue = GetField(ElementDef);
                {
                    if (fieldValue != null)
                    {
                        return fieldValue.SifValue;
                    }
                    return null;
                }
            }
            set { SetField(value.CreateField(this, ElementDef)); }
        }


        /// <summary>  Gets the number of fields for this object</summary>
        public virtual int FieldCount
        {
            get
            {
                if (fFields != null)
                {
                    lock (fFields)
                    {
                        return fFields.Count;
                    }
                }

                return 0;
            }
        }


        /// <summary>  Gets the SifVersion effective for this element by searching the ancestry
        /// until a valid SifVersion is returned by one of the parent objects.
        /// </summary>
        /// <returns> A SifVersion object that identifies the version of SIF that
        /// should be used to render this object (or that was used to parse it).
        /// </returns>
        public virtual SifVersion EffectiveSIFVersion
        {
            get
            {
                SifVersion v = SifVersion;
                SifElement p = (SifElement)Parent;
                while (v == null && p != null)
                {
                    v = p.SifVersion;
                    p = (SifElement)p.Parent;
                }

                if (v == null)
                {
                    v = Adk.SifVersion;
                }

                return v;
            }
        }

        /// <summary>  Gets the tag name for this element. The effective version of SIF is
        /// used to determine the exact tag name since tag names may change from one
        /// version of SIF to the next.</summary>
        /// <remarks>
        /// <note type="note">
        /// In order for this method to return the proper tag name, it must
        /// know the version of SIF in use. The version is obtained by visiting the
        /// element ancestry and calling getSIFVersion on each parent until a non-null
        /// value is returned. Thus, this is a relatively expensive operation and
        /// should only be called when the SifVersion is not known. If the SifVersion
        /// is known, calling <c>getElementDef().Tag( <i>version</i>)</c>
        /// directly is preferred.
        /// </note>
        /// </remarks>
        /// <returns> The element tag name that should be used when rendering XML</returns>
        /// <seealso cref="EffectiveSIFVersion"></seealso>
        public virtual string Tag
        {
            get { return ElementDef.Tag(EffectiveSIFVersion); }
        }


        /// <summary>
        /// Gets or sets the unique identifier for this object.
        /// This value is not used by the ADK and is reserved for use by the application.
        /// </summary>
        /// <value>a string value that uniquely identifies this object to the application</value>
        public string XmlId
        {
            get { return fXmlId; }
            set { fXmlId = value; }
        }


        /// <summary>  Gets the SifElementList of child SifElements.</summary>
        //[CLSCompliant( false )]
        protected IList<SifElement> ChildList()
        {
            if (fChildren == null)
            {
                lock (fSyncLock)
                {
                    if (fChildren == null)
                    {
                        fChildren = CreateList();
                    }
                }
            }
            return fChildren;
        }

        /// <summary>
        /// Creates the list used for storing child elements
        /// </summary>
        /// <returns></returns>
        protected virtual IList<SifElement> CreateList()
        {
            return new List<SifElement>(1);
        }

        ///// <summary>
        ///// Creates the underlying list used for storage of child elements. This method
        ///// can be overriden to allow for specific types of lists to be used.
        ///// </summary>
        ///// <returns></returns>
        //protected List<SifElement> createChildList()
        //{
        //    return 
        //}

        /// <summary>  Adds a child SifElement</summary>
        /// <param name="def">The IElementDef representing the metadata for the child</param>
        /// <param name="element">The child element</param>
        /// <exception cref="ArgumentException">Thrown if the child element is already a child of a different parent or the IElementDef parameter
        /// is not a valid re-definition of the child element's IElementDef</exception>
        public virtual void AddChild(IElementDef def,
                                     SifElement element)
        {
            SafeAddChild(def, element);
        }

        /// <summary>Adds a child SifElement</summary>
        /// <param name="element">The child element</param>
        /// <exception cref="ArgumentException">Thrown if the child being added is already a child of a different parent</exception>
        public virtual SifElement AddChild(SifElement element)
        {
            return SafeAddChild(element);
        }


        /// <summary>
        ///  A safe, final implementation of AddChild that can be called from a constructor
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The SifElement that was added as a child</returns>
        /// <exception cref="ArgumentException">Thrown if the child being added is already a child of a different parent</exception>
        private SifElement SafeAddChild(SifElement element)
        {
            if (element == null || element.Parent == this)
                return element;

            if (element.Parent != null)
            {
                throw new InvalidOperationException("Element \"" + element.ElementDef.Name +
                                            "\" is already a child of another element");
            }

            RestoreImplementationDef(element);
            element.Parent = this;
            ICollection<SifElement> v = ChildList();
            lock (fSyncLock)
            {
                v.Add(element);
            }
            return element;
        }


        /// <summary>
        /// A Safe, internal implementation of AddChild that can be called from a constructor
        /// </summary>
        /// <param name="def">The IElementDef representing the metadata for the child</param>
        /// <param name="element">The SIFElement to be added as a child to this SIFElement</param>
        /// <exception cref="ArgumentException">Thrown if the SIFElement is already a child of another element</exception>
        protected void SafeAddChild(IElementDef def, SifElement element)
        {
            EvaluateChild(element);
            if (element == null || element.Parent == this)
            {
                // nothing to do
                return;
            }

            if (element.Parent != null)
            {
                throw new InvalidOperationException
                    ("Element \"" + element.ElementDef.Name +
                     "\" is already a child of another element");
            }

            element.Parent = this;
            element.ElementDef = def;

            /// SifElementList sel = ChildList();

            ICollection<SifElement> v = (ICollection<SifElement>)ChildList();
            lock (fSyncLock)
            {
                v.Add(element);
            }
        }

        /// <summary>
        /// Allows the element that is about to be added to be evaluated as to whether it
        /// can be added or not
        /// </summary>
        /// <param name="element"></param>
        protected virtual void EvaluateChild(SifElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException
                    (string.Format("Argument cannot be null element={0}", element));
            }
        }

        internal void RestoreImplementationDef(Element candidate)
        {
            IElementDef candidateDef = candidate.ElementDef;
            IElementDef parentDef = candidateDef.Parent;
            SifVersion adkVersion = Adk.SifVersion;
            if (ElementDef != parentDef &&
                candidateDef.IsSupported(adkVersion)
                )
            {
                //  Fixup the ElementRef of the child. For example, if this were a
                //  StudentPersonal and the child were a Name, we would reassign its
                //  ElementRef to be Adk.Dtd.STUDENTPERSONAL_NAME

                //  We also need to redefine the ElementRef to be ADK.DTD.STUDENTPERSONAL_NAME. 
                //  This will allow it to be written in the proper sequence in versions of SIF
                //  in which it is collapsed

                //	NOTE: Eric Petersen: Prior to build 1.1.0.31, the following line used 
                //		  to use o.fElementDef.name(...) instead of o.fElementDef.Tag( ...)
                //		  However, a customer discovered that it did not result in proper
                //		  sequencing of StudentSchoolEnrollment/HomeRoom elements, because
                //		  the tag name of this element is "Homeroom" but the name is "HomeRoom",
                //		  with a capital R. Thus, the line below would never find 
                //		  "StudentSchoolEnrollment_Homeroom". So this line was changed to 
                //		  use the tag() instead of the name(). So far no side-effects have
                //		  been noticed.

                String path = ElementDef.Tag(adkVersion) + "_" +
                              candidate.ElementDef.Tag(adkVersion);
                IElementDef implDef = Adk.Dtd.LookupElementDef(path);
                if (implDef != null)
                {
                    candidate.ElementDef = implDef;
                }
            }
        }


        /// <summary>
        /// Adds the specified children as an array to the SIFElement object.
        /// </summary>
        /// <param name="id">The type of element that is being added</param>
        /// <param name="children">The elements that are being added, all of which are defined
        /// with the same ElementDef</param>
        /// <remarks> All existing children that are defined as the same type as the ElementDef parameter
        /// are removed and replaced with this list. Calling this method with the Element[] 
        /// parameter set to null removes all children.</remarks>
        /// <exception cref="System.InvalidOperationException">Thrown if one of the children is already
        /// a child of a different parent element</exception>
        /// <exception cref="ArgumentNullException">Thrown if either parameter is passed in as null;</exception>
        public void SetChildren(IElementDef id,
                                SifElement[] children)
        {
            if (id == null)
            {
                throw new ArgumentNullException
                    (
                    String.Format("Parameters cannot be null: id={0}", id));
            }

            // First, remove all children of this type from the list
            IList<SifElement> v = ChildList();
            lock (fSyncLock)
            {
                // Go through the vector in reverse order, removing any children of this type
                for (int i = v.Count - 1; i >= 0; i--)
                {
                    SifElement o = v[i];
                    if (
                        ((ElementDefImpl)o.ElementDef).InternalName.Equals
                            (((ElementDefImpl)id).InternalName))
                    {
                        o.Parent = null;
                        v.RemoveAt(i);
                    }
                }
                // Successfully cleared the list
                if (children == null)
                {
                    return;
                }

                // Add any children that were passed in
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i] == null)
                    {
                        continue;
                    }

                    if (children[i].Parent != null && children[i].Parent != this)
                    {
                        throw new InvalidOperationException
                            ("Element \"" + children[i].ElementDef.Name +
                             "\" is already a child of another element");
                    }

                    children[i].Parent = this;
                    children[i].ElementDef = id;
                    v.Add(children[i]);
                }
            }
        }


        /// <summary>  Removes a child object</summary>
        /// <param name="element">The child element</param>
        /// <remarks> This method removes only the first element it finds that matches the IElementDef
        /// and key.</remarks>
        public virtual bool RemoveChild(SifElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("SifElement parameter cannot be null", "element");
            }
            ICollection<SifElement> v = ChildList();
            lock (fSyncLock)
            {
                if (v.Contains(element))
                {
                    element.Parent = null;
                    v.Remove(element);
                    return true;
                }
            }
            return false;
        }

        /// <summary>  Removes a child object identified by its IElementDef and key</summary>
        /// <param name="id">The IElementDef constant that identifies the type of child to remove</param>
        /// <param name="key">The element key value that identifies the specific child to remove</param>
        /// <remarks>This method removes only the first element it finds that matches the IElementDef</remarks>
        /// <exception cref="ArgumentNullException">Thrown if the IElementDef parameter is null</exception>
        public virtual bool RemoveChild(IElementDef id,
                                        string key)
        {
            if (id == null)
            {
                throw new ArgumentNullException("IElementDef parameter cannot be null", "id");
            }

            ICollection<SifElement> v = ChildList();
            lock (fSyncLock)
            {
                foreach (SifElement child in v)
                {
                    if (
                        ((ElementDefImpl)child.ElementDef).InternalName.Equals
                            (((ElementDefImpl)id).InternalName) &&
                        (key == null || (child.Key != null && child.Key.Equals(key))))
                    {
                        if (v.Contains(child))
                        {
                            child.Parent = null;
                            v.Remove(child);
                            return true;
                        }
                        break;
                    }
                }
            }
            return false;
        }


        /// <summary>  Removes a child object identified by its IElementDef</summary>
        /// <param name="id">The IElementDef constant that identifies the type of child to remove
        /// </param>
        /// <remarks>This method removes only the first element it finds that matches the IElementDef</remarks>
        public virtual void RemoveChild(IElementDef id)
        {
            if (id != null)
            {
                RemoveChild(id, (string)null);
            }
        }

        /// <summary>  Removes a child object identified by its IElementDef and complex key</summary>
        /// <param name="id">The IElementDef constant that identifies the type of child to remove
        /// </param>
        /// <param name="complexKey">The complex key values that, taken together, identify the
        /// specific child to remove
        /// </param>
        /// <remarks>This method removes only the first element it finds that matches the IElementDef and key</remarks>
        public virtual void RemoveChild(IElementDef id,
                                        string[] complexKey)
        {
            if (id == null || complexKey == null)
            {
                return;
            }

            StringBuilder b = new StringBuilder(complexKey[0]);
            for (int i = 1; i < complexKey.Length; i++)
            {
                b.Append(".");
                b.Append(complexKey[i]);
            }

            RemoveChild(id, b.ToString());
        }


        ///// <summary>
        ///// Returns a read-only list of all Child elements
        ///// </summary>
        ///// <returns></returns>
        //public SifElementList GetChildren()
        //{
        //   return new SifElementList(childList());
        //}


        /// <summary>  Gets all child objects with a matching IElementDef</summary>
        /// <param name="id">An IElementDef defined by the SifDtd class to uniquely identify this field
        /// </param>
        /// <returns> An ArrayList of the SifElements that have a matching IElementDef</returns>
        public SifElementList GetChildList(IElementDef id)
        {
            ElementDefImpl def = id as ElementDefImpl;
            if (def != null)
            {
                return GetChildList(def.InternalName);
            }
            else
            {
                // Return an empty list
                return new SifElementList();
            }
        }



        ///<summary>
        ///Gets all child objects 
        /// Returns an array of all SifElement children
        ///</summary>
        //public SifElement[] GetChildren()
        //{
        //   List<SifElement> v = (List<SifElement>)ChildList();
        //   lock (fSyncLock)
        //   {
        //      // SifElement[] arr = new SifElement[v.Count];
        //      return v.ToArray();
        //   }
        //}


        /////<summary>
        /////Gets all child objects as an unmodifiable list
        /////Return an array of all SifElement children
        /////</summary>
        //public IList<SifElement> GetChildList()
        //{
        //   return ChildList();
        //}

        ///<summary>
        ///Gets all child objects 
        /// Returns a list of all SifElement children
        ///</summary>
        public IList<SifElement> GetChildList()
        {
            return ChildList();
        }


        /// <summary>
        /// Gets all child objects that match the specified type of child element
        /// </summary>
        /// <typeparam name="T">The type of the child element to return</typeparam>
        /// <returns></returns>
        protected TypedElementList<T> GetChildren<T>()
            where T : SifElement
        {
            List<T> match = new List<T>(1);
            ICollection<SifElement> v = ChildList();

            lock (fSyncLock)
            {
                foreach (SifElement o in v)
                {
                    T t = o as T;
                    if (t != null)
                    {
                        match.Add((T)o);
                    }
                }
            }
            return new TypedElementList<T>(match);
        }

        /// <summary>  Gets all child objects with a matching version-independent element name</summary>
        /// <param name="name">The version-independent name of an element. Note the name is
        /// not necessarily the same as the element tag</param>
        /// <returns> An ArrayList of the SifElements that have a matching element name</returns>
        public SifElementList GetChildList(string name)
        {
            List<SifElement> match = new List<SifElement>();
            ICollection<SifElement> v = ChildList();

            lock (fSyncLock)
            {
                foreach (SifElement o in v)
                {
                    if (((ElementDefImpl)o.ElementDef).InternalName.Equals(name))
                    {
                        match.Add(o);
                    }
                }
            }

            return new SifElementList(match);
        }


        /// <summary>  Gets the child object with the matching IElementDef</summary>
        /// <param name="id">A IElementDef defined by the SifDtd class to uniquely identify this field</param>
        /// <returns> The SifElement that has a matching IElementDef, or null if none found</returns>
        public virtual SifElement GetChild(IElementDef id)
        {
            return GetChild(id, (string)null);
        }

        /// <summary>  Gets the child object with the matching IElementDef and key</summary>
        /// <param name="id">A IElementDef defined by the SifDtd class to uniquely identify this field</param>
        /// <param name="key">The key to match</param>
        /// <returns> The SifElement that has a matching IElementDef and key, or null
        /// if no matches found
        /// </returns>
        public virtual SifElement GetChild(IElementDef id,
                                           string key)
        {
            ICollection<SifElement> v = ChildList();
            lock (fSyncLock)
            {
                foreach (SifElement o in v)
                {
                    if (
                        ((ElementDefImpl)o.ElementDef).InternalName.Equals
                            (((ElementDefImpl)id).InternalName) &&
                        (key == null || (o.Key.Equals(key))))
                    {
                        return o;
                    }
                }
            }

            return null;
        }

        /// <summary>  Gets the child object with the matching IElementDef</summary>
        /// <param name="tag">A IElementDef defined by the SifDtd class to uniquely identify this field
        /// </param>
        /// <returns> The SifElement that has a matching IElementDef, or null if none found
        /// </returns>
        public virtual SifElement GetChild(string tag)
        {
            ICollection<SifElement> v = ChildList();
            lock (fSyncLock)
            {
                foreach (SifElement o in v)
                {
                    //	this is a special case. We're searching for the child element
                    //	that has an version-independent internal name equal to "tag", 
                    //	which is a version-dependent string, so we need to compare on
                    //	the IElementDef differently than usual. Note this method is 
                    //	currently only used by the SifDtd._xpath method, which uses
                    //	version-dependent tag names specified in the XPath query 
                    //	string to match SifElements in memory.
                    //	 
                    if (((ElementDefImpl)o.ElementDef).InternalName.Equals(tag))
                    {
                        return o;
                    }
                }
            }

            return null;
        }


        /// <summary>  Gets the child object with the matching element name and key</summary>
        /// <param name="name">The version-independent element name. Note the element name
        /// is not necessarily the same as the element tag, which is version
        /// dependent.
        /// </param>
        /// <param name="key">The key to match
        /// </param>
        /// <returns> The SifElement that has a matching element name and key, or null
        /// if no matches found
        /// </returns>
        public virtual SifElement GetChild(string name,
                                           string key)
        {
            ICollection<SifElement> v = ChildList();
            lock (fSyncLock)
            {
                foreach (SifElement o in v)
                {
                    if (((ElementDefImpl)o.ElementDef).InternalName.Equals(name) &&
                        (key == null || (o.Key.Equals(key))))
                    {
                        return o;
                    }
                }
            }

            return null;
        }

        /// <summary>  Gets a child object identified by its IElementDef and composite key</summary>
        /// <param name="id">A IElementDef defined by the SifDtd class to uniquely identify this field
        /// </param>
        /// <param name="compKey">The key values in sequential order
        /// </param>
        public virtual SifElement GetChild(IElementDef id,
                                           string[] compKey)
        {
            StringBuilder b = new StringBuilder(compKey[0]);
            for (int i = 1; i < compKey.Length; i++)
            {
                b.Append(".");
                b.Append(compKey[i]);
            }

            return GetChild(id, b.ToString());
        }

        /// <summary>  Does this element have a text value?</summary>
        /// <returns> true if this element has a text value
        /// (e.g. <c>&lt;element&gt;<i>text</i>&lt;element&gt;</c>)
        /// </returns>
        public virtual bool HasTextValue()
        {
            return GetField(ElementDef) != null;
        }


        /// <summary>  Gets a field's value and sequence number</summary>
        /// <param name="id">The field's IElementDef object defined by the SifDtd class.
        /// </param>
        /// <returns> A SimpleField object containing the field's value
        /// and sequence number, or null if the field has no value
        /// </returns>
        /// <seealso cref="GetFieldValue">
        /// </seealso>
        public virtual SimpleField GetField(IElementDef id)
        {
            if (fFields != null)
            {
                lock (fSyncLock)
                {
                    SimpleField fld;
                    fFields.TryGetValue(id.Name, out fld);
                    return fld;
                }
            }

            return null;
        }

        public virtual SimpleField GetField(string name)
        {
            if (fFields != null)
            {
                lock (fFields)
                {
                    SimpleField fld;
                    fFields.TryGetValue(name, out fld);
                    return fld;
                }
            }

            return null;
        }

        /// <summary>  Gets a field's value as a String</summary>
        /// <param name="id">The field's IElementDef object defined by the SifDtd class.
        /// </param>
        /// <returns> The current value of the field as a String, or null if the
        /// field has no value
        /// </returns>
        /// <seealso cref="GetField(IElementDef)">
        /// </seealso>
        public virtual string GetFieldValue(IElementDef id)
        {
            SimpleField v = GetField(id);
            if (v != null)
            {
                return v.TextValue;
            }
            return null;
        }

        /// <summary>
        /// Returns the Simple value of a Field.
        /// </summary>
        /// <param name="id">The IElementDef of the field to lookup</param>
        /// <returns>The value of the underlying SIF field, null if the field is not set</returns>
        protected object GetSifSimpleFieldValue(IElementDef id)
        {
            SimpleField v = GetField(id);
            if (v != null)
            {
                return v.Value;
            }
            return null;
        }


        /// <summary>
        /// Sets a field's value
        /// </summary>
        /// <param name="id">The metadata definition for the field</param>
        /// <param name="value">The value to set to the field</param>
        /// <returns></returns>
        public SimpleField SetField(IElementDef id,
                                    String value)
        {
            return SetField(id, new SifString(value));
        }

        /// <summary>  Sets a field's value</summary>
        /// <param name="id">The field definition object
        /// </param>
        /// <param name="val">The value to assign to the field or <c>null</c> if the field
        /// should be removed</param>
        /// <returns> The internal field object, returned as a convenience so the
        /// caller can mark the field as dirty or empty by calling its setDirty
        /// and setEmpty methods.
        /// </returns>
        public virtual SimpleField SetField(IElementDef id,
                                            SifSimpleType val)
        {
            AssertElementDef( id );
            if (val == null)
            {
                RemoveField(id);
                return null;
            }
            else
            {
                SimpleField field = val.CreateField(this, id);
                SetField(field);
                return field;
            }
        }


        /// <summary>
        /// Sets a SimpleField value to this SifElement
        /// </summary>
        /// <param name="field"></param>
        public virtual void SetField(SimpleField field)
        {
            if (fFields == null)
            {
                fFields = new Dictionary<String, SimpleField>(2);
            }
            lock (fFields)
            {
                fFields[field.ElementDef.Name] = field;
            }
        }



        /**
         *  Sets a field's value, after evaluating the raw data type<p>
         *  This method is a convenience method that can be used by property set methods. 
         *  @param id The field definition object
         *  @param wrapped The SIFSimpleType value to assign to the field
         *  @param unwrappedValue The raw, java value that was set. If this value is null, the field
         *  		will be removed, rather than added
         *  @return The internal field object, returned as a convenience so the
         *      caller can mark the field as dirty or empty by calling its setDirty
         *      and setEmpty methods.
         */

        /// <summary>
        /// Sets a field's value, after evaluating the raw data type.
        /// This method is a convenience method that can be used by property set methods. 
        /// </summary>
        /// <param name="id">The field definition object</param>
        /// <param name="wrappedValue">he SIFSimpleType value to assign to the field</param>
        /// <param name="unwrappedValue">The internal field object, returned as a convenience so the
        /// caller can mark the field as dirty or empty by setting its Dirty
        /// and Empty properties</param>
        /// <returns></returns>
        protected SimpleField SetFieldValue(IElementDef id, SifSimpleType wrappedValue, object unwrappedValue)
        {
            if (unwrappedValue == null)
            {
                RemoveField( id );
                return null;
            }
            return SetField(id, wrappedValue);
        }


        /// <summary>
        /// Removes a field with the specified ID
        /// </summary>
        /// <param name="id"></param>
        protected void RemoveField(IElementDef id)
        {
            if (fFields == null)
            {
                return;
            }
            fFields.Remove(id.Name);
        }

        /// <summary>
        /// Gets a read-only list of the fields for this object
        /// </summary>
        /// <returns></returns>
        public ICollection<SimpleField> GetFields()
        {
            if (fFields != null)
            {
                return fFields.Values;
            }
            else
            {
                return new List<SimpleField>().AsReadOnly();
            }
        }


        /// <summary>  Gets an ordered list of all child elements and fields that are in a
        /// changed state and do not have a null value.</summary>
        /// <remarks>
        /// Attributes are not included.
        /// Elements are ordered by sequence number according to the version of SIF
        /// effective for this object.
        /// </remarks>
        /// <returns> An Element array comprised of all child SifElement and SimpleField
        /// objects ordered according to sequence number. An empty array is
        /// returned if there are no child SifElements or fields (e.g. if this
        /// SifElement has a text value as its content.)
        /// 
        /// </returns>
        /// <seealso cref="EffectiveSIFVersion">
        /// </seealso>
        public virtual IList<Element> GetContent()
        {
            return GetContent(EffectiveSIFVersion);
        }

        /// <summary>  Gets an ordered list of all child elements and fields that are in a
        /// changed state and do not have a null value.</summary>
        /// <remarks>
        ///  Attributes are not included.
        /// Elements are ordered by sequence number according to the specified
        /// version of SIF.
        /// </remarks>
        /// <param name="version">The version of SIF to use when ordering the elements.</param>
        /// <returns> An Element array comprised of all child SifElement and SimpleField
        /// objects ordered according to sequence number. An empty array is
        /// returned if there are no child SifElements or fields (e.g. if this
        /// SifElement has a text value as its content.)
        /// </returns>
        public IList<Element> GetContent(SifVersion version)
        {
            return Adk.Dtd.GetFormatter(version).GetContent(this, version);
        }

        /// <summary>  Sets this element and each of its children to the specified empty
        /// state.  An object in the empty state will be written to an XML stream as
        /// an empty element with no children.
        /// 
        /// </summary>
        /// <param name="empty">true to set the empty state, false to clear it
        /// </param>
        public override void SetEmpty(bool empty)
        {
            base.SetEmpty(empty);
            foreach (SifElement o in ChildList())
            {
                o.SetEmpty(empty);
            }
        }

        /// <summary>  Sets this element and each of its children to the specified changed
        /// state. An object in the changed state will be written to the XML stream
        /// when a SIF message is rendered; objects that are not changed will be
        /// excluded.
        /// 
        /// </summary>
        /// <param name="changed">true to set the changed state, false to clear it
        /// </param>
        public override void SetChanged(bool changed)
        {
            base.SetChanged(changed);
            lock (fSyncLock)
            {
                if (fChildren != null)
                {
                    foreach (SifElement e in fChildren)
                    {
                        e.SetChanged(changed);
                    }
                }
                if (fFields != null)
                {
                    foreach (SimpleField field in fFields.Values)
                    {
                        field.SetChanged(changed);
                    }
                }
            }
        }

        /// <summary>  Compares all child elements and attributes of this Element with that
        /// of another Element.</summary>
        /// <param name="target">The Element to be compared</param>
        /// <returns> An dual-dimensioned array of Elements constituting the
        /// differences between this Element and the comparision Element. The
        /// size of these arrays will always be equals. If there are no elements
        /// in either array, this Element and the comparision Element have identical content. 
        /// </returns>
        /// <exception cref="System.ArgumentException"> is thrown if this Element and the
        /// target are not of the same type; that is, they do not have the same
        /// IElementDef value. For example, trying to compare a <c>SifDtd.STUDENTPERSONAL</c>
        /// element with a <c>SifDtd.BUSINFO</c> element will result in an
        /// exception.
        /// </exception>
        /// <remarks><para>
        ///  Any attributes or elements of the target that have a
        /// different text value from the corresponding attribute or element in this
        /// object, or that appear in one graph but not in the other, are returned
        /// in an array. Repeatable elements are considered the same object only if
        /// their <i>keys</i> match.</para>
        /// <para>
        /// The comparision is exclusive of the source and target objects; that is,
        /// their text values are not included in the comparision. This method is
        /// typically called on top-level SIF Data Objects such as StudentPersonal,
        /// LibraryPatronStatus, CircTx, BusInfo, etc.</para>
        /// <para>
        /// The result of the comparision is returned as a dual-dimensioned array,
        /// where the first element in the array represents the source object and
        /// the second element represents the target object. Each of these arrays
        /// will contain the same number of entries. Within each array, each slot
        /// consists of a <c>SifElement</c> or <c>SimpleField</c> object.
        /// In the Adk, the SifElement class encapsulates complex elements such as
        /// Name, PhoneNumber, and Demographics while the SimpleField class
        /// encapsulates elements with no children such as Name/FirstName and Name/LastName,
        /// or attributes such as StudentPersonal/@RefId. If a given element or
        /// attribute appears in one graph but not in the other, its slot in the
        /// array will contain a null value.
        /// </para>
        /// For example, the arrays returned by this method might consist of the
        /// following:
        /// <list type="table">
        /// <listheader><term>Array[0][0..5]</term><description>Array[1][0..5]</description></listheader>
        /// <item><term>@RefId='AB123...'</term><description>Your Description</description></item>
        /// <item><term>&lt;LastName&gt;Johnson&lt;/LastName&gt;</term><description>&lt;LastName>Johnsen&lt;/LastName&gt;</description></item>
        /// <item><term>&lt;OtherId Type='06'&gt;1004&lt;/OtherId&gt;</term><description>null</description></item>
        /// <item><term>&lt;OtherId Type='ZZ'&gt;SCHOOL:997&lt;/OtherId&gt;</term><description>null</description></item>
        /// <item><term>null</term><description>&lt;OtherId Type='ZZ'&gt;BARCODE:P12345&lt;/OtherId&gt;</description></item>
        /// </list>
        /// 
        /// 
        /// In this example, the RefId attribute (a SimpleField instance) has a
        /// different value in the source than in the target, as does the LastName
        /// element (a SifElement instance). Two OtherId elements of type '06' and 'ZZ'
        /// appeared in the source but not in the target, so they are included in
        /// Array[0][2] and Array[0][3] but have a corresponding null value in
        /// Array[1][2] and Array[1][3]. Finally, one OtherId element of type 'ZZ'
        /// appeared in the target but not in the source, so it appears in Array[1][5]
        /// but has a corresponding null value in Array[0][5].
        /// <para>
        /// For examples of how this method is used, please consult the SIFDiff and
        /// SchoolInfoProvider Adk Example agents.
        /// </para>
        /// </remarks>
        public virtual Element[][] CompareGraphTo(SifElement target)
        {
            if (target.ElementDef != ElementDef)
            {
                throw new ArgumentException
                    ("Element types differ (cannot compare " +
                     ElementDef.GetSQPPath(SifVersion) + " to " +
                     target.ElementDef.GetSQPPath(target.SifVersion));
            }

            List<Element> srcDiffs = new List<Element>();
            List<Element> dstDiffs = new List<Element>();

            _compareGraphTo(EffectiveSIFVersion, target, srcDiffs, dstDiffs, false);
            target._compareGraphTo(EffectiveSIFVersion, this, srcDiffs, dstDiffs, true);

            Element[] src = srcDiffs.ToArray();
            Element[] dst = dstDiffs.ToArray();
            return new Element[][] { src, dst };
        }

        private void _compareGraphTo(SifVersion ver,
                                     SifElement target,
                                   IList<Element> srcMap,
                                   IList<Element> dstMap,
                                   bool isDst)
        {
            IList<Element> diffs = isDst ? dstMap : srcMap;
            IList<Element> odiffs = isDst ? srcMap : dstMap;


            if (fFields != null)
            {
                lock (fFields)
                {
                    foreach (string name in fFields.Keys)
                    {
                        if (name.Length == 0)
                        {
                            continue;
                        }

                        SimpleField fld;
                        fFields.TryGetValue(name, out fld);
                        SimpleField fldComp = target.GetField(name);
                        if (fldComp != null)
                        {
                            if (fldComp.CompareTo(fld) != 0)
                            {
                                //							System.out.println("Field differs: " + fld.fElementDef.getSDOPath() + " = " + fld.getTextValue() );
                                if (!diffs.Contains(fld))
                                {
                                    diffs.Add(fld);
                                    odiffs.Add(fldComp);
                                }
                            }
                        }
                        else if (!isDst)
                        {
                            if (!diffs.Contains(fld))
                            {
                                diffs.Add(fld);
                                odiffs.Add(null);
                            }
                        }
                    }
                }
            }

            //  Compare child elements
            lock (fSyncLock)
            {
                ICollection<SifElement> v = ChildList();
                foreach (SifElement xCh in v)
                {
                    SifElement xComp = xCh.ElementDef.IsRepeatable(ver)
                                           ? target.GetChild(xCh.ElementDef, xCh.Key)
                                           : target.GetChild(xCh.ElementDef);

                    if (xComp != null)
                    {
                        if (xCh.CompareTo(xComp) != 0)
                        {
                            //						System.out.println("Element differs: " + xCh.fElementDef.Name + " - " + xCh.getClass() );
                            if (!diffs.Contains(xCh))
                            {
                                diffs.Add(xCh);
                                odiffs.Add(xComp);
                            }
                        }

                        xCh._compareGraphTo(ver, xComp, srcMap, dstMap, false);
                    }
                    else if (!isDst)
                    {
                        //					System.out.println("Element in "+(isDst?"target":"source")+" but not in "+(isDst?"source":"target")+": " + xCh.fElementDef.Name + " (srcKey=" + xCh.getKey() + ")" );
                        if (!diffs.Contains(xCh))
                        {
                            diffs.Add(xCh);
                            odiffs.Add(null);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// reates an instance of a SIFElement from its ID and adds it to the parent,
        /// taking into account rules about collapsed elements
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SifElement Create(SifElement parent,
                                        IElementDef id)
        {
            SifElement element;
            try
            {
                Type type = Type.GetType(id.FQClassName);
                if (type == null)
                {
                    throw new AdkSchemaException
                        (
                        "Could not create an instance of " + id.FQClassName +
                        " to wrap a " + id.Name + " object because that class doesn't exist", null);
                }
                element = (SifElement)Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                throw new AdkSchemaException
                    (
                    "Could not create an instance of " + id.FQClassName +
                    " to wrap a " + id.Name + ":" + e, null, e);
            }

            element.ElementDef = id;
            return element;
        }


        public override object Clone()
        {
            SifElement elementCopy = null;

            // Most SIFElement subclasses should have a constructor with no arguments that
            // sets the ElementDef automatically. If not, we need to find the 
            // constructor with the ElementDef argument
            foreach (ConstructorInfo c in GetType().GetConstructors())
            {
                ParameterInfo[] parameterTypes = c.GetParameters();
                if (parameterTypes.Length == 0)
                {
                    // A Zero-Parameter constructor
                    elementCopy = (SifElement)Activator.CreateInstance(GetType());
                    break;
                }
                else if (parameterTypes.Length == 1 &&
                         parameterTypes[0].ParameterType == typeof(IElementDef))
                {
                    elementCopy =
                        (SifElement)Activator.CreateInstance(GetType(), ElementDef);
                    break;
                }
            }

            if (elementCopy == null)
            {
                throw new NotSupportedException
                    ("Unable to find constructor suitable for cloning " +
                     GetType().FullName);
            }

            elementCopy.fXmlId = fXmlId;
            if (fFields != null)
            {
                foreach (SimpleField sf in fFields.Values)
                {
                    SimpleField fieldCopy = (SimpleField)sf.Clone();
                    elementCopy.SetField(fieldCopy);
                }
            }

            if (fChildren != null)
            {
                foreach (SifElement childElement in fChildren)
                {
                    elementCopy.AddChild((SifElement)childElement.Clone());
                }
            }

            return elementCopy;
        }

        /// <summary>
        /// Asserts the IElementDef parameter for methods that accept it
        /// </summary>
        /// <param name="id"></param>
        protected void AssertElementDef( IElementDef id )
        {
            if (!Adk.Initialized)
            {
                throw new ApplicationException("The Adk is not initialized");
            }

            if (id == null)
            {
                throw new ArgumentException("IElementDef cannot be null");
            }
        }

        #region Serialization

        // TODO: Andy E Serialization is an unsupported feature of the .Net ADK. We need to create
        // Unit tests for this and work out any issues
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected SifElement(SerializationInfo info,
                             StreamingContext context)
            : base(info, context)
        {
            fXmlId = info.GetString("fId");
            fChildren = (List<SifElement>)info.GetValue("fId", typeof(List<SifElement>));
            fFields =
                (Dictionary<String, SimpleField>)
                info.GetValue("fFields", typeof(Dictionary<String, SimpleField>));
        }

        //protected override void OnGetObjectData(SerializationInfo info,
        //                                        StreamingContext context)
        //{
        //   info.AddValue("fId", fXmlId);
        //   info.AddValue("fChildren", fChildren);
        //   info.AddValue("fFields", fFields);
        //}

        #endregion
    }
}
