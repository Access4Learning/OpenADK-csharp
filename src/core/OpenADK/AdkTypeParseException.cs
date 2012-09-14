//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OpenADK.Library
{
    /// <summary>
    /// Thrown when the ADK attempts to parse a SIF XML Value into a SIF SimpleType
    /// </summary>
    public class AdkTypeParseException : AdkParsingException
    {
        public AdkTypeParseException( string msg, IZone zone, Exception innerException ) : base( msg, zone, innerException )
        {
        }

        public AdkTypeParseException( string msg, IZone zone ) : base( msg, zone )
        {
        }

        public AdkTypeParseException( SerializationInfo info, StreamingContext context ) : base( info, context )
        {
        }
    }
}
