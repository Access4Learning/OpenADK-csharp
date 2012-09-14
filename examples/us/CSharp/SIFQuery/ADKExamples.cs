//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using OpenADK.Library;
using OpenADK.Util;

namespace OpenADK.Examples
{

    /// <summary>  A convenience class to parse the command-line of all Adk Example agents
    /// and to read a list of zones from a zones.properties file, if present in
    /// the current working directory.<p>
    /// *
    /// </summary>
    /// <version>  Adk 1.0
    /// 
    /// </version>
    public class AdkExamples
    {
        /// <summary>  False if the /noreg option was specified, indicating the agent should
        /// not send a SIF_Register message when connecting to zones
        /// </summary>
        public static bool Reg = true;

        /// <summary>  True if the /unreg option was specified, indicating the agent should
        /// send a SIF_Unregister when shutting down and disconnecting from zones
        /// </summary>
        public static bool Unreg = false;

        /// <summary>  The SIFVersion specified on the command-line
        /// </summary>
        public static SifVersion Version;

        /// <summary>  Parsed command-line arguments
        /// </summary>
        private static string[] sArguments = null;

        /// <summary>  Parse the command-line. This method may be called repeatedly, usually
        /// once from the sample agent's <c>main</code> function prior to
        /// initializing the Adk and again from the <c>Agent.initialize</code>
        /// method after initializing the Agent superclass. When called without an
        /// Agent instance, only those options that do not rely on an AgentProperties
        /// object are processed (e.g. the /D option).
        /// <p>
        /// *
        /// If a file named 'agent.rsp' exists in the current directory, any command
        /// line options specified will be appended to the command-line arguments
        /// passed to this method. Each line of the agent.rsp text file may be
        /// comprised of one or more arguments separated by spaces, so that the
        /// entirely set of arguments can be on one line or broken up onto many
        /// lines.<p>
        /// *
        /// </summary>
        /// <param name="agent">An Agent instance that will be updated when certain
        /// command-line options are parsed
        /// </param>
        /// <param name="arguments">The string of arguments provided by the <c>main</code>
        /// function
        /// 
        /// </param>
        public static NameValueCollection parseCL( Agent agent,
                                                   string[] arguments )
        {
            if( sArguments == null )
            {
                sArguments = ReadArgsFromResponseFile( arguments );
            }

            if( agent == null )
            {
                ParseGlobalProperties();
                return null;
            }
            else
            {
                return ParseAgentProperties( agent );
            }
        }

        private static NameValueCollection ParseAgentProperties( Agent agent )
        {
            //  Parse all other options...
            AgentProperties props = agent.Properties;
            NameValueCollection misc = new NameValueCollection();

            int port = -1;
            string host = null;
            bool useHttps = false;
            string sslCert = null;
            string clientCert = null;
            int clientAuth = 0;

            for( int i = 0; i < sArguments.Length; i++ )
            {
                if( sArguments[ i ].ToUpper().Equals( "/sourceId".ToUpper() ) &&
                     i != sArguments.Length - 1 )
                {
                    agent.Id = sArguments[ ++i ];
                }
                else if( sArguments[ i ].ToUpper().Equals( "/noreg".ToUpper() ) )
                {
                    Reg = false;
                }
                else if( sArguments[ i ].ToUpper().Equals( "/unreg".ToUpper() ) )
                {
                    Unreg = true;
                }
                else if( sArguments[ i ].ToUpper().Equals( "/pull".ToUpper() ) )
                {
                    props.MessagingMode = AgentMessagingMode.Pull;
                }
                else if( sArguments[ i ].ToUpper().Equals( "/push".ToUpper() ) )
                {
                    props.MessagingMode = AgentMessagingMode.Push;
                }
                else if( sArguments[ i ].ToUpper().Equals( "/port".ToUpper() ) &&
                          i != sArguments.Length - 1 )
                {
                    try
                    {
                        port = Int32.Parse( sArguments[ ++i ] );
                    }
                    catch( FormatException )
                    {
                        Console.WriteLine( "Invalid port: " + sArguments[ i - 1 ] );
                    }
                }
                else if( sArguments[ i ].ToUpper().Equals( "/https".ToUpper() ) )
                {
                    useHttps = true;
                }
                else if( sArguments[ i ].ToUpper().Equals( "/sslCert".ToUpper() ) )
                {
                    sslCert = sArguments[ ++i ];
                }
                else if( sArguments[ i ].ToUpper().Equals( "/clientCert".ToUpper() ) )
                {
                    clientCert = sArguments[ ++i ];
                }
                else if( sArguments[ i ].ToUpper().Equals( "/clientAuth".ToUpper() ) )
                {
                    try
                    {
                        clientAuth = int.Parse( sArguments[ ++i ] );
                    }
                    catch( FormatException )
                    {
                        clientAuth = 0;
                    }
                }
                else if( sArguments[ i ].ToUpper().Equals( "/host".ToUpper() ) &&
                          i != sArguments.Length - 1 )
                {
                    host = sArguments[ ++i ];
                }
                else if( sArguments[ i ].ToUpper().Equals( "/timeout".ToUpper() ) &&
                          i != sArguments.Length - 1 )
                {
                    try
                    {
                        props.DefaultTimeout =
                            TimeSpan.FromMilliseconds( Int32.Parse( sArguments[ ++i ] ) );
                    }
                    catch( FormatException )
                    {
                        Console.WriteLine( "Invalid timeout: " + sArguments[ i - 1 ] );
                    }
                }
                else if( sArguments[ i ].ToUpper().Equals( "/freq".ToUpper() ) &&
                          i != sArguments.Length - 1 )
                {
                    try
                    {
                        props.PullFrequency =
                            TimeSpan.FromMilliseconds( int.Parse( sArguments[ ++i ] ) );
                    }
                    catch( FormatException )
                    {
                        Console.WriteLine
                            ( "Invalid pull frequency: " + sArguments[ i - 1 ] );
                    }
                }
                else if( sArguments[ i ].ToUpper().Equals( "/opensif".ToUpper() ) )
                {
                    //  OpenSIF reports attempts to re-subscribe to objects as an
                    //  error instead of a success status code. The Adk would therefore
                    //  throw an exception if it encountered the error, so we can
                    //  disable that behavior here.

                    props.IgnoreProvisioningErrors = true;
                }
                else if( sArguments[ i ][ 0 ] == '/' )
                {
                    if( i == sArguments.Length - 1 ||
                         sArguments[ i + 1 ].StartsWith( "/" ) )
                    {
                        misc[ sArguments[ i ].Substring( 1 ) ] = null;
                    }
                    else
                    {
                        misc[ sArguments[ i ].Substring( 1 ) ] = sArguments[ ++i ];
                    }
                }
            }

            if( useHttps )
            {
                //  Set transport properties (HTTPS)
                HttpsProperties https = agent.DefaultHttpsProperties;
                if( sslCert != null )
                {
                    https.SSLCertName = sslCert;
                }
                if( clientCert != null )
                {
                    https.ClientCertName = clientCert;
                }

                https.ClientAuthLevel = clientAuth;

                if( port != -1 )
                {
                    https.Port = port;
                }
                https.Host = host;

                props.TransportProtocol = "https";
            }
            else
            {
                //  Set transport properties (HTTP)
                HttpProperties http = agent.DefaultHttpProperties;
                if( port != -1 )
                {
                    http.Port = port;
                }
                http.Host = host;

                props.TransportProtocol = "http";
            }

            return misc;
        }

