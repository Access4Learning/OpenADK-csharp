using System;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Summary description for ICompareOutput.
    /// </summary>
    public interface ICompareOutput
    {
        void ComparisonFailed( string source,
                               string objectName,
                               string fieldName,
                               string value1Name,
                               string schema2,
                               string Value2Name,
                               string sourceLocation );

        void ComparisonMissing( string definedSchema,
                                string ObjectName,
                                string value,
                                string missingFromSchema,
                                string sourceLocation );

        void ComparisonWarning( string definedSchema,
                                string targetSchema,
                                string data );

        void ComparisonDebug(string definedSchema,
                              string targetSchema,
                              string data);

        void StartComparisonSection( string title );
        void EndComparisonSection();
        void AddComment( string comment );
    }
}