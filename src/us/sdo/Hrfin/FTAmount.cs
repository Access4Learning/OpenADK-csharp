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

/// <summary>Dollar amount of the transaction.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class FTAmount : SifElement
{
	/// <summary>
	/// Creates an instance of a FTAmount
	/// </summary>
	public FTAmount() : base ( HrfinDTD.FTAMOUNT ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="financialAccountAccountingPeriodLocationInfoRefId">Account.</param>
	///<param name="type">Values: Debit, Credit</param>
	///<param name="value">Gets or sets the content value of the &amp;lt;FTAmount&amp;gt; element</param>
	///
	public FTAmount( string financialAccountAccountingPeriodLocationInfoRefId, FTAmountType type, decimal? value ) : base( HrfinDTD.FTAMOUNT )
	{
		this.FinancialAccountAccountingPeriodLocationInfoRefId = financialAccountAccountingPeriodLocationInfoRefId;
		this.SetType( type );
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected FTAmount( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { HrfinDTD.FTAMOUNT_FINANCIALACCOUNTACCOUNTINGPERIODLOCATIONINFOREFID, HrfinDTD.FTAMOUNT_TYPE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;FTAmount&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;FTAmount&amp;gt; element"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public decimal? Value
	{
		get
		{
			return (decimal?) GetSifSimpleFieldValue( HrfinDTD.FTAMOUNT ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.FTAMOUNT, new SifDecimal( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>FinancialAccountAccountingPeriodLocationInfoRefId</c> attribute.
	/// </summary>
	/// <value> The <c>FinancialAccountAccountingPeriodLocationInfoRefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Account."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string FinancialAccountAccountingPeriodLocationInfoRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.FTAMOUNT_FINANCIALACCOUNTACCOUNTINGPERIODLOCATIONINFOREFID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.FTAMOUNT_FINANCIALACCOUNTACCOUNTINGPERIODLOCATIONINFOREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>Type</c> attribute.
	/// </summary>
	/// <value> The <c>Type</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Values: Debit, Credit"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Type
	{
		get
		{ 
			return GetFieldValue( HrfinDTD.FTAMOUNT_TYPE );
		}
		set
		{
			SetField( HrfinDTD.FTAMOUNT_TYPE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>Type</c> attribute.
	/// </summary>
	/// <param name="val">A FTAmountType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Values: Debit, Credit"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetType( FTAmountType val )
	{
		SetField( HrfinDTD.FTAMOUNT_TYPE, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>Currency</c> attribute.
	/// </summary>
	/// <value> The <c>Currency</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Currency code. Where omitted, defaults to implementation-defined local currency, typically USD in the United States. "</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public string Currency
	{
		get
		{ 
			return GetFieldValue( HrfinDTD.FTAMOUNT_CURRENCY );
		}
		set
		{
			SetField( HrfinDTD.FTAMOUNT_CURRENCY, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>Currency</c> attribute.
	/// </summary>
	/// <param name="val">A CurrencyNames object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Currency code. Where omitted, defaults to implementation-defined local currency, typically USD in the United States. "</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void SetCurrency( CurrencyNames val )
	{
		SetField( HrfinDTD.FTAMOUNT_CURRENCY, val );
	}

}}
