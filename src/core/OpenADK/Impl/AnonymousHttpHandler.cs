//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Web.Http;

namespace OpenADK.Library.Impl
{
    /// <summary> Title:
    /// Description:
    /// Copyright:    Copyright (c) 2002
    /// Company:
    /// </summary>
    /// <author> 
    /// </author>
    /// <version>  1.0
    /// </version>
    internal class AnonymousHttpHandler : IAdkHttpHandler
    {
        public virtual void ProcessRequest( AdkHttpRequestContext context )
        {
            Console.Out.WriteLine( "Warning: Anonymous message received from ZIS" );
        }
    }
}
