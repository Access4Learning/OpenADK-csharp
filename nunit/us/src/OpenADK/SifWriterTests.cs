using System;
using System.IO;
using OpenADK.Library;
using OpenADK.Library.Global;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.UnitTesting.Framework;
using OpenADK.Library.Infra;


namespace Library.Nunit.US
{
   /// <summary>
   /// Summary description for SifWriterTests.
   /// </summary>
   [TestFixture]
   public class SifWriterTests : AdkTest
   {
      /// <summary>
      /// This method is not a true NUnit test, in that it doesn't do assertions.
      /// </summary>
      /// <remarks>
      /// However, it is useful when changing the behavior of the SIFWriter to see
      /// how changes affect the speed of writing.
      /// 
      /// All tests below run on the HP ZD7168CL laptop
      /// 
      /// .Net ADK Timings
      /// Ran in 7.093 - 7.39 seconds on 11/18/2004 ( XML Escaping is always on )
      /// Ran in 9.26  - 9.31 seconds on 12/20/2004 ( XML Escaping is always on )
      /// Ran in 8.75  - 9.0  seconds on 01/28/2005 ( Check XML Escaping on Elements, change to ElementDefImpl.FQClassName )
      /// Ran in 9.625        seconds on 02/28/2005 ( Check version before writing element )
      /// 
      /// Java ADK Timings:
      /// Ran in 14.953 - 15.062 seconds  on 11/18/2004 before XML escaping was added to SIFWriter
      /// Ran in 17.935 - 17.938 seconds  on 11/18/2004 after XML escaping was added to SIFWriter
      /// Ran in 15.053 - 15.127 seconds  on 11/18/2004 after XML escaping was added to SIFWriter with escaping turned off
      /// </remarks>
      [Test, Explicit]
      public void WriteSpeedTest()
      {
         Adk.Debug = AdkDebugFlags.None;

         StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
         Address addr = sp.AddressLists[0][0];
         // Add in a few cases where escaping needs to be done
         addr.Street.Line1 = "ATTN: \"Miss Thompson\"";
         addr.Street.Line2 = "321 Dunn & Bradstreeet Lane";
         addr.Street.Line3 = "Weyer's Way, MO 32254";

         // Dump the object once to the console
         SifWriter writer = new SifWriter(Console.Out);
         writer.Write(sp);
         writer.Flush();

         MemoryStream stream = new MemoryStream();
         writer = new SifWriter(stream);

         for (int a = 0; a < 50000; a++)
         {
            writer.Write(sp);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
         }

         Console.WriteLine("Test Complete. Please See timings for details");
      }
      [Test]
      public void FilterOutElementsFromDifferentVersion()
      {
         Adk.SifVersion = SifVersion.SIF11;
         StudentPersonal sp = ObjectCreator.CreateStudentPersonal();
         sp.StateProvinceId = "55889";
         sp.LocalId = "987987987987987";

         StudentPersonal sp11 = (StudentPersonal)AdkObjectParseHelper.WriteParseAndReturn(sp, SifVersion.SIF11);

         Assertion.AssertNull("LocalID", sp11.LocalId);
         Assertion.AssertNull("StatePRID", sp11.StateProvinceId);

         StudentPersonal sp15 = (StudentPersonal)AdkObjectParseHelper.WriteParseAndReturn(sp, SifVersion.SIF15r1);

         Assertion.AssertNotNull("LocalID", sp15.LocalId);
         Assertion.AssertNotNull("StatePRID", sp15.StateProvinceId);
      }


      [Test]
      public void TestEncodingHighAsciiChars()
      {
         StudentPersonal sp = new StudentPersonal();
         sp.RefId = Adk.MakeGuid();
         sp.StateProvinceId = "\u06DE55889";
         sp.LocalId = "987987987987987";

         StudentPersonal copy = (StudentPersonal)AdkObjectParseHelper.WriteParseAndReturn(sp, SifVersion.LATEST);

         Assert.AreEqual("\u06DE55889", copy.StateProvinceId, "LocalID, Encoded");
      }



