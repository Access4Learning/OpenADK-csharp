//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    /// <summary>Signals an error parsing SIF message content</summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    [Serializable]
    public class AdkParsingException : AdkException
    {
        /// <summary>
        /// Constructs an exception with a detailed message and the zone associated with the error
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="zone"></param>
        public AdkParsingException( String msg,
                                    IZone zone )
            : base( msg, zone ) {}


        /// <summary>
        /// Constructs an exception with a detailed message, the zone and the inner exception that was raised
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="zone"></param>
        /// <param name="innerException"></param>
        public AdkParsingException( String msg,
                                    IZone zone,
                                    Exception innerException )
            : base( msg, zone, innerException ) {}


        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected AdkParsingException( SerializationInfo info,
                                       StreamingContext context )
            : base( info, context ) {}
    }
}
