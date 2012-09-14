//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath
{
    internal class SifXsltContext : XsltContext
    {
        private IDictionary<String, IXPathFunctionLibrary> fFunctions;
        private IDictionary<String, IXPathVariableLibrary> fVariables;


        ///<summary>
        ///When overridden in a derived class, resolves a variable reference and returns an <see cref="T:System.Xml.Xsl.IXsltContextVariable"></see> representing the variable.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Xml.Xsl.IXsltContextVariable"></see> representing the variable at runtime.
        ///</returns>
        ///
        ///<param name="name">The name of the variable. </param>
        ///<param name="prefix">The prefix of the variable as it appears in the XPath expression. </param>
        public override IXsltContextVariable ResolveVariable( string prefix, string name )
        {
            if ( fVariables == null )
            {
                return null;
            }
            IXPathVariableLibrary lib;
            if ( fVariables.TryGetValue( prefix, out lib ) )
            {
                return lib.ResolveVariable( prefix, name );
            }
            return null;
        }

        ///<summary>
        ///When overridden in a derived class, resolves a function reference and returns an <see cref="T:System.Xml.Xsl.IXsltContextFunction"></see> representing the function. The <see cref="T:System.Xml.Xsl.IXsltContextFunction"></see> is used at execution time to get the return value of the function.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Xml.Xsl.IXsltContextFunction"></see> representing the function.
        ///</returns>
        ///
        ///<param name="name">The name of the function. </param>
        ///<param name="prefix">The prefix of the function as it appears in the XPath expression. </param>
        ///<param name="ArgTypes">An array of argument types for the function being resolved. This allows you to select between methods with the same name (for example, overloaded methods). </param>
        public override IXsltContextFunction ResolveFunction( string prefix, string name, XPathResultType[] ArgTypes )
        {
            if ( fFunctions == null )
            {
                return null;
            }
            IXPathFunctionLibrary lib;
            if ( fFunctions.TryGetValue( prefix, out lib ) )
            {
                return lib.ResolveFunction( prefix, name, ArgTypes );
            }
            return null;
        }

        ///<summary>
        ///When overridden in a derived class, evaluates whether to preserve white space nodes or strip them for the given context.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the white space is to be preserved or false if the white space is to be stripped.
        ///</returns>
        ///
        ///<param name="node">The white space node that is to be preserved or stripped in the current context. </param>
        public override bool PreserveWhitespace( XPathNavigator node )
        {
            return false;
        }

        ///<summary>
        ///When overridden in a derived class, compares the base Uniform Resource Identifiers (URIs) of two documents based upon the order the documents were loaded by the XSLT processor (that is, the <see cref="T:System.Xml.Xsl.XslTransform"></see> class).
        ///</summary>
        ///
        ///<returns>
        ///An integer value describing the relative order of the two base URIs: -1 if baseUri occurs before nextbaseUri; 0 if the two base URIs are identical; and 1 if baseUri occurs after nextbaseUri.
        ///</returns>
        ///
        ///<param name="baseUri">The base URI of the first document to compare. </param>
        ///<param name="nextbaseUri">The base URI of the second document to compare. </param>
        public override int CompareDocument( string baseUri, string nextbaseUri )
        {
            return 0;
        }

        ///<summary>
        ///When overridden in a derived class, gets a value indicating whether to include white space nodes in the output.
        ///</summary>
        ///
        ///<returns>
        ///true to check white space nodes in the source document for inclusion in the output; false to not evaluate white space nodes. The default is true.
        ///</returns>
        ///
        public override bool Whitespace
        {
            get { return false; }
        }

        [MethodImpl( MethodImplOptions.Synchronized )]
        public void AddVariables( string ns, IXPathVariableLibrary variables )
        {
            if ( fVariables == null )
            {
                fVariables = new Dictionary<String, IXPathVariableLibrary>();
            }
            fVariables.Add( ns, variables );
        }

        [MethodImpl( MethodImplOptions.Synchronized )]
        public void AddFunctions( string ns, IXPathFunctionLibrary functions )
        {
            if ( fFunctions == null )
            {
                fFunctions = new Dictionary<String, IXPathFunctionLibrary>();
            }
            fFunctions.Add( ns, functions );
        }

        public static XPathResultType GetXPathResultType( Type type )
        {
            if ( type.IsInstanceOfType( typeof ( IEnumerable<XmlNode> ) ) )
            {
                return XPathResultType.NodeSet;
            }
            if ( type.IsInstanceOfType( typeof ( XPathNavigator ) ) )
            {
                return XPathResultType.Navigator;
            }
            if ( type.IsInstanceOfType( typeof ( XmlNode ) ) )
            {
                return XPathResultType.Any;
            }
            switch ( Type.GetTypeCode( type ) )
            {
                case TypeCode.Boolean:
                    return XPathResultType.Boolean;
                case TypeCode.Char:
                case TypeCode.String:
                    return XPathResultType.String;
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return XPathResultType.Number;
                default:
                    return XPathResultType.Error;
            }
        }
    }
}
