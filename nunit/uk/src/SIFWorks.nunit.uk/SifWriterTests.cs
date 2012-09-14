using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenADK.Library.Common;
using OpenADK.Library.Infra;
using OpenADK.Library.Learner;
using OpenADK.Library.School;
using NUnit.Framework;
using Library.UnitTesting.Framework;
using OpenADK.Library;

namespace OpenADK.Library.Nunit.UK
{
    [TestFixture]
    public class SifWriterTests : AdkTest
    {
        [Test]
        public void TestxsiNill_SIFMessagePayload()
        {
            LearnerPersonal lp = new LearnerPersonal();

            // Add a null UPN
            SifString str= new SifString( null );
            lp.SetField( LearnerDTD.LEARNERPERSONAL_UPN, str );

            // Add a null AlertMsg
            AlertMsg msg = new AlertMsg( AlertMsgType.DISCIPLINE, null );
            lp.AlertMsgList = new AlertMsgList( msg );
            msg.SetField( CommonDTD.ALERTMSG, new SifString( null ) );



            SIF_Response sifMessage = new SIF_Response();
            sifMessage.AddChild( lp );


            //  Write the object to a file
            Console.WriteLine("Writing to file...");
            using (Stream fos = File.Open("SifWriterTest.Temp.xml", FileMode.Create, FileAccess.Write))
            {
                SifWriter writer = new SifWriter(fos);
                sifMessage.SetChanged(true);
                writer.Write( sifMessage );
                writer.Flush();
                fos.Close();
            }

            //  Parse the object from the file
            Console.WriteLine("Parsing from file...");
            SifParser p = SifParser.NewInstance();
            using (Stream fis = File.OpenRead("SifWriterTest.Temp.xml"))
            {
                sifMessage = (SIF_Response)p.Parse(fis, null);
            }



            lp = (LearnerPersonal) sifMessage.GetChildList()[0];


            SimpleField upn = lp.GetField( LearnerDTD.LEARNERPERSONAL_UPN );
            Assert.IsNotNull( upn );

            SifString rawValue = (SifString)upn.SifValue;
            Assert.IsNotNull( rawValue );
            Assert.IsNull( rawValue.Value );
            Assert.IsNull( upn.Value );

            AlertMsgList alertMsgs = lp.AlertMsgList;
            Assert.IsNotNull( alertMsgs );
            Assert.IsTrue( alertMsgs.Count == 1 );
            msg = (AlertMsg)alertMsgs.GetChildList()[0];

            Assert.IsNull( msg.Value );
            SifSimpleType msgValue = msg.SifValue;
            Assert.IsNotNull( msgValue );
            Assert.IsNull( msgValue.RawValue );




        }

        [Test]
        public void testmeth()
        {
            String c = GetColumnName( 1, 1 );
            Assert.AreEqual( "A1", c );
            c = GetColumnName(27, 1);
            Assert.AreEqual("AA1", c);

            c = GetColumnName(28, 1);
            Assert.AreEqual("AB1", c);
        }

        private String GetColumnName(int ordinal, int Row)
        {
            if (ordinal < 27)
            {
                return ((char)(ordinal + 'A' - 1)).ToString() + Row.ToString();
            }
            int charIndex = ordinal % 26;
            char c1 = (char)( charIndex + 'A' - 1);
            char c2 = (char)(ordinal - (charIndex * 26) + 'A' - 1);
            return c1.ToString() + c2.ToString() + Row.ToString();
        }

        [Test]
        public void TestxsiNill_SDOObjectXML()
        {
            LearnerPersonal lp = new LearnerPersonal();

            // Add a null UPN
            SifString str = new SifString(null);
            lp.SetField(LearnerDTD.LEARNERPERSONAL_UPN, str);

            // Add a null AlertMsg
            AlertMsg msg = new AlertMsg(AlertMsgType.DISCIPLINE, null);
            lp.AlertMsgList = new AlertMsgList(msg);
            msg.SetField(CommonDTD.ALERTMSG, new SifString(null));



            //  Write the object to a file
            Console.WriteLine("Writing to file...");
            using (Stream fos = File.Open("SifWriterTest.Temp.xml", FileMode.Create, FileAccess.Write))
            {
                SifWriter writer = new SifWriter(fos);
                lp.SetChanged(true);
                writer.Write(lp);
                writer.Flush();
                fos.Close();
            }

            //  Parse the object from the file
            Console.WriteLine("Parsing from file...");
            SifParser p = SifParser.NewInstance();
            using (Stream fis = File.OpenRead("SifWriterTest.Temp.xml"))
            {
                lp = (LearnerPersonal)p.Parse(fis, null);
            }


            SimpleField upn = lp.GetField(LearnerDTD.LEARNERPERSONAL_UPN);
            Assert.IsNotNull(upn);

            SifString rawValue = (SifString)upn.SifValue;
            Assert.IsNotNull(rawValue);
            Assert.IsNull(rawValue.Value);
            Assert.IsNull(upn.Value);

            AlertMsgList alertMsgs = lp.AlertMsgList;
            Assert.IsNotNull(alertMsgs);
            Assert.IsTrue(alertMsgs.Count == 1);
            msg = (AlertMsg)alertMsgs.GetChildList()[0];

            Assert.IsNull(msg.Value);
            SifSimpleType msgValue = msg.SifValue;
            Assert.IsNotNull(msgValue);
            Assert.IsNull(msgValue.RawValue);
        }


