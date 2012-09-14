//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;

namespace OpenADK.Library.Tools.XPath
{
    /// <summary>
    /// Contains built-in functions that are automatically registered with
    /// SifXPathContext using the namespace prefix "adk"
    /// </summary>
    public class AdkFunctions
    {
        /// <summary>
        /// Returns the string converted to all Upper case
        /// </summary>
        /// <param name="str"> The string to convert to upper case, or null</param>
        /// <returns></returns>
        public static String toUpperCase( String str )
        {
            if ( str == null )
            {
                return null;
            }
            return str.ToUpper();
        }

        /// <summary>
        /// Returns the string converted to all Lower case
        /// </summary>
        /// <param name="str">The string to convert to lower case, or null</param>
        /// <returns></returns>
        public static String toLowerCase( String str )
        {
            if ( str == null )
            {
                return null;
            }
            return str.ToLower();
        }


        /// <summary>
        /// Returns true if the two strings are equal, ignoring any differences in
        /// case
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns>true if both strings are equal, differing only by the case of the
        /// letters</returns>
        public static bool equalsIgnoreCase( String str1, String str2 )
        {
            if ( str1 == null )
            {
                return str2 == null;
            }
            return String.Compare( str1, str2, true ) == 0;
        }


        /// <summary>
        ///  Pads the beginning of the Source string with the specified PadChar 
        /// character so that the source string is at least Width characters in length. 
        /// </summary>
        /// <remarks>
        /// If the Source string is already equal to or greater than Width, 
        /// no action is taken.
        /// </remarks>
        /// <param name="source">The string to start with</param>
        /// <param name="padding">The string to use for padding (only the first char will be used)</param>
        /// <param name="width">The length the final string should be</param>
        /// <returns>The padded string or 'Null' if the source is null</returns>
        public static String padBegin( String source, String padding, int width )
        {
            return pad( source, padding, width, true );
        }


        /// <summary>
        /// Pads the end of the Source string with the specified PadChar
        /// character so that the source string is at least Width characters in length. 
        /// </summary>
        /// <remarks>
        ///  If the Source string is already equal to or greater than Width, 
        ///  no action is taken.
        /// </remarks>
        /// <param name="source">The string to start with</param>
        /// <param name="padding">The string to use for padding (only the first char will be used)</param>
        /// <param name="width">The length the final string should be</param>
        /// <returns>The padded string or 'Null' if the source is null</returns>
        public static String padEnd( String source, String padding, int width )
        {
            return pad( source, padding, width, false );
        }


        private static String pad( String source, String pad, int width, bool padBeginning )
        {
            if ( source == null )
            {
                return null;
            }
            if ( source.Length >= width || pad == null )
            {
                return source;
            }

            StringBuilder str = new StringBuilder( width );
            char padChar = pad[0];
            int padLength = width - source.Length;
            if ( padBeginning )
            {
                // put the padding on the beginning
                for ( int i = 0; i < padLength; i++ )
                {
                    str.Append( padChar );
                }
                str.Append( source );
            }
            else
            {
                // put the padding on the end
                str.Append( source );
                for ( int i = 0; i < padLength; i++ )
                {
                    str.Append( padChar );
                }
            }

            return str.ToString();
        }


        /// <summary>
        ///  Converts the source string to 'Proper' case. In general, this means
        ///  capitalizing the first letter of each word. However, in some cases, such
        /// as O'Reilly, letters within the word will be capitalized as well.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static String toProperCase( String source )
        {
            if ( source == null )
            {
                return null;
            }
            try
            {
                StringBuilder b = new StringBuilder( source.Trim() );
                int indexIntoWord = 1;
                for ( int i = 0; i < b.Length; i++ )
                {
                    char c = b[i];
                    switch ( c )
                    {
                        case ' ':
                        case '\t':
                        case '\r':
                        case '\n':
                            indexIntoWord = 0;
                            break;
                        case '\'':
                            if ( indexIntoWord < 3 )
                            {
                                indexIntoWord = 0;
                            }
                            break;
                        default:
                            if ( indexIntoWord == 1 )
                            {
                                b[i] = Char.ToUpper( c );
                            }
                            else
                            {
                                b[i] = Char.ToLower( c );
                            }
                            break;
                    }
                    indexIntoWord++;
                }

                return b.ToString();
            }
            catch ( Exception )
            {
                return source;
            }
        }


        /// <summary>
        /// Used internally by the ADK
        /// </summary>
        /// <returns></returns>
        public static bool x()
        {
            return true;
        }
    }
}
