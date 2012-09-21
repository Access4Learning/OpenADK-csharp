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
	/// Defines the set of values that can be specified whenever an AttendanceStatus
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an AttendanceStatus object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	[Serializable]
	public class AttendanceStatus : SifEnum
	{
	/// <summary>NA ("NA")</summary>
	public static readonly AttendanceStatus NA = new AttendanceStatus("NA");

	/// <summary>Excused ("Excused")</summary>
	public static readonly AttendanceStatus EXCUSED = new AttendanceStatus("Excused");

	/// <summary>Unexcused ("Unexcused")</summary>
	public static readonly AttendanceStatus UNEXCUSED = new AttendanceStatus("Unexcused");

	/// <summary>Unknown ("Unknown")</summary>
	public static readonly AttendanceStatus UNKNOWN = new AttendanceStatus("Unknown");

	///<summary>Wrap an arbitrary string value in an AttendanceStatus object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static AttendanceStatus Wrap( String wrappedValue ) {
		return new AttendanceStatus( wrappedValue );
	}

	private AttendanceStatus( string enumDefValue ) : base( enumDefValue ) {}
	}
}
