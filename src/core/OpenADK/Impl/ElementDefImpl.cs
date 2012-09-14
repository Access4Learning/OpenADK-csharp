//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using OpenADK.Library.Impl.Surrogates;
using OpenADK.Library.us;

namespace OpenADK.Library.Impl
{
    /// <summary>  Provides metadata for a single SIF data object type or field (an attribute
    /// or child element). This information is used internally by the class framework
    /// to parse and render messages.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <since>  1.0
    /// </since>
    public class ElementDefImpl : IElementDef
    {
        // Cached array of supported SIF versions for faster lookups
        private static SifVersion[] sSifVersions = Adk.SupportedSIFVersions;


        /// <summary>  Flag indicating this is a field that should be rendered as an attribute
        /// of its parent rather than a child of its parent.
        /// </summary>
        public const int FD_ATTRIBUTE = 0x01;

        /// <summary>  Flag indicating this is a simple field element with no children</summary>
        public const int FD_FIELD = 0x02;

        /// <summary>  Flag indicating this is a top-level element (SIF_Ack, StudentPersonal, etc.)</summary>
        public const int FD_OBJECT = 0x04;

        /// <summary>  Flag indicating this element is deprecated in this version of SIF</summary>
        public const int FD_DEPRECATED = 0x08;

        /// <summary>  Flag indicating this element is repeatable in this version of SIF</summary>
        public const int FD_REPEATABLE = 0x10;

        /// <summary>
        /// Flag indicating the content of this element should not be automatically
        /// escaped by the SIFWriter class.
        /// </summary>
        public const int FD_DO_NOT_ENCODE = 0x20;


        /// <summary>
        ///  Flag indicating this element is a repeatable element container  and that the /// 
        ///  container should be "collapsed" in this version of SIF.
        ///  This causes the repeatable element container to not be written in this version of SIF
        /// </summary>
        public const int FD_COLLAPSE = 0x40;

        /// <summary>
        /// Flag indicating this element is the payload value of an XML Element
        /// </summary>
        public const int FD_ELEMENT_VALUE = 0x80;


        /// <summary>
        /// A special flag in SIF used to indicate that the specified element is deleted 
        /// </summary>
        public static readonly ElementDefImpl DELETED_FLAG =
            new ElementDefImpl
                (null, "Deleted", null, 1, SifDtd.COMMON, null, 0, SifVersion.LATEST, SifVersion.LATEST,
                 SifTypeConverters.BOOLEAN);


        /// <summary>  An array of VersionInfo objects that describe the element tag name or
        /// attribute name and sequence number for each version of SIF. The first
        /// element in the array represents SIF 1.0r1 (the first version supported
        /// by the Adk), and the last element in the array represents the last
        /// version of SIF supported by the Adk. Any array element with a null value
        /// is considered to be identical to the first non-null element preceding
        /// it.
        /// </summary>
        private AbstractVersionInfo[] fInfo =
            new AbstractVersionInfo[sSifVersions.Length];

        /// <summary>  The version-independent name of this element (typically the same as
        /// the tag name for SIF 1.0r1)
        /// </summary>
        protected string fName;

        /// <summary>  Version-independent flags</summary>
        private int fFlags;

        /// <summary>
        /// The most recent version of SIF this element or attribute appeared in
        /// </summary>
        private readonly SifVersion fLatestVersion;

        /// <summary>  The parent element</summary>
        private ElementDefImpl fParent;

        /// <summary>  The local package name where this element is defined in the Sdo class library</summary>
        private string fPackage;

        /// <summary>  The SIF Variant where this element is defined, if applicable</summary>
        private string fVariant;

        /// <summary>  The children of this element</summary>
        private Dictionary<String, IElementDef> fChildren;

        /// <summary>
        /// The SIF data type converter to use for this element
        /// </summary>
        private TypeConverter fTypeConverter;

