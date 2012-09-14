using System;
using System.Collections.Generic;

namespace Edustructures.Metadata
{
    /// <summary>  An enumeration of valid values for a field.
    /// *
    /// </summary>
    public class EnumDef : AbstractDef
    {
        public EnumDef( string name,
                        string sourceFile )
            : base( name )
        {
            this.SourceLocation = sourceFile;
        }

        public ICollection<ValueDef> Values
        {
            get { return fValues.Values; }
        }

        /// <summary>  Gets the local package name where this object's class should be generated.
        /// The local package name excludes the "com.edustructures.sifworks." prefix
        /// </summary>
        public virtual String LocalPackage
        {
            get { return fPackage; }
            set { fPackage = value; }
        }

        private string fPackage;

        protected internal IDictionary<String, ValueDef> fValues = new Dictionary<String, ValueDef>();

        public virtual void DefineValue( string name,
                                         string val,
                                         string desc )
        {
            fValues[val] = new ValueDef( name, val, desc );
        }

        public bool ContainsValue( string value )
        {
            return fValues.ContainsKey( value );
        }
    }
}