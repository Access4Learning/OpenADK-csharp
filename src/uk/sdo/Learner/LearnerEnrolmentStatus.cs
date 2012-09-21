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
	/// Defines the set of values that can be specified whenever a LearnerEnrolmentStatus
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a LearnerEnrolmentStatus object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class LearnerEnrolmentStatus : SifEnum
	{
	/// <summary>Previous ("P")</summary>
	public static readonly LearnerEnrolmentStatus PREVIOUS = new LearnerEnrolmentStatus("P");

	/// <summary>Current Main (dual registration) ("M")</summary>
	public static readonly LearnerEnrolmentStatus CURRENT_MAIN = new LearnerEnrolmentStatus("M");

	/// <summary>Current (single registration at this school) ("C")</summary>
	public static readonly LearnerEnrolmentStatus CURRENT_SINGLE = new LearnerEnrolmentStatus("C");

	/// <summary>Guest (pupil not registered at this school but attending some lessons or sessions) ("G")</summary>
	public static readonly LearnerEnrolmentStatus GUEST_PUPIL = new LearnerEnrolmentStatus("G");

	/// <summary>Current Subsidiary (dual registration) ("S")</summary>
	public static readonly LearnerEnrolmentStatus CURRENT_SUBSIDIARY = new LearnerEnrolmentStatus("S");

	///<summary>Wrap an arbitrary string value in a LearnerEnrolmentStatus object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static LearnerEnrolmentStatus Wrap( String wrappedValue ) {
		return new LearnerEnrolmentStatus( wrappedValue );
	}

	private LearnerEnrolmentStatus( string enumDefValue ) : base( enumDefValue ) {}
	}
}