        /// <summary>  Constructs an IElementDef with flag
        /// 
        /// </summary>
        /// <param name="parent">The parent of this element
        /// </param>
        /// <param name="name">The version-independent name of the element
        /// </param>
        /// <param name="tag">The element or attribute tag (if different from the name)
        /// </param>
        /// <param name="sequence">The zero-based ordering of this element within its parent
        /// or -1 if a top-level element
        /// </param>
        /// <param name="localPackage">The name of the package where the corresponding
        /// DataObject class is defined, excluding the
        /// <c>OpenADK.Library</c> prefix
        /// </param>
        /// <param name="earliestVersion">The earliest version of SIF supported by this
        /// element. If the element is supported in any other version of SIF -
        /// or is deprecated in a later version - the SdoLibrary class must
        /// define it by calling <c>DefineVersionInfo</c>
        /// </param>
        /// <param name="latestVersion">The latest version of SIF supported by this element</param>
        public ElementDefImpl(IElementDef parent,
                              string name,
                              string tag,
                              int sequence,
                              string localPackage,
                              SifVersion earliestVersion,
                              SifVersion latestVersion)
            : this(parent, name, tag, sequence, localPackage, (int) 0, earliestVersion, latestVersion)
        {
        }

        /// <summary>  Constructs an IElementDef with flag
        /// 
        /// </summary>
        /// <param name="parent">The parent of this element
        /// </param>
        /// <param name="name">The version-independent name of the element
        /// </param>
        /// <param name="tag">The element or attribute tag (if different from the name)
        /// </param>
        /// <param name="sequence">The zero-based ordering of this element within its parent
        /// or -1 if a top-level element
        /// </param>
        /// <param name="localPackage">The name of the package where the corresponding
        /// DataObject class is defined, excluding the
        /// <c>OpenADK.Library</c> prefix
        /// </param>
        /// <param name="flags">One of the following: FD_ATTRIBUTE if this element should
        /// be rendered as an attribute of its parent rather than a child
        /// element; FD_FIELD if this element is a simple field with no child
        /// elements; or FD_OBJECT if this element is a SIF Data Object such
        /// as StudentPersonal or an infrastructure message such as SIF_Ack;
        /// FD_DEPRECATED if this element no longer applies to this version of
        /// SIF
        /// </param>
        /// <param name="earliestVersion">The earliest version of SIF supported by this
        /// element. If the element is supported in any other version of SIF -
        /// or is deprecated in a later version - the SdoLibrary class must
        /// define it by calling <c>DefineVersionInfo</c>
        /// </param>
        /// <param name="latestVersion"></param>
        public ElementDefImpl(IElementDef parent,
                              string name,
                              string tag,
                              int sequence,
                              string localPackage,
                              int flags,
                              SifVersion earliestVersion,
                              SifVersion latestVersion)
            : this(
                parent, name, tag, sequence, localPackage, null, flags, earliestVersion, latestVersion,
                (TypeConverter) null)
        {
        }

        /// <summary>
        /// Constructs an ElementDef 
        /// </summary>
        /// <param name="parent">The parent of this element</param>
        /// <param name="name">The version-independent name of the element</param>
        /// <param name="tag">The element or attribute tag (if different from the name)</param>
        /// <param name="sequence">The zero-based ordering of this element within its parent
        /// or -1 if a top-level element</param>
        /// <param name="localPackage">localPackage The name of the package where the corresponding
        /// DataObject class is defined, excluding the
        /// <code>OpenADK.Library</code> prefix</param>
        /// <param name="variant"></param>
        /// <param name="flags"></param>
        /// <param name="earliestVersion"></param>
        /// <param name="latestVersion"></param>
        /// <param name="typeConverter"></param>
        /// 
        public ElementDefImpl(IElementDef parent,  
                              string name,
                              string tag,
                              int sequence,
                              string localPackage,
                              string variant,
                              int flags,
                              SifVersion earliestVersion,
                              SifVersion latestVersion)
            : this(
                parent, name, tag, sequence, localPackage, variant, flags, earliestVersion, latestVersion,
                (TypeConverter)null)
        {
        }

