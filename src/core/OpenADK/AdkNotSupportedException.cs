//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    /// <summary>  Signals an operation is not supported.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class AdkNotSupportedException : ApplicationException
    {
        /// <summary>  Gets the Zone associated with this exception</summary>
        public virtual IZone Zone
        {
            get { return fZone; }
        }


        private IZone fZone;

        /// <summary>  Constructs an exception with a detailed message</summary>
        /// <param name="msg">The detailed message
        /// </param>
        public AdkNotSupportedException( String msg )
            : base( msg ) {}

        /// <summary>  Constructs an exception with a detailed message</summary>
        /// <param name="msg">The detailed message
        /// </param>
        /// <param name="zone">The Zone associated with this exception
        /// </param>
        public AdkNotSupportedException( String msg,
                                         IZone zone )
            : base( msg )
        {
            fZone = zone;
        }

        public AdkNotSupportedException(String msg,
                                 IZone zone, Exception ex )
            : base(msg, ex)
        {
            fZone = zone;
        }

        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected AdkNotSupportedException( SerializationInfo info,
                                            StreamingContext context )
            : base( info, context ) {}
    }
}
