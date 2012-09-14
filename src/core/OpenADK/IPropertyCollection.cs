//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;

namespace OpenADK.Library
{
    /// <summary>
    /// Used in places where name-value collections are needed
    /// </summary>
    public interface IPropertyCollection
    {
        /// <summary>
        /// Gets or Sets the property with the specified key
        /// </summary>
        string this[ string val ] { get; set; }

        /// <summary>
        /// Returns a collection of all Keys defined in the current collection
        /// </summary>
        ICollection Keys { get; }

        /// <summary>
        /// Returns the count of all settings defined in this collection
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Determines if the collection contains a property with the specified key
        /// </summary>
        /// <param name="key">The key of the property to check for in the collection</param>
        /// <returns>TRUE if the specified property is contained in the collection, otherwise FALSE</returns>
        bool Contains( string key );

        /// <summary>
        /// Clears the contents of this collection
        /// </summary>
        void Clear();
    }
}
