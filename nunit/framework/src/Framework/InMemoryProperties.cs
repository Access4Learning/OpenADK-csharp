using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library;

namespace Library.UnitTesting.Framework
{
    class InMemoryProperties : TransportProperties
    {
        /// <summary>  Gets the name of the transport protocol associated with these properties</summary>
        /// <returns> A protocol name such as <i>http</i> or <i>https</i>
        /// </returns>
        public override string Protocol
        {
            get { return "AdkInMemory"; }
        }
    }
}
