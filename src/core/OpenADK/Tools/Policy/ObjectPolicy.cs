//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library.Tools.Policy
{
    /// <summary>
    /// Represents policy around a specific type of object
    /// </summary>
    public abstract class ObjectPolicy
    {

        private IElementDef fObjectType;


        /// <summary>
        /// Creates an instance of ObjectPolicy
        /// </summary>
        /// <param name="objectType">The metadata definition of the SIF data object
        /// that this ObjectPolicy applies to</param>
        protected ObjectPolicy(IElementDef objectType)
        {
            fObjectType = objectType;
        }

        /// <summary>
        ///  Returns the metadata definition of the SIF data object that this
        /// ObjectPolicy applies to
        /// </summary>
        public IElementDef ObjectType
        {
            get { return fObjectType; }
        }

    }
}
