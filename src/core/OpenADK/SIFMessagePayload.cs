//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using OpenADK.Library.Infra;
using log4net;

namespace OpenADK.Library
{
    /// <summary>  A specialization of SifElement for SIF infrastructure messages such as
    /// SIF_Register, SIF_Request, and SIF_Event.
    /// 
    /// SifMessagePayload provides methods specific to infrastructure messages, such
    /// as retrieving the OpenADK.Library or its individual fields. When an instance of
    /// this class is constructed, it is done so without its OpenADK.Library element. The
    /// <c>getHeader</c> method adds a OpenADK.Library child if one does not exist,
    /// and the class framework takes care of assigning values to the header prior to
    /// sending messages.
    /// 
    /// 
    /// For consistency the Adk employs the same SifElement class hierarchy and
    /// conventions for SIF Infrastructure messages as it does for SIF Data Objects.
    /// Some inherited methods of SifElement, such as setChanged and setEmpty, have
    /// no effect for infrastructure messages.
    /// 
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public abstract class SifMessagePayload : SifElement
    {
        /// <summary>  Returns the OpenADK.Library element. A new OpenADK.Library is created for this
        /// object if one does not already exist.
        /// </summary>
        /// <returns> The infrastructure message header
        /// </returns>
        public virtual SIF_Header Header
        {
            get
            {
                SIF_Header hdr = (SIF_Header)GetChild(InfraDTD.SIF_HEADER);
                if (hdr == null)
                {
                    hdr = new SIF_Header();
                    AddChild(hdr);
                }
                return hdr;
            }
        }

        /// <summary>  Gets the SIF Version to which this message conforms.</summary>
        /// <value> A SifVersion encapsulating the version of SIF to which the
        /// message conforms, or null if the message does not provide a valid
        /// xmlns or Version attribute from which to determine the SIF Version
        /// </value>	
        /// <remarks>
        /// The SIF Version is determined by first inspecting the Version attribute
        /// of the SIF_Message. If present, that attribute identifies the version of
        /// SIF to which the version conforms. If not present, the namespace is
        /// inspected; if it is in the form "http://www.sifinfo.org/v1.0r1/messages",
        /// it is parsed to obtain the version of SIF. If it is in the form
        /// "http://www.sifinfo.org/infrastructure/1.x" and no Version attribute is
        /// present, the version of SIF is assumed to be "1.1"
        /// </remarks> 
        public override SifVersion SifVersion
        {
            get
            {
                if (fVersionAttr != null)
                {
                    //  SIF 1.1 or later
                    return SifVersion.Parse(fVersionAttr);
                }
                else if (fXmlns != null)
                {
                    return SifVersion.ParseXmlns(fXmlns);
                }

                return null;
            }

            set
            {
                fXmlns = value.Xmlns;
                if (value.CompareTo(SifVersion.SIF11) >= 0)
                {
                    fVersionAttr = value.ToString();
                }
                else
                {
                    fVersionAttr = null;
                }
            }
        }

        /// <summary>  Gets the SIF_MsgId value from this message's header.
        /// 
        /// If the message does not have a OpenADK.Library element, one is created.
        /// 
        /// </summary>
        /// <returns> The SIF_MsgId value
        /// </returns>
        public virtual string MsgId
        {
            get
            {
                SIF_Header h = this.Header;
                return (h != null ? h.SIF_MsgId : null);
            }
        }

        /// <summary>  Gets the SIF_SourceId value from this message's header.
        /// 
        /// If the message does not have a OpenADK.Library element, one is created.
        /// 
        /// </summary>
        /// <returns> The SIF_SourceId value
        /// </returns>
        public virtual string SourceId
        {
            get
            {
                SIF_Header h = Header;
                return (h != null ? h.SIF_SourceId : null);
            }
        }

        /// <summary>  Gets the SIF_DestinationId value from this message's header.
        /// 
        /// If the message does not have a OpenADK.Library element, one is created.
        /// 
        /// </summary>
        /// <returns> The SIF_DestinationId value
        /// </returns>
        public virtual string DestinationId
        {
            get
            {
                SIF_Header h = Header;
                return (h != null ? h.SIF_DestinationId : null);
            }
        }

        /// <summary>  Gets the timestamp of this message from the SIF_Date and SIF_Time
        /// elements in its header.
        /// 
        /// If the message does not have a OpenADK.Library element, one is created with
        /// a timestamp equal to the current time.
        /// 
        /// </summary>
        /// <returns> The message timestamp
        /// </returns>
        public virtual DateTime? Timestamp
        {
            get
            {
                SIF_Header h = Header;
                if (h != null)
                {
                    DateTime? timeStamp = h.SIF_Timestamp;
                    if (timeStamp.HasValue)
                    {
                        return timeStamp;
                    }
                    return DateTime.Now;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the list of SIFContexts associated with this message.
        /// </summary>
        /// <value>The list of SIFContexts that are associated with this message. If no contexts 
        /// were specified in the SIF message, the list will contain a single element indicating 
        /// the SIF_Default context</value>
        /// <exception cref="AdkNotSupportedException">If one of the contexts in the message is not defined
        /// by this agent instance
        /// </exception>
        public IList<SifContext> SifContexts
        {
            get
            {
                List<SifContext> contexts = new List<SifContext>();
                SIF_Contexts allContexts = this.Header.SIF_Contexts;
                if (allContexts == null || allContexts.ChildCount == 0)
                {
                    contexts.Add(SifContext.DEFAULT);
                }
                else
                {
                    foreach (SIF_Context context in allContexts)
                    {
                        SifContext definedContext = SifContext.IsDefined(context.Value);
                        if (definedContext == null)
                        {
                            throw new AdkNotSupportedException(
                                    "SIFContext \"" + context.Value +
                                    "\" is not supported");
                        }
                        contexts.Add(definedContext);
                    }
                }
                return contexts;
            }
        }


        /// <summary>  The namespace (e.g. "http://www.sifinfo.org/v1.0r1/message",
        /// "http://www.sifinfo.org/infrastructure/1.x", etc)
        /// </summary>
        protected internal string fXmlns;

        /// <summary>  The version attribute. SIF 1.1 uses a separate version attribute to indicate
        /// the version of SIF to which a message conforms; SIF 1.0 encodes the version
        /// in the namespace. If SIF 1.1 or later is in use and this value is empty, it
        /// is assumed to be the latest version of the SIF Specification.
        /// </summary>
        protected internal string fVersionAttr;


        /// <summary>  Constructor</summary>
        /// <param name="metadata">The ElementDef representing this object in the ADK metadata</param>
        public SifMessagePayload(IElementDef metadata)
            : base(metadata)
        {
            SifVersion = Adk.SifVersion;
        }

        /// <summary>  Constructor</summary>
        /// <param name="version">The SIFVersion to render this message in</param>
        /// <param name="metadata">The ElementDef representing this object in the ADK metadata</param>
        public SifMessagePayload(SifVersion version, IElementDef metadata)
            : base(metadata)
        {
            SifVersion = version;
        }


        /// <summary>  Returns the XML namespace of this message</summary>
        public virtual string GetXmlns()
        {
            return fXmlns;
        }

        /// <summary>  Sets the XML namespace of this message</summary>
        protected internal virtual void SetXmlns(string xmlns)
        {
            fXmlns = xmlns;
        }

        /// <summary>  Returns the Version attribute of this SIF_Message.
        /// 
        /// The Version attribute was introduced in SIF 1.1, which uses the attribute
        /// to identify the version of SIF to which a message conforms. Prior
        /// versions of SIF encode the version in the namespace.
        /// 
        /// </summary>
        /// <returns> The value of the Version attribute. If a null value is returned,
        /// the caller should assume "1.1"
        /// 
        /// @since SIF 1.1
        /// </returns>
        public virtual string VersionAttribute
        {
            get { return fVersionAttr; }
        }

        /// <summary>  Sets the Version attribute of this SIF_Message.
        /// 
        /// The Version attribute was introduced in SIF 1.1
        /// 
        /// </summary>
        /// <param name="version">The text for the Version attribute
        /// @since SIF 1.1
        /// </param>
        internal virtual void SetVersionAttribute(string version)
        {
            fVersionAttr = version;
        }




        /// <summary>  Create an Immediate SIF_Ack for this message.</summary>
        /// <returns> A new SIF_Ack instance where the SIF_Status/SIF_Code value is
        /// set to a value of "1" and SIF_Ack header values are derived from this
        /// message's header values
        /// </returns>
        public virtual SIF_Ack AckImmediate()
        {
            return ackStatus(1);
        }

        /// <summary>  Create an Intermediate SIF_Ack for this message.</summary>
        /// <returns> A new SIF_Ack instance where the SIF_Status/SIF_Code value is
        /// set to a value of "2" and SIF_Ack header values are derived from this
        /// message's header values
        /// </returns>
        public virtual SIF_Ack ackIntermediate()
        {
            return ackStatus(2);
        }

        /// <summary>  Create a Final SIF_Ack for this message.</summary>
        /// <returns> A new SIF_Ack instance where the SIF_Status/SIF_Code value is
        /// set to a value of "3" and SIF_Ack header values are derived from this
        /// message's header values
        /// </returns>
        public virtual SIF_Ack ackFinal()
        {
            return ackStatus(3);
        }

        /// <summary>  Create a SIF_Ack for this message.</summary>
        /// <param name="code">The SIF_Status/SIF_Code value
        /// </param>
        /// <returns> A new SIF_Ack instance where the SIF_Status/SIF_Code value is
        /// set to the specified value and SIF_Ack header values are derived
        /// from this message's header values
        /// </returns>
        public virtual SIF_Ack ackStatus(int code)
        {
            SIF_Ack ack = new SIF_Ack();
            SIF_Status status = new SIF_Status(code);
            ack.SIF_Status = status;
            ack.SIF_OriginalMsgId = MsgId;
            ack.SIF_OriginalSourceId = SourceId;


            SifVersion msgVersion = this.SifVersion;

            if (code == 8 /* Receiver is sleeping */ )
            {
                if (msgVersion.Major == 1)
                {
                    // SIF 1.x used SIF_Data for text
                    SIF_Data d = new SIF_Data();
                    d.TextValue = "Receiver is sleeping";
                    status.SIF_Data = d;
                }
                else
                {
                    status.SIF_Desc = "Receiver is sleeping";
                }
            }

            ack.message = this;

            //  Ack using the same version of SIF as this message
            ack.SifVersion = msgVersion;

            return ack;
        }

        /// <summary>
        /// Create an error SIF_Ack for this message.
        /// </summary>
        /// <param name="sifEx">The SIFException that is the cause of the error</param>
        /// <returns></returns>
        public SIF_Ack AckError(SifException sifEx)
        {
            SIF_Ack ack = new SIF_Ack();
            ack.message = this;

            ack.SIF_OriginalMsgId = this.MsgId;
            ack.SIF_OriginalSourceId = this.SourceId;

            SIF_Error error = new SIF_Error(
                sifEx.ErrorCategory,
                sifEx.ErrorCode,
                sifEx.ErrorDesc,
                sifEx.ErrorExtDesc);

            ack.SIF_Error = error;

            //  Ack using the same version of SIF as this message
            ack.SifVersion = this.SifVersion;

            return ack;
        }

        /// <summary>  Create an error SIF_Ack for this message.</summary>
        /// <param name="category">The value of the SIF_Error/SIF_Category element
        /// </param>
        /// <param name="code">The value of the SIF_Error/SIF_Code element
        /// </param>
        /// <param name="desc">The value of the SIF_Error/SIF_Desc element
        /// </param>
        /// <returns> A new SIF_Ack instance with a SIF_Error element and SIF_Ack
        /// header values derived from this message's header values
        /// </returns>
        public virtual SIF_Ack AckError(SifErrorCategoryCode category,
                                         int code,
                                         string desc)
        {
            return AckError(category, code, desc, null);
        }

        /// <summary>  Create an error SIF_Ack for this message.</summary>
        /// <param name="category">The value of the SIF_Error/SIF_Category element
        /// </param>
        /// <param name="code">The value of the SIF_Error/SIF_Code element
        /// </param>
        /// <param name="desc">The value of the SIF_Error/SIF_Desc element
        /// </param>
        /// <param name="extDesc">The value of the SIF_Error/SIF_ExtendedDesc element
        /// </param>
        /// <returns> A new SIF_Ack instance with a SIF_Error element and SIF_Ack
        /// header values derived from this message's header values
        /// </returns>
        public virtual SIF_Ack AckError(SifErrorCategoryCode category,
                                         int code,
                                         string desc,
                                         string extDesc)
        {
            SIF_Ack ack = new SIF_Ack();
            ack.SIF_OriginalMsgId = this.MsgId;
            ack.SIF_OriginalSourceId = this.SourceId;
            SIF_Error error = new SIF_Error
                (
                (int)category,
                code,
                desc ?? "");

            if (extDesc != null)
            {
                error.SIF_ExtendedDesc = extDesc;
            }
            ack.SIF_Error = error;


            //  Ack using the same version of SIF as this message
            ack.SifVersion = SifVersion;

            return ack;
        }

        /// <summary>  Utility method called by the class framework to log this SIF_Message
        /// prior to sending it to a zone.
        /// </summary>
        /// <param name="log">The logging framework Category instance representing the destination Zone
        /// </param>
        public virtual void LogSend(ILog log)
        {
            if ((Adk.Debug & AdkDebugFlags.Messaging) != 0)
            {
                log.Debug("Send " + ElementDef.Name);
            }
            if ((Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0)
            {
                string id = MsgId;
                log.Debug("  MsgId: " + (id == null ? "<none>" : id));
            }
        }

        /// <summary>  Utility method called by the class framework to log this SIF_Message
        /// upon receipt from a zone.
        /// </summary>
        /// <param name="log">The logging framework Category instance representing the source Zone
        /// </param>
        public virtual void LogRecv(ILog log)
        {
            if ((Adk.Debug & AdkDebugFlags.Messaging) != 0)
            {
                log.Debug("Receive " + ElementDef.Name);
            }
            if ((Adk.Debug & AdkDebugFlags.Messaging_Detailed) != 0)
            {
                string id = MsgId;
                log.Debug("  MsgId: " + (id == null ? "<none>" : id));
            }
        }


        /// <summary>
        /// .Net Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected SifMessagePayload(SerializationInfo info,
                                     StreamingContext context)
            : base(info, context) { }
    }
}
