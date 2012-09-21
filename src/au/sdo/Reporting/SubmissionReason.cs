// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.au.Reporting
{
	///<summary>
	/// Defines the set of values that can be specified whenever a SubmissionReason
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a SubmissionReason object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class SubmissionReason : SifEnum
	{
	/// <summary>Initial ("Initial")</summary>
	public static readonly SubmissionReason INITIAL = new SubmissionReason("Initial");

	/// <summary>Revision ("Revision")</summary>
	public static readonly SubmissionReason REVISION = new SubmissionReason("Revision");

	/// <summary>Correction ("Correction")</summary>
	public static readonly SubmissionReason CORRECTION = new SubmissionReason("Correction");

	/// <summary>Addition ("Addition")</summary>
	public static readonly SubmissionReason ADDITION = new SubmissionReason("Addition");

	///<summary>Wrap an arbitrary string value in a SubmissionReason object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static SubmissionReason Wrap( String wrappedValue ) {
		return new SubmissionReason( wrappedValue );
	}

	private SubmissionReason( string enumDefValue ) : base( enumDefValue ) {}
	}
}
