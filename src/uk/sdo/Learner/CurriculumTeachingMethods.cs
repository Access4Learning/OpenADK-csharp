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
	/// Defines the set of values that can be specified whenever a CurriculumTeachingMethods
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a CurriculumTeachingMethods object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class CurriculumTeachingMethods : SifEnum
	{
	/// <summary>Significant and targeted differentiation ("CT2")</summary>
	public static readonly CurriculumTeachingMethods SIGNIFICANT_AND_TARGETED = new CurriculumTeachingMethods("CT2");

	/// <summary>Significant curriculum modifications ("CT4")</summary>
	public static readonly CurriculumTeachingMethods SIGNIFICANT_CURRICULUM = new CurriculumTeachingMethods("CT4");

	/// <summary>Some targeted differentiation ("CT1")</summary>
	public static readonly CurriculumTeachingMethods SOME_TARGETED_DIFFERENTIATION = new CurriculumTeachingMethods("CT1");

	/// <summary>Some curriculum modifications ("CT3")</summary>
	public static readonly CurriculumTeachingMethods SOME_CURRICULUM = new CurriculumTeachingMethods("CT3");

	///<summary>Wrap an arbitrary string value in a CurriculumTeachingMethods object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static CurriculumTeachingMethods Wrap( String wrappedValue ) {
		return new CurriculumTeachingMethods( wrappedValue );
	}

	private CurriculumTeachingMethods( string enumDefValue ) : base( enumDefValue ) {}
	}
}
