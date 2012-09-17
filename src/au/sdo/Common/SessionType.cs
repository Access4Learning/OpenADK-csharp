// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.au.Common
{
	///<summary>
	/// Defines the set of values that can be specified whenever a SessionType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a SessionType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class SessionType : SifEnum
	{
	/// <summary>Semester ("0828")</summary>
	public static readonly SessionType C0828_SEMESTER = new SessionType("0828");

	/// <summary>Full school year ("0827")</summary>
	public static readonly SessionType C0827_FULL_SCHOOL_YEAR = new SessionType("0827");

	/// <summary>Twelve month ("0837")</summary>
	public static readonly SessionType C0837_TWELVE_MONTH = new SessionType("0837");

	/// <summary>Quarter ("0830")</summary>
	public static readonly SessionType C0830_QUARTER = new SessionType("0830");

	/// <summary>Mini-term ("0832")</summary>
	public static readonly SessionType C0832_MINITERM = new SessionType("0832");

	/// <summary>Trimester ("0829")</summary>
	public static readonly SessionType C0829_TRIMESTER = new SessionType("0829");

	/// <summary>Other ("9999")</summary>
	public static readonly SessionType C9999_OTHER = new SessionType("9999");

	/// <summary>Summer term ("0833")</summary>
	public static readonly SessionType C0833_SUMMER_TERM = new SessionType("0833");

	///<summary>Wrap an arbitrary string value in a SessionType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static SessionType Wrap( String wrappedValue ) {
		return new SessionType( wrappedValue );
	}

	private SessionType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
