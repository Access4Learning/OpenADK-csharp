//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using OpenADK.Util;

namespace OpenADK.Library.Impl
{
    /// <summary>
    /// The MessageStreamImpl class wraps a stream or a string
    /// </summary>
    public class MessageStreamImpl : IMessageInputStream, IMessageOutputStream
    {
        /// <summary>
        /// Creates a new, empty, instance of MessageStreamImpl
        /// </summary>
        public MessageStreamImpl()
        {
            fMessageStream = new MemoryStream();
        }

        /// <summary>
        /// Creates a new instance of MessageStreamImpl, based upon the message
        /// </summary>
        /// <param name="message"></param>
        public MessageStreamImpl( string message )
            : this()
        {
            StreamWriter writer = new StreamWriter( fMessageStream, SifIOFormatter.ENCODING );
            writer.Write( message );
            writer.Flush();
            fCacheString = message;
        }

        /// <summary>
        /// Creates a new instance of MessageStreamImpl, based upon the underlying stream
        /// </summary>
        /// <param name="message"></param>
        public MessageStreamImpl( Stream message )
        {
            if ( message.CanSeek ) {
                fMessageStream = message;
            }
            else {
                fMessageStream = new MemoryStream();
                Streams.CopyStream( message, fMessageStream );
                message.Close();
            }
           
        }

        /// <summary>
        /// Returns a string representation of the underlying binary data
        /// </summary>
        /// <returns>the underlying binary data converted to a string using UTF8 encoding</returns>
        public string Decode()
        {
            if ( fCacheString == null ) {
                StreamReader reader = new StreamReader( GetInputStream(), SifIOFormatter.ENCODING );
                fCacheString = reader.ReadToEnd();
            }
            return fCacheString;
        }

        /// <summary>
        /// The underlying stream that is being wrapped
        /// </summary>
        /// <returns></returns>
        public Stream GetInputStream()
        {
            SeekBeginning();
            return fMessageStream;
        }

        /// <summary>
        /// Returns the length of the message, in bytes
        /// </summary>
        public long Length
        {
            get { return fMessageStream.Length; }
        }

        /// <summary>
        /// Copies the message to the stream
        /// </summary>
        /// <param name="stream"></param>
        public void CopyTo( Stream stream )
        {
            SeekBeginning();
            Streams.CopyStream( fMessageStream, stream );
        }

        private bool SeekBeginning()
        {
            if ( fMessageStream.CanSeek ) {
                fMessageStream.Seek( 0, SeekOrigin.Begin );
                return true;
            }
            else {
                return fMessageStream.Position == 0;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Release any unmanaged resources held on to by this object
        /// </summary>
        public void Dispose()
        {
            Dispose( true );
        }

        /// <summary>
        /// Returns a string representation of the underlying binary data
        /// </summary>
        /// <remarks>
        /// <seealso cref="Decode"/>
        /// </remarks>
        /// <returns>The underlying buffer converted to a string</returns>
        public override string ToString()
        {
            return Decode();
        }

        #endregion

        private void Dispose( bool disposing )
        {
            if ( fMessageStream != null ) {
                ((IDisposable) fMessageStream).Dispose();
                fMessageStream = null;
            }
        }

        private Stream fMessageStream;
        private string fCacheString;
    }
}
