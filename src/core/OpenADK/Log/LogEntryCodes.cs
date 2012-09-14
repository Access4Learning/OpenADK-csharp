//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Log
{
    /// <summary> 	SIF_LogEntry error category and code constants as defined by the SIF 1.5 Specification.<p></summary>
    public class LogEntryCodes
    {
        //	Categories
        public const int CATEGORY_SUCCESS = 1;
        public const int CATEGORY_DATA_ISSUES_WITH_SUCCESS = 2;
        public const int CATEGORY_DATA_ISSUES_WITH_FAILURE = 3;
        public const int CATEGORY_ERROR = 4;

        //	Success Category Codes
        public const int CODE_SUCCESS = 1;

        //	Data Issues with Success Result Category Codes
        public const int CODE_DATA_CHANGED_SUCCESS = 1;
        public const int CODE_DATA_ADDED_SUCCESS = 2;

        //	Data Issues with Failure Result Category Codes
        public const int CODE_INSUFFICIENT_INFO_FAILURE = 1;
        public const int CODE_BUSINESS_RULE_FAILURE = 2;
        public const int CODE_INCOMPLETE_DATA_FAILURE = 3;

        //	Agent Error Conditions Category Codes
        public const int CODE_AGENT_FAILURE = 1;

        //	ZIS Error Conditions Category Codes
        public const int CODE_ZIS_FAILURE = 1;
        public const int CODE_MAXBUFFERSIZE_FAILURE = 2;
        public const int CODE_INSECURE_CHANNEL_FAILURE = 3;
    }
}
