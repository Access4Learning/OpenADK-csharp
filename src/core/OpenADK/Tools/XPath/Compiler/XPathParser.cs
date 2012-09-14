//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library.Tools.XPath.Compiler
{
    /// <summary>
    /// 
    /// </summary>
    public class XPathParser
    {
        public static AdkXPathStep[] Parse( String expression )
        {
            List<AdkXPathStep> steps = new List<AdkXPathStep>();
            LocationPathFragmentParser fragmentParser = new LocationPathFragmentParser( expression );
            while ( fragmentParser.MoveNext() )
            {
                steps.Add( ParseNextStep( fragmentParser.Current ) );
            }
            return steps.ToArray();
        }

        private static AdkXPathStep ParseNextStep( Fragment stepFragment )
        {
            Fragment predicateFragment = stepFragment.FindBoundedFragment( '[', ']' );
            if ( predicateFragment != null )
            {
                AdkExpression predicates = ParsePredicates( predicateFragment );
                String stepName = stepFragment.BeforeFragment( predicateFragment );

                return new AdkXPathStep( AdkAxisType.Child, new AdkNodeNameTest( stepName ), predicates );
            }
            else
            {
                return ParseStep( stepFragment.ToString() );
            }
        }

        private static AdkExpression ParsePredicates( Fragment predicateFragment )
        {
            // Currently, this implementation only supports predicate fragments
            // That consist of multiple predicate conditions and'ed together
            // e.g. 'PhoneNumber[@Format='na'and @Type='TE']

            // The fragment passed in should be the inner text of the predicate block
            // e.g. @Format='na'and @Type='TE'
            Fragment[] predicateFragments = predicateFragment.Split( " and " );
            if ( predicateFragments.Length == 1 )
            {
                return ParseSimplePredicate( predicateFragments[0] );
            }
            else
            {
                List<AdkExpression> predicates = new List<AdkExpression>();
                foreach ( Fragment singlePredicate in predicateFragments )
                {
                    predicates.Add( ParseSimplePredicate( singlePredicate ) );
                }
                return new AdkAndOperation( predicates.ToArray() );
            }
        }

        private static AdkExpression ParseSimplePredicate( Fragment predicateFragment )
        {
            Fragment[] predicateParts = predicateFragment.Split( "=" );

            if ( predicateParts.Length != 2 )
            {
                // This is a fragment type we don't support. Just return it as
                // a Custom function.
                return ParseExtensionFunction( predicateFragment );
            }

            String nodeName = predicateParts[0].ToString();

            String comparedValue = predicateParts[1].ToString();
            AdkStaticValue c = ParseConstant( comparedValue );
            AdkXPathStep step = ParseStep( nodeName );

            return new AdkEqualOperation( new AdkLocPath( false, step ), c );
        }

        private static AdkStaticValue ParseConstant( string fragment )
        {
            object predicateCompareValue;
            if ( fragment[0] == '\'' || fragment[0] == '"' )
            {
                predicateCompareValue = fragment.Substring( 1, fragment.Length - 2 );
            }
            else
            {
                // Try parsing it as a number first
                Decimal tryValue;
                if ( Decimal.TryParse( fragment, out tryValue ) )
                {
                    predicateCompareValue = tryValue;
                }
                else
                {
                    predicateCompareValue = fragment;
                }
            }
            return new AdkStaticValue( predicateCompareValue );
        }

        private static AdkXPathStep ParseStep( string nodeName )
        {
            AdkXPathStep step;
            if ( nodeName.StartsWith( "@" ) )
            {
                step = new AdkXPathStep( AdkAxisType.Attribute, new AdkNodeNameTest( nodeName.Substring( 1 ) ) );
            }
            else
            {
                step = new AdkXPathStep( AdkAxisType.Child, new AdkNodeNameTest( nodeName ) );
            }
            return step;
        }

        private static AdkExpression ParseExtensionFunction( Fragment predicateFragment )
        {
            String value = predicateFragment.ToString();

            int args = value.IndexOf( '(' );
            if ( args > -1 )
            {
                value = value.Substring( 0, args );
                return new AdkFunction( value );
            }
            else
            {
                return ParseConstant( value );
            }
        }


        private class LocationPathFragmentParser : IEnumerator<Fragment>
        {
            private FragmentIterator fParser;

            public LocationPathFragmentParser( String expression )
            {
                fParser = new FragmentIterator( expression );
            }

            ///<summary>
            ///Gets the element in the collection at the current position of the enumerator.
            ///</summary>
            ///
            ///<returns>
            ///The element in the collection at the current position of the enumerator.
            ///</returns>
            ///
            public Fragment Current
            {
                get { return fParser.CurrentFragment; }
            }

            ///<summary>
            ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            ///</summary>
            ///<filterpriority>2</filterpriority>
            public void Dispose()
            {
            }

            ///<summary>
            ///Advances the enumerator to the next element of the collection.
            ///</summary>
            ///
            ///<returns>
            ///true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            ///</returns>
            ///
            ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
            public bool MoveNext()
            {
                return fParser.ParseNext( '/' );
            }

            ///<summary>
            ///Sets the enumerator to its initial position, which is before the first element in the collection.
            ///</summary>
            ///
            ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
            public void Reset()
            {
                fParser.Reset();
            }

            ///<summary>
            ///Gets the current element in the collection.
            ///</summary>
            ///
            ///<returns>
            ///The current element in the collection.
            ///</returns>
            ///
            ///<exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element. </exception><filterpriority>2</filterpriority>
            object IEnumerator.Current
            {
                get { return fParser.CurrentFragment; }
            }
        }


        private class FragmentIterator : Fragment
        {
            /// <summary>
            /// The ordinal position of the start of the current fragment within the StringBuilder
            /// </summary>
            private int fTokenStart;

            /// <summary>
            /// The ordinal position of the end of the current fragment within the StringBuilder
            /// </summary>
            private int fTokenEnd;

            public FragmentIterator( string fragment ) : base( fragment )
            {
                Reset();
            }

            public void Reset()
            {
                // A negative number signals that the current fragment is not parsed
                if ( fBuffer[fStartChar] == '/' )
                {
                    fStartChar++;
                }
                fTokenEnd = fStartChar - 2;
            }

            /// <summary>
            /// Returns a substring from the current token
            /// </summary>
            /// <param name="start"></param>
            /// <param name="length"></param>
            /// <returns></returns>
            public String Substring( int start, int length )
            {
                return fBuffer.ToString( fTokenStart + start, length );
            }

            /// <summary>
            /// Returns the current token as a string
            /// </summary>
            public String CurrentToken
            {
                get
                {
                    if ( fTokenEnd < fTokenStart )
                    {
                        return "";
                    }
                    return fBuffer.ToString( fTokenStart, fTokenEnd - fTokenStart );
                }
            }

            public Fragment CurrentFragment
            {
                get
                {
                    if ( fTokenEnd < fTokenStart )
                    {
                        throw new InvalidOperationException( "Must Call ParseNext before calling CurrentFragment" );
                    }
                    return new SimpleFragment( fBuffer, fTokenStart, fTokenEnd );
                }
            }

            public Fragment CloneCurrentToken()
            {
                return new SimpleFragment( fBuffer, fTokenStart, fTokenEnd );
            }

            /// <summary>
            /// Parses the current fragment, searching for the specified character. If found,
            /// the current token will be set to the fragment between the current start location and
            /// the specified token
            /// </summary>
            /// <param name="searchChar"></param>
            /// <returns></returns>
            public bool ParseNext( char searchChar )
            {
                int searchStart = fTokenEnd + 2;
                if ( searchStart > fEndChar )
                {
                    return false;
                }
                fTokenStart = searchStart;
                int charLocation = IndexOfNextToken( searchChar, searchStart, fEndChar );
                if ( charLocation == -1 )
                {
                    // We've reached the end of the string without finding the 
                    // search char. Therefore, the entire remaining string is the match
                    fTokenEnd = fEndChar;
                }
                else
                {
                    fTokenEnd = charLocation - 1;
                }
                return true;
            }

            /// <summary>
            /// Finds a fragment within the current toekn, bounded by the specified chars
            /// </summary>
            /// <param name="startChar"></param>
            /// <param name="endChar"></param>
            /// <returns></returns>
            public override Fragment FindBoundedFragment( char startChar, char endChar )
            {
                return FindBoundedFragment( startChar, endChar, fTokenStart, fTokenEnd );
            }

            /// <summary>
            /// Splits the fragment into seperate fragments using the given delimiter
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public override Fragment[] Split( string s )
            {
                return Split( s, fTokenStart, fTokenEnd );
            }
        }

        public class SimpleFragment : Fragment
        {
            public SimpleFragment( String fragment ) : base( fragment )
            {
            }

            protected internal SimpleFragment( StringBuilder expression, int start, int end )
                : base( expression, start, end )
            {
            }

            /// <summary>
            /// Finds a fragment within the current fragment, bounded by the specified chars
            /// </summary>
            /// <param name="startChar"></param>
            /// <param name="endChar"></param>
            /// <returns></returns>
            public override Fragment FindBoundedFragment( char startChar, char endChar )
            {
                return FindBoundedFragment( startChar, endChar, fStartChar, fEndChar );
            }
        }

        public abstract class Fragment
        {
            protected StringBuilder fBuffer;

            ///
            /// The ordinal position of the end of this fragment within the StringBuilder
            /// </summary>
            protected int fEndChar = 0;

            /// <summary>
            /// The ordinal position of the start of this fragment within the StringBuilder
            /// </summary>
            protected int fStartChar = 0;

            protected Fragment( String fragment ) : this( new StringBuilder( fragment ), 0, fragment.Length - 1 )
            {
            }


            protected Fragment( StringBuilder expression, int start, int end )
            {
                fBuffer = expression;
                fEndChar = end;
                fStartChar = start;
            }


            /// <summary>
            /// Finds a fragment within the current fragment, bounded by the specified chars
            /// </summary>
            /// <param name="startChar"></param>
            /// <param name="endChar"></param>
            /// <returns></returns>
            public abstract Fragment FindBoundedFragment(
                char startChar,
                char endChar );


            /// <summary>
            /// Splits the fragment into seperate fragments using the given delimiter
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public virtual Fragment[] Split( string s )
            {
                return Split( s, fStartChar, fEndChar );
            }


            protected Fragment[] Split( string s, int start, int end )
            {
                List<Fragment> returnValue = new List<Fragment>();
                int startOfString = IndexOfNextToken( s, start, end );
                while ( true )
                {
                    if ( startOfString == -1 )
                    {
                        returnValue.Add( new SimpleFragment( fBuffer, start, end ) );
                        break;
                    }
                    else
                    {
                        returnValue.Add( new SimpleFragment( fBuffer, start, startOfString - 1 ) );
                        start = startOfString + s.Length;
                        startOfString = IndexOfNextToken( s, start, end );
                    }
                }
                return returnValue.ToArray();
            }


            /// <summary>
            /// Returns the next token, but does not look inside of strings
            /// </summary>
            /// <param name="str"></param>
            /// <param name="startLocation"></param>
            /// <param name="endLocation"></param>
            /// <returns></returns>
            protected int IndexOfNextToken( String str, int startLocation, int endLocation )
            {
                bool inString = false;
                int length = str.Length;
                int found = -1;
                endLocation++;
                endLocation -= length;
                for ( int a = startLocation; a < endLocation; a++ )
                {
                    char current = fBuffer[a];
                    if ( current == '\'' || current == '"' )
                    {
                        inString = !inString;
                    }
                    if ( !inString && current == str[0] )
                    {
                        bool isMatch = true;
                        for ( int x = 0; isMatch && x < length; x++ )
                        {
                            isMatch &= fBuffer[a + x] == str[x];
                        }
                        if ( isMatch )
                        {
                            found = a;
                            break;
                        }
                    }
                }
                return found;
            }


            protected Fragment FindBoundedFragment( char startChar, char endChar, int startLocation, int endLocation )
            {
                int start = IndexOfNextToken( startChar, startLocation, endLocation );
                if ( start > -1 )
                {
                    int end = IndexOfNextToken( endChar, start + 1, endLocation );
                    if ( end > -1 )
                    {
                        return new SimpleFragment( fBuffer, start + 1, end - 1 );
                    }
                }
                return null;
            }


            /// <summary>
            /// Returns the next token, but does not look inside of strings
            /// </summary>
            /// <param name="c"></param>
            /// <param name="startLocation"></param>
            /// <param name="endLocation"></param>
            /// <returns></returns>
            protected int IndexOfNextToken( Char c, int startLocation, int endLocation )
            {
                bool inString = false;
                int found = -1;
                endLocation++;
                for ( int a = startLocation; a < endLocation; a++ )
                {
                    char current = fBuffer[a];
                    if ( current == '\'' || current == '"' )
                    {
                        inString = !inString;
                    }
                    if ( !inString && current == c )
                    {
                        found = a;
                        break;
                    }
                }
                return found;
            }


            private delegate Boolean CompareTokenDelegate( int location );


            public override string ToString()
            {
                return fBuffer.ToString( fStartChar, fEndChar + 1 - fStartChar );
            }

            /// <summary>
            /// Returns the section of 
            /// </summary>
            /// <param name="predicateFragment"></param>
            /// <returns></returns>
            internal string BeforeFragment( Fragment predicateFragment )
            {
                return fBuffer.ToString( fStartChar, predicateFragment.fStartChar - fStartChar - 1 );
            }
        }
    }
}
