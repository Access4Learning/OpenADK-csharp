//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library.Impl
{
    /// <summary>  Thrown to signal an object has not been initialized or started, or that it
    /// has been closed or shut down
    /// </summary>
    [Serializable]
    public class LifecycleException : SystemException
    {
        /// <summary>
        /// Constructs an exception with an error message
        /// </summary>
        /// <param name="msg"></param>
        public LifecycleException( string msg )
            : base( msg ) {}

        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected LifecycleException( SerializationInfo info,
                                      StreamingContext context )
            : base( info, context ) {}
    }
}
