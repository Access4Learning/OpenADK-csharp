//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// A factory interface for create IAdkHttpHandlers
    /// </summary>
    public interface IAdkHttpHandlerFactory
    {
        /// <summary>
        /// Creates an instance of an IAdkHTTPHandler that can
        /// process the current request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IAdkHttpHandler CreateHandler( AdkHttpRequest request );
    }
}
