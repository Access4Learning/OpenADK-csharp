//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;

namespace OpenADK.Library.Tools.Policy
{
    public class PolicyManagerImpl : PolicyManager
    {
        private readonly PolicyFactory fPolicyFactory;
	
	public PolicyManagerImpl( Agent agentInstance )
	{
		fPolicyFactory = (PolicyFactory)ObjectFactory.GetInstance().CreateInstance( ObjectFactory.ADKFactoryType.POLICY_FACTORY, agentInstance );
	}
	
	
	
	public override void ApplyOutboundPolicy(SifMessagePayload msg, IZone zone ) {
		
		SifMessageType pload = Adk.Dtd.GetElementType(msg.ElementDef.Name);
		switch( pload ){
            case SifMessageType.SIF_Request:
			    SetRequestPolicy((SIF_Request)msg, zone );
		        break;
		}
		
	}
	
	
	private void SetRequestPolicy( SIF_Request request, IZone zone )
	{
		SIF_Query query = request.SIF_Query;
		if( query == null ) {
			// SIF_ExtendedQuery and SIF_Example are not supported by ADK Policy yet
			return;
		}
		
		//
		// Object Request Policy
		//
		// Determine if there is policy in effect for this Query
		//
		String objectName = query.SIF_QueryObject.ObjectName;
		ObjectRequestPolicy requestPolicy = fPolicyFactory.GetRequestPolicy( zone, objectName );
		if( requestPolicy != null ){
			
			//
			// SIF_Request/SIF_Version policy
			//
			String requestVersions = requestPolicy.RequestVersion;
			if( requestVersions != null ){
				if( (Adk.Debug & AdkDebugFlags.Policy ) > 0 ){
					zone.Log.Info( "POLICY: Setting SIF_Request/SIF_Version to " + requestVersions );
				}
				// Clear the list of SIF Versions
				foreach( SIF_Version existingVersion in request.GetSIF_Versions() ){
					request.RemoveChild( existingVersion );
				}
				
				// The version will be a comma-delimited list. Set each of these
				// as SIF_Version elements, but also try to derive the most logical
				// version element to set the SIF Message/@Version attribute to
				// NOTE: Someone could theoretically set versions incorrectly, such
				// as "1.1,1.5r1". Multiple SIF_Version elements are not supported in
				// SIF 1.x, but we won't bother with validating incorrect settings. Policy
				// is power in the configurator's hands to use or abuse.

				String[] versions = requestVersions.Split( ',' );
				String lowestVersion = versions[0];
				foreach( String version in versions ){
				    String ver = version.Trim();
                    request.AddSIF_Version(new SIF_Version(ver));
                    if (lowestVersion.CompareTo(ver) > 0)
                    {
                        lowestVersion = ver;
					}
				}
				
				// Determine how the SIF_Message/@Version should be set to
				//  * If the policy is set to a single version, use it 
				//  * If a list, use the lowest
				//  * If *, ignore
				//  * if [major].*, use the lowest version supported
				if( lowestVersion.Length > 0  ){
					SifVersion newMsgVersion = null;
					if( lowestVersion.EndsWith( "*" ) ){
						try
						{
							// 2.*, requests go out with a message version of 2.0r1
							int major = int.Parse(  lowestVersion.Substring( 0, 1 ) );
							newMsgVersion = SifVersion.GetEarliest( major );
							
						} catch( FormatException iae ){
							zone.Log.Warn( 
									"POLICY: Error parsing ObjectRequestPolicy version '" + 
									requestVersions + "' : " + 
									iae.Message, iae );
						}
						
					} else {
						try
						{
							newMsgVersion = SifVersion.Parse( lowestVersion );
						} catch( FormatException iae ){
							zone.Log.Warn( 
									"POLICY: Error parsing ObjectRequestPolicy version '" + 
									requestVersions + "' : " + 
									iae.Message, iae );
						}
					}
					if( newMsgVersion != null ){
						if( (Adk.Debug & AdkDebugFlags.Policy ) > 0 ){
							zone.Log.Info( "POLICY: Setting SIF_Messaage/@Version to " + newMsgVersion );
						}
						request.SifVersion = newMsgVersion;
					}
				}
			}
			
			//
			// SIF_DestinationID policy
			//
			String requestSourceId = requestPolicy.RequestSourceId ;
			if( requestSourceId != null ){
				if( (Adk.Debug & AdkDebugFlags.Policy) > 0 ){
					zone.Log.Info( "POLICY: Setting SIF_Request SIF_DestinationID to " + requestPolicy.RequestSourceId );
				}
				request.SIF_Header.SIF_DestinationId = requestSourceId;
			}
		}
	}


    }
}
