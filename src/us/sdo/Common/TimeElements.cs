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

namespace OpenADK.Library.us.Common{

/// <summary>A TimeElements</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class TimeElements : SifKeyedList<TimeElement>
{
	/// <summary>
	/// Creates an instance of a TimeElements
	/// </summary>
	public TimeElements() : base ( CommonDTD.TIMEELEMENTS ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="timeElement">A TimeElement</param>
	///
	public TimeElements( TimeElement timeElement ) : base( CommonDTD.TIMEELEMENTS )
	{
		this.SafeAddChild( CommonDTD.TIMEELEMENTS_TIMEELEMENT, timeElement );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected TimeElements( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;TimeElement&gt;</c> element.</summary>
	/// <param name="Type">A Type</param>
	/// <param name="Code">This element provides a place for the application to send structured data (code values, unique identifier, timestamps). This code value can, depending upon the use case agreement between agents, be used to qualify the data in the Value element.</param>
	/// <param name="Name">Contains a human-readable description of the value in Value.</param>
	/// <param name="Value">Contains the human-readable value.</param>
	///<remarks>
	/// <para>This form of <c>setTimeElement</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddTimeElement</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void AddTimeElement( TimeElementType Type, string Code, string Name, string Value ) {
		AddChild( CommonDTD.TIMEELEMENTS_TIMEELEMENT, new TimeElement( Type, Code, Name, Value ) );
	}

}}
