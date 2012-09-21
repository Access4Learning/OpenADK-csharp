// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.uk.Learning
{
	///<summary>
	/// Defines the set of values that can be specified whenever a NonTeachingActivityType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a NonTeachingActivityType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class NonTeachingActivityType : SifEnum
	{
	/// <summary>teacher unavailable (but in school) ("U")</summary>
	public static readonly NonTeachingActivityType U_TEACHER_UNAVAILABLE_BUT_IN_SCHOOL = new NonTeachingActivityType("U");

	/// <summary>teacher unavailable (out of school) ("X")</summary>
	public static readonly NonTeachingActivityType X_TEACHER_UNAVAILABLE_OUT_OF_SCHOOL = new NonTeachingActivityType("X");

	/// <summary>outside school hours (eg: clubs) ("O")</summary>
	public static readonly NonTeachingActivityType O_OUTSIDE_SCHOOL_HOURS_EG_CLUBS = new NonTeachingActivityType("O");

	/// <summary>lunch ("L")</summary>
	public static readonly NonTeachingActivityType L_LUNCH = new NonTeachingActivityType("L");

	/// <summary>registration ("R")</summary>
	public static readonly NonTeachingActivityType R_REGISTRATION = new NonTeachingActivityType("R");

	/// <summary>other (or unspecified) non-teaching ("N")</summary>
	public static readonly NonTeachingActivityType N_OT_OR_UNSPECIFIED_NONTEACHING = new NonTeachingActivityType("N");

	/// <summary>break ("B")</summary>
	public static readonly NonTeachingActivityType B_BREAK = new NonTeachingActivityType("B");

	///<summary>Wrap an arbitrary string value in a NonTeachingActivityType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static NonTeachingActivityType Wrap( String wrappedValue ) {
		return new NonTeachingActivityType( wrappedValue );
	}

	private NonTeachingActivityType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
