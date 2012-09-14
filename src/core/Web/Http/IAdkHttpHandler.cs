//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// The interface definition that the ADK HTTP server uses for processing requests.
    /// </summary>
    public interface IAdkHttpHandler
    {
        /// <summary>
        /// Process the current Http Request
        /// </summary>
        /// <param name="context"></param>
        void ProcessRequest( AdkHttpRequestContext context );
    }
}
