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

namespace OpenADK.Library.Infra{

/// <summary>Contains one of the SIF message types.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 1.1</para>
/// </remarks>
[Serializable]
public class SIF_Response : SifMessagePayload
{
	/// <summary>
	/// Creates an instance of a SIF_Response
	/// </summary>
	public SIF_Response() : base ( InfraDTD.SIF_RESPONSE ){}
	/// <summary>
	/// Creates an instance of a SIF_Response
	/// </summary>
	///  <param name="sifVersion">The version of SIF to render this message in</param>
	///
	public SIF_Response( SifVersion sifVersion ) : base( sifVersion, InfraDTD.SIF_RESPONSE ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SIF_Response( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Header&gt;</c> element.
	/// </summary>
	/// <value> A SIF_Header </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Header information associated with this message. The SIF_DestinationId needs to be the SIF_SourceId of the original SIF_Request message being processed."</para>
	/// <para>To remove the <c>SIF_Header</c>, set <c>SIF_Header</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public SIF_Header SIF_Header
	{
		get
		{
			return (SIF_Header)GetChild( InfraDTD.SIF_RESPONSE_SIF_HEADER);
		}
		set
		{
			RemoveChild( InfraDTD.SIF_RESPONSE_SIF_HEADER);
			if( value != null)
			{
				AddChild( InfraDTD.SIF_RESPONSE_SIF_HEADER, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_RequestMsgId&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_RequestMsgId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This is the message ID of the SIF_Request message being processed. It provides a unique match between a SIF_Response and a previous SIF_Request. Since the ID of each message from an agent is unique, the receiver of a SIF_Response message will be able to relate the SIF_Response to a SIF_Request that it sent out previously."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string SIF_RequestMsgId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InfraDTD.SIF_RESPONSE_SIF_REQUESTMSGID ) ;
		}
		set
		{
			SetFieldValue( InfraDTD.SIF_RESPONSE_SIF_REQUESTMSGID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Error&gt;</c> element.
	/// </summary>
	/// <value> A SIF_Error </value>
	/// <remarks>
	/// <para>To remove the <c>SIF_Error</c>, set <c>SIF_Error</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public SIF_Error SIF_Error
	{
		get
		{
			return (SIF_Error)GetChild( InfraDTD.SIF_RESPONSE_SIF_ERROR);
		}
		set
		{
			RemoveChild( InfraDTD.SIF_RESPONSE_SIF_ERROR);
			if( value != null)
			{
				AddChild( InfraDTD.SIF_RESPONSE_SIF_ERROR, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_PacketNumber&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_PacketNumber</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This element represents the index of the SIF_Response message in the sequence of packets that make up a complete response. Its value must be in the range of 1 through n, with n equal to the total number of packets that make up a response."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public int? SIF_PacketNumber
	{
		get
		{
			return (int?) GetSifSimpleFieldValue( InfraDTD.SIF_RESPONSE_SIF_PACKETNUMBER ) ;
		}
		set
		{
			SetFieldValue( InfraDTD.SIF_RESPONSE_SIF_PACKETNUMBER, new SifInt( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_MorePackets&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_MorePackets</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This element provides an indication as to whether there are more packets besides this one to make up a complete response."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string SIF_MorePackets
	{
		get
		{ 
			return GetFieldValue( InfraDTD.SIF_RESPONSE_SIF_MOREPACKETS );
		}
		set
		{
			SetField( InfraDTD.SIF_RESPONSE_SIF_MOREPACKETS, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;SIF_MorePackets&gt;</c> element.
	/// </summary>
	/// <param name="val">A YesNo object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This element provides an indication as to whether there are more packets besides this one to make up a complete response."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public void SetSIF_MorePackets( YesNo val )
	{
		SetField( InfraDTD.SIF_RESPONSE_SIF_MOREPACKETS, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_ObjectData&gt;</c> element.
	/// </summary>
	/// <value> A SIF_ObjectData </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "
	///         The SIF_ObjectData element contains the data objects matching the supplied criteria in the SIF_Request message if the
	///         SIF_Request contained SIF_Query.  If the SIF_Request contained SIF_ExtendedQuery, include SIF_ExtendedQueryResults.
	///         If the SIF_Error element is present, this element MUST be empty.
	///       "</para>
	/// <para>To remove the <c>SIF_ObjectData</c>, set <c>SIF_ObjectData</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public SIF_ObjectData SIF_ObjectData
	{
		get
		{
			return (SIF_ObjectData)GetChild( InfraDTD.SIF_RESPONSE_SIF_OBJECTDATA);
		}
		set
		{
			RemoveChild( InfraDTD.SIF_RESPONSE_SIF_OBJECTDATA);
			if( value != null)
			{
				AddChild( InfraDTD.SIF_RESPONSE_SIF_OBJECTDATA, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_ExtendedQueryResults&gt;</c> element.
	/// </summary>
	/// <value> A SIF_ExtendedQueryResults </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "
	///         This element contains the elements requested by SIF_ExtendedQuery in SIF_Request.
	///         If the SIF_Error element is present, this element MUST be empty.
	///       "</para>
	/// <para>To remove the <c>SIF_ExtendedQueryResults</c>, set <c>SIF_ExtendedQueryResults</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public SIF_ExtendedQueryResults SIF_ExtendedQueryResults
	{
		get
		{
			return (SIF_ExtendedQueryResults)GetChild( InfraDTD.SIF_RESPONSE_SIF_EXTENDEDQUERYRESULTS);
		}
		set
		{
			RemoveChild( InfraDTD.SIF_RESPONSE_SIF_EXTENDEDQUERYRESULTS);
			if( value != null)
			{
				AddChild( InfraDTD.SIF_RESPONSE_SIF_EXTENDEDQUERYRESULTS, value );
			}
		}
	}

}}
