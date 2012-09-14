using System;
using OpenADK.Library.Global;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;

namespace OpenADK.Library
{
   /// <summary>
   /// Summary description for ObjectCreator.
   /// </summary>
   public sealed class ObjectCreator
   {
      public static StudentPersonal CreateStudentPersonal()
      {
         StudentPersonal sp = new StudentPersonal();

         AlertMessages alm = new AlertMessages();
         alm.AddAlertMessage(AlertMessageType.LEGAL, "This is the Legal Alert for Joe Student");
         sp.AlertMessages = alm;

         // TODO: Consider adding helper methods to the ADK that allow 
         // list elements to be set or gotten from the parent?
         OtherIdList oidList = new OtherIdList();
         oidList.AddOtherId(OtherIdType.SIF1x_OTHER, "P00001");
         oidList.AddOtherId(OtherIdType.SIF1x_HEATH_RECORD, "WB0025");
         oidList.AddOtherId(OtherIdType.SIF1x_SSN, "123-45-6789");

         sp.OtherIdList = oidList;

         Name name = new Name(NameType.BIRTH, "Student", "Joe");
         name.MiddleName = "";
         name.PreferredName = "Joe";
         sp.Name = name;

         EmailList elist = new EmailList();
         elist.AddEmail(EmailType.PRIMARY, "joe.student@anyschool.com");
         sp.EmailList = elist;

         sp.OnTimeGraduationYear = 1982;
         Demographics demo = new Demographics();
         demo.BirthDate = new DateTime(1981, 12, 20);
         demo.SetCitizenshipStatus(CitizenshipStatus.USCITIZEN);

         CountriesOfCitizenship countries = new CountriesOfCitizenship();
         countries.AddCountryOfCitizenship(CountryCode.US);
         countries.AddCountryOfCitizenship(CountryCode.Wrap("CA"));
         demo.CountriesOfCitizenship = countries;
         demo.SetCountryOfBirth(CountryCode.US);

         CountriesOfResidency cre = new CountriesOfResidency(new Country(CountryCode.IE));
         demo.CountriesOfResidency = cre;

         demo.SetStateOfBirth(StatePrCode.AK);
         sp.Demographics = demo;
         Address addr = new Address();
         addr.City = "Salt Lake City";
         addr.SetStateProvince(StatePrCode.UT);
         addr.SetCountry(CountryCode.US);
         addr.PostalCode = "84102";
         Street str = new Street();
         str.Line1 = "1 IBM Plaza";
         str.ApartmentNumber = "2000";
         str.Line2 = "Suite 2000";
         str.Line3 = "Salt Lake City, UT 84102";
         str.StreetName = "IBM";
         str.StreetNumber = "1";
         str.StreetType = "Plaza";
         str.ApartmentType = "Suite";
         addr.Street = str;
         sp.AddAddressList(PickupOrDropoff.NA, "MoTuWeThFrSaSu", addr);
        
          PhoneNumberList plist = new PhoneNumberList();
         plist.AddPhoneNumber(PhoneNumberType.SIF1x_HOME_PHONE, "(312) 555-1234");
         sp.PhoneNumberList = plist;

         //  Test changing the name
         sp.SetName(NameType.BIRTH, "STUDENT", "JOE");

         return sp;
      }

      public static StaffAssignment CreateStaffAssignment()
      {
          StaffAssignment sa = new StaffAssignment(Adk.MakeGuid(),
                 Adk.MakeGuid(), 2008, Adk.MakeGuid(), YesNo.YES);
          sa.Description = "Description of this Assignment" ;
          sa.EmployeePersonalRefId = Adk.MakeGuid() ;
          sa.GradeClassification = new GradeClassification( GradeClassificationCode.POSTSECONDARY);
          return sa;
      }
  }
}