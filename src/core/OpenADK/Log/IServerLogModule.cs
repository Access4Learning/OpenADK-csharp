//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Log
{
    /// <summary>	Interface of a ServerLog module to which log information will be posted
    /// whenever any of the logging methods of the ServerLog class are called.
    /// </summary>
    public interface IServerLogModule
    {
        /// <summary> 	Gets the ID of this logger</summary>
        /// <returns> The ID of this ServerLogModule instance
        /// </returns>
        string ID { get; }

        /// <summary> 	Post a string message to the server log.</summary>
        /// <param name="zone">The zone on the server to post the message to
        /// </param>
        /// <param name="message">The message text
        /// </param>
        void Log( IZone zone,
                  string message );

        /// <summary> 	Post information encapsulated by a SIF <code>SIF_LogEntry</code> object to the server log.</summary>
        /// <param name="zone">The zone on the server to post the message to
        /// </param>
        /// <param name="data">The SIF_LogEntry object 
        /// </param>
        void Log( IZone zone,
                  SIF_LogEntry data );
    }
}
