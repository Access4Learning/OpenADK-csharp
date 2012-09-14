//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;
using OpenADK.Library.Impl;

namespace OpenADK.Util
{
    /// <summary>  Various static helper routines for manipulating text strings.
    /// 
    /// </summary>
    public class AdkStringUtils
    {
        /// <summary>  Replaces characters that are illegal in filenames to underscores ("_").</summary>
        /// <param name="path">A string to be used in a file path or file name
        /// </param>
        /// <returns> The input string with illegal characters converted to an underscore.
        /// The following characters are replaced: : % / \ ; $ &gt; &lt; * . ? " | ! @
        /// </returns>
        public static string SafePathString( string path )
        {
            if ( (Object) path == null ) {
                return "null";
            }

            StringBuilder b = new StringBuilder();

            int cnt = path.Length;
            for ( int i = 0; i < cnt; i++ ) {
                char c = path[i];
                if ( c == ':' || c == '%' || c == '/' || c == '\\' || c == ';' || c == '$' ||
                     c == '>' || c == '<' || c == '*' || c == '.' || c == '?' || c == '"' ||
                     c == '|' || c == '!' || c == '@' ) {
                    b.Append( '_' );
                }
                else {
                    b.Append( (char) c );
                }
            }

            return b.ToString();
        }

        /// <summary>  Escapes an XML string by replacing the characters shown below with their
        /// equivalent entity references as defined by the XML specification.
        /// </summary>
        /// <remarks>
        /// <table>
        /// <tr><td>Character</td><td>Entity Reference</td></tr>
        /// <tr><td>&lt;</td><td>&amp;lt;</td></tr>
        /// <tr><td>&gt;</td><td>&amp;gt;</td></tr>
        /// <tr><td>&amp;</td><td>&amp;amp;</td></tr>
        /// <tr><td>&apos;</td><td>&amp;apos;</td></tr>
        /// <tr><td>&quot;</td><td>&amp;quot;</td></tr>
        /// </table>
        /// </remarks>
        /// 
        /// <param name="str">The source string
        /// </param>
        /// <returns> The escaped string
        /// </returns>
        public static string EncodeXml( string str )
        {
            if ( (Object) str == null ) {
                return null;
            }

            StringBuilder b = new StringBuilder();

            int cnt = str.Length;
            for ( int i = 0; i < cnt; i++ ) {
                char c = str[i];

                switch ( c ) {
                    case '<':
                        b.Append( "&lt;" );
                        break;

                    case '>':
                        b.Append( "&gt;" );
                        break;

                    case '\'':
                        b.Append( "&apos;" );
                        break;

                    case '"':
                        b.Append( "&quot;" );
                        break;

                    case '&':
                        b.Append( "&amp;" );
                        break;

                    default:
                        b.Append( (char) c );
                        break;
                }
            }

            return b.ToString();
        }


        /// <summary>  Unescapes an XML string by replacing the entity references shown below
        /// with their equivalent characters as defined by the XML specification.
        /// </summary>
        /// <remarks>
        /// <table>
        /// <tr><td>Character</td><td>Entity Reference</td></tr>
        /// <tr><td>&lt;</td><td>&amp;lt;</td></tr>
        /// <tr><td>&gt;</td><td>&amp;gt;</td></tr>
        /// <tr><td>&amp;</td><td>&amp;amp;</td></tr>
        /// <tr><td>&apos;</td><td>&amp;apos;</td></tr>
        /// <tr><td>&quot;</td><td>&amp;quot;</td></tr>
        /// </table>
        /// </remarks>
        /// <param name="str">The source string
        /// </param>
        /// <returns> The escaped string
        /// </returns>
        public static string UnencodeXml( string str )
        {
            if ( str == null ) {
                return null;
            }

            StringBuilder b = new StringBuilder();

            int c = 0;
            while ( c < str.Length ) {
                if ( str[c] == '&' ) {
                    string entity = _entity( str, c );
                    if ( entity == null ) {
                        b.Append( '&' );
                    }
                    else {
                        if ( entity.Equals( "lt" ) ) {
                            b.Append( '<' );
                        }
                        else if ( entity.Equals( "gt" ) ) {
                            b.Append( '>' );
                        }
                        else if ( entity.Equals( "amp" ) ) {
                            b.Append( '&' );
                        }
                        else if ( entity.Equals( "apos" ) ) {
                            b.Append( "'" );
                        }
                        else if ( entity.Equals( "quot" ) ) {
                            b.Append( '"' );
                        }
                        else {
                            b.Append( "&" );
                            b.Append( entity );
                            b.Append( ";" );
                        }

                        c += entity.Length + 1;
                    }
                }
                else {
                    b.Append( str[c] );
                }

                c++;
            }

            return b.ToString();
        }

        private static string _entity( string src,
                                       int ch )
        {
            int c = ch + 1;

            while ( c < src.Length ) {
                if ( src[c] == '&' ) {
                    return null;
                }
                if ( src[c] == ';' ) {
                    return src.Substring( ch + 1, (c) - (ch + 1) );
                }
                c++;
            }

            return null;
        }

        public static string ReplaceFirst( string src,
                                           string searchString,
                                           string replaceString )
        {
            StringBuilder builder = new StringBuilder( src );
            int index = src.IndexOf( searchString );
            if ( index > -1 ) {
                builder.Replace( searchString, replaceString, index, searchString.Length );
                return builder.ToString();
            }
            else {
                // search string was not found
                return src;
            }
        }
    }
}
