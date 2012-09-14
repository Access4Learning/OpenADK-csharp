//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using OpenADK.Library.Impl;
using log4net;

namespace OpenADK.Library
{
    /// <summary>  The base class for all exception classes defined by the Adk.</summary>
    /// <remarks>
    /// AdkExceptions have the following characteristics:
    /// <list type="bullet">
    /// <item>
    /// <term>An exception may contain one or more child exceptions. All Adk
    /// methods that operate on multiple zones or topics take a fail-late
    /// approach where the method continues processing even when an error
    /// occurs. Only after all zones and topics have been enumerated is a
    /// final exception thrown. It may contain multiple child exceptions
    /// collected during the processing</term>
    /// </item>
    /// <item><term>The caller can obtain a reference to the Zone in which the exception
    /// occurred.</term></item>
    /// </list>
    /// <para>Author: Eric Peterson</para>
    /// <para>Version: Adk 1.0</para>
    /// </remarks>
    [Serializable]
    public class AdkException : Exception
    {
        /// <summary>  When this flag is enabled, the Adk will retain the message associated
        /// with the exception rather than removing it from the agent's queue, if
        /// possible.
        /// </summary>
        /// <seealso cref="Retry">
        /// </seealso>
        private const int FLG_RETRY = 0x00000001;

        /// <summary>  The Zone associated with this exception. Because the Zone interface does
        /// not support RMI, this is a transient data member. Rather than calling
        /// <c>getZone</c> to retrieve the zone associated with an AdkException,
        /// RMI-based clients should use the <c>getZoneId</c> method
        /// instead.
        /// </summary>
        [NonSerialized()]
        private IZone fZone;

        /// <summary>  The ID of the zone associated with this exception</summary>
        private string fZoneId;

        /// <summary>  The nested child exceptions</summary>
        private List<Exception> fChildren;

        /// <summary>  Optional exception flags that may influence how the Adk handles this
        /// exception when it is caught within the class framework
        /// </summary>
        private int fFlags;

        /// <summary>  Constructs an exception with a detailed message that occurs in the
        /// context of a zone
        /// </summary>
        /// <param name="msg">A message describing the exception
        /// </param>
        /// <param name="zone">The zone associated with the exception
        /// </param>
        public AdkException(string msg,
                             IZone zone)
            : base(msg)
        {
            fZone = zone;
            fZoneId = zone == null ? null : zone.ZoneId;
        }

        /// <summary>
        /// Constructs an exception with zone information and the exception that caused this exception to be raised
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="zone"></param>
        /// <param name="innerException"></param>
        public AdkException(string msg,
                             IZone zone,
                             Exception innerException)
            : base(msg, innerException)
        {
            fZone = zone;
            fZoneId = zone == null ? null : zone.ZoneId;
        }


        /// <summary>
        /// The .Net Serialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected AdkException(SerializationInfo info,
                                StreamingContext context)
            : base(info, context) { }





        /// <summary>  Gets the zone associated with this exception.</summary>
        /// <remarks>
        /// <note type="note">The Adk's <c>IZone</c> interface does not support remoting.
        /// Therefore, this method will return a <c>null</c> value when called
        /// by an remoting client on a marshalled AdkException object. Remoting clients
        /// should instead use the <c>ZoneId</c> method to learn the ID of
        /// the zone passed to the constructor.</note>
        /// </remarks>
        /// <returns> The Zone associated with the exception</returns>
        /// <seealso cref="ZoneId"/>
        public virtual IZone Zone
        {
            get { return fZone; }
        }

        /// <summary>  Gets the ID of the zone associated with this exception.</summary>
        /// <returns> the ID of the Zone passed to the constructor</returns>
        /// <seealso cref="Zone">
        /// </seealso>
        public virtual string ZoneId
        {
            get { return fZoneId; }
        }

        /// <summary>  Gets the child exceptions, if any</summary>
        public virtual Exception [] Children
        {
            get
            {
                if( fChildren == null )
                {
                    return new Exception[0];
                }
                return fChildren.ToArray();
            }
        }

        /// <summary>  Determines if the Adk should attempt to retry the operation associated
        /// with this exception.
        /// </summary>
        /// <value>True if the exception is flagged for retry (the default is
        /// false). The method that catches the exception should attempt to
        /// retry the operation in progress.
        /// </value>
        public virtual bool Retry
        {
            get { return (fFlags & FLG_RETRY) != 0; }

            set
            {
                if ( value ) {
                    fFlags |= FLG_RETRY;
                }
                else {
                    fFlags &= ~ FLG_RETRY;
                }
            }
        }

        /// <summary>Gets all child SifExceptions, if any</summary>
        /// <remarks>
        /// This API does not return all child exceptions. It only returns
        /// those that can be cast to a SIFException. To get an array of all
        /// exceptions, use the <see cref="AdkException.Children"/> property
        /// </remarks>
        /// <value> An array of SIFExceptions</value>
        public virtual SifException [] SIFExceptions
        {
            get
            {
                if( fChildren == null )
                {
                    return null;
                }

                List<SifException> seList = new List<SifException>();
                foreach( Exception ex in fChildren )
                {
                    if( ex is SifException )
                    {
                        seList.Add( (SifException) ex );
                    }
                }
                return seList.ToArray();
            }
        }

      
        /// <summary>  Determines if this exception has nested child exceptions</summary>
        public virtual bool HasChildren()
        {
            return fChildren != null;
        }

        /// <summary>  Adds a child exception</summary>
        public virtual void Add( Exception thr )
        {
            if ( fChildren == null ) {
                fChildren = new List<Exception>();
            }
            fChildren.Add( thr );
        }

