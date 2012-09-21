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
	/// Defines the set of values that can be specified whenever a CountryCode
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a CountryCode object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class CountryCode : SifEnum
	{
	/// <summary>Australia ("2301")</summary>
	public static readonly CountryCode C2301 = new CountryCode("2301");

	/// <summary>Canada ("8102")</summary>
	public static readonly CountryCode C8102 = new CountryCode("8102");

	/// <summary>Ireland ("2201")</summary>
	public static readonly CountryCode C2201 = new CountryCode("2201");

	/// <summary>Not Stated ("0003")</summary>
	public static readonly CountryCode C0003 = new CountryCode("0003");

	/// <summary>United Kingdom ("0923")</summary>
	public static readonly CountryCode C0923 = new CountryCode("0923");

	/// <summary>Isle of Man ("2103")</summary>
	public static readonly CountryCode C2103 = new CountryCode("2103");

	/// <summary>New Zealand ("1201")</summary>
	public static readonly CountryCode C1201 = new CountryCode("1201");

	/// <summary>United States of America ("8104")</summary>
	public static readonly CountryCode C8104 = new CountryCode("8104");

	///<summary>Wrap an arbitrary string value in a CountryCode object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static CountryCode Wrap( String wrappedValue ) {
		return new CountryCode( wrappedValue );
	}

	private CountryCode( string enumDefValue ) : base( enumDefValue ) {}
	}
}
