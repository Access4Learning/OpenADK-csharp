//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.IO;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Impl
{
    /// <summary>  Default implementation of the ISIFPrimitives interface.
    /// 
    /// 
    /// </summary>
    internal class SIFPrimitives : ISIFPrimitives
    {
        /**
	 *  SIF_Register
	 */

        public SIF_Ack SifRegister(IZone zone)
        {
            ZoneImpl AdkZone = (ZoneImpl)zone;
            AgentProperties props = zone.Properties;
            SifVersion effectiveZISVersion = AdkZone.HighestEffectiveZISVersion;

            SIF_Register msg = new SIF_Register(effectiveZISVersion);
            msg.SIF_MaxBufferSize = props.MaxBufferSize;
            msg.SIF_Mode = props.MessagingMode == AgentMessagingMode.Pull ? "Pull" : "Push";

            // Set the agent's name and version
            Agent agent = zone.Agent;
            msg.SIF_Name = agent.Name;

            String vendor = props.AgentVendor;
            if (vendor != null)
            {
                msg.SIF_NodeVendor = vendor;
            }

            String version = props.AgentVersion;
            if (version != null)
            {
                msg.SIF_NodeVersion = version;
            }


            SIF_Application applicationInfo = new SIF_Application();
            String appName = props.ApplicationName;
            if (appName != null)
            {
                applicationInfo.SIF_Product = appName;
            }
            String appVersion = props.ApplicationVersion;
            if (appVersion != null)
            {
                applicationInfo.SIF_Version = appVersion;
            }
            String appVendor = props.ApplicationVendor;
            if (appVendor != null)
            {
                applicationInfo.SIF_Vendor = appVendor;
            }

            if (applicationInfo.FieldCount > 0)
            {
                // All three fields under SIF_Application are required by the 
                // SIF_Specification. Determine if any are missing. If so,
                // create the field with an empty value
                if (applicationInfo.SIF_Product == null)
                {
                    applicationInfo.SIF_Product = string.Empty;
                }
                if (applicationInfo.SIF_Version == null)
                {
                    applicationInfo.SIF_Version = string.Empty;
                }
                if (applicationInfo.SIF_Vendor == null)
                {
                    applicationInfo.SIF_Vendor = string.Empty;
                }
                msg.SIF_Application = applicationInfo;
            }


            String propVal = props.AgentIconUrl;
            if (propVal != null)
            {
                msg.SIF_Icon = propVal;
            }

            //
            //  SIF_Version handling:
            //
            //  * If the "Adk.provisioning.zisVersion" property is set to > SIF 1.1
            //    (the default), use SIF 1.1+ registration where multiple SIF_Version
            //    elements are included in the SIF_Register message. Otherwise use 
            //	  SIF 1.0 registration where only a single SIF_Version is included in 
            //	  the SIF_Register message.
            //
            //  For SIF 1.1 registrations:
            //
            //  * If the "Adk.sifRegister.sifVersions" System property is set,
            //    enumerate its comma-delimited list of SIF_Version values and use
            //    those instead of building a list. This is primarily used for
            //    testing wildcards (which the Adk doesn't normally use) or when an
            //    agent wants to connect to a ZIS where wildcarding works better for
            //    some reason.
            //
            //  * Otherwise, build a list of SIF_Versions: Set the first SIF_Version
            //    element to the version initialized by the Adk, then add a SIF_Version
            //    element for each additional version of SIF supported by the Adk
            //

            String forced = zone.Properties.OverrideSifVersions;
            if (forced != null)
            {
                ((ZoneImpl)zone).Log.Debug("Using custom SIF_Register/SIF_Version: " + forced);
                foreach (String token in forced.Split(','))
                {
                    msg.AddSIF_Version(new SIF_Version(token.Trim()));
                }
            }
            else
            {

                SifVersion zisVer = SifVersion.Parse(zone.Properties.ZisVersion);

                if (zisVer.CompareTo(SifVersion.SIF11) >= 0)
                {
                    // Add the Adk version first. This is the "default"
                    // agent version, which has special meaning to the
                    // ZIS
                    msg.AddSIF_Version(new SIF_Version(effectiveZISVersion));

                    // TT 2007
                    // If the Adk Version is set to 1.1 or 1.5r1, only send those two
                    // versions in the SIF_Register message. The downside to this is
                    // that we can't connect to a 2.0 ZIS using SIF 1.5r1 and still
                    // receive 2.0 events. However, this seems to be the best approach
                    // because it ensures greater compatibility with older ZIS's that will
                    // otherwise fail if they get a 2.0 version in the SIF_Register message
                    SifVersion[] supported = Adk.SupportedSIFVersions;
                    for (int i = 0; i < supported.Length; i++)
                    {
                        // Exclude the version added above
                        if (supported[i].CompareTo(effectiveZISVersion) < 0)
                        {
                            msg.AddSIF_Version(new SIF_Version(supported[i]));
                        }
                    }

                }
                else
                {
                    msg.AddSIF_Version(new SIF_Version(Adk.SifVersion));
                }
            }


            //
            //  Set the SIF_Protocol object as supplied by the Transport. Depending
            //  on the transport protocol and the messaging mode employed by the
            //  zone we may or may not get a SIF_Protocol object back
            //
            SIF_Protocol po = ((ZoneImpl)zone).ProtocolHandler.MakeSIF_Protocol(zone);
            if (po != null)
            {
                msg.SIF_Protocol = po;
            }

            return AdkZone.Dispatcher.send(msg);
        }


        /**
         *  SIF_Unregister
         */
        public SIF_Ack SifUnregister(IZone zone)
        {
            ZoneImpl AdkZone = (ZoneImpl)zone;
            SifVersion effectiveZISVersion = AdkZone.HighestEffectiveZISVersion;
            SifMessagePayload message = new SIF_Unregister(effectiveZISVersion);
            return AdkZone.Dispatcher.send(message);
        }

        /**
         *  SIF_Subscribe
         */
        public SIF_Ack SifSubscribe(IZone zone, String[] objectType)
        {
            ZoneImpl AdkZone = (ZoneImpl)zone;
            SIF_Subscribe msg = new SIF_Subscribe(AdkZone.HighestEffectiveZISVersion);
            for (int i = 0; i < objectType.Length; i++)
            {
                SIF_Object obj = new SIF_Object();
                obj.ObjectName = objectType[i];
                msg.AddSIF_Object(obj);
            }
            return AdkZone.Dispatcher.send(msg);
        }

        /**
         *  SIF_Unsubscribe
         */
        public SIF_Ack SifUnsubscribe(IZone zone, String[] objectType)
        {
            ZoneImpl AdkZone = (ZoneImpl)zone;
            SIF_Unsubscribe msg = new SIF_Unsubscribe(AdkZone.HighestEffectiveZISVersion);
            for (int i = 0; i < objectType.Length; i++)
            {
                SIF_Object obj = new SIF_Object();
                obj.ObjectName = objectType[i];
                msg.AddSIF_Object(obj);
            }
            return AdkZone.Dispatcher.send(msg);
        }

        /**
         *  SIF_Provide
         */
        public SIF_Ack SifProvide(IZone zone, String[] objectType)
        {
            ZoneImpl AdkZone = (ZoneImpl)zone;
            SIF_Provide msg = new SIF_Provide(AdkZone.HighestEffectiveZISVersion);
            for (int i = 0; i < objectType.Length; i++)
            {
                SIF_Object obj = new SIF_Object();
                obj.ObjectName = objectType[i];
                msg.AddSIF_Object(obj);
            }

            return AdkZone.Dispatcher.send(msg);
        }

        /**
         *  SIF_Unprovide
         */
        public SIF_Ack SifUnprovide(IZone zone, String[] objectType)
        {
            ZoneImpl AdkZone = (ZoneImpl)zone;
            SIF_Unprovide msg = new SIF_Unprovide(AdkZone.HighestEffectiveZISVersion);
            for (int i = 0; i < objectType.Length; i++)
            {
                SIF_Object obj = new SIF_Object();
                obj.ObjectName = objectType[i];
                msg.AddSIF_Object(obj);
            }

            return AdkZone.Dispatcher.send(msg);
        }

        /**
         *  SIF_Ping
         */
        public SIF_Ack SifPing(IZone zone)
        {

            return SifSystemControl(new SIF_Ping(), (ZoneImpl)zone);
        }

        /**
         *  SIF_ZoneStatus
         */
        public SIF_Ack SifZoneStatus(IZone zone)
        {
            return SifSystemControl(new SIF_GetZoneStatus(), (ZoneImpl)zone);
        }

        /**
         *  SIF_Sleep
         */
        public SIF_Ack SifSleep(IZone zone)
        {
            return SifSystemControl(new SIF_Sleep(), (ZoneImpl)zone);
        }

        /**
         *  SIF_GetAgentACL
         */
        public SIF_Ack SifGetAgentACL(IZone zone)
        {
            return SifSystemControl(new SIF_GetAgentACL(), (ZoneImpl)zone);
        }


        /**
         *  SIF_Wakeup
         */
        public SIF_Ack SifWakeup(IZone zone)
        {
            return SifSystemControl(new SIF_Wakeup(), (ZoneImpl)zone);
        }

        private SIF_Ack SifSystemControl(SifElement command, ZoneImpl zone)
        {
            SIF_SystemControl msg = new SIF_SystemControl(zone.HighestEffectiveZISVersion);
            SIF_SystemControlData cmd = new SIF_SystemControlData();
            cmd.AddChild(command);
            msg.SIF_SystemControlData = cmd;
            return zone.Dispatcher.send(msg);
        }

        /**
         *  Sends a SIF_Event
         *  @param zone The zone to send the sifEvent to
         */
        public SIF_Ack SifEvent(IZone zone, Event sifEvent, String destinationId, String sifMsgId)
        {
            if (sifEvent.Data == null || sifEvent.Data.Available == false)
            {
                throw new AdkException("The sifEvent has no SIFDataObjects", zone);
            }

            SIF_ObjectData od = new SIF_ObjectData();

            //  Fill out the SIF_ObjectData
            IDataObjectInputStream inStr = sifEvent.Data;
            SifDataObject data = inStr.ReadDataObject();

            SifVersion msgVersion = data.EffectiveSIFVersion;

            SIF_EventObject eo = new SIF_EventObject();
            od.SIF_EventObject = eo;
            eo.Action = sifEvent.ActionString;
            eo.ObjectName = data.ElementDef.Tag(msgVersion);

            // Create the SIF_Event object
            SIF_Event msg = new SIF_Event(msgVersion);
            msg.SIF_ObjectData = od;

            SIF_Header msgHdr = msg.Header;

            //	Assign SIF_DestinationId if applicable
            if (destinationId != null)
            {
                msgHdr.SIF_DestinationId = destinationId;
            }

            while (data != null)
            {
                eo.Attach(data);
                data = inStr.ReadDataObject();
            }

            if (sifMsgId != null)
            {
                msgHdr.SIF_MsgId = sifMsgId;
            }

            SifContext[] contexts = sifEvent.Contexts;
            if (contexts == null)
            {
                contexts = new SifContext[] { SifContext.DEFAULT };
            }

            SIF_Contexts msgContexts = new SIF_Contexts();
            foreach (SifContext context in contexts)
            {
                msgContexts.AddSIF_Context(context.Name);
            }
            msgHdr.SIF_Contexts = msgContexts;
            return ((ZoneImpl)zone).Dispatcher.send(msg);
        }

        /**
         *  SIF_Request
         */
        public SIF_Ack SifRequest(IZone zone, Query query, String destinationId, String sifMsgId)
        {
            //  Send SIF_Request...
            SIF_Request msg = new SIF_Request();
            // Find the maxmimum requested version and set the version of the message to lower
            // if the version is currently higher than the highest requested version.
            // In other words, if the Adk is initialized to 2.0, but the highest requested version
            // is 1.5r1, set the message version to 1.5r1
            SifVersion highestRequestVersion = SifVersion.SIF11;
            if (query.ObjectType == InfraDTD.SIF_ZONESTATUS)
            {
                // This query will be satisfied by the ZIS. Use the ZIS compatibility
                // version, which returns the highest version supported by the ZIS 
                // (Default to Adk.SIFVersion() if not specified in the config)
                highestRequestVersion = ((ZoneImpl)zone).HighestEffectiveZISVersion;
                msg.AddSIF_Version(new SIF_Version(highestRequestVersion));
            }
            else
            {
                SifVersion[] requestVersions = query.SifVersions;
			if( requestVersions.Length > 0 ){
				// If the Query has one or more SIFVersions set, use them,
				// and also add [major].*
				foreach( SifVersion version in requestVersions ){
					msg.AddSIF_Version(  new SIF_Version( version ) );
					if( version.CompareTo( highestRequestVersion ) > 0 ){
						highestRequestVersion = version;
					}
				}
			} else {
				highestRequestVersion = Adk.SifVersion;
				if( highestRequestVersion.Major == 1 ){
					msg.AddSIF_Version(  new SIF_Version( highestRequestVersion ) );
				} else {
					// 2.0 and greater, request all data using
					// [major].*, with 2.0r1 as the message version
					// This allows for maximum compatibility will all 2.x providers
					msg.AddSIF_Version( new SIF_Version( highestRequestVersion.Major + ".*" ));
					msg.SifVersion = SifVersion.GetEarliest( highestRequestVersion.Major );
				}
			}

            }

            AgentProperties zoneProperties = zone.Properties;

            if (zoneProperties.OverrideSifMessageVersionForSifRequests != null)
            {
                //There is a property in Agent.cfg that can be used to override the message version from the
                //default of 2.0r1 This is needed to pass the test harness for 2.3
                msg.SifVersion = SifVersion.Parse(zoneProperties.OverrideSifMessageVersionForSifRequests);
            }

            else if(msg.SifVersion.CompareTo(highestRequestVersion) > 0)
            {
                // The current version of the SIF_Message is higher than the highest 
                // requested version. Back the version number of message down to match
                msg.SifVersion = highestRequestVersion;
            }

            msg.SIF_MaxBufferSize = zone.Properties.MaxBufferSize;

            SIF_Query sifQ = CreateSIF_Query(query, highestRequestVersion, zone);
            msg.SIF_Query = sifQ;

            SIF_Header msgHeader = msg.Header;

            if (destinationId != null)
            {
                msgHeader.SIF_DestinationId = destinationId;
            }
            if (sifMsgId != null)
            {
                msgHeader.SIF_MsgId = sifMsgId;
            }

            // Set the SIF_Context
            msgHeader.SIF_Contexts = new SIF_Contexts(
                            new SIF_Context(query.SifContext.Name));

            return ((ZoneImpl)zone).Dispatcher.send(msg);
        }

        /**
         * Creates a SIF_Query element from the specified Adk query object using
         * zone-specific querying rules
         * @param query The Query to convert to a SIF_Query
         * @param zone The IZone to retrieve query settings from, or null
         * @return a SIF_Query instance
         */
        public static SIF_Query CreateSIF_Query(Query query, IZone zone)
        {
            bool allowFieldRestrictions = query.HasFieldRestrictions;
            if (allowFieldRestrictions && zone != null)
            {
                allowFieldRestrictions = !zone.Properties.NoRequestIndividualElements;
            }
            return CreateSIF_Query(query, query.EffectiveVersion, allowFieldRestrictions);
        }

        /**
         * Creates a SIF_Query element from the specified Adk query object using
         * zone-specific querying rules
         * @param query The Query to convert to a SIF_Query
         * @param version The version of SIF to render the SIF_Query xml in
         * @param zone The IZone to retrieve query settings from, or null
         * @return a SIF_Query instance
         */
        public static SIF_Query CreateSIF_Query(Query query, SifVersion version, IZone zone)
        {
            bool allowFieldRestrictions = query.HasFieldRestrictions;
            if (allowFieldRestrictions && zone != null)
            {
                allowFieldRestrictions = !zone.Properties.NoRequestIndividualElements;
            }
            return CreateSIF_Query(query, version, allowFieldRestrictions);
        }

        /**
         * Creates a SIF_Query element from the specified Adk query object using
         * the specified version of SIF
         * @param query The Query to convert to a SIF_Query
         * @param version The version of SIF to render the SIF_Query xml in
         * @param allowFieldRestrictions True if the field restrictions in the query should be rendered
         * @return a SIF_Query object
         */
        public static SIF_Query CreateSIF_Query(Query query, SifVersion version, bool allowFieldRestrictions)
        {

            SIF_QueryObject sqo = new SIF_QueryObject(query.ObjectType.Tag(version));
            SIF_Query sifQ = new SIF_Query(sqo);
            if (query.HasConditions)
            {
                sifQ.SIF_ConditionGroup = createConditionGroup(query, version);
            }

            if (allowFieldRestrictions && query.HasFieldRestrictions)
            {
                foreach( ElementRef elementRef in query.FieldRestrictionRefs )
                {
                    String path = null;
                    IElementDef field = elementRef.Field;
                    if( field != null )
                    {
                        if( !field.IsSupported( version ) )
                        {
                            continue;
                        }
                        path = field.GetSQPPath( version );
                    }
                    if( path == null )
                    {
                        path = elementRef.XPath;
                    }
                    if (path != null)
                    {
                        path = Adk.Dtd.TranslateSQP(query.ObjectType, path, version);
                        sqo.AddSIF_Element(new SIF_Element(path));
                    }
                }
            }

            return sifQ;

        }

        private static SIF_ConditionGroup createConditionGroup(Query query, SifVersion effectiveVersion)
        {

            // Create the hierarchy SIF_ConditionGroup
            //                              >    SIF_Conditons
            //                                        > SIF_Condition

            // From 
            //                       ConditionGroup
            //								> [ConditionGroup (Optional)]
            //										> Condition

            SIF_ConditionGroup returnGroup = new SIF_ConditionGroup();
            returnGroup.Type = ConditionType.NONE.ToString();
            ConditionGroup cg = query.RootConditionGroup;
            ConditionGroup[] groups = cg.Groups;
            if (groups != null && groups.Length > 0)
            {
                //	
                //	There's one or more ConditionGroups...
                // 	These get translated to SIF_Conditions elements
                //
                if (cg.Operator == GroupOperator.Or)
                {
                    returnGroup.Type = ConditionType.OR.ToString();
                }
                else if (cg.Operator == GroupOperator.And)
                {
                    returnGroup.Type = ConditionType.AND.ToString();
                }

                foreach (ConditionGroup group in groups)
                {
                    returnGroup.AddSIF_Conditions(createConditions(query, group, effectiveVersion));
                }
            }
            else
            {
                //
                //	There are no SIF_Conditions groups, so build one...
                //
                returnGroup.AddSIF_Conditions(createConditions(query, cg, effectiveVersion));
            }
            return returnGroup;
        }

        private static SIF_Conditions createConditions(Query query, ConditionGroup group, SifVersion effectiveVersion)
        {
            ConditionType typ = ConditionType.NONE;
            if (group.Operator == GroupOperator.And)
            {
                typ = ConditionType.AND;
            }
            else if (group.Operator == GroupOperator.Or)
            {
                typ = ConditionType.OR;
            }
            Condition[] conditions = group.Conditions;
            SIF_Conditions conds = new SIF_Conditions(conditions.Length > 1 ? typ : ConditionType.NONE);
            foreach (Condition c in conditions)
            {
                conds.AddSIF_Condition(
                    c.GetXPath(query, effectiveVersion),
                    Operators.Wrap(c.Operators.ToString()),
                    c.Value);
            }
            return conds;

        }


        /* (non-Javadoc)
         * @see com.OpenADK.Library.impl.ISIFPrimitives#sifProvision(com.OpenADK.Library.IZone, com.OpenADK.Library.infra.SIF_Provision)
         */
        public SIF_Ack SifProvision(IZone zone,
                SIF_ProvideObjects providedObjects,
                SIF_SubscribeObjects subscribeObjects,
                SIF_PublishAddObjects publishAddObjects,
                SIF_PublishChangeObjects publishChangeObjects,
                SIF_PublishDeleteObjects publishDeleteObjects,
                SIF_RequestObjects requestObjects,
                SIF_RespondObjects respondObjects)
        {

            SIF_Provision msg = new SIF_Provision(((ZoneImpl)zone).HighestEffectiveZISVersion);
            if (providedObjects != null)
            {
                msg.SIF_ProvideObjects = providedObjects;
            }
            if (publishAddObjects != null)
            {
                msg.SIF_PublishAddObjects = publishAddObjects;
            }
            if (publishChangeObjects != null)
            {
                msg.SIF_PublishChangeObjects = publishChangeObjects;
            }
            if (publishDeleteObjects != null)
            {
                msg.SIF_PublishDeleteObjects = publishDeleteObjects;
            }
            if (subscribeObjects != null)
            {
                msg.SIF_SubscribeObjects = subscribeObjects;
            }
            if (requestObjects != null)
            {
                msg.SIF_RequestObjects = requestObjects;
            }
            if (respondObjects != null)
            {
                msg.SIF_RespondObjects = respondObjects;
            }

            return ((ZoneImpl)zone).Dispatcher.send(msg);
        }



        /// <summary>
        ///  Attempts to parse attributes out of the source message enough to make a valid
        ///  SIF_Ack with a SIF_Error. This is useful in conditions where the source message cannot
        /// be parsed by the ADK
        /// </summary>
        /// <param name="sourceMessage"> The original message as a string</param>
        /// <param name="error">The error to place in the SIF_Ack/SIF_Error</param>
        /// <param name="zone">The zone associated with this message</param>
        /// <returns></returns>
        /// <exception cref="AdkMessagingException"></exception>
        public static SIF_Ack ackError(String sourceMessage, SifException error, ZoneImpl zone)
        {
            SifMessageInfo parsed = null;
            try
            {
                StringReader reader = new StringReader(sourceMessage);
                parsed = SifMessageInfo.Parse(reader, false, zone);
                reader.Close();
            }
            catch (Exception e)
            {
                zone.Log.Error(e, e);
            }

            SIF_Ack errorAck = new SIF_Ack(zone.HighestEffectiveZISVersion );

            if (parsed != null)
            {
                // Set SIFVersion, OriginalSourceId, and OriginalMsgId;
                if (parsed.SifVersion != null)
                {
                    errorAck.SifVersion = parsed.SifVersion;
                }
                errorAck.SIF_OriginalMsgId = parsed.GetAttribute("SIF_MsgId");
                errorAck.SIF_OriginalSourceId = parsed.GetAttribute("SIF_SourceId");
            }
            SetRequiredAckValues(errorAck);

            SIF_Error newErr = new SIF_Error();
            newErr.SIF_Category = (int)error.ErrorCategory;
            newErr.SIF_Code = error.ErrorCode;
            newErr.SIF_Desc = error.ErrorDesc;
            newErr.SIF_ExtendedDesc = error.ErrorExtDesc;
            errorAck.SIF_Error = newErr;

            return errorAck;
        }

        /**
         * If the SIF_OriginalMsgID or SIF_OriginalSourceId are not set,
         * process according to Infrastructure resolution #157
         * @param errorAck
         */
        public static void SetRequiredAckValues(SIF_Ack errorAck)
        {
            //  Return a SIF_Ack with a blank SIF_OriginalSourceId and
            //  SIF_OriginalMsgId per SIFInfra resolution #157
            // Also See 4.1.2.1 SIF_Message processing
            if (errorAck.GetField(InfraDTD.SIF_ACK_SIF_ORIGINALMSGID) == null)
            {
                // Set SIF_OriginalMsgId to xsi:nill
                errorAck.SetField(InfraDTD.SIF_ACK_SIF_ORIGINALMSGID, new SifString(null));
            }
            if (errorAck.GetField(InfraDTD.SIF_ACK_SIF_ORIGINALSOURCEID) == null)
            {
                // Set SIF_OriginalSource to an empty string
                errorAck.SIF_OriginalSourceId = "";
            }
        }


    }

}//end namespace
