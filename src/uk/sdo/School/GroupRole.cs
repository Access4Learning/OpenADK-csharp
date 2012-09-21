// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.uk.School
{
	///<summary>
	/// Defines the set of values that can be specified whenever a GroupRole
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a GroupRole object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class GroupRole : SifEnum
	{
	/// <summary>Volunteer ("VOL")</summary>
	public static readonly GroupRole VOLUNTEER = new GroupRole("VOL");

	/// <summary>Academic ("ACD")</summary>
	public static readonly GroupRole ACADEMIC = new GroupRole("ACD");

	/// <summary>Deputy head ("DHT")</summary>
	public static readonly GroupRole DEPUTY_HEAD = new GroupRole("DHT");

	/// <summary>Advanced Skills Teacher ("AST")</summary>
	public static readonly GroupRole ADVANCED_SKILLS_TEACHER = new GroupRole("AST");

	/// <summary>Other ("OTH")</summary>
	public static readonly GroupRole OTHER = new GroupRole("OTH");

	/// <summary>Educational Psychologist ("EPS")</summary>
	public static readonly GroupRole EDUCATIONAL_PSYCHOLOGIST = new GroupRole("EPS");

	/// <summary>Assistant head ("AHT")</summary>
	public static readonly GroupRole ASSISTANT_HEAD = new GroupRole("AHT");

	/// <summary>Classroom teacher ("TCH")</summary>
	public static readonly GroupRole CLASSROOM_TEACHER = new GroupRole("TCH");

	/// <summary>Administrator ("ADM")</summary>
	public static readonly GroupRole ADMINISTRATOR = new GroupRole("ADM");

	/// <summary>Head teacher ("HDT")</summary>
	public static readonly GroupRole HEAD_TEACHER = new GroupRole("HDT");

	/// <summary>Support staff ("SUP")</summary>
	public static readonly GroupRole SUPPORT_STAFF = new GroupRole("SUP");

	/// <summary>Advisory Teacher ("AVT")</summary>
	public static readonly GroupRole ADVISORY_TEACHER = new GroupRole("AVT");

	/// <summary>Excellent Teacher ("EXL")</summary>
	public static readonly GroupRole EXCELLENT_TEACHER = new GroupRole("EXL");

	/// <summary>Governor ("GOV")</summary>
	public static readonly GroupRole GOVERNOR = new GroupRole("GOV");

	///<summary>Wrap an arbitrary string value in a GroupRole object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static GroupRole Wrap( String wrappedValue ) {
		return new GroupRole( wrappedValue );
	}

	private GroupRole( string enumDefValue ) : base( enumDefValue ) {}
	}
}
