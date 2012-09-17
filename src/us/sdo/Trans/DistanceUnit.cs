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
	/// Defines the set of values that can be specified whenever a DistanceUnit
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a DistanceUnit object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class DistanceUnit : SifEnum
	{
	/// <summary>Kilometers ("km")</summary>
	public static readonly DistanceUnit KM = new DistanceUnit("km");

	/// <summary>Miles ("m")</summary>
	public static readonly DistanceUnit M = new DistanceUnit("m");

	///<summary>Wrap an arbitrary string value in a DistanceUnit object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static DistanceUnit Wrap( String wrappedValue ) {
		return new DistanceUnit( wrappedValue );
	}

	private DistanceUnit( string enumDefValue ) : base( enumDefValue ) {}
	}
}
