//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Data;
using Edustructures.SifWorks;
using Edustructures.SifWorks.us;
using Edustructures.SifWorks.Infra;
using Edustructures.SifWorks.us.Student;
using Edustructures.SifWorks.Tools.Cfg;
using Edustructures.SifWorks.Tools.Mapping;
using Edustructures.Util;
using System.Collections.Generic;

namespace SifWorks.Examples.Mapping
{
    /// <summary>  The Mappings agent demonstrates how to respond to requests for StudentPersonal
    /// by reading a list of students from a Microsoft Access database using JDBC,
    /// then using the ADK's Mappings class to convert the field data into
    /// StudentPersonal objects. The mappings can be changed in the agent.cfg
    /// configuration file, which is read by the program at startup using the ADK's
    /// AgentConfig class.
    /// <p>
    /// *
    /// Before this agent can be used an ODBC data source must be created. On the
    /// Windows platform, open the ODBC Control Panel and create a new System DSN
    /// with the name "MappingsDemo". This DSN should point to the Microsoft Access
    /// database (Students.mdb) in this agent's directory. The JDBC-ODBC bridge is
    /// then used to read data from that data source.
    /// <p>
    /// *
    /// To register the agent as the provider of StudentPersonal objects in one or
    /// more zones, open the agent.cfg file and provide zone connection parameters.
    /// Then run the agent to register in each zone as the provider of StudentPersonal.
    /// Unlike the other ADK Example agents, this one reads all zone information and
    /// connection settings from the agent.cfg file to demonstrate the use of the
    /// AgentConfig class.
    /// <p>
    /// *
    /// </summary>
    /// <author> Data Solutions
    /// </author>
    /// <version>  ADK 1.0
    /// 
    /// </version>
    public class MappingsDemo : Agent, IPublisher, ISubscriber, IQueryResults
    {
        /// <summary>The configuration file 
        /// </summary>
        internal AgentConfig fCfg;

        /// <summary>The SQL connection 
        /// </summary>
        internal IDbConnection fConn;

        /// <summary>  Constructor
        /// </summary>
        public MappingsDemo()
            : base("MappingsDemo") { }

        /// <summary>  Initialize and start the agent
        /// </summary>
        /// <param name="args">Command-line arguments (run with no arguments to display help)
        /// 
        /// </param>
        public virtual void startAgent(string[] args)
        {
            Name = "SIFWorks ADK Example";
            Console.WriteLine("Initializing agent...");

            //  Read the configuration file
            fCfg = new AgentConfig();
            Console.WriteLine("Reading configuration file...");
            fCfg.Read("agent.cfg", false);

            //  Override the SourceId passed to the constructor with the SourceId
            //  specified in the configuration file
            Id = fCfg.SourceId;

            //  Inform the ADK of the version of SIF specified in the sifVersion=
            //  attribute of the <agent> element
            SifVersion version = fCfg.Version;
            //SifVersion version = SifVersion.SIF15r1;
            Adk.SifVersion = version;

            //  Now call the superclass initialize once the configuration file has been read
            base.Initialize();

            //
            //  Ask the AgentConfig instance to "apply" all configuration settings
            //  to this Agent; for example, all <property> elements that are children
            //  of the root <agent> node are parsed and applied to this Agent's
            //  AgentProperties object; all <zone> elements are parsed and registered
            //  with the Agent's ZoneFactory, and so on.
            //
            fCfg.Apply(this, true);

            //  Establish the ODBC connection to the Students.mdb database file.
            //  The JDBC driver and URL are specified in the agent.cfg configuration
            //  file and were automatically added to the AgentProperties when the
            //  apply method was called above.
            //
            Console.WriteLine("Opening database...");

            AgentProperties props = Properties;
            string driver = props.GetProperty("Connection");
            string url = props.GetProperty("ConnectionString");
            Console.WriteLine("- Using driver: " + driver);
            Console.WriteLine("- Connecting to URL: " + url);

            //  Load the DataDriver driver

            //  Get a Connection

            fConn = (IDbConnection)ClassFactory.CreateInstance(driver);
            fConn.ConnectionString = url;

            //  Connect to each zone specified in the configuration file, registering
            //  this agent as the Provider of StudentPersonal objects. Once connected,
            //  send a request for StudentPersonal.
            //
            Console.WriteLine("Connecting to zones and requesting StudentPersonal objects...");
            IZone[] allZones = ZoneFactory.GetAllZones();
            for (int i = 0; i < allZones.Length; i++) {
                try {
                    //  Connect to this zone
                    Console.WriteLine
                        ("- Connecting to zone \"" + allZones[i].ZoneId + "\" at " +
                          allZones[i].ZoneUrl);
                    allZones[i].SetPublisher(this, StudentDTD.STUDENTPERSONAL, null);
                    allZones[i].SetQueryResults(this);
                    allZones[i].Connect(ProvisioningFlags.Register);



                    //  Request all students
                    Query q = new Query(StudentDTD.STUDENTPERSONAL);
                    q.UserData = "Mappings Demo";
                    allZones[i].Query(q);
                } catch (AdkException ex) {
                    Console.WriteLine("  " + ex.Message);
                }
            }
        }

