//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using OpenADK.Library;
using OpenADK.Library.Infra;
using OpenADK.Library.Tools.XPath;
using OpenADK.Util;
using OpenADK.Examples;

namespace SIFQuery 
{

 public class SIFQuery : Agent, IQueryResults {
     private static readonly SIFQuery _agent = new SIFQuery();
     private static IDictionary<string, ComparisonOperators> supportedComparisons = new Dictionary<string, ComparisonOperators>();

     public SIFQuery() : 
         base("SIFQuery") 
     {
         
     }

     private static void InitializeComparisonList()
     {
         lock (supportedComparisons) {
             if (supportedComparisons.Count == 0) {
                 supportedComparisons.Add("=", ComparisonOperators.EQ);
                 supportedComparisons.Add(">", ComparisonOperators.GT);
                 supportedComparisons.Add("<", ComparisonOperators.LT);
                 supportedComparisons.Add(">=", ComparisonOperators.GE);
                 supportedComparisons.Add("<=", ComparisonOperators.LE);
                 supportedComparisons.Add("!=", ComparisonOperators.NE);
             }
         }
     }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="args"></param>
     [STAThread]
     public static void Main(string[] args) {
        
        try {
            if( args.Length < 2 ) {
	            Console.WriteLine("Usage: SIFQuery /zone zone /url url [/events] [options]");
	            Console.WriteLine("    /zone zone     The name of the zone");
	            Console.WriteLine("    /url url       The zone URL");
	            AdkExamples.printHelp();
	            return;
            }
        	
            //	Pre-parse the command-line before initializing the ADK
            Adk.Debug = AdkDebugFlags.Moderate;
            AdkExamples.parseCL( null, args);
        	
            //  Initialize the ADK with the specified version, loading only the Student SDO package
            int sdoLibs;
            sdoLibs = (int)OpenADK.Library.us.SdoLibraryType.All;
            Adk.Initialize(SifVersion.SIF23,SIFVariant.SIF_US,sdoLibs);
            // Call StartAgent. 
            _agent.StartAgent(args);
        	
            // Turn down debugging
            Adk.Debug = AdkDebugFlags.None;
        	
            // Call runConsole() This method does not return until the agent shuts down
            _agent.RunConsole();
        	
            //	Wait for Ctrl-C to be pressed
            Console.WriteLine( "Agent is running (Press Ctrl-C to stop)" );
            new AdkConsoleWait().WaitForExit();
        	
        } catch(Exception e) {
            Console.WriteLine(e);
        } finally {
            if( _agent != null && _agent.Initialized ){
	            //  Always shutdown the agent on exit
	            try {
		            _agent.Shutdown( AdkExamples.Unreg ?  ProvisioningFlags.Unprovide : ProvisioningFlags.None );
	            }
	            catch( AdkException adkEx ){
		            Console.WriteLine( adkEx );
	            }
            }
						// set breakpoint here to prevent console from closing on errors
            Console.WriteLine("");
        }

	}

	private void StartAgent(String[] args)
	{
		this.Initialize();
        NameValueCollection parameters = AdkExamples.parseCL(this, args);
		
        string zoneId = parameters["zone"];
		string url = parameters["url"];
		
		if( zoneId == null || url == null ) {
			Console.WriteLine("The /zone and /url parameters are required");
			Environment.Exit(0);
		}

		// only for SIF_Register and versions in SIF_Request messages...
    // this.Properties.OverrideSifVersions = "2.3,2.*";
		// only for SIF_Message Version attribute in SIF_Request messages
    // this.Properties.OverrideSifMessageVersionForSifRequests = "2.3";
		
		// 1) Get an instance of the zone to connect to
		IZone zone = ZoneFactory.GetInstance(zoneId, url);
		zone.SetQueryResults( this );
		
		// 2) Connect to zones
		zone.Connect( AdkExamples.Reg ? ProvisioningFlags.Register : ProvisioningFlags.None );
		
		
	}
	
