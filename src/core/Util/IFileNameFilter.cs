//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;

namespace OpenADK.Util
{
    /// <summary>
    /// Summary description for IFileNameFilter.
    /// </summary>
    public interface IFileNameFilter
    {
        /// <summary>
        /// Allows an implementation to determine whether a particular file listing should be returned
        /// </summary>
        /// <param name="info">The current file being examined</param>
        /// <param name="name">The name of the file</param>
        /// <returns><c>True</c> if the file should be returned, otherwise <c>False</c></returns>
        bool Accept( FileInfo info,
                     string name );

        /// <summary>
        /// The initial search pattern to use (e.g. "*.*") ( limits the number of comparisons that need to be done )
        /// </summary>
        string SearchPattern { get; }
    }
}
