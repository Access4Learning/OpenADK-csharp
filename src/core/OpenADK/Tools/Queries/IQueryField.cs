//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Tools.Queries
{
    /// <summary>
    /// This interface defines an optional interface that can be implemented by a class and mapped to an element to satisfy a SIF_Query.</summary>
    /// <remarks>
    /// When mapped to a a SIF_Query element and passed in as part of the table to the <see cref="QueryFormatter.Format"/> method, this class
    /// will be invoked if query contains a condition that maps to the field
    /// </remarks>
    public interface IQueryField
    {
        /// <summary>
        /// Returns a portion of an SQL query that should be used to satisfy this specific query condition
        /// </summary>
        /// <param name="formatter">The formatter that is formatting the query</param>
        /// <param name="query">The SIF_Query that is being processed</param>
        /// <param name="cond">The specific condition that this class should render</param>
        /// <returns>an sql query appropriate for the formatter being used</returns>
        string Render( QueryFormatter formatter,
                       Library.Query query,
                       Condition cond );
    }
}
