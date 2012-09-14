using System;
using System.Collections;
using System.Data;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.Queries;
using NUnit.Framework;

namespace Library.Nunit.US.Tools.Queries
{
    /// <summary>
    /// Summary description for SQLQueryFormatterTests.
    /// </summary>
    [TestFixture]
    public class SQLQueryFormatterTests
    {
        [SetUp]
        public void Setup()
        {
            Adk.Initialize();
        }

        [Test]
        public void testSQLQueryFormatter010()
        {
            Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition(CommonDTD.NAME_FIRSTNAME, ComparisonOperators.EQ, "Johnny");

            IDictionary fields = new Hashtable();
            fields[CommonDTD.NAME_FIRSTNAME] = new SQLField("vchFirstName", DbType.String);

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            String sql = formatter.Format(q, fields);

            Assert.AreEqual("( vchFirstName = 'Johnny' )", sql, "Query format");
        }

        [Test]
        public void testSQLQueryFormatter020()
        {
           Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition(CommonDTD.NAME_FIRSTNAME, ComparisonOperators.EQ, "Johnny");

            IDictionary fields = new Hashtable();

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            try
            {
                String sql = formatter.Format(q, fields);
            }
            catch (QueryFormatterException)
            {
                // Expected because the map doesn't have an entry for the query condition
                return;
            }

            Assert.Fail("QueryFormatterException should have been thrown");
        }


        [Test]
        public void testSQLQueryFormatter030()
        {
           Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition(CommonDTD.NAME_FIRSTNAME, ComparisonOperators.EQ, "Johnny");

            IDictionary fields = new Hashtable();

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            String sql = formatter.Format(q, fields, false);
            Assert.AreEqual("( 1=1 )", sql, "Query format");
        }

        [Test]
        public void testSQLQueryFormatter050()
        {
           Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition("Demographics/RaceList/Race/Code", ComparisonOperators.EQ, "1002");

            // Convert the query to XML and back
            Query reparsed = QueryTests.SaveToXMLAndReparse(q, SifVersion.LATEST);

            IDictionary fields = new Hashtable();
            fields["Demographics/RaceList/Race/Code"] =
                new SQLField("Users.vchFirstName{0998=I;0999=A;1000=B;1001=H;1002=W}",
                             DbType.String);

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            String sql = formatter.Format(reparsed, fields);

            Assert.AreEqual( "( Users.vchFirstName = 'W' )", sql, "Query format");
        }


        [Test]
        public void testSQLQueryFormatter060()
        {
            Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition("Name/FirstName", ComparisonOperators.GT, "Sally");

            // Convert the query to XML and back
            Query reparsed = QueryTests.SaveToXMLAndReparse(q, SifVersion.LATEST);

            IDictionary fields = new Hashtable();
            fields["Name/FirstName"] =
                new SQLField("Users.FName", DbType.String);

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            String sql = formatter.Format(reparsed, fields);

            Assert.AreEqual("( Users.FName > 'Sally' )", sql, "Query format");
        }

        [Test]
        public void testSQLQueryFormatter070()
        {
            Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition("Name/FirstName", ComparisonOperators.LT, "Sally");

            // Convert the query to XML and back
            Query reparsed = QueryTests.SaveToXMLAndReparse(q, SifVersion.LATEST);

            IDictionary fields = new Hashtable();
            fields["Name/FirstName"] =
                new SQLField("Users.FName", DbType.String);

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            String sql = formatter.Format(reparsed, fields);

            Assert.AreEqual("( Users.FName < 'Sally' )", sql, "Query format");
        }

        [Test]
        public void testSQLQueryFormatter080()
        {
            Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition("Name/FirstName", ComparisonOperators.NE, "Sally");

            // Convert the query to XML and back
            Query reparsed = QueryTests.SaveToXMLAndReparse(q, SifVersion.LATEST);

            IDictionary fields = new Hashtable();
            fields["Name/FirstName"] =
                new SQLField("Users.FName", DbType.String);

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            String sql = formatter.Format(reparsed, fields);

            Assert.AreEqual("( Users.FName != 'Sally' )", sql, "Query format");
        }

        [Test]
        public void testSQLQueryFormatter090()
        {
            Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition("Name/FirstName", ComparisonOperators.GE, "Sally");

            // Convert the query to XML and back
            Query reparsed = QueryTests.SaveToXMLAndReparse(q, SifVersion.LATEST);

            IDictionary fields = new Hashtable();
            fields["Name/FirstName"] =
                new SQLField("Users.FName", DbType.String);

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            String sql = formatter.Format(reparsed, fields);

            Assert.AreEqual("( Users.FName >= 'Sally' )", sql, "Query format");
        }

        [Test]
        public void testSQLQueryFormatter100()
        {
            Query q = new Query(StudentDTD.STUDENTPERSONAL);
            q.AddCondition("Name/FirstName", ComparisonOperators.LE, "Sally");

            // Convert the query to XML and back
            Query reparsed = QueryTests.SaveToXMLAndReparse(q, SifVersion.LATEST);

            IDictionary fields = new Hashtable();
            fields["Name/FirstName"] =
                new SQLField("Users.FName", DbType.String);

            SQLQueryFormatter formatter = new SQLQueryFormatter();
            String sql = formatter.Format(reparsed, fields);

            Assert.AreEqual("( Users.FName <= 'Sally' )", sql, "Query format");
        }




    }
}