//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Tools.XPath.Compiler
{
    public abstract class AdkOpExpression : AdkExpression
    {
        protected AdkExpression[] fArgs;


        protected AdkOpExpression( params AdkExpression[] args )
        {
            fArgs = args;
        }

        public AdkExpression[] Arguments
        {
            get { return fArgs; }
        }

        protected override bool ComputeContextDependent()
        {
            if ( fArgs != null )
            {
                foreach ( AdkExpression expr in fArgs )
                {
                    if ( expr.IsContextDependent() )
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
