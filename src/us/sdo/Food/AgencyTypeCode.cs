// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Food
{
	///<summary>
	/// Defines the set of values that can be specified whenever an AgencyTypeCode
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an AgencyTypeCode object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class AgencyTypeCode : SifEnum
	{
	/// <summary>Other ("Other")</summary>
	public static readonly AgencyTypeCode OTHER = new AgencyTypeCode("Other");

	/// <summary>Federal ("Federal")</summary>
	public static readonly AgencyTypeCode FEDERAL = new AgencyTypeCode("Federal");

	/// <summary>Local ("Local")</summary>
	public static readonly AgencyTypeCode LOCAL = new AgencyTypeCode("Local");

	/// <summary>State ("State")</summary>
	public static readonly AgencyTypeCode STATE = new AgencyTypeCode("State");

	///<summary>Wrap an arbitrary string value in an AgencyTypeCode object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static AgencyTypeCode Wrap( String wrappedValue ) {
		return new AgencyTypeCode( wrappedValue );
	}

	private AgencyTypeCode( string enumDefValue ) : base( enumDefValue ) {}
	}
}
