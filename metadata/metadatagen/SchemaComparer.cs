using System;
using System.Collections;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Summary description for SchemaComparer.
    /// </summary>
    public class SchemaComparer
    {
        public void Compare( ISchemaDefinition officialSpec,
                             ISchemaDefinition unofficialSpec,
                             ICompareOutput output,
                             bool detailedCheck )
        {
            if ( officialSpec.Version != unofficialSpec.Version ) {
                output.ComparisonFailed
                    ( officialSpec.Name, "SifVersion", "ToString", officialSpec.Version.ToString(),
                      unofficialSpec.Name, unofficialSpec.Version.ToString(), "" );
            }

            CompareObjectDictionary
                ( officialSpec.GetAllObjects(), officialSpec.Name, unofficialSpec.GetAllObjects(),
                  unofficialSpec.Name, output, detailedCheck );
            // Only compare enums that are defined in the official spec, but not in the unofficial spec

            // TODO: Provide warnings for undocumented members

            CompareEnumDictionary
                ( officialSpec.GetAllEnums(), officialSpec.Name, unofficialSpec.GetAllEnums(),
                  unofficialSpec.Name, output );
        }

        private static void CompareEnumDictionary( IDictionary officialSpecEnums,
                                            string db1Name,
                                            IDictionary unofficialSpecEnums,
                                            string db2Name,
                                            ICompareOutput output )
        {
            CompareDictionaryDefs
                ( officialSpecEnums, db1Name, unofficialSpecEnums, db2Name, output, "EnumDef", false );

            output.StartComparisonSection
                ( "Starting search for missing enumeration fields in both Schemas" );
            CompareEnumDefs( officialSpecEnums, db1Name, unofficialSpecEnums, db2Name, output );
            output.EndComparisonSection();
        }

        private static void CompareObjectDictionary( IDictionary objects1,
                                              string db1Name,
                                              IDictionary objects2,
                                              string db2Name,
                                              ICompareOutput output,
                                              bool detailed )
        {
            CompareDictionaryDefs( objects1, db1Name, objects2, db2Name, output, "ObjectDef", true );
            CompareDictionaryDefs( objects2, db2Name, objects1, db1Name, output, "ObjectDef", true );

            output.StartComparisonSection( "Starting search for missing fields in both Schemas" );
            CompareObjectDefs( objects1, db1Name, objects2, db2Name, output, detailed );
            output.EndComparisonSection();
        }

        private static void CompareDictionaryDefs( IDictionary objects,
                                            string source,
                                            IDictionary targetObjects,
                                            string targetName,
                                            ICompareOutput output,
                                            string objectType,
                                            bool error )
        {
            output.StartComparisonSection
                ( "Starting search for missing " + objectType + "s in " + targetName );

            foreach ( AbstractDef def in objects.Values ) {
                //	System.Diagnostics.Debug.Assert( !( def.Name.ToLower() == "homeroom" ) );
                if ( !targetObjects.Contains( def.Name ) ) {
                    if ( def.Draft ) {
                        output.ComparisonWarning
                            ( source, targetName, "Missing draft object " + def.Name );
                    }
                    else if ( def.Deprecated ) {
                        output.ComparisonWarning
                            ( source, targetName, "Missing deprecated object " + def.Name );
                    }
                    else if ( ! def.ShouldValidate ) {
                        output.ComparisonWarning
                            ( source, targetName, "Missing ignored object " + def.Name );
                    }
                        // There are some cases where the SIF XSD defines an element as a complex type, even though it has
                        // no internal elements. We'll check here for defs that have no Fields in them and output a warning instead
                        // of an error
                    else if ( def is ObjectDef && ((ObjectDef) def).Fields.Length == 0 ) {
                        output.ComparisonWarning
                            ( source, targetName,
                              "Missing " + objectType + " " + def.Name +
                              ", but no fields were defined in source." );
                    }
                    else if ( def is ObjectDef && ((ObjectDef) def).RenderAs != null &&
                              targetObjects.Contains( ((ObjectDef) def).RenderAs ) ) {
                        // We've found a match using RenderAs, so this case is OK
                    }
                    else if ( !error ) {
                        output.ComparisonWarning
                            ( source, targetName, "Missing " + objectType + " " + def.Name );
                    }

                    else {
                        output.ComparisonMissing
                            ( source, def.Name, "", targetName, def.SourceLocation );
                    }
                }
            }
            output.EndComparisonSection();
        }

        private static void CompareEnumDefs( IDictionary objects,
                                      string source,
                                      IDictionary targetObjects,
                                      string targetName,
                                      ICompareOutput output )
        {
            foreach ( EnumDef def in objects.Values ) {
                EnumDef def2 = targetObjects[def.Name] as EnumDef;
                if ( def2 != null ) {
                    CompareEnumDef( def, source, def2, targetName, output );
                }
            }
        }

        private static void CompareEnumDef( EnumDef def1,
                                     string source1,
                                     EnumDef def2,
                                     string source2,
                                     ICompareOutput output )
        {
            foreach ( ValueDef def in def1.Values ) {
                if ( !def2.ContainsValue( def.Value ) ) {
                    output.ComparisonMissing
                        ( source1, def1.Name, def.Name, source2, def1.SourceLocation );
                }
            }

            foreach ( ValueDef val in def2.Values ) {
                if ( !def1.ContainsValue( val.Value )) {
                    output.ComparisonMissing
                        ( source2, def2.Name, val.Name, source1, def2.SourceLocation );
                }
            }
        }

        private static void CompareObjectDefs( IDictionary objects,
                                        string source,
                                        IDictionary targetObjects,
                                        string targetName,
                                        ICompareOutput output,
                                        bool detailed )
        {
            foreach ( ObjectDef def in objects.Values ) {
                ObjectDef def2 = targetObjects[def.Name] as ObjectDef;
                if ( def2 != null ) {
                    CompareObjectDef( def, source, def2, targetName, output, detailed );
                }
            }
        }

        private static void CompareObjectDef( ObjectDef def1,
                                       string source1,
                                       ObjectDef def2,
                                       string source2,
                                       ICompareOutput output,
                                       bool detailed )
        {
            if ( def1.Infra != def2.Infra ) {
                output.ComparisonFailed
                    ( source1, def1.Name, "Infra", def1.Infra.ToString(), source2,
                      def2.Infra.ToString(), def1.SourceLocation );
            }
            if ( def1.Topic != def2.Topic ) {
                output.ComparisonFailed
                    ( source1, def1.Name, "Topic", def1.Topic.ToString(), source2,
                      def2.Topic.ToString(), def1.SourceLocation );
            }

            CompareFields( source1, def1, source2, def2, output, detailed );
            CompareFields( source2, def2, source1, def1, output, detailed );
        }

        private static void CompareFields( string source,
                                    ObjectDef sourceDef,
                                    string target,
                                    ObjectDef targetDef,
                                    ICompareOutput output,
                                    bool detailed )
        {
            
            foreach ( FieldDef field1 in sourceDef.Fields ) {
                if ( field1.Name == "SIF_ExtendedElements" || field1.Name == "SIF_Metadata" ) {
                    // ignore. adkgen automatically adds to "topic" objects
                }
                else {
                    FieldDef field2;
                    if ( field1.RenderAs != null ) {
                        field2 = targetDef.GetField( field1.RenderAs );
                    }
                    else {
                        field2 = targetDef.GetField( field1.Name );
                    }
                    if ( field2 == null ) {

                        output.ComparisonMissing
                            ( source, sourceDef.Name, field1.Name, target, sourceDef.SourceLocation );
                    }
                    else if ( detailed ) {
                        CompareField
                            ( source, sourceDef, field1, target, field2, output, true );
                    }
                }
            }
        }

        private static void CompareField( string source1,
                                          ObjectDef def1,
                                          FieldDef field1,
                                          string source2,
                                          FieldDef field2,
                                          ICompareOutput output,
                                          bool detailedCheck )
        {

           if ( field1.Attribute != field2.Attribute ) {
                output.ComparisonFailed
                    ( source1, def1.Name + "." + field1.Name, "Attribute",
                      field1.Attribute.ToString(), source2, field2.Attribute.ToString(),
                      def1.SourceLocation );
            }

            if (field1.Sequence != field2.Sequence)
            {
                output.ComparisonWarning
                    (source1, source2, "Differing sequence: " + def1.Name + "." + field1.Name + " src(" + field1.Sequence + ") target:(" + field2.Sequence + ")");
            }

            if ( detailedCheck ) {
                //				if( field1.Complex != field2.Complex )
                //				{
                //					output.ComparisonFailed( source1, def1.Name + "." + field1.Name, "Complex", field1.Complex.ToString(), source2, field2.Complex.ToString(), def1.SourceLocation );
                //				}

                //System.Diagnostics.Debug.Assert( !(def1.Name == "FundingSource") );

                if ( !field1.FlagIntrinsicallyMatches( field2 ) ) {
                    output.ComparisonWarning
                        ( source1, source2, "Differing flags: " + def1.Name + "." + field1.Name + " src(" + field1.GetFlags() +
                                ") target:(" + field2.GetFlags() + ")" );
                }



                if ( !field1.FieldType.Equals( field2.FieldType ) ) {

                    if( field1.FieldType.IsEnum && field2.FieldType.IsEnum )
                    {
                        output.ComparisonDebug
                        (source2, source1,
                          "differing class type: " + def1.Name + "." + field1.Name + " src(" +
                          field1.FieldType + ") target:(" + field2.FieldType + ")");
                    }

                    output.ComparisonWarning
                        ( source2, source1,
                          "differing class type: " + def1.Name + "." + field1.Name + " src(" +
                          field1.FieldType + ") target:(" + field2.FieldType + ")" );
                }
            }
        }
    }
}