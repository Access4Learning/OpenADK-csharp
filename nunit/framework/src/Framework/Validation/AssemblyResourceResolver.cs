using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Library.UnitTesting.Framework.Validation
{
    public class AssemblyResourceResolver : XmlUrlResolver
    {
        private readonly Assembly resourceAssembly = null;
        private readonly String rootNamespace = null;


        public AssemblyResourceResolver( Assembly resourceAssembly, string rootNamespace )

        {
            if ( resourceAssembly == null )
            {
                throw new ArgumentNullException( "resourceAssembly must not be null" );
            }
            if ( rootNamespace == null )
            {
                throw new ArgumentNullException( "rootNamespace must not be null" );
            }

            this.resourceAssembly = resourceAssembly;
            this.rootNamespace = rootNamespace;
        }

        public override Uri ResolveUri( Uri baseUri, string relativeUri )
        {
            //ri resolved =  base.ResolveUri(baseUri, relativeUri);
            return new Uri( new Uri( "assembly://" + rootNamespace ), relativeUri );
            //return resolved;
        }


        public override object GetEntity( Uri absoluteUri, string role, Type ofObjectToReturn )

        {
            string resourcePath = absoluteUri.AbsolutePath;
            resourcePath = rootNamespace + resourcePath.Replace( '/', '.' );
            Stream stream = resourceAssembly.GetManifestResourceStream( resourcePath );
            return stream;
        }
    }
}