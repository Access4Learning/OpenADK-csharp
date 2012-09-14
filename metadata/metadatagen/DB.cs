using System;
using System.Collections;
using System.Text;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary>  Metadata database for a version of the SIF Specification.
    /// *
    /// *
    /// </summary>
    public class DB : ISchemaDefinition
    {
        private void InitBlock()
        {
            fEnums = new Hashtable();
            fObjects = new Hashtable();
        }

        public virtual ObjectDef[] Objects
        {
            get
            {
                ObjectDef[] arr = new ObjectDef[fObjects.Count];
                fObjects.Values.CopyTo( arr, 0 );
                return arr;
            }
        }

        public virtual EnumDef[] Enums
        {
            get
            {
                EnumDef[] arr = new EnumDef[fEnums.Count];
                fEnums.Values.CopyTo( arr, 0 );
                return arr;
            }
        }

        public virtual SifVersion Version
        {
            get { return fVersion; }
        }

        public virtual String Namespace
        {
            get { return fNamespace; }
        }

        protected internal SifVersion fVersion;
        protected internal String fNamespace;
        protected internal Hashtable fEnums;
        protected internal Hashtable fObjects;

        public DB( SifVersion version,
                   String namespace_Renamed,
                   string schemaTagName )
        {
            InitBlock();
            fVersion = version;
            fNamespace = namespace_Renamed;
            fSchemaName = schemaTagName;
        }

        /// <summary>  Merge the definitions of this DB into another DB
        /// </summary>
        public virtual void mergeInto( DB target )
        {
            Console.Out.WriteLine
                ( "Merging metadata for \"" + this.fVersion + "\" into \"" +
                  target.Version + "\"..." );

            Console.Out.WriteLine( "- Processing enumerated types..." );
            for ( IEnumerator e = fEnums.Keys.GetEnumerator(); e.MoveNext(); ) {
                String key = (String) e.Current;
                Object val = fEnums[key];

                EnumDef targetEnum = (EnumDef) target.fEnums[key];
                if ( targetEnum == null ) {
                    //  Add the missing EnumDef to the target
                    Console.Out.WriteLine( "  (+) \"" + key + "\" not found in target; adding" );
                    target.fEnums[key] = val;
                }
                else {
                    //  Visit all values in the target's enumeration and add values
                    //  for any that are new in this version of SIF
                    
                    foreach(ValueDef def in ((EnumDef)val).Values  ) {
                        if ( !targetEnum.ContainsValue( def.Value ) ) {
                            Console.Out.WriteLine
                                ( "  (~) \"" + key + "::" + def.Value + "\" not found in target; adding" );
                            targetEnum.DefineValue( def.Name, def.Value, def.Desc );
                        }
                    }
                }
            }

            //  Update the target's SifVersion range
            for ( IEnumerator e = target.fEnums.Keys.GetEnumerator(); e.MoveNext(); ) {
                String key = (String) e.Current;
                EnumDef enumDef = (EnumDef) target.fEnums[key];
                enumDef.LatestVersion = fVersion;
                Console.Out.WriteLine
                    ( "Enum " + enumDef.Name + " now has SifVersion range of " +
                      enumDef.EarliestVersion + ".." + enumDef.LatestVersion );
            }

            for ( IEnumerator e = target.fObjects.Keys.GetEnumerator(); e.MoveNext(); ) {
                String key = (String) e.Current;
                ObjectDef objDef = (ObjectDef) target.fObjects[key];
                objDef.LatestVersion = fVersion;
                Console.Out.WriteLine
                    ( "Object " + objDef.Name + " now has SifVersion range of " +
                      objDef.EarliestVersion + ".." + objDef.LatestVersion );

                for ( IEnumerator e2 = objDef.fFields.Keys.GetEnumerator(); e2.MoveNext(); ) {
                    key = (String) e2.Current;
                    FieldDef fldDef = objDef.fFields[key];
                    fldDef.LatestVersion = fVersion;
                    Console.Out.WriteLine
                        ( "  Field " + fldDef.Name + " now has SifVersion range of " +
                          fldDef.EarliestVersion + ".." + fldDef.LatestVersion );
                }
            }


            Console.Out.WriteLine( "- Processing object definitions..." );
            for ( IEnumerator e = fObjects.Keys.GetEnumerator(); e.MoveNext(); ) {
                String key = (String) e.Current;
                ObjectDef val = (ObjectDef) fObjects[key];

                ObjectDef targetObj = (ObjectDef) target.fObjects[key];
                if ( targetObj == null ) {
                    //  Add the missing ObjectDef to the target
                    Console.Out.WriteLine
                        ( "  (+) \"" + key + "\" (" + val.Fields.Length +
                          " fields) not found in target; adding" );
                    target.fObjects[key] = val;
                }
                else {
                    //  Do some sanity checking
                    if ( !targetObj.LocalPackage.Equals( val.LocalPackage ) ) {
                        throw new MergeException
                            ( "Target and source have different package values (target=\"" +
                              targetObj.Name + "\", package=\"" + targetObj.LocalPackage +
                              "\", source=\"" + val.Name + "\", package=\"" + val.LocalPackage +
                              "\"" );
                    }
                    if ( !targetObj.Superclass.Equals( val.Superclass ) ) {
                        throw new MergeException
                            ( "Target and source have different superclass values (target=\"" +
                              targetObj.Name + "\", superclass=\"" + targetObj.Superclass +
                              "\", source=\"" + val.Name + "\", superclass=\"" + val.Superclass +
                              "\"" );
                    }

                    //  Append this fExtrasFile to the target's if necessary
                    if ( val.ExtrasFile != null ) {
                        Console.Out.WriteLine( "  (+) \"" + key + "\" has an Extras File; adding" );
                        targetObj.ExtrasFile = targetObj.ExtrasFile + ";" + val.ExtrasFile;
                    }

                    //  Determine if the object's key fields (required elements and
                    //  attributes) differ
                    StringBuilder keyCmp1s = new StringBuilder();
                    FieldDef[] keyCmp1 = val.Key;
                    for ( int n = 0; n < keyCmp1.Length; n++ ) {
                        keyCmp1s.Append( keyCmp1[n].Name == null ? "null" : keyCmp1[n].Name );
                        if ( n != keyCmp1.Length - 1 ) {
                            keyCmp1s.Append( '+' );
                        }
                    }

                    StringBuilder keyCmp2s = new StringBuilder();
                    FieldDef[] keyCmp2 = targetObj.Key;
                    for ( int n = 0; n < keyCmp2.Length; n++ ) {
                        keyCmp2s.Append( keyCmp2[n].Name == null ? "null" : keyCmp2[n].Name );
                        if ( n != keyCmp2.Length - 1 ) {
                            keyCmp2s.Append( '+' );
                        }
                    }

                    if ( !keyCmp1s.ToString().Equals( keyCmp2s.ToString() ) ) {
                        throw new MergeException
                            ( "\"" + key +
                              "\" target and source have different key signature; merge not yet supported by adkgen." +
                              " Target=\"" + keyCmp2s + "\"; Source=\"" +
                              keyCmp1s + "\"" );
                    }

                    targetObj.LatestVersion = Version;

                    //  Determine if any of the object's fields differ in their definition
                    for ( IEnumerator k = val.fFields.Keys.GetEnumerator(); k.MoveNext(); ) {
                        String key2 = (String) k.Current;
                        FieldDef srcFld = val.fFields[key2];
                        FieldDef targetFld;
                        targetFld = targetObj.fFields[key2];

                        if ( targetFld == null ) {
                            Console.Out.WriteLine
                                ( "  (+) Field \"" + key + "::" + key2 +
                                  "\" not found in target; adding" );
                            targetObj.fFields[key2] = srcFld;
                        }
                        else {
                            bool methodDiff = false;
                            bool attrDiff = false;

                            //  Check the field for differences...
                            if ( !Equals( srcFld.FieldType, targetFld.FieldType ) ) {
                                Console.Out.WriteLine
                                    ( "    (~) Field \"" + key + "::" + key2 +
                                      "\" has different ClassType; adding version-specific field" );
                                methodDiff = true;
                            }


                            if ( !targetFld.Tag.Equals( srcFld.Tag ) ) {
                                Console.Out.WriteLine
                                    ( "    (~) Field \"" + key + "::" + key2 +
                                      "\" has different tag; adding alias" );
                                Console.Out.WriteLine( "        " + fVersion + " -> " + srcFld.Tag );
                                Console.Out.WriteLine
                                    ( "        " + target.Version + " -> " + targetFld.Tag );
                                attrDiff = true;
                            }

                            if ( targetFld.Sequence != srcFld.Sequence ) {
                                Console.Out.WriteLine
                                    ( "    (~) Field \"" + key + "::" + key2 +
                                      "\" has different sequence number; adding alias" );
                                Console.Out.WriteLine
                                    ( "        " + fVersion + " -> " + srcFld.Sequence );
                                Console.Out.WriteLine
                                    ( "        " + target.Version + " -> " + targetFld.Sequence );
                                attrDiff = true;
                            }

                            if ( methodDiff ) {
                                //  If there were any differences that would result in new
                                //  methods to the implementation class, create a new FieldDef
                                Console.Out.WriteLine( "*** DIFF ***" );
                                throw new MergeException( "Method merge not yet supported" );
                            }
                            else if ( attrDiff ) {
                                //  If there were any differences in tag name or sequence
                                //  number, add an alias to the FieldDef
                                targetFld.addAlias( fVersion, srcFld.Tag, srcFld.Sequence );
                            }
                            else {
                                targetFld.LatestVersion = fVersion;
                            }
                        }
                    }
                }
            }
        }

        public virtual ObjectDef defineObject( int id,
                                               String name,
                                               String pkg,
                                               string sourceLocation )
        {
            //System.Diagnostics.Debug.Assert( !(name == "FundingSource" ));
            ObjectDef d = (ObjectDef) fObjects[name];
            if ( d == null ) {
                d = new ObjectDef( id, name, sourceLocation, pkg, fVersion );
                fObjects[name] = d;
            }
            return d;
        }


        public virtual ObjectDef GetObject( String name )
        {
            return (ObjectDef) fObjects[name];
        }

        public IDictionary GetAllObjects()
        {
            return fObjects;
        }

        public IDictionary GetAllEnums()
        {
            return fEnums;
        }

        public virtual void defineEnum( string name,
                                        EnumDef enumdef )
        {
            fEnums[name] = enumdef;
        }

        #region ISchemaDefinition Members

        public IDictionary GetInfraObjects()
        {
            IDictionary infraObjects = new Hashtable();
            foreach ( ObjectDef def in fObjects.Values ) {
                if ( def.Infra ) {
                    infraObjects.Add( def.Name, def );
                }
            }
            return infraObjects;
        }

        public IDictionary GetTopicObjects()
        {
            IDictionary topicObjects = new Hashtable();
            foreach ( ObjectDef def in fObjects.Values ) {
                if ( def.Topic ) {
                    topicObjects.Add( def.Name, def );
                }
            }
            return topicObjects;
        }

        public EnumDef GetEnum( string name )
        {
            return (EnumDef) fEnums[name];
        }


        public string Name
        {
            get { return fSchemaName; }
        }

        #endregion

        private string fSchemaName;
    }
}