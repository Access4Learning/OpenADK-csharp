//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using Edustructures.SifWorks;
using Edustructures.SifWorks.Tools.Mapping;

namespace SifWorks.Examples.Mapping
{
    /// <summary>
    /// Custom macro that can be called during the outbound mapping process within the ADK.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class can be used as an example for how to call custom macros via the agent.cfg file.
    /// To call it correctly, the agent configuration file will need to apply an "=" operator to the 
    /// right of the XPath assignment in a field rule, and provide a fully qualified path to the method
    /// to be called in this class.
    /// </para>
    /// <para>
    /// For example in the agent.cfg file, you should have something like:
    /// </para>
    /// <para>
    /// <example>
    /// &lt;field name="zipcode"&gt;AddressList/Address[@Type='0123']/PostalCode=@SifWorks.Examples.Mapping.DataUtilMacro.CleanupPostalCode($(zipcode))&lt;/field&gt;
    /// </example>
    /// </para>
    /// <para>
    /// Notice the use of the "@" symbol in front of the fully qualified path. That is the indicator to the 
    /// ADK Mappings engine that a macro needs to be called. Also, when passing in the needed arguments to the 
    /// macro method, the argument name must be enclosed in "$()", and the name of the argument should match 
    /// the value of the name attribute in the field tag.
    /// </para>
    /// <para>
    /// Custom macros must derive from <seealso cref="DefaultValueBuilder" >DefaultValueBuilder</seealso> as this class 
    /// contains the <seealso cref="DefaultValueBuilder.DefaultClass">DefaultClass</seealso> property. This property is 
    /// initialized to the path of the DefaultValueBuilder and must be overridden with the AssemblyQualified path of 
    /// your macro class.
    /// </para>
    /// <para>
    /// It is recommended that in the constructor of your macro class that the DefaultClass property be set. For example:
    /// <example>
    ///     <code>
    ///     public DataUtilMacro(IFieldAdaptor data) : base(data) { 
    ///         DefaultClass = typeof(DataUtilMacro).AssemblyQualifiedName; 
    ///     }
    ///     </code>
    /// </example>
    /// </para>
    /// 
    /// </remarks>
    public class DataUtilMacro : DefaultValueBuilder
    {
        /// <summary>
        /// Constructor called by the ADK. Set the <seealso cref="Edustructures.SifWorks.DefaultValueBuilder.DefaultClass">DefaultClass</seealso> property here so the ADK can find your macro method to call.
        /// </summary>
        /// <param name="data"></param>
        public DataUtilMacro(IFieldAdaptor data)
            : base(data)
        {
            DefaultClass = typeof(DataUtilMacro).AssemblyQualifiedName;
        }

        /// <summary>
        /// Strips Postal codes to report only the first 5 digits
        /// </summary>
        /// <returns></returns>
        public static String CleanupPostalCode(IValueBuilder vb, string zipcode)
        {
            //we only want to report 4 digit zip codes
            string formatted = zipcode;
            int index = zipcode.IndexOf("-");
            if (index != -1)
            {
                formatted = zipcode.Substring(0, index);
            }

            return formatted;
        }
    }
}