        /// <summary>
        /// The real constructor
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="tag"></param>
        /// <param name="sequence"></param>
        /// <param name="localPackage"></param>
        /// <param name="variant"></param>
        /// <param name="flags"></param>
        /// <param name="earliestVersion"></param>
        /// <param name="latestVersion"></param>
        /// <param name="typeConverter"></param>
        public ElementDefImpl(IElementDef parent,
                              string name,
                              string tag,
                              int sequence,
                              string localPackage,
                              string variant,
                              int flags,
                              SifVersion earliestVersion,
                              SifVersion latestVersion,
                              TypeConverter typeConverter)
        {
            fName = name;

            if ((flags & FD_ATTRIBUTE) != 0)
            {
                // If this is an attribute, it is also a simple field
                flags |= FD_FIELD;
            }

            fFlags = flags;
            fPackage = localPackage;

            fVariant = variant;
            fParent = (ElementDefImpl) parent;
            fTypeConverter = typeConverter;

            DefineVersionInfo(earliestVersion, tag == null ? name : tag, sequence, flags);
            fLatestVersion = latestVersion;

            if (fParent != null)
            {
                fParent.addChild(this);
            }
        }


        public virtual string SDOPath
        {
            get
            {
                StringBuilder b = new StringBuilder(fName);
                ElementDefImpl p = fParent;
                while (p != null)
                {
                    b.Insert(0, p.fName + "_");
                    p = p.fParent;
                }

                return b.ToString();
            }
        }

        public virtual IElementDef Parent
        {
            get { return fParent; }
        }


        /// <summary>
        /// Gets all of the children of this element
        /// </summary>
        /// <returns></returns>
        public IList<IElementDef> Children
        {
            get
            {
                List<IElementDef> children = new List<IElementDef>();
                if (fChildren != null)
                {
                    children.AddRange(fChildren.Values);
                    // TODO: Add support for looking up the children of Common elements
                    // For example, LibraryPatronStatus_ElectronicIdList/ElectronicId has no children
                    // in it's metadata because it's been re-assigned
                }
                return children.AsReadOnly();
            }
        }

        /// <summary>  Gets the root metadata object</summary>
        /// <returns> The root metadata object
        /// </returns>
        public virtual IElementDef Root
        {
            get
            {
                ElementDefImpl d = this;
                while (d.fParent != null)
                {
                    d = d.fParent;
                }

                return d;
            }
        }

        public virtual string ClassName
        {
            get { return fName; }
        }

        public virtual string FQClassName
        {
            get
            {
                StringBuilder sbuf = new StringBuilder();
                sbuf.Append("OpenADK.Library.");
                if (fVariant != null)
                {
                    sbuf.Append(fVariant);
                    sbuf.Append('.');
                }

                if (fPackage != null)
                {
                    sbuf.Append(fPackage);
                    sbuf.Append('.');
                }
                sbuf.Append(ClassName);
                
                // TODO: Class loading and distinguishing of when a type can be loaded internally 
                // or externally needs to be made smarter.
                if ( fVariant != null )
                {
                    sbuf.Append(", ");
                    sbuf.Append(Adk.Dtd.SDOAssembly);
                    
                }
                return sbuf.ToString();
            }
        }

        public virtual bool IsAttribute(SifVersion version)
        {
            return Info(version).GetFlag(AbstractVersionInfo.FLAG_ATTRIBUTE);
        }

        public bool HasSimpleContent
        {
            get { return fTypeConverter != null; }
        }

        public virtual bool Field
        {
            get { return (fFlags & FD_FIELD) != 0; }
        }


        public virtual bool Object
        {
            get { return (fFlags & FD_OBJECT) != 0; }
        }

        public virtual string Package
        {
            get { return fPackage; }
        }

        public virtual SifVersion EarliestVersion
        {
            get
            {

                for (int i = 0; i < sSifVersions.Length; i++)
                {
                    if (fInfo[i] != null)
                    {
                        return sSifVersions[i];
                    }
                }
                return null;
            }
        }

        public virtual SifVersion LatestVersion
        {
            get { return fLatestVersion; }
        }

