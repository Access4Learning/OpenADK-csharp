//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using OpenADK.Util;
using Microsoft.Win32;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpSocketHandler.
    /// </summary>
    internal class AdkHttpFileRequestHandler : IAdkHttpHandler
    {
        internal AdkHttpFileRequestHandler( AdkHttpDirectoryServer virtualDirectoryServer )
        {
            fVirtualDirectoryServer = virtualDirectoryServer;
        }

        /**
			 * We need to make sure that the url that we are trying to treat as a file
			 * lies below the document root of the http server so that people can't grab
			 * random files off your computer while this is running.
			 */

        public void ProcessRequest( AdkHttpRequestContext context )
        {
            try {
                FileInfo filename =
                    new FileInfo( fVirtualDirectoryServer.MapPath( context.Request.Url ) );
                if ( !filename.FullName.StartsWith( fVirtualDirectoryServer.PhysicalPath ) ) {
                    context.Response.Status = AdkHttpStatusCode.ClientError_403_Forbidden;
                }
                else {
                    if ( !filename.Exists ) {
                        if ( filename.Extension == "" ) {
                            context.Response.Status = AdkHttpStatusCode.Redirection_302_Found;
                            string newUrl = context.Request.Url.AbsolutePath + "/";
                            context.Response.Headers["Location"] = newUrl;
                            context.Response.AdditionalInfo = "<a href=\"" + newUrl + "\">" + newUrl +
                                                              "</a>";
                            return;
                        }
                        else {
                            throw new FileNotFoundException( "File Not Found", filename.FullName );
                        }
                    }
                    string aFileTime =
                        filename.LastWriteTimeUtc.ToString( "ddd, dd MMM yyyy HH:mm:ss" ) + " GMT";
                    string aDateTime = context.Request.Headers["If-Modified-Since"];
                    if ( aDateTime != null && aFileTime == aDateTime ) {
                        context.Response.Status = AdkHttpStatusCode.Redirection_304_Not_Modified;
                    }
                    else {
                        context.Response.ContentType = GetContentType( filename );
                        context.Response.Headers["Last-Modified"] = aFileTime;
                        // Open the file
                        using (
                            FileStream fs =
                                new FileStream( filename.FullName, FileMode.Open, FileAccess.Read )
                            ) {
                            Streams.CopyStream( fs, context.Response.GetResponseStream(), 4096 );
                            fs.Close();
                        }
                    }
                }
            }
            catch ( FileNotFoundException ) {
                context.Response.Status = AdkHttpStatusCode.ClientError_404_Not_Found;
            }
        }

        private string GetContentType( FileInfo file )
        {
            string aReturnVal = "text/html";
            using ( RegistryKey aKey = Registry.ClassesRoot.OpenSubKey( file.Extension, false ) ) {
                if ( aKey != null ) {
                    string aVal = (string) aKey.GetValue( "Content Type" );
                    if ( aVal != null ) {
                        aReturnVal = aVal;
                    }
                    aKey.Close();
                }
            }
            return aReturnVal;
        }


        private AdkHttpDirectoryServer fVirtualDirectoryServer;
    }
}
