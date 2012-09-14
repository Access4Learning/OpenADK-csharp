//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Reflection;
using System.Text;
using OpenADK.Library.Tools.Mapping;

namespace OpenADK.Library
{
    /// <summary>  The default IValueBuilder implementation evaluates an expression to produce
    /// a string value.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The IValueBuilder interface is used by the SifDtd, SifDataObject, and Mappings
    /// classes when evaluating XPath-like query strings. It enables developers to
    /// customize the way the Adk evaluates value expressions in these query strings
    /// to produce a value for a SIF element or attribute. The DefaultIValueBuilder
    /// implementation supports <c>$(variable)</c> token replacement as well as
    /// <c>@com.class.method</c> style calls to static .Net methods.
    /// </para>
    /// <para>
    /// Token Replacement
    /// </para>
    /// <para>
    /// When a <c>$(variable)</c> token is found in an expression, it is
    /// replaced with a value from the Map passed to <c>evaluate</c>. For
    /// example, if the Map constains the entry "color=blue", calling the <c>evaluate</c>
    /// method with the expression "The color is $(color)" would produce the
    /// string "The color is blue".
    /// </para>
    /// <para>
    /// .Net Method Calls
    /// </para>
    /// <para>
    /// When a <c>@method( arg1, arg2, ... )</c> call is found in an expression,
    /// the static .Net method is called and its return value inserted into the value
    /// string. Token replacement is performed before calling the method.
    /// If <c>method</c> is not fully-qualified, it is assumed to be a method
    /// declared by this DefaultIValueBuilder class. The default class can be changed
    /// by calling the <c>setDefaultClass</c> method. When writing your own
    /// static method, the first parameter must be of type IValueBuilder; zero or
    /// more String parameters may follow. The function must return a String:
    /// <c>String method( IValueBuilder vb, String p1, String p2, ... )</c>.
    /// </para>
    /// </remarks>
    /// <example>
    /// In the following example, the toUpperCase static method is called to convert
    /// the $(color) variable to uppercase. This expression would yield the result
    /// "The color is BLUE":
    /// <code>The color is @OpenADK.Library.DefaultValueBuilder.ToUpperCase( $(color) )</code>
    /// </example>
    public class DefaultValueBuilder : IValueBuilder
    {
        /// <summary>  Returns the variables Map</summary>
        /// <returns> The Map passed to the constructor
        /// </returns>
        public virtual IFieldAdaptor Data
        {
            get { return fVars; }
        }

        /// <summary>  Specifies the default class for .Net method calls that do not reference
        /// a fully-qualified class name. <c>OpenADK.Library.DefaultValueBuilder</c>
        /// is used as the default unless this method is called to change it.
        /// </summary>
        /// <value>The name of a class (e.g. "OpenADK.Library.DefaultValueBuilder"). 
        ///     <para>This should be the <seealso cref="System.Type.AssemblyQualifiedName">AssemblyQualifiedName</seealso> of the class.</para> 
        /// </value>
        public static string DefaultClass
        {
            set { sDefClass = value; }
        }

        private static string sDefClass = typeof ( DefaultValueBuilder ).AssemblyQualifiedName;

        /// <summary>
        /// The aliases currently in effect
        /// </summary>
        protected internal static IDictionary sAliases;

        /// <summary>
        /// The dictionary of variables
        /// </summary>
        private IFieldAdaptor fVars;

        private SifFormatter fFormatter;

        /// <summary>  Constructor</summary>
        public DefaultValueBuilder( IFieldAdaptor data ) : this( data, Adk.TextFormatter )
        {}

        /// <summary>
        /// Creates an instance of DefaultValueBuilder that builds values based
        /// on the SIFDataMap, using the specified <c>SIFFormatter</c> instance
        /// </summary>
        /// <param name="data"></param>
        /// <param name="formatter"></param>
        public DefaultValueBuilder(IFieldAdaptor data, SifFormatter formatter)
        {
            fVars = data;
            fFormatter = formatter;
        }

        /// <summary>  Evaluate an expression that the implementation of this interface
        /// understands to return a String value.
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <returns> The value built from the expression</returns>
        public virtual string Evaluate( string expression )
        {
            return EvaluateCode( ReplaceTokens( expression, fVars ) );
        }

