// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using System.Text;
using System.Security.Permissions;
using System.Runtime.Serialization;
using OpenADK.Library;
using OpenADK.Library.Global;

namespace OpenADK.Library.us.Common{

/// <summary>A list of Address elements</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.1</para>
/// </remarks>
[Serializable]
public class AddressList : SifActionList<Address>
{
	/// <summary>
	/// Creates an instance of an AddressList
	/// </summary>
	public AddressList() : base ( CommonDTD.ADDRESSLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="address">This element contains information related to employee's address information.For a description of this element, see 5.1.1 Address.</param>
	///
	public AddressList( Address address ) : base( CommonDTD.ADDRESSLIST )
	{
		this.SafeAddChild( CommonDTD.ADDRESSLIST_ADDRESS, address );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected AddressList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;Address&gt;</c> element.</summary>
	/// <param name="Type">Code that defines the location of the address.  Note:  A subset of specific valid values for each instance in a data object may be listed in that object.</param>
	/// <param name="Street">The street part of the address</param>
	/// <param name="City">The city part of the address.</param>
	/// <param name="StateProvince">The state or province code.</param>
	/// <param name="Country">The country code.</param>
	/// <param name="PostalCode">The ZIP/postal code.</param>
	///<remarks>
	/// <para>This form of <c>setAddress</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddAddress</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public void AddAddress( AddressType Type, Street Street, string City, StatePrCode StateProvince, CountryCode Country, string PostalCode ) {
		AddChild( CommonDTD.ADDRESSLIST_ADDRESS, new Address( Type, Street, City, StateProvince, Country, PostalCode ) );
	}

}}
