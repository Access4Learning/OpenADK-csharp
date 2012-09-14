//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Specialized;
using OpenADK.Util;

namespace OpenADK.Library
{
    /// <summary>  The abstract base class for agent and zone properties</summary>
    /// <remarks>
    /// <para>
    /// AdkProperties adds convenience methods to the <c>Configuration.AppSettings</c> class for
    /// getting and setting property values by data type. In addition, it overrides
    /// the getProperty method to inherit the property value from its parent if not
    /// defined locally. In the Adk, the properties of a zone are always inherited
    /// from the default properties of the agent.
    /// </para>
    /// <para>
    /// Note that property inheritance is only provided by the getProperty method;
    /// no other superclass methods support inheritance. Thus, calling enumerating
    /// property elements does not include the properties of the parent object.
    /// Similarly, saving an AdkProperties to disk with the store method only writes
    /// the properties that are defined locally. When the properties are
    /// subsequently read from disk, they will not include the inherited properties
    /// of the parent object, nor will the object's relationship with its parent be
    /// re-established.
    /// </para>
    /// </remarks>
    [Serializable]
    public class AdkProperties : IPropertyCollection
    {
        /// <summary>  Gets the parent object from which properties are inherited</summary>
        public virtual AdkProperties Parent
        {
            get { return fParent; }
        }


        /// <summary>  Protected constructor called by Agent to create root properties
        /// inherited by all zones
        /// </summary>
        protected internal AdkProperties()
            : this( (AdkProperties) null )
        {
            Defaults( null );
        }

        /// <summary>  Protected constructor called by Agent to create root properties
        /// inherited by all zones
        /// </summary>
        protected internal AdkProperties( Agent agent )
            : this( (AdkProperties) null )
        {
            Defaults( agent );
        }

        /// <summary>  Creates a properties object that inherits values from a parent</summary>
        /// <param name="inherit">The parent AdkProperties object
        /// </param>
        public AdkProperties( AdkProperties inherit )
        {
            fParent = inherit;
        }

        /// <summary>  Called by the default constructor to set default property values.
        /// Defaults are usually imported from the app.config file
        /// </summary>
        public virtual void Defaults( Object owner )
        {
            Properties.CopyDefaultsTo( fProperties );
        }


        /// <summary>  Gets a property value as an <c>int</c>. The property is
        /// inherited from this object's parent if not defined locally.
        /// </summary>
        /// <param name="prop">The name of the property
        /// </param>
        /// <param name="defaultValue">The default value to return when the proprety is
        /// undefined or is not an integer
        /// </param>
        /// <returns> The value of the property, or <i>defaultValue</i> if
        /// undefined or not an integer
        /// </returns>
        public virtual int GetProperty( string prop,
                                        int defaultValue )
        {
            string s = GetProperty( prop );
            if ( s != null ) {
                try {
                    return Int32.Parse( s );
                }
                catch {}
            }

            return defaultValue;
        }

        /// <summary>  Gets a property value as a <c>boolean</c>. The property is
        /// inherited from this object's parent if not defined.
        /// </summary>
        /// <param name="prop">The name of the property
        /// </param>
        /// <param name="def">The default value to return when the proprety is
        /// undefined or is not set to "true"
        /// </param>
        /// <returns> The value of the property, or <i>defaultValue</i> if undefined
        /// or is not set to "true"
        /// </returns>
        public virtual bool GetProperty( string prop,
                                         bool def )
        {
            string s = GetProperty( prop );
            return s == null ? def : bool.Parse( s );
        }

        /// <summary>  Sets an <c>int</c> property</summary>
        /// <param name="prop">The name of the property
        /// </param>
        /// <param name="val">The property value
        /// </param>
        public virtual void SetProperty( string prop,
                                         int val )
        {
            fProperties.Remove( prop );
            fProperties[prop] = val.ToString();
        }

        /// <summary>  Sets a <c>boolean</c> property</summary>
        /// <param name="prop">The name of the property
        /// </param>
        /// <param name="val">The property value
        /// </param>
        public virtual void SetProperty( string prop,
                                         bool val )
        {
            fProperties.Remove( prop );
            fProperties[prop] = val.ToString();
        }

        /// <summary>  Sets the value of a string property</summary>
        /// <param name="name">The name of the property
        /// </param>
        /// <param name="val">The property value
        /// </param>
        public void SetProperty( string name,
                                 string val )
        {
            if ( val != null ) {
                fProperties.Remove( name );
                fProperties[name] = val;
            }
        }

        /// <summary>  Overridden to inherit properties from parent</summary>
        /// <param name="name">The property name
        /// </param>
        /// <returns> The value of the property if defined by this object or by its parent
        /// </returns>
        public string GetProperty( string name )
        {
            String s = fProperties[name];
            if ( s == null && fParent != null ) {
                return fParent.GetProperty( name );
            }
            else {
                return s;
            }
        }

        /// <summary>  Gets a property value as a <c>String</c>. The property is
        /// inherited from this object's parent if not defined locally.
        /// </summary>
        /// <param name="name">The name of the property
        /// </param>
        /// <param name="defaultVal">The default value to return when the proprety is undefined
        /// </param>
        /// <returns> The value of the property, or <i>defaultValue</i> if undefined
        /// </returns>
        public string GetProperty( string name,
                                   string defaultVal )
        {
            return GetProperty( name ) ?? defaultVal;
        }

        /// <summary>
        /// The indexer for the property collection
        /// </summary>
        /// <remarks>
        /// This indexer has the same behavior as the <see cref="GetProperty"/> and <see cref="SetProperty"/> methods.
        /// </remarks>
        public string this[ string key ]
        {
            get { return GetProperty( key ); }
            set { SetProperty( key, value ); }
        }


        /// <summary>
        /// Removes the property with the specified key
        /// </summary>
        /// <param name="key">the name of the property to remove</param>
        public void Remove( string key )
        {
            fProperties.Remove( key );
        }

        // Hiding the underlying collection so that access has to be done through members
        private NameValueCollection fProperties = new NameValueCollection();

        /// <summary>
        /// The parent properties object 
        /// </summary>
        protected internal AdkProperties fParent;

        #region IPropertyCollection Members

        ICollection IPropertyCollection.Keys
        {
            get { return fProperties.Keys; }
        }

        /// <summary>
        /// Returns the count of all properties that are currently set in the collection
        /// </summary>
        public int Count
        {
            get { return fProperties.Count; }
        }

        /// <summary>
        /// Determines if the collection contains a property with the specified key. It does not check the
        /// parent properties class, so calling GetProperty with the specified key could still return a value.
        /// </summary>
        /// <param name="key">The key of the property to check for in the collection</param>
        /// <returns>TRUE if the specified property is contained in the collection, otherwise FALSE</returns>
        /// <remarks>To determine if this class or any parent contains a key with the specified value, call GetProperty( key )
        /// and check to see if the returned value is not null. Alternately, you can check each parent in the 
        /// chain by repeatedly calling .Parent and then calling Contains on the parent.</remarks>
        public bool Contains( string key )
        {
            return fProperties[key] != null;
        }

        /// <summary>
        /// Clears the contents of this collection
        /// </summary>
        public void Clear()
        {
            fProperties.Clear();
        }

        #endregion
    }
}
