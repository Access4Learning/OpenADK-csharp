//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Globalization;
using System.IO;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for AdkHttpFileServerDirectory.
    /// </summary>
    public class AdkHttpDirectoryServer : IAdkHttpHandlerFactory
    {
        public AdkHttpDirectoryServer( string inPhysicalPath,
                                       string inVirtualPath,
                                       string inDefaultFileName )
        {
            fPhysicalPath = inPhysicalPath;
            fVirtualPath = inVirtualPath;
            fLowerCasedVirtualPath = CultureInfo.InvariantCulture.TextInfo.ToLower( fVirtualPath );
            fLowerCasedVirtualPathWithTrailingSlash = fVirtualPath.EndsWith( "/" )
                                                          ? fVirtualPath
                                                          : fVirtualPath + "/";
            fLowerCasedVirtualPathWithTrailingSlash =
                CultureInfo.InvariantCulture.TextInfo.ToLower
                    ( fLowerCasedVirtualPathWithTrailingSlash );
            fDefaultFileName = inDefaultFileName;
        }

        public string DefaultFileName
        {
            get { return fDefaultFileName; }
            set { fDefaultFileName = value; }
        }


        public string NormalizedVirtualPath
        {
            get { return fLowerCasedVirtualPathWithTrailingSlash; }
        }

        public string PhysicalPath
        {
            get { return fPhysicalPath; }
        }

        public String VirtualPath
        {
            get { return fVirtualPath; }
        }


        public bool IsVirtualPathInApp( String path )
        {
            if ( path == null ) {
                return false;
            }

            if ( fVirtualPath == "/" && path.StartsWith( "/" ) ) {
                return true;
            }

            path = CultureInfo.InvariantCulture.TextInfo.ToLower( path );

            if ( path.StartsWith( fLowerCasedVirtualPathWithTrailingSlash ) ) {
                return true;
            }

            if ( path == fLowerCasedVirtualPath ) {
                return true;
            }

            return false;
        }


        public bool IsVirtualPathAppPath( String path )
        {
            if ( path == null ) {
                return false;
            }

            path = CultureInfo.InvariantCulture.TextInfo.ToLower( path );
            return
                (path == fLowerCasedVirtualPath || path == fLowerCasedVirtualPathWithTrailingSlash);
        }


        public string MapPath( Uri url )
        {
            // Replace the forward slashes with back-slashes to make a file name
            string filename = url.LocalPath.Replace( "/", "\\" );
            // Chop off the virtual path portion
            if ( fLowerCasedVirtualPath.Length > 1 ) {
                filename = filename.Substring( fLowerCasedVirtualPath.Length );
            }
            // Construct a filename from the doc root and the filename
            if ( filename.EndsWith( @"\" ) ) {
                filename += this.DefaultFileName;
            }
            filename = filename.Replace( "%20", " " );
            DirectoryInfo aRoot = new DirectoryInfo( this.PhysicalPath );
            FileInfo file = new FileInfo( aRoot.FullName + filename );
            // Make sure they aren't trying in funny business by checking that the
            // resulting canonical name of the file has the doc root as a subset.
            return file.FullName;
        }

        #region IAdkHttpHandlerFactory Members

        public IAdkHttpHandler CreateHandler( AdkHttpRequest request )
        {
            if ( request.Method != "GET" ) {
                // TODO: We should be adding an "Allow" header as per RFC 2616
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ClientError_405_Method_Not_Allowed,
                      request.Method + " Method Not Allowed" );
            }
            return new AdkHttpFileRequestHandler( this );
        }

        #endregion

        private string fPhysicalPath;
        private string fVirtualPath;
        private string fLowerCasedVirtualPath;
        private string fLowerCasedVirtualPathWithTrailingSlash;
        private string fDefaultFileName;
    }
}
