//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using OpenADK.Library.Infra;

namespace OpenADK.Library
{
    /// <summary>  SifException describes a SIF error condition.
    /// 
    /// When handling inbound messages, the agent may throw a SifException from a
    /// message handler to signal that an error has occurred and should be returned 
    /// to the ZIS in the SIF_Ack message. The Adk will convert the SifException to 
    /// a SIF_Error element when sending the associated SIF_Ack. It is desirable to 
    /// throw SifException in your message handlers (versus generic exceptions) if 
    /// you want control over setting the SIF_Error category, code, description, and 
    /// extended description elements.
    /// 
    /// SifException may also be thrown by Adk methods in response to a SIF_Ack 
    /// received from the server. The actual SIF_Ack object that generated the exception
    /// can be retrieved by calling <c>getAck</c>. Any SIF_Error elements 
    /// included in the acknowledgement can be retrieved by calling <c>getErrors</c> 
    /// and associated methods such as <c>hasError</c>. Note SIF 1.0r1 allowed 
    /// for multiple SIF_Error elements per SIF_Ack, but later versions of SIF do 
    /// not. For backward compatibility, the Adk captures all SIF_Error elements 
    /// received in SIF_Ack messages and makes them available as an array. The array
    /// can be obtained by calling the <c>getErrors</c> method.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class SifException : AdkMessagingException
    {
        /// <summary>  Gets all SIF_Errors wrapped by this exception</summary>
        /// <returns> an array of SIF_Error elements
        /// </returns>
        /// 
        public virtual SIF_Error Error
        {
            get { return fError; }
        }

        /// <summary>Gets or sets the Error Category of the first SIF_Error element, or 0xFFFFFFFF
        /// if there are no errors. If more than one SIF_Error is present, the
        /// category code from the first error is returned.
        /// </summary>
        /// <value>A <c>SifErrorCategoryCode.</c> error category
        /// </value>
        public virtual SifErrorCategoryCode ErrorCategory
        {
            get
            {
                if ( fError != null ) {
                    return (SifErrorCategoryCode) fError.SIF_Category;
                }

                return SifErrorCategoryCode.None;
            }

            set
            {
                _checkErrorExists();
                fError.SIF_Category = (int) value;
            }
        }

        /// <summary>  Gets the Error Code of the first SIF_Error element, or 0xFFFFFFFF
        /// if there are no errors. If more than one SIF_Error is present, the
        /// error code from the first error is returned.
        /// </summary>
        /// <value>A <c>SifErrorCodes</c> error code
        /// </value>
        public virtual int ErrorCode
        {
            get
            {
                if ( fError != null && fError.SIF_Code.HasValue ) {
                    return fError.SIF_Code.Value;
                }

                return -1;
            }

            set
            {
                _checkErrorExists();
                fError.SIF_Code = value;
            }
        }

        /// <summary>Gets or Sets the error description of the first SIF_Error wrapped by this 
        /// exception. If no SIF_Errors are wrapped by this exception, a new one 
        /// is created.
        /// </summary>
        /// <value>The error description
        /// </value>
        public virtual string ErrorDesc
        {
            get { return fError.SIF_Desc; }

            set
            {
                _checkErrorExists();
                fError.SIF_Desc = value;
            }
        }

        /// <summary>Gets or Sets the optional extended error description of the first SIF_Error 
        /// wrapped by this exception. If no SIF_Errors are wrapped by this exception, 
        /// a new one is created.
        /// </summary>
        /// <value>The extended error description
        /// </value>
        public virtual string ErrorExtDesc
        {
            get
            {
                if ( fError != null ) {
                    return fError.SIF_ExtendedDesc;
                }

                return null;
            }

            set
            {
                _checkErrorExists();
                fError.SIF_ExtendedDesc = value;
            }
        }

        public virtual SIF_Ack Ack
        {
            get { return fAck; }
        }

        public override string Message
        {
            get
            {
                StringBuilder buf = new StringBuilder();

                string msg = base.Message;
                if ( msg != null ) {
                    //  Only include super.getMessage() text if there is no SIF_Error or
                    //  if there is a SIF_Error and its SIF_Desc text is different. This
                    //  makes for much less annoying error output.
                    if ( fError == null || (fError.SIF_Desc == msg) ) {
                        buf.Append( msg );
                        buf.Append( ": " );
                    }
                }

                if ( fError != null ) {
                    buf.Append( "[Category=" );
                    buf.Append( fError.SIF_Category );
                    buf.Append( "; Code=" );
                    buf.Append( fError.SIF_Code );
                    buf.Append( "] " );
                    string desc = fError.SIF_Desc;
                    if ( desc != null ) {
                        buf.Append( desc );
                    }
                    desc = fError.SIF_ExtendedDesc;
                    if ( desc != null ) {
                        buf.Append( ". " );
                        buf.Append( desc );
                    }
                }

                return buf.ToString();
            }
        }


        // TODO: Serialize these values

        /// <summary>  The SIF_Ack that caused this exception (if the exception was raised in
        /// response to an incoming message)
        /// </summary>
        private SIF_Ack fAck;

        /// <summary>Optional SIF_Error wrapped by this exception</summary>
        private SIF_Error fError;


        /// <summary>  Constructs an exception to wrap one or more SIF_Errors received from an
        /// inbound SIF_Ack message. This form of constructor is only called by
        /// the Adk.
        /// </summary>
        public SifException( SIF_Ack ack,
                             IZone zone )
            : base( null, zone )
        {
            fAck = ack;
            fError = ack != null ? ack.SIF_Error : null;
        }


        /// <summary>  Constructs an exception to wrap one or more SIF_Errors received from an
        /// inbound SIF_Ack message. This form of constructor is only called by
        /// the Adk.
        /// </summary>
        public SifException( string msg,
                             SIF_Ack ack,
                             IZone zone )
            : base( msg, zone )
        {
            fAck = ack;
            fError = ack != null ? ack.SIF_Error : null;
        }

        /// <summary>  Constructs a SifException for delivery to the ZIS.</summary>
        /// <param name="category">A <c>SifErrorCategoryCode.</c> error category
        /// </param>
        /// <param name="code">A <c>SifErrorCodes</c> error code
        /// </param>
        /// <param name="desc">The error description
        /// </param>
        /// <param name="zone">The zone on which the error occurred
        /// </param>
        /// <remarks> The Adk will include
        /// the error information provided by the exception when it sends a SIF_Ack
        /// in response to the message being processed. This form of constructor is
        /// typically called by the Adk, but may also be called by agent code if an
        /// exception occurs in a <i>Publisher</i>, <i>Subscriber</i>, or <i>QueryResults</i>
        /// message handler implementation.</remarks>
        public SifException( SifErrorCategoryCode category,
                             int code,
                             string desc,
                             IZone zone )
            : this( category, code, desc, null, zone ) {}

        /// <summary>  Constructs a SifException for delivery to the ZIS.</summary>
        /// <param name="category">A <c>SifErrorCategoryCode.</c> error category
        /// </param>
        /// <param name="code">A <c>SifErrorCodes</c> error code
        /// </param>
        /// <param name="desc">The error description
        /// </param>
        /// <param name="zone">The zone on which the error occurred
        /// </param>
        /// <param name="innerException">The exception that is being handled by the agent</param>
        /// <remarks> The Adk will include
        /// the error information provided by the exception when it sends a SIF_Ack
        /// in response to the message being processed. This form of constructor is
        /// typically called by the Adk, but may also be called by agent code if an
        /// exception occurs in a <i>Publisher</i>, <i>Subscriber</i>, or <i>QueryResults</i>
        /// message handler implementation.</remarks>
        public SifException( SifErrorCategoryCode category,
                             int code,
                             string desc,
                             IZone zone,
                             Exception innerException )
            :
                this( category, code, desc, null, zone, innerException ) {}


        /// <summary>
        /// Constructs a SifException for delivery to the ZIS
        /// </summary>
        /// <param name="category">A <c>SifErrorCategoryCode.</c> error category</param>
        /// <param name="code">A <c>SifErrorCodes</c> error code</param>
        /// <param name="desc">The error description</param>
        /// <param name="extDesc">An option extended error description</param>
        /// <param name="zone">The zone on which the error occurred</param>
        /// <remarks>
        ///  The Adk will include
        /// the error information provided by the exception when it sends a SIF_Ack
        /// in response to the message being processed. This form of constructor is
        /// typically called by the Adk, but may also be called by agent code if an
        /// exception occurs in a <c>IPublisher</c>, <c>ISubscriber</c>, or <c>IQueryResults</c>
        /// message handler implementation.
        /// </remarks>
        public SifException(
            SifErrorCategoryCode category,
            int code,
            string desc,
            string extDesc,
            IZone zone )
            : base( desc, zone )
        {
            fAck = null;
            fError = new SIF_Error
                (
                (int) category,
                code,
                desc == null ? "" : desc );
            if ( extDesc != null ) {
                fError.SIF_ExtendedDesc = extDesc;
            }
        }

        /// <summary>
        /// Constructs a SifException for delivery to the ZIS
        /// </summary>
        /// <param name="category">A <c>SifErrorCategoryCode.</c> error category</param>
        /// <param name="code">A <c>SifErrorCodes</c> error code</param>
        /// <param name="desc">The error description</param>
        /// <param name="extDesc">An option extended error description</param>
        /// <param name="zone">The zone on which the error occurred</param>
        /// <param name="innerException">The internal error that was thrown by the agent</param>
        /// <remarks>
        ///  The Adk will include
        /// the error information provided by the exception when it sends a SIF_Ack
        /// in response to the message being processed. This form of constructor is
        /// typically called by the Adk, but may also be called by agent code if an
        /// exception occurs in a <c>IPublisher</c>, <c>ISubscriber</c>, or <c>IQueryResults</c>
        /// message handler implementation.
        /// </remarks>
        public SifException( SifErrorCategoryCode category,
                             int code,
                             string desc,
                             string extDesc,
                             IZone zone,
                             Exception innerException )
            : base( desc, zone, innerException )
        {
            fAck = null;
            fError = new SIF_Error( (int) category, code, desc == null ? "" : desc );
            if ( extDesc != null ) {
                fError.SIF_ExtendedDesc = extDesc;
            }
        }

        /// <summary>
        /// The .Net Serialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected SifException( SerializationInfo info,
                                StreamingContext context )
            : base( info, context ) {}


