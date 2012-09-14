using System;
using System.Collections;
using System.Collections.Generic;
using Edustructures.Metadata.DataElements;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary>  Encapsulates a DataObject (<object>) or Infrastructure Message (<infra>) definition
    /// *
    /// *
    /// </summary>
    public class ObjectDef : AbstractDef
    {
        public const int FLAG_TOPICOBJECT = 0x00100000;
        public const int FLAG_EMPTYOBJECT = 0x00200000;
        public const int FLAG_INFRAOBJECT = 0x00400000;

        internal Dictionary<String, FieldDef> fFields = new Dictionary<String, FieldDef>();
        private int fFieldSeq = 1;
        private string mPackage;
        private string mSuperClass;
        private string mExtrasFile;
        private string mRenderAs;


        /**
         * If this object accepts a text value, holds the datatype of that value.
         */
        private FieldType fValueType;


        /// <summary>  Gets the local package name where this object's class should be generated.
        /// The local package name excludes the "com.edustructures.sifworks." prefix
        /// </summary>
        public virtual string LocalPackage
        {
            get { return mPackage; }
            set { mPackage = value; }
        }

        /// <summary>  Gets the name used to represent this object in the DTD class. A static
        /// String is defined with this name, having a value equal to the string
        /// returned by getName.
        /// </summary>
        public virtual string DTDSymbol
        {
            get { return Name.ToUpper(); }
        }

        /// <summary>  Gets the element tag name
        /// </summary>
        public override string Name
        {
            get { return fName; }
        }

        public virtual string ExtrasFile
        {
            get { return mExtrasFile; }

            set { mExtrasFile = value; }
        }

        public virtual string Superclass
        {
            get { return mSuperClass; }

            set { mSuperClass = value; }
        }

        public virtual string RenderAs
        {
            get { return mRenderAs; }

            set { mRenderAs = value; }
        }

        public virtual string fTag
        {
            get
            {
                if ( mRenderAs == null ) {
                    return fName;
                }
                return mRenderAs;
            }
        }

        public virtual bool Empty
        {
            get { return (fFlags & FLAG_EMPTYOBJECT) != 0; }
        }

        /// <summary>  Is this object a top-level SIF object such as StudentPersonal?
        /// </summary>
        /// <summary>  Sets the topic flag
        /// </summary>
        public virtual bool Topic
        {
            get { return (fFlags & FLAG_TOPICOBJECT) != 0; }

            set
            {
                if ( value ) {
                    fFlags |= FLAG_TOPICOBJECT;
                }
                else {
                    fFlags &= ~ FLAG_TOPICOBJECT;
                }
            }
        }

        /// <summary>  Is this an <infra> object describing a SIF Infrastructure message?
        /// </summary>
        public virtual bool Infra
        {
            get { return (fFlags & FLAG_INFRAOBJECT) != 0; }
        }

        /// <summary>  Does this object serve as the superclass for one or more subclasses?
        /// </summary>
        public virtual bool Shared
        {
            get { return fShared; }

            set { fShared = value; }
        }

        public virtual FieldDef[] Fields
        {
            get
            {
                FieldDef[] arr = new FieldDef[fFields.Count];
                fFields.Values.CopyTo( arr, 0 );

                //  Sort by sequence # first
                Array.Sort( arr );

                return arr;
            }
        }

        public IDictionary GetAllFields()
        {
            return fFields;
        }


        public virtual FieldDef[] Attributes
        {
            get { return GetFields( FieldDef.FLAG_ATTRIBUTE ); }
        }

        public virtual FieldDef[] Elements
        {
            get { return GetFields( FieldDef.FLAG_ELEMENT ); }
        }

        /// <summary>  Get all attributes and elements that are marked mandatory (M) or
        /// required (R)
        /// </summary>
        public virtual FieldDef[] MandatoryFields
        {
            get { return GetFields( FLAG_MANDATORY | FLAG_REQUIRED ); }
        }

        /// <summary>  For complex fields, returns the names of the fields that serve as the
        /// object's key. By default this method returns all attributes marked with
        /// an "R" flag (and if no attributes exist or none are marked with an "R"
        /// flag, returns all elements marked with an "R" flag).
        /// </summary>
        public virtual FieldDef[] Key
        {
            get
            {
                ArrayList v = new ArrayList();

                FieldDef[] attrs = Attributes;

                int loop = 0;
                while ( attrs != null ) {
                    for ( int i = 0; i < attrs.Length; i++ ) {
                        int f = attrs[i].Flags;
                        if ( ((f & FLAG_REQUIRED) != 0 || (f & FLAG_MANDATORY) != 0) &&
                             ((f & FieldDef.FLAG_NOT_A_KEY) == 0) ) {
                            v.Add( attrs[i] );
                        }
                    }

                    //  There are a few inconsistencies in the SIF schema such that some
                    //  objects' keys are described by elements, not attributes. So, if we
                    //  get here and have no keys, loop again processing element fields
                    //  instead of attribute fields.
                    //
                    if ( v.Count == 0 && loop == 0 ) {
                        attrs = Elements;
                    }
                    else {
                        attrs = null;
                    }

                    loop++;
                }

                FieldDef[] arr = new FieldDef[v.Count];
                v.CopyTo( arr );
                Array.Sort( arr );
                return arr;
            }
        }

        /// <summary>  The shared flag indicates this object serves as the superclass for
        /// one or more sub-classes. For example, "CountryOfResidency" uses "Country"
        /// as its superclass, so the "Country" object is marked as shared in the
        /// definition file.
        /// </summary>
        private bool fShared;

        /// <summary>  Constructor
        /// </summary>
        /// <param name="id">Sequence number of this object
        /// </param>
        /// <param name="name">Element name (e.g. "StudentPersonal", "OtherId", etc.)
        /// </param>
        /// <param name="localPackage">Package name (e.g. "common", "student", "food", etc.)
        /// </param>
        /// <param name="version">Version of SIF this object is being defined for
        /// 
        /// </param>
        public ObjectDef( int id,
                          string name,
                          string srcLocation,
                          string localPackage,
                          SifVersion version )
            : base( name )
        {
            this.SourceLocation = srcLocation;
            this.LocalPackage = localPackage;
            //this.LatestVersion =
        }


        /// <summary>  Indicates this is an <infra> object describing a SIF Infrastructure message
        /// </summary>
        public virtual void setInfra()
        {
            fFlags |= FLAG_INFRAOBJECT;
        }


        public virtual FieldDef DefineAttr( string name,
                                            string classType )
        {
            return DefineAttrOrElement( name, classType, FieldDef.FLAG_ATTRIBUTE );
        }

        public virtual FieldDef DefineElement( string name,
                                               string classType )
        {
            return DefineAttrOrElement( name, classType, FieldDef.FLAG_ELEMENT );
        }

        private FieldDef DefineAttrOrElement( string name,
                                              string classType,
                                              int type )
        {
            FieldDef def = null;

            if ( fFields.ContainsKey( name ) ) {
                def = fFields[name];
            }
            else {
                def = new FieldDef( this, name, classType, fFieldSeq++, type );
                fFields[name] = def;
            }

            return def;
        }

        public virtual FieldDef GetField( string name )
        {
            if ( fFields.ContainsKey( name ) ) {
                return (FieldDef) fFields[name];
            }
            return null;
        }


        public String PackageQualifiedDTDSymbol
        {
            get { return PackageDTDName + "." + DTDSymbol; }
        }

        public String PackageDTDName
        {
            get { return mPackage.Substring( 0, 1 ).ToUpper() + mPackage.Substring( 1 ) + "DTD"; }
        }

        public void SetEnumType( string enumType )
        {
            fValueType = FieldType.ToEnumType( FieldType.GetFieldType( "String" ), enumType );
        }


        private FieldDef[] GetFields( int flags )
        {
            ArrayList v = new ArrayList();
            foreach ( FieldDef f in fFields.Values ) {
                if ( (f.Flags & flags) != 0 ) {
                    v.Add( f );
                }
            }

            if ( (flags & FLAG_MANDATORY) != 0 ) {
                FieldDef valueDef = GetValueDef();
                if ( valueDef != null ) {
                    v.Add( valueDef );
                }
            }


            FieldDef[] arr = new FieldDef[v.Count];
            v.CopyTo( arr );

            Array.Sort( arr );

            return arr;
        }

        public FieldDef GetValueDef()
        {
            FieldDef returnValue = null;
            FieldType valueType = GetValueType();
            if ( valueType != null ) {
                try {
                    returnValue =
                        new FieldDef
                            ( this, "Value", valueType, 999,
                              FieldDef.FLAG_TEXT_VALUE | FLAG_MANDATORY );
                }
                catch ( ParseException parseEx ) {
                    Console.WriteLine( parseEx );
                    throw parseEx;
                }
                returnValue.Desc = "Gets or sets the content value of the &lt;" + this.fName +
                                   "&gt; element";
                returnValue.EarliestVersion = this.EarliestVersion;
                returnValue.LatestVersion = this.LatestVersion;
                // returnValue.ElementDefConst =  this.PackageQualifiedDTDSymbol;
            }
            return returnValue;
        }


        /**
 *  Does this element have a text value?
 *  @return true if this element has no elements or attributes, or it has
 *      no elements but at least one attribute and the FLAG_EMPTYOBJECT flag
 *      
 *      is not set
 */

        public FieldType GetValueType()
        {
            if ( fValueType == null ) {
                if ( (fFlags & FLAG_EMPTYOBJECT) == 0 ) {
                    if ( fFields == null || fFields.Count == 0 || GetElements().Length == 0 ) {
                        fValueType = FieldType.GetFieldType( "String" );
                    }
                }
            }
            return fValueType;
        }


        /// <summary>
        /// If this this object accepts an element value the datatype of that 
        /// value is set in the metadata using a "datatype" element/// </summary>
        /// <param name="dataType"></param>
        public void SetDataType( String dataType )
        {
            fValueType = FieldType.GetFieldType( dataType );
        }


        /// <summary>
        ///  Does this element have a text value?
        /// </summary>
        /// <returns>true if this element has no elements or attributes, or it has
        /// no elements but at least one attribute and the FLAG_EMPTYOBJECT flag
        /// is not set</returns>
        public FieldType ValueType
        {
            get
            {
                if ( fValueType == null ) {
                    if ( (fFlags & FLAG_EMPTYOBJECT) == 0 ) {
                        if ( fFields == null || fFields.Count == 0 || GetElements().Length == 0 ) {
                            fValueType = FieldType.GetFieldType( "String" );
                        }
                    }
                }
                return fValueType;
            }
        }

        public FieldDef[] GetElements()
        {
            return GetFields( FieldDef.FLAG_ELEMENT );
        }
    }
}