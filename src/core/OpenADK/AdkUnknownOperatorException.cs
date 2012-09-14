//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    /// <summary>
    /// Represents the exception that is thrown when an unkown SIF_Operator is used in a SIF_Query
    /// </summary>
    [Serializable]
    public class AdkUnknownOperatorException : Exception
    {
        /// <summary>
        /// Constructs an error with the error message
        /// </summary>
        /// <param name="op"></param>
        public AdkUnknownOperatorException( string op )
            : base( op ) {}

        /// <summary>
        /// Constructs an error with the error message and the original cause of the exception
        /// </summary>
        /// <param name="op"></param>
        /// <param name="innerException"></param>
        public AdkUnknownOperatorException( string op,
                                            Exception innerException )
            : base( op, innerException ) {}

        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected AdkUnknownOperatorException( SerializationInfo info,
                                               StreamingContext context )
            : base( info, context ) {}
    }
}
