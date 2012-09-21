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

namespace OpenADK.Library.us.Dw{

/// <summary>Rule for calculating the aggregate statistic</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class CalculationRule : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a CalculationRule
	/// </summary>
	public CalculationRule() : base ( DwDTD.CALCULATIONRULE ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="type">Values Description, Formula, URL</param>
	///<param name="value">Gets or sets the content value of the &amp;lt;CalculationRule&amp;gt; element</param>
	///
	public CalculationRule( CalculationRuleType type, string value ) : base( DwDTD.CALCULATIONRULE )
	{
		this.SetType( type );
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected CalculationRule( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { DwDTD.CALCULATIONRULE_TYPE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;CalculationRule&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;CalculationRule&amp;gt; element"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( DwDTD.CALCULATIONRULE ) ;
		}
		set
		{
			SetFieldValue( DwDTD.CALCULATIONRULE, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>Type</c> attribute.
	/// </summary>
	/// <value> The <c>Type</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Values Description, Formula, URL"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Type
	{
		get
		{ 
			return GetFieldValue( DwDTD.CALCULATIONRULE_TYPE );
		}
		set
		{
			SetField( DwDTD.CALCULATIONRULE_TYPE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>Type</c> attribute.
	/// </summary>
	/// <param name="val">A CalculationRuleType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Values Description, Formula, URL"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetType( CalculationRuleType val )
	{
		SetField( DwDTD.CALCULATIONRULE_TYPE, val );
	}

}}
