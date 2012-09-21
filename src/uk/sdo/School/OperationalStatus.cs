// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.uk.School
{
	///<summary>
	/// Defines the set of values that can be specified whenever an OperationalStatus
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an OperationalStatus object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class OperationalStatus : SifEnum
	{
	/// <summary>Open, but proposed to close ("3")</summary>
	public static readonly OperationalStatus PROPOSED_TO_CLOSE = new OperationalStatus("3");

	/// <summary>Proposed to open ("4")</summary>
	public static readonly OperationalStatus PROPOSED_TO_OPEN = new OperationalStatus("4");

	/// <summary>De-registered as EY Setting ("5")</summary>
	public static readonly OperationalStatus DEREGISTERED_AS_EY_SETTING = new OperationalStatus("5");

	/// <summary>Open ("1")</summary>
	public static readonly OperationalStatus OPEN = new OperationalStatus("1");

	/// <summary>Closed ("2")</summary>
	public static readonly OperationalStatus CLOSED = new OperationalStatus("2");

	///<summary>Wrap an arbitrary string value in an OperationalStatus object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static OperationalStatus Wrap( String wrappedValue ) {
		return new OperationalStatus( wrappedValue );
	}

	private OperationalStatus( string enumDefValue ) : base( enumDefValue ) {}
	}
}
