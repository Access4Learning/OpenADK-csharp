//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpException.
    /// </summary>
    [Serializable]
    public class AdkHttpException : Exception
    {
        /// <summary>
        /// Constructs an exception with an HttpStatus code and a detailed error message
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public AdkHttpException( AdkHttpStatusCode code,
                                 string message )
            : base( _constructErrorMessage( code, message ) )
        {
            fCode = _getErrorCode( code );
        }

        /// <summary>
        ///  Constructs an exception with an HttpStatus code, a detailed error message and the cause of the exception
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="innerException">the exception that caused this exception to be raised</param>
        public AdkHttpException( AdkHttpStatusCode code,
                                 string message,
                                 Exception innerException )
            : base( _constructErrorMessage( code, message ), innerException )
        {
            fCode = _getErrorCode( code );
        }

        /// <summary>
        /// The "magic" constructor used in .Net serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected AdkHttpException( SerializationInfo info,
                                    StreamingContext context )
        {
            fCode = (AdkHttpStatusCode) info.GetInt32( "fCode" );
        }

        public override void GetObjectData( SerializationInfo info,
                                            StreamingContext context )
        {
            base.GetObjectData( info, context );
            info.AddValue( "fCode", (int) fCode );
        }

        private static AdkHttpStatusCode _getErrorCode( AdkHttpStatusCode code )
        {
            if ( (int) code < 400 ) {
                code = AdkHttpStatusCode.ServerError_500_Internal_Server_Error;
            }
            return code;
        }

        private static string _constructErrorMessage( AdkHttpStatusCode code,
                                                      string message )
        {
            AdkHttpStatusCode finalCode = _getErrorCode( code );
            string enumName = code.ToString();
            // Find the third underscore and convert the remaining portion of the enum name to a string
            int loc = enumName.IndexOf( '_' );
            loc = enumName.IndexOf( '_', loc + 1 );
            enumName = enumName.Substring( loc + 1 );
            enumName = enumName.Replace( '_', ' ' );
            return string.Format( "HTTP Error {0} {1} : {2}", (int) finalCode, enumName, message );
        }


        public AdkHttpStatusCode HttpExceptionCode
        {
            get { return fCode; }
        }

        private AdkHttpStatusCode fCode;
    }
}
