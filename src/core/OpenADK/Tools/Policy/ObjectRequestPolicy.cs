//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Tools.Policy
{
    /// <summary>
    /// Contains policy information that prescribes how the ADK requests
    /// </summary>
    public class ObjectRequestPolicy : ObjectPolicy
    {
        private String fRequestVersion;
        private String fSourceId;

        public ObjectRequestPolicy( IElementDef objectType ) : base( objectType )
        {
        }

        /// <summary>
        /// Gets the SIF version that should be used for requesting objects
        /// of this type.
        /// </summary>
        /// <value>The version to use when requesting this object. E.g. "1.1" or "2.*"</value>
        public String RequestVersion
        {
            get { return fRequestVersion; }
            set { fRequestVersion = value; }
        }


        /// <summary>
        /// Gets or sets the SourceId of the agent from whom data should be requested 
        /// for this object type
        /// </summary>
        public String RequestSourceId
        {
            get { return fSourceId; }
            set { fSourceId = value; }
        }
    }
}