        /// <summary>
        /// Adds a child to this ElementDef
        /// </summary>
        /// <param name="child"></param>
        protected internal void addChild(IElementDef child)
        {
            // ElementDefAlias has to add itself to a parent later because
            // it's Name property returns null in the constructor
            if (child.Name == null)
            {
                return;
            }
            if (fChildren == null)
            {
                fChildren = new Dictionary<String, IElementDef>();
            }
            fChildren[child.Name] = child;
        }


        /// <summary>
        /// Defines version-specific metadata
        /// </summary>
        /// <param name="version"></param>
        /// <param name="tag"></param>
        /// <param name="sequence"></param>
        /// <param name="flags"></param>
        public void DefineVersionInfo(SifVersion version,
                                      string tag,
                                      int sequence,
                                      int flags)
        {
            AbstractVersionInfo vi = null;
            
            for (int i = 0; i < sSifVersions.Length; i++)
            {
                if (version == sSifVersions[i])
                {
                    if (fInfo[i] == null)
                    {
                        fInfo[i] = vi = CreateVersionInfo(tag);
                    }
                    else
                    {
                        vi = fInfo[i];
                    }
                    break;
                }
            }

            if (vi == null)
            {
                throw new ArgumentException
                    ("SIF " + version + " is not supported by the Adk");
            }

            if ((flags & FD_DEPRECATED) != 0)
            {
                vi.SetFlag(AbstractVersionInfo.FLAG_DEPRECATED, true);
            }
            if ((flags & FD_REPEATABLE) != 0)
            {
                vi.SetFlag(AbstractVersionInfo.FLAG_REPEATABLE, true);
            }
            if ((flags & FD_ATTRIBUTE) != 0)
            {
                vi.SetFlag(AbstractVersionInfo.FLAG_ATTRIBUTE, true);
            }
            if ((flags & FD_COLLAPSE) != 0)
            {
                vi.SetFlag(AbstractVersionInfo.FLAG_COLLAPSE, true);
            }


            vi.Sequence = sequence;
        }

        /// <summary>
        /// Returns the name of this element
        /// </summary>
        public virtual string Name
        {
            get { return fName; }
        }


        /// <summary>
        /// Returns the Internal Name used by this element.
        /// </summary>
        internal virtual string InternalName
        {
            get { return fName; }
        }

        /// <summary>
        /// Returns the TagName for the specified element, using the specified version
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public virtual string Tag(SifVersion version)
        {
            return Info(version).Tag;
        }


        public virtual string GetSQPPath(SifVersion version)
        {
            StringBuilder b = new StringBuilder();
            if (IsAttribute(version))
            {
                b.Append('@');
            }
            b.Append(Tag(version));
            ElementDefImpl p = fParent;
            while (p != null && !p.Object)
            {
                b.Insert(0, p.Tag(version) + "/");
                p = p.fParent;
            }

            return b.ToString();
        }

        /// <summary>  Returns the version-independent name of this element or attribute</summary>
        /// <seealso cref="Name">
        /// </seealso>
        public override string ToString()
        {
            return fName;
        }

        public virtual int GetSequence(SifVersion version)
        {
            return Info(version).Sequence;
        }

        /// <summary>
        ///  Determines if this metadata describes an element that is contained in the
        ///  specified version of SIF.
        /// </summary>
        /// <param name="version">The version of the SIF Specification</param>
        /// <returns> <c>TRUE</c> if the metadata is included in the specified version of SIF</returns>
        public bool IsSupported(SifVersion version)
        {
            SifVersion earliestVersion = EarliestVersion;
            return (earliestVersion == null || earliestVersion.CompareTo( version ) < 1) &&
                   (fLatestVersion == null || fLatestVersion.CompareTo( version ) > -1);
        }

        public virtual bool IsDeprecated(SifVersion version)
        {
            return Info(version).GetFlag(AbstractVersionInfo.FLAG_DEPRECATED);
        }

        public virtual bool IsRepeatable(SifVersion version)
        {
            return Info(version).GetFlag(AbstractVersionInfo.FLAG_REPEATABLE);
        }

        public TypeConverter TypeConverter
        {
            get { return fTypeConverter; }
        }


        public bool IsCollapsed(SifVersion version)
        {
            return Info(version).GetFlag(AbstractVersionInfo.FLAG_COLLAPSE);
        }

