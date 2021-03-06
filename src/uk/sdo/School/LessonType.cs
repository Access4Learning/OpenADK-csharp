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
	/// Defines the set of values that can be specified whenever a LessonType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a LessonType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class LessonType : SifEnum
	{
	/// <summary>teaching ("T")</summary>
	public static readonly LessonType T_TEACHING = new LessonType("T");

	/// <summary>non-teaching ("N")</summary>
	public static readonly LessonType N_NONTEACHING = new LessonType("N");

	///<summary>Wrap an arbitrary string value in a LessonType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static LessonType Wrap( String wrappedValue ) {
		return new LessonType( wrappedValue );
	}

	private LessonType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
