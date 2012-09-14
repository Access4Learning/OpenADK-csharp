using System;
using System.IO;

using NUnit.Framework;
using Edustructures.SifWorks;

namespace Edustructures.SifWorks
{
	/// <summary>
	/// Summary description for AdkTest.
	/// </summary>
	public class AdkTest
	{
		[SetUp]
		public virtual void StartTests()
		{
			if( ! Adk.Initialized )
			{
				Adk.Initialize(SifVersion.LATEST, SdoLibraryType.All);
			}
		}

		public const string RESOURCE_ROOT = "Edustructures.res." ;

		protected Stream GetResourceStream( string shortName, Type testType )
		{
			return testType.Assembly.GetManifestResourceStream( RESOURCE_ROOT + shortName );
		}

	}
}
