using System;
using System.IO;
using Library.UnitTesting.Framework;

namespace Library.NUnit.Core
{
    public class CoreAdkTest : AdkTest
    {
        protected Stream GetResourceStream(string shortName, Type testType)
        {
            Type thisType = typeof (CoreAdkTest);
            return thisType.Assembly.GetManifestResourceStream(thisType.Namespace + ".res." + shortName);
        }
    }
}