//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library.Tools.Cfg
{
    /// <summary>  Signals an error in an AgentConfig configuration file
    /// 
    /// </summary>
    /// <author>  OpenADK
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class AdkConfigException : AdkException
    {
        /// <summary>Constructs an exception with the error message</summary>
        /// <param name="msg">A detailed message
        /// </param>
        public AdkConfigException( string msg )
            : base( msg, null ) {}

        /// <summary>
        /// Constructs an exception with the error message and the inner exception that was raised
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public AdkConfigException( string msg,
                                   Exception innerException )
            : base( msg, null, innerException ) {}

        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected AdkConfigException( SerializationInfo info,
                                      StreamingContext context )
            : base( info, context ) {}
    }
}
