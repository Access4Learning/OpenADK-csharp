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
using OpenADK.Library.Infra;
using OpenADK.Library.au.Common;

namespace OpenADK.Library.au.Infrastructure{

/// <summary>An Identity</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class Identity : SifDataObject
{
	/// <summary>
	/// Creates an instance of an Identity
	/// </summary>
	public Identity() : base( Adk.SifVersion, InfrastructureDTD.IDENTITY ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">The SIF RefId that uniquely identifies this object.</param>
	///<param name="sifRefId">A SIF_RefId</param>
	///<param name="authenticationSource">The type of source system that produced this Identityobject.</param>
	///
	public Identity( string refId, SIF_RefId sifRefId, AuthenticationSource authenticationSource ) : base( Adk.SifVersion, InfrastructureDTD.IDENTITY )
	{
		this.RefId = refId;
		this.SIF_RefId = sifRefId;
		this.SetAuthenticationSource( authenticationSource );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected Identity( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { InfrastructureDTD.IDENTITY_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The SIF RefId that uniquely identifies this object."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InfrastructureDTD.IDENTITY_REFID ) ;
		}
		set
		{
			SetFieldValue( InfrastructureDTD.IDENTITY_REFID, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;SIF_RefId&gt;</c> element.</summary>
	/// <param name="SifRefObject">A SIF_RefObject</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;SIF_RefId&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setSIF_RefId</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>SIF_RefId</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetSIF_RefId( string SifRefObject, string Value ) {
		RemoveChild( InfrastructureDTD.IDENTITY_SIF_REFID);
		AddChild( InfrastructureDTD.IDENTITY_SIF_REFID, new SIF_RefId( SifRefObject, Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_RefId&gt;</c> element.
	/// </summary>
	/// <value> A SIF_RefId </value>
	/// <remarks>
	/// <para>To remove the <c>SIF_RefId</c>, set <c>SIF_RefId</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public SIF_RefId SIF_RefId
	{
		get
		{
			return (SIF_RefId)GetChild( InfrastructureDTD.IDENTITY_SIF_REFID);
		}
		set
		{
			RemoveChild( InfrastructureDTD.IDENTITY_SIF_REFID);
			if( value != null)
			{
				AddChild( InfrastructureDTD.IDENTITY_SIF_REFID, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AuthenticationSource&gt;</c> element.
	/// </summary>
	/// <value> The <c>AuthenticationSource</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The type of source system that produced this Identityobject."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string AuthenticationSource
	{
		get
		{ 
			return GetFieldValue( InfrastructureDTD.IDENTITY_AUTHENTICATIONSOURCE );
		}
		set
		{
			SetField( InfrastructureDTD.IDENTITY_AUTHENTICATIONSOURCE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;AuthenticationSource&gt;</c> element.
	/// </summary>
	/// <param name="val">A AuthenticationSource object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The type of source system that produced this Identityobject."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetAuthenticationSource( AuthenticationSource val )
	{
		SetField( InfrastructureDTD.IDENTITY_AUTHENTICATIONSOURCE, val );
	}

	///<summary>Sets the value of the <c>&lt;IdentityAssertions&gt;</c> element.</summary>
	/// <param name="IdentityAssertion">The identification string for this user.</param>
	///<remarks>
	/// <para>This form of <c>setIdentityAssertions</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>IdentityAssertions</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetIdentityAssertions( IdentityAssertion IdentityAssertion ) {
		RemoveChild( InfrastructureDTD.IDENTITY_IDENTITYASSERTIONS);
		AddChild( InfrastructureDTD.IDENTITY_IDENTITYASSERTIONS, new IdentityAssertions( IdentityAssertion ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;IdentityAssertions&gt;</c> element.
	/// </summary>
	/// <value> An IdentityAssertions </value>
	/// <remarks>
	/// <para>To remove the <c>IdentityAssertions</c>, set <c>IdentityAssertions</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public IdentityAssertions IdentityAssertions
	{
		get
		{
			return (IdentityAssertions)GetChild( InfrastructureDTD.IDENTITY_IDENTITYASSERTIONS);
		}
		set
		{
			RemoveChild( InfrastructureDTD.IDENTITY_IDENTITYASSERTIONS);
			if( value != null)
			{
				AddChild( InfrastructureDTD.IDENTITY_IDENTITYASSERTIONS, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;PasswordList&gt;</c> element.</summary>
	/// <param name="Password">A representation of the user's password using the given algorithm.</param>
	///<remarks>
	/// <para>This form of <c>setPasswordList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>PasswordList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetPasswordList( Password Password ) {
		RemoveChild( InfrastructureDTD.IDENTITY_PASSWORDLIST);
		AddChild( InfrastructureDTD.IDENTITY_PASSWORDLIST, new PasswordList( Password ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;PasswordList&gt;</c> element.
	/// </summary>
	/// <value> A PasswordList </value>
	/// <remarks>
	/// <para>To remove the <c>PasswordList</c>, set <c>PasswordList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public PasswordList PasswordList
	{
		get
		{
			return (PasswordList)GetChild( InfrastructureDTD.IDENTITY_PASSWORDLIST);
		}
		set
		{
			RemoveChild( InfrastructureDTD.IDENTITY_PASSWORDLIST);
			if( value != null)
			{
				AddChild( InfrastructureDTD.IDENTITY_PASSWORDLIST, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AuthenticationSourceGlobalUID&gt;</c> element.
	/// </summary>
	/// <value> The <c>AuthenticationSourceGlobalUID</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The globally unique person identifier that links together separate Identity objects which reference the same Person."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string AuthenticationSourceGlobalUID
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InfrastructureDTD.IDENTITY_AUTHENTICATIONSOURCEGLOBALUID ) ;
		}
		set
		{
			SetFieldValue( InfrastructureDTD.IDENTITY_AUTHENTICATIONSOURCEGLOBALUID, new SifString( value ), value );
		}
	}

}}
