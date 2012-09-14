//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    /// <summary>  Exception signaling that a method was called on a Zone instance but the zone
    /// is not in a connected state.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class AdkZoneNotConnectedException : AdkException
    {
        /// <summary>
        /// Constructs an exception with the error message and the zone the error occured in
        /// </summary>
        /// <param name="msg">The error that occured</param>
        /// <param name="zone">The zone that is not in a connected state</param>
        public AdkZoneNotConnectedException( String msg,
                                             IZone zone )
            : base( msg, zone ) {}

        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected AdkZoneNotConnectedException( SerializationInfo info,
                                                StreamingContext context )
            : base( info, context ) {}
    }
}
