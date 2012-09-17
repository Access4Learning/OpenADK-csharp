// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.uk.Common
{
	///<summary>
	/// Defines the set of values that can be specified whenever a NameType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a NameType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class NameType : SifEnum
	{
	/// <summary>Married name ("M")</summary>
	public static readonly NameType MARRIED = new NameType("M");

	/// <summary>Name of record ("R")</summary>
	public static readonly NameType NAME_OF_RECORD = new NameType("R");

	/// <summary>Former Name ("F")</summary>
	public static readonly NameType FORMER = new NameType("F");

	/// <summary>Alias ("A")</summary>
	public static readonly NameType ALIAS = new NameType("A");

	/// <summary>Professional name ("P")</summary>
	public static readonly NameType PROFESSIONAL = new NameType("P");

	/// <summary>Current legal name ("C")</summary>
	public static readonly NameType CURRENT_LEGAL = new NameType("C");

	/// <summary>Birth name ("B")</summary>
	public static readonly NameType BIRTH = new NameType("B");

	///<summary>Wrap an arbitrary string value in a NameType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static NameType Wrap( String wrappedValue ) {
		return new NameType( wrappedValue );
	}

	private NameType( string enumDefValue ) : base( enumDefValue ) {}
	}
}