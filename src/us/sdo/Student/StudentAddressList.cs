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
using OpenADK.Library.us.Common;

namespace OpenADK.Library.us.Student{

/// <summary>A student address</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.1</para>
/// </remarks>
[Serializable]
public class StudentAddressList : SifActionList<Address>
{
	/// <summary>
	/// Creates an instance of a StudentAddressList
	/// </summary>
	public StudentAddressList() : base ( StudentDTD.STUDENTADDRESSLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="pickupOrDropoff">Specifies if this is a pickup or dropoff address</param>
	///<param name="dayOfWeek">The days of the week for which the pickup or dropoff address is valid</param>
	///<param name="address">The street address</param>
	///
	public StudentAddressList( PickupOrDropoff pickupOrDropoff, string dayOfWeek, Address address ) : base( StudentDTD.STUDENTADDRESSLIST )
	{
		this.SetPickupOrDropoff( pickupOrDropoff );
		this.DayOfWeek = dayOfWeek;
		this.SafeAddChild( StudentDTD.STUDENTADDRESSLIST_ADDRESS, address );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected StudentAddressList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { StudentDTD.STUDENTADDRESSLIST_ADDRESS }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>PickupOrDropoff</c> attribute.
	/// </summary>
	/// <value> The <c>PickupOrDropoff</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Specifies if this is a pickup or dropoff address"</para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string PickupOrDropoff
	{
		get
		{ 
			return GetFieldValue( StudentDTD.STUDENTADDRESSLIST_PICKUPORDROPOFF );
		}
		set
		{
			SetField( StudentDTD.STUDENTADDRESSLIST_PICKUPORDROPOFF, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>PickupOrDropoff</c> attribute.
	/// </summary>
	/// <param name="val">A PickupOrDropoff object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Specifies if this is a pickup or dropoff address"</para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public void SetPickupOrDropoff( PickupOrDropoff val )
	{
		SetField( StudentDTD.STUDENTADDRESSLIST_PICKUPORDROPOFF, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>DayOfWeek</c> attribute.
	/// </summary>
	/// <value> The <c>DayOfWeek</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The days of the week for which the pickup or dropoff address is valid"</para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string DayOfWeek
	{
		get
		{
			return (string) GetSifSimpleFieldValue( StudentDTD.STUDENTADDRESSLIST_DAYOFWEEK ) ;
		}
		set
		{
			SetFieldValue( StudentDTD.STUDENTADDRESSLIST_DAYOFWEEK, new SifString( value ), value );
		}
	}

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
		AddChild( StudentDTD.STUDENTADDRESSLIST_ADDRESS, new Address( Type, Street, City, StateProvince, Country, PostalCode ) );
	}

}}
