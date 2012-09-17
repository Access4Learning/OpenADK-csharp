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
using OpenADK.Library.us.Gradebook;
using OpenADK.Library.us.Student;

namespace OpenADK.Library.us.Etranscripts{

/// <summary>An ActivityAwardHistory</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class ActivityAwardHistory : SifList<ActivityAward>
{
	/// <summary>
	/// Creates an instance of an ActivityAwardHistory
	/// </summary>
	public ActivityAwardHistory() : base ( EtranscriptsDTD.ACTIVITYAWARDHISTORY ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="activityAward">An ActivityAward</param>
	///
	public ActivityAwardHistory( ActivityAward activityAward ) : base( EtranscriptsDTD.ACTIVITYAWARDHISTORY )
	{
		this.SafeAddChild( EtranscriptsDTD.ACTIVITYAWARDHISTORY_ACTIVITYAWARD, activityAward );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ActivityAwardHistory( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
}}