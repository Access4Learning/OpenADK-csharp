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

namespace OpenADK.Library.us.Hrfin{

/// <summary>This object contains basic vendor information.  SIF_Events are reported.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class VendorInfo : SifDataObject
{
	/// <summary>
	/// Creates an instance of a VendorInfo
	/// </summary>
	public VendorInfo() : base( Adk.SifVersion, HrfinDTD.VENDORINFO ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">Vendor ID.</param>
	///<param name="name">Name of the vendor.</param>
	///
	public VendorInfo( string refId, string name ) : base( Adk.SifVersion, HrfinDTD.VENDORINFO )
	{
		this.RefId = refId;
		this.Name = name;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected VendorInfo( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { HrfinDTD.VENDORINFO_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Vendor ID."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.VENDORINFO_REFID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.VENDORINFO_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Name&gt;</c> element.
	/// </summary>
	/// <value> The <c>Name</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Name of the vendor."</para>
	/// <para>This element is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering elements of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;Name&gt;</description></item>
	/// </list>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Name
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.VENDORINFO_NAME ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.VENDORINFO_NAME, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;ContactInfo&gt;</c> element.</summary>
	/// <param name="Name">The name of the contact person.</param>
	///<remarks>
	/// <para>This form of <c>setContactInfo</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ContactInfo</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void SetContactInfo( Name Name ) {
		RemoveChild( HrfinDTD.VENDORINFO_CONTACTINFO);
		AddChild( HrfinDTD.VENDORINFO_CONTACTINFO, new ContactInfo( Name ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ContactInfo&gt;</c> element.
	/// </summary>
	/// <value> A ContactInfo </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Contact information."</para>
	/// <para>To remove the <c>ContactInfo</c>, set <c>ContactInfo</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public ContactInfo ContactInfo
	{
		get
		{
			return (ContactInfo)GetChild( HrfinDTD.VENDORINFO_CONTACTINFO);
		}
		set
		{
			RemoveChild( HrfinDTD.VENDORINFO_CONTACTINFO);
			if( value != null)
			{
				AddChild( HrfinDTD.VENDORINFO_CONTACTINFO, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;Address&gt;</c> element.</summary>
	/// <param name="Type">Code that defines the location of the address.  Note:  A subset of specific valid values for each instance in a data object may be listed in that object.</param>
	/// <param name="Street">The street part of the address</param>
	/// <param name="City">The city part of the address.</param>
	/// <param name="StateProvince">The state or province code.</param>
	/// <param name="Country">The country code.</param>
	/// <param name="PostalCode">The ZIP/postal code.</param>
	///<remarks>
	/// <para>This form of <c>setAddress</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>Address</c></para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetAddress( AddressType Type, Street Street, string City, StatePrCode StateProvince, CountryCode Country, string PostalCode ) {
		RemoveChild( HrfinDTD.VENDORINFO_ADDRESS);
		AddChild( HrfinDTD.VENDORINFO_ADDRESS, new Address( Type, Street, City, StateProvince, Country, PostalCode ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Address&gt;</c> element.
	/// </summary>
	/// <value> An Address </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "This element has the staff member's address information (in SIF 2.x use the ContactInfo element)"</para>
	/// <para>To remove the <c>Address</c>, set <c>Address</c> to <c>null</c></para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public Address Address
	{
		get
		{
			return (Address)GetChild( HrfinDTD.VENDORINFO_ADDRESS);
		}
		set
		{
			RemoveChild( HrfinDTD.VENDORINFO_ADDRESS);
			if( value != null)
			{
				AddChild( HrfinDTD.VENDORINFO_ADDRESS, value );
			}
		}
	}

	/// <summary>Adds a new <c>&lt;PhoneNumber&gt;</c> child element.</summary>
	/// <param name="val">A PhoneNumber object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This is the vendor's phone number (In SIF 2.x use the ContactInfo element)"</para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddPhoneNumber( PhoneNumber val ) { 
		AddChild( HrfinDTD.VENDORINFO_PHONENUMBER, val );
	}

	///<summary>Adds the value of the <c>&lt;PhoneNumber&gt;</c> element.</summary>
	/// <param name="Type">Code that specifies what type of phone number this is.  Note: A subset of valid values may be specified in data objects.</param>
	/// <param name="Number">Phone number.  Acceptable formats:</param>
	///<remarks>
	/// <para>This form of <c>setPhoneNumber</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddPhoneNumber</c></para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddPhoneNumber( PhoneNumberType Type, string Number ) {
		AddChild( HrfinDTD.VENDORINFO_PHONENUMBER, new PhoneNumber( Type, Number ) );
	}

	/// <summary>
	/// Removes a <see cref="PhoneNumber"/> object instance. More than one instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <param name="Type">Identifies the PhoneNumber object to remove by its Type value</param>
	/// <remarks>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void RemovePhoneNumber( PhoneNumberType Type ) { 
		RemoveChild( HrfinDTD.VENDORINFO_PHONENUMBER, new String[] { Type.ToString() } );
	}

	/// <summary>
	/// Gets a <see cref="PhoneNumber"/> object instance. More than one instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <param name="Type">Identifies the PhoneNumber object to return by its "Type" attribute value</param>
	/// <returns>A PhoneNumber object</returns>
	/// <remarks>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public PhoneNumber GetPhoneNumber( PhoneNumberType Type ) { 
		return (PhoneNumber)GetChild( HrfinDTD.VENDORINFO_PHONENUMBER, new string[] { Type.ToString() } );
	}

	/// <summary>
	/// Gets all PhoneNumber object instances. More than once instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <returns>An array of PhoneNumber objects</returns>
	/// <remarks>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public PhoneNumber[] GetPhoneNumbers()
	{
		return GetChildren<PhoneNumber>().ToArray();
	}

	/// <summary>
	/// Sets all PhoneNumber object instances. All existing 
	/// <c>PhoneNumber</c> instances 
	/// are removed and replaced with this list. Calling this method with the 
	/// parameter value set to null removes all <c>PhoneNumbers</c>.
	/// </summary>
	/// <remarks>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetPhoneNumbers( PhoneNumber[] items)
	{
		SetChildren( HrfinDTD.VENDORINFO_PHONENUMBER, items );
	}

	/// <summary>Adds a new <c>&lt;Email&gt;</c> child element.</summary>
	/// <param name="val">A Email object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This is the vendor's email address number (In SIF 2.x use the ContactInfo element)"</para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddEmail( Email val ) { 
		AddChild( HrfinDTD.VENDORINFO_EMAIL, val );
	}

	///<summary>Adds the value of the <c>&lt;Email&gt;</c> element.</summary>
	/// <param name="Type">This attribute specifies the type of e-mail address.</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;Email&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setEmail</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddEmail</c></para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddEmail( EmailType Type, string Value ) {
		AddChild( HrfinDTD.VENDORINFO_EMAIL, new Email( Type, Value ) );
	}

	/// <summary>
	/// Removes an <see cref="Email"/> object instance. More than one instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <param name="Type">Identifies the Email object to remove by its Type value</param>
	/// <remarks>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void RemoveEmail( EmailType Type ) { 
		RemoveChild( HrfinDTD.VENDORINFO_EMAIL, new String[] { Type.ToString() } );
	}

	/// <summary>
	/// Gets an <see cref="Email"/> object instance. More than one instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <param name="Type">Identifies the Email object to return by its "Type" attribute value</param>
	/// <returns>An Email object</returns>
	/// <remarks>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public Email GetEmail( EmailType Type ) { 
		return (Email)GetChild( HrfinDTD.VENDORINFO_EMAIL, new string[] { Type.ToString() } );
	}

	/// <summary>
	/// Gets all Email object instances. More than once instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <returns>An array of Email objects</returns>
	/// <remarks>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public Email[] GetEmails()
	{
		return GetChildren<Email>().ToArray();
	}

	/// <summary>
	/// Sets all Email object instances. All existing 
	/// <c>Email</c> instances 
	/// are removed and replaced with this list. Calling this method with the 
	/// parameter value set to null removes all <c>Emails</c>.
	/// </summary>
	/// <remarks>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetEmails( Email[] items)
	{
		SetChildren( HrfinDTD.VENDORINFO_EMAIL, items );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ContactName&gt;</c> element.
	/// </summary>
	/// <value> The <c>ContactName</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Contact of salesperson name (In SIF 2.x use the ContactInfo element)"</para>
	/// <para>Version: 1.5r1</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string ContactName
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.VENDORINFO_CONTACTNAME ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.VENDORINFO_CONTACTNAME, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;CustomerId&gt;</c> element.
	/// </summary>
	/// <value> The <c>CustomerId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Account number or other ID."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string CustomerId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.VENDORINFO_CUSTOMERID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.VENDORINFO_CUSTOMERID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EmployeePersonalRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>EmployeePersonalRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "
	///         When the vendor and employee are the same, it may be appropriate to link the EmployeePersonal records to
	///         the VendorInfo when tracking expense and tuition reimbursement.  This is the GUID of the associated EmployeePersonal object.
	///       "</para>
	/// <para>This element is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering elements of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;EmployeePersonalRefId&gt;</description></item>
	/// </list>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string EmployeePersonalRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.VENDORINFO_EMPLOYEEPERSONALREFID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.VENDORINFO_EMPLOYEEPERSONALREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Send1099&gt;</c> element.
	/// </summary>
	/// <value> The <c>Send1099</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Send 1099 to this vendor."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public bool? Send1099
	{
		get
		{
			return (bool?) GetSifSimpleFieldValue( HrfinDTD.VENDORINFO_SEND1099 ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.VENDORINFO_SEND1099, new SifBoolean( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;FederalTaxId&gt;</c> element.</summary>
	/// <param name="Code">The type tax ID that this is. TIN based on IRS Publication 1915 [IRSTIN].</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;FederalTaxId&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setFederalTaxId</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>FederalTaxId</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetFederalTaxId( FederalTaxIdCode Code, string Value ) {
		RemoveChild( HrfinDTD.VENDORINFO_FEDERALTAXID);
		AddChild( HrfinDTD.VENDORINFO_FEDERALTAXID, new FederalTaxId( Code, Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;FederalTaxId&gt;</c> element.
	/// </summary>
	/// <value> A FederalTaxId </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Taxpayer identification number/Federal tax ID for this vendor."</para>
	/// <para>To remove the <c>FederalTaxId</c>, set <c>FederalTaxId</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public FederalTaxId FederalTaxId
	{
		get
		{
			return (FederalTaxId)GetChild( HrfinDTD.VENDORINFO_FEDERALTAXID);
		}
		set
		{
			RemoveChild( HrfinDTD.VENDORINFO_FEDERALTAXID);
			if( value != null)
			{
				AddChild( HrfinDTD.VENDORINFO_FEDERALTAXID, value );
			}
		}
	}

}}