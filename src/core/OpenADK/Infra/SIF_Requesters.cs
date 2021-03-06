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

/// <summary>A SIF_Requesters</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class SIF_Requesters : SifKeyedList<SIF_Requester>
{
	/// <summary>
	/// Creates an instance of a SIF_Requesters
	/// </summary>
	public SIF_Requesters() : base ( InfraDTD.SIF_REQUESTERS ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="sifRequester">A SIF_Requester</param>
	///
	public SIF_Requesters( SIF_Requester sifRequester ) : base( InfraDTD.SIF_REQUESTERS )
	{
		this.SafeAddChild( InfraDTD.SIF_REQUESTERS_SIF_REQUESTER, sifRequester );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SIF_Requesters( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;SIF_Requester&gt;</c> element.</summary>
	/// <param name="SourceId">The identifier of the SIF node that is providing objects. This is the agent or ZIS identifier that would appear in the SIF_SourceId field of any SIF_Header created by the SIF node.</param>
	///<remarks>
	/// <para>This form of <c>setSIF_Requester</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddSIF_Requester</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void AddSIF_Requester( string SourceId ) {
		AddChild( InfraDTD.SIF_REQUESTERS_SIF_REQUESTER, new SIF_Requester( SourceId ) );
	}

}}
