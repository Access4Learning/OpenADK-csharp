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
	/// Defines the set of values that can be specified whenever a YesNoMixed
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a YesNoMixed object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	[Serializable]
	public class YesNoMixed : SifEnum
	{
	/// <summary>Yes ("Yes")</summary>
	public static readonly YesNoMixed YES = new YesNoMixed("Yes");

	/// <summary>No ("No")</summary>
	public static readonly YesNoMixed NO = new YesNoMixed("No");

	///<summary>Wrap an arbitrary string value in a YesNoMixed object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static YesNoMixed Wrap( String wrappedValue ) {
		return new YesNoMixed( wrappedValue );
	}

	private YesNoMixed( string enumDefValue ) : base( enumDefValue ) {}
	}
}