	private void RunConsole()
	{
		Console.WriteLine( "SIFQuery Command Line" );
        Version version = Assembly.GetExecutingAssembly().GetName().Version;
		Console.WriteLine( "Version " + version.ToString(3) );
		Console.WriteLine( "Copyright " + DateTime.Now.Year + ", Data Solutions" );
		
		PrintSQLHelp();
        Regex sqlPattern = new Regex("(?:select)(.*)(?:from)(.*)(?:where)(.*)$", RegexOptions.IgnoreCase);
		bool finished = false;
		while( !finished ){
			PrintPrompt();
            string query = Console.ReadLine().Trim();

			if( query.Length == 0 ){
				continue;
			}

			string lcaseQuery = query.ToLower();
            if( lcaseQuery.StartsWith("q")){
				finished = true;
				continue;
			}
			
			if( lcaseQuery.IndexOf("where") == -1 ){
				// The regular expression requires a where clause
				query = query + " where ";
			}
			
			Match results;
			try {
                results = sqlPattern.Match(query);
			} catch( Exception ex ){
				Console.WriteLine( "ERROR evaluating expression: " + ex );
				continue;
			}
            
			if(results.Captures.Count == 0 ){
				Console.WriteLine( "Unknown error evaluating expression."  );
				continue;
			}

			if( results.Groups.Count >= 3 ){
                Query q = CreateQuery(results.Groups[2].Value);
                
				if( q != null &&
					AddConditions( q, results.Groups[3].Value ) &&
					AddSelectFields( q, results.Groups[1].Value ) )
				{
				    Console.WriteLine( "Sending Query to zone.... " );
                    string queryXML = q.ToXml(SifVersion.LATEST);
                    Console.WriteLine( queryXML );
					// Store the original source query in the userData property
					q.UserData = queryXML;
                    this.ZoneFactory.GetAllZones()[0].Query(q);
				}
			} else {
				Console.WriteLine( "ERROR: Unrecognized query syntax..." );
				PrintSQLHelp();
			}
		}
		
	}
	
	private void PrintPrompt(){
		Console.Write( "SIF:  " );
	}
	
	private void PrintSQLHelp(){
		Console.WriteLine( "Syntax: Select {fields} From {SIF Object} [Where {field}={value}] " );
		Console.WriteLine( "  {fields} one or more field names, seperated by a comma" );
		Console.WriteLine( "           (may by empty or * )" );
		Console.WriteLine( "  {SIF Object} the name of a SIF Object that is provided in the zone" );
		Console.WriteLine( "  {field} a field name" );
		Console.WriteLine( "  {value} a value" );
		Console.WriteLine( "Examples:" );
		Console.WriteLine( "SIF: Select * from StudentPersonal" );
		Console.WriteLine( "SIF: Select * from StudentPersonal where RefId=43203167CFF14D08BB9C8E3FD0F9EC3C" );
		Console.WriteLine( "SIF: Select * from StudentPersonal where Name/FirstName=Amber" );
		Console.WriteLine( "SIF: Select Name/FirstName, Name/LastName from StudentPersonal where Demographics/Gender=F" );
		Console.WriteLine( "SIF: Select * from StudentSchoolEnrollment where RefId=43203167CFF14D08BB9C8E3FD0F9EC3C" );
		Console.WriteLine();
	}
	
	private Query CreateQuery( String fromClause ){
		IElementDef queryDef = Adk.Dtd.LookupElementDef( fromClause.Trim() );
		if( queryDef == null ){
			Console.WriteLine( "ERROR: Unrecognized FROM statement: " + fromClause );
			PrintSQLHelp();
			return null;
		} else{
			return new Query( queryDef );
		}
	}
	
	private bool AddSelectFields(Query q, String selectClause )
	{
        if( selectClause.Length == 0 || selectClause.IndexOf( "*" ) > -1 ){
			return true;
		}
        string[] fields = selectClause.Split(new char[] { ',' });
		foreach(string field in fields){
			string val = field.Trim();
			if( val.Length > 0 ){
				IElementDef restriction = Adk.Dtd.LookupElementDefBySQP( q.ObjectType, val );
				if( restriction == null ){
					Console.WriteLine( "ERROR: Unrecognized SELECT field: " + val );
					PrintSQLHelp();
					return false;
				} else {
                    q.AddFieldRestriction(restriction);
				}
			}
		}
		return true;
	}
	
	private bool AddConditions(Query q, String whereClause )
	{
        InitializeComparisonList();
		bool added = true;
		whereClause = whereClause.Trim();
		if( whereClause.Length == 0 ){
			return added;
		}
		
		string[] whereConditions = Regex.Split(whereClause, "[aA][nN][dD]");
        ComparisonOperators cmpOperator = ComparisonOperators.EQ;
        string[] fields = null;

		if (whereConditions.Length > 0) {
			foreach (String condition in whereConditions) {
                fields = null;
                foreach (KeyValuePair<string, ComparisonOperators> kvp in supportedComparisons) {
                    string cmpString = kvp.Key;
                    cmpOperator = kvp.Value;
                    if (cmpOperator == ComparisonOperators.EQ) {
                        int index = condition.LastIndexOf(cmpString);
                        fields = new string[2];
                        if (index > 0) {
                            fields[0] = condition.Substring(0, index);
                            fields[1] = condition.Substring((index + 1));
                        } else {
                            fields[0] = condition;
                        }
                            
                    }//end if

                    if (fields == null) {
                        fields = Regex.Split(condition, cmpString);
                    }
                    
                    if (fields[0] == condition) { 
                        //Means no match found using that current comparison operator
                        //so skip this condition
                        fields = null;
                        continue;
                    }

                    if (fields.Length != 2) {
                        Console.WriteLine("ERROR: Unsupported where clause: " + whereClause);
                        PrintSQLHelp();
                        added = false;
                        break;
                    }

                    string fieldExpr = fields[0].Trim();
                    IElementDef def = Adk.Dtd.LookupElementDefBySQP(q.ObjectType, fieldExpr );
                    if (def == null) {
                        Console.WriteLine("ERROR: Unrecognized field in where clause: " + fieldExpr );
                        PrintSQLHelp();
                        added = false;
                        break;
                    } else {
                        if (fieldExpr.IndexOf('[') > 0)
                        {
                            // If there is a square bracket in the field syntax, use the raw XPath,
                            // rather then the ElementDef because using ElementDef restrictions
                            // does not work for XPath expressions that contain predicates
                            // Note that using raw XPath expressions works fine, but the ADK is no longer
                            // going to be able to use version-independent rendering of the query
                            q.AddCondition(fieldExpr, cmpOperator, fields[1].Trim());
                        }
                        else
                        {
                            q.AddCondition( def, cmpOperator, fields[1].Trim() );
                            
                        }
                        //our condition has been found, no need to check the other comparison
                        //operators for a match so move to the next condition
                        break;
                    }
                }//end foreach
			}//end foreach
		}
		
		return added;
	}
    
	
	public void OnQueryPending(IMessageInfo info, IZone zone){
		SifMessageInfo smi = (SifMessageInfo)info;
		Console.WriteLine( "Sending SIF Request with MsgId " + smi.MsgId + " to zone " + zone.ZoneId );
	}

