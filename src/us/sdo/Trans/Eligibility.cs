// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Trans
{
	///<summary>
	/// Defines the set of values that can be specified whenever an Eligibility
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an Eligibility object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	[Serializable]
	public class Eligibility : SifEnum
	{
	/// <summary>Bus Out of Attendance ("BusOutOfAttendance")</summary>
	public static readonly Eligibility BUS_OUT_OF_ATTENDANCE = new Eligibility("BusOutOfAttendance");

	/// <summary>Parent Pay ("ParentPay")</summary>
	public static readonly Eligibility PARENT_PAY = new Eligibility("ParentPay");

	/// <summary>Unknown ("Unknown")</summary>
	public static readonly Eligibility UNKNOWN = new Eligibility("Unknown");

	/// <summary>Walk ("Walk")</summary>
	public static readonly Eligibility WALK = new Eligibility("Walk");

	/// <summary>Bus Hazard ("BusHazard")</summary>
	public static readonly Eligibility BUS_HAZARD = new Eligibility("BusHazard");

	/// <summary>Bus ("Bus")</summary>
	public static readonly Eligibility BUS = new Eligibility("Bus");

	///<summary>Wrap an arbitrary string value in an Eligibility object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static Eligibility Wrap( String wrappedValue ) {
		return new Eligibility( wrappedValue );
	}

	private Eligibility( string enumDefValue ) : base( enumDefValue ) {}
	}
}
