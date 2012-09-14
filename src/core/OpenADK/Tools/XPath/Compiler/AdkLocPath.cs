//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Text;
using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath.Compiler
{
    public class AdkLocPath : AdkXPath
    {
        private bool fIsAbsolute = false;

        public AdkLocPath( bool absolute, params AdkXPathStep[] steps )
            : base( steps )
        {
            fIsAbsolute = absolute;
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


        protected override bool ComputeContextDependent()
        {
            if( !fIsAbsolute)
            {
                return true;
            }
            return base.ComputeContextDependent();
        }


        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            AdkXPathStep[] steps = Steps;
            if ( steps != null )
            {
                for ( int i = 0; i < steps.Length; i++ )
                {
                    if ( i > 0 || fIsAbsolute )
                    {
                        buffer.Append( '/' );
                    }
                    buffer.Append( steps[i] );
                }
            }
            return buffer.ToString();
        }
    }
}
