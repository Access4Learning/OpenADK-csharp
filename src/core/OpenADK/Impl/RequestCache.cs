//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Infra;
using OpenADK.Util;

namespace OpenADK.Library.Impl
{
    /// <summary>  Stores the message ID and SIF Data Object type of each pending SIF_Request
    /// message. The RequestCache is a global resource of the class framework. It is
    /// only necessary because of an inconsistency in the SIF 1.0r2 Specification in
    /// which a SIF_Response with a SIF_Error must have an empty SIF_ObjectData
    /// element, which prevents the framework from determining the associated object
    /// type. It needs to know the object type to dispatch the SIF_Response to the
    /// appropriate QueryResults message handler when Topics are being used.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public abstract class RequestCache
    {
        private static RequestCache sSingleton = null;

        /// <summary>  Protected constructor; clients must use getInstance</summary>
        protected internal RequestCache() {}


        /// <summary>  Get a RequestCache instance</summary>
        public static RequestCache GetInstance( Agent agent )
        {
            if ( sSingleton == null ) {
                String cls = Properties.GetProperty( "adkglobal.factory.RequestCache" );
                try {
                    if ( cls == null ) {
                        sSingleton = new RequestCacheFile();
                    }
                    else {
                        sSingleton = (RequestCache) ClassFactory.CreateInstance( cls );
                    }

                    sSingleton.Initialize( agent );
                }
                catch ( Exception thr ) {
                    sSingleton = null;
                    throw new AdkException
                        ( "Adk could not create an instance of the class " + cls + ": " + thr, null,
                          thr );
                }
            }

            return sSingleton;
        }

        /// <summary>  Initialize the RequestCache</summary>
        protected internal abstract void Initialize( Agent agent );

        /// <summary>  Closes the RequestCache</summary>
        public virtual void Close()
        {
            sSingleton = null;
        }

        /// <summary>
        /// Returns the number of requests that are currently active
        /// </summary>
        public abstract int ActiveRequestCount { get; }

        /// <summary>  Store the request MsgId and associated SIF Data Object type in the cache</summary>
        public abstract IRequestInfo StoreRequestInfo( SIF_Request request,
                                                       Query query,
                                                       IZone zone );

        /// <summary>  Lookup information on a pending request given its MsgId and remove it from the cache</summary>
        public abstract IRequestInfo GetRequestInfo( String msgId,
                                                     IZone zone );

        /// <summary> Lookup information on a pending request given its MsgId and leave it inf the cache</summary>
        public abstract IRequestInfo LookupRequestInfo( String msgId,
                                                        IZone zone );
    }
}
