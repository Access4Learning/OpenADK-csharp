//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library;
namespace OpenADK.Util
{
    /// <summary>
    /// Summary description for ClassFactory.
    /// </summary>
    public sealed class ClassFactory
    {
        /// <summary>
        /// This method instantiates an object for the fully qualified class name
        /// </summary>
        /// <param name="className">The fully qualified class name to create</param>
        /// <returns>a created instance of the specified type</returns>
        /// <exception cref="System.TypeLoadException">Thrown if the type cannot be found</exception>
        public static object CreateInstance( string className )
        {
            return CreateInstance( className, false );
        }


        /// <summary>
        /// This method instantiates an object for the fully qualified class name
        /// </summary>
        /// <param name="className">The fully qualified class name to create</param>
        /// <param name="nonPublic">If true, both public and non-public constructors are searched for. If false, only the public constructor is called</param>
        /// <returns>a created instance of the specified type</returns>
        /// <exception cref="System.TypeLoadException">Thrown if the type cannot be found</exception>
        public static object CreateInstance( string className,
                                             bool nonPublic )
        {
            object aObject = null;
            Type aType = Type.GetType( className );
            

            if ( aType != null ) {
                aObject = Activator.CreateInstance( aType, nonPublic );
            }
            else {
                throw new TypeLoadException( "The class " + className + " could not be found!" );
            }

            return aObject;
        }
    }
}
