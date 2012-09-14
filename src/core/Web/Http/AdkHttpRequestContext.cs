//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpContext.
    /// </summary>
    public class AdkHttpRequestContext
    {
        public AdkHttpRequestContext( AdkHttpConnection connection,
                                      AdkHttpRequest request,
                                      AdkHttpResponse response,
                                      AdkHttpServer server )
        {
            fConnection = connection;
            fRequest = request;
            fResponse = response;
            fServer = server;
        }

        public AdkHttpRequest Request
        {
            get { return fRequest; }
        }

        public AdkHttpResponse Response
        {
            get { return fResponse; }
        }

        public AdkHttpServer Server
        {
            get { return fServer; }
        }

        public AdkHttpConnection Connection
        {
            get { return fConnection; }
        }

        private AdkHttpConnection fConnection;
        private AdkHttpServer fServer;
        private AdkHttpRequest fRequest;
        private AdkHttpResponse fResponse;
    }
}