        private static void ParseGlobalProperties()
        {
            //  Look for options that do not affect the AgentProperties...
            for( int i = 0; i < sArguments.Length; i++ )
            {
                if( sArguments[ i ].ToUpper().Equals( "/debug".ToUpper() ) )
                {
                    if( i < sArguments.Length - 1 )
                    {
                        try
                        {
                            Adk.Debug = AdkDebugFlags.None;
                            int k = Int32.Parse( sArguments[ ++i ] );
                            if( k == 1 )
                            {
                                Adk.Debug = AdkDebugFlags.Minimal;
                            }
                            else if( k == 2 )
                            {
                                Adk.Debug = AdkDebugFlags.Moderate;
                            }
                            else if( k == 3 )
                            {
                                Adk.Debug = AdkDebugFlags.Detailed;
                            }
                            else if( k == 4 )
                            {
                                Adk.Debug = AdkDebugFlags.Very_Detailed;
                            }
                            else if( k == 5 )
                            {
                                Adk.Debug = AdkDebugFlags.All;
                            }
                        }
                        catch( Exception )
                        {
                            Adk.Debug = AdkDebugFlags.All;
                        }
                    }
                    else
                    {
                        Adk.Debug = AdkDebugFlags.All;
                    }
                }
                else if( sArguments[ i ].StartsWith( "/D" ) )
                {
                    string prop = sArguments[ i ].Substring( 2 );
                    if( i != sArguments.Length - 1 )
                    {
                        Properties.SetProperty( prop, sArguments[ ++i ] );
                    }
                    else
                    {
                        Console.WriteLine( "Usage: /Dproperty value" );
                    }
                }
                else if( sArguments[ i ].ToUpper().Equals( "/log".ToUpper() ) &&
                          i != sArguments.Length - 1 )
                {
                    try
                    {
                        Adk.SetLogFile( sArguments[ ++i ] );
                    }
                    catch( IOException ioe )
                    {
                        Console.WriteLine( "Could not redirect debug output to log file: " + ioe );
                    }
                }
                else if( sArguments[ i ].ToUpper().Equals( "/ver".ToUpper() ) &&
                          i != sArguments.Length - 1 )
                {
                    Version = SifVersion.Parse( sArguments[ ++i ] );
                }
                else if( sArguments[ i ].Equals( "/?" ) )
                {
                    Console.WriteLine();
                    Console.WriteLine
                        ( "These options are common to all Adk Example agents. For help on the usage" );
                    Console.WriteLine
                        ( "of this agent in particular, run the agent without any parameters. Note that" );
                    Console.WriteLine
                        ( "most agents support multiple zones if a zones.properties file is found in" );
                    Console.WriteLine( "the current directory." );
                    Console.WriteLine();
                    printHelp();

                    Environment.Exit( 0 );
                }
            }
        }

