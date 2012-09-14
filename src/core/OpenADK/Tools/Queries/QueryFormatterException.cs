//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library.Tools.Queries
{
    /// <summary>  Signals an unrecoverable processing error by a QueryFormatter
    /// 
    /// </summary>
    /// <author> Data Solutions
    /// </author>
    /// <version>  ADK 1.0
    /// </version>
    [Serializable]
    public class QueryFormatterException : SifException
    {
        /// <summary>
        /// Constructs an exception with the error message
        /// </summary>
        /// <param name="msg"></param>
        public QueryFormatterException( string msg )
            : base(
                SifErrorCategoryCode.RequestResponse, SifErrorCodes.REQRSP_UNSUPPORTED_QUERY_9, msg,
                null ) {}

        /// <summary>
        /// Constructs an exception with a message and the inner exception
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="innerException"></param>
        public QueryFormatterException( string msg,
                                        Exception innerException )
            : base(
                SifErrorCategoryCode.RequestResponse, SifErrorCodes.REQRSP_UNSUPPORTED_QUERY_9, msg,
                null, innerException ) {}

        /// <summary>
        /// The .Net Serialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected QueryFormatterException( SerializationInfo info,
                                           StreamingContext context )
            : base( info, context ) {}
    }
}
