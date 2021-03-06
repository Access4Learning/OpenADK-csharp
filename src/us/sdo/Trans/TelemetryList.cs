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

/// <summary>A TelemetryList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class TelemetryList : SifKeyedList<Telemetry>
{
	/// <summary>
	/// Creates an instance of a TelemetryList
	/// </summary>
	public TelemetryList() : base ( TransDTD.TELEMETRYLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="telemetry">Indicators and values provided by the hardware along with this position snapshot. For events, it is safe to assume that one of these items triggered the hardware to issue an update.</param>
	///
	public TelemetryList( Telemetry telemetry ) : base( TransDTD.TELEMETRYLIST )
	{
		this.SafeAddChild( TransDTD.TELEMETRYLIST_TELEMETRY, telemetry );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected TelemetryList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;Telemetry&gt;</c> element.</summary>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;Telemetry&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setTelemetry</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddTelemetry</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddTelemetry( string Value ) {
		AddChild( TransDTD.TELEMETRYLIST_TELEMETRY, new Telemetry( Value ) );
	}

}}
