//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Impl
{
    /// <summary>  An IElementDef that accepts an implementation class name as one of its
    /// constructor parameters and returns that class name in the getClassName
    /// method. This class is used only for those elements where the name of the
    /// implementation class (e.g. "Country") is different than the name of the
    /// element tag (e.g. "CountryOfBirth").
    /// </summary>
    public class ElementDefAlias : ElementDefImpl
    {
        /// <summary> 	Return the class name of this alias IElementDef, which will be different
        /// than the tag name (e.g. "Country" for an alias that represents 
        /// "CountryOfBirth")
        /// </summary>
        public override string ClassName
        {
            get { return fClassName; }
        }


        protected internal string fClassName;


        /// <summary>
        /// Constructs an ElementDefAlias
        /// </summary>
        /// <param name="parent">The parent of this element</param>
        /// <param name="name">The name of the element</param>
        /// <param name="tag">The name of the element</param>
        /// <param name="className">The name of the class to create</param>
        /// <param name="localPackage">The name of the package where the corresponding
        /// DataObject class is defined, excluding the <c>OpenADK.Library</c> prefix</param>
        /// <param name="sequence">The zero-based ordering of this element within its parent
        /// or -1 if a top-level element</param>
        /// <param name="earliestVersion">The earliest version of SIF supported by this element</param>
        public ElementDefAlias(
            IElementDef parent,
            string name,
            string tag,
            string className,
            int sequence,
            string localPackage,
            SifVersion earliestVersion )
            :
                base( parent, name, tag, sequence, localPackage, sequence, earliestVersion, null )
        {
            fClassName = className;
        }

        /// <summary>  Constructs an ElementDefAlias with flag
        /// </summary>
        /// <param name="parent">The parent of this element
        /// </param>
        /// <param name="name"></param>
        /// <param name="tag">The name of the element
        /// </param>
        /// <param name="className"></param>
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
        /// as StudentPersonal or an infrastructure message such as SIF_Ack
        /// </param>
        /// <param name="earliestVersion">The earliest version of SIF supported by this element
        /// </param>
        /// <param name="latestVersion"></param>
        public ElementDefAlias(
            IElementDef parent,
            string name,
            string tag,
            string className,
            int sequence,
            string localPackage,
            string variant,
            int flags,
            SifVersion earliestVersion,
            SifVersion latestVersion )
            :
                base( parent, name, tag, sequence, localPackage, variant, flags, earliestVersion, latestVersion, null )
        {
            fClassName = className;
        }

        /// <summary>
        ///  Constructs an ElementDefAlias with flag
        /// </summary>
        /// <param name="parent">The parent of this element</param>
        /// <param name="name">The name of the element</param>
        /// <param name="tag"></param>
        /// <param name="className"></param>
        /// <param name="sequence">The zero-based ordering of this element within its parent
        /// or -1 if a top-level element</param>
        /// <param name="localPackage">The name of the package where the corresponding
        /// DataObject class is defined, excluding the
        /// <c>OpenADK.Library</c> prefix</param>
        /// <param name="flags">One of the following: FD_ATTRIBUTE if this element should
        /// be rendered as an attribute of its parent rather than a child
        /// element; FD_FIELD if this element is a simple field with no child
        /// elements; or FD_OBJECT if this element is a SIF Data Object such
        /// as StudentPersonal or an infrastructure message such as SIF_Ack</param>
        /// <param name="earliestVersion">he earliest version of SIF supported by this element</param>
        /// <param name="latestVersion"></param>
        /// <param name="converter"></param>
        public ElementDefAlias(
            IElementDef parent,
            string name,
            string tag,
            string className,
            int sequence,
            string localPackage,
            string variant,
            int flags,
            SifVersion earliestVersion,
            SifVersion latestVersion,
            TypeConverter converter )
            : base( parent, name, tag, sequence, localPackage, variant, flags, earliestVersion, latestVersion, converter )
        {
            fClassName = className;
            if ( parent != null ) {
                ((ElementDefImpl) parent).addChild( this );
            }
        }

        /// <summary> 	Return the name of this alias IElementDef, overridden here to be the same 
        /// as the classname regardless of what is passed to the constructor. Note the
        /// name will be different than the tag name for an alias (e.g. "Country" is
        /// returned for an alias that represents "CountryOfBirth")
        /// </summary>
        public override string Name
        {
            get { return fClassName; }
        }


        internal override string InternalName
        {
            get { return fName; }
        }
    }
}
