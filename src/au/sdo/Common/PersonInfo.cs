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

namespace OpenADK.Library.au.Common{

/// <summary>A PersonInfo</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class PersonInfo : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a PersonInfo
	/// </summary>
	public PersonInfo() : base ( CommonDTD.PERSONINFO ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="name">The name of the person. Note: Type attribute value of LGL must be used here.</param>
	///
	public PersonInfo( Name name ) : base( CommonDTD.PERSONINFO )
	{
		this.Name = name;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected PersonInfo( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { CommonDTD.PERSONINFO_NAME }; }
	}

	///<summary>Sets the value of the <c>&lt;Name&gt;</c> element.</summary>
	/// <param name="Type">
	/// Code that specifies what type of name this is. If
	/// unsure, use LGL.</param>
	///<remarks>
	/// <para>This form of <c>setName</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>Name</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetName( NameType Type ) {
		RemoveChild( CommonDTD.PERSONINFO_NAME);
		AddChild( CommonDTD.PERSONINFO_NAME, new Name( Type ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Name&gt;</c> element.
	/// </summary>
	/// <value> A Name </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "The name of the person. Note: Type attribute value of LGL must be used here."</para>
	/// <para>To remove the <c>Name</c>, set <c>Name</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public Name Name
	{
		get
		{
			return (Name)GetChild( CommonDTD.PERSONINFO_NAME);
		}
		set
		{
			RemoveChild( CommonDTD.PERSONINFO_NAME);
			if( value != null)
			{
				AddChild( CommonDTD.PERSONINFO_NAME, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;OtherNames&gt;</c> element.</summary>
	/// <param name="Name">A Name</param>
	///<remarks>
	/// <para>This form of <c>setOtherNames</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>OtherNames</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetOtherNames( Name Name ) {
		RemoveChild( CommonDTD.PERSONINFO_OTHERNAMES);
		AddChild( CommonDTD.PERSONINFO_OTHERNAMES, new OtherNames( Name ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;OtherNames&gt;</c> element.
	/// </summary>
	/// <value> An OtherNames </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Previous, alternate or other names or aliases associated with the person."</para>
	/// <para>To remove the <c>OtherNames</c>, set <c>OtherNames</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public OtherNames OtherNames
	{
		get
		{
			return (OtherNames)GetChild( CommonDTD.PERSONINFO_OTHERNAMES);
		}
		set
		{
			RemoveChild( CommonDTD.PERSONINFO_OTHERNAMES);
			if( value != null)
			{
				AddChild( CommonDTD.PERSONINFO_OTHERNAMES, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Demographics&gt;</c> element.
	/// </summary>
	/// <value> A Demographics </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Demographic information about the person."</para>
	/// <para>To remove the <c>Demographics</c>, set <c>Demographics</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public Demographics Demographics
	{
		get
		{
			return (Demographics)GetChild( CommonDTD.PERSONINFO_DEMOGRAPHICS);
		}
		set
		{
			RemoveChild( CommonDTD.PERSONINFO_DEMOGRAPHICS);
			if( value != null)
			{
				AddChild( CommonDTD.PERSONINFO_DEMOGRAPHICS, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;AddressList&gt;</c> element.</summary>
	/// <param name="Address">An Address</param>
	///<remarks>
	/// <para>This form of <c>setAddressList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>AddressList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetAddressList( Address Address ) {
		RemoveChild( CommonDTD.PERSONINFO_ADDRESSLIST);
		AddChild( CommonDTD.PERSONINFO_ADDRESSLIST, new AddressList( Address ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AddressList&gt;</c> element.
	/// </summary>
	/// <value> An AddressList </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "The person's address(es)."</para>
	/// <para>To remove the <c>AddressList</c>, set <c>AddressList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public AddressList AddressList
	{
		get
		{
			return (AddressList)GetChild( CommonDTD.PERSONINFO_ADDRESSLIST);
		}
		set
		{
			RemoveChild( CommonDTD.PERSONINFO_ADDRESSLIST);
			if( value != null)
			{
				AddChild( CommonDTD.PERSONINFO_ADDRESSLIST, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;PhoneNumberList&gt;</c> element.</summary>
	/// <param name="PhoneNumber">A PhoneNumber</param>
	///<remarks>
	/// <para>This form of <c>setPhoneNumberList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>PhoneNumberList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetPhoneNumberList( PhoneNumber PhoneNumber ) {
		RemoveChild( CommonDTD.PERSONINFO_PHONENUMBERLIST);
		AddChild( CommonDTD.PERSONINFO_PHONENUMBERLIST, new PhoneNumberList( PhoneNumber ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;PhoneNumberList&gt;</c> element.
	/// </summary>
	/// <value> A PhoneNumberList </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "The person's phone number(s)."</para>
	/// <para>To remove the <c>PhoneNumberList</c>, set <c>PhoneNumberList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public PhoneNumberList PhoneNumberList
	{
		get
		{
			return (PhoneNumberList)GetChild( CommonDTD.PERSONINFO_PHONENUMBERLIST);
		}
		set
		{
			RemoveChild( CommonDTD.PERSONINFO_PHONENUMBERLIST);
			if( value != null)
			{
				AddChild( CommonDTD.PERSONINFO_PHONENUMBERLIST, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;EmailList&gt;</c> element.</summary>
	/// <param name="Email">An Email</param>
	///<remarks>
	/// <para>This form of <c>setEmailList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>EmailList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetEmailList( Email Email ) {
		RemoveChild( CommonDTD.PERSONINFO_EMAILLIST);
		AddChild( CommonDTD.PERSONINFO_EMAILLIST, new EmailList( Email ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EmailList&gt;</c> element.
	/// </summary>
	/// <value> An EmailList </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "The person's e-mail address(es)."</para>
	/// <para>To remove the <c>EmailList</c>, set <c>EmailList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public EmailList EmailList
	{
		get
		{
			return (EmailList)GetChild( CommonDTD.PERSONINFO_EMAILLIST);
		}
		set
		{
			RemoveChild( CommonDTD.PERSONINFO_EMAILLIST);
			if( value != null)
			{
				AddChild( CommonDTD.PERSONINFO_EMAILLIST, value );
			}
		}
	}

}}
