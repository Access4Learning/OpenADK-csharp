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
	/// Defines the set of values that can be specified whenever an AUCodeSetsEmploymentTypeType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an AUCodeSetsEmploymentTypeType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	[Serializable]
	public class AUCodeSetsEmploymentTypeType : SifEnum
	{
	/// <summary>Out of employed work for 12 months or more (If less use previous occupational group.) ("8")</summary>
	public static readonly AUCodeSetsEmploymentTypeType C8_OUT_OF_EMPLOYMENT = new AUCodeSetsEmploymentTypeType("8");

	/// <summary>Tradsesmen/women, clerks and skilled office, sales and service staff ("3")</summary>
	public static readonly AUCodeSetsEmploymentTypeType C3_TRADIES = new AUCodeSetsEmploymentTypeType("3");

	/// <summary>Senior management in large business organisation, government administration and defence and qualified professionals. ("1")</summary>
	public static readonly AUCodeSetsEmploymentTypeType C1_SEN_MGM = new AUCodeSetsEmploymentTypeType("1");

	/// <summary>Unknown ("9")</summary>
	public static readonly AUCodeSetsEmploymentTypeType C9_UNKNOWN = new AUCodeSetsEmploymentTypeType("9");

	/// <summary>Machine Operators, hospitality staff, assistants, labourers and related workers ("4")</summary>
	public static readonly AUCodeSetsEmploymentTypeType C4_LABOURERS = new AUCodeSetsEmploymentTypeType("4");

	/// <summary>Other business manages, arts/media/sportspersons and associate professionals ("2")</summary>
	public static readonly AUCodeSetsEmploymentTypeType C2_OTHER_MGR = new AUCodeSetsEmploymentTypeType("2");

	///<summary>Wrap an arbitrary string value in an AUCodeSetsEmploymentTypeType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static AUCodeSetsEmploymentTypeType Wrap( String wrappedValue ) {
		return new AUCodeSetsEmploymentTypeType( wrappedValue );
	}

	private AUCodeSetsEmploymentTypeType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
