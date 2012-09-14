//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Summary description for HttpExceptionCode.
    /// </summary>
    public enum AdkHttpStatusCode
    {
        /// <summary>returns an Http Status code of 100 ( Continue )</summary>
        Informational_100_Continue = 100,
        /// <summary>returns an Http Status code of 200 ( OK )</summary>
        Success_200_OK = 200,
        /// <summary>returns an Http Status code of 301</summary>
        Redirection_301_Moved_Permanently = 301,
        /// <summary>returns an Http Status code of 302</summary>
        Redirection_302_Found = 302,
        /// <summary>returns an Http Status code of 304</summary>
        Redirection_304_Not_Modified = 304,
        /// <summary>returns an Http Status code of 400</summary>
        ClientError_400_Bad_Request = 400,
        /// <summary>returns an Http Status code of 403</summary>
        ClientError_403_Forbidden = 403,
        /// <summary>returns an Http Status code of 404</summary>
        ClientError_404_Not_Found = 404,
        /// <summary>returns an Http Status code of 405</summary>
        ClientError_405_Method_Not_Allowed = 405,
        /// <summary>returns an Http Status code of 411</summary>
        ClientError_411_Length_Required = 411,
        /// <summary>returns an Http Status code of 415</summary>
        ClientError_415_Unsupported_Media_Type = 415,
        /// <summary>returns an Http Status code of 500</summary>
        ServerError_500_Internal_Server_Error = 500,
        /// <summary>returns an Http Status code of 501</summary>
        ServerError_501_Not_Implemented = 501,
        /// <summary>returns an Http Status code of 502</summary>
        ServerError_502_Bad_Gateway = 502,
        /// <summary>returns an Http Status code of 503</summary>
        ServerError_503_Service_Unavailable = 503,
        /// <summary>returns an Http Status code of 504</summary>
        ServerError_504_Gateway_Timeout = 504,
        /// <summary>returns an Http Status code of 505</summary>
        ServerError_505_Http_Version_Not_Supported = 505
    }
}
