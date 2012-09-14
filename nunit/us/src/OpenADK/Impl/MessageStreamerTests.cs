using System;
using System.IO;
using System.Text;
using OpenADK.Library;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;
using NUnit.Framework;
using Library.Nunit.US;

namespace Library.Nunit.US.Impl
{
    /// <summary>
    /// Summary description for MessageStreamerTests.
    /// </summary>
    [TestFixture]
    public class MessageStreamerTests : UsAdkTest
    {
        [Test]
        public void SinglePayloadTest()
        {
            using (Stream studentStream = GetResourceStream("StudentPersonal.xml"))
            {
                // Console.WriteLine("Payload Length: {0}", studentStream.Length );
                RunLengthTest(new Stream[] {studentStream}, false, false, SifVersion.SIF20r1);
                studentStream.Close();
            }
        }

        [Test]
        public void MultiplePayloadTest()
        {
            Stream studentStream = GetResourceStream("StudentPersonal.xml");
            Stream studentStream2 = GetResourceStream("StudentPersonal_Small.xml");
            // Console.WriteLine("Payload Lengths: {0},{1}", studentStream.Length, studentStream2.Length );
            RunLengthTest(new Stream[] {studentStream, studentStream2}, false, false, SifVersion.SIF20r1);
        }


        [Test]
        public void SinglePayloadError()
        {
            Stream errorStream = GetResourceStream("SIF_Error.xml");
            // Console.WriteLine("Payload Lengths: {0}", errorStream.Length );
            RunLengthTest(new Stream[] { errorStream }, true, true, SifVersion.SIF20r1);
        }


        [Test]
        public void SinglePayloadNormal()
        {
            Stream errorStream = GetResourceStream("StudentPersonal.xml");
            // Console.WriteLine("Payload Lengths: {0}", errorStream.Length );
            RunLengthTest(new Stream[] { errorStream }, false, false, SifVersion.SIF20r1);
        }


        [Test]
        public void EmptyPayload()
        {
            Stream emptyStream = GetResourceStream("EmptyResponse.pkt");
            RunLengthTest(new Stream[] { emptyStream }, false, false, SifVersion.SIF20r1);
        }


        private void RunLengthTest(Stream[] payloads, bool error, bool replace, SifVersion version)
        {
            // This test loosely emulates what ResponseDelivery does when it builds up the MessageStreamer class
            //  Prepare SIF_Response
            SIF_Response rsp = new SIF_Response();
            rsp.SIF_MorePackets = "No";
            rsp.SIF_RequestMsgId = "12345123451234512345";
            rsp.SIF_PacketNumber = 1;

            rsp.SifVersion = version;

            //  Write an empty "<SIF_ObjectData> </SIFObjectData>" for the
            //  MessageStreamer to fill in. If this is an errorPacket, the empty
            //  element is required per SIF Specifications.
            SIF_ObjectData placeholder = new SIF_ObjectData();
            placeholder.TextValue = " ";
            rsp.SIF_ObjectData = placeholder;

            if (error)
            {
                SIF_Error err = new SIF_Error();
                err.TextValue = " ";
                rsp.SIF_Error = err;
            }


            //  Assign values to message header - this is usually done by
            //  MessageDispatcher.send() but because we're preparing a SIF_Response
            //  output stream we need to do it manually
            SIF_Header hdr = rsp.Header;
            hdr.SIF_Timestamp = DateTime.Now;
            hdr.SIF_MsgId = SifFormatter.GuidToSifRefID(Guid.NewGuid());
            hdr.SIF_SourceId = "UnitTest";
            hdr.SIF_DestinationId = "123451234512345";

            //  Write SIF_Response -- without its SIF_ObjectData payload -- to a buffer
            using (MemoryStream envelope = new MemoryStream())
            {
                SifWriter writer = new SifWriter(envelope);
                writer.Write(rsp);
                writer.Flush();

                envelope.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader( envelope, Encoding.UTF8 );
                String envelopeStr = reader.ReadToEnd();
                Console.Out.WriteLine( envelopeStr );

                envelope.Seek(0, SeekOrigin.Begin);

                using (
                    MessageStreamer ms =
                        new MessageStreamer(envelope, payloads, error ? "<SIF_Error>" : "<SIF_ObjectData>", replace))
                {
                    AssertMessageStreamer(ms, version);
                }
                envelope.Close();
            }
        }

        private void AssertMessageStreamer(MessageStreamer streamer, SifVersion version)
        {
            long length = streamer.Length;
            MemoryStream ms = new MemoryStream();
            streamer.CopyTo(ms);

            // Copy to a string for debugging purposes
            Console.WriteLine("********************************************************************");
            string data = Encoding.UTF8.GetString(ms.ToArray());
            Console.WriteLine("RawLength:{0}, Text Length:{1}", ms.Length, data.Length);
            Console.WriteLine(data);

            Assert.AreEqual(length, ms.Length, "Length property and final length are not the same.");


            // Try parsing the final stream to see if it is a valid message
            SifParser parser = SifParser.NewInstance();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(ms);
            SifElement o = parser.Parse(reader, null, SifParserFlags.None, version);

            Assert.IsNotNull(o);
        }
    }
}