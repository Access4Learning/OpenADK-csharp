//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.Impl
{

   ///<summary>
   ///Utilities for interacting with the SIFProfilerClient when support for the OpenADK
   ///SIF Profiling Harness is enabled.  
   ///</summary>
   public class ProfilerUtils
   {
#if PROFILED
      ///<summary>
      /// Profiler name assigned by the Agent class upon initialization
      ///</summary>
      private static String fProfName;

      ///<summary>
      ///Profiler session ID
      ///</summary>
      private static int fSessionId;

      ///<summary>
      ///Called by the agent when it is ready to initialize the SIFProfilerClient instance
      ///of the ADK. This must be done after the agent has learned the Session ID to associate
      ///with all recorded metrics.
      ///</summary>
      public static void StartProfiling(int sessionId, ObjectTypeCodes otcImpl)
	{
		Adk.Log.Debug( "SIFProfilerClient instance name: " + fProfName );
		ProfilerUtils.setProfilerSessionId( sessionId );
		ADK.getLog().debug( "SIFProfilerClient session ID: " + fSessionId );

        
		com.OpenADK.sifprofiler.SIFProfilerClient prof = 
			com.OpenADK.sifprofiler.SIFProfilerClient.getInstance( fProfName );
		if( prof != null ) {
			try {
				prof.setObjectTypeCodesImpl( otcImpl );
				prof.open( new com.OpenADK.sifprofiler.api.ProfilerSession( ProfilerUtils.getProfilerSessionId() ) );
			} catch( Exception ex ) {
				Console.WriteLine( "Failed to open SIFProfilerClient instance: " + ex );
				System.exit(-1);
			}
		}
	}

      ///<summary>
      ///Sets the profiler name. 
      ///This method is called by the Agent class upon initialization. 
      ///</summary>
      ///<param name="name">
      ///The string that identifies the SIFProfilerClient instance for the ADK,
      ///which may differ from other components being profiled. The name must be unique
      ///among all SIFProfilerClient instances.
      ///</param>
      public static void setProfilerName(String name)
      {
         fProfName = name;
      }

      ///<summary>
      ///Gets the profiler name.
      ///</summary>
      ///<returns>The string that identifies the SIFProfilerClient instance for the ADK,
      ///which may differ from other components being profiled. The name must be unique
      ///among all SIFProfilerClient instances.
      ///</returns>
      public static String getProfilerName()
      {
         return fProfName;
      }


      ///<summary>
      ///Sets the profiler name.
      ///This method is called by the Agent class upon initialization.
      /// </summary>
      ///<param name="id"> The string that identifies the SIFProfilerClient instance for the ADK,
      /// which may differ from other components being profiled. The name must be unique
      /// among all SIFProfilerClient instances.
      ///</param>
      public static void setProfilerSessionId(int id)
      {
         fSessionId = id;
      }


      /// <summary>
      /// Gets the profiler session ID.
      /// </summary>
      /// <returns></returns>
      public static int getProfilerSessionId()
      {
         return fSessionId;
      }

      /// <summary>
      ///Start recording metric data 
      /// </summary>
      /// <param name="oid"></param>
      /// <param name="objType"></param>
      /// <param name="msgId"></param>
      public static void profileStart(String oid, short objType, String msgId)
      {
         com.OpenADK.sifprofiler.SIFProfilerClient c = com.OpenADK.sifprofiler.SIFProfilerClient.getInstance(fProfName);
         if (c != null)
         {
            c.metricStart(oid + "." + objType, msgId, objType);
         }
      }

      /// <summary>
      /// Start recording metric data 
      /// </summary>
      /// <param name="oid"></param>
      /// <param name="objType"></param>
      /// <param name="msgId"></param>
      public static void profileStart(String oid, IElementDef objType, String msgId)
      {
         ProfilerUtils.
         Adk.Log.Debug("SIFProfilerClient instance name: " + fProfName);
         ProfilerUtils.setProfilerSessionId(sessionId);
         ADK.getLog().debug("SIFProfilerClient session ID: " + fSessionId);

 
         OpenADK.sifprofiler.SIFProfilerClient c = com.OpenADK.sifprofiler.SIFProfilerClient.getInstance(fProfName);
         if (c != null)
         {
            short objTypeCode = c.getObjectTypeCode(objType);
            c.metricStart(oid + "." + objTypeCode, msgId, objTypeCode);
         }
      }

      /// <summary>
      ///Stop recording metric data
      /// </summary>
      public static void profileStop()
      {
         com.OpenADK.sifprofiler.SIFProfilerClient c = com.OpenADK.sifprofiler.SIFProfilerClient.getInstance(fProfName);
         if (c != null)
         {
            c.metricStop();
         }
      }
  

#endif

   }
}
