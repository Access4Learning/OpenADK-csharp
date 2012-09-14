//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using OpenADK.Util;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  A ValueSet is an arbitrary mapping table used to map an application's
    /// proprietary codes and constants to SIF codes and constants. For example, an
    /// agent might define a ValueSet to map grade levels, ethnicity codes, english
    /// proficiency codes, and so on.
    /// 
    /// 
    /// ValueSet stores its data in a HashMap. For each entry in the map, the key is
    /// a value defined by the application and the value is a ValueSetEntry object
    /// that encapsulates the associated SIF value and other fields like a display
    /// title for user interfaces.
    /// 
    /// For example, a ValueSet that maps grade levels might be comprised of the
    /// following entries:
    /// 
    /// 
    /// <table>
    /// <tr><td><b>Key</b></td><td><b>Value</b></td></tr>
    /// <tr><td><b>PREK</b></td><td><b>PK</b></td></tr>
    /// <tr><td><b>K</b></td><td><b>0K</b></td></tr>
    /// <tr><td><b>1</b></td><td><b>01</b></td></tr>
    /// <tr><td><b>2</b></td><td><b>02</b></td></tr>
    /// <tr><td><b>3</b></td><td><b>03</b></td></tr>
    /// <tr><td><b>4</b></td><td><b>04</b></td></tr>
    /// <tr><td><b>5</b></td><td><b>05</b></td></tr>
    /// <tr><td><b>6</b></td><td><b>06</b></td></tr>
    /// <tr><td><b>7</b></td><td><b>07</b></td></tr>
    /// <tr><td><b>8</b></td><td><b>08</b></td></tr>
    /// <tr><td><b>9</b></td><td><b>09</b></td></tr>
    /// </table>
    /// 
    /// 
    /// To translate an application-defined value to its SIF equivalent, call the
    /// <code>translate</code> method. To translate a SIF-defined value to its
    /// application-defined equivalent, call the <code>translateReverse</code>
    /// method.
    /// 
    /// 
    /// </summary>
    /// <author> Data Solutions
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class ValueSet : IComparer
    {
        /// <summary>  The values of this ValueSet</summary>
        protected internal IDictionary fTable = new HybridDictionary();

        /// <summary>  Reverse lookup table</summary>
        protected internal IDictionary fReverseTable = new HybridDictionary();

        /// <summary>  The unique ID of this ValueSet</summary>
        protected internal string fId;

        /// <summary>  Optional display title for this ValueSet</summary>
        protected internal string fTitle;

        /// <summary>  Optional DOM XmlElement that defines this ValueSet</summary>
        [NonSerialized()] protected internal XmlElement fNode;

        /**
 * The default ValueSetEntry to use if no match is found  
 */
        protected ValueSetEntry fDefaultAppEntry;

        /**
         * True to render the app default if the value being translated is null
         */
        protected bool fRenderAppDefaultIfNull;

        /**
         * The default ValueSetEntry to use if no match is found  
         */
        protected ValueSetEntry fDefaultSifEntry;

        /**
         * True to render SIF default if the value being translated is null 
         */
        protected bool fRenderSifDefaultIfNull;


        /// <summary>  Constructs a ValueSet with an ID</summary>
        public ValueSet( string id )
            : this( id, null )
        {
        }

        /// <summary>  Constructs a ValueSet with an ID and display title</summary>
        public ValueSet( string id,
                         string title )
            : this( id, title, null )
        {
        }

        /// <summary>  Constructs a ValueSet with an ID, display title, and associated DOM XmlElement</summary>
        public ValueSet( string id,
                         string title,
                         XmlElement node )
        {
            fId = id;
            fTitle = title;
            fNode = node;
        }

        public virtual string Id
        {
            get { return fId; }

            set { fId = value; }
        }

        public virtual string Title
        {
            get { return fTitle == null ? fId : fTitle; }

            set { fTitle = value; }
        }

        public virtual XmlElement XmlElement
        {
            get { return fNode; }

            set { fNode = value; }
        }

        /// <summary>  Return a sorted array of the ValueSet's entries</summary>
        /// <returns> An array of ValueSetEntry objects sorted by display order
        /// </returns>
        public virtual ValueSetEntry[] Entries
        {
            get
            {
                ValueSetEntry[] entries = new ValueSetEntry[fTable.Count];
                fTable.Values.CopyTo( entries, 0 );
                Array.Sort( entries, this );
                return entries;
            }
        }

        public virtual IDictionary Map
        {
            get { return fTable; }
        }

        public virtual IDictionary ReverseMap
        {
            get { return fReverseTable; }
        }


        public virtual ValueSet Copy( Mappings newParent )
        {
            ValueSet copy = new ValueSet( fId, fTitle );
            if ( fNode != null && newParent.fNode != null )
            {
                XmlElement newNode = (XmlElement) newParent.fNode.OwnerDocument.ImportNode( fNode, true );
                newParent.fNode.AppendChild( newNode );
                copy.fNode = newNode;
            }

            //	Copy the ValueSetEntry's
            ValueSetEntry[] entries = Entries;
            for ( int i = 0; i < entries.Length; i++ )
            {
                copy.Define( entries[i].Name, entries[i].Value, entries[i].Title );
            }

            if (fDefaultAppEntry != null)
            {
                copy.SetAppDefault(fDefaultAppEntry.Name, fRenderAppDefaultIfNull);
            }
            if (fDefaultSifEntry != null)
            {
                copy.SetSifDefault(fDefaultSifEntry.Value, fRenderSifDefaultIfNull);
            }

            return copy;
        }

        public override string ToString()
        {
            return Title;
        }

        /// <summary>  Compares two ValueSetEntry objects for order</summary>
        public virtual int Compare( Object o1,
                                    Object o2 )
        {
            int i1 = ((ValueSetEntry) o1).DisplayOrder;
            int i2 = ((ValueSetEntry) o2).DisplayOrder;
            if ( i1 < i2 )
            {
                return - 1;
            }
            if ( i1 == i2 )
            {
                return 0;
            }

            return 1;
        }

        /// <summary>  Sets a value</summary>
        public virtual void Define( string appValue,
                                    string sifValue,
                                    string title )
        {
            XmlElement element = null;
            if (fNode != null)
            {
                element = fNode.OwnerDocument.CreateElement("value");
                fNode.AppendChild(element);
            }
            Define( appValue, sifValue, title, element );
        }

        /// <summary>  Sets a value</summary>
        public virtual void Define( string appValue,
                                    string sifValue,
                                    string title,
                                    XmlElement node )
        {
            if ( appValue != null )
            {
                ValueSetEntry entry = new ValueSetEntry( appValue, sifValue, title );
                entry.DisplayOrder = fTable.Count;
                entry.Node = node;

                if ( node != null )
                {
                    entry.ToXml( node );
                }


                fTable[appValue] = entry;
                fReverseTable[sifValue] = entry;
            }
        }

        /// <summary>  Gets a value</summary>
        public virtual string Lookup( string appValue )
        {
            ValueSetEntry e = appValue == null ? null : (ValueSetEntry) fTable[appValue];

            return e == null ? null : e.Value;
        }

        /// <summary>  Translates a value.
        /// 
        /// This method differs from <code>get</code> in that it returns <i>appValue</i>
        /// if no mapping is defined in the ValueSet. The <code>get</code> method
        /// returns null if no mapping is defined.
        /// 
        /// 
        /// </summary>
        /// <param name="appValue">An application-defined value
        /// </param>
        /// <returns> The corresponding SIF-defined value
        /// </returns>
        public virtual string Translate( string appValue )
        {
            ValueSetEntry e = GetEntry( appValue );
            if ( e != null )
            {
                return e.Value;
            }
            else
            {
                return EvaluateDefault(
                    fDefaultSifEntry == null ? null : fDefaultSifEntry.Value,
                    fRenderSifDefaultIfNull, appValue );
            }
        }

        /// <summary>
        /// Translates a value.
        /// </summary>
        /// <remarks>
        /// If there is no mapping defined, the default value passed in is returned.
        /// If, however, the default value passed in is NULL, the valueset will be searched
        /// for a default value and that value will be returned. If there is no default,
        /// the <c>appValue</c> passed in is returned. 
        /// </remarks>
        /// <param name="appValue">An application defined value</param>
        /// <param name="defaultValue"></param>
        /// <returns>The corresponding SIF-defined value</returns>
        public virtual string Translate( String appValue, String defaultValue )
        {
            ValueSetEntry e = GetEntry( appValue );
            if ( e != null )
            {
                return e.Value;
            }
            if ( defaultValue != null )
            {
                return defaultValue;
            }

            return EvaluateDefault(
                fDefaultSifEntry == null ? null : fDefaultSifEntry.Value,
                fRenderSifDefaultIfNull, appValue );
        }

        /**
 * Encapsulates the logic for returning default values for a valueset
 * @param defaultValue
 * @param renderIfNull
 * @param srcValue
 * @return
 */

        private String EvaluateDefault(
            String defaultValue,
            bool renderIfNull,
            String srcValue )
        {
            if ( srcValue == null && !renderIfNull )
            {
                return null;
            }
            if ( defaultValue != null )
            {
                return defaultValue;
            }
            return srcValue;
        }

        /// <summary>  Performs a reverse translation.
        /// 
        /// </summary>
        /// <param name="sifValue">An SIF-defined value
        /// </param>
        /// <returns> The corresponding application-defined value
        /// </returns>
        public virtual string TranslateReverse( string sifValue )
        {
            ValueSetEntry e = GetReverseEntry( sifValue );
            if ( e != null )
            {
                return e.Name;
            }
            else
            {
                return EvaluateDefault(
                    fDefaultAppEntry == null ? null : fDefaultAppEntry.Name,
                    fRenderAppDefaultIfNull, sifValue );
            }
        }


        public string TranslateReverse( string sifValue, string defaultValue )
        {
            ValueSetEntry e = GetReverseEntry( sifValue );
            if ( e != null )
            {
                return e.Name;
            }
            if ( defaultValue != null )
            {
                return defaultValue;
            }
            return EvaluateDefault(
                fDefaultAppEntry == null ? null : fDefaultAppEntry.Name,
                fRenderAppDefaultIfNull, sifValue );
        }

        /**
         * Gets a ValueSet entry for the specified application value 
         * @param appValue
         * @return The ValueSetEntry that matches, if found, or null
         */

        private ValueSetEntry GetEntry( String appValue )
        {
            ValueSetEntry e = appValue != null ? (ValueSetEntry) fTable[appValue] : null;
            return e;
        }

        public ValueSetEntry[] GetEntries()
        {
            ValueSetEntry[] entries = new ValueSetEntry[fTable.Count];
            fTable.Values.CopyTo( entries, 0 );
            return entries;
        }

        /**
         * Looks up a ValueSetEntry by a SIF value 
         * @param sifValue
         * @return The ValueSetEntry that matches, if found, or null
         */

        private ValueSetEntry GetReverseEntry( String sifValue )
        {
            ValueSetEntry e = sifValue != null ? (ValueSetEntry) fReverseTable[sifValue] : null;
            return e;
        }


        /**
	 * Sets the default application value that will be returned if no match
	 * is found during a valueset translation
	 * @param appValue The value to return if there is no match. Pass in <code>Null</code> if the 
	 * previously-set default is to be removed
	 * @param renderIfNull True if the default value should be returned even if the SIF Value being
	 * translated is NULL. If false, NULL will be returned.
	 * @throws ADKMappingException Thrown if the value has not yet been defined in this valueset 
 	 * by calling <code>define</code> 
	 */

        public void SetAppDefault( String appValue, bool renderIfNull )

        {
            fRenderAppDefaultIfNull = renderIfNull;
            if ( fDefaultAppEntry != null )
            {
                ValueSetEntry oldEntry = fDefaultAppEntry;
                fDefaultAppEntry = null;
                ToXml( oldEntry, oldEntry.Node );
            }

            if ( appValue != null )
            {
                ValueSetEntry entry = GetEntry( appValue );
                if ( entry == null )
                {
                    throw new AdkMappingException( "Value: '" + appValue + "' is not defined.", null );
                }
                fDefaultAppEntry = entry;
                ToXml( fDefaultAppEntry, fDefaultAppEntry.Node );
            }
        }

        /**
	 * Sets the default SIF value that will be returned if no match
	 * is found during a valueset translation
	 * @param sifValue The value to return if there is no match. Pass in <code>Null</code> if the 
	 * previously-set default is to be removed
	 * @param renderIfNull True if the default value should be returned even if the app value
	 * being translated is null. If false, NULL will be returned.
	 * @throws ADKMappingException Thrown if the value has not yet been defined in this valueset 
	 * by calling <code>define</code> 
	 * 
	 * @see #define(String, String, String)
	 * @see #define(String, String, String, Node)
	 */

        public void SetSifDefault( String sifValue, bool renderIfNull )

        {
            fRenderSifDefaultIfNull = renderIfNull;
            if ( fDefaultSifEntry != null )
            {
                ValueSetEntry oldEntry = fDefaultSifEntry;
                fDefaultSifEntry = null;
                ToXml( oldEntry, oldEntry.Node );
            }
            if ( sifValue != null )
            {
                ValueSetEntry entry = GetReverseEntry( sifValue );
                if ( entry == null )
                {
                    throw new AdkMappingException( "Value: '" + sifValue + "' is not defined.", null );
                }
                fDefaultSifEntry = entry;
                ToXml( fDefaultSifEntry, fDefaultSifEntry.Node );
            }
        }


        /**
	 *  Clears the table
	 */

        public void Clear()
        {
            fTable.Clear();
            fReverseTable.Clear();
            fDefaultAppEntry = null;
            fDefaultSifEntry = null;
            fRenderAppDefaultIfNull = false;
            fRenderSifDefaultIfNull = false;
        }

        /**
	 *  Removes a value
	 */

        public void Remove( String appValue )
        {
            if ( appValue != null )
            {
                ValueSetEntry entry = (ValueSetEntry) fTable[appValue];
                if ( entry != null )
                {
                    fTable.Remove( appValue );
                    fReverseTable.Remove( entry.Value );
                    fNode.RemoveChild( entry.Node );

                    if ( fDefaultAppEntry == entry )
                    {
                        fDefaultAppEntry = null;
                    }
                    if ( fDefaultSifEntry == entry )
                    {
                        fDefaultSifEntry = null;
                    }
                }
            }
        }

        public IDictionary Dictionary
        {
            get { return fTable; }
        }

        public IDictionary ReverseDictionary
        {
            get { return fReverseTable; }
        }

        /**
	 * Writes this valueset to an XML element.
	 * @param element
	 */

        public void ToXml( XmlElement element )
        {
            if ( element == null )
            {
                return;
            }
            element.SetAttribute( "id", fId );
            XmlUtils.SetOrRemoveAttribute( element, "title", fTitle );

            //  Add <value> elements to the <valueset>...
            ValueSetEntry[] entries = GetEntries();
            for ( int i = 0; i < entries.Length; i++ )
            {
                XmlElement vsElement = element.OwnerDocument.CreateElement( "value" );
                element.AppendChild( vsElement );
                entries[i].Node = vsElement;
                ToXml( entries[i], vsElement );
            }
        }

        private void ToXml( ValueSetEntry entry, XmlElement element )
        {
            // If the element passed in is null, this ValueSet doesn't currently have an 
            // XML Element that it is associated with. This is OK. Exit Gracefully.
            if ( element == null )
            {
                return;
            }

            entry.ToXml( element );

            // Since this class controls the notion of defaults, write the 
            // attributes controlling defaults here
            bool isDefaultAppValue = fDefaultAppEntry == entry;
            bool isDefaultSifValue = fDefaultSifEntry == entry;
            if ( isDefaultAppValue )
            {
                if ( isDefaultSifValue )
                {
                    element.SetAttribute( "default", "both" );
                    element.SetAttribute( "ifnull", GetIfNull( fRenderSifDefaultIfNull ) );
                }
                else
                {
                    element.SetAttribute( "default", "inbound" );
                    element.SetAttribute( "ifnull", GetIfNull( fRenderAppDefaultIfNull ) );
                }
            }
            else if ( isDefaultSifValue )
            {
                element.SetAttribute( "default", "outbound" );
                element.SetAttribute( "ifnull", GetIfNull( fRenderSifDefaultIfNull ) );
            }
            else
            {
                element.RemoveAttribute( "default" );
                element.RemoveAttribute( "ifnull" );
            }
        }

        private String GetIfNull( bool value )
        {
            return value ? "default" : "suppress";
        }
    }
}