        /// <summary>  Respond to SIF RequestsGetTopicMap
        /// </summary>
        public virtual void OnRequest(IDataObjectOutputStream outStream,
                                       Query query,
                                       IZone zone,
                                       IMessageInfo inf)
        {
            SifMessageInfo info = (SifMessageInfo)inf;
            SifWriter debug = new SifWriter(Console.Out);

            Console.WriteLine
                ("Received a request for " + query.ObjectTag + " from agent \"" + info.SourceId +
                  "\" in zone " + zone.ZoneId);

            //  Read all students from the database to populate a HashMap of
            //  field/value pairs. The field names can be whatever we choose as long
            //  as they match the field names used in the <mappings> section of the
            //  agent.cfg configuration file. Each time a record is read, convert it
            //  to a StudentPersonal object using the Mappings class and stream it to
            //  the supplied output stream.
            //
            IDbCommand command = null;

            // Set a basic filter on the outgoing data stream
            // What will happen is that any object written to the output stream will
            // be evaluated based on the query conditions. If the object doesn't meet the
            // query conditions, it will be excluded
            outStream.Filter = query;

            //  Get the root Mappings object from the configuration file
            Edustructures.SifWorks.Tools.Mapping.Mappings m = fCfg.Mappings.GetMappings("Default");

            //  Ask the root Mappings instance to select a Mappings from its
            //  hierarchy. For example, you might have customized the agent.cfg
            //  file with mappings specific to zones, versions of SIF, or
            //  requesting agents. The Mappings.select() method will select
            //  the most appropriate instance from the hierarchy given the
            //  three parameters passed to it.
            //
            //IDictionary<string, string> dataMap = new System.Collections.Generic.Dictionary<string, string>();
            // IFieldAdaptor adaptor = new StringMapAdaptor(dataMap);
            //m.MapOutbound();
            MappingsContext mc = m.SelectOutbound(StudentDTD.STUDENTPERSONAL, info);

            try {
                int count = 0;

                //  Query the database for all students
                command = fConn.CreateCommand();
                fConn.Open();
                command.CommandText = "SELECT * FROM Students";
                using (IDataReader rs = command.ExecuteReader(CommandBehavior.CloseConnection)) {

                    DataReaderAdaptor dra = new DataReaderAdaptor(rs);

                    while (rs.Read()) {

                        //  Finally, create a new StudentPersonal object and ask the
                        //  Mappings to populate it with SIF elements from the HashMap
                        //  of field/value pairs. As long as there is an <object>/<field>
                        //  definition for each entry in the HashMap, the ADK will take
                        //  care of producing the appropriate SIF element/attribute in
                        //  the StudentPersonal object.
                        //
                        StudentPersonal sp = new StudentPersonal();
                        sp.RefId = Adk.MakeGuid();
                        // TODO: When using custom macros for outboud mapping operations, set the ValueBuilder.
                        // You will need to call SetValueBuilder() giving the MappingsContext a derived version 
                        // of DefaultValueBuilder that has the macro methods available in it.
                        mc.SetValueBuilder(new DataUtilMacro(dra));
                        mc.Map(sp, dra);

                        //  Now write out the StudentPersonal to the output stream and
                        //  we're done publishing this student.
                        //
                        Console.WriteLine("\nThe agent has read these values from the database:");

                        DumpFieldsToConsole(rs);
                        Console.WriteLine("To produce this StudentPersonal object:");
                        debug.Write(sp);
                        debug.Flush();

                        outStream.Write(sp);
                    }

                    rs.Close();
                }
                Console.WriteLine
                    ("- Returned " + count + " records from the Student database in response");
            } catch (Exception ex) {
                Console.WriteLine("- Returning a SIF_Error response: " + ex);
                throw new SifException
                    (SifErrorCategoryCode.RequestResponse, SifErrorCodes.REQRSP_GENERIC_ERROR_1,
                      "An error occurred while querying the database for students", ex.ToString(), zone);
            } finally {
                if (command != null) {
                    try {
                        fConn.Close();
                    } catch (Exception ignored) {
                        Log.Warn(ignored.Message, ignored);
                    }
                }
            }
        }

