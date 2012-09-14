//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;

namespace OpenADK.Library
{
    /// <summary>
    /// 
    /// </summary>
    public class SifContext
    {
        protected internal const string SIF_DEFAULT = "SIF_Default";

        /// <summary>
        /// The name of this context e.g. "SIF_Default"
        /// </summary>
        private readonly string fContextName;

        static SifContext()
        {
            DEFAULT = new SifContext(SIF_DEFAULT);
            sDefinedContexts = new Dictionary<String, SifContext>();
        }

        ///
        /// A list of SIF Contexts that have been defined by this agent instance 
        ///
        private static readonly IDictionary<String, SifContext> sDefinedContexts;



        /// <summary>
        /// The SifContext instance representing the SIF_Default context
        /// </summary>
        public static readonly SifContext DEFAULT;


        private SifContext(String contextName)
        {
            fContextName = contextName;
        }

        /// <summary>
        /// Creates a SIFContext object with the given name. If the name
        /// matches a context name already defined by the ADK, the existing context will
        /// be returned.
        /// </summary>
        /// <param name="contextName"></param>
        /// <returns></returns>
        public static SifContext Create(String contextName)
        {
            if (contextName == null ||
                 contextName.Length == 0 ||
                 contextName == SIF_DEFAULT || //optimized
                 contextName.ToLower().Equals(SIF_DEFAULT.ToLower()))
            {
                return DEFAULT;
            }

            // Determine if the context is already defined
            SifContext returnValue; 
            if( !sDefinedContexts.TryGetValue( contextName, out returnValue ) )
            {
                returnValue = new SifContext(contextName);
                sDefinedContexts[contextName] = returnValue;
            }

            return returnValue; 
            
        }


        /// <summary>
        /// Returns a SIFContext that has been defined by the agent, <c>null</c>
        /// if the context has not been defined
        /// </summary>
        /// <param name="contextName"></param>
        /// <returns>the matching SIFContext instance or <c>null</c> if it
        /// has not been defined</returns>
        public static SifContext IsDefined(String contextName)
        {
            if (contextName == null ||
                    contextName.Length == 0 ||
                    String.Compare( contextName, SIF_DEFAULT, true ) == 0 )
            {
                return DEFAULT;
            }
            SifContext returnValue;
            sDefinedContexts.TryGetValue( contextName, out returnValue );
            return returnValue;
        }


        /// <summary>
        /// Returns the name of this context (e.g. SIF_Default)
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get { return fContextName; }
        }

        /// <summary>
        /// Evaluates the native wrapped value of this object to see if
        /// it equals the value of the compared object, using a case-insensitive comparison
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(Object o)
        {
            if (this == o)
            {
                return true;
            }
            if ((o != null) && (o is SifContext))
            {
                SifContext compared = (SifContext)o;
                return
                    StringComparer.InvariantCultureIgnoreCase.Compare
                        (fContextName, compared.fContextName) == 0;
            }
            return false;
        }

        /// <summary>
        /// Returns a case-insensitive hashcode, based on the context name
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(fContextName);
        }
    }
}
