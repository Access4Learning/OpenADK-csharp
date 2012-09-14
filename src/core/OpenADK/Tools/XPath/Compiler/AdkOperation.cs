//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;

namespace OpenADK.Library.Tools.XPath.Compiler
{
    public abstract class AdkOperation : AdkOpExpression
    {
        protected AdkOperation( params AdkExpression[] args ) : base( args )
        {
        }

        /// <summary>
        /// Returns the XPath symbol for this operation, e.g. "+", "div", etc.
        /// </summary>
        protected abstract String Symbol { get; }

        /// <summary>
        /// Computes the precedence of the operation
        /// </summary>
        /// <returns></returns>
        protected abstract int Precedence { get; }

        /// <summary>
        /// Returns true if the operation is not sensitive to the order of arguments,
        /// e.g. "=", "and" etc, and false if it is, e.g. "&lt;=", "div".
        /// </summary>
        protected abstract bool Symmetric { get; }

        public override String ToString()
        {
            if ( fArgs.Length == 1 )
            {
                return Symbol + Parenthesize( fArgs[0], false );
            }
            else
            {
                StringBuilder buffer = new StringBuilder();
                for ( int i = 0; i < fArgs.Length; i++ )
                {
                    if ( i > 0 )
                    {
                        buffer.Append( ' ' );
                        buffer.Append( Symbol );
                        buffer.Append( ' ' );
                    }
                    buffer.Append( Parenthesize( fArgs[i], i == 0 ) );
                }
                return buffer.ToString();
            }
        }

        private String Parenthesize( AdkExpression expression, bool left )
        {
            if ( !(expression is AdkOperation) )
            {
                return expression.ToString();
            }
            AdkOperation op = (AdkOperation) expression;
            int myPrecedence = Precedence;
            int thePrecedence = op.Precedence;

            bool needParens = true;
            if ( myPrecedence < thePrecedence )
            {
                needParens = false;
            }
            else if ( myPrecedence == thePrecedence )
            {
                if ( Symmetric )
                {
                    needParens = false;
                }
                else
                {
                    needParens = !left;
                }
            }

            if ( needParens )
            {
                return "(" + expression.ToString() + ")";
            }
            else
            {
                return expression.ToString();
            }
        }
    }
}
