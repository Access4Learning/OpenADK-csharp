//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;

namespace OpenADK.Web.Http
{
    /// <summary>
    /// Reads data from the underlying stream until the end of the headers section has been reached
    /// </summary>
    public class AdkHttpHeadersReader : TextReader
    {
        public AdkHttpHeadersReader( Stream stream )
        {
            fStream = stream;
        }

        public override int Read()
        {
            if ( fSequencePosition == 4 ) // || !fStream.DataAvailable )
            {
                return -1;
            }
            byte [] aData = new byte[1];
            int aChr = fStream.Read( aData, 0, 1 );
            if ( aChr > 0 ) {
                if ( aData[0] == '\r' || aData[0] == '\n' ) {
                    fSequencePosition++;
                    if ( fSequencePosition == 4 ) {
                        return -1;
                    }
                }
                else {
                    fSequencePosition = 0;
                }
                return aData[0];
            }
            else {
                return -1;
            }
        }

        public override int Peek()
        {
            // Our stream does not support seeking
            return -1;
        }

        public long Position
        {
            get { return fStream.Position; }
        }

        private Stream fStream;
        private int fSequencePosition;
    }
}
