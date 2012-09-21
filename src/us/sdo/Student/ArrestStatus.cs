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
	/// Defines the set of values that can be specified whenever an ArrestStatus
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an ArrestStatus object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class ArrestStatus : SifEnum
	{
	/// <summary>Pending ("Pending")</summary>
	public static readonly ArrestStatus PENDING = new ArrestStatus("Pending");

	/// <summary>Yes ("Yes")</summary>
	public static readonly ArrestStatus YES = new ArrestStatus("Yes");

	/// <summary>No ("No")</summary>
	public static readonly ArrestStatus NO = new ArrestStatus("No");

	///<summary>Wrap an arbitrary string value in an ArrestStatus object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static ArrestStatus Wrap( String wrappedValue ) {
		return new ArrestStatus( wrappedValue );
	}

	private ArrestStatus( string enumDefValue ) : base( enumDefValue ) {}
	}
}
