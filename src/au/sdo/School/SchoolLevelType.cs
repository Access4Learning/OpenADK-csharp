// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.au.School
{
	///<summary>
	/// Defines the set of values that can be specified whenever a SchoolLevelType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a SchoolLevelType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class SchoolLevelType : SifEnum
	{
	/// <summary>Junior Primary ("JunPri")</summary>
	public static readonly SchoolLevelType JunPri_JUNIOR_PRIMARY = new SchoolLevelType("JunPri");

	/// <summary>Maternal Child Health Centre ("MCH")</summary>
	public static readonly SchoolLevelType MCH_MATERNAL_CHILD_HEALTH = new SchoolLevelType("MCH");

	/// <summary>Senior Secondary School ("Senior")</summary>
	public static readonly SchoolLevelType Senior_SENIOR_SECONDARY = new SchoolLevelType("Senior");

	/// <summary>Early Childhood ("EarlyCh")</summary>
	public static readonly SchoolLevelType EarlyCh_EARLY_CHILDHOOD = new SchoolLevelType("EarlyCh");

	/// <summary>Community College ("Commty")</summary>
	public static readonly SchoolLevelType Commty_COMMUNITY_COLLEGE = new SchoolLevelType("Commty");

	/// <summary>Primary ("Prim")</summary>
	public static readonly SchoolLevelType Prim_PRIMARY = new SchoolLevelType("Prim");

	/// <summary>Secondary ("Sec")</summary>
	public static readonly SchoolLevelType Sec_SECONDARY = new SchoolLevelType("Sec");

	/// <summary>Preschool/Kindergarten ("Kind")</summary>
	public static readonly SchoolLevelType Kind_PRESCHOOL_KINDERGARTEN = new SchoolLevelType("Kind");

	/// <summary>Language ("Lang")</summary>
	public static readonly SchoolLevelType Lang_LANGUAGE = new SchoolLevelType("Lang");

	/// <summary>Camp ("Camp")</summary>
	public static readonly SchoolLevelType CAMP = new SchoolLevelType("Camp");

	/// <summary>Specific Purpose ("Specif")</summary>
	public static readonly SchoolLevelType Specif_SPECIFIC_PURPOSE = new SchoolLevelType("Specif");

	/// <summary>Kindergarten only ("Kgarten")</summary>
	public static readonly SchoolLevelType Kgarten_KINDERGARTEN = new SchoolLevelType("Kgarten");

	/// <summary>Primary/Seconday Combined ("Pri/Sec")</summary>
	public static readonly SchoolLevelType Pri_Sec_PRIMARY_SECONDAY = new SchoolLevelType("Pri/Sec");

	/// <summary>PreSchool only ("PreSch")</summary>
	public static readonly SchoolLevelType PreSch_PRESCHOOL_ONLY = new SchoolLevelType("PreSch");

	/// <summary>Special ("Special")</summary>
	public static readonly SchoolLevelType SPECIAL = new SchoolLevelType("Special");

	/// <summary>Middle School ("Middle")</summary>
	public static readonly SchoolLevelType Middle_MIDDLE_SCHOOL = new SchoolLevelType("Middle");

	/// <summary>Other ("Other")</summary>
	public static readonly SchoolLevelType OTHER = new SchoolLevelType("Other");

	/// <summary>Unknown ("Unknown")</summary>
	public static readonly SchoolLevelType UNKNOWN = new SchoolLevelType("Unknown");

	/// <summary>SupportCentre ("Supp")</summary>
	public static readonly SchoolLevelType Supp_SUPPORTCENTRE = new SchoolLevelType("Supp");

	///<summary>Wrap an arbitrary string value in a SchoolLevelType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static SchoolLevelType Wrap( String wrappedValue ) {
		return new SchoolLevelType( wrappedValue );
	}

	private SchoolLevelType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
