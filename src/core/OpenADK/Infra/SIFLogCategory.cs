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
	/// Defines the set of values that can be specified whenever a SIFLogCategory
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a SIFLogCategory object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class SIFLogCategory : SifEnum
	{
	/// <summary>Data Issues with Failure Result ("3")</summary>
	public static readonly SIFLogCategory ISSUES_FAILURE = new SIFLogCategory("3");

	/// <summary>Error Conditions ("4")</summary>
	public static readonly SIFLogCategory ERROR = new SIFLogCategory("4");

	/// <summary>Success ("1")</summary>
	public static readonly SIFLogCategory SUCCESS = new SIFLogCategory("1");

	/// <summary>Data Issues with Success Result ("2")</summary>
	public static readonly SIFLogCategory ISSUES_SUCCESS = new SIFLogCategory("2");

	///<summary>Wrap an arbitrary string value in a SIFLogCategory object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static SIFLogCategory Wrap( String wrappedValue ) {
		return new SIFLogCategory( wrappedValue );
	}

	private SIFLogCategory( string enumDefValue ) : base( enumDefValue ) {}
	}
}
