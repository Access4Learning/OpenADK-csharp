using System;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    public abstract class AbstractDef
    {
        protected AbstractDef( string name )
        {
            this.Name = name;
        }

        /// <summary>A description of this object/field</summary>
        public virtual String Desc
        {
            get { return fDesc; }

            set { fDesc = value; }
        }

        /// <summary>The name of this object/field 
        /// </summary>
        public virtual String Name
        {
            get { return fName; }

            set { fName = value; }
        }


        /// <summary> 	If the sequence # of this object or element has been overridden, this is
        /// the sequence # that will be assigned to the ADK's ElementDef instance for
        /// this object or element. Otherwise, -1.
        /// </summary>
        public virtual int SequenceOverride
        {
            get { return fSeqOverride; }

            set { fSeqOverride = value; }
        }

        /// <summary>
        /// Whether or not to validate this element, defaults to true
        /// </summary>
        public virtual bool ShouldValidate
        {
            get { return fValidate; }
            set { fValidate = value; }
        }

        public virtual bool Deprecated
        {
            get { return (fFlags & FLAG_DEPRECATED) != 0; }
        }

        public virtual void SetFlags( string flag )
        {
            String ff = flag.ToUpper();

            if ( ff.ToUpper().Equals( "O".ToUpper() ) ) {
                fFlags |= FLAG_OPTIONAL;
            }
            else if ( ff.ToUpper().Equals( "R".ToUpper() ) ) {
                fFlags |= FLAG_REQUIRED;
            }
            else if ( ff.ToUpper().Equals( "M".ToUpper() ) ) {
                fFlags |= FLAG_MANDATORY;
            }
            else if ( ff.ToUpper().Equals( "C".ToUpper() ) ) {
                fFlags |= FLAG_CONDITIONAL;
            }
            else if ( ff.ToUpper().Equals( "MR".ToUpper() ) ) {
                fFlags |= (FLAG_MANDATORY | FLAG_REPEATABLE);
            }
            else if ( ff.ToUpper().Equals( "OR".ToUpper() ) ) {
                fFlags |= (FLAG_OPTIONAL | FLAG_REPEATABLE);
            }
            else if ( ff.ToUpper().Equals( "CR".ToUpper() ) ) {
                fFlags |= (FLAG_CONDITIONAL | FLAG_REPEATABLE);
            }
        }

        /// <summary>
        /// Gets the Field's Flags as a string
        /// </summary>
        /// <returns></returns>
        public virtual string GetFlags()
        {
            if ( (fFlags & FLAG_REQUIRED) > 0 ) {
                return "R";
            }
            else {
                string returnVal;
                if ( (fFlags & FLAG_MANDATORY) > 0 ) {
                    returnVal = "M";
                }
                else if ( (fFlags & FLAG_CONDITIONAL) > 0 ) {
                    returnVal = "C";
                }
                else {
                    // Default to Optional, if nothing else specified
                    returnVal = "O";
                }

                if ( (fFlags & FLAG_REPEATABLE) > 0 ) {
                    returnVal += "R";
                }
                return returnVal;
            }
        }

        public bool FlagIntrinsicallyMatches( FieldDef counterpart )
        {
            if (fFlags == counterpart.fFlags)
            {
                return true;
            }
            else if ((fFlags & FLAG_REPEATABLE) == 0 &&
                ((fFlags & FLAG_REQUIRED) > 0 || (fFlags & FLAG_MANDATORY) > 0) )
            {
                return ((counterpart.fFlags & FLAG_REQUIRED) > 0 ||
                        (counterpart.fFlags & FLAG_MANDATORY) > 0 );
                
            }
            return GetFlags() == counterpart.GetFlags();
        }



        /// <summary>Flags describing characteristics of this object/field 
        /// </summary>
        public virtual int Flags
        {
            get { return fFlags; }

            set { fFlags = value; }
        }

        public virtual bool Draft
        {
            get { return (fFlags & FLAG_DRAFTOBJECT) != 0; }
        }

        /// <summary>  Gets the earliest version of SIF this definition applies to.
        /// </summary>
        /// <summary>  Sets the earliest version of SIF this definition applies to.
        /// *
        /// If the version is earlier than the current earliest version, it becomes
        /// the earliest version and will be returned by the getEarliestVersion
        /// method. Otherwise no action is taken.
        /// </summary>
        public virtual SifVersion EarliestVersion
        {
            get { return fMinVersion; }

            set
            {
                if ( fMinVersion == null || value.CompareTo( fMinVersion ) < 0 ) {
                    fMinVersion = value;
                }
            }
        }

        /// <summary>  Gets the latest version of SIF this definition applies to.
        /// </summary>
        /// <summary>  Sets the latest version of SIF this definition applies to.
        /// </summary>
        public virtual SifVersion LatestVersion
        {
            get { return fMaxVersion; }

            set
            {
                fMaxVersion = value;
                if ( fMinVersion == null ) {
                    fMinVersion = value;
                }
            }
        }

        public const int FLAG_REQUIRED = 0x00000001;
        public const int FLAG_REPEATABLE = 0x00000002;
        public const int FLAG_OPTIONAL = 0x00000004;
        public const int FLAG_MANDATORY = 0x00000008;
        public const int FLAG_CONDITIONAL = 0x00000010;
        public const int FLAG_DEPRECATED = 0x00000020;
        public const int FLAG_DRAFTOBJECT = 0x00000040;
        public const int FLAG_NO_SIFDTD = 0x00000080;

        /// <summary>A description of this object/field 
        /// </summary>
        protected internal String fDesc;

        /// <summary>The name of this object/field 
        /// </summary>
        protected internal String fName;

        /// <summary>Flags describing characteristics of this object/field 
        /// </summary>
        protected internal int fFlags;

        /// <summary>The earliest version of SIF this definition applies to 
        /// </summary>
        protected internal SifVersion fMinVersion;

        /// <summary>The latest version of SIF this definition applies to 
        /// </summary>
        protected internal SifVersion fMaxVersion;

        /// 
        /// <summary> 	If the sequence # of this object or element has been overridden, this is
        /// the sequence # that will be assigned to the ADK's ElementDef instance for
        /// this object or element. Otherwise, -1.
        /// </summary>
        protected internal int fSeqOverride = - 1;


        public virtual void setDraft()
        {
            fFlags |= FLAG_DRAFTOBJECT;
        }

        /// <summary>
        /// The source file that generated this object
        /// </summary>
        public string SourceLocation
        {
            get { return fSourceLocation; }
            set { fSourceLocation = value; }
        }

        /// <summary>
        /// Whether or not to validate this element, defaults to true
        /// </summary>
        private bool fValidate = true;

        /// <summary>
        /// The source file that generated this object
        /// </summary>
        private string fSourceLocation;
    }
}