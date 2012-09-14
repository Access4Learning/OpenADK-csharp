//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpRequest.
    /// </summary>
    public class AdkHttpRequest
    {
        private Uri fUri;
        private Stream fRequestStream;
        private int fContentLength = -1;
        private NameValueCollection fHeaders = new NameValueCollection();
        private string fProtocol;
        private AdkHttpConnection fConnection;
        private string fMethod;
        private string fPath;
        private bool fIsGet = false;


        public AdkHttpRequest( AdkHttpConnection connection )
        {
            fConnection = connection;
        }

        private void ParseRequestLine( TextReader reader )
        {
            // HTTP request lines are of the form:
            // [METHOD] [Encoded URL] HTTP/1.?

            string aRequestLine = reader.ReadLine();
            if ( aRequestLine == null ) {
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ClientError_400_Bad_Request, "Expected Http Headers" );
            }

            string [] tokens = aRequestLine.Split( new char [] {' '} );
            if ( tokens.Length != 3 ) {
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ClientError_400_Bad_Request,
                      "Improperly formed request line" );
            }

            fMethod = tokens[0];

            fPath = tokens[1];
            // Only accept valid urls
            if ( !fPath.StartsWith( "/" ) ) {
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ClientError_400_Bad_Request, "Bad Url" );
            }
            // Decode all encoded parts of the URL using the built in URI processing class
            int i = 0;
            while ( (i = fPath.IndexOf( "%", i )) != -1 ) {
                fPath = fPath.Substring( 0, i ) + Uri.HexUnescape( fPath, ref i ) +
                        fPath.Substring( i );
            }
            fProtocol = tokens[2];
            if ( !fProtocol.StartsWith( "HTTP/" ) ) {
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ClientError_400_Bad_Request,
                      "Unknown protocol: " + fProtocol );
            }
        }

        public void Receive( AdkSocketConnection connection )
        {
            if ( fContentLength == -1 ) {
                MemoryStream initialBuffer =
                    new MemoryStream
                        ( connection.RawBuffer, 0, connection.RawBufferLength, false, false );
                AdkHttpHeadersReader reader = new AdkHttpHeadersReader( initialBuffer );
                ParseRequestLine( reader );
                ParseHeaders( reader );
                // This server does not support transfer encoding
                if ( this.Headers["Transfer-Encoding"] != null ) {
                    throw new AdkHttpException
                        ( AdkHttpStatusCode.ServerError_501_Not_Implemented,
                          "'Transfer-Encoding' is not supported" );
                }


                IPEndPoint endPoint = (IPEndPoint) connection.Socket.LocalEndPoint;
                string host = fHeaders["Host"];
                if ( host == null ) {
                    host = endPoint.Address.ToString();
                }
                else {
                    int loc = host.IndexOf( ':' );
                    if ( loc > 0 ) {
                        host = host.Substring( 0, loc );
                    }
                }
                // TODO: Implement better parsing of the query string. This will sometimes blow up when it shouldn't.
                string [] pathAndQuery = this.Path.Split( '?' );
                UriBuilder builder =
                    new UriBuilder
                        ( "http", host, connection.Binding.Port, pathAndQuery[0],
                          pathAndQuery.Length > 1 ? pathAndQuery[1] : "" );
                fUri = new Uri( builder.ToString() );

                // Now create the request stream, but chop of the headers
                if ( this.ContentLength > 0 ) {
                    fRequestStream = new MemoryStream( (int) this.ContentLength );
                    fRequestStream.Write
                        ( connection.RawBuffer, (int) reader.Position,
                          (int) (connection.RawBufferLength - reader.Position) );
                }
            }
            else if ( ReceiveComplete ) {
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ClientError_400_Bad_Request,
                      "Too much data received for specified Content-Length" );
            }
            else {
                fRequestStream.Write( connection.RawBuffer, 0, connection.RawBufferLength );
            }
        }

        public bool ReceiveComplete
        {
            get { return fIsGet || fRequestStream.Length >= this.ContentLength; }
        }


        public string Path
        {
            get { return fPath; }
        }

        public string Protocol
        {
            get { return fProtocol; }
        }

        public string Method
        {
            get { return fMethod; }
        }

        public NameValueCollection Headers
        {
            get { return fHeaders; }
        }

        public string HostName
        {
            get { return fHeaders["Host"]; }
        }

        public string ConnectionHeader
        {
            get { return fHeaders["Connection"]; }
        }

        public string RemoteAddress
        {
            get { return fConnection.ClientEndPoint.Address.ToString(); }
        }

        public string ContentType
        {
            get { return fHeaders["Content-Type"]; }
        }

        public long ContentLength
        {
            get
            {
                if ( fContentLength == -1 ) {
                    string length = fHeaders["Content-Length"];
                    if ( length == null ) {
                        fContentLength = 0;
                    }
                    try {
                        fContentLength = int.Parse( length );
                    }
                    catch {
                        fContentLength = 0;
                    }
                }
                return fContentLength;
            }
        }

        private void ParseHeaders( TextReader reader )
        {
            string aLine;
            string aHeaderName = null;
            // The headers end with either a socket close (!) or an empty line
            while ( (aLine = reader.ReadLine()) != null ) {
                if ( aLine.Length > 0 ) {
                    // If the value begins with a space or a hard tab then this
                    // is an extension of the value of the previous header and
                    // should be appended
                    if ( aHeaderName != null && Char.IsWhiteSpace( aLine[0] ) ) {
                        string aVal = fHeaders[aHeaderName];
                        aVal += aLine;
                        fHeaders[aHeaderName] = aVal;
                        continue;
                    }
                    // Headers consist of [NAME]: [VALUE] + possible extension lines
                    int firstColon = aLine.IndexOf( ":" );
                    if ( firstColon > -1 ) {
                        aHeaderName = aLine.Substring( 0, firstColon );
                        string value = aLine.Substring( firstColon + 1 ).Trim();
                        fHeaders[aHeaderName] = value;
                    }
                    else {
                        throw new ArgumentException( "Bad header: " + aLine );
                    }
                }
            }

            if ( fMethod == "GET" ) {
                fContentLength = 0;
                fIsGet = true;
            }
            else {
                if ( fHeaders["Content-Length"] == null ) {
                    throw new AdkHttpException
                        ( AdkHttpStatusCode.ClientError_411_Length_Required,
                          "Expected Content-Length header" );
                }
                fIsGet = false;
            }
        }

        /// <summary>
        /// Returns the entire stream of data that has been read from the transport
        /// </summary>
        /// <returns></returns>
        public Stream GetRequestStream()
        {
            if ( !(fRequestStream.Position == 0) ) {
                fRequestStream.Seek( 0, SeekOrigin.Begin );
            }
            return fRequestStream;
        }

        public Uri Url
        {
            get { return fUri; }
        }
    }
}
