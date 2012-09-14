//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Impl.Surrogates;

namespace OpenADK.Library
{
    /// <summary>
    /// Represents information about an IElementDef that is specific to a version of SIF
    /// </summary>
    public interface IElementVersionInfo
    {
        /// <summary>
        /// The XML tag name used for this version of SIF
        /// </summary>
        string Tag { get; }

        /// <summary>
        /// A RenderSurrogate instance, if necessary for rendering this element in this version of SIF
        /// </summary>
        IRenderSurrogate GetSurrogate();
        /// <summary>
        /// The Sequence number of this element in this version of SIF
        /// </summary>
        int Sequence { get; }

        /// <summary>
        /// Does this element "collapse" in this version of SIF? This happens with list container elements
        /// in SIF 1.x
        /// </summary>
        Boolean IsCollapsed { get;}

        /// <summary>
        /// Is this element repeatable in this version of SIF?
        /// </summary>
        Boolean IsRepeatable { get; }
        
        /// <summary>
        /// Is this an element or attribute in this version of SIF?
        /// </summary>
        /// <returns></returns>
        bool IsAttribute{ get; }
    }
}
