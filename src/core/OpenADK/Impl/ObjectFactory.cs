//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Configuration;
using OpenADK.Util;

namespace OpenADK.Library.Impl
{
    /// <summary>
    ///  Factory for creating objects used by the ADK. 
    /// </summary>
    /// <remarks>
    /// The following object are currently created by this class
    /// <list type="List">
    ///     <item>ZoneFactory (The ZoneFactory used to create zone objects in the ADK)</item>
    /// <item>TopicFactory (The Topic factory used by the ADK)</item>
    /// </list>
    /// </remarks>
    public abstract class ObjectFactory
    {

        /// <summary>
        /// The name of the System property that is checked for a class name used to
        /// create an instance of the ObjectFactory
        /// </summary>
        public const String OBJECT_FACTORY_CLASS = "adkglobal.factory.ObjectFactory";

        private static ObjectFactory sInstance;

        /// <summary>
        /// Returns the object factory used by the ADK to create objects
        /// </summary>
        /// <returns>the object factory used by the ADK to create objects</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ObjectFactory GetInstance()
        {
            if (sInstance == null)
            {
                String cls =  ConfigurationManager.AppSettings[OBJECT_FACTORY_CLASS];
                if( String.IsNullOrEmpty( cls )  )
                {
                    sInstance = new ObjectFactoryImpl();
                }
                else
                {
                    try
                    {
                        sInstance = (ObjectFactory)ClassFactory.CreateInstance( cls );
                    }
                    catch (Exception thr)
                    {
                        throw new Exception(
                                "ADK could not create an instance of the class "
                                        + cls + ": " + thr, null);
                    }
                }
            }
            return sInstance;
        }



        /// <summary>
        /// Creates an instance of the object factory of a specified type
        /// </summary>
        /// <param name="factoryType">the type of Object factory to return</param>
        /// <param name="agentInstance">the running instance of Agent (required)</param>
        /// <returns>the requested object factory</returns>
        public abstract Object CreateInstance(ADKFactoryType factoryType, Agent agentInstance);





        /// <summary>
        /// The Types of objects that can be created by ObjectFactory
        /// </summary>
        public enum ADKFactoryType
        {
            
            /// <summary>
            /// Indicates that the requested type implements IZoneFactory
            /// </summary>
            ZONE,

            /// <summary>
            /// Indicates that the requested type implements ITopicFactory
            /// </summary>
            TOPIC, 

            /// <summary>
            /// Indicates that the requested type is a subclass of PolicyManager
            /// </summary>
            POLICY_MANAGER,

            /// <summary>
            /// Indicates that the requested type is a subclass of PolicyFactory
            /// </summary>
            POLICY_FACTORY 

        }


    }
}
