//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;
using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath.Compiler
{
    internal class AdkFunction : AdkOpExpression
    {
        public AdkFunction( String name )
        {
            fName = name;
        }

        private string fName;

        public String FunctionName
        {
            get { return fName; }
        }

        /// <summary>
        ///  Evaluates the expression. If the result is a node set, returns
        /// the first element of the node set.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object ComputeValue( XsltContext context )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An extension function gets the current context, therefore it MAY be
        /// context dependent.
        /// </summary>
        /// <returns></returns>
        protected override bool ComputeContextDependent()
        {
            return true;
        }

        public override String ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append( fName );
            buffer.Append( '(' );
            AdkExpression[] args = Arguments;
            if ( args != null )
            {
                for ( int i = 0; i < args.Length; i++ )
                {
                    if ( i > 0 )
                    {
                        buffer.Append( ", " );
                    }
                    buffer.Append( args[i] );
                }
            }
            buffer.Append( ')' );
            return buffer.ToString();
        }
    }
}
