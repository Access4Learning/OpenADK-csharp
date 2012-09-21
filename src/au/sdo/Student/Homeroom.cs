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
using OpenADK.Library.au.Common;

namespace OpenADK.Library.au.Student{

/// <summary>An Homeroom</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class Homeroom : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of an Homeroom
	/// </summary>
	public Homeroom() : base ( StudentDTD.HOMEROOM ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="sifRefObject">The name of the object referenced.</param>
	///<param name="value">Gets or sets the content value of the &amp;lt;Homeroom&amp;gt; element</param>
	///
	public Homeroom( SIF_RefObject sifRefObject, string value ) : base( StudentDTD.HOMEROOM )
	{
		this.SetSIF_RefObject( sifRefObject );
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected Homeroom( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { StudentDTD.HOMEROOM_SIF_REFOBJECT }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Homeroom&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;Homeroom&amp;gt; element"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( StudentDTD.HOMEROOM ) ;
		}
		set
		{
			SetFieldValue( StudentDTD.HOMEROOM, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>SIF_RefObject</c> attribute.
	/// </summary>
	/// <value> The <c>SIF_RefObject</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The name of the object referenced."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string SIF_RefObject
	{
		get
		{ 
			return GetFieldValue( StudentDTD.HOMEROOM_SIF_REFOBJECT );
		}
		set
		{
			SetField( StudentDTD.HOMEROOM_SIF_REFOBJECT, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>SIF_RefObject</c> attribute.
	/// </summary>
	/// <param name="val">A SIF_RefObject object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The name of the object referenced."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetSIF_RefObject( SIF_RefObject val )
	{
		SetField( StudentDTD.HOMEROOM_SIF_REFOBJECT, val );
	}

}}
