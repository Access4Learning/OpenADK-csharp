//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Timers;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using Timer = System.Timers.Timer;

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
         catch (AdkSchemaException adkse)
         {
            Console.WriteLine(adkse);
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

      public void OnRequest(IDataObjectOutputStream outStream,
                             Query query,
                             IZone zone,
                             IMessageInfo info)
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

         Console.WriteLine("Responding to SIF_Request for StudentPersonal");

         // 2) Write any data to the output stream
         foreach (StudentPersonal sp in fData)
         {
            outStream.Write(sp);
         }
      }

      /**
  * @param zone
  * @throws ADKException
  */

      public void Provision(IZone zone)
      {
         zone.SetPublisher(this, StudentDTD.STUDENTPERSONAL, null);
      }


      /**
  * This class periodically publishes change events to the zone. This 
  * method starts up the thread that publishes the change events.
  * 
  * A normal SIF Agent would, instead look for changes in the application's database
  * and publish those as changes.
  */

      public void StartEventProcessing(IZone zone)
      {
         if (fZone != null)
         {
            throw new AdkException("Event Processing thread is already running", zone);
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
             ("Event publishing enabled with an interval of " + EVENT_INTERVAL / 1000 +
               " seconds.");

         Random random = new Random();

         bool isProcessing = true;
         // Go into a loop and send events
         while (isProcessing)
         {
            try
            {
               Thread.Sleep(EVENT_INTERVAL);

               StudentPersonal changedObject = fData[random.Next(fData.Count)];
               StudentPersonal eventObject = new StudentPersonal();
               eventObject.RefId = changedObject.RefId;

               // Create a change event with a random Student ID;
               String newNum = "A" + random.Next(999999).ToString();
               eventObject.LocalId = newNum;
               fZone.ReportEvent(eventObject, EventAction.Change);

            }
            catch (Exception ex)
            {

               Console.WriteLine("Error during event processing: " + ex);
               isProcessing = false;
            }
         }
      }


      protected void InitializeDB()
      {
         fData = new List<StudentPersonal>();

         Random rand = new Random();
         fData.Add
             (CreateStudent
                   ("C831540004395", "Ackerman", "Brian",
                     "133 Devon Drive", "Birmingham", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4541000", Gender.MALE, GradeLevelCode.C07, RaceType.WHITE, "19910102"));
         fData.Add
             (CreateStudent
                   ("M830540004340", "Acosta", "Stacey",
                     "17 Wellesley Park", "Birmingham", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4541001", Gender.FEMALE, GradeLevelCode.C08, RaceType.AFRICAN_AMERICAN,
                     "19871012"));
         fData.Add
             (CreateStudent
                   ("X831540004405", "Addicks", "Amber",
                     "162 Crossfield Road", "Birmingham", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4541002", Gender.FEMALE, GradeLevelCode.C07, RaceType.WHITE,
                     "19920402"));
         fData.Add
             (CreateStudent
                   ("Birmingham", "Aguilar", "Mike",
                     "189 Writtle Mews", "Edgebaston", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4541003", Gender.MALE, GradeLevelCode.C07, RaceType.WHITE, "19910102"));
         fData.Add
             (CreateStudent
                   ("A831540004820", "Alaev", "Dianna",
                     "737 Writtle Mews", "Birmingham", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4541004", Gender.FEMALE, GradeLevelCode.C07, RaceType.PACISLANDER,
                     "19980704"));
         fData.Add
             (CreateStudent
                   ("G831540004469", "Balboa", "Amy",
                     "87 Almond Avenue", "Birmingham", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4545000", Gender.FEMALE, GradeLevelCode.C01, RaceType.WHITE,
                     "19901205"));
         fData.Add
             (CreateStudent
                   ("H831540004078", "Baldwin", "Joshua",
                     "142 Clarendon Avenue", "Edgebaston", StatePrCode.AL, CountryCode.US,
                     "35203",
                     "0121 4545001", Gender.MALE, GradeLevelCode.C02, RaceType.WHITE, "19960105"));
         fData.Add
             (CreateStudent
                   ("J830540004064", "Ballard", "Aimee",
                     "134 Dixon Close", "Birmingham", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4545002", Gender.FEMALE, GradeLevelCode.C03, RaceType.WHITE,
                     "19940506"));
         fData.Add
             (CreateStudent
                   ("V831540004012", "Banas", "Ryan",
                     "26 Mountview Drive", "Birmingham", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4545003", Gender.MALE, GradeLevelCode.C04, RaceType.WHITE, "19930808"));
         fData.Add
             (CreateStudent
                   ("B830540004119", "Barba", "Ashley",
                     "958 Manchester Road", "Birmingham", StatePrCode.AL, CountryCode.US, "35203",
                     "0121 4545004", Gender.FEMALE, GradeLevelCode.C05, RaceType.ASIAN,
                     "19890102"));
      }

      private static StudentPersonal CreateStudent(
          String id,
          String lastName,
            String firstName,
            String street,
            String city,
            StatePrCode state,
            CountryCode country,
            String post,
            String phone,
            Gender gender,
            GradeLevelCode grade,
            RaceType race,
            String birthDateyyyyMMdd)
      {
         StudentPersonal student = new StudentPersonal();
         ;
         student.RefId = Adk.MakeGuid();
         student.LocalId = id;

         // Set the Name
         Name name = new Name(NameType.LEGAL, firstName, lastName);
         student.Name = name;

         Address addr = new Address();
         addr.SetType(AddressType.C0369_PERMANENT);
         addr.SetStreet(street);
         addr.City = city;
         addr.SetStateProvince(state);
         addr.PostalCode = post;
         addr.SetCountry(country);

         student.AddressList = new StudentAddressList(PickupOrDropoff.NA, "NA", addr);
         student.PhoneNumberList =
             new PhoneNumberList(new PhoneNumber(PhoneNumberType.PRIMARY, phone));


         Demographics dem = new Demographics();
         dem.RaceList = new RaceList(new Race("", race));
         dem.SetGender(gender);
         dem.BirthDate =
             DateTime.ParseExact
                 (birthDateyyyyMMdd, "yyyyMMdd", CultureInfo.InvariantCulture.DateTimeFormat);

         student.Demographics = dem;

         return student;
      }
   }
}
