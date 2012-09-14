using System;
using System.IO;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Summary description for ConsoleCompareOutput.
    /// </summary>
    public class CompareOutput : ICompareOutput
    {
        public CompareOutput( TextWriter writer,
                              bool showWarnings, bool showDebug )
        {
            fWriter = writer;
            fShowWarnings = showWarnings;
            fShowDebug = showDebug;
        }

        public void ComparisonFailed( string schema1,
                                      string objectName,
                                      string fieldName,
                                      string value1,
                                      string schema2,
                                      string value2,
                                      string sourceLocation )
        {
            fErrors++;
            fWriter.WriteLine
                ( "Error:   {0}.{1}.{2} ({3}) != {4}.{5}.{6} ({7}) ({8})", schema1, objectName,
                  fieldName, value1, schema2, objectName, fieldName, value2, sourceLocation );
        }

        public void ComparisonMissing( string definedSchema,
                                       string ObjectName,
                                       string objectValue,
                                       string missingFromSchema,
                                       string sourceLocation )
        {
            fErrors++;
            fWriter.WriteLine
                ( "Error:   [{2}] missing {0}.{1} ({3})", ObjectName, objectValue, missingFromSchema,
                  sourceLocation );
        }

        public void ComparisonWarning( string definedSchema,
                                       string targetSchema,
                                       string data )
        {
            fWarnings++;
            if ( fShowWarnings ) {
                fWriter.WriteLine( "Warning: [{0}] : {1}", targetSchema, data );
            }
        }

        public void ComparisonDebug(string definedSchema,
                               string targetSchema,
                               string data)
        {
            if (fShowDebug)
            {
                fWriter.WriteLine("Warning: [{0}] : {1}", targetSchema, data);
            }
        }

        public void StartComparisonSection( string title )
        {
            ResetCounts();
            fWriter.WriteLine
                ( "**********************************************************************" );
            fWriter.WriteLine( "** " + title );
            fWriter.WriteLine
                ( "**********************************************************************" );
        }

        public void EndComparisonSection()
        {
            fWriter.WriteLine( "Section Complete: {0} Errors; {1} Warnings", fErrors, fWarnings );
            fWriter.WriteLine();
            ResetCounts();
        }

        public void AddComment( string comment )
        {
            fWriter.WriteLine( comment );
        }

        private void ResetCounts()
        {
            fWarnings = 0;
            fErrors = 0;
        }

        private TextWriter fWriter;
        private bool fShowWarnings;
        private bool fShowDebug;
        private int fErrors = 0;
        private int fWarnings = 0;
    }
}