        /// <summary>  Calls all .Net methods referenced in the source string to replace the
        /// method reference with the string representation of the method's return
        /// value
        /// </summary>
        public virtual string EvaluateCode( string src )
        {
            if ( src == null ) 
            {
                return null;
            }

            StringBuilder b = new StringBuilder();

            int len = src.Length;
            int at = 0;
            int mark = 0;
            int x = 0;

            do
            {
                at = src.IndexOf( "@", at );
                if ( at == - 1 ) {
                    b.Append( src.Substring( mark ) );
                    at = len;
                }

                if ( at < len ) {
                    ParseResults mm = ParseResults.parse( src, at );
                    if ( mm != null ) {
                        b.Append( src.Substring( mark, (at - mark) ) );

                        mark = mm.Position;

                        string method = mm.MethodName;
                        MyStringTokenizer myStringTokenizer = mm.Parameters;
                        int methodParamCount = 0;

                        Type targetClass = null;

                        try {
                            x = mm.MethodName.LastIndexOf( (Char) '.' );
                            if ( x != - 1 ) {
                                //  Use the fully-qualified Java method
                                String typeName = method.Substring(0, (x - 0));
                                targetClass = Type.GetType( typeName );
                                method = method.Substring( x + 1 );
                            }
                            else {
                                //  Was an alias registered?
                                string aliasClass = (string) sAliases[method];
                                if ( aliasClass != null ) {
                                    targetClass = Type.GetType( aliasClass );
                                }
                            }

                            if ( targetClass == null ) {
                                targetClass = Type.GetType( sDefClass );
                            }
                        }
                        catch ( TypeLoadException cnfe ) {
                            throw new SystemException( "Class not found: " + method, cnfe );
                        }

                        MethodInfo targetMethod = null;
                        MethodInfo [] methods =
                            targetClass.GetMethods( BindingFlags.Static | BindingFlags.Public );
                        for ( int m = 0; m < methods.Length; m++ ) {
                            if ( methods[m].Name.Equals( method ) ) {
                                targetMethod = methods[m];
                                methodParamCount = methods[m].GetParameters().Length;
                                break;
                            }
                        }

                        if ( targetMethod == null ) {
                            throw new SystemException( "Method not found: " + method );
                        }

                        x = 1;
                        Object [] args = new Object[methodParamCount];
                        args[0] = this;

                        //	Assign parameters
                        while ( x < myStringTokenizer.TokenCount + 1 && x < methodParamCount ) {
                            args[x] = myStringTokenizer.GetToken( x - 1 );
                            x++;
                        }

                        //	Fill in any remaining parameters with a blank string value
                        while ( x < methodParamCount ) {
                            args[x++] = "";
                        }

                        at = mark;

                        try {
                            Object result = targetMethod.Invoke( null, (Object []) args );
                            if ( result != null ) {
                                b.Append( result.ToString() );
                            }
                        }
                        catch ( Exception ex ) {
                            throw new SystemException
                                ( "Failed to call method '" + method + "': " + ex.ToString(), ex );
                        }

                        continue;
                    }

                    b.Append( src.Substring( mark, (len - mark) ) );
                    at = len;
                }
            }
            while ( at < len );

            return b.ToString();
        }

