using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Reporting;
namespace Library.Nunit.US
{
   /// <summary>
   /// Summary description for SifElementTests.
   /// </summary>
   [TestFixture]
   public class SifElementTests : AdkTest
   {
      /// <summary>
      /// Passing a null SIFDate to an SDO object property still causes the element to be written out. 
      /// For example, calling ReportingPeriod.setBeginReportDate() with a null value will still cause 
      /// the <BeginReportDate> element to be written out.
      /// </summary>
      [Test]
      public void TestSIFDate()
      {

         ReportingPeriod period = new ReportingPeriod();
         period.BeginReportDate = new DateTime?();
         period.EndReportDate = new DateTime?();
         period.BeginSubmitDate = new DateTime?();
         period.EndSubmitDate = new DateTime?();
         period.DueDate = new DateTime?();
         Assert.IsNull(period.BeginReportDate, "BeginReportDate contains an empty date element");
         Assert.IsNull(period.EndReportDate, "EndReportDate contains an empty date element");
         Assert.IsNull(period.BeginSubmitDate, "BeginSubmitDate contains an empty date element");
         Assert.IsNull(period.EndSubmitDate, "EndSubmitDate contains an empty date element");

      }//end TestSIFDate
      [Test]
      public void TestSerializeable()
      {
         MemoryStream ms = new MemoryStream();
         BinaryFormatter formatter = new BinaryFormatter();
        
         //************************************************************************
         //Serialize / Deserialize Name object                           **********
         Name name = new Name(NameType.LEGAL, "Nahorniak", "Mike");
         Name result;
         formatter.Serialize(ms, name);
         ms.Seek(0, SeekOrigin.Begin);
         result = (Name)formatter.Deserialize(ms);
         Assert.AreEqual(name.FirstName, result.FirstName, "Name.FirstName field did not properly deserialize");
         Assert.AreEqual(name.FirstName, result.FirstName, "Name.LastName field did not properly deserialize");

         //************************************************************************
         //Serialize / Deserialize StudentPersonal object                **********        
         ms.SetLength(0);
         ms.Position = 0;
         StudentPersonal sp = new StudentPersonal();
         StudentPersonal spResult;
         formatter.Serialize(ms, sp);
         ms.Seek(0, SeekOrigin.Begin);
         spResult = (StudentPersonal)formatter.Deserialize(ms);
         Assert.AreEqual(sp.Name, spResult.Name, "Deserialized StudentPersonal Name elements do not match");
      
         //************************************************************************
         //Serialize / Deserialize SIF_ERROR object                      **********
         ms.SetLength(0);
         ms.Position = 0;
         SIF_Error error = new SIF_Error
          ((int)SifErrorCategoryCode.Generic,
           SifErrorCodes.GENERIC_GENERIC_ERROR_1,
           "Could not serialize the SIF_Err object");
         SIF_Error result_error;

         formatter.Serialize(ms, error);
         ms.Seek(0, SeekOrigin.Begin);
         result_error = (SIF_Error)formatter.Deserialize(ms);
         Assert.AreEqual(error.ToString(), result_error.ToString(), "Deserialized SIF_Error  match");
         ms.Close();


      }


      [Test]
      public void SharedChildren()
      {
         Adk.SifVersion = SifVersion.LATEST;
         StudentPersonal sp = new StudentPersonal(Adk.MakeGuid(), new Name(NameType.LEGAL, "hello", "world"));
         // Replace the existing demographics so there is no confusion
         Demographics d = new Demographics();
         sp.Demographics = d;
         d.SetCountryOfBirth(CountryCode.US);
         CountriesOfCitizenship countries = new CountriesOfCitizenship();
         d.CountriesOfCitizenship = countries;
         CountriesOfResidency residencies = new CountriesOfResidency();
         d.CountriesOfResidency = residencies;

         countries.AddCountryOfCitizenship(CountryCode.Wrap("UK"));
         residencies.AddCountryOfResidency(CountryCode.Wrap("AU"));

         // overwrite the country codes again, just to try to repro the issue
         d.SetCountryOfBirth(CountryCode.Wrap("AA")); // Should overwrite the existing one

         //Remove the existing CountryOfCitizenship, add three more, and remove the middle one
         Assert.IsTrue(countries.Remove(CountryCode.Wrap("UK")));
         countries.AddCountryOfCitizenship(CountryCode.Wrap("BB1"));
         countries.AddCountryOfCitizenship(CountryCode.Wrap("BB2"));
         countries.AddCountryOfCitizenship(CountryCode.Wrap("BB3"));
         Assert.IsTrue(countries.Remove(CountryCode.Wrap("BB2")));

         // Remove the existing CountryOfResidency, add three more, and remove the first one
         Assert.IsTrue(residencies.Remove(CountryCode.Wrap("AU")));
         residencies.AddCountryOfResidency(CountryCode.Wrap("CC1"));
         residencies.AddCountryOfResidency(CountryCode.Wrap("CC2"));
         residencies.AddCountryOfResidency(CountryCode.Wrap("CC3"));
         Assert.IsTrue(residencies.Remove(CountryCode.Wrap("CC1")));

         StudentPersonal sp2 = AdkObjectParseHelper.runParsingTest(sp, SifVersion.LATEST);

         // The runParsingTest() method will compare the objects after writing them and reading them
         // back in, but to be completely sure, let's assert the country codes again

         // NOTE: Due to the .Net Array.Sort algorithm, repeatable elements come out in reverse order.
         // This doesn't appear to be a problem yet, but may be fixed in a future release.
         // For now, these tests look for the elements in reverse order

         Demographics d2 = sp2.Demographics;
         Assert.AreEqual("AA", d2.CountryOfBirth.ToString(), "Country of Birth");
         Country[] citizenships = d2.CountriesOfCitizenship.ToArray();

         Assert.AreEqual(2, citizenships.Length, "Should be two CountryOfCitizenships");
         Assert.AreEqual("BB1", citizenships[0].TextValue, "First CountryOfCitizenship");
         Assert.AreEqual("BB3", citizenships[1].TextValue, "Second CountryOfCitizenship");

         // assert
         Country[] resid = d2.CountriesOfResidency.ToArray();
         Assert.AreEqual(2, resid.Length, "Should be two CountryOfResidencys");
         Assert.AreEqual("CC2", resid[0].TextValue, "First CountryOfResidencys");
         Assert.AreEqual("CC3", resid[1].TextValue, "Second CountryOfResidencys");
      }

