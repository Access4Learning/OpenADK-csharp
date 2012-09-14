//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;
using OpenADK.Library;
using OpenADK.Util;
using OpenADK.Library.us;

namespace Library.Examples.SimpleProvider
{
    class SimpleProvider : Agent
    {
          // Call the superclass constructor with the agent ID
        protected SimpleProvider()
            : base( "SimpleProvider" ) { }

        /// <summary>
        /// Run the agent as an application
        /// </summary>
        /// <example>
        /// Pull mode
        ///Debug Command line arguments: /zone test /url http://127.0.0.1:7080/test /pull /events true</example>
        /// <example> Push mode
        /// /zone test /url http://127.0.0.1:7080/test /push /port 10000 /events true</example>
        /// <param name="args"></param>
        public static void Main( String[] args )
        {
            SimpleProvider agent = null;
            try {
                if ( args.Length < 2 ) {
                    Console.WriteLine
                        ( "Usage: SimpleSubscriber /zone zone /url url [/full] [options]" );
                    AdkExamples.printHelp();
                    return;
                }

                //	Pre-parse the command-line before initializing the Adk
                Adk.Debug = AdkDebugFlags.None;
                AdkExamples.parseCL( null, args );

                //  Initialize the Adk with the specified version, loading only the student SDO package
                Adk.Initialize( SifVersion.SIF24, SIFVariant.SIF_US, (int)SdoLibraryType.Student);

                //  Start the agent...
                agent = new SimpleProvider();

                // Call StartAgent. This method does not return until the agent shuts down
                agent.StartAgent( args );

                //	Wait for Ctrl-C to be pressed
                Console.WriteLine( "Agent is running (Press Ctrl-C to stop)" );
                new AdkConsoleWait().WaitForExit();
            }
            catch ( Exception e ) {
                Console.WriteLine( e );
            }
            finally {
                if ( agent != null && agent.Initialized ) {
                    //  Always shutdown the agent on exit
                    try {
                        agent.Shutdown
                            ( AdkExamples.Unreg
                                  ? ProvisioningFlags.Unprovide
                                  : ProvisioningFlags.None );
                    }
                    catch ( AdkException adkEx ) {
                        Console.WriteLine( adkEx );
                    }
                }
            }
        }


        /**
	 * This method shows how to connect to zones and hook up a basic message handler
	 * @param args
	 */
	private void StartAgent( String[] args )
	{
            this.Initialize();
            NameValueCollection parameters = AdkExamples.parseCL( this, args );
            //    /url http://127.0.0.1:7080/test /zone test /pull /sourceID SimpleProvider
            String zoneId = parameters["zone"];
            String url = parameters["url"];

            if ( zoneId == null || url == null ) {
                Console.WriteLine( "The /zone and /url parameters are required" );
                Environment.Exit( 0 );
            }


            // 1) Get an instance of the zone to connect to
            IZone zone = ZoneFactory.GetInstance( zoneId, url );
          		
		// 2) Create an instance of the the StudentPersonalProvider class
		// This class is responsible for publishing LearnerPersonal
		StudentPersonalProvider lpp = new StudentPersonalProvider();
		// The StudentPersonalProvider is allowed to provision itself with the zone
	 lpp.Provision( zone );
		
	
		// 3) Connect to zones
      
		zone.Connect( AdkExamples.Reg ? ProvisioningFlags.Register : ProvisioningFlags.None );
		
		// The StudentPersonalProvider will periodically send changes to the StudentPersonal
		// objects
        if( parameters["events"] != null )
        {
			lpp.StartEventProcessing( zone );
		}
	}



    }
}
