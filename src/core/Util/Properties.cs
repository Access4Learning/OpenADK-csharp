//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;

namespace OpenADK.Util
{
    /// <summary>
    /// Returns the configuration information for the agent. The configuration information comes from
    /// the "appSettings" section in the app.config file for the application
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationSettings.AppSettings"/>
    public sealed class Properties
    {
        /// <summary>
        /// Returns the NameValueCollection returned by <see cref="System.Configuration.ConfigurationSettings.AppSettings"/>
        /// </summary>
        /// <returns>A NameValueCollection from the "appSettings" section in the app.config file for the application</returns>
        /// <seealso cref="System.Configuration.ConfigurationSettings.AppSettings"/>
        public static NameValueCollection GetProperties()
        {
            lock ( typeof ( Properties ) ) {
                if ( sDefaultProperties == null ) {
                    sDefaultProperties = new NameValueCollection();
                    CopyDefaultsTo( sDefaultProperties );
                }
            }
            return sDefaultProperties;
        }

        /// <summary>
        /// Returns a boolean value from the specified named property from the "appSettings" section of the app.config
        /// </summary>
        /// <param name="key">the key of the value that should be returned</param>
        /// <returns>true if the specified property is equal to <c>"true"</c></returns>
        public static bool GetBool( string key )
        {
            string val = GetProperty( key );
            return string.Compare( val, key, true, CultureInfo.InvariantCulture ) == 0;
        }

        /// <summary>
        /// Returns the value of the specified named property from the "appSettings" section of the app.config
        /// </summary>
        /// <param name="key">the key of the value that should be returned</param>
        /// <returns>the value from the app.config</returns>
        /// <remarks>If the specified property does not exist, an empty string <c>("")</c> is returned</remarks>
        public static string GetProperty( string key )
        {
            return GetProperties()[key];
        }


        /// <summary>
        /// Sets a system property. This overrides the setting that set in the "appSettings" section of the app.config
        /// </summary>
        /// <param name="key">the key of the property to set</param>
        /// <param name="value">the value to set</param>
        public static void SetProperty( string key,
                                        string value )
        {
            NameValueCollection collection = GetProperties();
            collection[key] = value;
        }

        public static void CopyDefaultsTo( NameValueCollection col )
        {
            NameValueCollection defaults = ConfigurationManager.AppSettings;
            foreach ( string key in defaults ) {
                col[key] = defaults[key];
            }
        }

        private static NameValueCollection sDefaultProperties;
    }
}
