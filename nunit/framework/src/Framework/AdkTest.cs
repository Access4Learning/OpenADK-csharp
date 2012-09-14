using OpenADK.Library;
using NUnit.Framework;
using OpenADK.Library.us;

namespace Library.UnitTesting.Framework
{
    /// <summary>
    /// Summary description for AdkTest.
    /// </summary>
    public class AdkTest
    {
        [SetUp]
        public virtual void setUp()
        {
            if (! Adk.Initialized)
            {
                Adk.Initialize(SifVersion.LATEST, SIFVariant.SIF_US, (int)SdoLibraryType.All);
            }
            Adk.SifVersion = SifVersion.LATEST;
        }
    }
}