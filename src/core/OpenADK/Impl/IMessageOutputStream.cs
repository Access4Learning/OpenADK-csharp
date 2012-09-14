//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;

namespace OpenADK.Library.Impl
{
    /// <summary>
    /// Defines a class that contains message data that has already been encoded for transmission to the zone
    /// </summary>
    public interface IMessageOutputStream : IDisposable
    {
        /// <summary>
        /// Returns the length in bytes of the message
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Copies the entire contents of this message to the specified stream
        /// </summary>
        /// <param name="stream">The stream that the message will be copied to</param>
        void CopyTo( Stream stream );

        /// <summary>
        /// Converts the underlying data buffer to a string 
        /// </summary>
        /// <returns></returns>
        string Decode();
    }
}
