//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;
using OpenADK.Library.Infra;
using log4net;

namespace OpenADK.Library.Impl
{
    /// <summary>  Utility routines used internally by the Adk
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    internal class AdkUtils
    {
//		/// <summary>  Throws an AdkMessagingException, optionally logging its message first</summary>
//		public static void  _throw(AdkMessagingException thr, Category log)
//		{
//			if ((Adk.Debug & AdkDebugFlags.Exceptions) != 0)
//				thr.log(log);
//			throw thr;
//		}
//		
//		/// <summary>  Throws an AdkTransportException, optionally logging its message first</summary>
//		public static void  _throw(AdkTransportException thr, Category log)
//		{
//			if ((Adk.Debug & AdkDebugFlags.Exceptions) != 0)
//				thr.log(log);
//			throw thr;
//		}

        /// <summary>  Throws a SifException, optionally logging its message first</summary>
        public static void _throw( SifException thr,
                                   ILog log )
        {
            SifException exc = thr;

            //  If exception has a non-success status code and no errors, substitute a
            //  more descriptive exception
            if ( thr.Ack != null && (!thr.Ack.HasStatusCode( 0 ) && !thr.Ack.HasError()) ) {
                StringBuilder b = new StringBuilder();
                b.Append( "Received non-success status code (" );

                SIF_Status s = thr.Ack.SIF_Status;
                if ( s == null ) {
                    b.Append( "and no SIF_Status element exists" );
                }
                else {
                    b.Append( s.SIF_Code );
                }

                b.Append( ") but no error information" );

                exc = new SifException( b.ToString(), thr.Ack, thr.Zone );
            }

            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 ) {
                exc.Log( log );
            }

            throw exc;
        }

//		/// <summary>  Throws an AdkQueueException, optionally logging its message first</summary>
//		public static void  _throw(AdkQueueException thr, Category log)
//		{
//			if ((Adk.Debug & AdkDebugFlags.Exceptions) != 0)
//				thr.log(log);
//			throw thr;
//		}

        /// <summary>  Throws an AdkException, optionally logging its message first</summary>
        public static void _throw( AdkException thr,
                                   ILog log )
        {
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 ) {
                thr.Log( log );
            }
            throw thr;
        }

        /// <summary>  Throws an exception, optionally logging its message first</summary>
        public static void _throw( SystemException thr,
                                   ILog log )
        {
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 ) {
                log.Error( thr.Message, thr );
            }
            throw thr;
        }

        /// <summary>  Throws an exception, optionally logging its message first</summary>
        public static void _throw( Exception thr,
                                   ILog log )
        {
            if ( (Adk.Debug & AdkDebugFlags.Exceptions) != 0 ) {
                log.Error( thr.Message, thr );
            }
            throw thr;
        }

//		/// <summary>  Throws an Error, optionally logging its message first</summary>
//		public static void  _throw(System.ApplicationException thr, Category log)
//		{
//			if ((Adk.Debug & AdkDebugFlags.Exceptions) != 0)
//			{
//				
//				log.Debug(thr.ToString());
//			}
//			throw thr;
//		}
    }
}
