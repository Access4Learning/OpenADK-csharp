//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Tools.XPath.Compiler
{
    public abstract class AdkXPath : AdkExpression
    {
        private AdkXPathStep[] fSteps;

        protected AdkXPath( params AdkXPathStep[] steps )
        {
            fSteps = steps;
        }

        public AdkXPathStep[] Steps
        {
            get { return fSteps; }
        }

        protected override bool ComputeContextDependent()
        {
            if ( fSteps != null )
            {
                foreach ( AdkXPathStep step in fSteps )
                {
                    if ( step.IsContextDependent() )
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
