//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Tools.Mapping
{
    public enum MappingBehavior
    {
        /// <summary>
        /// the field mapping behavior for null fields is unspecified. In this
        /// case the behavior is identical to <see cref="IfNullDefault"/>
        /// </summary>
        IfNullUnspecified = 0,
        /// <summary>
        /// Specifies that if the field being mapped is null, this field mapping
        /// should use the default value, if set
        /// </summary>
        IfNullDefault = 1,
        /// <summary>
        /// Specifies that if the field being mapped is null, this field mapping
        /// should not generate a SIF element, even if a default value is specified
        /// </summary>
        IfNullSuppress = 2
    }
}
