//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using OpenADK.Library;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;
using OpenADK.Util;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpPushProtocolHandler.
    /// </summary>
    internal class AdkHttpPushProtocolHandler : BaseHttpProtocolHandler, IAdkHttpHandler
    {
        internal AdkHttpPushProtocolHandler( AdkHttpApplicationServer server, HttpTransport transport ) : base(transport)
        {
            fServer = server;
        }

        #region Overrides of BaseHttpProtocolHandler

        public override void Start()
        {
            try {
                string ctx = this.BuildContextPath();
                fServer.AddHandlerContext( "", ctx, this, true );

                // TODO: Investigate if we need to implement the anonymous handler

                #region Disabled Code

                /*	if (false)
					{
						//  Also establish a catch-all handler for "/" in case the ZIS
						//  disregards our URL path of "/zone/{zoneId}/". Any traffic
						//  received on this handler will be routed as follows: if the
						//  agent is connected to only one zone in Push mode, the
						//  traffic is assumed to have come from that zone. If the
						//  agent is connected to more than one zone in Push mode, the
						//  traffic is disregarded (i.e. no Ack is returned to the ZIS,
						//  which means the message will remain in the agent's queue.)
						//  If the agent is connected to a ZIS that behaves this way
						//  it must use Pull mode.
						//
						if (sAnonymousHandler == null)
						{
							sAnonymousHandler = new AnonymousHttpHandler();
							context = server.addContext("/");
							context.addHandler(sAnonymousHandler);
							server.addContext(context);
							context.start();
						}
					}
					*/

                #endregion
            }
            catch ( Exception e ) {
                throw new AdkException
                    ( "HttpProtocolHandler could not establish HttpContext: " + e, this.Zone );
            }
        }


        public override void Shutdown()
        {
            if ( fServer != null ) {
                fServer.RemoveHandlerContext( this.BuildContextPath() );
            }
        }

        public override void Close( IZone zone )
        {
            fServer.RemoveHandlerContext( this.BuildContextPath() );
        }

        #endregion

        private string BuildContextPath()
        {
            return "/zone/" + this.Zone.ZoneId + "/";
        }

        private SIF_Ack ProcessPush( SifMessagePayload parsed )
        {
            try {
                //  Dispatch. When the result is an Integer it is an ack code to
                //  return; otherwise it is ack data to return and the code is assumed
                //  to be 1 for an immediate acknowledgement.
                int ackStatus = this.Zone.Dispatcher.dispatch( parsed );

                //  Ask the original message to generate a SIF_Ack for itself
                return parsed.ackStatus( ackStatus );
            }
            catch ( SifException se ) {
                return
                    parsed.AckError( se );
            }
            catch ( AdkException adke ) {
                return
                    parsed.AckError
                        ( SifErrorCategoryCode.Generic, SifErrorCodes.GENERIC_GENERIC_ERROR_1,
                          adke.Message );
            }
            catch ( Exception thr ) {
                if ( (Adk.Debug & AdkDebugFlags.Messaging) != 0 ) {
                    this.Zone.Log.Debug( "Uncaught exception dispatching push message: " + thr );
                }

                return
                    parsed.AckError
                        ( SifErrorCategoryCode.Generic, SifErrorCodes.GENERIC_GENERIC_ERROR_1,
                          "An unexpected error has occurred", thr.ToString() );
            }
        }

        #region Push Handling

        #region IAdkHttpHandler Members


        public void ProcessRequest( AdkHttpRequestContext context )
        {
            if ( (Adk.Debug & AdkDebugFlags.Messaging) != 0 ) {
                Zone.Log.Debug
                    ( "Received push message from " + context.Request.RemoteAddress + " (" +
                      context.Request.Url.Scheme + ")" );
            }

            SIF_Ack ack = null;
            SifMessagePayload parsed = null;

            //Check request length and type
            if (!SifIOFormatter.IsValidContentLength(context.Request.ContentLength)) {
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ClientError_400_Bad_Request,
                      "Content length Must be greater than zero" );
            }

            if (!SifIOFormatter.IsValidContentMediaType(context.Request.ContentType)) {
                throw new AdkHttpException(AdkHttpStatusCode.ClientError_415_Unsupported_Media_Type, 
                    "Content header does not support the specified media type: " + context.Request.ContentType);
            }
            
            //  Read raw content
            Stream requestStream = context.Request.GetRequestStream();
            StringBuilder requestXml = null;

            // If we need to convert the request stream to a string for either logging or messaging, do so
            if ( (Adk.Debug & AdkDebugFlags.Message_Content) != 0 ) {
                requestXml = ConvertRequestToString( requestStream );
                Zone.Log.Debug
                    ( "Received " + context.Request.ContentLength + " bytes:\r\n" +
                      requestXml.ToString() );
            }

            TextReader requestReader = new StreamReader( requestStream, SifIOFormatter.ENCODING );

            Exception parseEx = null;
            bool reparse = false;
            bool cancelled = false;
            int reparsed = 0;

            do {
                try {
                    parseEx = null;

                    //  Parse content
                    parsed = (SifMessagePayload) CreateParser().Parse( requestReader, Zone );
                    reparse = false;
                    parsed.LogRecv( Zone.Log );
                }
                catch ( AdkParsingException adke ) {
                    parseEx = adke;
                }
                catch ( Exception ex ) {
                    parseEx = ex;
                }

                //		
                //	Notify listeners...
                //	
                //	If we're asked to reparse the message, do so but do not notify 
                //	listeners the second time around.
                //
                if ( reparsed == 0 ) {
                   ICollection<IMessagingListener> msgList = MessageDispatcher.GetMessagingListeners(Zone);
                    if ( msgList.Count > 0 ) {
                        // Convert the stream to a string builder
                        if ( requestXml == null ) {
                            requestXml = ConvertRequestToString( requestStream );
                        }

                        //	Determine message type before parsing
                        foreach ( IMessagingListener listener in msgList ) {
                            try {
                                SifMessageType pload =
                                    Adk.Dtd.GetElementType( parsed.ElementDef.Name );
                                MessagingReturnCode code =
                                    listener.OnMessageReceived( pload, requestXml );
                                switch ( code ) {
                                    case MessagingReturnCode.Discard:
                                        cancelled = true;
                                        break;

                                    case MessagingReturnCode.Reparse:
                                        requestReader = new StringReader( requestXml.ToString() );
                                        reparse = true;
                                        break;
                                }
                            }
                            catch ( AdkException adke ) {
                                parseEx = adke;
                            }
                        }
                    }
                }

                if ( cancelled ) {
                    return;
                }

                reparsed++;
            }
            while ( reparse );

            if ( parseEx != null ) {
                //  TODO: Handle the case where SIF_OriginalSourceId and SIF_OriginalMsgId
				//  are not available because parsing failed. See SIFInfra
				//  Resolution #157.
				if( parseEx is SifException && parsed != null )
				{
					//  Specific SIF error already provided to us by SIFParser
					ack = parsed.AckError( (SifException)parseEx );
				}
				else{
					String errorMessage = null;
					if( parseEx is AdkException )
					{
						errorMessage = parseEx.Message;
					} else {
						// Unchecked Throwable
						errorMessage = "Could not parse message";
					}

					if( parsed == null )
					{
						SifException sifError = null;
						if( parseEx is SifException ){
							sifError = (SifException) parseEx;

						}else {
							sifError = new SifException(
                                SifErrorCategoryCode.Xml,
								SifErrorCodes.XML_GENERIC_ERROR_1,
								"Could not parse message", 
                                parseEx.ToString(), 
                                this.Zone, 
                                parseEx );
						}
                        if( requestXml == null )
                        {
                            requestXml = ConvertRequestToString( requestStream );
                        }
						ack = SIFPrimitives.ackError(
                                requestXml.ToString( ),
								sifError,
								this.Zone );
					}
					else
					{
						ack = parsed.AckError(
							SifErrorCategoryCode.Generic,
							SifErrorCodes.GENERIC_GENERIC_ERROR_1,
				    		errorMessage,
					    	parseEx.ToString() );
					}

				}

                if ( (Adk.Debug & AdkDebugFlags.Messaging) != 0 ) {
                    Zone.Log.Warn
                        ( "Failed to parse push message from zone \"" + Zone + "\": " + parseEx );
                }

                if ( ack != null ) {
                    //  Ack messages in the same version of SIF as the original message
                    if ( parsed != null ) {
                        ack.SifVersion = parsed.SifVersion;
                    }
                    AckPush( ack, context.Response );
                }
                else {
                    //  If we couldn't build a SIF_Ack, returning an HTTP 500 is
                    //  probably the best we can do to let the server know that
                    //  we didn't get the message. Note this should cause the ZIS
                    //  to resend the message, which could result in a deadlock
                    //  condition. The administrator would need to manually remove
                    //  the offending message from the agent's queue.

                    if ( (Adk.Debug & AdkDebugFlags.Messaging) != 0 ) {
                        Zone.Log.Debug
                            ( "Could not generate SIF_Ack for failed push message (returning HTTP/1.1 500)" );
                    }
                    throw new AdkHttpException
                        ( AdkHttpStatusCode.ServerError_500_Internal_Server_Error,
                          "Could not generate SIF_Ack for failed push message (returning HTTP/1.1 500)" );
                }

                return;
            }

            //  Check SourceId to see if it matches this agent's SourceId
            String destId = parsed.DestinationId;
            if ( destId != null && !destId.Equals( Zone.Agent.Id ) ) {
                Zone.Log.Warn
                    ( "Received push message for DestinationId \"" + destId +
                      "\", but agent is registered as \"" + Zone.Agent.Id + "\"" );

                ack = parsed.AckError
                    (
                    SifErrorCategoryCode.Transport,
                    SifErrorCodes.WIRE_GENERIC_ERROR_1,
                    "Message not intended for this agent (SourceId of agent does not match DestinationId of message)",
                    "Message intended for \"" + destId + "\" but this agent is registered as \"" +
                    Zone.Agent.Id + "\"" );

                AckPush( ack, context.Response );

                return;
            }

            //  Convert content to SIF message object and dispatch it
            ack = ProcessPush( parsed );

            //  Send SIF_Ack reply
            AckPush( ack, context.Response );
        }

        #endregion

        private StringBuilder ConvertRequestToString( Stream requestStream )
        {
            requestStream.Seek( 0, SeekOrigin.Begin );
            StreamReader reader = new StreamReader( requestStream, SifIOFormatter.ENCODING );
            StringBuilder data = new StringBuilder( reader.ReadToEnd() );
            // Reset the stream so that it can be read again.
            requestStream.Seek( 0, SeekOrigin.Begin );
            return data;
        }

        private void AckPush( SIF_Ack ack,
                              AdkHttpResponse response )
        {
            try {
                //  Set SIF_Ack / SIF_Header fields
                SIF_Header hdr = ack.Header;
                hdr.SIF_Timestamp = DateTime.Now;

                hdr.SIF_MsgId = SifFormatter.GuidToSifRefID( Guid.NewGuid() );
                hdr.SIF_SourceId = this.Zone.Agent.Id;

                ack.LogSend( this.Zone.Log );

                response.ContentType = SifIOFormatter.CONTENTTYPE;
                // TODO: This code may need to change. The ADKHttpResponse should not automatically set the content length
                // and other implementations will not do so.
                SifWriter w = new SifWriter( response.GetResponseStream() );
                w.Write( ack );
                w.Flush();
            }
            catch ( Exception thr ) {
                Console.Out.WriteLine
                    ( "HttpProtocolHandler failed to send SIF_Ack for pushed message (zone=" +
                      this.Zone.ZoneId + "): " + thr );
                throw new AdkHttpException
                    ( AdkHttpStatusCode.ServerError_500_Internal_Server_Error, thr.Message, thr );
            }
        }

        #endregion

        #region Private Fields

        //public static AnonymousHttpHandler sAnonymousHandler;
        //private int URI_OFFSET;


        /// <summary>  The internal  http/https server</summary>
        protected internal static AdkHttpApplicationServer fServer = null;

        #endregion

        /// <summary>
        /// Returns true if the protocol and underlying transport are currently active
        /// for this zone
        /// </summary>
        /// <param name="zone"></param>
        /// <returns>True if the protocol handler and transport are active</returns>
        public override bool IsActive( ZoneImpl zone )
        {
            return fTransport.IsActive( zone ) && fServer != null && fServer.IsStarted;
        }

        /// <summary>
        /// Creates the SIF_Protocol object that will be included with a SIF_Register
        /// message sent to the zone associated with this Transport.</Summary>
        /// <remarks>
        /// The base class implementation creates an empty SIF_Protocol with zero
        /// or more SIF_Property elements according to the parameters that have been
        /// defined by the client via setParameter. Derived classes should therefore
        /// call the superclass implementation first, then add to the resulting
        /// SIF_Protocol element as needed.
        /// </remarks>
        /// <param name="zone"></param>
        /// <returns></returns>
        public override SIF_Protocol MakeSIF_Protocol( IZone zone )
        {
            SIF_Protocol proto = new SIF_Protocol();
            fTransport.ConfigureSIF_Protocol(proto, zone);
            return proto;
        }
    }
}