       [Test]
       public void TestWriteXSINill()
       {
           StudentPersonal sp = new StudentPersonal();
           sp.RefId = Adk.MakeGuid();
           sp.StateProvinceId = "\u06DE55889";
           sp.LocalId = "987987987987987";
           Name name = new Name(NameType.LEGAL, "Johnson", "Steve");
           sp.Name = name;
           name.SetField( CommonDTD.NAME_TYPE, new SifString( null ) );
           name.SetField(CommonDTD.NAME_MIDDLENAME, new SifString(null));

           SIF_ExtendedElement see = new SIF_ExtendedElement("FOO", null );
           see.SetField(GlobalDTD.SIF_EXTENDEDELEMENT, new SifString(null));
           see.XsiType = "Integer";
           sp.SIFExtendedElementsContainer.Add(see);

           sp.SetField( StudentDTD.STUDENTPERSONAL_LOCALID, new SifString( null ) );


           Console.WriteLine(sp.ToXml());
           StudentPersonal copy = (StudentPersonal)AdkObjectParseHelper.WriteParseAndReturn(sp, SifVersion.LATEST, null, true);
           Console.WriteLine(copy.ToXml());

           name = copy.Name;
           Assert.IsNull(name.Type);
           Assert.IsNull(name.MiddleName);
           Assert.IsNotNull(name.FirstName);
           Assert.IsNotNull(name.LastName);

           // Attributes cannot be represented using xs nil
           SimpleField field = name.GetField(CommonDTD.NAME_TYPE);
           Assert.IsNull(field);


           field = name.GetField(CommonDTD.NAME_MIDDLENAME);
           Assert.IsNotNull(field);
           Assert.IsNull(field.Value);

           see = copy.GetSIFExtendedElement("FOO");
           field = see.GetField(GlobalDTD.SIF_EXTENDEDELEMENT);
           Assert.IsNotNull(field);
           Assert.IsNull(field.Value);

           field = copy.GetField(StudentDTD.STUDENTPERSONAL_LOCALID);
           Assert.IsNotNull(field);
           Assert.IsNull(field.Value);

           

       }

       [Test]
       public void TestWriteXSINillMultiple()
       {
           SIF_Data data = new SIF_Data();

           for (int a = 0; a < 3; a++)
           {
               StudentPersonal sp = new StudentPersonal();
               sp.RefId = Adk.MakeGuid();
               sp.StateProvinceId = "\u06DE55889";
               sp.LocalId = "987987987987987";
               Name name = new Name( NameType.LEGAL, "Johnson", "Steve" );
               sp.Name = name;
               name.SetField( CommonDTD.NAME_TYPE, new SifString( null ) );
               name.SetField( CommonDTD.NAME_MIDDLENAME, new SifString( null ) );

               SIF_ExtendedElement see = new SIF_ExtendedElement( "FOO", null );
               see.SetField( GlobalDTD.SIF_EXTENDEDELEMENT, new SifString( null ) );
               see.XsiType = "Integer";
               sp.SIFExtendedElementsContainer.Add( see );

               sp.SetField( StudentDTD.STUDENTPERSONAL_LOCALID, new SifString( null ) );
               data.AddChild( sp );
           }


           
           SIF_Data data2 = (SIF_Data)AdkObjectParseHelper.WriteParseAndReturn(data, SifVersion.LATEST, null, true);

           foreach (SifElement child in data2.GetChildList())
           {
               StudentPersonal copy = (StudentPersonal) child;
               Name name = copy.Name;
               Assert.IsNull( name.Type );
               Assert.IsNull( name.MiddleName );
               Assert.IsNotNull( name.FirstName );
               Assert.IsNotNull( name.LastName );

               // Attributes cannot be represented using xs nil
               SimpleField field = name.GetField( CommonDTD.NAME_TYPE );
               Assert.IsNull( field );


               field = name.GetField( CommonDTD.NAME_MIDDLENAME );
               Assert.IsNotNull( field );
               Assert.IsNull( field.Value );

               SIF_ExtendedElement see = copy.GetSIFExtendedElement( "FOO" );
               field = see.GetField( GlobalDTD.SIF_EXTENDEDELEMENT );
               Assert.IsNotNull( field );
               Assert.IsNull( field.Value );

               field = copy.GetField( StudentDTD.STUDENTPERSONAL_LOCALID );
               Assert.IsNotNull( field );
               Assert.IsNull( field.Value );
           }
       }


       [Test]
       public void TestWriteXSIType()
       {
           StudentPersonal sp = new StudentPersonal();
           sp.RefId = Adk.MakeGuid();
           sp.StateProvinceId = "\u06DE55889";
           sp.LocalId = "987987987987987";


           SIF_ExtendedElement see = new SIF_ExtendedElement( "FOO", "BAR" );
           see.XsiType = "Integer";
           sp.SIFExtendedElementsContainer.Add( see );

           Console.WriteLine( sp.ToXml() );

           StudentPersonal copy =
               (StudentPersonal) AdkObjectParseHelper.WriteParseAndReturn( sp, SifVersion.LATEST, null, true );

           see = copy.SIFExtendedElements[0];

           Assert.IsNotNull( see );
           Assert.AreEqual( "Integer", see.XsiType );


       }
   }
}
