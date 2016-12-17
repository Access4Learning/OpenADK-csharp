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

/// <summary>A Role</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class Role : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a Role
	/// </summary>
	public Role() : base ( StudentDTD.ROLE ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="value">Gets or sets the content value of the &amp;lt;Role&amp;gt; element</param>
	///
	public Role( RoleType value ) : base( StudentDTD.ROLE )
	{
		this.SetValue( value );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected Role( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { StudentDTD.ROLE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Role&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;Role&amp;gt; element"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Value
	{
		get
		{ 
			return GetFieldValue( StudentDTD.ROLE );
		}
		set
		{
			SetField( StudentDTD.ROLE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;Value&gt;</c> element.
	/// </summary>
	/// <param name="val">A RoleType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;Role&amp;gt; element"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetValue( RoleType val )
	{
		SetField( StudentDTD.ROLE, val );
	}

}}