        /// <summary>  Determines if this exception contains any SifException children.
        /// SifException encapsulates SIF errors returned to the agent in SIF_Ack
        /// messages.
        /// 
        /// </summary>
        /// <returns> true if this exception has at least one nested SifException
        /// </returns>
        public virtual bool HasSifExceptions()
        {
            if ( fChildren != null ) {
                foreach( Exception ex in fChildren ) {
                    if ( ex is SifException ) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>  Determines if this exception contains at least one nested SifException
        /// with the specified error category.
        /// </summary>
        /// <param name="category">The error category to search for
        /// </param>
        /// <returns> true if this exception has nested SIFExceptions and at least one
        /// of those has the specified error category.
        /// </returns>
        public virtual bool HasSifError( SifErrorCategoryCode category )
        {
            if ( this is SifException ) {
                return ((SifException) this).HasErrorCategory( category );
            }

            return _recurseError( this, category );
        }

        /// <summary>  Determines if this exception contains at least one nested SifException
        /// with the specified error category and code.
        /// </summary>
        /// <param name="category">The error category to search for
        /// </param>
        /// <param name="code">The error code to search for
        /// </param>
        /// <returns> true if this exception has nested SIFExceptions and at least one
        /// of those has the specified error category and code
        /// </returns>
        public virtual bool HasSifError( SifErrorCategoryCode category,
                                         int code )
        {
            if ( this is SifException ) {
                return ((SifException) this).HasError( category, code );
            }

            return _recurseError( this, category, code );
        }

        private bool _recurseError( AdkException parent,
                                    SifErrorCategoryCode category,
                                    int code )
        {
            if ( parent.fChildren != null ) {
                Exception ch = null;

                for ( int i = 0; i < fChildren.Count; i++ ) {
                    ch = (Exception) fChildren[i];

                    if ( ch is SifException ) {
                        if ( ((SifException) ch).HasError( category, code ) ) {
                            return true;
                        }
                    }
                    else if ( ch is AdkException ) {
                        if ( _recurseError( (AdkException) ch, category, code ) ) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool _recurseError( AdkException parent,
                                    SifErrorCategoryCode category )
        {
            if ( parent.fChildren != null ) {
                Exception ch = null;

                for ( int i = 0; i < fChildren.Count; i++ ) {
                    ch = (Exception) fChildren[i];

                    if ( ch is SifException ) {
                        if ( ((SifException) ch).HasErrorCategory( category ) ) {
                            return true;
                        }
                    }
                    else if ( ch is AdkException ) {
                        if ( _recurseError( (AdkException) ch, category ) ) {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>  Returns a string representation of this exception and all child
        /// exceptions formatted for printing to System.out. Each exception is
        /// on its own line.
        /// </summary>
        public override string ToString()
        {
            return ToString( 0, true );
        }

        /// <summary>  Returns a string representation of this exception and all child
        /// exceptions formatted for printing to System.out. Each exception is
        /// on its own line.
        /// </summary>
        /// <param name="indent">The amount of indentation to apply to the string</param>
        /// <param name="includeChildren">True if all children should be returned</param>
        public virtual string ToString( int indent,
                                        bool includeChildren )
        {
            StringBuilder b = new StringBuilder();

            for ( int i = 0; i < indent; i++ ) {
                b.Append( "  " );
            }

            //  Print detailed message if any
            if ( indent != 0 ) {
                b.Append( "Zone " + (fZone == null ? "(Unknown)" : fZone.ZoneId) + ": " );
            }
            b.Append( Message );

            //  Print nested exceptions if any
            if ( fChildren != null && includeChildren ) {
                b.Append( ":" );
                b.Append( "\r\n" );
                int cnt = fChildren.Count;
                for ( int i = 0; i < cnt; i++ ) {
                    Exception innerX = (Exception) fChildren[i];
                    if ( innerX is AdkException ) {
                        b.Append( ((AdkException) innerX).ToString( indent + 1, true ) );
                    }
                    else {
                        StringBuilder msg = new StringBuilder();
                        for ( int k = 0; k < (indent + 1); k++ ) {
                            msg.Append( "  " );
                        }

                        msg.Append( innerX.ToString() );
                        b.Append( msg.ToString() );
                    }

                    b.Append( "\r\n" );
                }
            }

            return b.ToString();
        }

        /// <summary>  Write this exception and all of its nested exceptions to the logging framework. For
        /// any exception that is not associated with a zone, the supplied default
        /// Category will be used. Otherwise the Category of the zone is used.
        /// </summary>
        public virtual void Log( ILog def )
        {
            Log( def, 0 );
        }

        /// <summary>
        /// Write this exception and all of its nested exception to the logging framework
        /// </summary>
        /// <param name="def">The log to write to</param>
        /// <param name="indent">The amoung of indentation to apply</param>
        public virtual void Log( ILog def,
                                 int indent )
        {
            ILog target = fZone == null ? def : ((ZoneImpl) fZone).Log;
            if( target == null )
            {
                return;
            }

            target.Error( ToString( indent, false ) );

            if ( fChildren != null ) {
                int cnt = fChildren.Count;
                for ( int i = 0; i < cnt; i++ ) {
                    Exception innerX = (Exception) fChildren[i];
                    if ( innerX is AdkException ) {
                        ((AdkException) innerX).Log( def, indent + 1 );
                    }
                    else {
                        StringBuilder msg = new StringBuilder();
                        for ( int k = 0; k < (indent + 1); k++ ) {
                            msg.Append( "  " );
                        }

                        msg.Append( innerX.ToString() );
                        target.Error( msg, this );
                    }
                }
            }
        }
    }
}
