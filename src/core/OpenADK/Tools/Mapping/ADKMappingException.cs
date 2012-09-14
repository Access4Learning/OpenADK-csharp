//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  Signals an exception in a field mapping rule definition or mapping operation.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class AdkMappingException : AdkException
    {
        /// <summary>
        /// Creates an exception that signals an exception in a field mapping rule definition or mapping operation.
        /// </summary>
        /// <param name="msg">>A detailed message to associated with this exception</param>
        /// <param name="zone">The zone the error was associated with, or null</param>
        public AdkMappingException(string msg,
                                   IZone zone)
            : base(msg, zone)
        {
        }

        /// <summary>
        /// Constructs an exception with the error message, zone and the exception that caused it
        /// </summary>
        /// <param name="msg">>A detailed message to associated with this exception</param>
        /// <param name="zone">The zone the error was associated with, or null</param>
        /// <param name="innerException">The cause of the exception</param>
        public AdkMappingException(string msg,
                                   IZone zone,
                                   Exception innerException)
            : base(msg, zone, innerException)
        {
        }

        /// <summary>
        /// The .Net Serialization constructor, used to allow exception to be serialized across AppDomain boundaries
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        protected AdkMappingException(SerializationInfo info,
                                      StreamingContext context)
            : base(info, context)
        {
        }
    }
}
