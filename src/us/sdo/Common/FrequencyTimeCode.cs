// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Common
{
	///<summary>
	/// Defines the set of values that can be specified whenever a FrequencyTimeCode
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a FrequencyTimeCode object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class FrequencyTimeCode : SifEnum
	{
	/// <summary>Semi-annually ("S006")</summary>
	public static readonly FrequencyTimeCode SEMI_ANNUALLY = new FrequencyTimeCode("S006");

	/// <summary>Semi-monthly ("S003")</summary>
	public static readonly FrequencyTimeCode SEMI_MONTHLY = new FrequencyTimeCode("S003");

	/// <summary>quarterly ("S005")</summary>
	public static readonly FrequencyTimeCode QUARTERLY = new FrequencyTimeCode("S005");

	/// <summary>Other ("S999")</summary>
	public static readonly FrequencyTimeCode OTHER = new FrequencyTimeCode("S999");

	/// <summary>Monthly ("S004")</summary>
	public static readonly FrequencyTimeCode MONTHLY = new FrequencyTimeCode("S004");

	/// <summary>Annually ("S007")</summary>
	public static readonly FrequencyTimeCode ANNUALLY = new FrequencyTimeCode("S007");

	/// <summary>Daily ("S008")</summary>
	public static readonly FrequencyTimeCode DAILY = new FrequencyTimeCode("S008");

	/// <summary>Weekly ("S001")</summary>
	public static readonly FrequencyTimeCode WEEKLY = new FrequencyTimeCode("S001");

	/// <summary>Bi-weekly ("S002")</summary>
	public static readonly FrequencyTimeCode BI_WEEKLY = new FrequencyTimeCode("S002");

	///<summary>Wrap an arbitrary string value in a FrequencyTimeCode object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static FrequencyTimeCode Wrap( String wrappedValue ) {
		return new FrequencyTimeCode( wrappedValue );
	}

	private FrequencyTimeCode( string enumDefValue ) : base( enumDefValue ) {}
	}
}
