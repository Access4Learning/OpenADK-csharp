//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    /// <summary>Exception signaling that an error has occurred in the Agent Queue.</summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class AdkQueueException : AdkException
    {
        /// <summary>
        /// Constructs an exception with the error message and zone
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="zone"></param>
        public AdkQueueException( string msg,
                                  IZone zone )
            : base( msg, zone ) {}

        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected AdkQueueException( SerializationInfo info,
                                     StreamingContext context )
            : base( info, context ) {}
    }
}
