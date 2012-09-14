using System;
using System.Collections;
using System.IO;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Summary description for MetaDataSchema.
    /// </summary>
    public class MetaDataSchema
    {
        public MetaDataSchema( string path )
        {
            ArrayList defFiles = new ArrayList();
            DirectoryInfo info = new DirectoryInfo( path );
            foreach ( FileInfo file in info.GetFiles( "*.xml" ) ) {
                DefinitionFile defFile = new DefinitionFile( file );
                defFile.parse();
                defFiles.Add( defFile );
            }
        }

        public ISchemaDefinition[] GetSchemaDefinitions()
        {
            ISchemaDefinition[] dbs = new ISchemaDefinition[fDBs.Count];
            fDBs.Values.CopyTo( dbs, 0 );
            return dbs;
        }

        public ISchemaDefinition GetSchemaDefinition( SifVersion version )
        {
            return getDB( version );
        }


        /// <summary>  Gets the DB object associated with the specified version of SIF (or
        /// creates a new object if none currently exists).
        /// </summary>
        /// <param name="version">The SIF version (e.g. "1.0r1")
        /// </param>
          public static DB getDB( SifVersion version )
        {
            DB db = (DB) fDBs[version.ToString()];
            if ( db == null ) {
                db = new DB( version, version.Xmlns, "Edustructures MetaData" );
                fDBs[version.ToString()] = db;
            }
            return db;
        }

        private static IDictionary fDBs = new Hashtable();
    }
}