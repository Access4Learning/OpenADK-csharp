using System;
using System.Collections;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary>
    /// Summary description for IDB.
    /// </summary>
    public interface ISchemaDefinition
    {
        IDictionary GetAllObjects();
        IDictionary GetAllEnums();
        EnumDef GetEnum( string name );
        ObjectDef GetObject( string name );
        string Name { get; }
        SifVersion Version { get; }
    }
}