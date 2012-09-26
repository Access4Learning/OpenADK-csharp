using System;

namespace OpenADK.Library
{

    /// <summary>
    /// Values identifying each package that is considered to be an intrinsic library.
    /// </summary>
    [Flags]
    public enum IntrinsicLibraryType : int
    {
        /// <summary> All SDO libraries </summary>
        All = -1, // 0xFFFFFFFF

        /// <summary> No SDO libraries </summary>
        None = 0x00000000,

        //  These are always loaded regardless of what the user specifies.
        //  They are considered "built-in" SDO libraries but under the hood they're
        //  treated just like any other SDO package.
        /// <summary>Identifies the Infrastructure Sdo library</summary>
        Global = 0x40000000,

        /// <summary>Identifies the Infrastructure Sdo library</summary>
        Infra = 0x20000000,

        /// <summary>Identifies the Infrastructure Sdo library</summary>
        Common = 0x10000000
    }

}
