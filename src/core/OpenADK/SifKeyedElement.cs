//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace OpenADK.Library
{
    /// <summary>
    /// Represents a SIFElement which has a primary key
    /// </summary>
    [Serializable]
    public abstract class SifKeyedElement : SifElement
    {
        /// <summary>
        /// Used by the Serialization Formatter
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
        protected SifKeyedElement( SerializationInfo info,
                                   StreamingContext context )
            : base( info, context ) {}

        /// <summary>
        /// Creates a new instance of a SifKeyedElement
        /// </summary>
        /// <param name="def"></param>
        public SifKeyedElement( IElementDef def )
            : base( def ) {}

        /// <summary>
        /// Returns the key for this object. This is the key used for comparing this object to
        /// other objects in a SIFAction List
        /// </summary>
        public override string Key
        {
            get
            {
                // All Key comparisons are done using the latest
                // SIFVersion
                SifFormatter formatter = Adk.Dtd.GetFormatter( SifVersion.LATEST );
                StringBuilder keyBuilder = new StringBuilder();
                IElementDef [] keys = this.KeyFields;
                for ( int a = 0; a < keys.Length; a++ ) {
                    SimpleField field = GetField( keys[a] );
                    if (field == null)
                    {
                        keyBuilder.Append( (object)null);
                    }
                    else
                    {
                        keyBuilder.Append(field.SifValue.ToString(formatter));
                    }
                    if ( a < (keys.Length - 1) ) {
                        keyBuilder.Append( '.' );
                    }
                }
                return keyBuilder.ToString();
            }
        }

        public abstract IElementDef[] KeyFields
        { get; }


        /// <summary>
        /// Compares the current object's key value to the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyEquals( object key )
        {
            if( key == null )
            {
                return false;
            }
            IElementDef[] keys = this.KeyFields;

            if( keys == null )
            {
                return false;
            }
            if( keys.Length == 1 )
            {
                SimpleField field = GetField( keys[0] );
                if( field != null )
                {
                    SifSimpleType data = field.SifValue;
                    if (key is SifSimpleType )
                    {
                        return key.Equals(data);
                    }
                    else
                    {
                        object fieldValue = data.RawValue;
                        return key.Equals(fieldValue);
                    }
                } 
                else
                {
                    return false;
                }
            } else
            {
                return key.Equals(this.Key);
            }


           
        }
    }
}
