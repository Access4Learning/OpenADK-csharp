using System;
using System.Collections.Generic;
using System.Text;
using Edustructures.SifWorks;
using Edustructures.SifWorks.Infra;
using SIFWorks.UnitTesting.Framework;

using NUnit.Framework;

namespace SIFWorks.NUnit.Core.infra
{

   //package com.edustructures.adkjunit.infra;

   //import com.edustructures.adkjunit.ADKObjectParseHelper;
   //import com.edustructures.sifworks.ADK;
   //import com.edustructures.sifworks.ADKException;
   //import com.edustructures.sifworks.SIFDTD;
   //import com.edustructures.sifworks.SIFVersion;
   //import com.edustructures.sifworks.infra.SIF_Object;
   //import com.edustructures.sifworks.infra.SIF_Provision;
   //import com.edustructures.sifworks.infra.SIF_PublishAddObjects;

   //import junit.framework.TestCase;
   [TestFixture]
   public class SIF_ProvisionTests
   {

      [SetUp]
      public void setUp()
      {
         // Load only the Common and Infra objects
         Adk.Initialize(SifVersion.LATEST, SdoLibraryType.None);
      }
      [Test]
      public void testSIF_Provision010()
      {
         SIF_Provision prov = new SIF_Provision();

         SIF_PublishAddObjects spao = new SIF_PublishAddObjects();
         spao.Add(new SIF_Object("Authentication"));
         prov.SIF_PublishAddObjects = spao;


         prov = (SIF_Provision)AdkObjectParseHelper.WriteParseAndReturn((SifElement)prov, Adk.SifVersion);
         spao = prov.SIF_PublishAddObjects;
         Assertion.AssertNotNull("SIF_PublishAddObjects", spao);
         Assertion.AssertEquals("spao child count", 1, spao.ChildCount);


         SIF_Object so = (SIF_Object)spao.GetChild(InfraDTD.SIF_PUBLISHADDOBJECTS_SIF_OBJECT, new string[] { "Authentication" });
         Assertion.AssertNotNull("SIF_Object", so);

      }

   }//end class
}//end namespace