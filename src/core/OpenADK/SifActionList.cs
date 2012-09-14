//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    [Serializable]
    public class SifActionList<TValue> : SifKeyedList<TValue>
        where TValue : SifKeyedElement
    {
        /// <summary>
        /// Creates an instance of a SifActionList
        /// </summary>
        /// <param name="def"></param>
        public SifActionList( IElementDef def )
            : base( def ) {}


        /// <summary>
        /// .Net Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected SifActionList( SerializationInfo info,
                                 StreamingContext context )
            : base( info, context ) {}

    }
}
