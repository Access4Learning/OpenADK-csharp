//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using System.Text;

namespace OpenADK.Library.Impl
{
    /// <summary>  A MessageStreamer is a Reader class used to assemble and stream a SIF_Message
    /// by concatenating an outer <i>envelope</i> with one or more inner <i>payloads</i>. 
    /// The MessageStreamer can be passed to any method that accepts a Reader as an
    /// input source.
    /// 
    /// MessageStreamer takes care of assembling the envelope and payload by reading
    /// from the Envelope Reader and Payload Reader passed to the constructor.
    /// Bytes are read from the Envelope Reader up until the named element is
    /// encountered (inclusive). Subsequent bytes are read from the Payload Reader(s)
    /// until the closing tag of the named element is encountered (also inclusive).
    /// Finally, the remaining bytes of the Envelope Reader are read.
    /// 
    /// An instance of this class is typically passed to the IProtocolHandler.send
    /// method when sending an arbitrarily large SIF_Message.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class MessageStreamer : TextReader, IMessageOutputStream
    {
        //private int fPos = 0;
        private Stage fStage = Stage.StartEnvelope;
        private Stream fEnvelope;
        private StreamReader fEnvelopeReader;
        private Stream [] fPayloads;
        private StreamReader fPayloadReader;
        private int fPayloadIndex = 0;
        private string fElement;
        private bool fReplaceMode = false;

        /// <summary>  Constructor</summary>
        /// <param name="envelope">A Reader that provides the content of the <i>envelope</i>
        /// </param>
        /// <param name="payloads">An array of Readers that provide the content of the <i>payload</i>
        /// </param>
        /// <param name="element">The element that signals the beginning of the payload.
        /// The first element of this type will be considered the beginning of
        /// the payload.
        /// </param>
        /// <param name="replace">True if in replace mode</param>
        public MessageStreamer( Stream envelope,
                                Stream [] payloads,
                                string element,
                                bool replace )
        {
            fReplaceMode = replace;
            fEnvelope = envelope;
            fPayloads = payloads;
            fElement = element;
            fLength = CalculateLength();
            fEnvelopeReader = new StreamReader( fEnvelope, SifIOFormatter.ENCODING );
        }

        /// <summary>
        /// Reads a portion of the data
        /// </summary>
        /// <param name="cbuf"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public override int Read( Char [] cbuf,
                                  int off,
                                  int len )
        {
            if ( len == 0 ) {
                return 0;
            }
            int bytes = 0;

            switch ( fStage ) {
                case Stage.StartEnvelope: // Reading the envelope up to and including the element 
                    {
                        int valueRead = 0;
                        int start = - 1;
                        int pos = off;

                        //  Read from the Envelope Reader, looking for <element> to signal
                        //  advancement to the next stage
                        do {
                            valueRead = fEnvelopeReader.Read();
                            if ( valueRead > -1 ) {
                                cbuf[pos] = (char) valueRead;
                                if ( cbuf[pos] == '<' ) {
                                    start = pos;
                                }
                                else if ( cbuf[pos] == '>' ) {
                                    string cmp = new string( cbuf, start, (pos - start) + 1 );
                                    if ( cmp.Equals( fElement ) ) {
                                        fStage = Stage.Payload;
                                        if ( fReplaceMode ) {
                                            return start;
                                        }
                                        else {
                                            return (pos - off) + 1;
                                        }
                                    }
                                }
                                pos++;
                            }
                        }
                        while ( valueRead != - 1 && pos < len );
                    }
                    break;
                case Stage.Payload: // Reading the payload
                    while ( fPayloadIndex < fPayloads.Length ) {
                        if ( fPayloadReader == null ) {
                            fPayloadReader =
                                new StreamReader
                                    ( fPayloads[fPayloadIndex], SifIOFormatter.ENCODING );
                        }
                        bytes = fPayloadReader.Read( cbuf, off, len );
                        if ( bytes > 0 ) {
                            return bytes;
                        }
                        else {
                            fPayloadReader.Close();
                            fPayloadReader = null;
                            fPayloadIndex ++;
                        }
                    }
                    fStage = Stage.Done;
                    return _readEndEnvelope( cbuf, off, len );
            }

            return 0;
        }

        private int _readEndEnvelope( char [] cbuf,
                                      int off,
                                      int len )
        {
            // skip the fElement if in  replace mode
            if ( fReplaceMode ) {
                char [] ignored = new char[fElement.Length + 2];
                fEnvelopeReader.Read( ignored, 0, ignored.Length );
                // Remove the end of the element, including the space and the "/" as in </SIF_Error>
            }
            else {
                // Otherwise, skip the empty space
                fEnvelopeReader.Read();
            }
            int bytes = fEnvelopeReader.Read( cbuf, off, len );
            if ( fEnvelopeReader.Peek() == -1 ) {
                fStage = Stage.Done;
            }
            return bytes;
        }

        private long CalculateLength()
        {
            long length = CalculateLengthOfStream( fEnvelope );
            foreach ( Stream stream in fPayloads ) {
                length += CalculateLengthOfStream( stream );
                if ( fReplaceMode ) {
                    length -= (fElement.Length*2) + 1;
                }
            }
            // We strip out the extra space inside the placeholder element, so remove it from the calculation
            length--;
            return length;
        }

        /// <summary>
        /// This method strips of the high-ascii bytes at the beginning of a UTF8 encoded string to accurately determine the real text length of the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private long CalculateLengthOfStream( Stream stream )
        {
            stream.Seek( 0, SeekOrigin.Begin );
            // Advance the stream to the first readable char ( value will be less than 0x80 );
            int data = 0;
            long length = stream.Length;
            while ( true ) {
                data = stream.ReadByte();
                if ( data < 0x80 ) {
                    break;
                }
                length--;
            }
            stream.Seek( 0, SeekOrigin.Begin );
            return length;
        }

        private long fLength;

        /// <summary>
        /// The length of the message, in bytes, after encoding with the SIF Encoding algorithm ( UTF-8 )
        /// </summary>
        public long Length
        {
            get { return fLength; }
        }

        /// <summary>
        /// Closes the Message object and releases any resources
        /// </summary>
        public override void Close()
        {
            if ( fEnvelope != null ) {
                fEnvelope.Close();
                fEnvelope = null;
            }
            if ( fPayloads != null ) {
                foreach ( Stream stream in fPayloads ) {
                    stream.Close();
                }
                fPayloads = null;
            }
        }

        #region IMessageStream Members

        /// <summary>
        /// Copies the underlying message to the stream
        /// </summary>
        /// <param name="stream"></param>
        public void CopyTo( Stream stream )
        {
            char [] aData = new char[4096];
            Encoding encoder = SifIOFormatter.ENCODING;
            int aBytesRead;
            while ( true ) {
                aBytesRead = this.Read( aData, 0, 4096 );
                if ( aBytesRead > 0 ) {
                    byte [] binaryData = encoder.GetBytes( aData, 0, aBytesRead );
                    stream.Write( binaryData, 0, binaryData.Length );
                }
                else {
                    break;
                }
            }
            stream.Flush();
        }

        /// <summary>
        /// Implementation of IMessageInputStream.ToString(). Currently returns null
        /// </summary>
        /// <returns>null</returns>
        public string Decode()
        {
            return string.Empty;
        }

        #endregion

        /// <summary>
        /// Release any unmanaged resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );
            Close();
        }


        private enum Stage
        {
            StartEnvelope = 0,
            Payload = 1,
            EndEnvelope = 2,
            Done = 3
        }
    }
}
