//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;

namespace OpenADK.Library.Impl
{
    /// <summary>
    /// Defines a class that contains received message data before it has been parsed
    /// </summary>
    public interface IMessageInputStream : IDisposable
    {
        /// <summary>
        /// The stream that contains data that has been received but not yet parsed
        /// </summary>
        Stream GetInputStream();
    }
}
