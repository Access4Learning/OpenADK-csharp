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
	/// Defines the set of values that can be specified whenever a TermInfoSessionType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a TermInfoSessionType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class TermInfoSessionType : SifEnum
	{
	/// <summary>Semester ("0828")</summary>
	public static readonly TermInfoSessionType C0828_SEMESTER = new TermInfoSessionType("0828");

	/// <summary>Full school year ("0827")</summary>
	public static readonly TermInfoSessionType C0827_FULL_SCHOOL_YEAR = new TermInfoSessionType("0827");

	/// <summary>Twelve month ("0837")</summary>
	public static readonly TermInfoSessionType C0837_TWELVE_MONTH = new TermInfoSessionType("0837");

	/// <summary>Quarter ("0830")</summary>
	public static readonly TermInfoSessionType C0830_QUARTER = new TermInfoSessionType("0830");

	/// <summary>Mini-term ("0832")</summary>
	public static readonly TermInfoSessionType C0832_MINITERM = new TermInfoSessionType("0832");

	/// <summary>Trimester ("0829")</summary>
	public static readonly TermInfoSessionType C0829_TRIMESTER = new TermInfoSessionType("0829");

	/// <summary>Other ("9999")</summary>
	public static readonly TermInfoSessionType C9999_OTHER = new TermInfoSessionType("9999");

	/// <summary>Summer term ("0833")</summary>
	public static readonly TermInfoSessionType C0833_SUMMER_TERM = new TermInfoSessionType("0833");

	///<summary>Wrap an arbitrary string value in a TermInfoSessionType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static TermInfoSessionType Wrap( String wrappedValue ) {
		return new TermInfoSessionType( wrappedValue );
	}

	private TermInfoSessionType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
