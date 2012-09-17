// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Library
{
	///<summary>
	/// Defines the set of values that can be specified whenever an HoldStatus
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an HoldStatus object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	[Serializable]
	public class HoldStatus : SifEnum
	{
	/// <summary>(which means that a previously placed hold is ready for the patron to pick up at the library desk) ("Ready")</summary>
	public static readonly HoldStatus READY = new HoldStatus("Ready");

	/// <summary>(hold has been placed but hasn't been fulfilled yet) ("NotReady")</summary>
	public static readonly HoldStatus NOTREADY = new HoldStatus("NotReady");

	///<summary>Wrap an arbitrary string value in an HoldStatus object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static HoldStatus Wrap( String wrappedValue ) {
		return new HoldStatus( wrappedValue );
	}

	private HoldStatus( string enumDefValue ) : base( enumDefValue ) {}
	}
}
