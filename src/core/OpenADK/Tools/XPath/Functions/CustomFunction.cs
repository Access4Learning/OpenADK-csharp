//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath.Functions
{
    /// <summary>
    /// Represents a single or overloaded set of methods on a class
    /// </summary>
    public class CustomFunction : IXsltContextFunction
    {
        private MethodInfo[] fMethodInfos;
        private object fDeclaringObject;

        /// <summary>
        /// Creates a CustomFunction instance with an array of MethodInfos,
        /// all representing overloads of the same method name.
        /// </summary>
        /// <param name="methods">A set of overloaded methods, ordered by the number
        /// of arguments</param>
        /// <param name="declaringObject">The object declaring this method, or
        /// null if the method is static</param>
        public CustomFunction( MethodInfo[] methods, Object declaringObject )
        {
            fMethodInfos = methods;
            fDeclaringObject = declaringObject;
        }


        ///<summary>
        ///Provides the method to invoke the function with the given arguments in the given context.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Object"></see> representing the return value of the function.
        ///</returns>
        ///
        ///<param name="xsltContext">The XSLT context for the function call. </param>
        ///<param name="args">The arguments of the function call. Each argument is an element in the array. </param>
        ///<param name="docContext">The context node for the function call. </param>
        public object Invoke( XsltContext xsltContext, object[] args, XPathNavigator docContext )
        {
            foreach ( MethodInfo mi in fMethodInfos )
            {
                ParameterInfo[] parameters = mi.GetParameters();
                if ( parameters.Length == args.Length )
                {
                    // The arguments come in as a set of node iterators
                    // Convert the arguments to a set of .Net Objects
                    object[] functionArgs = ConvertXPathArgsToObjects( args, parameters );
                    return mi.Invoke( fDeclaringObject, functionArgs );
                }
            }
            // TODO: should this return an exception?
            return null;
        }

        private object[] ConvertXPathArgsToObjects( object[] args, ParameterInfo[] parameters )
        {
            ArrayList returnValue = new ArrayList( args.Length );
            for ( int a = 0; a < parameters.Length; a++ )
            {
                XPathNodeIterator iterator = args[a] as XPathNodeIterator;
                if ( iterator != null )
                {
                    if ( iterator.MoveNext() )
                    {
                        returnValue.Add( ExtractRawValue( iterator.Current.TypedValue, parameters[a] ) );
                    }
                }
                else
                {
                    returnValue.Add( ExtractRawValue( args[a], parameters[a] ) );
                }
            }
            return returnValue.ToArray();
        }

        private object ExtractRawValue( Object arg, ParameterInfo param )
        {
            if ( arg == null )
            {
                return null;
            }

            Type argType = arg.GetType();

            if ( param.ParameterType.IsAssignableFrom( argType ) )
            {
                return arg;
            }
            else if ( param.ParameterType.IsPrimitive || param.ParameterType == typeof ( string ) )
            {
                if ( arg is Element )
                {
                    return ExtractRawValue( ((Element) arg).SifValue );
                }
                else if ( arg is SifSimpleType )
                {
                    return ExtractRawValue( arg as SifSimpleType );
                }
                else
                {
                    return arg.ToString();
                }
            }
            else
            {
                // This will probably fail. .Net will throw an exception, which is what we want
                return arg;
            }
        }

        private object ExtractRawValue( SifSimpleType data )
        {
            if ( data == null )
            {
                return null;
            }
            return data.RawValue;
        }


        ///<summary>
        ///Gets the minimum number of arguments for the function. This enables the user to differentiate between overloaded functions.
        ///</summary>
        ///
        ///<returns>
        ///The minimum number of arguments for the function.
        ///</returns>
        ///
        public int Minargs
        {
            get { return fMethodInfos[0].GetParameters().Length; }
        }

        ///<summary>
        ///Gets the maximum number of arguments for the function. This enables the user to differentiate between overloaded functions.
        ///</summary>
        ///
        ///<returns>
        ///The maximum number of arguments for the function.
        ///</returns>
        ///
        public int Maxargs
        {
            get { return fMethodInfos[fMethodInfos.Length - 1].GetParameters().Length; }
        }

        ///<summary>
        ///Gets the <see cref="T:System.Xml.XPath.XPathResultType"></see> representing the XPath type returned by the function.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Xml.XPath.XPathResultType"></see> representing the XPath type returned by the function 
        ///</returns>
        ///
        public XPathResultType ReturnType
        {
            get
            {
                Type returnType = fMethodInfos[0].ReturnType;
                return SifXsltContext.GetXPathResultType( returnType );
            }
        }


        ///<summary>
        ///Gets the supplied XML Path Language (XPath) types for the function's argument list. 
        /// This information can be used to discover the signature of the function which allows you to 
        /// differentiate between overloaded functions.
        ///</summary>
        ///
        ///<returns>
        ///An array of <see cref="T:System.Xml.XPath.XPathResultType"></see> representing the types for the function's argument list.
        ///</returns>
        ///
        public XPathResultType[] ArgTypes
        {
            get
            {
                MethodInfo method = fMethodInfos[fMethodInfos.Length - 1];
                List<XPathResultType> types = new List<XPathResultType>();
                foreach ( ParameterInfo arg in method.GetParameters() )
                {
                    types.Add( SifXsltContext.GetXPathResultType( arg.ParameterType ) );
                }
                return types.ToArray();
            }
        }
    }
}
