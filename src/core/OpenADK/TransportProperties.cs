//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Specialized;
using OpenADK.Library.Impl;
using OpenADK.Util;

namespace OpenADK.Library
{
    /// <summary>  Properties describing operational settings of a transport protocol.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    [Serializable]
    public abstract class TransportProperties : AdkProperties
    {
        private bool fEnabled = true;

        public bool Enabled
        {
            get { return fEnabled; }
            set { fEnabled = value; }
        }


        /// <summary>  Gets the name of the transport protocol associated with these properties</summary>
        /// <returns> A protocol name such as <i>http</i> or <i>https</i>
        /// </returns>
        public abstract string Protocol { get; }

        /// <summary>  Constructor</summary>
        public TransportProperties()
            : this( (TransportProperties) null ) {}

        /// <summary>  Constructs a TransportProperties object that inherits its properties
        /// from a parent. Call the Agent.getDefaultTransportProperties method to
        /// obtain the default TransportProperties object for a given transport
        /// protocol.
        /// 
        /// </summary>
        /// <param name="parent">The parent TransportProperties object, usually obtained
        /// by calling Agent.getDefaultTransportProperties
        /// </param>
        public TransportProperties( TransportProperties parent )
        {
            fParent = parent;
        }

        /// <summary>  Initialize the TransportProperties with default values</summary>
        public override void Defaults( Object owner )
        {
            string key = "adk.transport." + Protocol;
            int keyLen = key.Length;

            NameValueCollection properties = Properties.GetProperties();
            foreach ( string k in properties.Keys ) {
                if ( k.StartsWith( key ) ) {
                    string name = k.Substring( keyLen + 1 );
                    string val = properties[k];

                    if ( (Adk.Debug & AdkDebugFlags.Properties) != 0 ) {
                        if ( owner == null ) {
                            Adk.Log.Debug( "Using System property " + k + " = " + val );
                        }
                        else if ( owner is ZoneImpl ) {
                            ((ZoneImpl) owner).Log.Debug
                                ( "Using System property " + k + " = " + val );
                        }
                        else if ( owner is Agent ) {
                            Agent.Log.Debug( "Using System property " + k + " = " + val );
                        }
                    }

                    this.SetProperty( name, val );
                }
            }
        }
    }
}
