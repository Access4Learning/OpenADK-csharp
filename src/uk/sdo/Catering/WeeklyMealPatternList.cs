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
using OpenADK.Library.uk.Common;

namespace OpenADK.Library.uk.Catering{

/// <summary>A WeeklyMealPatternList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class WeeklyMealPatternList : SifList<WeeklyMealPattern>
{
	/// <summary>
	/// Creates an instance of a WeeklyMealPatternList
	/// </summary>
	public WeeklyMealPatternList() : base ( CateringDTD.WEEKLYMEALPATTERNLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="weeklyMealPattern">A WeeklyMealPattern</param>
	///
	public WeeklyMealPatternList( WeeklyMealPattern weeklyMealPattern ) : base( CateringDTD.WEEKLYMEALPATTERNLIST )
	{
		this.SafeAddChild( CateringDTD.WEEKLYMEALPATTERNLIST_WEEKLYMEALPATTERN, weeklyMealPattern );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected WeeklyMealPatternList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { CateringDTD.WEEKLYMEALPATTERNLIST_WEEKLYMEALPATTERN }; }
	}

	///<summary>Adds the value of the <c>&lt;WeeklyMealPattern&gt;</c> element.</summary>
	/// <param name="StartDate">Date Specific pattern starts</param>
	/// <param name="EndDate">Date Specific pattern Ends.</param>
	/// <param name="Meals">A Meals</param>
	///<remarks>
	/// <para>This form of <c>setWeeklyMealPattern</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddWeeklyMealPattern</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void AddWeeklyMealPattern( DateTime? StartDate, DateTime? EndDate, Meal Meals ) {
		AddChild( CateringDTD.WEEKLYMEALPATTERNLIST_WEEKLYMEALPATTERN, new WeeklyMealPattern( StartDate, EndDate, Meals ) );
	}

}}
