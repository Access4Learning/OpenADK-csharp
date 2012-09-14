//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using OpenADK.Util;
using OpenADK.Library.Impl;


namespace OpenADK.Library
{
    public interface ISifDtd : IDtd
    {
        string Variant { get; }

        string XMLNS_BASE{ get; }

        String BasePackageName { get; }
        
        int[] AvailableLibraries{ get; }

        List<string> LoadedLibraryNames{ get; }
    }
}
