//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Util;

namespace OpenADK.Library.Tools
{
    public class VariantAssisstant
    {
        public static readonly string README = "THIS CLASS HAS NOT BEEN TESTED.  USE AT YOUR OWN RISK.";

        public static Type GetSifElementType(string library, string sdoName)
        {
            string sdoAssembly = Adk.Dtd.SDOAssembly;

            string variantString = sdoAssembly.Substring(sdoAssembly.Length - 2).ToLower();

            string className = "OpenADK.Library." + variantString + "." + library + "." + sdoName + ", " + sdoAssembly;

            Type type = Type.GetType(className);

            return type;
        }


        public static Type GetSifElementType(string sdoName)
        {
            string sdoAssembly = Adk.Dtd.SDOAssembly;

            string variantString = sdoAssembly.Substring(sdoAssembly.Length - 2).ToLower();

            foreach (string library in ((ISifDtd)Adk.Dtd).LoadedLibraryNames)
            {
                string className = "OpenADK.Library." + variantString + "." + library + "." + sdoName + ", " + sdoAssembly;

                Type type = Type.GetType(className);

                if (type != null)
                    return type;
            }

            return null;
        }



        public static SifElement GetSifElement(string sdoName)
        {
            string sdoAssembly = Adk.Dtd.SDOAssembly;

            string variantString = sdoAssembly.Substring(sdoAssembly.Length - 2).ToLower();

            foreach (string library in ((ISifDtd)Adk.Dtd).LoadedLibraryNames)
            {
                string className = "OpenADK.Library." + variantString + "." + library + "." + sdoName + ", " + sdoAssembly;

                Type type = Type.GetType(className);

                if (type != null)
                    return (SifElement)Activator.CreateInstance(type, false);
            }

            return null;
        }


        public static SifElement GetSifElement(string library, string sdoName)
        {
            string sdoAssembly = Adk.Dtd.SDOAssembly;

            string variantString = sdoAssembly.Substring(sdoAssembly.Length - 2).ToLower();

            string className = "OpenADK.Library." + variantString + "." + library + "." + sdoName + ", " + sdoAssembly;

            Type type = Type.GetType(className);

            if (type != null)
                return (SifElement)Activator.CreateInstance(type, false);
            else
                return null;
        }
    }
}
