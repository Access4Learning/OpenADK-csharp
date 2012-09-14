//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>
    /// Summary description for MappingDirection.
    /// </summary>
    public enum MappingDirection
    {
        /// <summary> 	Direction flag passed to the <c>map</c> method to indicate the 
        /// agent is not specifying whether the method is being called for an inbound
        /// or outbound operation.</summary>
        Unspecified = 0,
        /// <summary> 	Direction flag passed to the <c>map</c> method to indicate the 
        /// agent is mapping values for an outbound message (for example, a SIF_Event 
        /// that is being reported to the zone or a SIF_Response being prepared.) 
        /// Currently, this flag is used only in conjunction with the <c>ValueSet</c> 
        /// attribute of the FieldMapping class. When a ValueSet is associated with 
        /// a FieldMapping rule and this flag is passed to the <c>map</c> 
        /// method, it will automatically lookup the ValueSet by ID and call 
        /// its <c>translate</c> function on the value produced from
        /// the mapping.</summary>
        /// <seealso cre
        Outbound = 1,

        /// <summary> 	Direction flag passed to the <c>map</c> method to indicate the 
        /// agent is mapping values for an inbound message (for example, a SIF_Event 
        /// that is received from the zone or a SIF_Response). Currently, this flag 
        /// is used only in conjunction with the <c>ValueSet</c> attribute of the 
        /// FieldMapping class. When a ValueSet is associated with a FieldMapping 
        /// rule and this flag is passed to the <c>map</c> method, it will 
        /// automatically lookup the ValueSet by ID and call its <c>translateReverse</c> 
        /// function on the value produced from the mapping.</summary>
        Inbound = 2
    }
}
