//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    /// <summary>  Exception signaling that an error has occurred processing a message.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class AdkMessagingException : AdkException
    {
        /// <summary>
        /// Constructs an exception with a detailed error message and the zone the exception is associated with
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="zone"></param>
        public AdkMessagingException( String msg,
                                      IZone zone )
            : base( msg, zone ) {}

        /// <summary>
        /// Constructs an exception with a detailed error message, zone, and the internal exception that was raised
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="zone"></param>
        /// <param name="innerException"></param>
        public AdkMessagingException( string msg,
                                      IZone zone,
                                      Exception innerException )
            : base( msg, zone, innerException ) {}

        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected AdkMessagingException( SerializationInfo info,
                                         StreamingContext context )
            : base( info, context ) {}
    }
}
