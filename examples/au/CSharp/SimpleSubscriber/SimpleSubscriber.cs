//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;
using OpenADK.Library;
using OpenADK.Util;
using Library.Examples.SimpleSubscriber;
using OpenADK.Library.au;
/**
 *  A simple agent the demonstrates basic SIFAgent connectivity 
 *  for a subscriber agentusing the ADK
 *
 *  @version ADK 2.0
 */

public class SimpleSubscriber : Agent
{
    protected SimpleSubscriber() : base( "SimpleSubscriber" )
    {
    }

    /**
	 *  Run the agent as an application.
	 */

    [STAThread]
    public static void Main( string[] args )
    {
        SimpleSubscriber agent = null;
        try
        {
            if ( args.Length < 2 )
            {
                Console.WriteLine( "Usage: SimpleSubscriber /zone zone /url url [/full] [options]" );
                Console.Beep();
                AdkExamples.printHelp();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;

            //	Pre-parse the command-line before initializing the ADK
            Adk.Debug = AdkDebugFlags.None;
            AdkExamples.parseCL( null, args );

            //  Initialize the ADK with the specified version, loading only the learner SDO package
            Adk.Initialize( AdkExamples.Version, SIFVariant.SIF_AU, (int)SdoLibraryType.Student );

            //  Start the agent...
            agent = new SimpleSubscriber();

            // Call StartAgent. This method does not return until the agent shuts down
            agent.startAgent( args );


            //	Wait for Ctrl-C to be pressed
            Console.WriteLine( "Agent is running (Press Ctrl-C to stop)" );
            new AdkConsoleWait().WaitForExit();
        }
        catch ( Exception e )
        {
            Console.WriteLine( e );
        }
        finally
        {
            if ( agent != null && agent.Initialized )
            {
                //  Always shutdown the agent on exit
                try
                {
                    agent.Shutdown( AdkExamples.Unreg ? ProvisioningFlags.Unregister : ProvisioningFlags.None );
                }
                catch ( AdkException adkEx )
                {
                    Console.WriteLine( adkEx );
                }
            }
        }
    }

    /**
	 * This method shows how to connect to zones and hook up a basic message handler
	 * @param args
	 */

    private void startAgent( string[] args )
    {
        Initialize();
        NameValueCollection parameters = AdkExamples.parseCL( this, args );

        string zoneId = parameters["zone"];
        string url = parameters["url"];

        if ( zoneId == null || url == null )
        {
            Console.WriteLine( "The /zone and /url parameters are required" );
            Console.Beep();
            throw new ArgumentException( "The /zone and /url parameters are required" );
        }

        // 1) Get an instance of the zone to connect to
        IZone zone = ZoneFactory.GetInstance( zoneId, url );

        // 2) Create an instance of the the StudentPersonalHandler class
        // This class is responsible for dealing with events and responses
        // received that contain LearnerPersonal data objects
        StudentPersonalHandler sdh = new StudentPersonalHandler();
        // The LearnerPersonalHandler is allowed to provision itself with the zone
        sdh.provision( zone );

        // Connect
        zone.Connect( AdkExamples.Reg ? ProvisioningFlags.Register : ProvisioningFlags.None );

        // The LearnerPersonalHandler class has a sync() method, which it uses to 
        // request data from the SIF zone
        sdh.sync( zone );
    }
}
