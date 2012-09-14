using System;
using System.Collections.Generic;
using System.IO;
using OpenADK.Library;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;

namespace Library.UnitTesting.Framework
{
    public class InMemoryProtocolHandler : IProtocolHandler
    {
        private ZoneImpl fZone = null;
        private List<SifMessagePayload> fMessages = new List<SifMessagePayload>();

        public void Open( ZoneImpl zone )
        {
            zone.Log.Info( "TestProtocolHandler openned" );
            fZone = zone;
        }

        public void Close( IZone zone )
        {
            zone.Log.Info( "TestProtocolHandler closed" );
            fZone = null;
        }

        public string Name
        {
            get { return "TestProtocolHandler"; }
        }

        public void Start()
        {
            Adk.Log.Info( "TestProtocolHandler started" );
        }

        public void Shutdown()
        {
            Adk.Log.Info( "TestProtocolHandler shutdown" );
        }


        private static IMessageInputStream makeAck()
        {
            SIF_Ack retval = new SIF_Ack();
            retval.SIF_Status = new SIF_Status( 0 );
            MemoryStream ms = new MemoryStream();
            try
            {
                SifWriter sifWriter = new SifWriter( ms );
                sifWriter.Write( retval );
                sifWriter.Flush();
                //sifWriter.Close();


                MessageStreamImpl imp = new MessageStreamImpl( ms );
                return (IMessageInputStream) imp;
            }
            catch ( Exception )
            {
                return null;
            }
        }

        //public string Send(string msg)
        //{
        //   lock (this)
        //   {
        //      fMessages.AddLast(msg);
        //   }
        //   return makeAck();
        //}

        public IMessageInputStream Send( IMessageOutputStream msg )
        {
            lock ( this )
            {
                try
                {
                    MemoryStream stream = new MemoryStream();
                    msg.CopyTo( stream );
                    stream.Seek( 0, SeekOrigin.Begin );
                    SifParser parser = SifParser.NewInstance();
                    SifMessagePayload smp = (SifMessagePayload) parser.Parse( stream, fZone );

                    fMessages.Add( smp );
                    parser = null;

                    SIF_Ack ack = smp.ackStatus( 0 );
                    SIF_Header hdr = ack.Header;
                    hdr.SIF_Timestamp = DateTime.Now;
                    hdr.SIF_MsgId = Adk.MakeGuid();
                    hdr.SIF_SourceId = fZone.Agent.Id;

                    StringWriter str = new StringWriter();
                    SifWriter writer = new SifWriter( str );
                    writer.Write( ack );
                    writer.Flush();
                    writer.Close();
                    writer = null;
                    return new MessageStreamImpl( str.ToString() );
                }
                catch( Exception ex )
                {
                    // Possible error parsing. Write the message to console out
                    Console.Out.WriteLine(msg.Decode());
                    throw new AdkMessagingException(ex.Message, fZone, ex);
                }
            }
        }

        /// <summary>
        /// Returns true if the protocol and underlying transport are currently active
        /// for this zone
        /// </summary>
        /// <param name="zone"></param>
        /// <returns>True if the protocol handler and transport are active</returns>
        public bool IsActive( ZoneImpl zone )
        {
            return true;
        }

        #region IProtocolHandler Members

        /// <summary>
        /// Creates the SIF_Protocol object that will be included with a SIF_Register
        /// message sent to the zone associated with this Transport.</Summary>
        /// <remarks>
        /// The base class implementation creates an empty SIF_Protocol with zero
        /// or more SIF_Property elements according to the parameters that have been
        /// defined by the client via setParameter. Derived classes should therefore
        /// call the superclass implementation first, then add to the resulting
        /// SIF_Protocol element as needed.
        /// </remarks>
        /// <param name="zone"></param>
        /// <returns></returns>
        public SIF_Protocol MakeSIF_Protocol( IZone zone )
        {
            return null;
        }

        #endregion

        ///<summary> Pop off the message that was written to the queue.  Returns null if there 
        /// were no messages written.</summary>
        public SifMessagePayload readMsg()
        {
            lock ( this )
            {
                if ( fMessages.Count < 1 )
                {
                    return null;
                }
                else
                {
                    SifMessagePayload smp = fMessages[0];
                    fMessages.RemoveAt( 0 );
                    return smp;
                }
            }
        }

        public void clear()
        {
            fMessages.Clear();
        }
    } //end class
} //end namespace