        /// <summary>  Replaces all <c>$(variable)</c> tokens in the source string with
        /// the corresponding entry in the supplied Map
        /// </summary>
        /// <param name="src">The source string
        /// </param>
        /// <param name="data">The field adaptor to use
        /// </param>
        public static string ReplaceTokens( string src,
                                            IFieldAdaptor data )
        {
            if ( src == null ) {
                return null;
            }

            StringBuilder b = new StringBuilder();

            int len = src.Length;
            int at = 0;
            int mark = 0;

            do {
                at = src.IndexOf( "$(", at );
                if ( at == - 1 ) {
                    at = len;
                }
                b.Append( src.Substring( mark, (at - mark) ) );

                if ( at < len ) {
                    int i = src.IndexOf( ")", at + 2 );
                    if ( i != - 1 ) {
                        mark = i + 1;
                        string key = src.Substring( at + 2, i - (at + 2) );
                        at = mark;
                        object val = data.GetValue(key);
                        if ( val != null ) {
                            b.Append( val );
                        }
                    }
                    else {
                        b.Append( src.Substring( mark, (len - mark) ) );
                        at = len;
                    }
                }
            }
            while ( at < len );

            return b.ToString();
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// <summary>  "@pad( Source, PadChar, Width )"
        /// 
        /// Pads the Source string with the specified PadChar character so that the
        /// source string is at least Width characters in length. If the Source
        /// string is already equal to or greater than Width, no action is taken.
        /// </summary>
        public static string Pad( IValueBuilder vb,
                                  string source,
                                  string padding,
                                  string width )
        {
            try {
                string _source = source.Trim();
                int _width = Int32.Parse( width.ToString().Trim() );
                if ( _source.Length >= _width ) {
                    return _source;
                }

                string _padding = padding.ToString().Trim();

                StringBuilder b = new StringBuilder();
                for ( int i = _source.Length; i < _width; i++ ) {
                    b.Append( _padding );
                }
                b.Append( _source );

                return b.ToString();
            }
            catch {
                return source.Trim();
            }
        }

        /// <summary>  "@toUpperCase( Source )"
        /// 
        /// Converts the source string to uppercase
        /// </summary>
        public static string ToUpperCase( IValueBuilder vb,
                                          string source )
        {
            try {
                return source.Trim().ToUpper();
            }
            catch {
                return source;
            }
        }

        /// <summary>  "@toLowerCase( Source )"
        /// 
        /// Converts the source string to lowercase
        /// </summary>
        public static string ToLowerCase( IValueBuilder vb,
                                          string source )
        {
            try {
                return source.Trim().ToLower();
            }
            catch {
                return source;
            }
        }

        /// <summary>  "@toMixedCase( Source )"
        /// 
        /// Converts the source string to mixed case
        /// </summary>
        public static string toMixedCase( IValueBuilder vb,
                                          string source )
        {
            try {
                StringBuilder b = new StringBuilder();
                string _source = source.Trim();
                for ( int i = 0; i < _source.Length; i++ ) {
                    if ( i == 0 || _source[i - 1] == ' ' ) {
                        b.Append( Char.ToUpper( _source[i] ) );
                    }
                    else {
                        b.Append( Char.ToLower( _source[i] ) );
                    }
                }

                return b.ToString();
            }
            catch {
                return source;
            }
        }

        /// <summary>  Registers an alias to a static .Net method.</summary>
        /// <param name="alias">The alias name (e.g. "doSomething")
        /// </param>
        /// <param name="method">The fully-qualified .Net method name (e.g. "mycompany.MyValueBuilder.doSomething")
        /// </param>
        public static void AddAlias( string alias,
                                     string method )
        {
            int i = method.LastIndexOf( "." );
            if ( i != - 1 ) {
                sAliases[alias] = method.Substring( 0, i );
            }
            else {
                sAliases[alias] = method;
            }
        }

        /// <summary>A StringTokenizer replacement. We need a replacement for two
        /// reasons: first, the default does not consider empty tokens to be 
        /// tokens. For example, ",,,blue" is considered to have one token, not 4.
        /// Second, we need to ignore commas in literal strings so that a parameter
        /// to a method can itself be comprised of delimiters. If a double-quote is
        /// found, all commas until the next double-quote are considered literal.
        /// The commas are not included in the resulting tokens.
        /// </summary>
        public class MyStringTokenizer
        {
            private string [] fTokens;

            /// <summary>
            /// Creates an instance of MyStringTokenizer
            /// </summary>
            /// <param name="src"></param>
            /// <param name="delimiter"></param>
            public MyStringTokenizer( string src,
                                      char delimiter )
            {
                //	Parse the source string into an array of tokens
                ArrayList v = new ArrayList( 10 );
                if ( src != null ) {
                    int i = 0;
                    bool inQuote = false;
                    StringBuilder token = new StringBuilder();

                    while ( i < src.Length ) {
                        if ( src[i] == '"' ) {
                            inQuote = !inQuote;
                        }
                        else if ( src[i] == delimiter ) {
                            if ( inQuote ) {
                                token.Append( delimiter );
                            }
                            else {
                                v.Add( token.ToString() );
                                token.Length = 0;
                            }
                        }
                        else {
                            token.Append( src[i] );
                        }

                        i++;
                    }

                    v.Add( token.ToString() );
                }

                fTokens = new string[v.Count];
                v.CopyTo( fTokens );
            }

            /// <summary>
            /// Returns the number of tokens
            /// </summary>
            public virtual int TokenCount
            {
                get { return fTokens.Length; }
            }

            /// <summary>
            /// Returns the token at the specified index
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            public virtual string GetToken( int i )
            {
                return fTokens[i];
            }
        }

        /// <summary> 	A helper class to parse "@method( parameterlist )" into .Net method name
        /// and parameter list, and to return the position in the source string where
        /// the caller should continue processing.  
        /// </summary>
        public class ParseResults
        {
            /// <summary>
            /// Position in source string immediately after closing parenthesis 
            /// </summary>
            public int Position;


            /// <summary>
            /// The name of the .Net method
            /// </summary>
            public string MethodName;

            /// <summary>
            /// The parameters to send to the .Net method
            /// </summary>
            public MyStringTokenizer Parameters;

            /// <summary> 	Given a source string and an index into that string where a .Net
            /// method begins (i.e. the location of the @ character), parse the name
            /// of the .Net method and the list of parameters. Return a new ParseResults
            /// instance of both components were found, otherwise return null.
            /// 
            /// For example, if this string were passed: '@random() @strip("(801) 323-1131")',
            /// and the <i>position</i> parameter were 11, this function would return
            /// a new ParseResults with Position set to 32, MethodName set to 'strip',
            /// and Parameters having a single parameter of "(801) 323-1131".
            /// </summary>
            public static ParseResults parse( string src,
                                              int position )
            {
                ParseResults results = new ParseResults();

                int i = position + 1;
                bool inQuote = false;
                StringBuilder buf = new StringBuilder();

                while ( i < src.Length ) {
                    if ( src[i] == '"' ) {
                        inQuote = !inQuote;
                    }
                    else if ( src[i] == '(' ) {
                        if ( !inQuote ) {
                            results.MethodName = src.Substring( position + 1, i - position - 1 );

                            buf.Length = 0;
                        }
                        else {
                            buf.Append( '(' );
                        }
                    }
                    else if ( src[i] == ')' ) {
                        if ( !inQuote ) {
                            results.Parameters = new MyStringTokenizer( buf.ToString(), ',' );
                            results.Position = i + 1;
                            return results;
                        }
                        else {
                            buf.Append( ')' );
                        }
                    }
                    else {
                        buf.Append( src[i] );
                    }

                    i++;
                }

                return null;
            }
        }

        static DefaultValueBuilder()
        {
            sAliases = new Hashtable();
        }
    }
}

// Synchronized with DefaultValueBuilder.java 
