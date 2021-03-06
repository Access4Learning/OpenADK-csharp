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
	/// Defines the set of values that can be specified whenever an AddressRoleType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an AddressRoleType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class AddressRoleType : SifEnum
	{
	/// <summary>Other Address ("9999")</summary>
	public static readonly AddressRoleType C9999_OT_ADDRESS = new AddressRoleType("9999");

	/// <summary>Overseas Address ("013A")</summary>
	public static readonly AddressRoleType C013A_OVERSEAS_ADDRESS = new AddressRoleType("013A");

	/// <summary>Term Address ("012A")</summary>
	public static readonly AddressRoleType C012A_TERM_ADDRESS = new AddressRoleType("012A");

	/// <summary>Home Address ("012B")</summary>
	public static readonly AddressRoleType C012B_HOME_ADDRESS = new AddressRoleType("012B");

	/// <summary>Employment address ("1075")</summary>
	public static readonly AddressRoleType C1075_EMPLOYMENT_ADDRESS = new AddressRoleType("1075");

	/// <summary>Other organisation address ("2382")</summary>
	public static readonly AddressRoleType C2382_OT_ORGANISATION = new AddressRoleType("2382");

	/// <summary>Other home address ("1073")</summary>
	public static readonly AddressRoleType C1073_OT_HOME_ADDRESS = new AddressRoleType("1073");

	/// <summary>Home Stay Address ("012C")</summary>
	public static readonly AddressRoleType C012C_HOME_STAY_ADDRESS = new AddressRoleType("012C");

	/// <summary>Employer's address ("1074")</summary>
	public static readonly AddressRoleType C1074_EMPLOYERS_ADDRESS = new AddressRoleType("1074");

	///<summary>Wrap an arbitrary string value in an AddressRoleType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static AddressRoleType Wrap( String wrappedValue ) {
		return new AddressRoleType( wrappedValue );
	}

	private AddressRoleType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
