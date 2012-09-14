using System;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Common
{
    [TestFixture]
    public class PartialDateTests : AdkTest
    {
        [Test]
        public void testPartialDate005()
        {
            PartialDateType date = new PartialDateType("1999-12-25");
            assertPartialDate(date, 1999, 12, 25, "1999-12-25");
        }

        [Test]
        public void testPartialDate006()
        {
            PartialDateType date = new PartialDateType("2006-06-01+13:00");
            assertPartialDate(date, 2006, 6, 1, "2006-06-01+13:00");
        }

        [Test]
        public void testPartialDate007()
        {
            PartialDateType date = new PartialDateType("2006-06-01Z");
            assertPartialDate(date, 2006, 6, 1, "2006-06-01Z");
        }

        //[Test]
        public void assertPartialDate(PartialDateType date, int year, int month, int day, String lexicalValue)
        {
            Assert.AreEqual(year, date.Year.Value);
            Assert.AreEqual(month, date.Month.Value);
            Assert.AreEqual(day, date.Day.Value);

            DateTime cal = date.Date;
            Assert.IsNotNull(cal, "PartialDateType.getDate()");

            Assert.AreEqual(year, cal.Year);
            Assert.AreEqual(month, cal.Month);
            Assert.AreEqual(day, cal.Day);

            Assert.AreEqual(date.Value, lexicalValue);
            Assert.AreEqual(date.TextValue, lexicalValue);

            Assert.AreEqual(PartialDateType.DateType.Date, date.DataType);
        }

        [Test]
        public void testPartialDate010()
        {
            PartialDateType date = new PartialDateType(1999);
            Assert.AreEqual((int) date.Year, 1999);
            Assert.AreEqual(PartialDateType.DateType.GYear, date.DataType);
        }

        [Test]
        public void testPartialDate011()
        {
            PartialDateType date = new PartialDateType("1999");
            Assert.AreEqual((int) date.Year, 1999);
            Assert.AreEqual(date.Value, "1999");
            Assert.AreEqual(date.TextValue, "1999");
            Assert.AreEqual(PartialDateType.DateType.GYear, date.DataType);
        }

        [Test]
        public void testPartialDate014()
        {
            PartialDateType date = new PartialDateType(1999);
            Assert.AreEqual((int) date.Year, 1999);
            Assert.AreEqual(date.Value, "1999");
            Assert.AreEqual(date.TextValue, "1999");
            Assert.AreEqual(PartialDateType.DateType.GYear, date.DataType);
        }

        [Test]
        public void testPartialDate012()
        {
            PartialDateType date = new PartialDateType("1999Z");
            Assert.AreEqual(1999, date.Year);
            Assert.AreEqual("1999Z", date.Value);
            Assert.AreEqual("1999Z", date.TextValue);
            Assert.AreEqual(PartialDateType.DateType.GYear, date.DataType);
        }

        [Test]
        public void testPartialDate013()
        {
            PartialDateType date = new PartialDateType("1999-06:00");
            Assert.AreEqual(1999, date.Year);
            Assert.AreEqual("1999-06:00", date.Value);
            Assert.AreEqual("1999-06:00", date.TextValue);
            Assert.AreEqual(PartialDateType.DateType.GYear, date.DataType);
        }

        [Test]
        public void testPartialDate020()
        {
            PartialDateType date = new PartialDateType(1999, 12);
            Assert.AreEqual(1999, date.Year);
            Assert.AreEqual(12, date.Month);
            Assert.AreEqual("1999-12", date.Value);
            Assert.AreEqual("1999-12", date.TextValue);
            Assert.AreEqual(PartialDateType.DateType.GYearMonth, date.DataType);
        }

        [Test]
        public void testPartialDate021()
        {
            PartialDateType date = new PartialDateType("1999-12Z");
            Assert.AreEqual(1999, (int) date.Year);
            Assert.AreEqual(12, (int) date.Month);
            Assert.AreEqual("1999-12Z", date.Value);
            Assert.AreEqual("1999-12Z", date.TextValue);
            Assert.AreEqual(PartialDateType.DateType.GYearMonth, date.DataType);
        }

        [Test]
        public void testPartialDate022()
        {
            PartialDateType date = new PartialDateType("0010-12-06:00");
            Assert.AreEqual(10, (int) date.Year);
            Assert.AreEqual(12, (int) date.Month);
            Assert.IsNull(date.Day);
            Assert.AreEqual("0010-12-06:00", date.Value);
            Assert.AreEqual("0010-12-06:00", date.TextValue);
            Assert.AreEqual(PartialDateType.DateType.GYearMonth, date.DataType);
        }

        [Test]
        public void testPartialDate030()
        {
            PartialDateType date = new PartialDateType(10, 12, 25);
            assertPartialDate(date, 10, 12, 25, "0010-12-25");
        }

        [Test]
        public void testPartialDate050()
        {
            PartialDateType date = new PartialDateType(1999, 12, 25);
            assertPartialDate(date, 1999, 12, 25, "1999-12-25");

            date.TextValue = "1999-12-06:00";
            Assert.AreEqual(1999, (int) date.Year);
            Assert.AreEqual(12, (int) date.Month);
            Assert.IsNull(date.Day);
            Assert.AreEqual(date.Value, "1999-12-06:00");
            Assert.AreEqual(date.TextValue, "1999-12-06:00");

            date.TextValue = "2007-06-01";
            assertPartialDate(date, 2007, 06, 01, "2007-06-01");

            date.TextValue = "2020-06-05Z";
            assertPartialDate(date, 2020, 06, 05, "2020-06-05Z");
        }

        [Test]
        public void testPartialDate040()
        {
            DateTime? c = Adk.Dtd.GetFormatter(SifVersion.SIF20).ToDate("1999-12-25");
            PartialDateType date = new PartialDateType(c);
            assertPartialDate(date, 1999, 12, 25, "1999-12-25");
        }
    }
}