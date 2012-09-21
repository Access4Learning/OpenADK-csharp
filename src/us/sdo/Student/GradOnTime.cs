// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Student
{
	///<summary>
	/// Defines the set of values that can be specified whenever a GradOnTime
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a GradOnTime object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class GradOnTime : SifEnum
	{
	/// <summary>NA ("NA")</summary>
	public static readonly GradOnTime NA = new GradOnTime("NA");

	/// <summary>Yes ("Yes")</summary>
	public static readonly GradOnTime YES = new GradOnTime("Yes");

	/// <summary>Unavailable ("Unavailable")</summary>
	public static readonly GradOnTime UNAVAILABLE = new GradOnTime("Unavailable");

	/// <summary>No ("No")</summary>
	public static readonly GradOnTime NO = new GradOnTime("No");

	///<summary>Wrap an arbitrary string value in a GradOnTime object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static GradOnTime Wrap( String wrappedValue ) {
		return new GradOnTime( wrappedValue );
	}

	private GradOnTime( string enumDefValue ) : base( enumDefValue ) {}
	}
}
