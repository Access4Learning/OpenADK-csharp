using System;
using OpenADK.Library;
using OpenADK.Library.us.Infrastructure;
using OpenADK.Library.us.Common;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;
using OpenADK.Library.us.Student;
using OpenADK.Library.Global;
using OpenADK.Library.Tools.XPath;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Library
{
    [TestFixture]
    public class SifResponseSenderTests : InMemoryProtocolTest
    {
        /**
	 * Tests basic support for SifResponseSender and that the SIF_Responses
	 * are actually sent
	 * @throws Exception
	 */
        [Test]
        public void testSifResponseSender()
        {
            MessageDispatcher testDispatcher = new MessageDispatcher( Zone );
            Zone.SetDispatcher( testDispatcher );
            Zone.Connect( ProvisioningFlags.None );
            InMemoryProtocolHandler testProto = (InMemoryProtocolHandler) Zone.ProtocolHandler;
            testProto.clear();

            // Send a single SIF_Response with a small Authentication object

            String SifRequestMsgId = Adk.MakeGuid();
            String sourceId = "TEST_SOURCEID";
            SifVersion testVersion = SifVersion.LATEST;
            int maxBufferSize = int.MaxValue;
            IElementDef[] testRestrictions = new IElementDef[] {InfrastructureDTD.AUTHENTICATION_REFID};

            SifResponseSender srs = new SifResponseSender();
            srs.Open( Zone, SifRequestMsgId, sourceId, testVersion, maxBufferSize, testRestrictions );
            srs.Write( new Authentication( Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.EMPLOYEEPERSONAL ) );
            srs.Close();

            // Retrieve the SIF_Response message off the protocol handler and asssert the results
            SIF_Response response = (SIF_Response) testProto.readMsg();

            Assert.AreEqual( SifRequestMsgId, response.SIF_RequestMsgId );
            Assert.AreEqual( 1, response.SIF_PacketNumber.Value );
            Assert.AreEqual( "No", response.SIF_MorePackets );

            SIF_Header header = response.SIF_Header;
            Assert.AreEqual( sourceId, header.SIF_DestinationId );

            SifElement responseObject = response.SIF_ObjectData.GetChildList()[0];
            Assert.IsNotNull( responseObject );
        }


        /**
	 * Tests basic support for SifResponseSender and that the SIF_Responses
	 * are actually sent
	 * @throws Exception
	 */
        [Test]
        public void testSifResponseSenderMultiplePackets()
        {
            MessageDispatcher testDispatcher = new MessageDispatcher( Zone );
            Zone.Properties.OneObjectPerResponse = true;
            Zone.SetDispatcher( testDispatcher );
            Zone.Connect( ProvisioningFlags.None );
            InMemoryProtocolHandler testProto = (InMemoryProtocolHandler) Zone.ProtocolHandler;
            testProto.clear();

            // Send a single SIF_Response with a small Authentication object

            String SifRequestMsgId = Adk.MakeGuid();
            String sourceId = "TEST_SOURCEID";
            SifVersion testVersion = SifVersion.LATEST;
            int maxBufferSize = int.MaxValue;
            IElementDef[] testRestrictions = new IElementDef[] {InfrastructureDTD.AUTHENTICATION_REFID};

            SifResponseSender srs = new SifResponseSender();
            srs.Open( Zone, SifRequestMsgId, sourceId, testVersion, maxBufferSize, testRestrictions );
            srs.Write( new Authentication( Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.EMPLOYEEPERSONAL ) );
            srs.Write( new Authentication( Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.EMPLOYEEPERSONAL ) );
            srs.Write( new Authentication( Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.EMPLOYEEPERSONAL ) );
            srs.Write( new Authentication( Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.EMPLOYEEPERSONAL ) );
            srs.Write( new Authentication( Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.EMPLOYEEPERSONAL ) );
            srs.Close();


            for ( int x = 0; x < 5; x ++ )
            {
                // Retrieve the SIF_Response message off the protocol handler and asssert the results
                SIF_Response response = (SIF_Response) testProto.readMsg();

                Assert.AreEqual( SifRequestMsgId, response.SIF_RequestMsgId );
                Assert.AreEqual( x + 1, response.SIF_PacketNumber.Value );
                if ( x == 4 )
                {
                    Assert.AreEqual( "No", response.SIF_MorePackets );
                }
                else
                {
                    Assert.AreEqual( "Yes", response.SIF_MorePackets );
                }

                SIF_Header header = response.SIF_Header;
                Assert.AreEqual( sourceId, header.SIF_DestinationId );

                SifElement responseObject = response.SIF_ObjectData.GetChildList()[0];
                Assert.IsNotNull( responseObject );
            }
        }


        /**
	 * Tests basic support for SifResponseSender when an error packet is set
	 * @throws Exception
	 */
        [Test]
        public void testSifResponseSenderError()
        {
            MessageDispatcher testDispatcher = new MessageDispatcher( Zone );
            Zone.SetDispatcher( testDispatcher );
            Zone.Connect( ProvisioningFlags.None );
            InMemoryProtocolHandler testProto = (InMemoryProtocolHandler) Zone.ProtocolHandler;
            testProto.clear();

            // Send a single SIF_Response with a small Authentication object

            String SifRequestMsgId = Adk.MakeGuid();
            String sourceId = "TEST_SOURCEID";
            SifVersion testVersion = SifVersion.LATEST;
            int maxBufferSize = int.MaxValue;
            SIF_Error error =
                new SIF_Error( SifErrorCategoryCode.Generic, SifErrorCodes.GENERIC_GENERIC_ERROR_1, "ERROR", "EXT_ERROR" );
            IElementDef[] testRestrictions = new IElementDef[] {InfrastructureDTD.AUTHENTICATION_REFID};

            SifResponseSender srs = new SifResponseSender();
            srs.Open( Zone, SifRequestMsgId, sourceId, testVersion, maxBufferSize, testRestrictions );
            srs.Write( new Authentication( Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.EMPLOYEEPERSONAL ) );
            srs.Write( error );
            srs.Close();

            // Retrieve the SIF_Response message off the protocol handler and asssert the results
            SIF_Response response = (SIF_Response) testProto.readMsg();

            Assert.AreEqual( SifRequestMsgId, response.SIF_RequestMsgId );
            Assert.AreEqual( 1, response.SIF_PacketNumber.Value );
            Assert.AreEqual( "Yes", response.SIF_MorePackets );

            SIF_Header header = response.SIF_Header;
            Assert.AreEqual( sourceId, header.SIF_DestinationId );

            SifElement responseObject = response.SIF_ObjectData.GetChildList()[0];
            Assert.IsNotNull( responseObject );

            // now test the error packet
            response = (SIF_Response) testProto.readMsg();

            Assert.AreEqual( SifRequestMsgId, response.SIF_RequestMsgId );
            Assert.AreEqual( 2, response.SIF_PacketNumber.Value );
            Assert.AreEqual( "No", response.SIF_MorePackets );

            header = response.SIF_Header;
            Assert.AreEqual( sourceId, header.SIF_DestinationId );

            Assert.IsNull( response.SIF_ObjectData );
            SIF_Error respError = response.SIF_Error;
            Assert.IsNotNull( respError );
            Assert.AreEqual( 12, respError.SIF_Category.Value );
            Assert.AreEqual( 1, respError.SIF_Code.Value );
            Assert.AreEqual( "ERROR", respError.SIF_Desc );
            Assert.AreEqual( "EXT_ERROR", respError.SIF_ExtendedDesc );
        }

        /**
	 * Tests basic support for SifResponseSender and that the SIF_Responses
	 * are actually sent
	 * @throws Exception
	 */
        [Test]
        public void testSetPacketNumberAndMorePackets()
        {

            MessageDispatcher testDispatcher = new MessageDispatcher(this.Zone);
            this.Zone.SetDispatcher(testDispatcher);
            this.Zone.Connect( ProvisioningFlags.None );
            InMemoryProtocolHandler testProto = (InMemoryProtocolHandler)this.Zone.ProtocolHandler;
            testProto.clear();

            // Send a single SIF_Response with a small Authentication object

            String SifRequestMsgId = Adk.MakeGuid();
            String sourceId = "TEST_SOURCEID";
            SifVersion testVersion = SifVersion.LATEST;
            int maxBufferSize = int.MaxValue;
            int packetNumber = 999;
            YesNo morePacketsValue = YesNo.YES;
            IElementDef[] testRestrictions = new IElementDef[] { InfrastructureDTD.AUTHENTICATION_REFID };

            SifResponseSender srs = new SifResponseSender();
            srs.Open(this.Zone, SifRequestMsgId, sourceId, testVersion, maxBufferSize, testRestrictions);

            srs.SIF_PacketNumber = packetNumber;
            srs.SIF_MorePackets = morePacketsValue;

            // Assert the values of the properties set before writing
            Assert.AreEqual(packetNumber, srs.SIF_PacketNumber);
            Assert.AreEqual(morePacketsValue, srs.SIF_MorePackets);

            srs.Write(new Authentication(Adk.MakeGuid(), Adk.MakeGuid(), AuthSifRefIdType.EMPLOYEEPERSONAL));
            srs.Close();

            // Assert the values of the properties set after writing
            Assert.AreEqual(packetNumber, srs.SIF_PacketNumber);
            Assert.AreEqual(morePacketsValue, srs.SIF_MorePackets);

            // Retrieve the SIF_Response message off the protocol handler and asssert the results
            SIF_Response response = (SIF_Response)testProto.readMsg();

            Assert.AreEqual(SifRequestMsgId, response.SIF_RequestMsgId);
            Assert.AreEqual(packetNumber, response.SIF_PacketNumber.Value);
            Assert.AreEqual(morePacketsValue.ToString(), response.SIF_MorePackets);

            SIF_Header header = response.SIF_Header;
            Assert.AreEqual(sourceId, header.SIF_DestinationId);

            SifElement responseObject = response.SIF_ObjectData.GetChildList()[0];
            Assert.IsNotNull(responseObject);
        }

        [Test]
        public void testSifResponseSender010()
        {
            string queryStr =
                @"<SIF_Query>
                                         <SIF_QueryObject ObjectName='SectionInfo'>
                                            <SIF_Element>@RefId</SIF_Element>
                                            <SIF_Element>@SchoolCourseInfoRefId</SIF_Element>
                                            <SIF_Element>@SchoolYear</SIF_Element>
                                            <SIF_Element>LocalId</SIF_Element>
                                            <SIF_Element>ScheduleInfoList/ScheduleInfo/@TermInfoRefId</SIF_Element>
                                            <SIF_Element>Description</SIF_Element>
                                            <SIF_Element>LanguageOfInstruction</SIF_Element>
                                            <SIF_Element>LanguageOfInstruction/Code</SIF_Element>
                                         </SIF_QueryObject>
                                      </SIF_Query>";

            string sectionInfoStr =
                @"<SectionInfo RefId='D9C9889878144863B190C7D3428D7953' SchoolCourseInfoRefId='587F89D23EDD4761A59C04BA0D39E8D9' SchoolYear='2008'>
                                                  <LocalId>1</LocalId>
                                                  <Description>section 19</Description>
                                                  <ScheduleInfoList>
                                                    <ScheduleInfo TermInfoRefId='0D8165B1ADB34780BD1DFF9E38A7B935'>
                                                      <TeacherList>
                                                        <StaffPersonalRefId>F9D3916707634682B84C530BCF96B5CA</StaffPersonalRefId>
                                                      </TeacherList>
                                                      <SectionRoomList>
                                                        <RoomInfoRefId>EED167D761CD493EA94A875F56ABB0CB</RoomInfoRefId>
                                                      </SectionRoomList>
                                                      <MeetingTimeList>
                                                        <MeetingTime>
                                                          <TimetableDay>R</TimetableDay>
                                                          <TimetablePeriod>6</TimetablePeriod>
                                                        </MeetingTime>
                                                        <MeetingTime>
                                                          <TimetableDay>F</TimetableDay>
                                                          <TimetablePeriod>6</TimetablePeriod>
                                                        </MeetingTime>
                                                        <MeetingTime>
                                                          <TimetableDay>W</TimetableDay>
                                                          <TimetablePeriod>6</TimetablePeriod>
                                                        </MeetingTime>
                                                        <MeetingTime>
                                                          <TimetableDay>M</TimetableDay>
                                                          <TimetablePeriod>6</TimetablePeriod>
                                                        </MeetingTime>
                                                        <MeetingTime>
                                                          <TimetableDay>T</TimetableDay>
                                                          <TimetablePeriod>6</TimetablePeriod>
                                                        </MeetingTime>
                                                      </MeetingTimeList>
                                                    </ScheduleInfo>
                                                  </ScheduleInfoList>
                                                  <MediumOfInstruction><Code>0605</Code></MediumOfInstruction>
                                                  <LanguageOfInstruction><Code>eng</Code></LanguageOfInstruction>
                                                  <SummerSchool>No</SummerSchool>
                                                </SectionInfo>";


            SifParser parser = SifParser.NewInstance();
            SIF_Query sifquery = (SIF_Query) parser.Parse( queryStr );
            SectionInfo section = (SectionInfo) parser.Parse( sectionInfoStr );
            Query query = new Query( sifquery );

            String SifRequestMsgId = Adk.MakeGuid();
            String sourceId = "TEST_SOURCEID";
            SifVersion testVersion = SifVersion.LATEST;
            int maxBufferSize = int.MaxValue;

            MessageDispatcher testDispatcher = new MessageDispatcher(Zone);
            Zone.SetDispatcher(testDispatcher);
            Zone.Connect(ProvisioningFlags.None);
            InMemoryProtocolHandler testProto = (InMemoryProtocolHandler)Zone.ProtocolHandler;
            testProto.clear();

            SifResponseSender srs = new SifResponseSender();
            srs.Open(Zone, SifRequestMsgId, sourceId, testVersion, maxBufferSize, query );
            srs.Write( section );
            srs.Close();

            // Retrieve the SIF_Response message off the protocol handler and asssert the results
            SIF_Response response = (SIF_Response)testProto.readMsg();

            Assert.AreEqual(SifRequestMsgId, response.SIF_RequestMsgId);
            Assert.AreEqual(1, response.SIF_PacketNumber.Value);
            Assert.AreEqual("No", response.SIF_MorePackets);

            SIF_Header header = response.SIF_Header;
            Assert.AreEqual(sourceId, header.SIF_DestinationId);

            SifDataObject responseObject = (SifDataObject)response.SIF_ObjectData.GetChildList()[0];
            Assert.IsNotNull(responseObject);

            Console.Out.WriteLine( responseObject.ToXml() );

            SifXPathContext context = SifXPathContext.NewSIFContext( responseObject );

            foreach( ElementRef reference in query.FieldRestrictionRefs )
            {
                Element found = context.GetElementOrAttribute( reference.XPath );
                Assert.IsNotNull( found, reference.XPath );
            }

    
            Element sectionInfoList =
                responseObject.GetElementOrAttribute( "ScheduleInfoList/ScheduleInfo/SectionInfoList" );
            Assert.IsNull( sectionInfoList );



        }
    }
}