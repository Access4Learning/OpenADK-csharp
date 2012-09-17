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
	/// Defines the set of values that can be specified whenever an IndigenousStatusType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an IndigenousStatusType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class IndigenousStatusType : SifEnum
	{
	/// <summary>Torres Strait Islander but Not Aboriginal Origin ("2")</summary>
	public static readonly IndigenousStatusType C2_TORRES_STRAIT_ISLANDER_BUT_NOT_ABORIGINAL = new IndigenousStatusType("2");

	/// <summary>Aboriginal but not Torres Strait Islander Origin ("1")</summary>
	public static readonly IndigenousStatusType C1_ABORIGINAL_BUT_NOT_TORRES_STRAIT_ISLANDER = new IndigenousStatusType("1");

	/// <summary>Not Stated/Unknown ("9")</summary>
	public static readonly IndigenousStatusType C9_NOT_STATED_UNKNOWN = new IndigenousStatusType("9");

	/// <summary>Both Torres Strait and Aboriginal Origin ("3")</summary>
	public static readonly IndigenousStatusType C3_BOTH_TORRES_STRAITABORIGINAL = new IndigenousStatusType("3");

	/// <summary>Neither  Aboriginal or Torres Strait Origin ("4")</summary>
	public static readonly IndigenousStatusType C4_NEITHER_ABORIGINAL_OR_TORRES_STRAIT = new IndigenousStatusType("4");

	///<summary>Wrap an arbitrary string value in an IndigenousStatusType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static IndigenousStatusType Wrap( String wrappedValue ) {
		return new IndigenousStatusType( wrappedValue );
	}

	private IndigenousStatusType( string enumDefValue ) : base( enumDefValue ) {}
	}
}