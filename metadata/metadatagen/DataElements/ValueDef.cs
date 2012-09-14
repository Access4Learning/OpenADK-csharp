using System;

namespace Edustructures.Metadata
{
    public class ValueDef : AbstractDef
    {
        public virtual String Value
        {
            get { return fValue; }
        }

        protected internal String fValue;

        public ValueDef( string tag,
                         string val,
                         string desc )
            : base( tag )
        {
            fValue = val;
            fDesc = desc;
        }
    }
}