// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Programs
{
	///<summary>
	/// Defines the set of values that can be specified whenever a StudentParticipationType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a StudentParticipationType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class StudentParticipationType : SifEnum
	{
	/// <summary>IDEA-C ("IDEA-C")</summary>
	public static readonly StudentParticipationType IDEA_C = new StudentParticipationType("IDEA-C");

	/// <summary>IDEA-B ("IDEA-B")</summary>
	public static readonly StudentParticipationType IDEA_B = new StudentParticipationType("IDEA-B");

	/// <summary>Section504 ("Section504")</summary>
	public static readonly StudentParticipationType SECTION504 = new StudentParticipationType("Section504");

	/// <summary>Other ("Other")</summary>
	public static readonly StudentParticipationType OTHER = new StudentParticipationType("Other");

	/// <summary>LIT ("LIT")</summary>
	public static readonly StudentParticipationType LIT = new StudentParticipationType("LIT");

	/// <summary>EvenStart ("EvenStart")</summary>
	public static readonly StudentParticipationType EVENSTART = new StudentParticipationType("EvenStart");

	/// <summary>Correctional ("Correctional")</summary>
	public static readonly StudentParticipationType CORRECTIONAL = new StudentParticipationType("Correctional");

	/// <summary>Vocational ("Vocational")</summary>
	public static readonly StudentParticipationType VOCATIONAL = new StudentParticipationType("Vocational");

	/// <summary>HeadStart ("HeadStart")</summary>
	public static readonly StudentParticipationType HEADSTART = new StudentParticipationType("HeadStart");

	/// <summary>BehaviorDisorder ("BehaviorDisorder")</summary>
	public static readonly StudentParticipationType BEHAVIOR_DISORDER = new StudentParticipationType("BehaviorDisorder");

	/// <summary>GiftedTalented ("GiftedTalented")</summary>
	public static readonly StudentParticipationType GIFTED_TALENTED = new StudentParticipationType("GiftedTalented");

	/// <summary>Migrant ("Migrant")</summary>
	public static readonly StudentParticipationType MIGRANT = new StudentParticipationType("Migrant");

	/// <summary>ESL ("ESL")</summary>
	public static readonly StudentParticipationType ESL = new StudentParticipationType("ESL");

	/// <summary>Title1 ("Title1")</summary>
	public static readonly StudentParticipationType TITLE1 = new StudentParticipationType("Title1");

	///<summary>Wrap an arbitrary string value in a StudentParticipationType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static StudentParticipationType Wrap( String wrappedValue ) {
		return new StudentParticipationType( wrappedValue );
	}

	private StudentParticipationType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
