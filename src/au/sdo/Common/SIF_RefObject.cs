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
	/// Defines the set of values that can be specified whenever a SIF_RefObject
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a SIF_RefObject object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class SIF_RefObject : SifEnum
	{
	/// <summary>StudentContactPersonal ("StudentContactPersonal")</summary>
	public static readonly SIF_RefObject STUDENTCONTACTPERSONAL = new SIF_RefObject("StudentContactPersonal");

	/// <summary>StaffPersonal ("StaffPersonal")</summary>
	public static readonly SIF_RefObject STAFFPERSONAL = new SIF_RefObject("StaffPersonal");

	/// <summary>StudentPersonal ("StudentPersonal")</summary>
	public static readonly SIF_RefObject STUDENTPERSONAL = new SIF_RefObject("StudentPersonal");

	///<summary>Wrap an arbitrary string value in a SIF_RefObject object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static SIF_RefObject Wrap( String wrappedValue ) {
		return new SIF_RefObject( wrappedValue );
	}

	private SIF_RefObject( string enumDefValue ) : base( enumDefValue ) {}
	}
}
