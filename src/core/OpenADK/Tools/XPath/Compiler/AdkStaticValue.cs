//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath.Compiler
{
    public class AdkStaticValue : AdkExpression
    {
        private object fValue;

        public AdkStaticValue( object value )
        {
            fValue = value;
        }

        /// <summary>
        ///  Evaluates the expression. If the result is a node set, returns
        /// the first element of the node set.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object ComputeValue( XsltContext context )
        {
            return fValue;
        }

        /// <summary>
        /// Implemented by subclasses and result is cached by isContextDependent()
        /// </summary>
        /// <returns></returns>
        protected override bool ComputeContextDependent()
        {
            return false;
        }

        public override string ToString()
        {
            if ( fValue is string )
            {
                return "'" + fValue + "'";
            }
            else
            {
                return fValue.ToString();
            }
        }
    }
}
