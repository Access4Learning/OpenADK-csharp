//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath.Functions
{
    /// <summary>
    /// Extension functions provided by a .NET class
    /// </summary>
    public class ClassFunctions : IXPathFunctionLibrary
    {
        private Type fType;
        private object fInstance;

        public ClassFunctions( Type type, object instance )
        {
            fType = type;
            fInstance = instance;
        }

        #region IXPathFunctionLibrary Members

        IXsltContextFunction IXPathFunctionLibrary.ResolveFunction( string prefix, string name,
                                                                    XPathResultType[] argTypes )
        {
            BindingFlags flags = BindingFlags.Static | BindingFlags.Public;
            if ( fInstance != null )
            {
                flags |= BindingFlags.Instance;
            }
            MethodInfo[] methods = fType.GetMethods( flags );
            List<MethodInfo> methodList = new List<MethodInfo>();
            foreach ( MethodInfo info in methods )
            {
                if ( info.Name == name )
                {
                    methodList.Add( info );
                }
            }

            if ( methodList.Count > 0 )
            {
                return new CustomFunction( methodList.ToArray(), fInstance );
            }
            return null;
        }

        #endregion
    }
}
