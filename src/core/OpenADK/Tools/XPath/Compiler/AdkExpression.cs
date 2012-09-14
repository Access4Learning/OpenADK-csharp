//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Xml.Xsl;

namespace OpenADK.Library.Tools.XPath.Compiler
{
    public abstract class AdkExpression

    {
        private bool fDependencyKnown = false;
        private bool fIsContextDependent;

        /// <summary>
        /// Returns true if this expression should be re-evaluated
        /// each time the current position in the context changes.
        /// </summary>
        /// <returns></returns>
        public bool IsContextDependent()
        {
            if ( !fDependencyKnown )
            {
                fIsContextDependent = ComputeContextDependent();
                fDependencyKnown = true;
            }
            return fIsContextDependent;
        }

        /// <summary>
        ///  Evaluates the expression. If the result is a node set, returns
        /// the first element of the node set.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract object ComputeValue( XsltContext context );

        /// <summary>
        /// Implemented by subclasses and result is cached by isContextDependent()
        /// </summary>
        /// <returns></returns>
        protected abstract bool ComputeContextDependent();
    }
}
