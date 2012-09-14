using System;
using System.Xml;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Instr;
using NUnit.Framework;
using Library.UnitTesting.Framework;

namespace Library.Nunit.US.Instr
{
    /// <summary>
    /// Summary description for TestLearningStandardDocument.
    /// </summary>
    [TestFixture]
    public class TestLearningStandardDocument : AdkTest
    {
        [Test]
        public void WriteXml()
        {
            if (Adk.SifVersion < SifVersion.SIF15r1)
            {
                return;
            }
            LearningStandardDocument doc = new LearningStandardDocument();
            doc.RefId = "A5A575C789175101B8E7F08ED123A823";
            doc.Language = "en-us";
            doc.Title = "Washington Essential Academic Learning Requirements";
            doc.Description = "This document addresses high school English Language Arts";
            doc.Source = "State";
            doc.Organizations = new Organizations(new Organization("State of Washington"));
            doc.Authors = new Authors(new Author("McREL"));
            doc.OrganizationContactPoint = "http://www.mcrel.org";
            doc.SubjectAreas = new SubjectAreas(new SubjectArea("10"));
            doc.DocumentStatus = "Adopted";
            doc.DocumentDate = new DateTime(2001, 4, 15);
            doc.LocalAdoptionDate = new DateTime(2002, 1, 6);
            doc.EndOfLifeDate = new DateTime(2003, 4, 15);
            doc.Copyright = new Copyright();
            doc.Copyright.Date = new DateTime(2001, 02, 04);
            doc.Copyright.Holder = "State of Washington";
            doc.GradeLevels = new GradeLevels();
            doc.GradeLevels.AddGradeLevel(GradeLevelCode.C09);
            doc.GradeLevels.AddGradeLevel(GradeLevelCode.C10);
            doc.GradeLevels.AddGradeLevel(GradeLevelCode.C11);
            doc.GradeLevels.AddGradeLevel(GradeLevelCode.C12);
            doc.RepositoryDate = new DateTime(2001, 04, 15);
            doc.LearningStandardItemRefId = "B7D26D789139214A8C7F08EA123A8234";
            doc.RelatedLearningStandards =
                new RelatedLearningStandards(new LearningStandardDocumentRefId("B216162FC98D202E62A64D53C991A25A"));

            string xml = doc.ToXml();
            Console.WriteLine(xml);

            // Mainly, we want to test for the lang element to be written properly
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlAttribute langAttr = xmlDoc.DocumentElement.Attributes["lang", "http://www.w3.org/XML/1998/namespace"];
            Assert.AreEqual("http://www.w3.org/XML/1998/namespace", langAttr.NamespaceURI);

            LearningStandardDocument lsDoc2 = (LearningStandardDocument) AdkObjectParseHelper.runParsingTest(doc, SifVersion.LATEST );
            Assert.AreEqual("en-us", lsDoc2.Language, "xml:lang");
        }
    }
}