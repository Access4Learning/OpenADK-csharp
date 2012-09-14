//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using OpenADK.Library;
using OpenADK.Library.au.Common;
using OpenADK.Library.au.Student;
using Timer=System.Timers.Timer;

namespace Library.Examples.SimpleProvider
{
    internal class StudentPersonalProvider : IPublisher
    {
        public StudentPersonalProvider()
        {
            try
            {
                InitializeDB();
            }
            catch ( AdkSchemaException adkse )
            {
                Console.WriteLine( adkse );
            }
        }

        /**
  * The amount of time to wait in between sending change events 
  */
        private int EVENT_INTERVAL = 30000;
        /**
  * The data provided by this simple provider class 
  */
        private List<StudentPersonal> fData;
        /**
  *  The zone to send SIF events to
  */
        private IZone fZone;

        public void OnRequest( IDataObjectOutputStream outStream,
                               Query query,
                               IZone zone,
                               IMessageInfo info )
        {
            // To be a successful publisher of data using the ADK, follow these steps

            // 1) Examine the query conditions. If they are too complex for your agent, 
            // 	throw the appropriate SIFException

            // This example agent uses the autoFilter() capability of DataObjectOutputStream. Using
            // this capability, any object can be written to the output stream and the stream will 
            // filter out any objects that don't meet the conditions of the Query. However, a more
            // robust agent with large amounts of data would want to pre-filter the data when it does its
            // initial database query.
            outStream.Filter = query;

            Console.WriteLine( "Responding to SIF_Request for StudentPersonal" );

            // 2) Write any data to the output stream
            foreach ( StudentPersonal sp in fData )
            {
                outStream.Write( sp );
            }
        }

        /**
  * @param zone
  * @throws ADKException
  */

        public void Provision( IZone zone )
        {
            zone.SetPublisher( this, StudentDTD.STUDENTPERSONAL, null );
        }


        /**
  * This class periodically publishes change events to the zone. This 
  * method starts up the thread that publishes the change events.
  * 
  * A normal SIF Agent would, instead look for changes in the application's database
  * and publish those as changes.
  */

        public void StartEventProcessing( IZone zone )
        {
            if ( fZone != null )
            {
                throw new AdkException( "Event Processing thread is already running", zone );
            }
            fZone = zone;
            Timer timer = new Timer();
            timer.Elapsed += delegate { SendEvent(); };
            timer.Interval = 5000;
            timer.Start();
        }


        public void SendEvent()
        {
            /**
    * This class periodically publishes change events to the zone. This 
    * method starts up the thread that publishes the change events.
    * 
    * A normal SIF Agent would, instead look for changes in the application's database
    * and publish those as changes.
    */

            Console.WriteLine
                ( "Event publishing enabled with an interval of " + EVENT_INTERVAL/1000 +
                  " seconds." );

            Random random = new Random();

            bool isProcessing = true;
            // Go into a loop and send events
            while ( isProcessing )
            {
                try
                {
                    Thread.Sleep( EVENT_INTERVAL );

                    StudentPersonal changedObject = fData[random.Next( fData.Count )];
                    StudentPersonal eventObject = new StudentPersonal();
                    eventObject.RefId = changedObject.RefId;

                    // Create a change event with a random Student ID;
                    String newNum = "A" + random.Next( 999999 ).ToString();
                    eventObject.LocalId = newNum;
                    fZone.ReportEvent( eventObject, EventAction.Change );
                }
                catch ( Exception ex )
                {
                    Console.WriteLine( "Error during event processing: " + ex );
                    isProcessing = false;
                }
            }
        }


