using System;
using System.IO;
using OpenADK.Library;
using NUnit.Framework;


namespace Library.Nunit.US
{
    public class UsAdkTest
    {
        [SetUp]
        public virtual void setUp()
        {
            Adk.Initialize();
        }

        protected Stream GetResourceStream(string shortName)
        {
            Type thisType = typeof (UsAdkTest);
            return thisType.Assembly.GetManifestResourceStream(thisType.Namespace + ".res." + shortName);
        }
    }
}