        public bool DoNotEncode
        {
            get { return (fFlags & FD_DO_NOT_ENCODE) != 0; }
        }

        /// <summary>  Lookup the VersionInfo object for a specific version of SIF.</summary>
        private AbstractVersionInfo Info(SifVersion v)
        {
            return GetAbstractVersionInfo(v, true);
        }


        private AbstractVersionInfo CreateVersionInfo(String renderTag)
        {
            if (renderTag.StartsWith("~"))
            {
                // The tilde (~) symbolizes that this version needs a
                // custom surrogate. The Surrogate expression syntax is:
                // "~SurrogateName{constructor}renderAs"
                int surrogateEnd = renderTag.LastIndexOf('}');
                String surrogate = renderTag.Substring(0, surrogateEnd + 1);
                String renderAs = null;
                if (surrogateEnd + 1 < renderTag.Length)
                {
                    renderAs = renderTag.Substring(surrogateEnd + 1);
                }
                if (renderAs == null)
                {
                    renderAs = Name;
                }
                return new SurrogateVersionInfo(renderAs, surrogate, this);
            }
            else
            {
                return new TaggedVersionInfo(renderTag);
            }
        }

        public IElementVersionInfo GetVersionInfo(SifVersion version)
        {
            return GetAbstractVersionInfo(version, false);
        }

        /// <summary>
        /// Lookup the VersionInfo object for a specific version of SIF.
        /// </summary>
        /// <param name="version">v The version of SIF to search</param>
        /// <param name="throwIfNotExists">True if a RuntimeException should be thrown if the version information does not exist. 
        /// If False, NULL will be returned</param>
        /// <returns>The AbstractVersionInfo instance or null</returns>
        private AbstractVersionInfo GetAbstractVersionInfo(SifVersion version, bool throwIfNotExists)
        {
            int last = -1;

            // Search the list of SIFVersions that the ADK supports. The list
            // is searched incrementally, starting with the oldest version. If
            // a version is found that directly matches the requested SIF Version, 
            // return that entry. Otherwise, return the next previous entry from the list
            for (int i = 0; i < sSifVersions.Length; i++)
            {
                int comparison = sSifVersions[i].CompareTo( version );
                if (comparison < 1 && fInfo[i] != null)
                {
                    last = i;
                }

                if (comparison > -1)
                {
                    // We have reached the SIFVersion in the list of supported versions that
                    // is greater than or equal to the requested version. Return the last AbstractVersionInfo
                    // from our array that we found
                    break;
                }
            }

            if (last == -1)
            {
                if (throwIfNotExists)
                {
                    throw new AdkSchemaException("Element or attribute \"" + Name + "\" is not supported in SIF " + version );
                }
                else
                {
                    return null;
                }
            }

            return fInfo[last];
        }


        private abstract class AbstractVersionInfo : IElementVersionInfo
        {
            /**
             *  Flag indicating this is a field that should be rendered as an attribute
             *  of its parent rather than a child of its parent.
             */
            public const int FLAG_ATTRIBUTE = 0x01000000;
            public const int FLAG_REPEATABLE = 0x02000000;
            public const int FLAG_DEPRECATED = 0x04000000;
            public const int FLAG_COLLAPSE = 0x08000000;

            private int fFlags;
            private string fTag;

            protected AbstractVersionInfo(String tag)
            {
                if (tag != null)
                {
                    fTag = string.Intern(tag);
                }
            }

            public int Sequence
            {
                get { return fFlags & 0x00FFFFFF; }
                set
                {
                    if (value >= 0)
                    {
                        fFlags = fFlags | value;
                    }
                }
            }

            /// <summary>
            /// Does this element "collapse" in this version of SIF? This happens with list container elements
            /// in SIF 1.x
            /// </summary>
            public bool IsCollapsed
            {
                get { return GetFlag(FLAG_COLLAPSE); }
            }

            /// <summary>
            /// Is this element repeatable in this version of SIF?
            /// </summary>
            public bool IsRepeatable
            {
                get { return GetFlag(FLAG_REPEATABLE); }
            }

