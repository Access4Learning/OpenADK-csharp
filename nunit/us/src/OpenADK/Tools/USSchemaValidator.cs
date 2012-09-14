using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenADK.Library;
using Library.UnitTesting.Framework.Validation;

namespace Library.Nunit.US.Library.Tools
{
    public class USSchemaValidator
    {
        public static SchemaValidator NewInstance( SifVersion schemaVersion )
        {
            Type rootNamespaceType = typeof(UsAdkTest);
            String schemaVersionStr = getShortenedVersion(schemaVersion);
            String schemaResourcePath = rootNamespaceType.Namespace + ".schemas." + schemaVersionStr;

            AssemblyResourceResolver asr = new AssemblyResourceResolver(rootNamespaceType.Assembly, schemaResourcePath);

            SchemaValidator sv = null;

            using (Stream rootSchema = rootNamespaceType.Assembly.GetManifestResourceStream(schemaResourcePath + ".SIF_Message.xsd"))
            using (TextReader textReader = new StreamReader(rootSchema))
            {
                sv = SchemaValidator.NewInstance(textReader, asr);
                textReader.Close();
                rootSchema.Close();
            }

            return sv;
            
        }


        private static String getShortenedVersion(SifVersion version)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SIF");
            builder.Append(version.Major);
            builder.Append(version.Minor);
            if (version.Revision > 0)
            {
                builder.Append('r');
                builder.Append(version.Revision);
            }
            return builder.ToString();
        }

    }
}
