//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;

namespace OpenADK.Library.Impl
{
    public class SifIOFormatter
    {
        private static Encoding sUTF8 = new UTF8Encoding( false );

        /// <summary>
        /// Returns the Encoding implementation used to encode SIF XML documents to binary transports
        /// </summary>
        public static Encoding ENCODING
        {
            get { return sUTF8; }
        }

        /// <summary>
        /// Returns the content type associated with a SIF  document
        /// </summary>
        public static string CONTENTTYPE
        {
            get { return "application/xml;charset=\"utf-8\""; }
        }

        public static string CONTENTTYPE_BASE 
        {
            get { return "application/xml"; }
        }
        
        public static string CONTENTTYPE_UTF8
        {
            get { return "utf-8"; }
        }

        /// <summary>
        /// Validates the length of the HTTP ContentLength header.
        /// Any value other than length 0 will return tru.
        /// </summary>
        /// <param name="contentLength"></param>
        /// <returns>bool true if greater than 1</returns>
        public static bool IsValidContentLength(long contentLength)
        {
            bool isValid = true;
            if (contentLength < 1) {
                isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// Checks the HTTP ContentType for existence of application/xml and charset of UTF-8
        /// </summary>
        /// <param name="httpContentType"></param>
        /// <returns></returns>
        public static bool IsValidContentMediaType(string httpContentType)
        {
            bool isValid = true;
            string contentTest = httpContentType.ToLower();
            if ((!contentTest.Contains(SifIOFormatter.CONTENTTYPE_BASE)) ||
                    (!contentTest.Contains(SifIOFormatter.CONTENTTYPE_UTF8))) {
                isValid = false;
            }
            return isValid;
        }
    }
}
