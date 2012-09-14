//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath.Compiler
{
    public class AdkEqualOperation : AdkOperation
    {
        public AdkEqualOperation( AdkExpression arg1, AdkExpression arg2 ) : base( arg1, arg2 )
        {
        }

        /// <summary>
        /// Returns the XPath symbol for this operation, e.g. "+", "div", etc.
        /// </summary>
        protected override string Symbol
        {
            get { return "="; }
        }

        /// <summary>
        /// Computes the precedence of the operation
        /// </summary>
        /// <returns></returns>
        protected override int Precedence
        {
            get { return 2; }
        }

        /// <summary>
        /// Returns true if the operation is not sensitive to the order of arguments,
        /// e.g. "=", "and" etc, and false if it is, e.g. "&lt;=", "div".
        /// </summary>
        protected override bool Symmetric
        {
            get { return true; }
        }

        /// <summary>
        ///  Evaluates the expression. If the result is a node set, returns
        /// the first element of the node set.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object ComputeValue( XsltContext context )
        {
            object value1 = fArgs[0].ComputeValue( context );
            object value2 = fArgs[1].ComputeValue( context );
            if ( value1 == null )
            {
                return value2 == null;
            }
            return value1.Equals( value2 );
        }
    }
}
