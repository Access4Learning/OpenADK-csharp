//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Globalization;
using System.Net;
using OpenADK.Library;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpConnection.
    /// </summary>
    public class AdkHttpConnection
    {
        public static void ProcessRequest( AdkSocketConnection socket,
                                           AdkHttpListener listener )
        {
            AdkHttpRequestContext context = socket.UserData as AdkHttpRequestContext;
            if ( context == null ) {
                AdkHttpConnection conn = new AdkHttpConnection( socket, listener );
                AdkHttpResponse response = new AdkHttpResponse( conn );
                AdkHttpRequest request = new AdkHttpRequest( conn );
                context = listener.Server.CreateContext( conn, request, response );
            }

            context.Connection.ProcessRequest( context, socket );
        }

        private AdkHttpConnection( AdkSocketConnection socket,
                                   AdkHttpListener listener )
        {
            fSocketConnection = socket;
            fListener = listener;
        }

        private void ProcessRequest( AdkHttpRequestContext context,
                                     AdkSocketConnection socket )
        {
            int aStart = Environment.TickCount;

            bool keepAlive = true;
            AdkHttpRequest request = context.Request;
            AdkHttpResponse response = context.Response;
            try {
                context.Request.Receive( socket );
                if ( !context.Request.ReceiveComplete ) {
                    socket.UserData = context;
                    return;
                }
                else {
                    IAdkHttpHandler handler = fListener.GetHandlerForContext( request );
                    handler.ProcessRequest( context );
                }
            }

            catch ( AdkHttpException httpEx ) {
                _logError( httpEx );
                response.Clear();
                response.Status = httpEx.HttpExceptionCode;
                if ( (int) httpEx.HttpExceptionCode > 499 ) {
                    keepAlive = false;
                }
                response.AdditionalInfo = httpEx.GetType().FullName + " - " + httpEx.Message + " - " +
                                          httpEx.StackTrace;
            }
            catch ( Exception ex ) {
                keepAlive = false;
                _logError( ex );
                // TODO : Implement more verbose error output ( internalexceptions and such )
                response.Clear();
                response.Status = AdkHttpStatusCode.ServerError_500_Internal_Server_Error;
                response.AdditionalInfo = ex.GetType().FullName + " - " + ex.Message + " - " +
                                          ex.StackTrace;
            }

            // Clear out the context state because we are done with this request and the socket
            // may remain open for the next request
            socket.UserData = null;

            if ( socket.Connected ) {
                if ( keepAlive ) {
                    keepAlive = ShouldKeepAlive( context.Request );
                }
                context.Response.Headers.Add( "X-Powered-By", fListener.Server.Name );
                // Write the Response
                context.Response.AsyncFinishRequest( socket, context.Request, keepAlive );

                if ( (Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0 &&
                     fListener.Server.Log.IsDebugEnabled ) {
                    fListener.Server.Log.Info
                        ( string.Format
                              ( "Processed Request for {0}:{1} ( {2} ) in {3} milliseconds",
                                this.ClientEndPoint.Address, this.ClientEndPoint.Port,
                                context.Request.Path, (Environment.TickCount - aStart).ToString() ) );
                }
                if ( !keepAlive ) {
                    socket.Close();
                }
            }
        }


        private void _logError( Exception ex )
        {
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 ) {
                fListener.Server.Log.Error( ex.Message, ex );
            }
        }

        private bool ShouldKeepAlive( AdkHttpRequest request )
        {
            if ( !fListener.KeepAlivesEnabled ) {
                return false;
            }
            string connection = request.ConnectionHeader;
            if ( request.Protocol == "HTTP/1.1" ) {
                // Keep Alive unless the Connection header is set to "close"
                return
                    string.Compare( connection, "close", true, CultureInfo.InvariantCulture ) != 0;
            }
            else {
                // Only keep alive if the Connection header is set to Keep-Alive
                return
                    string.Compare( connection, "keep-alive", true, CultureInfo.InvariantCulture ) ==
                    0;
            }
        }


        public bool IsConnected
        {
            get { return fSocketConnection.Connected; }
        }


        public IPEndPoint ServerEndPoint
        {
            get { return (IPEndPoint) fSocketConnection.Socket.LocalEndPoint; }
        }

        public IPEndPoint ClientEndPoint
        {
            get { return (IPEndPoint) fSocketConnection.Socket.RemoteEndPoint; }
        }

        public AdkHttpListener Listener
        {
            get { return fListener; }
        }

        private AdkSocketConnection fSocketConnection;
        private AdkHttpListener fListener;
    }
}
