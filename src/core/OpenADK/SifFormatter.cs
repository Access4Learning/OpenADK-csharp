//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library.Impl;
using OpenADK.Util;
using System.Xml;

namespace OpenADK.Library
{
    /// <summary>
    /// Contains functions helpful for converting data to and from native datatypes to SIF datatypes
    /// </summary>
    public abstract class SifFormatter
    {
        /// <summary>
        /// Converts a <code>DateTime?</code> value to a String XML representation as an XML date
        /// </summary>
        /// <param name="date">A nullable DateTime to convert</param>
        /// <returns></returns>
        public abstract String ToDateString( DateTime? date );

        /// <summary>
        /// Converts a <code>DateTime?</code> value to a String XML representation as an XML dateTime
        /// </summary>
        /// <param name="date">A nullable DateTime to convert</param>
        /// <returns></returns>
        public abstract String ToDateTimeString( DateTime? date );

        /// <summary>
        /// Converts a <code>DateTime?</code> value to a String XML representation as an XML dateTime
        /// </summary>
        /// <param name="time">A nullable DateTime to convert</param>
        /// <returns></returns>
        public abstract String ToTimeString( DateTime? time );


        /// <summary>
        /// Converts an <code>int?</code> value to a String XML representation as an XML int
        /// </summary>
        /// <param name="intValue">A nullable int to convert</param>
        /// <returns></returns>
        public abstract String ToString( int? intValue );


        /// <summary>
        /// Converts an <code>int?</code> value to a String XML representation as an XML long
        /// </summary>
        /// <param name="longValue">A nullable long to convert</param>
        /// <returns></returns>
        public abstract String ToString(long? longValue);

        /// <summary>
        /// Converts an <code>float?</code> value to a String XML representation as an XML float
        /// </summary>
        /// <param name="floatValue">A nullable float to convert</param>
        /// <returns></returns>
        public String ToString(float? floatValue)
        {
            if (floatValue.HasValue)
            {
                return XmlConvert.ToString( floatValue.Value );
            } else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a <code>Decimal?</code> value to a String XML representation as an XML Decimal
        /// </summary>
        /// <param name="decimalValue">A nullable Decimal to convert</param>
        /// <returns></returns>
        public abstract String ToString( decimal? decimalValue );


        /// <summary>
        /// Converts a <code>DateTime?</code> value to a String XML representation as an XML dateTime
        /// </summary>
        /// <param name="boolValue">A nullable DateTime to convert</param>
        /// <returns></returns>
        public abstract String ToString( bool? boolValue );


        /// <summary>
        /// Converts a <code>TimeSpan?</code> value to a String XML representation as an XML duration
        /// </summary>
        /// <param name="duration">A nullable TimeSpan to convert</param>
        /// <returns></returns>
        public abstract String ToString( TimeSpan? duration );


        /// <summary>
        /// Converts a String XML representation of an XML date to a <code>DateTime?</code> value
        /// </summary>
        /// <param name="dateValue">A nullable DateTime to convert</param>
        /// <returns></returns>
        public abstract DateTime? ToDate( String dateValue );


        /// <summary>
        /// Converts a String XML representation of an XML datetime to a <code>DateTime?</code> value
        /// </summary>
        /// <param name="xmlValue">A nullable DateTime to convert</param>
        /// <returns></returns>
        public abstract DateTime? ToDateTime( String xmlValue );

        /// <summary>
        /// Converts a String XML representation of an XML time to a <code>DateTime?</code> value
        /// </summary>
        /// <param name="xmlValue">A nullable DateTime to convert</param>
        /// <returns></returns>
        public abstract DateTime? ToTime( String xmlValue );

        /// <summary>
        /// Converts a SIF XML integer value to a  .Net <code>int?</code> value
        /// </summary>
        public abstract int? ToInt( String intValue );

        /// <summary>
        /// Converts a SIF XML integer value to a  .Net <code>long?</code> value
        /// </summary>
        public abstract long? ToLong(String longValue);


        /// <summary>
        /// Converts a SIF XML decimal value to a  .Net <code>decimal?</code> value
        /// </summary>
        public abstract decimal? ToDecimal( String intValue );

        /// <summary>
        /// Converts a SIF XML float value to a  .Net <code>float?</code> value
        /// </summary>
        public float? ToFloat(String floatValue)
        {
            if (floatValue == null)
            {
                return null;
            }
            floatValue = floatValue.Trim();
            if (floatValue.Length == 0)
            {
                return null;
            }
            return System.Xml.XmlConvert.ToSingle( floatValue );
          
        }

        /// <summary>
        /// Converts a SIF XML duration value to a .NET <code>TimeSpan?</code> valueS
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract TimeSpan? ToTimeSpan( string value );


        /// <summary>
        /// Converts a SIF XML boolean value to a  .Net <code>bool?</code> value
        /// </summary>
        public abstract bool? ToBool( String inValue );


        /// <summary>
        /// Returns true if the formatter supports XML Namespaces.
        /// If this value returns true, XML Namespaces will be declared when writing
        /// SIF Elements, and the xsi:nil attribute will be enabled
        /// </summary>
        public abstract bool SupportsNamespaces { get; }


        /// <summary>
        /// Returns the Encoding implementation used to encode SIF XML documents to binary transports
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                Encoding enc = new UTF8Encoding( false );
                return enc;
            }
        }


