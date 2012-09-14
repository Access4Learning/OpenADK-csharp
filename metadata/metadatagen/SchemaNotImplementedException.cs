using System;
using System.Xml.Schema;

namespace Edustructures.Metadata
{
    internal class SchemaNotImplementedException : Exception
    {
        private XmlSchemaParticle fParticle;

        public SchemaNotImplementedException( XmlSchemaParticle particle )
        {
            fParticle = particle;
        }

        public XmlSchemaParticle Particle
        {
            get { return fParticle; }
        }
    }
}