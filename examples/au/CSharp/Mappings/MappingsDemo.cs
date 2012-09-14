//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using OpenADK.Library;
using OpenADK.Library.Infra;
using OpenADK.Library.au.Student;
using OpenADK.Library.Tools.Cfg;
using OpenADK.Library.Tools.Mapping;
using OpenADK.Util;
using OpenADK.Library.au;

/// <summary>  The Mappings agent demonstrates how to respond to requests for LearnerPersonal
/// by reading a list of students from a Microsoft Access database using JDBC,
/// then using the ADK's Mappings class to convert the field data into
/// LearnerPersonal objects. The mappings can be changed in the agent.cfg
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
/// To register the agent as the provider of LearnerPersonal objects in one or
/// more zones, open the agent.cfg file and provide zone connection parameters.
/// Then run the agent to register in each zone as the provider of LearnerPersonal.
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
public class MappingsDemo : Agent, IPublisher, IQueryResults
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
        : base( "MappingsDemo" )
    {
        Name = "Library ADK Example";
    }

    /// <summary>  Initialize and start the agent
    /// </summary>
    /// <param name="args">Command-line arguments (run with no arguments to display help)
    /// 
    /// </param>
    public virtual void startAgent( string[] args )
    {
        Console.WriteLine( "Initializing agent..." );
        Adk.Initialize( SifVersion.LATEST, SIFVariant.SIF_AU, (int)SdoLibraryType.Student );

        //  Read the configuration file
        fCfg = new AgentConfig();
        Console.WriteLine( "Reading configuration file..." );
        fCfg.Read( "agent.cfg", false );

        //  Override the SourceId passed to the constructor with the SourceId
        //  specified in the configuration file
        Id = fCfg.SourceId;

        //  Inform the ADK of the version of SIF specified in the sifVersion=
        //  attribute of the <agent> element
        SifVersion version = fCfg.Version;
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
        fCfg.Apply( this, true );

        //  Establish the ODBC connection to the Students.mdb database file.
        //  The JDBC driver and URL are specified in the agent.cfg configuration
        //  file and were automatically added to the AgentProperties when the
        //  apply method was called above.
        //
        Console.WriteLine( "Opening database..." );

        AgentProperties props = Properties;
        string driver = props.GetProperty( "Connection" );
        string url = props.GetProperty( "ConnectionString" );
        Console.WriteLine( "- Using driver: " + driver );
        Console.WriteLine( "- Connecting to URL: " + url );

        //  Load the DataDriver driver

        //  Get a Connection

        fConn = new OleDbConnection();
        fConn.ConnectionString = url;

        //  Connect to each zone specified in the configuration file, registering
        //  this agent as the Provider of Learner objects. Once connected,
        //  send a request for LearnerPersonal.
        //
        Console.WriteLine( "Connecting to zones and requesting LearnerPersonal objects..." );
        IZone[] allZones = ZoneFactory.GetAllZones();
        for ( int i = 0; i < allZones.Length; i++ )
        {
            try
            {
                //  Connect to this zone
                Console.WriteLine
                    ( "- Connecting to zone \"" + allZones[i].ZoneId + "\" at " +
                      allZones[i].ZoneUrl );
                allZones[i].SetPublisher
                    ( this, StudentDTD.STUDENTPERSONAL, new PublishingOptions() );
                allZones[i].SetQueryResults( this );
                allZones[i].Connect( ProvisioningFlags.Register );

                //  Request all students
                Query q = new Query( StudentDTD.STUDENTPERSONAL );
                q.UserData = "Mappings Demo";
                allZones[i].Query( q );
            }
            catch ( AdkException ex )
            {
                Console.WriteLine( "  " + ex.Message );
            }
        }
    }

    /// <summary>  Respond to SIF Requests
    /// </summary>
    public virtual void OnRequest( IDataObjectOutputStream outStream,
                                   Query query,
                                   IZone zone,
                                   IMessageInfo inf )
    {
        SifMessageInfo info = (SifMessageInfo) inf;
        SifWriter debug = new SifWriter( Console.Out );

        Console.WriteLine
            ( "Received a request for " + query.ObjectTag + " from agent \"" + info.SourceId +
              "\" in zone " + zone.ZoneId );

        // Tell the ADK to automatically filter out any objects that don't meet the requirements
        // of the query conditions
        outStream.Filter = query;

        //  Read all learners from the database to populate a HashMap of
        //  field/value pairs. The field names can be whatever we choose as long
        //  as they match the field names used in the <mappings> section of the
        //  agent.cfg configuration file. Each time a record is read, convert it
        //  to a LearnerPersonal object using the Mappings class and stream it to
        //  the supplied output stream.
        //
        IDictionary data = new Hashtable();
        IDbCommand command = null;

        Console.WriteLine( "The SIF Request was requested in the following SIF Versions" );
        foreach ( SifVersion version in info.SIFRequestVersions )
        {
            Console.WriteLine( "    - " + version );
        }

        Console.WriteLine( "This agent will respond in its latest supported version, which is: " );
        Console.WriteLine( "    - " + info.LatestSIFRequestVersion );

        //  Get the root Mappings object from the configuration file
        Mappings m =
            fCfg.Mappings.GetMappings( "Default" ).Select
                ( info.SourceId, zone.ZoneId, info.LatestSIFRequestVersion );

        //  Ask the root Mappings instance to select a Mappings from its
        //  hierarchy. For example, you might have customized the agent.cfg
        //  file with mappings specific to zones, versions of SIF, or
        //  requesting agents. The Mappings.selectOutbound() method will select
        //  the most appropriate instance from the hierarchy given the
        //  three parameters passed to it.
        MappingsContext mappings = m.SelectOutbound( StudentDTD.STUDENTPERSONAL, info );

        try
        {
            int count = 0;

            //  Query the database for all students
            command = fConn.CreateCommand();
            fConn.Open();
            command.CommandText = "SELECT * FROM Students";
            using ( IDataReader rs = command.ExecuteReader( CommandBehavior.CloseConnection ) )
            {
                DataReaderAdaptor dra = new DataReaderAdaptor( rs );
                while ( rs.Read() )
                {
                    count++;
                    //  Finally, create a new LearnerPersonal object and ask the
                    //  Mappings to populate it with SIF elements from the HashMap
                    //  of field/value pairs. As long as there is an <object>/<field>
                    //  definition for each entry in the HashMap, the ADK will take
                    //  care of producing the appropriate SIF element/attribute in
                    //  the LearnerPersonal object.
                    //
                    StudentPersonal sp = new StudentPersonal();
                    sp.RefId = Adk.MakeGuid();
                    mappings.Map( sp, dra );

                    //  Now write out the LearnerPersonal to the output stream and
                    //  we're done publishing this student.
                    //
                    Console.WriteLine( "\nThe agent has read these values from the database:" );
                    DumpDictionaryToConsole( data );
                    Console.WriteLine( "To produce this LearnerPersonal object:" );
                    debug.Write( sp );
                    debug.Flush();

                    outStream.Write( sp );
                    data.Clear();
                }

                rs.Close();
            }

            Console.WriteLine
                ( "- Returned " + count + " records from the Student database in response" );
        }
        catch ( Exception ex )
        {
            Console.WriteLine( "- Returning a SIF_Error response: " + ex.ToString() );
            throw new SifException
                ( SifErrorCategoryCode.RequestResponse, SifErrorCodes.REQRSP_GENERIC_ERROR_1,
                  "An error occurred while querying the database for students", ex.ToString(), zone );
        }
        finally
        {
            if ( command != null )
            {
                try
                {
                    fConn.Close();
                }
                catch ( Exception ignored )
                {
                }
            }
        }
    }

    private void DumpDictionaryToConsole( IDictionary dictionary )
    {
        foreach ( string key in dictionary.Keys )
        {
            Console.WriteLine( " [{0}] = \"{1}\"", key, dictionary[key] );
        }
    }

    /// <summary>  Called by the ADK when a SIF_Request is successfully sent to a zone
    /// </summary>
    public virtual void OnQueryPending( IMessageInfo info,
                                        IZone zone )
    {
        //  Nothing to do.
    }

    /// <summary>  Handles SIF_Responses
    /// </summary>
    public virtual void OnQueryResults( IDataObjectInputStream inStream,
                                        SIF_Error error,
                                        IZone zone,
                                        IMessageInfo inf )
    {
        SifMessageInfo info = (SifMessageInfo) inf;

        Console.WriteLine
            ( "\nReceived a query response from agent \"" + info.SourceId + "\" in zone " +
              zone.ZoneId );
        IRequestInfo reqInfo = info.SIFRequestInfo;
        if ( reqInfo != null )
        {
            Console.WriteLine
                ( "\nResponse was received in {0}. Object State is '{1}'",
                  DateTime.Now.Subtract( reqInfo.RequestTime ), reqInfo.UserData );
        }

        //
        //  Use the Mappings class to translate the LearnerPersonal objects received
        //  from the zone into a HashMap of field/value pairs, then dump the table
        //  to System.out
        //

        //  Always check for an error response
        if ( error != null )
        {
            Console.WriteLine
                ( "The request for LearnerPersonal failed with an error from the provider:" );
            Console.WriteLine
                ( "  [Category=" + error.SIF_Category + "; Code=" + error.SIF_Code + "]: " +
                  error.SIF_Desc +
                  ". " + error.SIF_ExtendedDesc );
            return;
        }

        //  Get the root Mappings object from the configuration file
        Mappings m = fCfg.Mappings.GetMappings( "Default" );

        //  Ask the root Mappings instance to select a Mappings from its
        //  hierarchy. For example, you might have customized the agent.cfg
        //  file with mappings specific to zones, versions of SIF, or
        //  requesting agents. The Mappings.select() method will select
        //  the most appropriate instance from the hierarchy given the
        //  three parameters passed to it.
        MappingsContext mappings = m.SelectInbound( StudentDTD.STUDENTPERSONAL, info );
        Hashtable data = new Hashtable();
        StringMapAdaptor sma = new StringMapAdaptor( data, Adk.Dtd.GetFormatter( SifVersion.LATEST ) );

        int count = 0;
        while ( inStream.Available )
        {
            Console.WriteLine( "Object Number {0}", count++ );
            StudentPersonal sp = (StudentPersonal) inStream.ReadDataObject();
            //  Ask the Mappings object to populate the dictionary with field/value pairs
            //  by using the mapping rules in the configuration file to decompose
            //  the LearnerPersonal object into field values.
            mappings.Map( sp, sma );
            //  Now dump the field/value pairs to System.out
            DumpDictionaryToConsole( data );
            Console.WriteLine();
            data.Clear();
        }
    }


    public override void Shutdown( ProvisioningFlags provisioningOptions )
    {
        if ( fConn != null && fConn.State != ConnectionState.Closed )
        {
            fConn.Close();
        }
        base.Shutdown( provisioningOptions );
    }

    /// <summary>  Runs the agent from the command-line.
    /// </summary>
    [STAThread]
    public static void Main( string[] args )
    {
        MappingsDemo agent = null;
        try
        {
            //  Pre-parse the command line before initializing the ADK
            Adk.Debug = AdkDebugFlags.Moderate;

            //  Start agent...
            agent = new MappingsDemo();

            agent.startAgent( args );

            //  Wait for Ctrl-C to be pressed
            Console.WriteLine();
            Console.WriteLine( "Agent is running (Press Ctrl-C to stop)" );
            Console.WriteLine();

            //  Install a shutdown hook to cleanup when Ctrl+C is pressed
            new AdkConsoleWait().WaitForExit();
            agent.Shutdown( ProvisioningFlags.None );
        }
        catch ( Exception e )
        {
            Console.WriteLine( e );
        }
    }
}
