//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using OpenADK.Util;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpResponse.
    /// </summary>
    public class AdkHttpResponse
    {
        public AdkHttpResponse( AdkHttpConnection connection )
        {
            fResponseStream = new MemoryStream();
            fWriter = new StreamWriter( fResponseStream );
            fHeaders = new NameValueCollection();

            this.AddStandardHeaders( connection );
        }

        private void AddStandardHeaders( AdkHttpConnection connection )
        {
            fHeaders["Server"] = connection.Listener.Server.Name;
            fHeaders["Date"] =
                DateTime.Now.ToUniversalTime().ToString( "ddd, dd MMM yyyy HH:mm:ss" ) + " GMT";
        }

        public Stream GetResponseStream()
        {
            return fResponseStream;
        }

        public void Clear()
        {
            fWriter.Flush();
            fResponseStream.SetLength( 0 );
            fResponseStream.Seek( 0, SeekOrigin.Begin );
        }


        public void Write( string inValue )
        {
            fWriter.Write( inValue );
        }

        public void Flush()
        {
            fWriter.Flush();
        }


        public AdkHttpStatusCode Status
        {
            get { return fStatus; }
            set { fStatus = value; }
        }

        public NameValueCollection Headers
        {
            get { return fHeaders; }
        }

        public string ContentType
        {
            get { return fHeaders["Content-Type"]; }
            set { fHeaders["Content-Type"] = value; }
        }

        public string AdditionalInfo
        {
            get { return fAdditionalInfo; }
            set { fAdditionalInfo = value; }
        }

        /// <summary>
        /// Copies the response buffer to the outgoing socket using an efficient asynchronous pattern.
        /// </summary>
        /// <param name="socket">The socket to write the result to</param>
        /// <param name="request">The source request</param>
        /// <param name="finishHandler">The delegate to call when the asynchronous operation is complete</param>
        /// <param name="asyncState">The state that should be returned to the async delegate</param>
        internal void AsyncFinishRequest( AdkSocketConnection socket,
                                          AdkHttpRequest request,
                                          bool keepAlive )
        {
            string statusDescription = ParseDescription( fStatus );

            if ( this.Status != AdkHttpStatusCode.Success_200_OK ) {
                this.Clear();
                if ( ShouldReturnBody( this.Status ) ) {
                    this.Write( "<html><head><title>" );
                    this.Write( statusDescription );
                    this.Write( "</title></head><body><h1>HTTP " );
                    this.Write( ((int) fStatus).ToString() );
                    this.Write( " " );
                    this.Write( statusDescription );
                    this.Write( "</h1><br>" );
                    this.Write( this.AdditionalInfo );
                    this.Write( "</body></html>" );
                }
            }

            // Flush our underlying stream. We are done writing to the response and we need the correct length
            this.Flush();
            Stream outputStream = socket.GetOutputDataStream();
            StreamWriter aWriter = new StreamWriter( outputStream );
            {
                aWriter.Write( request.Protocol );
                aWriter.Write( " " );
                aWriter.Write( (int) fStatus );
                aWriter.Write( " " );
                aWriter.Write( statusDescription );
                aWriter.Write( "\r\n" );
                aWriter.Write( "Content-Length: " + fResponseStream.Length.ToString() + "\r\n" );

                if ( keepAlive ) {
                    aWriter.Write( "Connection: Keep-Alive\r\n" );
                }
                else {
                    aWriter.Write( "Connection: Close\r\n" );
                }

                foreach ( string aKey in fHeaders.Keys ) {
                    aWriter.WriteLine( "{0}: {1}", aKey, fHeaders[aKey] );
                }

                aWriter.Write( string.Empty + "\r\n" );
                aWriter.Flush();
                GC.SuppressFinalize( aWriter );
            }
            if ( socket.Connected && fResponseStream.Length > 0 ) {
                fWriter.Flush();
                GC.SuppressFinalize( fWriter );
                fResponseStream.Seek( 0, SeekOrigin.Begin );
                Streams.CopyStream( fResponseStream, outputStream, 4096 );
            }
        }

        private bool ShouldReturnBody( AdkHttpStatusCode code )
        {
            if ( code == AdkHttpStatusCode.Informational_100_Continue ||
                 code == AdkHttpStatusCode.Redirection_304_Not_Modified ) {
                return false;
            }
            else {
                return true;
            }
        }

        private string ParseDescription( AdkHttpStatusCode code )
        {
            if ( code == AdkHttpStatusCode.Success_200_OK ) {
                return "OK";
            }
            else {
                // Parse off the string before the second underscore
                // and add spaces before caps
                StringBuilder builder = new StringBuilder();
                int underscoreCount = 0;
                foreach ( char c in code.ToString().ToCharArray() ) {
                    if ( c == '_' ) {
                        underscoreCount++;
                        continue;
                    }
                    if ( underscoreCount > 1 ) {
                        if ( c > 64 && c < 91 ) {
                            builder.Append( ' ' );
                        }
                        builder.Append( c );
                    }
                }
                return builder.ToString().TrimStart();
            }
        }


        private NameValueCollection fHeaders;
        private StreamWriter fWriter;
        private MemoryStream fResponseStream;
        private AdkHttpStatusCode fStatus = AdkHttpStatusCode.Success_200_OK;
        private string fAdditionalInfo = string.Empty;
    }
}
