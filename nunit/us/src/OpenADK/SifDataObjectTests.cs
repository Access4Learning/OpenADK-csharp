using System;
using OpenADK.Library;
using OpenADK.Library.Global;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;
using System.Collections;

namespace Library.Nunit.US
{
   /// <summary>
   /// Summary description for SifDataObjectTests.
   /// </summary>
   [TestFixture]
   public class SifDataObjectTests : AdkTest
   {
      [Test]
      public void CreateElementOrAttribute()
      {
         String firstName = "Rosemary";
         String middleName = null;
         String lastName = "Evans";
         StudentPersonal retval = new StudentPersonal();

         retval.SetElementOrAttribute("Name[@Type='04']/FirstName", firstName);
         retval.SetElementOrAttribute("Name[@Type='04']/MiddleName", middleName);
         retval.SetElementOrAttribute("Name[@Type='04']/LastName", lastName);

         Name name = retval.Name;
         Assert.AreEqual(firstName, name.FirstName, "First Name");
         Assert.AreEqual(lastName, name.LastName, "Last Name");
         Assert.IsNull(name.MiddleName, "Middle Name");

         // echo to the console so we can see what's going on
         SifWriter writer = new SifWriter(Console.Out);
         writer.Write(retval);
         writer.Flush();
      }

      /**
  * Asserts that ADKGen adds a setSIFExtendedElements( SIF_ExtendedElements ) to each
  * SIFDataObject 
  */

      [Test]
      public void testSetSIF_ExtendedElementsContainer()
      {
         StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
         SIF_ExtendedElements elements = new SIF_ExtendedElements();
         elements.AddSIF_ExtendedElement("urn:foo", "bar");
         // Add another one with the same key
         elements.AddSIF_ExtendedElement("urn:foo", "bar2");
         // Add a third one with a different key
         elements.AddSIF_ExtendedElement("urn:foo2", "bar3");

         sp.SIFExtendedElementsContainer = elements;

         SIF_ExtendedElement[] elementArray = sp.SIFExtendedElementsContainer.ToArray();
         Assert.AreEqual(3, elementArray.Length, "Should have three items in it.");

         // Try to find all three elements to make sure that they are all there
         bool found1 = false, found2 = false, found3 = false;
         for (int i = 0; i < 3; i++)
         {
            SIF_ExtendedElement test = elementArray[i];
            String key = test.Key;
            if (key.Equals("urn:foo"))
            {
               String value = test.TextValue;
               if (value.Equals("bar"))
               {
                  found1 = true;
               }
               else if (value.Equals("bar2"))
               {
                  found2 = true;
               }
            }
            else
            {
               found3 = key.Equals("urn:foo2") && test.TextValue.Equals("bar3");
            }
         }
         Assert.IsTrue(found1, "Element1 was not found");
         Assert.IsTrue(found2, "Element2 was not found");
         Assert.IsTrue(found3, "Element3 was not found");
      }



      /**
     * Asserts that ADKGen adds a setSIFExtendedElements( SIF_ExtendedElements ) to each
     * SIFDataObject 
     */

      [Test]
      public void testSetttingSIF_ExtendedElements()
      {
         StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
         sp.AddSIFExtendedElement("urn:foo", "bar");
         // Add another one with the same key
         sp.AddSIFExtendedElement("urn:foo", "bar2");
         // Add a hird one with a different key
         sp.AddSIFExtendedElement("urn:foo2", "bar3");

         SIF_ExtendedElement[] elementArray = sp.SIFExtendedElementsContainer.ToArray();
         Assert.AreEqual(2, elementArray.Length, "Should have two items in it.");

         // Try to find both elements to make sure that they are there
         bool found2 = false, found3 = false;
         for (int i = 0; i < 2; i++)
         {
            SIF_ExtendedElement test = elementArray[i];
            String key = test.Key;
            if (key.Equals("urn:foo"))
            {
               String value = test.TextValue;
               if (value.Equals("bar2"))
               {
                  found2 = true;
               }
            }
            else
            {
               found3 = key.Equals("urn:foo2") && test.TextValue.Equals("bar3");
            }
         }
         Assert.IsTrue(found2, "Element2 was not found");
         Assert.IsTrue(found3, "Element3 was not found");
      }
      //Asserts that SetChildren Completely replaces extendedElements list
      [Test]
      public void testExtendedElements()
      {
         StudentPersonal sp = new StudentPersonal();
         SIF_ExtendedElements container = sp.SIFExtendedElementsContainer;
         container.SetChildren(new SIF_ExtendedElement[] { new SIF_ExtendedElement("key1", "value1") });
         container.SetChildren(new SIF_ExtendedElement[] { new SIF_ExtendedElement("key1", "value1") });
         Assert.AreEqual(1, sp.SIFExtendedElements.Length, "Result should be 1");

      }

      //Asserts that null passed to SetChildren clears the extendedElements list
     
      [Test]
      public void testNullExtendedElements()
      {
         StudentPersonal sp = new StudentPersonal();
         SIF_ExtendedElements container = sp.SIFExtendedElementsContainer;
        
         container.SetChildren(new SIF_ExtendedElement[] { new SIF_ExtendedElement("key1", "value1") });
         container.AddChild(new SIF_ExtendedElement("key2", "value1"));
         container.SetChildren(new SIF_ExtendedElement[] { null });
        
         Assert.AreEqual(0, sp.SIFExtendedElements.Length, "Result should be 0");

      }





      //[Test]
      //public void testSIF1_5XPaths_Address()
      //{
      //   Adk.SifVersion = SifVersion.SIF15r1;
      //   StudentPersonal sp = new StudentPersonal();

      //   // StudentAddress
      //   sp.SetElementOrAttribute(
      //       "StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/Street/Line1", "321 Oak St");

      //   // Assert Student address
      //   StudentAddressList[] addrList = sp.AddressLists;

      //   Assertion.AssertNotNull("AddressList", addrList);
      //   Assertion.AssertEquals("One AddrList", 1, addrList.Length);

      //   //Address addr = addrList[0].getAddress(AddressType.Wrap("01"));
      //   Address addr = sp.AddressList[AddressType.Wrap("01")];


      //   Assertion.AssertNotNull("Address", addr);
      //   //Assertion.AssertEquals("Street", "321 Oak St", addr.Street.Line1);
      //   //SimpleField line1 = (SimpleField)sp.GetElementOrAttribute("StudentAddress[@PickupOrDropoff='NA',@DayOfWeek='NA']/Address[@Type='01']/Street/Line1");
      //   //Assertion.AssertNotNull("Line1 By xPath", line1);
      //   //Assertion.AssertEquals("Street", "321 Oak St", line1.TextValue());
      //}






   }
}