        protected void InitializeDB()
        {
            fData = new List<StudentPersonal>();

            Random rand = new Random();
            fData.Add
                ( CreateStudent
                      ( "C831540004395", "Ackerman", "Brian",
                        "133 Devon Drive", "Birmingham", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4541000", Sex.C1_MALE, YearLevelCode.C7_YEAR_7, "19910102" ) );
            fData.Add
                ( CreateStudent
                      ( "M830540004340", "Acosta", "Stacey",
                        "17 Wellesley Park", "Birmingham", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4541001", Sex.C2_FEMALE, YearLevelCode.C8_YEAR_8,
                        "19871012" ) );
            fData.Add
                ( CreateStudent
                      ( "X831540004405", "Addicks", "Amber",
                        "162 Crossfield Road", "Birmingham", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4541002", Sex.C2_FEMALE, YearLevelCode.C9_YEAR_9,
                        "19920402" ) );
            fData.Add
                ( CreateStudent
                      ( "Birmingham", "Aguilar", "Mike",
                        "189 Writtle Mews", "Edgebaston", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4541003", Sex.C1_MALE, YearLevelCode.C5_YEAR_5, "19910102" ) );
            fData.Add
                ( CreateStudent
                      ( "A831540004820", "Alaev", "Dianna",
                        "737 Writtle Mews", "Birmingham", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4541004", Sex.C2_FEMALE, YearLevelCode.C4_YEAR_4,
                        "19980704" ) );
            fData.Add
                ( CreateStudent
                      ( "G831540004469", "Balboa", "Amy",
                        "87 Almond Avenue", "Birmingham", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4545000", Sex.C2_FEMALE, YearLevelCode.C3_YEAR_3,
                        "19901205" ) );
            fData.Add
                ( CreateStudent
                      ( "H831540004078", "Baldwin", "Joshua",
                        "142 Clarendon Avenue", "Edgebaston", "VIC", CountryCode.Wrap( "AU" ),
                        "35203",
                        "0121 4545001", Sex.C1_MALE, YearLevelCode.C8_YEAR_8, "19960105" ) );
            fData.Add
                ( CreateStudent
                      ( "J830540004064", "Ballard", "Aimee",
                        "134 Dixon Close", "Birmingham", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4545002", Sex.C2_FEMALE, YearLevelCode.C5_YEAR_5,
                        "19940506" ) );
            fData.Add
                ( CreateStudent
                      ( "V831540004012", "Banas", "Ryan",
                        "26 Mountview Drive", "Birmingham", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4545003", Sex.C1_MALE, YearLevelCode.C4_YEAR_4, "19930808" ) );
            fData.Add
                ( CreateStudent
                      ( "B830540004119", "Barba", "Ashley",
                        "958 Manchester Road", "Birmingham", "VIC", CountryCode.Wrap( "AU" ), "35203",
                        "0121 4545004", Sex.C2_FEMALE, YearLevelCode.C9_YEAR_9,
                        "19890102" ) );
        }

        private static StudentPersonal CreateStudent(
            String id,
            String lastName,
            String firstName,
            String street,
            String city,
            String state,
            CountryCode country,
            String post,
            String phone,
            Sex gender,
            YearLevelCode grade,
            String birthDateyyyyMMdd )
        {
            StudentPersonal student = new StudentPersonal();
            ;
            student.RefId = Adk.MakeGuid();
            student.LocalId = id;

            PersonInfo stupersonal = new PersonInfo();
            student.PersonInfo = stupersonal;

            // Set the Name
            Name name = new Name( NameType.LEGAL );
            name.FamilyName = lastName;
            name.GivenName = firstName;
            stupersonal.Name = name;

            Address addr = new Address();
            addr.SetType( AddressType.C0765_PHYSICAL_LOCATION );
            addr.SetStreet( street );
            addr.City = city;
            addr.StateProvince = state;
            addr.PostalCode = post;
            addr.Country = country.ToString();

            stupersonal.AddressList = new AddressList( addr );

            stupersonal.PhoneNumberList =
                new PhoneNumberList( new PhoneNumber( PhoneNumberType.PRIMARY, phone ) );


            Demographics dem = new Demographics();
            dem.SetSex( gender );
            dem.BirthDate =
                DateTime.ParseExact
                    ( birthDateyyyyMMdd, "yyyyMMdd", CultureInfo.InvariantCulture.DateTimeFormat );

            stupersonal.Demographics = dem;

            return student;
        }
    }
}