        private static string[] ReadArgsFromResponseFile( string[] arguments )
        {
            if( arguments.Length > 0 && arguments[ 0 ][ 0 ] != '/' )
            {
                //  Look for an agent.rsp response file
                FileInfo rsp = new FileInfo( arguments[ 0 ] );
                if( rsp.Exists )
                {
                    try
                    {
                        ArrayList v = new ArrayList();
                        using( StreamReader reader = File.OpenText( rsp.FullName ) )
                        {
                            string line;
                            while( ( line = reader.ReadLine() ) != null )
                            {
                                // allow comment lines, starting with a ;
                                if( !line.StartsWith( ";" ) )
                                {
                                    foreach( string token in line.Split( ' ' ) )
                                    {
                                        v.Add( token );
                                    }
                                }
                            }
                            reader.Close();
                        }

                        //  Append any arguments found to the args array
                        if( v.Count > 0 )
                        {
                            string[] args = new string[ arguments.Length + v.Count ];
                            Array.Copy( arguments, 0, arguments, 0, arguments.Length );
                            v.CopyTo( arguments, arguments.Length );
                            Console.Out.Write
                                ( "Reading command-line arguments from " + arguments[ 0 ] + ": " );
                            for( int i = 0; i < args.Length; i++ )
                            {
                                Console.Out.Write( args[ i ] + " " );
                            }
                            Console.WriteLine();
                            Console.WriteLine();

                            return args;
                        }
                    }
                    catch( Exception ex )
                    {
                        Console.WriteLine
                            ( "Error reading command-line arguments from agent.rsp file: " + ex );
                    }
                }
            }
            return arguments;
        }

        /// <summary>  Display help to System.out
        /// </summary>
        public static void printHelp()
        {
            Console.WriteLine( "    /sourceId name    The name of the agent" );
            Console.WriteLine
                ( "    /ver version      Default SIF Version to use (e.g. 10r1, 10r2, etc.)" );
            Console.WriteLine( "    /debug level      Enable debugging to the console" );
            Console.WriteLine( "                         1 - Minimal" );
            Console.WriteLine( "                         2 - Moderate" );
            Console.WriteLine( "                         3 - Detailed" );
            Console.WriteLine( "                         4 - Very Detailed" );
            Console.WriteLine( "                         5 - All" );
            Console.WriteLine( "    /log file         Redirects logging to the specified file" );
            Console.WriteLine( "    /pull             Use Pull mode" );
            Console.WriteLine
                ( "    /freq             Sets the Pull frequency (defaults to 15 seconds)" );
            Console.WriteLine( "    /push             Use Push mode" );
            Console.WriteLine
                ( "    /port n           The local port for Push mode (defaults to 12000)" );
            Console.WriteLine
                ( "    /host addr        The local IP address for push mode (defaults to any)" );
            Console.WriteLine
                ( "    /noreg            Do not send a SIF_Register on startup (sent by default)" );
            Console.WriteLine
                ( "    /unreg            Send a SIF_Unregister on exit (not sent by default)" );
            Console.WriteLine( "    /timeout ms       Sets the Adk timeout period (defaults to 30000)" );
            Console.WriteLine( "    /opensif          Ignores provisioning errors from OpenSIF" );
            Console.WriteLine( "    /Dproperty val    Sets a Java System property" );
            Console.WriteLine();
            Console.WriteLine( "  HTTPS Transport Options:" );
            Console.WriteLine
                ( "  Certificates will be retrieved from the Current User's Personal Store" );
            Console.WriteLine( "    /https				Use HTTPS instead of HTTP." );
            Console.WriteLine( "    /clientAuth [1|2|3]	Require Client Authentication level" );
            Console.WriteLine( "    /sslCert cert	    The subject of the certificate to use for SSL" );
            Console.WriteLine
                ( "    /clientCert			The subject of the certificate to use for Client Authentication" );
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine( "  Response Files:" );
            Console.WriteLine
                ( "    To use a response file instead of typing arguments on the command-line," );
            Console.WriteLine
                ( "    pass the name of the response file as the first argument. This text" );
            Console.WriteLine
                ( "    file may contain any combination of arguments on one or more lines," );
            Console.WriteLine
                ( "    which are appended to any arguments specified on the command-line." );
        }

        /// <summary>  Looks for a file named zones.properties and if found reads its contents
        /// into a HashMap where the key of each entry is the Zone ID and the value
        /// is the Zone URL. A valid HashMap is always returned by this method; the
        /// called typically assigns the value of the /zone and /url command-line
        /// options to that map (when applicable).
        /// </summary>
        public static NameValueCollection ReadZonesList()
        {
            NameValueCollection list = new NameValueCollection();
            return list;
        }

        static AdkExamples()
        {
            Version = SifVersion.LATEST;
        }
    }
}
