//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath
{
    public interface IXPathFunctionLibrary
    {
        IXsltContextFunction ResolveFunction( String prefix, String name, XPathResultType[] argTypes );
    }
}
