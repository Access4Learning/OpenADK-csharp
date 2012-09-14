using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using OpenADK.Library.Infra;
using NUnit.Framework;
using OpenADK.Library;
using System.Reflection;
using OpenADK.Library.us;
namespace Library.NUnit.Core.Library
{
    [TestFixture]
    public class SerializableTests
    {
        [SetUp]
        public void Setup()
        {
            Adk.Initialize(SifVersion.LATEST, SIFVariant.SIF_US, (int)SdoLibraryType.None);
        }
        /// <summary>
        /// Tests whether any of the Adk's classes which inherit from SifSimpleType cannot be 
        /// serialized.  By default they all should be.
        /// 
        /// </summary>
        [Test]
        public void TestSifSimpleTypes()
        {
            IList<SifSimpleType> originalList = new List<SifSimpleType>();
            originalList.Add( new SifBoolean( true ) );
            originalList.Add( new SifDate( new DateTime( 2007, 12, 1 ) ) );
            originalList.Add( new SifDateTime( new DateTime( 2007, 12, 1 ) ) );
            originalList.Add( new SifDecimal( 10 ) );
            originalList.Add( new SifDuration( new TimeSpan( 1000 ) ) );
            originalList.Add( new SifInt( 5 ) );
            originalList.Add( new SifString( "This is a test" ) );
            originalList.Add( new SifTime( new DateTime( 2007, 12, 1 ) ) );

            //Serialize the list, deserialize it, and assert the results
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize( stream, originalList );

            stream.Seek( 0, SeekOrigin.Begin );
            IList<SifSimpleType> desierializedList = (IList<SifSimpleType>) formatter.Deserialize( stream );

            for( int a = 0; a < originalList.Count; a++ )
            {
                Assert.AreEqual( originalList[a], desierializedList[a], originalList[a].GetType().Name );
            }


        }
    }
}
