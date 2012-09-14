//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using OpenADK.Library;

namespace OpenADK.Util
{
    /// <summary>
    /// Summary description for BasePropertyCollection.
    /// </summary>
    [Serializable]
    public class AdkPropertyCollection : NameValueCollection, IPropertyCollection
    {
        /// <summary>
        /// DeSerialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AdkPropertyCollection( SerializationInfo info,
                                         StreamingContext context )
            : base( info, context ) {}

        /// <summary>
        /// Public Constructor
        /// </summary>
        public AdkPropertyCollection() {}

        ICollection IPropertyCollection.Keys
        {
            get { return base.Keys; }
        }

        /// <summary>
        /// Determines if the collection contains a property with the specified key
        /// </summary>
        /// <param name="key">The key of the property to check for in the collection</param>
        /// <returns>TRUE if the specified property is contained in the collection, otherwise FALSE</returns>
        public bool Contains( string key )
        {
            return this[key] != null;
        }
    }
}
