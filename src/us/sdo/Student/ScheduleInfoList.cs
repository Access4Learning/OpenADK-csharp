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

namespace OpenADK.Library.us.Student{

/// <summary>A ScheduleInfoList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.1</para>
/// </remarks>
[Serializable]
public class ScheduleInfoList : SifKeyedList<ScheduleInfo>
{
	/// <summary>
	/// Creates an instance of a ScheduleInfoList
	/// </summary>
	public ScheduleInfoList() : base ( StudentDTD.SCHEDULEINFOLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="scheduleInfo">A ScheduleInfo</param>
	///
	public ScheduleInfoList( ScheduleInfo scheduleInfo ) : base( StudentDTD.SCHEDULEINFOLIST )
	{
		this.SafeAddChild( StudentDTD.SCHEDULEINFOLIST_SCHEDULEINFO, scheduleInfo );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ScheduleInfoList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;ScheduleInfo&gt;</c> element.</summary>
	/// <param name="TermInfoRefId">The schedule-related information for a section repeating for each term in which the section is scheduled. The TermInfoRefId attribute value should repeat as necessary to show the appropriate relationship between meeting times, teachers and rooms.</param>
	///<remarks>
	/// <para>This form of <c>setScheduleInfo</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddScheduleInfo</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public void AddScheduleInfo( string TermInfoRefId ) {
		AddChild( StudentDTD.SCHEDULEINFOLIST_SCHEDULEINFO, new ScheduleInfo( TermInfoRefId ) );
	}

}}