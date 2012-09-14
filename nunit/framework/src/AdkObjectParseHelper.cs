using System;
using System.IO;
using System.Text;

using Edustructures.SifWorks;

using NUnit.Framework;

namespace Edustructures.SifWorks
{
	/// <summary>
	/// Summary description for AdkObjectParseHelper.
	/// </summary>
	public sealed class AdkObjectParseHelper
	{
		public static SifDataObject runParsingTest( SifDataObject o )
		{
			SifDataObject o2 = WriteParseAndReturn( o, Adk.SifVersion );

			Assert.IsNotNull( o2, "Object is null after parsing" );
			runAssertions( o, o2 );

			return o2;
		}

		private static void runAssertions( SifDataObject originalObject,
			SifDataObject reparsedObject )
		{
			// run additional assertions by overriding this method
			//  Compare the objects
			Element[][] diffs = originalObject.CompareGraphTo( reparsedObject );
			if (diffs[0].Length == 0)
				Console.WriteLine( "\r\n*** Objects are identical ***\r\n" );
			else
			{
				bool isChanged = false;
				//	Display differences in two columns
				Console.WriteLine();
				Console.WriteLine( pad( "Original Object" ) + " "
						   + pad( "Reparsed Object" ) );
				Console.WriteLine( underline( 50 ) + " " + underline( 50 ) );

				StringBuilder str = new StringBuilder();

				for (int i = 0; i < diffs[0].Length; i++)
				{
					str.Length = 0;
					str.Append( pad( (diffs[0][i] == null ? "" : 
						diffs[0][i].ElementDef.GetSQPPath( Adk.SifVersion )
								+ " ('")
								+ (diffs[0][i] == null ? "" : 
						diffs[0][i].TextValue + "')") ) );
					str.Append( ' ' );
					str.Append( pad( (
								diffs[1][i] == null ? "" : 
								diffs[1][i].ElementDef.GetSQPPath( Adk.SifVersion )
									+ " ('")
									+ (diffs[1][i] == null ? "" : 
								diffs[1][i].TextValue + "')") ) );
					Console.WriteLine( str.ToString() );
					// The Element.compareGraphTo() method either has a bug, or else
					// the behavior is supposed to be this
					// way. ( Eric, please verify ). We only want to fail if the two
					// fields are entirely different.
					String s1 = diffs[0][i] == null ? "" : diffs[0][i].TextValue;
					s1 = s1 == null ? "" : s1;
					String s2 = diffs[1][i] == null ? "" : diffs[1][i].TextValue;
					s2 = s2 == null ? "" : s2;
					if (!s1.Equals( s2 ))
					{
						isChanged = true;
					}

				}
				if (isChanged)
				{

					Assert.Fail( " Original and reparsed object differ, see System.Out for details " );
				}
			}

		}

		private static String underline( int length )
		{
			return new string( '-', length );
		}

		private static String pad( String text )
		{
			return text.PadRight( 50 );
		}

		public static SifDataObject WriteParseAndReturn( SifDataObject o,
			SifVersion version )
		{
			SifDataObject returnVal = null;

			try
			{
				//  Write the object to System.out
				Console.WriteLine( "Writing object : " + o.ObjectTag
						   + " using SifVersion: " + version.ToString() );
				SifWriter echo = new SifWriter( Console.Out );
				echo.Write( o, version );
				o.SetChanged( false );
				echo.Write( o );

				//  Write the object to a file
				Console.WriteLine( "Writing to file..." );
				using(Stream fos = File.Open( "test.xml", FileMode.Create, FileAccess.Write ) )
				{
					SifWriter writer = new SifWriter( fos );
					o.SetChanged( true );
					writer.Write( o, version  );
					writer.Flush();
					fos.Close();
				}

				//  Parse the object from the file
				Console.WriteLine( "Parsing from file..." );
				SifParser p = SifParser.NewInstance();
				using( Stream fis = File.OpenRead( "test.xml"))
				{
					returnVal = (SifDataObject) p.Parse( fis, null );
				}
			

				//  Write the parsed object to System.out
				returnVal.SetChanged( true );
				Console.WriteLine( "Read object : " + returnVal.ObjectTag );
				echo.Write( returnVal, version  );
			} 
			catch (Exception e)
			{
				Console.WriteLine( e );
				Assert.Fail( "Exception: " + e.Message );
			}

			return returnVal;

		}
	}
}
