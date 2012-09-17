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
	/// Defines the set of values that can be specified whenever a ProgressLevelType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a ProgressLevelType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class ProgressLevelType : SifEnum
	{
	/// <summary>At level ("At")</summary>
	public static readonly ProgressLevelType At_AT_LEVEL = new ProgressLevelType("At");

	/// <summary>Above level ("Above")</summary>
	public static readonly ProgressLevelType Above_ABOVE_LEVEL = new ProgressLevelType("Above");

	/// <summary>Below level ("Below")</summary>
	public static readonly ProgressLevelType Below_BELOW_LEVEL = new ProgressLevelType("Below");

	///<summary>Wrap an arbitrary string value in a ProgressLevelType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static ProgressLevelType Wrap( String wrappedValue ) {
		return new ProgressLevelType( wrappedValue );
	}

	private ProgressLevelType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
