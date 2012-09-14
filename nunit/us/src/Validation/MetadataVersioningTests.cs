using System;
using System.Collections.Generic;
using System.Text;
using Edustructures.SifWorks;
using Edustructures.SifWorks.Library;
using NUnit.Framework;

namespace SIFWorks.Nunit.US.Versioning
{
    [TestFixture]
    public class MetadataVersioningTests
    {
        [SetUp]
        public void setUp()
        {
            Adk.Initialize();
        }

        [Test]
        public void testTagNameChanges()
        {
            // Create a Transaction element and show its version-dependent tag name
            Transaction trans = new Transaction();

            assertTransactionInfo(trans);

            TransactionList tl = new TransactionList();
            tl.Add(trans);
            assertTransactionInfo(trans);

        }

        private void assertTransactionInfo(Transaction trans)
        {
            Assert.AreEqual("Transaction", trans.ElementDef.Name, "version-independent name");

            // assert the tag name for SIF 1.1: “CircTx”
            Assert.AreEqual("CircTx", trans.ElementDef.Tag(SifVersion.SIF11), "SIF 1.1 Should be CircTx");

            // assert the tag name for SIF 1.5: “CircTx”
            Assert.AreEqual("CircTx", trans.ElementDef.Tag(SifVersion.Parse("1.5")), "SIF 1.5 Should be CircTx");

            // assert the tag name for SIF 1.5r1: “CircTx”
            Assert.AreEqual("CircTx", trans.ElementDef.Tag(SifVersion.SIF15r1), "SIF 1.5r1 Should be CircTx");

            // assert the tag name for SIF 2.0: “Transaction”
            Assert.AreEqual("Transaction", trans.ElementDef.Tag(SifVersion.SIF20), "SIF 2.0 Should be Transaction");

            // assert the tag name for SIF 2.1: “Transaction”
            Assert.AreEqual("Transaction", trans.ElementDef.Tag(SifVersion.SIF21), "SIF 2.1 Should be Transaction");

        }
    }
}
