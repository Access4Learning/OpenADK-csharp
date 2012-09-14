//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Student;

namespace Library.Examples.SimpleSubscriber
{
   internal class StudentPersonalHandler : ISubscriber, IQueryResults
   {
      ///<summary>The total number of objects received from the request for StudentPersonal </summary>
      private int fObjectCount = 0;

      ///<summary> Allows this data handler class to provision itself with the zone.<p>
      /// 
      /// Provisioning involves notifying the zone of any objects this class
      /// subscribes, or provides data for, or whether this class should be notified
      /// of SIF_Responses for a specific data type.</summary>
      /// <param name="zone"></param>
      public void provision(IZone zone)
      {
         zone.SetSubscriber(this, StudentDTD.STUDENTPERSONAL, null);
         zone.SetQueryResults(this, StudentDTD.STUDENTPERSONAL, null );
      }



      /// <summary> Signals this class to begin the syncrhonization process. This class is responsible
      /// for querying the zone for any data it needs to synchronize itself.
      /// </summary>
      /// <param name="zone"></param>
      public void sync(IZone zone)
      {
         // This class simply requests all LearnerPersonal objects from the zone
         Query q = new Query(StudentDTD.STUDENTPERSONAL);
         // Add any query conditions you may have
         //q.addCondition( LearnerDTD.LEARNERPERSONAL_UPN, ComparisonOperators.LE, "M830540004340" );
         zone.Query(q);
      }

      ////////////////////////////////////////////////////////////////////////////////
      //
      //  QueryResults interface
      //

      public void OnQueryPending(IMessageInfo info,
                                  IZone zone)
      {
         //  Not used by this agent.
      }


      public void OnQueryResults(IDataObjectInputStream data,
                                  SIF_Error error,
                                  IZone zone,
                                  IMessageInfo info)
      {
         // Demonstrates basic handling of a SIF_Query response

         // 1) 	To read data from a SIF_Response, first check to see if an error was returned
         if (error != null)
         {
            // The provider returned an error message for this SIF_Request
            Console.WriteLine("An error was received from the provider of LearnerPersonal.");
            Console.WriteLine(error.SIF_Desc + "\r\n" + error.SIF_ExtendedDesc);
            return;
         }

         // 2) 	Now, read each object from the DataObjectInputStream until available() returns false
         while (data.Available)
         {
            StudentPersonal sp = (StudentPersonal)data.ReadDataObject();
            fObjectCount++;
            Name stuName = sp.Name;
            Console.WriteLine
                (fObjectCount + ") Refid:{" + sp.RefId +
                  "} Name: " + stuName.FirstName + " " + stuName.LastName);
         }

         // Demonstration purposes only: print out the total number of objects recieved
         Console.WriteLine(fObjectCount + " total objects received.");


         // 3)	To determine if you have completed receiving all responses, check the
         // 		MorePackets property of the SIFMessageInfo object
         if (((SifMessageInfo)info).MorePackets)
         {
            Console.WriteLine("Waiting for more packets...");
         }
         else
         {
            Console.WriteLine("All requested packets have been received");
         }
      }


      public void OnEvent(Event evnt,
                           IZone zone,
                           IMessageInfo info)
      {
         // Demonstrates basic handling of a SIF Event
         Console.WriteLine("Received a " + evnt.ActionString + " event for LearnerPersonal");
         StudentPersonal sp = (StudentPersonal)evnt.Data.ReadDataObject();

         // Simply write the XML of the event object to System.out
         Console.WriteLine(sp.ToXml());
         Console.WriteLine("End Event");
      }
   }
}
