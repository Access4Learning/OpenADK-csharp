// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Profdev
{
	///<summary>
	/// Defines the set of values that can be specified whenever a TeachingCredentialBasis
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a TeachingCredentialBasis object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class TeachingCredentialBasis : SifEnum
	{
	/// <summary>Special/alternative program completion ("1240")</summary>
	public static readonly TeachingCredentialBasis C0395_1240_SPECIAL_PROGRAM = new TeachingCredentialBasis("1240");

	/// <summary>Doctoral degree ("1238")</summary>
	public static readonly TeachingCredentialBasis DOCTORAL = new TeachingCredentialBasis("1238");

	/// <summary>Credentials based on reciprocation with another state ("1242")</summary>
	public static readonly TeachingCredentialBasis C0395_1242_RECIPROCATION_CREDENTIALS = new TeachingCredentialBasis("1242");

	/// <summary>5-year bachelor's degree ("1236")</summary>
	public static readonly TeachingCredentialBasis C0395_1236_5_YEAR_BACHELORS = new TeachingCredentialBasis("1236");

	/// <summary>Master's degree ("1237")</summary>
	public static readonly TeachingCredentialBasis C0395_1237_MASTERS = new TeachingCredentialBasis("1237");

	/// <summary>Relevant experience ("1241")</summary>
	public static readonly TeachingCredentialBasis C0395_1241_RELAVENT_EXPERIENCE = new TeachingCredentialBasis("1241");

	/// <summary>Doctoral degree ("1238")</summary>
	public static readonly TeachingCredentialBasis C0395_1238_DOCTORAL = new TeachingCredentialBasis("1238");

	/// <summary>Master's degree ("1237")</summary>
	public static readonly TeachingCredentialBasis MASTERS = new TeachingCredentialBasis("1237");

	/// <summary>Credentials based on reciprocation with another state ("1242")</summary>
	public static readonly TeachingCredentialBasis RECIPROCATION = new TeachingCredentialBasis("1242");

	/// <summary>4-year bachelor's degree ("1235")</summary>
	public static readonly TeachingCredentialBasis C0395_1235_4_YEAR_BACHELORS = new TeachingCredentialBasis("1235");

	/// <summary>Met state testing requirement ("1239")</summary>
	public static readonly TeachingCredentialBasis MET_STATE_REQ = new TeachingCredentialBasis("1239");

	/// <summary>5-year bachelor's degree ("1236")</summary>
	public static readonly TeachingCredentialBasis FIVE_YEAR_BACHELOR = new TeachingCredentialBasis("1236");

	/// <summary>Met state testing requirement ("1239")</summary>
	public static readonly TeachingCredentialBasis C0395_1239_STATE_REQUIREMENT = new TeachingCredentialBasis("1239");

	/// <summary>Special/alternative program completion ("1240")</summary>
	public static readonly TeachingCredentialBasis ALTERNATIVE = new TeachingCredentialBasis("1240");

	/// <summary>4-year bachelor's degree ("1235")</summary>
	public static readonly TeachingCredentialBasis FOUR_YEAR_BACHELOR = new TeachingCredentialBasis("1235");

	/// <summary>Relevant experience ("1241")</summary>
	public static readonly TeachingCredentialBasis EXPERIENCE = new TeachingCredentialBasis("1241");

	///<summary>Wrap an arbitrary string value in a TeachingCredentialBasis object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static TeachingCredentialBasis Wrap( String wrappedValue ) {
		return new TeachingCredentialBasis( wrappedValue );
	}

	private TeachingCredentialBasis( string enumDefValue ) : base( enumDefValue ) {}
	}
}
