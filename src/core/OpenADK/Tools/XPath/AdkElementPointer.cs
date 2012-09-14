//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Tools.XPath
{
    public abstract class AdkElementPointer : AbstractNodePointer
    {
        protected IElementDef fElementDef;
        protected Element fElement;
        private SifVersion fVersion;

        /// <summary>
        /// Creates a new AdkElementPointer
        /// </summary>
        /// <param name="parentPointer">The parent of this element</param>
        /// <param name="element">The element being wrapped</param>
        /// <param name="version">The SifVersion in use</param>
        /// within its parent list</param>
        protected AdkElementPointer( INodePointer parentPointer, Element element, SifVersion version )
            : base( parentPointer )
        {
            fVersion = version;
            fElement = element;
            fElementDef = element.ElementDef;
        }

        public override string Name
        {
            get { return fElementDef.Tag( fVersion ); }
        }

        /// <summary>
        /// Gets the value that this NodePointer points to
        /// </summary>
        public override object GetBaseValue()
        {
            return fElement;
        }

        /// <summary>
        /// returns the raw node pointed to by this pointer
        /// </summary>
        public override object Node
        {
            get { return fElement; }
        }


        public Element Element
        {
            get { return fElement; }
        }

        /// <summary>
        /// The SifVersion in effect for this object
        /// </summary>
        protected internal SifVersion Version
        {
            get { return fVersion; }
        }


        protected SifSimpleType GetSIFSimpleTypeValue( IElementDef def, object rawValue )
        {
            if ( rawValue is SifSimpleType )
            {
                return (SifSimpleType) rawValue;
            }
            SifSimpleType sst = def.TypeConverter.GetSifSimpleType( rawValue );
            return sst;
        }

        public bool IsLegacyVersion
        {
            get { return fVersion.Major < 2; }
        }
    }
}
