//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;
using OpenADK.Library;
using OpenADK.Library.uk;
using OpenADK.Util;


internal class SimpleProvider : Agent
{
    // Call the superclass constructor with the agent ID
    protected SimpleProvider()
        : base("SimpleProvider")
    {
    }


    /// <summary>
    /// Run the agent as an application
    /// </summary>
    /// <example>
    /// Pull mode
    ///Debug Command line arguments: /zone test /url http://127.0.0.1:7080/test /pull /events true</example>
    /// <example> Push mode
    /// /zone test /url http://127.0.0.1:7080/test /push /port 10000 /events true</example>
    /// <param name="args"></param>

    [STAThread]
    public static int Main(string[] args)
    {
        SimpleProvider agent = null;
        try
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: SimpleProvider /zone zone /url url [/events] [options]");
                Console.WriteLine("    /zone zone     The name of the zone");
                Console.WriteLine("    /url url       The zone URL");
                Console.WriteLine("    /events        Periodically send change events");
                AdkExamples.printHelp();
                return 0;
            }

            Console.ForegroundColor = ConsoleColor.Green;

            //	Pre-parse the command-line before initializing the ADK
            Adk.Debug = AdkDebugFlags.None;
            AdkExamples.parseCL(null, args);

            //  Initialize the ADK with the specified version, loading only the Student SDO package
            Adk.Initialize(SifVersion.SIF23, SIFVariant.SIF_UK, (int)SdoLibraryType.All );

            //  Start the agent...
            agent = new SimpleProvider();
            
            // Call StartAgent. This method does not return until the agent shuts down
            agent.startAgent(args);

            //	Wait for Ctrl-C to be pressed
            Console.WriteLine("Agent is running (Press Ctrl-C to stop)");
            new AdkConsoleWait().WaitForExit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            if (agent != null && agent.Initialized)
            {
                //  Always shutdown the agent on exit
                try
                {
                    agent.Shutdown
                        (AdkExamples.Unreg
                             ? ProvisioningFlags.Unregister
                             : ProvisioningFlags.None);
                }
                catch (AdkException adkEx)
                {
                    Console.WriteLine(adkEx);
                }
            }
        }
        return 0;
    }

    /**
     * This method shows how to connect to zones and hook up a basic message handler
     * @param args
     */

    private void startAgent(string[] args)
    {
        Initialize();
        NameValueCollection parameters = AdkExamples.parseCL(this, args);

        string zoneId = parameters["zone"];
        string url = parameters["url"];

        if (zoneId == null || url == null)
        {
            Console.WriteLine("The /zone and /url parameters are required");
            Console.Beep();
            throw new ArgumentException("The /zone and /url parameters are required");
        }


        // 1) Get an instance of the zone to connect to
        IZone zone = ZoneFactory.GetInstance(zoneId, url);

        // 2) Create an instance of the the LearnerPersonalProvider class
        // This class is responsible for publishing LearnerPersonal
        LearnerPersonalProvider lpp = new LearnerPersonalProvider();
        // The LearnerPersonalProvider is allowed to provision itself with the zone
        lpp.provision(zone);

        // 2) Create an instance of the the LearnerPersonalProvider class
        // This class is responsible for publishing LearnerPersonal
        WorkforcePersonalProvider wpp = new WorkforcePersonalProvider();
        // The LearnerPersonalProvider is allowed to provision itself with the zone

        wpp.provision(zone);

        // Connect
        zone.Connect(AdkExamples.Reg ? ProvisioningFlags.Register : ProvisioningFlags.None);

        // The StudentPersonalProvider will periodically send changes to the StudentPersonal
        // objects
        if (parameters["events"] != null)
        {
            lpp.startEventProcessing(zone);
        }
    }
}
