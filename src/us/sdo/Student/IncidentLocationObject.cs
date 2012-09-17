// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Student
{
	///<summary>
	/// Defines the set of values that can be specified whenever an IncidentLocationObject
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an IncidentLocationObject object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class IncidentLocationObject : SifEnum
	{
	/// <summary>SchoolInfo ("SchoolInfo")</summary>
	public static readonly IncidentLocationObject SCHOOLINFO = new IncidentLocationObject("SchoolInfo");

	/// <summary>LocationInfo ("LocationInfo")</summary>
	public static readonly IncidentLocationObject LOCATIONINFO = new IncidentLocationObject("LocationInfo");

	///<summary>Wrap an arbitrary string value in an IncidentLocationObject object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static IncidentLocationObject Wrap( String wrappedValue ) {
		return new IncidentLocationObject( wrappedValue );
	}

	private IncidentLocationObject( string enumDefValue ) : base( enumDefValue ) {}
	}
}
