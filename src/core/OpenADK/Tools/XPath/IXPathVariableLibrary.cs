//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath
{
    public interface IXPathVariableLibrary
    {
        IXsltContextVariable ResolveVariable( string prefix, string name );
    }
}