        private static void DumpDictionaryToConsole(IDictionary dictionary)
        {
            foreach (string key in dictionary.Keys) {
                Console.WriteLine(" [{0}] = \"{1}\"", key, dictionary[key]);
            }
        }

        private static void DumpFieldsToConsole(IDataReader reader)
        {
            DataTable table = reader.GetSchemaTable();
            object[] values = new object[table.Columns.Count];
            reader.GetValues(values);
            for (int a = 0; a < values.Length; a++) {
                if (values[a] != null) {
                    Console.WriteLine(" [{0}] = \"{1}\"", reader.GetName(a), values[a]);
                }
            }
        }

        /// <summary>  Called by the ADK when a SIF_Request is successfully sent to a zone
        /// </summary>
        public virtual void OnQueryPending(IMessageInfo info,
                                            IZone zone)
        {
            //  Nothing to do.
        }

        /// <summary>  Handles SIF_Responses
        /// </summary>
        public virtual void OnQueryResults(IDataObjectInputStream in_Renamed,
                                            SIF_Error error,
                                            IZone zone,
                                            IMessageInfo inf)
        {
            SifMessageInfo info = (SifMessageInfo)inf;

            Console.WriteLine
                ("\nReceived a query response from agent \"" + info.SourceId + "\" in zone " +
                  zone.ZoneId);
            IRequestInfo reqInfo = info.SIFRequestInfo;
            if (reqInfo != null) {
                Console.WriteLine
                    ("\nResponse was received in {0}. Object State is '{1}'",
                      DateTime.Now.Subtract(reqInfo.RequestTime), reqInfo.UserData);
            }

            //
            //  Use the Mappings class to translate the StudentPersonal objects received
            //  from the zone into a HashMap of field/value pairs, then dump the table
            //  to System.out
            //

            //  Always check for an error response
            if (error != null) {
                Console.WriteLine
                    ("The request for StudentPersonal failed with an error from the provider:");
                Console.WriteLine
                    ("  [Category=" + error.SIF_Category + "; Code=" + error.SIF_Code + "]: " +
                      error.SIF_Desc + ". " + error.SIF_ExtendedDesc);
                return;
            }

            //  Get the root Mappings object from the configuration file
            Edustructures.SifWorks.Tools.Mapping.Mappings m = fCfg.Mappings.GetMappings("Default");

            //  Ask the root Mappings instance to select a Mappings from its
            //  hierarchy. For example, you might have customized the agent.cfg
            //  file with mappings specific to zones, versions of SIF, or
            //  requesting agents. The Mappings.select() method will select
            //  the most appropriate instance from the hierarchy given the
            //  three parameters passed to it.
            //

            SifFormatter textFormatter = Adk.Dtd.GetFormatter(info.SifVersion);
            MappingsContext context =
                m.SelectInbound(StudentDTD.STUDENTPERSONAL, info);

            while (in_Renamed.Available) {
                StudentPersonal sp = (StudentPersonal)in_Renamed.ReadDataObject();

                //  Ask the Mappings object to populate a HashMap with field/value pairs
                //  by using the mapping rules in the configuration file to decompose
                //  the StudentPersonal object into field values.
                //
                Hashtable data = new Hashtable();
                StringMapAdaptor sma = new StringMapAdaptor(data, textFormatter);
                context.Map(sp, sma);

                //  Now dump the field/value pairs to System.out
                DumpDictionaryToConsole(data);
            }
        }

        /// <summary>  Runs the agent from the command-line.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            try {
                //  Pre-parse the command line before initializing the ADK
                Adk.Debug = AdkDebugFlags.Moderate;
                AdkExamples.parseCL(null, args);

                //  Initialize the ADK. Note even though this example uses raw XML,
                //  it is still required that the appropriate SDO libraries be loaded
                Adk.Initialize(AdkExamples.Version, SIFVariant.SIF_US, (int)SdoLibraryType.Student);

                //  Start agent...
                MappingsDemo agent;
                agent = new MappingsDemo();

                agent.startAgent(args);

                //  Wait for Ctrl-C to be pressed
                Console.WriteLine();
                Console.WriteLine("Agent is running (Press Ctrl-C to stop)");
                Console.WriteLine();

                //  Install a shutdown hook to cleanup when Ctrl+C is pressed
                new AdkConsoleWait().WaitForExit();
                agent.Shutdown
                    (AdkExamples.Unreg ? ProvisioningFlags.Unregister : ProvisioningFlags.None);
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        #region ISubscriber Members

        public void OnEvent(Event evnt, IZone zone, IMessageInfo info)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
