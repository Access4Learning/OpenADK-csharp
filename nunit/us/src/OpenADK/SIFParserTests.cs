using System.IO;
using System.Text;
using OpenADK.Library;
using OpenADK.Library.us.Hrfin;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Reporting;
using OpenADK.Library.us.Student;
using NUnit.Framework;
using Library.Nunit.US;

namespace Library.Nunit.US
{
    /// <summary>
    /// Summary description for SIFParserTests.
    /// </summary>
    [TestFixture]
    public class SIFParserTests : UsAdkTest
    {
      
        #region Embedded Performance Test run the PerfTest target to run

#if PERFTEST

    /// <summary>
    /// Runs a perf test on the SIF Parser by parsing an in-memory stream 5000 times
    /// </summary>
    /// <remarks>
    /// The first time this test was run, the total time averaged 3.3 seconds
    /// After turning WhitespaceHandling to None in the SIFParser, the time went down to 3.2 seconds
    /// </remarks>
		[Test]
		public void PerfTestParsing5000Times()
		{
			// Do one warmup parse . . .

			SifElement element = null;
			using( Stream aStream = this.GetType().Assembly.GetManifestResourceStream( AdkTest.RESOURCE_ROOT + "StudentPersonalResponse_AddForDelete.xml") )
			{
				SifParser parser = new SifParser();
				// Do one warmup parse . . .
				TextReader aReader = new StreamReader( aStream );
				element = parser.Parse( aReader, null, SifParserFlags.None, SifVersion.SIF11 );

				for( int a = 0; a< 5000; a++ )
				{
					aStream.Seek( 0, SeekOrigin.Begin );
					element = parser.Parse( aStream, null, SifParserFlags.None );
				}
				aStream.Close();
			}

			Assert.IsNotNull( element, "SIFElement was not parsed" );
		}

#endif

        #endregion

        [Test]
        public void EmbeddedSIFMessage()
        {
            SifElement element = null;
            using (Stream aStream = GetResourceStream("GetNextMessageResponse.xml"))
            {
                TextReader aReader = new StreamReader(aStream);
                SifParser parser = SifParser.NewInstance();
                element = parser.Parse(aReader, null, SifParserFlags.ExpectInnerEnvelope, SifVersion.SIF11);
                aReader.Close();
                aStream.Close();
            }

            Assert.IsNotNull(element, "SIFElement was not parsed");
            SIF_Ack ack = (SIF_Ack) element;
            SifElement messageElement = ack.SIF_Status.SIF_Data.GetChild("SIF_Message");
            SIF_Event aEvent = (SIF_Event) messageElement.GetChild("SIF_Event");
            SIF_EventObject eventObject = aEvent.SIF_ObjectData.SIF_EventObject;

            Assert.AreEqual("SchoolCourseInfo", eventObject.ObjectName, "Wrong object name");
            Assert.AreEqual("Change", eventObject.Action, "Wrong Action");
            Assert.IsTrue(eventObject.GetChildList()[0] is SchoolCourseInfo, "Wrong object type");
        }

        [Test]
        [ExpectedException( typeof (AdkParsingException), "Unexpected SIF_Message encountered in parsing")]
        public void UnexpectedEmbeddedSIFMessage()
        {
            // this test should throw an exception because we are not passing "ExpectInnerEnvelope" in the 
            // parser flags
            using (Stream aStream = GetResourceStream("GetNextMessageResponse.xml"))
            {
                TextReader aReader = new StreamReader(aStream);
                SifParser parser = SifParser.NewInstance();
                SifElement element = parser.Parse(aReader, null, SifParserFlags.None, SifVersion.SIF11);
                aReader.Close();
                aStream.Close();
            }
        }

        [Test]
        public void TestLooseXmlTypeParsing100()
        {
            string vendorInfo15r1 =
                            @"<VendorInfo RefId='F138FF5017DC11DBAC45A0329DB3F005'>
                               <VendorName>Kleinman &amp; Co.</VendorName>
                               <Send1099 Code='XXX' />
                             </VendorInfo>";

            SifParser parser = SifParser.NewInstance();
            VendorInfo vi = (VendorInfo) parser.Parse( vendorInfo15r1, null, SifParserFlags.None, SifVersion.SIF15r1 );

            Assert.IsNotNull( vi );
            Assert.IsFalse( vi.Send1099.HasValue );
        }

        [Test]
        public void TestLooseXmlTypeParsing101()
        {
            string vendorInfo20r1 =
                            @"<VendorInfo RefId='F138FF5017DC11DBAC45A0329DB3F005'>
                               <VendorName>Kleinman &amp; Co.</VendorName>
                               <Send1099>xxx</Send1099>
                             </VendorInfo>";

            SifParser parser = SifParser.NewInstance();
            VendorInfo vi = (VendorInfo)parser.Parse(vendorInfo20r1, null, SifParserFlags.None, SifVersion.SIF20r1);

            Assert.IsNotNull(vi);
            Assert.IsFalse(vi.Send1099.HasValue);
        }


        [Test]
        public void TestParseReportData()
        {
            SIF_ReportObject reportObject = null;
            using (Stream aStream = GetResourceStream("ReportData.xml"))
            {
                TextReader aReader = new StreamReader(aStream);
                SifParser parser = SifParser.NewInstance();
                reportObject = (SIF_ReportObject) parser.Parse(aReader, null, SifParserFlags.None, SifVersion.SIF20r1);
                aReader.Close();
                aStream.Close();
            }

            Assert.IsNotNull( reportObject );
            Assert.AreEqual( 1, reportObject.ChildCount );

            SifElement reportData = reportObject.GetChildList()[0];
            Assert.AreEqual(2060, reportData.ChildCount);


        }



    }
}