        /// <summary>
        /// Converts a Guid to a string using the SIF format
        /// </summary>
        /// <param name="guid">the Guid to convert</param>
        /// <returns>the Guid formatted as a SIF refid or NULL if the Guid? passed in is null</returns>
        public static string GuidToSifRefID( Guid? guid )
        {
            if ( !guid.HasValue ) {
                return null;
            }
            return guid.Value.ToString( "N" ).ToUpper();
        }

        /// <summary>
        /// Converts a SIF RefId to a .Net Guid
        /// </summary>
        /// <param name="refid">the string formatted as a SIF refid</param>
        /// <returns>a Guid</returns>
        /// <exception cref="ArgumentNullException">Thrown if the argument is null</exception>
        /// <exception cref="ArgumentException">Thrown if the input string does not appear to be a valid GUID</exception>
        /// 
        public static Guid? SifRefIDToGuid( string refid )
        {
            if ( refid == null ) {
                throw new ArgumentNullException( "refid", "Argument cannot be null" );
            }
            if ( refid.Length != 32 ) {
                throw new ArgumentException
                    ( "RefId is not in proper format. Expected 32 characters, was " + refid.Length,
                      "refid" );
            }
            return new Guid( refid );
        }


        /// <summary>
        /// Gets the content from the SIFElement for the specified version of SIF. Only
        /// elements that apply to the requested version of SIF will be returned.
        /// </summary>
        /// <param name="element">The element to retrieve content from</param>
        /// <param name="version"></param>
        /// <returns></returns>
        public virtual IList<Element> GetContent( SifElement element, SifVersion version )
        {
            List<Element> returnValue = new List<Element>();
		    ICollection<SimpleField> fields = element.GetFields();
		    foreach (SimpleField val in fields) {
			    IElementDef def = val.ElementDef;
                if (def.IsSupported(version) &&
                    !def.IsAttribute(version) &&
                    def.Field)
                {
                    returnValue.Add(val);
			    }
		    }

           IList<SifElement> children = element.GetChildList();
		    foreach(SifElement val in children ) {
			    IElementDef def = val.ElementDef;
                if (def.IsSupported(version))
			    {
				    returnValue.Add(val);
			    }
		    }


            MergeSort.Sort<Element>(returnValue, ElementSorter<Element>.GetInstance(version));
            //returnValue.Sort( ElementSorter<Element>.GetInstance( version ) );
		    return returnValue;
        }


	    /// <summary>
        /// Gets the fields from the SIFElement for the specified version of SIF. Only
        /// the fields that apply to the requested version of SIF will be returned.
	    /// </summary>
	    /// <param name="element"></param>
	    /// <param name="version"></param>
	    /// <returns></returns>
	    public virtual ICollection<SimpleField> GetFields(SifElement element, SifVersion version) {
		    ICollection<SimpleField> fields = element.GetFields();
	        List<SimpleField> returnValue = new List<SimpleField>();
            // remove any fields that do not belong in this version of SIF
            foreach(SimpleField field in fields )
            {
                IElementDef def = field.ElementDef;
                if (version.CompareTo(def.EarliestVersion) > -1 &&
                        version.CompareTo(def.LatestVersion) < 1)
                {
                    returnValue.Add( field );           
                }
            }
		    returnValue.Sort( ElementSorter<SimpleField>.GetInstance( version ) );
            return returnValue;
	    }

        /// <summary>
        /// Adds a SimpleField parsed from a specific version of SIF to the parent.
        /// </summary>
        /// <param name="contentParent">The element to add content to</param>
        /// <param name="fieldDef">The metadata definition of the field to set</param>
        /// <param name="data">The value to set to the field</param>
        /// <param name="version">The version of SIF that the SIFElement is being constructed
        /// from</param>
        /// <returns></returns>
        public virtual SimpleField SetField(SifElement contentParent,
            IElementDef fieldDef,
            SifSimpleType data,
            SifVersion version)
        {
            return contentParent.SetField(fieldDef, data);
        }

        public virtual SifElement AddChild(SifElement contentParent, SifElement content, SifVersion version)
        {
            return contentParent.AddChild(content);
        }
    }
}
