//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    /// <summary>  A simple field value that is strongly typed to match its XSD 
    /// Datatype in the SIF Schema. Unlike complex elements, which are
    /// stored as child objects of their parent, simple fields (i.e. attributes or
    /// elements that have no children) are wrapped in a SimpleField instance and
    /// stored in the field table of their parent object.
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    [Serializable]
    public class SimpleField : Element
    {
        private SifSimpleType fValue;

        /// <summary>
        /// Creates an instance of a SimpleField
        /// </summary>
        /// <param name="def">The Metadata instance representing this field</param>
        protected SimpleField( IElementDef def )
            : base( def ) {}


        /// <summary>
        /// Creates an instance of a SimpleField
        /// </summary>
        /// <param name="def">The Metadata instance representing this field</param>
        /// <param name="parent">The Parent element of this field</param>
        protected SimpleField( IElementDef def,
                               SifElement parent )
            : base( def, parent ) {}


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">The SIFElement that is the parent of this field</param>
        /// <param name="def">The metadata definition of this field</param>
        /// <param name="inValue">A typed subclass of SIFSimpleType</param>
        /// <exception cref="ArgumentNullException">Thrown if the value passed in is null</exception>
        public SimpleField( SifElement parent,
                            IElementDef def,
                            SifSimpleType inValue )
            : base( def, parent )
        {
            if ( inValue == null ) {
                throw new ArgumentNullException
                    (
                    string.Format
                        ( "Cannot construct an instance of {0} with a null value. Create an appropriate SifSimpleType subclass to wrap the null value.",
                          this.GetType().FullName ), "inValue" );
            }

            fValue = inValue;
        }


        /// <summary>
        /// Returns the AdkDataType value of this field
        /// </summary>TypedField
        public override SifSimpleType SifValue
        {
            get { return fValue; }
            set { fValue = value; }
        }


        /// <summary>
        /// Returns the native datatype value of this field
        /// </summary>
        public object Value
        {
            get { return fValue.RawValue; }
        }


        /// <summary>Gets or sets the datatype of this field as a string value. 
        /// The string is parsed using the default SIF formatter, which is the 
        /// SIF 1.x formatter by default</summary>
        /// <value> The text value of this element</value>
        /// <seealso cref="Adk.TextFormatter"/>
    public override string TextValue
        {
            get { return fValue.ToString( Adk.TextFormatter ); }

            set { SetTextValue( value, Adk.TextFormatter ); }
        }

        /// <summary>
        /// Sets the text value of this element using the appropriate formatter
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="formatter"></param>
        public void SetTextValue( string strValue,
                                  SifFormatter formatter )
        {
            fValue = fValue.TypeConverter.Parse( formatter, strValue );
        }

        /// <summary>
        /// Checks the underlying data type and flags to determine if XML
        /// encoding should turned off for this field
        /// </summary>
        public override bool DoNotEncode
        {
            get { return fValue.DoNotEncode | base.DoNotEncode; }
            set { base.DoNotEncode = value; }
        }


        /// <summary>
        /// Creates a copy of this field
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
//            ConstructorInfo ci =
//                this.GetType().GetConstructor
//                    ( new Type []
//                          {typeof ( Element ), typeof ( IElementDef ), typeof ( SifSimpleType )} );
//            if ( ci == null ) {
//                throw new NotSupportedException
//                    ( "Unable to find Clone constructor on type: " + this.GetType().FullName );
//            }

            // The cloned copy will not have the parent field set
            SimpleField fieldCopy = new SimpleField( null, this.ElementDef, fValue );

            //object fieldCopy = ci.Invoke( new object [] {this.Parent, this.ElementDef, fValue} );
            return fieldCopy;
        }


        /// <summary>
        /// Used by the Serialization Formatter
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter = true )]
        protected SimpleField( SerializationInfo info,
                               StreamingContext context )
            : base( info, context )
        {
            info.AddValue( "fValue", Value );
        }

        /// <summary>
        /// Called when the object is being deserialized
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        //protected override void OnGetObjectData( SerializationInfo info,
        //                                         StreamingContext context )
        //{
        //    fValue = (SifSimpleType) info.GetValue( "fValue", typeof ( SifSimpleType ) );
        //}
    }
}
