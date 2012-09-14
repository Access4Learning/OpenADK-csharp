//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using Microsoft.Win32;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpUtilities.
    /// </summary>
    public sealed class AdkHttpUtilities
    {
        /// <summary>
        /// Returns the mime type for the selected file extension
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetMimeType( string extension )
        {
            using ( RegistryKey aFileKey = Registry.ClassesRoot.OpenSubKey( extension ) ) {
                if ( aFileKey != null ) {
                    string aVal = (string) aFileKey.GetValue( "Content Type" );
                    if ( aVal != null ) {
                        return aVal;
                    }
                }
                return "text/html";
            }
        }
    }
}