        public void TestXsiNill_AllChildrenNil()
        {
            SchoolInfo si = new SchoolInfo();
            AddressableObjectName paon = new AddressableObjectName( );
            paon.Description = "The little white school house";
            paon.StartNumber = "321";
            Address addr = new Address( AddressType.CURRENT, paon );
            GridLocation gl = new GridLocation();
            gl.SetField( CommonDTD.GRIDLOCATION_PROPERTYEASTING, new SifDecimal( null ) );
            gl.SetField( CommonDTD.GRIDLOCATION_PROPERTYNORTHING, new SifDecimal( null ) );
            addr.GridLocation = gl;

            si.AddressList = new AddressList( addr );


            //  Write the object to a file
            Console.WriteLine("Writing to file...");
            using (Stream fos = File.Open("SifWriterTest.Temp.xml", FileMode.Create, FileAccess.Write))
            {
                SifWriter writer = new SifWriter(fos);
                si.SetChanged(true);
                writer.Write(si);
                writer.Flush();
                fos.Close();
            }

            //  Parse the object from the file
            Console.WriteLine("Parsing from file...");
            SifParser p = SifParser.NewInstance();
            using (Stream fis = File.OpenRead("SifWriterTest.Temp.xml"))
            {
                si = (SchoolInfo)p.Parse(fis, null);
            }


            AddressList al = si.AddressList;
            Assert.IsNotNull( al );

            addr = al.ItemAt( 0 ); 
            Assert.IsNotNull( addr );

            gl = addr.GridLocation;
            Assert.IsNotNull( gl );

            Assert.IsNull( gl.PropertyEasting );
            Assert.IsNull(gl.PropertyNorthing );

            SimpleField sf = gl.GetField( CommonDTD.GRIDLOCATION_PROPERTYEASTING );
            Assert.IsNotNull( sf );
            Assert.IsNull( sf.Value );

            sf = gl.GetField(CommonDTD.GRIDLOCATION_PROPERTYNORTHING );
            Assert.IsNotNull(sf);
            Assert.IsNull(sf.Value);

        }

        public void TestXsiNill_AllChildrenNilMultiple()
        {

            SIF_Data data = new SIF_Data();

            for (int a = 0; a < 3; a++)
            {
                SchoolInfo si = new SchoolInfo();
                AddressableObjectName paon = new AddressableObjectName();
                paon.Description = "The little white school house";
                paon.StartNumber = "321";
                Address addr = new Address( AddressType.CURRENT, paon );
                GridLocation gl = new GridLocation();
                gl.SetField( CommonDTD.GRIDLOCATION_PROPERTYEASTING, new SifDecimal( null ) );
                gl.SetField( CommonDTD.GRIDLOCATION_PROPERTYNORTHING, new SifDecimal( null ) );
                addr.GridLocation = gl;
                si.AddressList = new AddressList( addr );

                data.AddChild( si );
            }



            //  Write the object to a file
            Console.WriteLine("Writing to file...");
            using (Stream fos = File.Open("SifWriterTest.Temp.xml", FileMode.Create, FileAccess.Write))
            {
                SifWriter writer = new SifWriter(fos);
                data.SetChanged(true);
                writer.Write(data);
                writer.Flush();
                fos.Close();
            }

            //  Parse the object from the file
            Console.WriteLine("Parsing from file...");
            SifParser p = SifParser.NewInstance();
            using (Stream fis = File.OpenRead("SifWriterTest.Temp.xml"))
            {
                data = (SIF_Data)p.Parse(fis, null);
            }

            foreach ( SchoolInfo si in data.GetChildList() )
            {
                AddressList al = si.AddressList;
                Assert.IsNotNull(al);

                Address addr = al.ItemAt(0);
                Assert.IsNotNull(addr);

                GridLocation gl = addr.GridLocation;
                Assert.IsNotNull(gl);

                Assert.IsNull(gl.PropertyEasting);
                Assert.IsNull(gl.PropertyNorthing);

                SimpleField sf = gl.GetField(CommonDTD.GRIDLOCATION_PROPERTYEASTING);
                Assert.IsNotNull(sf);
                Assert.IsNull(sf.Value);

                sf = gl.GetField(CommonDTD.GRIDLOCATION_PROPERTYNORTHING);
                Assert.IsNotNull(sf);
                Assert.IsNull(sf.Value);
            }

            

        }


    }
}
