//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>
    /// Summary description for SifCategoryCode.
    /// </summary>
    public enum SifErrorCategoryCode
    {
        /// <summary>  Unknown (Error Category Code, never use if possible)</summary>
        Unknown = 0,

        /// <summary>  XML Validation category</summary>
        Xml = 1,

        /// <summary>  Encryption category</summary>
        Encryption = 2,

        /// <summary>  Authentication category</summary>
        Authentication = 3,

        /// <summary>  Access and Permissions category</summary>
        AccessAndPermissions = 4,

        /// <summary>  Registration category</summary>
        Registration = 5,

        /// <summary>  Provision category</summary>
        Provision = 6,

        /// <summary>  Subscription category</summary>
        Subscription = 7,

        /// <summary>  Request and Response category</summary>
        RequestResponse = 8,

        /// <summary>  Event Reporting and Processing category</summary>
        EventReportingAndProcessing = 9,

        /// <summary>  Transport category</summary>
        Transport = 10,

        /// <summary>  System category</summary>
        System = 11,

        /// <summary>  Agent Message Handling category. DEPRECATED: Please use SifCategoryCode.Generic instead</summary>
        [Obsolete( "Please use SifCategoryCode.Generic", false )] Agent = 12,

        /// <summary>  Generic Message Handling</summary>
        Generic = 12,

        /// <summary>
        /// Indicates that no SIFError is present
        /// </summary>
        None = -1 // 0xFFFFFFFF
    }
}
