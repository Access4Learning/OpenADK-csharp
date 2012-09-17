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
	/// Defines the set of values that can be specified whenever a RecordClosureReason
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a RecordClosureReason object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class RecordClosureReason : SifEnum
	{
	/// <summary>SchoolExit ("SchoolExit")</summary>
	public static readonly RecordClosureReason SCHOOLEXIT = new RecordClosureReason("SchoolExit");

	/// <summary>EndOfYear ("EndOfYear")</summary>
	public static readonly RecordClosureReason ENDOFYEAR = new RecordClosureReason("EndOfYear");

	/// <summary>TimeDependentDataChange ("TimeDependentDataChange")</summary>
	public static readonly RecordClosureReason TIMEDEPENDENTDATACHANGE = new RecordClosureReason("TimeDependentDataChange");

	///<summary>Wrap an arbitrary string value in a RecordClosureReason object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static RecordClosureReason Wrap( String wrappedValue ) {
		return new RecordClosureReason( wrappedValue );
	}

	private RecordClosureReason( string enumDefValue ) : base( enumDefValue ) {}
	}
}
