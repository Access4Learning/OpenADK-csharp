//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    ///<summary>
    /// Encapsulates all of the optional behavior that a publisher is able to support. 
    /// Flags in this class are used to control behavior of the agent during provisioning
    ///</summary>
    public class PublishingOptions : ProvisioningOptions
    {
        ///<summary>
        /// Flag that indicates whether or not the ADK should provision this 
        /// agent as the default provider in the zone for the object.
        ///</summary>
        private Boolean fSendSIFProvide;

        ///<summary>
        /// Flag the indicates whether or not this publisher supports SIF_ExtendedQueries<p>
        /// 
        /// If <code>false</code>, the ADK will automatically send an error packet response
        /// back for any SIF_ExtendedQueries received.<p>
        ///  
        /// If <code>true</code>, the ADK will notify the zone of SIF_ExtendedQuery support during
        /// agent provisioning.
        /// </summary>
        private Boolean fSupportsExtendedQuery;

        ///<summary>
        /// Creates an instance of PublishingOptions that supports the
        /// default SIF Context.
        /// sendSIFProvide <code>True</code> if the ADK should provision this 
        /// agent as the default provider in the zone for the object.
        ///</summary>
        public PublishingOptions() : this(true)
        {
        }

        ///<summary>
        /// Creates an instance of PublishingOptions that supports the
        /// default SIF Context.
        /// </summary>
        /// <param name="sendSIFProvide">
        /// sendSIFProvide <code>True</code> if the ADK should provision this 
        /// agent as the default provider in the zone for the object.
        ///</param>
        public PublishingOptions(Boolean sendSIFProvide)
            : base()
        {
            fSendSIFProvide = sendSIFProvide;
        }


        ///<summary>
        /// Creates an instance of PublishingOptions that only supports
        /// the given set of SifContexts. 
        /// </summary>
        ///<param name="contexts"></param>
        public PublishingOptions(params SifContext[] contexts) : base(contexts)
        {
        }


        ///<summary>
        /// Creates an instance of PublishingOptions that only supports
        /// the given set of SifContext. If the set of contexts given does not
        ///include the default SIF context, the default context will not be supported
        /// by this ReportPublisher      
        ///</summary>
        ///<param name="sendSIFProvide">
        /// <code>True</code> if the ADK should provision this 
        /// agent as the default provider in the zone for the object.
        ///</param>
        ///<param name="contexts">the explicit list of contexts to support</param>
        public PublishingOptions(Boolean sendSIFProvide, params SifContext[] contexts)
            : base(contexts)
        {
            fSendSIFProvide = sendSIFProvide;
        }


        ///<summary>
        /// Gets the flag that indicates whether or not the ADK should provision this 
        /// agent as the default provider in the zone for the object.
        ///</summary>
        public bool SendSIFProvide
        {
            get { return fSendSIFProvide; }
            set { fSendSIFProvide = value; }
        }


        ///<summary>
        /// Sets a flag the indicates whether or not this publisher supports SIF_ExtendedQueries
        /// If <code>false</code>, the ADK will automatically
        /// send an error packet response back for any SIF_ExtendedQueries received.
        /// If <code>true</code>, the ADK will notify the zone of SIF_ExtendedQuery support during
        /// agent provisioning.
        ///</summary>
        public Boolean SupportsExtendedQuery
        {
            get { return fSupportsExtendedQuery; }
            set { fSupportsExtendedQuery = value; }
        }
    } //end class
} //end namespace
