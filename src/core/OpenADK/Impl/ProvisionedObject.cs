//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

namespace OpenADK.Library.Impl
{
    ///<summary>
    ///Information about a specific handler for a specific message type
    /// </summary>
    public class ProvisionedObject<T, V> where V : ProvisioningOptions
    {
        private T fHandler;
        private V fOptions;
        private IElementDef fObjectType;

        ///<summary>
        /// Creates a ProvisionedObject instance
        ///</summary>
        ///<param name="objectType">The IElementDef describing the SIFDataObject represented by this instance</param>
        ///<param name="handler">The handler for this instance, such as a Subscriber or Publisher</param>
        ///<param name="options">The Provisioning options in effect for this instance</param>
        public ProvisionedObject(IElementDef objectType, T handler, V options)
        {
            fObjectType = objectType;
            fHandler = handler;
            fOptions = options;
        }


        ///<summary>Returns the IElementDef describing the SIFDataObject represented by this instance</summary>
        ///<returns>the IElementDef describing the SIFDataObject represented by this instance</returns>
        public IElementDef ObjectType
        {
            get { return fObjectType; }
        }


        ///<summary>Returns the handler for this message type, such as a Subscriber, Publisher, ReportPublisher, etc.</summary>
        ///<returns>the handler for this message type, such as a Subscriber, Publisher, ReportPublisher, etc.</returns>
        public T Handler
        {
            get { return fHandler; }
        }

        ///<summary>Returns the provisioining options in effect for this instance</summary>
        ///<returns>The provisioning options in effect for this instance</returns>
        public V ProvisioningOptions
        {
            get { return fOptions; }
        }
    } //end class
} //end namespace