	public void OnQueryResults(IDataObjectInputStream data, SIF_Error error, IZone zone, IMessageInfo info) {
		
		SifMessageInfo smi = (SifMessageInfo)info;
	    DateTime start = DateTime.Now;
        if (smi.Timestamp.HasValue) {
            start = smi.Timestamp.Value;
        }

		Console.WriteLine();
		Console.WriteLine( "********************************************* " );
		Console.WriteLine( "Received SIF_Response packet from zone" + zone.ZoneId );
		Console.WriteLine( "Details... " );
		Console.WriteLine( "Request MsgId: " + smi.SIFRequestMsgId );
		Console.WriteLine( "Packet Number: " + smi.PacketNumber );
		Console.WriteLine();
		
		if( error != null ){
		
			Console.WriteLine( "The publisher returned an error: " ); 
			Console.WriteLine( "Category: " + error.SIF_Category + " Code: " + error.SIF_Code  );
			Console.WriteLine( "Description " + error.SIF_Desc );
			if( error.SIF_ExtendedDesc != null )
			{
				Console.WriteLine( "Details: " + error.SIF_ExtendedDesc );
			}
			return;
		}
		
		try
		{
			int objectCount = 0;
            while( data.Available ){
                SifDataObject next = data.ReadDataObject();
				objectCount++;
				Console.WriteLine();
				Console.WriteLine( "Text Values for " + next.ElementDef.Name + " " + objectCount + " {" + next.Key + "}" );
				
				SifXPathContext context = SifXPathContext.NewSIFContext(next);
                
				//	Print out all attributes 
                Console.WriteLine("Attributes:");
				XPathNodeIterator textNodes = context.Select("//@*");
                while( textNodes.MoveNext() ) {
                    XPathNavigator navigator = textNodes.Current;
                    Element value = (Element)navigator.UnderlyingObject;
					IElementDef valueDef = value.ElementDef;
					Console.WriteLine( valueDef.Parent.Tag( SifVersion.LATEST ) + "/@" + valueDef.Tag( SifVersion.LATEST ) + "=" + value.TextValue + ", " );
				}
				Console.WriteLine();
				// Print out all  elements that have a text value
                Console.WriteLine("Element:");
				textNodes = context.Select("//*");
				while( textNodes.MoveNext() ) {
                    XPathNavigator navigator = textNodes.Current;
                    Element value = (Element)navigator.UnderlyingObject;
					String textValue = value.TextValue;
					if( textValue != null ){
						IElementDef valueDef = value.ElementDef;
						Console.WriteLine( valueDef.Tag( SifVersion.LATEST ) + "=" + textValue + ", " );
					}
				}

			}
			Console.WriteLine();
			Console.WriteLine( "Total Objects in Packet: " + objectCount );
			
			
			
		} catch( Exception ex ){
			Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
		}

        if (!smi.MorePackets) {
			// This is the final packet. Print stats
			Console.WriteLine( "Final Packet has been received." );
            IRequestInfo ri = smi.SIFRequestInfo;
			if( ri != null ){
				Console.WriteLine( "Source Query: " );	
				Console.WriteLine( ri.UserData );
                TimeSpan difference = start.Subtract(ri.RequestTime);
				Console.WriteLine( "Query execution time: " + difference.Milliseconds + " ms" );
			}
			
		} else {
			Console.WriteLine( "This is not the final packet for this SIF_Response" );	
		}
		
		Console.WriteLine( "********************************************* " );
		Console.WriteLine( );
		PrintPrompt();
	}


    
}

}
