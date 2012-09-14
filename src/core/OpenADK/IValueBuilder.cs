//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  Classes that implement the IValueBuilder interface evaluate an arbitrary
    /// expression to produce a string value. The string value is then assigned as
    /// the value to a SIF element or attribute.
    /// 
    /// The IValueBuilder interface is used by the SifDtd, SifDataObject, and Mappings
    /// classes when evaluating XPath-like query strings. It enables developers to
    /// customize the way the Adk evaluates value expressions in these query strings
    /// to produce a value for a SIF element or attribute. The DefaultIValueBuilder
    /// implementation supports <c>$(variable)</c> token replacement as well as
    /// <c>@com.class.method</c> style calls to static .Net methods.
    /// 
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public interface IValueBuilder
    {
        /// <summary>  Evaluate an expression to return a String value.
        /// 
        /// </summary>
        /// <param name="expression">The expression to evaluate
        /// </param>
        /// <returns> The value that results from the expression
        /// </returns>
        string Evaluate( string expression );
    }
}
