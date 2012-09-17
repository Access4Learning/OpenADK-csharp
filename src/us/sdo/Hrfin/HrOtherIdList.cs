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

namespace OpenADK.Library.us.Hrfin{

/// <summary>An HrOtherIdList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class HrOtherIdList : SifKeyedList<OtherId>
{
	/// <summary>
	/// Creates an instance of an HrOtherIdList
	/// </summary>
	public HrOtherIdList() : base ( HrfinDTD.HROTHERIDLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="otherId">List all "other" IDs associated with employee.</param>
	///
	public HrOtherIdList( OtherId otherId ) : base( HrfinDTD.HROTHERIDLIST )
	{
		this.SafeAddChild( HrfinDTD.HROTHERIDLIST_OTHERID, otherId );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected HrOtherIdList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;OtherId&gt;</c> element.</summary>
	/// <param name="Type">Code that defines the type of this other ID.  Note: A subset of valid values may be specified in data objects.</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;OtherId&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setOtherId</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddOtherId</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddOtherId( OtherIdType Type, string Value ) {
		AddChild( HrfinDTD.HROTHERIDLIST_OTHERID, new OtherId( Type, Value ) );
	}

}}
