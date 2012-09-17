// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Common
{
	///<summary>
	/// Defines the set of values that can be specified whenever an ExceptionalityPriority
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an ExceptionalityPriority object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class ExceptionalityPriority : SifEnum
	{
	/// <summary>Identifies relative severity of disability - only one can be primary ("Primary")</summary>
	public static readonly ExceptionalityPriority PRIMARY = new ExceptionalityPriority("Primary");

	/// <summary>Multiple disabilities can be identified as "Additional" ("Additional")</summary>
	public static readonly ExceptionalityPriority ADDITIONAL = new ExceptionalityPriority("Additional");

	/// <summary>Identifies relative severity of disability - only one can be tertiary ("Tertiary")</summary>
	public static readonly ExceptionalityPriority TERTIARY = new ExceptionalityPriority("Tertiary");

	/// <summary>Identifies relative severity of disability - only one can be secondary ("Secondary")</summary>
	public static readonly ExceptionalityPriority SECONDARY = new ExceptionalityPriority("Secondary");

	///<summary>Wrap an arbitrary string value in an ExceptionalityPriority object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static ExceptionalityPriority Wrap( String wrappedValue ) {
		return new ExceptionalityPriority( wrappedValue );
	}

	private ExceptionalityPriority( string enumDefValue ) : base( enumDefValue ) {}
	}
}
