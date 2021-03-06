// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Serialization;
using OpenADK.Library;
using OpenADK.Library.Global;
using OpenADK.Library.us.Common;

namespace OpenADK.Library.us.Trans{

/// <summary>The schedule for a bus route</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 1.1</para>
/// </remarks>
[Serializable]
public class BusRouteDetail : SifDataObject
{
	/// <summary>
	/// Creates an instance of a BusRouteDetail
	/// </summary>
	public BusRouteDetail() : base( Adk.SifVersion, TransDTD.BUSROUTEDETAIL ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">GUID that identifies this object.</param>
	///<param name="busRouteInfoRefId">GUID that identifies the bus route. See the BusRoute specification for more details.</param>
	///<param name="busStopInfoRefId">Describes a bus stop</param>
	///<param name="arrivalTime">The time that the bus associated with this route will stop at this bus stop.</param>
	///
	public BusRouteDetail( string refId, string busRouteInfoRefId, string busStopInfoRefId, DateTime? arrivalTime ) : base( Adk.SifVersion, TransDTD.BUSROUTEDETAIL )
	{
		this.RefId = refId;
		this.BusRouteInfoRefId = busRouteInfoRefId;
		this.BusStopInfoRefId = busStopInfoRefId;
		this.ArrivalTime = arrivalTime;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected BusRouteDetail( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { TransDTD.BUSROUTEDETAIL_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "GUID that identifies this object."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( TransDTD.BUSROUTEDETAIL_REFID ) ;
		}
		set
		{
			SetFieldValue( TransDTD.BUSROUTEDETAIL_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>BusRouteInfoRefId</c> attribute.
	/// </summary>
	/// <value> The <c>BusRouteInfoRefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "GUID that identifies the bus route. See the BusRoute specification for more details."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string BusRouteInfoRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( TransDTD.BUSROUTEDETAIL_BUSROUTEINFOREFID ) ;
		}
		set
		{
			SetFieldValue( TransDTD.BUSROUTEDETAIL_BUSROUTEINFOREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;BusStopInfoRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>BusStopInfoRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Describes a bus stop"</para>
	/// <para>This element is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering elements of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;BusStopInfoRefId&gt;</description></item>
	/// </list>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string BusStopInfoRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( TransDTD.BUSROUTEDETAIL_BUSSTOPINFOREFID ) ;
		}
		set
		{
			SetFieldValue( TransDTD.BUSROUTEDETAIL_BUSSTOPINFOREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ArrivalTime&gt;</c> element.
	/// </summary>
	/// <value> The <c>ArrivalTime</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The time that the bus associated with this route will stop at this bus stop."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public DateTime? ArrivalTime
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( TransDTD.BUSROUTEDETAIL_ARRIVALTIME ) ;
		}
		set
		{
			SetFieldValue( TransDTD.BUSROUTEDETAIL_ARRIVALTIME, new SifTime( value ), value );
		}
	}

}}
