//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using Edustructures.SifWorks;
using Edustructures.SifWorks.Infra;
using Edustructures.SifWorks.Tools.Cfg;
using Edustructures.Util;

namespace SifWorks.Examples.Chameleon
{
    /// <summary>
    /// Chameleon is a universal subscribing and logging agent. 
    /// </summary>
    public class Chameleon : Agent, IQueryResults
    {
        private delegate void SimpleMethod();


        private AgentConfig fCfg;
        private static AdkConsoleWait sWaitMutex;
        private ObjectLogger fLogger;
        private string fSessionId = Guid.NewGuid().ToString();

        public Chameleon()
            : base("SubscribeZilla")
        {
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                Adk.Debug = AdkDebugFlags.Detailed;
                Adk.Initialize();
                Chameleon agent;
                agent = new Chameleon();

                //  Start agent...
                agent.StartAgent(args);

                Console.WriteLine("Agent is running (Press Ctrl-C to stop)");
                sWaitMutex = new AdkConsoleWait();
                sWaitMutex.WaitForExit();

                //  Always shutdown the agent on exit
                agent.Shutdown(ProvisioningFlags.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>  Initialize and start the agent
        /// </summary>
        /// <param name="args">Command-line arguments (run with no arguments to display help)
        /// </param>
        public virtual void StartAgent(string[] args)
        {
            Console.WriteLine("Initializing agent...");


            //  Read the configuration file
            fCfg = new AgentConfig();
            Console.Out.WriteLine("Reading configuration file...");
            fCfg.Read("agent.cfg", false);

            //  Override the SourceId passed to the constructor with the SourceId
            //  specified in the configuration file
            Id = fCfg.SourceId;

            //  Inform the ADK of the version of SIF specified in the sifVersion=
            //  attribute of the <agent> element
            SifVersion version = fCfg.Version;
            Adk.SifVersion = version;

            //  Now call the superclass initialize once the configuration file has been read
            base.Initialize();

            //  Ask the AgentConfig instance to "apply" all configuration settings
            //  to this Agent; for example, all <property> elements that are children
            //  of the root <agent> node are parsed and applied to this Agent's
            //  AgentProperties object; all <zone> elements are parsed and registered
            //  with the Agent's ZoneFactory, and so on.
            //
            fCfg.Apply(this, true);

            // Create the logging object
            fLogger = new ObjectLogger(this);

            // Now, connect to all zones and just get the zone status
            foreach (IZone zone in ZoneFactory.GetAllZones())
            {
                if (getChameleonProperty(zone, "logRaw", false))
                {
                    zone.Properties.KeepMessageContent = true;
                    zone.AddMessagingListener(fLogger);
                    // Set this class as the recipient of all SIF_ZoneStatus 
                    // query results
                    zone.SetQueryResults(this, InfraDTD.SIF_ZONESTATUS);
                }

                // Provision the logger class to log all QueryResults

                zone.Connect(ProvisioningFlags.Register);

            }

            // On a seperate thread, go through the exercise of getting the
            // SIF_ZoneStatus object from all zones
            AsyncUtils.QueueTaskToThreadPool(new SimpleMethod(GetZoneStatus));

        }

        /// <summary>
        /// Gets the SIF_ZoneStatus object 
        /// </summary>
        private void GetZoneStatus()
        {

            Console.WriteLine();
            Console.WriteLine("Requesting SIF_ZoneStatus from all zones...");
            // Determine if the synchronous SIF_GetZoneStatus method can be used
            Query zoneStatusQuery = new Query(InfraDTD.SIF_ZONESTATUS);
            zoneStatusQuery.AddFieldRestriction(InfraDTD.SIF_ZONESTATUS_SIF_PROVIDERS);
            zoneStatusQuery.UserData = fSessionId;

            foreach (IZone zone in ZoneFactory.GetAllZones())
            {
                if (zone.Properties.UseZoneStatusSystemControl)
                {
                    SIF_ZoneStatus zs = zone.GetZoneStatus();
                    _processSIF_ZoneStatus(zs, zone);
                }
                else
                {
                    // Just create a SIF_Request and it will be handled by the
                    // OnQueryResults method
                    zone.Query(zoneStatusQuery);
                }
            }

        }

        /// <summary>
        /// Overriden so that any event is logged by the logger
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override ISubscriber GetSubscriber(SifContext context, IElementDef objectType)
        {
            return fLogger;
        }

        /// <summary>
        /// Overriden so that any SIF_Response is logged by the logger
        /// </summary>
        /// <param name="context"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override IQueryResults GetQueryResults(SifContext context, IElementDef objectType)
        {
            if (objectType == InfraDTD.SIF_ZONESTATUS)
            {
                return this;
            }
            else
            {
                return fLogger;
            }
        }


        public void OnQueryPending(IMessageInfo info,
                                   IZone zone)
        {
            Adk.Log.Info
                (
                string.Format
                    ("Requested {0} from {1}", ((SifMessageInfo)info).SIFRequestObjectType.Name,
                     zone.ZoneId));
        }

        public bool getChameleonProperty(IZone zone,
                                         string propertyName,
                                         bool defaultValue)
        {
            bool retValue = true;
            try
            {
                retValue = zone.Properties.GetProperty("chameleon." + propertyName, defaultValue);
            }
            catch (Exception ex)
            {
                Log.Warn(ex.Message, ex);
            }
            return retValue;
        }

        public void OnQueryResults(IDataObjectInputStream data,
                                   SIF_Error error,
                                   IZone zone,
                                   IMessageInfo info)
        {
            SifMessageInfo smi = (SifMessageInfo)info;
            if (!(fSessionId.Equals(smi.SIFRequestInfo.UserData)))
            {
                // This is a SIF_ZoneStatus response from a previous invocation of the agent
                return;
            }
            if (data.Available)
            {
                SIF_ZoneStatus zoneStatus = data.ReadDataObject() as SIF_ZoneStatus;
                AsyncUtils.QueueTaskToThreadPool(new zsDelegate(_processSIF_ZoneStatus), zoneStatus, zone);
            }
        }

        private delegate void zsDelegate(SIF_ZoneStatus zoneStatus, IZone zone);

        private void _processSIF_ZoneStatus(SIF_ZoneStatus zoneStatus, IZone zone)
        {
            if (zoneStatus == null)
            {
                return;
            }

            bool sync = getChameleonProperty(zone, "sync", false);
            bool events = getChameleonProperty(zone, "logEvents", true);
            bool logEntry = getChameleonProperty(zone, "sifLogEntrySupport", false);
            
            fLogger.UseLogEntry = logEntry;

            string syncObjects = zone.Properties.GetProperty("chameleon.syncObjects");

            ArrayList objectDefs = new ArrayList();

            SIF_Providers providers = zoneStatus.SIF_Providers;
            if (providers != null)
            {
                foreach (SIF_Provider p in providers)
                {
                    if (p.SIF_ObjectList != null)
                    {
                        foreach (SIF_Object obj in p.SIF_ObjectList)
                        {
                            // Lookup the topic for each provided object in the zone
                            IElementDef def = Adk.Dtd.LookupElementDef(obj.ObjectName);
                            if (def != null)
                            {
                                // Pull out just the objects specified according to configuration settings 
                                // or all, if the syncObjects setting is not specified
                                if ( syncObjects == null || (syncObjects.Length > 0 && syncObjects.IndexOf( def.Name ) > -1) )
                                {
                                    objectDefs.Add( def );
                                    if ( events )
                                    {
                                        ITopic topic = TopicFactory.GetInstance( def );
                                        if ( topic.GetSubscriber() == null )
                                        {
                                            topic.SetSubscriber( fLogger, null );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (logEntry)
            {
                ITopic sifLogEntryTopic = TopicFactory.GetInstance(InfraDTD.SIF_LOGENTRY);
                sifLogEntryTopic.SetSubscriber(fLogger, null);
            }

            // Disconnect the zone before provisioning
            zone.Disconnect(ProvisioningFlags.None);

            foreach (ITopic topic in TopicFactory.GetAllTopics( SifContext.DEFAULT ))
            {
                try
                {
                    // Join the topic to each zone ( causes the agent to subscribe to the joined objects )
                    // TODO: Add an "isJoinedTo()" API to topic so that it doesn't throw an exception
                    if (topic.ObjectType != InfraDTD.SIF_ZONESTATUS.Name)
                    {
                        topic.Join(zone);
                    }
                }
                catch (Exception ex)
                {
                    zone.Log.Error(ex.Message, ex);
                }
            }

           
            zone.Connect(ProvisioningFlags.Register);

            if (sync)
            {
                if (objectDefs.Count == 0)
                {
                    zone.ServerLog.Log
                        (LogLevel.WARNING, "No objects are being provided in this zone", null,
                         "1001");
                }
                
                foreach (IElementDef def in objectDefs)
                {
                    if (def.IsSupported(Adk.SifVersion))
                    {
                      
                            Query q = new Query(def);

                            // Query by specific parameters
                            string condition =
                                zone.Properties.GetProperty
                                    ("chameleon.syncConditions." + def.Name);
                            if (condition != null && condition.Length > 0)
                            {
                                // The condition should be in the format "path=value" e.g "@RefId=123412341...1234|@Name=asdfasdf"
                                String[] queryConditions = condition.Split('|');
                                foreach (String cond in queryConditions)
                                {
                                    string[] conds = cond.Split('=');
                                    if (conds.Length == 2)
                                    {
                                        q.AddCondition(conds[0], "EQ", conds[1]);
                                    }
                                }
                            }

                            if (logEntry)
                            {
                                zone.ServerLog.Log
                                    (LogLevel.INFO,
                                     "Requesting " + q.ObjectType.Name + " from the zone",
                                     q.ToXml(Adk.SifVersion), "1002");
                            }

                            zone.Query(q);
                        
                    }
                    else
                    {
                        String debug = "Will not request " + def.Name +
                                       " because it is not supported in " +
                                       Adk.SifVersion.ToString();
                        Console.WriteLine(debug);
                        zone.ServerLog.Log(LogLevel.WARNING, debug, null, "1001");
                    }
                }
            }
        }
    }
}
