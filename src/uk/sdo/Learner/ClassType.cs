// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.uk.Learner
{
	///<summary>
	/// Defines the set of values that can be specified whenever a ClassType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a ClassType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class ClassType : SifEnum
	{
	/// <summary>Nursery Class (England) ("N")</summary>
	public static readonly ClassType NURSERY = new ClassType("N");

	/// <summary>Not a Nursery Class (England) ("O")</summary>
	public static readonly ClassType NON_NURSERY = new ClassType("O");

	/// <summary>Nursery Special Class/Unit (Wales) ("S")</summary>
	public static readonly ClassType NURSERY_SPECIAL_WALES = new ClassType("S");

	///<summary>Wrap an arbitrary string value in a ClassType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static ClassType Wrap( String wrappedValue ) {
		return new ClassType( wrappedValue );
	}

	private ClassType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
