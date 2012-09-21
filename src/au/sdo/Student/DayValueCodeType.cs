// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.au.Student
{
	///<summary>
	/// Defines the set of values that can be specified whenever a DayValueCodeType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a DayValueCodeType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class DayValueCodeType : SifEnum
	{
	/// <summary>All Day ("Full")</summary>
	public static readonly DayValueCodeType Full_ALL_DAY = new DayValueCodeType("Full");

	/// <summary>Morning ("AM")</summary>
	public static readonly DayValueCodeType AM_MORNING = new DayValueCodeType("AM");

	/// <summary>Afternoon ("PM")</summary>
	public static readonly DayValueCodeType PM_AFTERNOON = new DayValueCodeType("PM");

	/// <summary>Not Applicable ("N/A")</summary>
	public static readonly DayValueCodeType N_A_NOT_APPLICABLE = new DayValueCodeType("N/A");

	/// <summary>Partial Day ("Partial")</summary>
	public static readonly DayValueCodeType Partial_PARTIAL_DAY = new DayValueCodeType("Partial");

	///<summary>Wrap an arbitrary string value in a DayValueCodeType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static DayValueCodeType Wrap( String wrappedValue ) {
		return new DayValueCodeType( wrappedValue );
	}

	private DayValueCodeType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