        /// <summary>  Determines if this SifException describes any SIF_Errors</summary>
        /// <returns> true if the exception wraps at least on SIF_Error
        /// </returns>
        public virtual bool HasErrors
        {
            get { return fError != null; }
        }

        /// <summary>  Determines if this SifException has an error with the specified category
        /// and code. In some versions of SIF, a SifException may describe more than
        /// one error. This method searches through all of the wrapped errors and
        /// returns <c>true</c> if any match the category and code.
        /// 
        /// </summary>
        /// <param name="category">The SIF error category to search for
        /// </param>
        /// <param name="code">The SIF error code to search for
        /// 
        /// </param>
        /// <returns> <c>true</c> if any errors wrapped by this exception match
        /// the category and code
        /// </returns>
        public virtual bool HasError( SifErrorCategoryCode category,
                                      int code )
        {
            if ( fError != null ) {
                if ( fError.SIF_Category == (int) category && fError.SIF_Code == code ) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>  Determines if this SifException has an error with the specified category.
        /// In some versions of SIF, a SifException may describe more than
        /// one error. This method searches through all of the wrapped errors and
        /// returns <c>true</c> if any match the category.
        /// 
        /// </summary>
        /// <param name="category">The SIF error category to search for
        /// 
        /// </param>
        /// <returns> <c>true</c> if any errors wrapped by this exception match
        /// the category
        /// </returns>
        public virtual bool HasErrorCategory( SifErrorCategoryCode category )
        {
            if ( fError != null && fError.SIF_Category == (int) category ) {
                return true;
            }

            return false;
        }


        /// <summary>  Sets the SIF_Error element associated with this exception.</summary>
        public virtual void SetSIF_Error( int category,
                                          int code,
                                          string desc,
                                          string extDesc )
        {
            _checkErrorExists();
            fError.SIF_Category = category;
            fError.SIF_Code = code;
            fError.SIF_Desc = desc;
            fError.SIF_ExtendedDesc = desc;
        }

        private void _checkErrorExists()
        {
            if ( fError == null ) {
                fError = new SIF_Error();
            }
        }
    }
}
