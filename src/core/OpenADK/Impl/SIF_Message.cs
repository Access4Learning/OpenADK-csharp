//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Impl
{
    /// <summary>  Used internally by the class framework to encapsulate a SIF message envelope.
    /// 
    /// Agents should not construct SIF_Message objects directly.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  SIF10r1
    /// @since SIF10r1
    /// </version>
    internal class SIF_Message : SifMessagePayload
    {
        public SIF_Message()
            : base(SifDtd.SIF_MESSAGE) { }

        /// <summary>  Gets the value of the <c>Version</c> attribute.
        /// The SIF specification defines the meaning of this attribute as: "The version of SIF to which this message conforms"
        /// </summary>
        /// <value> The <c>Version</c> attribute of this object.
        /// </value>
        /// <version>  1.1
        /// @since 1.0r1
        /// </version>
        public virtual String Version
        {
            get { return GetFieldValue(SifDtd.SIF_MESSAGE_VERSION); }

            set { SetField(SifDtd.SIF_MESSAGE_VERSION, value); }
        }
    }
}
