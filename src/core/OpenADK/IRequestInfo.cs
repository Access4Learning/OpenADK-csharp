//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Represents information cached by the ADK about a specific SIF_Request.
    /// </summary>
    public interface IRequestInfo
    {
        /// The Object Type of the Request. e.g. "StudentPersonal"
        /// </summary>
        string ObjectType { get; }

        /// <summary>
        /// The SIF_Request MessageId
        /// </summary>
        string MessageId { get; }

        /// <summary>
        /// The Date and Time that that this request was initially made
        /// </summary>
        DateTime RequestTime { get; }

        /// <summary>
        /// Returns whether or not this Request is Active
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Returns the Serializable UserData state object that was placed in the 
        /// <see cref="OpenADK.Library.Queries"/> query class at the time of the original request.
        /// </summary>
        object UserData { get; }
    }
}