            /// <summary>
            /// Is this an element or attribute in this version of SIF
            /// </summary>
            public bool IsAttribute
            {
                get { return GetFlag(FLAG_ATTRIBUTE); }
            }

            public bool GetFlag(int flag)
            {
                return (flag & fFlags) != 0;
            }

            public void SetFlag(int flag,
                                bool value)
            {
                if (value)
                {
                    fFlags |= flag;
                }
                else
                {
                    fFlags &= ~flag;
                }
            }

            public virtual String Tag
            {
                get { return fTag; }
            }

            /// <summary>
            /// A RenderSurrogate instance, if necessary for rendering this element in this version of SIF
            /// </summary>
            public abstract IRenderSurrogate GetSurrogate();
        }

        private class TaggedVersionInfo : AbstractVersionInfo
        {
            public TaggedVersionInfo(String tag)
                : base(tag)
            {
            }


            /// <summary>
            /// A RenderSurrogate instance, if necessary for rendering this element in this version of SIF
            /// </summary>
            public override IRenderSurrogate GetSurrogate()
            {
                return null;
            }
        }


        private class SurrogateVersionInfo : AbstractVersionInfo
        {
            private IRenderSurrogate fSurrogate;
            private String fInitializer;
            private IElementDef fDef;

            public SurrogateVersionInfo(string renderAs, string surrogateString, IElementDef def)
                : base(renderAs)
            {
                fInitializer = surrogateString;
                fDef = def;
            }

            public override String Tag
            {
                get
                {
                    String tag = base.Tag;
                    if (tag == null)
                    {
                        tag = GetSurrogate().Path;
                    }
                    return tag;
                }
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            public override IRenderSurrogate GetSurrogate()
            {
                if (fSurrogate == null)
                {
                    String surrogateClassName;
                    String initializer = null;
                    int classInitializerStart = fInitializer.IndexOf("{");
                    if (classInitializerStart > -1)
                    {
                        surrogateClassName = fInitializer.Substring(1, classInitializerStart - 1);
                        initializer = fInitializer.Substring(classInitializerStart + 1);
                        if (initializer.Equals("}"))
                        {
                            initializer = null;
                        }
                        else
                        {
                            initializer = initializer.Substring(0, initializer.Length - 1);
                        }
                    }
                    else
                    {
                        surrogateClassName = fInitializer.Substring(1);
                    }
                    if (surrogateClassName.Equals("XPathSurrogate"))
                    {
                        fSurrogate = new XPathSurrogate(fDef, initializer);
                    }
                    else
                    {
                        Type surrogateClass = Type.GetType("OpenADK.Library.Impl.Surrogates." + surrogateClassName);

                        //Is it a locale-specific surrogate?
                        if (surrogateClass == null)
                        {
                            surrogateClass = Type.GetType("OpenADK.Library.Impl.Surrogates." + surrogateClassName + ", " + Adk.Dtd.SDOAssembly);
                        }

                        if (surrogateClass == null)
                        {
                            throw new NotImplementedException
                                ("Surrogate " + fInitializer +
                                 " is not defined in this version of the ADK.");
                        }
                        try
                        {
                            ConstructorInfo c;
                            if (initializer == null)
                            {
                                c =
                                    surrogateClass.GetConstructor
                                        (new Type[] {typeof (IElementDef)});
                                fSurrogate = (IRenderSurrogate) c.Invoke(new object[] {fDef});
                            }
                            else
                            {
                                c = surrogateClass.GetConstructor
                                    (new Type[] {typeof (IElementDef), typeof (String)});
                                fSurrogate =
                                    (IRenderSurrogate)
                                    c.Invoke(new object[] {fDef, initializer});
                            }
                        }
                        catch (Exception iex)
                        {
                            throw new ApplicationException
                                (
                                "Unable to create instance of " + fInitializer + ":" +
                                iex.Message, iex);
                        }
                    }
                }

                return fSurrogate;
            }
        }
    }
}

// Synchronized with ElementDefImpl.java Branch Library-ADK-1.5.0 version 5
