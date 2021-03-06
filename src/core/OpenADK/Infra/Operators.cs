// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.Infra
{
	///<summary>
	/// Defines the set of values that can be specified whenever an Operators
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an Operators object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	[Serializable]
	public class Operators : SifEnum
	{
	/// <summary>Less Than ("LT")</summary>
	public static readonly Operators LT = new Operators("LT");

	/// <summary>Equals ("EQ")</summary>
	public static readonly Operators EQ = new Operators("EQ");

	/// <summary>Greater Than ("GT")</summary>
	public static readonly Operators GT = new Operators("GT");

	/// <summary>Less Than Or Equals ("LE")</summary>
	public static readonly Operators LE = new Operators("LE");

	/// <summary>Greater Than Or Equals ("GE")</summary>
	public static readonly Operators GE = new Operators("GE");

	/// <summary>Not Equals ("NE")</summary>
	public static readonly Operators NE = new Operators("NE");

	///<summary>Wrap an arbitrary string value in an Operators object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static Operators Wrap( String wrappedValue ) {
		return new Operators( wrappedValue );
	}

	private Operators( string enumDefValue ) : base( enumDefValue ) {}
	}
}
