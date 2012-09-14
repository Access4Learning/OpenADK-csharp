//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;

using OpenADK.Util;
using OpenADK.Library;
using OpenADK.Library.us;


namespace Library.Examples.SimpleSubscriber
{
    internal class SimpleSubscriber : Agent
    {
        // Call the superclass constructor with the agent ID
        protected SimpleSubscriber()
            : base( "SimpleSubscriber" ) {}

        /// <summary>
        /// Run the agent as an application
        /// </summary>
        /// <param name="args"></param>
        /// <example>Debug Commandline arguments: /zone test /url http://127.0.0.1:7080/test</example>
        public static void Main( String[] args )
        {
            SimpleSubscriber agent = null;
            try {
                if ( args.Length < 2 ) {
                    Console.WriteLine
                        ( "Usage: SimpleSubscriber /zone zone /url url [/full] [options]" );
                    AdkExamples.printHelp(); // /zone test /url http://127.0.0.1:7080/test 
                    return;
                }

                //	Pre-parse the command-line before initializing the Adk
                Adk.Debug = AdkDebugFlags.None;
                AdkExamples.parseCL( null, args );

                //  Initialize the Adk with the specified version, loading only the learner SDO package
                Adk.Initialize( AdkExamples.Version, SIFVariant.SIF_US, (int)SdoLibraryType.Student );

                //  Start the agent...
                agent = new SimpleSubscriber();

                // Call StartAgent. This method does not return until the agent shuts down
                agent.startAgent( args );

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


        ///<summary>This method shows how to connect to zones and hook up a basic message handler</summary>
        ///<param name="args"></param>
        private void startAgent( String[] args )

        {
            this.Initialize();
            NameValueCollection parameters = AdkExamples.parseCL( this, args );

            String zoneId = parameters["zone"];
            String url = parameters["url"];

            if ( zoneId == null || url == null ) {
                Console.WriteLine( "The /zone and /url parameters are required" );
                Environment.Exit( 0 );
            }


            // 1) Get an instance of the zone to connect to
            IZone zone = ZoneFactory.GetInstance( zoneId, url );

            // 2) Create an instance of the the StudentPersonalHandler class
            // This class is responsible for dealing with events and responses
            // received that contain LearnerPersonal data objects
            StudentPersonalHandler sdh = new StudentPersonalHandler();
            // The StudentPersonalHandler is allowed to provision itself with the zone
            sdh.provision( zone );

            // Connect
            zone.Connect( AdkExamples.Reg ? ProvisioningFlags.Register : ProvisioningFlags.None );

            // The StudentPersonalHandler class has a sync() method, which it uses to 
            // request data from the SIF zone
            sdh.sync( zone );
        }
    }
}
