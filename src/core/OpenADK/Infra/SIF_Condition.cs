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

namespace OpenADK.Library.Infra{

/// <summary>A SIF_Condition</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.1</para>
/// </remarks>
[Serializable]
public class SIF_Condition : SifElement
{
	/// <summary>
	/// Creates an instance of a SIF_Condition
	/// </summary>
	public SIF_Condition() : base ( InfraDTD.SIF_CONDITION ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="sifElement">This is the element/attribute being queried.  See below for syntax.</param>
	///<param name="sifOperator">The comparison operator for the condition.</param>
	///<param name="sifValue">SIF_Value is the data that is used to compare with the value of the element or attribute.</param>
	///
	public SIF_Condition( string sifElement, Operators sifOperator, string sifValue ) : base( InfraDTD.SIF_CONDITION )
	{
		this.SIF_Element = sifElement;
		this.SetSIF_Operator( sifOperator );
		this.SIF_Value = sifValue;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SIF_Condition( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { InfraDTD.SIF_CONDITION_SIF_ELEMENT, InfraDTD.SIF_CONDITION_SIF_OPERATOR, InfraDTD.SIF_CONDITION_SIF_VALUE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Element&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_Element</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This is the element/attribute being queried.  See below for syntax."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string SIF_Element
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InfraDTD.SIF_CONDITION_SIF_ELEMENT ) ;
		}
		set
		{
			SetFieldValue( InfraDTD.SIF_CONDITION_SIF_ELEMENT, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Operator&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_Operator</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The comparison operator for the condition."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string SIF_Operator
	{
		get
		{ 
			return GetFieldValue( InfraDTD.SIF_CONDITION_SIF_OPERATOR );
		}
		set
		{
			SetField( InfraDTD.SIF_CONDITION_SIF_OPERATOR, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;SIF_Operator&gt;</c> element.
	/// </summary>
	/// <param name="val">A Operators object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The comparison operator for the condition."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public void SetSIF_Operator( Operators val )
	{
		SetField( InfraDTD.SIF_CONDITION_SIF_OPERATOR, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Value&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_Value</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "SIF_Value is the data that is used to compare with the value of the element or attribute."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string SIF_Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InfraDTD.SIF_CONDITION_SIF_VALUE ) ;
		}
		set
		{
			SetFieldValue( InfraDTD.SIF_CONDITION_SIF_VALUE, new SifString( value ), value );
		}
	}

}}
