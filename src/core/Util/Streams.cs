//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;

namespace OpenADK.Util
{
    /// <summary>
    /// Summary description for Streams.
    /// </summary>
    public class Streams
    {
        private static AsyncCallback sEndWriteHandler;

        static Streams()
        {
            sEndWriteHandler = new AsyncCallback( EndCopyStream );
        }

        /// <summary>
        /// Copies a stream
        /// </summary>
        /// <param name="inSource"></param>
        /// <param name="inDestination"></param>
        /// <param name="inRewindStreams">If set to true, rewinds the source before the copy and rewinds both after the copy</param>
        public static void CopyStream( Stream inSource,
                                       Stream inDestination,
                                       int blockSize )
        {
            byte [] aData = new byte[blockSize];

            int aBytesRead;
            while ( true ) {
                aBytesRead = inSource.Read( aData, 0, blockSize );
                if ( aBytesRead > 0 ) {
                    inDestination.Write( aData, 0, aBytesRead );
                }
                else {
                    break;
                }
            }
            inDestination.Flush();
        }

        /// <summary>
        /// Copies a stream asynchronously. This method is useful for copying data to a network stream because the network stream 
        /// will actually buffer at the hardware level and the calling thread will not have to block. when the asynchronous copy
        /// has complete, the delegate specified by the endHandler argument will be called to complete the operation
        /// </summary>
        /// <param name="inSource">The stream to copy from</param>
        /// <param name="inDestination">The stream to copy to. Flush() will be called when the copy is complete</param>
        /// <param name="endHandler">The delegate to call when the async copy operation is complete</param>
        /// <param name="state">The state, if any that the delegate would like returned to it</param>
        public static void BeginCopyStream( Stream inSource,
                                            Stream inDestination,
                                            int bufferSize,
                                            EduAsyncHandler endHandler,
                                            object state )
        {
            AsyncStreamState streamState =
                new AsyncStreamState( inSource, inDestination, bufferSize, endHandler, state );
            DoAsyncCopyStep( streamState );
        }

        private static void EndCopyStream( IAsyncResult result )
        {
            AsyncStreamState state = (AsyncStreamState) result.AsyncState;
            try {
                state.fDestinationStream.EndWrite( result );
            }
            catch ( Exception ex ) {
                state.SetException( ex );
                state.AsyncHandler( state );
            }
            DoAsyncCopyStep( state );
        }

        private static void DoAsyncCopyStep( AsyncStreamState state )
        {
            int aBytesRead = state.fSourceStream.Read( state.fBuffer, 0, state.fBufferSize );
            if ( aBytesRead > 0 ) {
                state.fDestinationStream.BeginWrite
                    ( state.fBuffer, 0, aBytesRead, sEndWriteHandler, state );
            }
            else {
                state.fDestinationStream.Flush();
                state.AsyncHandler( state );
            }
        }


        /// <summary>
        /// Copies a stream
        /// </summary>
        /// <param name="in_Source">The source stream</param>
        /// <param name="in_Destination">The destination stream</param>
        /// <remarks>
        /// If the source stream supports seeking, it is rewound before reading and rewound again after copying.
        /// If the destination stream supports seeking, it is rewound after the copy
        /// </remarks>
        public static void CopyStream( Stream inSource,
                                       Stream inDestination )
        {
            CopyStream( inSource, inDestination, 4096 );
        }

        /// <summary>
        /// Returns an array of bytes from a stream
        /// </summary>
        /// <param name="inStream">The source stream, which must support seeking</param>
        /// <returns></returns>
        public static byte [] GetDataFromStream( Stream inStream )
        {
            byte [] aReturnData = new byte[inStream.Length];
            inStream.Seek( 0, SeekOrigin.Begin );
            inStream.Read( aReturnData, 0, aReturnData.Length );
            return aReturnData;
        }

        private class AsyncStreamState : EduAsyncResult
        {
            public Stream fSourceStream;
            public Stream fDestinationStream;
            public int fBufferSize;
            public byte [] fBuffer;

            public AsyncStreamState( Stream sourceStream,
                                     Stream destinationStream,
                                     int bufferSize,
                                     EduAsyncHandler finishedHandler,
                                     object finishedHandlerState )
                : base( finishedHandler, finishedHandlerState )
            {
                fSourceStream = sourceStream;
                fDestinationStream = destinationStream;
                fBufferSize = bufferSize;
                fBuffer = new byte[fBufferSize];
            }
        }
    }

    /// <summary>
    /// A delegate for handling asynchronous operation in the Library ADK
    /// </summary>
    public delegate void EduAsyncHandler( EduAsyncResult state );


    /// <summary>
    /// A class that follows a similar programming pattern to the .Net AsyncResult class. It encapsulates
    /// the state of an async operation.
    /// </summary>
    public class EduAsyncResult
    {
        private object fState;
        private Exception fException;
        private EduAsyncHandler fAsyncDelegate;

        /// <summary>
        /// Creates an instance of an EduAsyncResult
        /// </summary>
        /// <param name="handler">The delegate the will accept the result of the asynchronous invocation</param>
        /// <param name="state">The state, if necessary to be returned to the result delegate</param>
        public EduAsyncResult( EduAsyncHandler handler,
                               object state )
        {
            fAsyncDelegate = handler;
            fState = state;
        }

        /// <summary>
        /// Signals to the caller that an exception occurred during the async operation
        /// </summary>
        /// <param name="ex"></param>
        public void SetException( Exception ex )
        {
            fException = ex;
        }

        /// <summary>
        /// Call this method to retrieve any exceptions that may have occurred while processing the event
        /// </summary>
        public void Finish()
        {
            if ( fException != null ) {
                throw fException;
            }
        }

        public EduAsyncHandler AsyncHandler
        {
            get { return fAsyncDelegate; }
        }

        /// <summary>
        /// The original state that was passed to the async method
        /// </summary>
        public object AsyncState
        {
            get { return fState; }
        }
    }
}