      /**
  * Asserts that the new setArray() method is there and works as expected
  */

      [Test]
      public void testSetChildren()
      {
         StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
         Email email1 = new Email(EmailType.PRIMARY, "email@mail.com");
         Email email2 = new Email(EmailType.ALT1, "email2@mail.com");

         sp.EmailList = new EmailList();
         sp.EmailList.SetChildren(CommonDTD.EMAIL, new Email[] { email1, email2 });

         EmailList studentEmails = sp.EmailList;
         Assert.AreEqual(2, studentEmails.ChildCount, "Should be two emails");

         studentEmails.SetChildren(CommonDTD.EMAIL, new Email[0]);
         studentEmails = sp.EmailList;
         Assert.AreEqual(0, studentEmails.ChildCount, "Should be zero emails after setting empty array");

         studentEmails.SetChildren(CommonDTD.EMAIL, new Email[] { email1, email2 });
         studentEmails = sp.EmailList;
         Assert.AreEqual(2, studentEmails.ChildCount, "Should be two emails");

         studentEmails.SetChildren(CommonDTD.EMAIL, null);
         studentEmails = sp.EmailList;
         Assert.AreEqual(0, studentEmails.ChildCount, "Should be zero emails after setting null");
      }


      /**
     * Asserts that ADKGen adds a method that takes an array of objects for repeatable elements
     */

      [Test]
      public void testSetEmails()
      {
         StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
         Email email1 = new Email(EmailType.PRIMARY, "email@mail.com");
         Email email2 = new Email(EmailType.ALT1, "email2@mail.com");

         EmailList eList = new EmailList();
         eList.AddRange(email1, email2);
         sp.EmailList = eList;


         EmailList studentEmails = sp.EmailList;
         Assert.AreEqual(2, studentEmails.Count, "Should be two emails");

         studentEmails.Clear();
         Assert.AreEqual(0, studentEmails.Count, "Should be zero emails after clearing");

         sp.EmailList = new EmailList();
         studentEmails = sp.EmailList;
         Assert.AreEqual(0, studentEmails.Count, "Should be zero emails after setting empty list");
      }


      /**
     *  Asserts that an object can have the same child object set to it more than once
     */

      [Test]
      public void testAddChildTwice()
      {
         StudentPersonal sp1 = new StudentPersonal();
         Email email1 = new Email(EmailType.PRIMARY, "email@mail.com");

         EmailList eList = new EmailList();
         sp1.EmailList = eList;

         eList.AddChild(CommonDTD.EMAIL, email1);
         // We should be able to add the same child twice without getting an exception
         eList.AddChild(CommonDTD.EMAIL, email1);

         // Add it again, using the overload
         eList.AddChild(email1);

         Email[] studentEmails = sp1.EmailList.ToArray();
         Assert.AreEqual(1, studentEmails.Length, "Should be one email");

         StudentPersonal sp2 = ObjectCreator.CreateStudentPersonal();
         Email email2 = new Email(EmailType.ALT1, "email2@mail.com");
         EmailList elist2 = new EmailList();
         elist2.Add(email2);
         sp2.EmailList = elist2;

         bool exceptionThrown = false;
         try
         {
            eList.AddChild(email2); // should throw here
         }
         catch (InvalidOperationException)
         {
            exceptionThrown = true;
         }

         Assert.IsTrue(exceptionThrown, "IllegalStateException should have been thrown in addChild(SIFElement)");

         exceptionThrown = false;
         try
         {
            eList.AddChild(CommonDTD.EMAILLIST, email2); // should throw here
         }
         catch (InvalidOperationException)
         {
            exceptionThrown = true;
         }

         Assert.IsTrue(exceptionThrown,
                       "IllegalStateException should have been thrown in addChild( ElementDef, SIFElement)");
      }

      [Test]
      public void testIDProperty()
      {
         SifElement element = new StudentPersonal();
         element.XmlId = "Foo";

         Assert.AreEqual("Foo", element.XmlId);
      }
   }
}