using System;
using System.Collections;
using Edustructures.Metadata.DataElements;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary> Title:
    /// Description:
    /// Copyright:    Copyright (c) 2002
    /// Company:
    /// </summary>
    /// <author> 
    /// </author>
    /// <version>  1.0
    /// 
    /// </version>
    public class FieldDef : AbstractDef, IComparable
    {
        private FieldType fFieldType;

        public FieldType FieldType
        {
            get { return fFieldType; }
            set { fFieldType = value; }
        }

        private string fSuperclass;
        private int fSequence;
        private ObjectDef fParent;
        private string fRenderAs;
        protected String fDtdSymbol;
        private String fElementDefConst;

        /// <summary>  If this field is known by more than one tag, or has different sequence
        /// numbers for different versions of SIF, an Alias is added to the table
        /// by the DB.mergeInto method. Alias entries are keyed by SIF version string.
        /// </summary>
        private Hashtable fAliases;


        public FieldDef( ObjectDef parent,
                         String name,
                         String classType,
                         int sequence,
                         int nodeTypeFlag )
            : this( parent, name, FieldType.GetFieldType( classType ), sequence, nodeTypeFlag ) {}

        public FieldDef( ObjectDef parent,
                         String name,
                         FieldType fieldType,
                         int sequence,
                         int nodeTypeFlag )
            : base( name )
        {
            fParent = parent;
            fSequence = sequence;
            fFieldType = fieldType;
            fFlags = nodeTypeFlag;

            if ( fFieldType.IsComplex ) {
                if ( fFlags == FLAG_ATTRIBUTE ) {
                    throw new ParseException
                        (
                        "Cannot create an attribute as a ComplexType: " +
                        parent.Name + "." + name + " {" + fieldType.ClassType + "}" );
                }
                fFlags |= FLAG_COMPLEX;
                if ( fieldType.ClassType.Equals( name ) ) {
                    fSuperclass = fieldType.ClassType;
                }
            }
            fDtdSymbol = fParent.DTDSymbol + "_" + fName.ToUpper();
            fElementDefConst = fParent.PackageQualifiedDTDSymbol + "_" + (fName.ToUpper());
        }

        public virtual Hashtable Aliases
        {
            get { return fAliases; }
        }

        /// <summary>  Gets the name used to represent this object in the DTD class.
        /// *
        /// A static String is defined in the DTD class having the name "parent_this",
        /// where "parent" is the value returned by the parent ObjectDef's getDTDSymbol
        /// and "this" is the name of this field in uppercase (e.g. "STUDENTPERSONAL_REFID").
        /// The value of that static will be the string returned by getName.
        /// </summary>
        public virtual string DTDSymbol
        {
            get { return fDtdSymbol; }
        }

        public override string Name
        {
            get { return fName; }
        }

        public virtual int Sequence
        {
            get
            {
                if ( fSeqOverride != - 1 ) {
                    return fSeqOverride;
                }

                return fSequence;
            }
        }

        public virtual string Superclass
        {
            get { return fSuperclass; }

            set { fSuperclass = value; }
        }

        public virtual string RenderAs
        {
            get { return fRenderAs; }

            set { fRenderAs = value; }
        }

        public virtual string Tag
        {
            get
            {
                if ( fRenderAs == null ) {
                    return fName;
                }
                return fRenderAs;
            }
        }

        public bool Attribute
        {
            get { return (fFlags & FLAG_ATTRIBUTE) != 0; }
        }

        public virtual bool Complex
        {
            get { return (fFlags & FLAG_COMPLEX) != 0; }
        }

        /// <summary>  For complex fields, returns the names of the fields that serve as the
        /// object's key. By default this method returns all attributes marked with
        /// an "R" flag. For nearly all SIF objects this method returns a single
        /// key named "RefId".
        /// </summary>
        public virtual FieldDef[] Key
        {
            get
            {
                ArrayList v = new ArrayList();
                FieldDef[] attrs = fParent.Attributes;
                for ( int i = 0; i < attrs.Length; i++ ) {
                    if ( ((attrs[i].Flags & FLAG_REQUIRED) != 0) &&
                         ((attrs[i].Flags & FLAG_NOT_A_KEY) == 0) ) {
                        v.Add( attrs[i] );
                    }
                }

                FieldDef[] arr = new FieldDef[v.Count];
                v.CopyTo( arr );
                return arr;
            }
        }

        public const int
            FLAG_DO_NOT_ENCODE = 0x00001000,
            FLAG_ELEMENT = 0x10000000,
            FLAG_ATTRIBUTE = 0x20000000,
            FLAG_COMPLEX = 0x40000000,
            FLAG_NOT_A_KEY = 0x01000000,
            FLAG_TEXT_VALUE = 0x02000000,
            FLAG_COLLAPSED = 0x04000000;


        public virtual void addAlias( SifVersion version,
                                      string tag,
                                      int sequence )
        {
            if ( fAliases == null ) {
                fAliases = new Hashtable();
            }
            Alias a = (Alias) fAliases[version.ToString()];
            if ( a != null ) {
                throw new MergeException( Tag + " alias already defined for " + version );
            }

            a = new Alias();
            a.tag = tag;
            a.sequence = sequence;
            string verStr = version.Major + version.Minor.ToString() +
                            (version.Revision == 0 ? "" : "r" + version.Revision);
            fAliases[verStr] = a;
        }

        #region IComparable Members

        public int CompareTo( object obj )
        {
            int cmp = ((FieldDef) obj).fSequence;
            if ( fSequence < cmp ) {
                return - 1;
            }
            if ( fSequence > cmp ) {
                return 1;
            }

            return 0;
        }

        #endregion

        internal void SetEnum( string enumName )
        {
            if ( enumName != null ) {
                fFieldType = FieldType.ToEnumType( fFieldType, enumName );
            }
        }

        internal void Validate() {}
    }
}