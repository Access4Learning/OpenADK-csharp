//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.IO;

namespace OpenADK.Util
{
    /// <summary>
    /// A class that somewhat mimics the Java pattern of allowing filtering of files in a directory
    /// </summary>
    public sealed class DirectorySearcher
    {
        /// <summary>
        /// Returns an IEnumerator whose <c>Current</c> property returns a <see cref="System.IO.FileInfo"/>
        /// </summary>
        /// <param name="directory">The directory to search</param>
        /// <param name="filter">The file filter to use to filter filenames</param>
        /// <returns>an IEnumerator</returns>
        public static IEnumerator GetFileEnumerator( DirectoryInfo directory,
                                                     IFileNameFilter filter )
        {
            return new FileListingEnumerator( directory, filter );
        }

        /// <summary>
        /// Returns an array of <see cref="System.IO.FileInfo"/> object for the specified directory and file filter
        /// </summary>
        /// <param name="directory">The directory to search</param>
        /// <param name="filter">The file filter to use to filter filenames</param>
        /// <returns></returns>
        public static FileInfo [] GetFiles( DirectoryInfo directory,
                                            IFileNameFilter filter )
        {
            ArrayList fileList = new ArrayList();
            IEnumerator enumerator = GetFileEnumerator( directory, filter );
            while ( enumerator.MoveNext() ) {
                fileList.Add( enumerator.Current );
            }
            return (FileInfo []) fileList.ToArray( typeof ( FileInfo ) );
        }

        /// <summary>
        /// An enumerator whose 'current' returns
        /// </summary>
        private class FileListingEnumerator : EnumeratorWrapper
        {
            public FileListingEnumerator( DirectoryInfo directory,
                                          IFileNameFilter filter )
                : base( directory.GetFiles( filter.SearchPattern ).GetEnumerator() )
            {
                fFilter = filter;
            }

            public override bool MoveNext()
            {
                while ( base.MoveNext() ) {
                    FileInfo info = (FileInfo) base.Current;
                    if ( fFilter.Accept( info, info.Name ) ) {
                        return true;
                    }
                }
                return false;
            }

            private IFileNameFilter fFilter;
        }
    }
}
