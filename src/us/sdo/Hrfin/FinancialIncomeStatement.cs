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

/// <summary>This object communicates the income statement details for a location and an accounting period.  SIF_Events are reported.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class FinancialIncomeStatement : SifDataObject
{
	/// <summary>
	/// Creates an instance of a FinancialIncomeStatement
	/// </summary>
	public FinancialIncomeStatement() : base( Adk.SifVersion, HrfinDTD.FINANCIALINCOMESTATEMENT ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">GUID that identifies this income statement.</param>
	///<param name="generationDate">Generation date.</param>
	///<param name="generationTime">Generation time.</param>
	///<param name="locationInfoRefId">LocationInfo reference.</param>
	///<param name="period">Period of the income statement.</param>
	///<param name="amounts">Amount in the account.</param>
	///
	public FinancialIncomeStatement( string refId, DateTime? generationDate, DateTime? generationTime, string locationInfoRefId, FISPeriod period, FISAmounts amounts ) : base( Adk.SifVersion, HrfinDTD.FINANCIALINCOMESTATEMENT )
	{
		this.RefId = refId;
		this.GenerationDate = generationDate;
		this.GenerationTime = generationTime;
		this.LocationInfoRefId = locationInfoRefId;
		this.Period = period;
		this.Amounts = amounts;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected FinancialIncomeStatement( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { HrfinDTD.FINANCIALINCOMESTATEMENT_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "GUID that identifies this income statement."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.FINANCIALINCOMESTATEMENT_REFID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.FINANCIALINCOMESTATEMENT_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;GenerationDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>GenerationDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Generation date."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public DateTime? GenerationDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( HrfinDTD.FINANCIALINCOMESTATEMENT_GENERATIONDATE ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.FINANCIALINCOMESTATEMENT_GENERATIONDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;GenerationTime&gt;</c> element.
	/// </summary>
	/// <value> The <c>GenerationTime</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Generation time."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public DateTime? GenerationTime
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( HrfinDTD.FINANCIALINCOMESTATEMENT_GENERATIONTIME ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.FINANCIALINCOMESTATEMENT_GENERATIONTIME, new SifTime( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;LocationInfoRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>LocationInfoRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "LocationInfo reference."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string LocationInfoRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.FINANCIALINCOMESTATEMENT_LOCATIONINFOREFID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.FINANCIALINCOMESTATEMENT_LOCATIONINFOREFID, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;Period&gt;</c> element.</summary>
	/// <param name="StartDate">Start date.</param>
	/// <param name="EndDate">End date.</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;FISPeriod&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setPeriod</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>Period</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetPeriod( DateTime? StartDate, DateTime? EndDate, string Value ) {
		RemoveChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PERIOD);
		AddChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PERIOD, new FISPeriod( StartDate, EndDate, Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Period&gt;</c> element.
	/// </summary>
	/// <value> A FISPeriod </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Period of the income statement."</para>
	/// <para>This null is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering nulls of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;Period&gt;</description></item>
	/// </list>
	/// <para>To remove the <c>FISPeriod</c>, set <c>Period</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public FISPeriod Period
	{
		get
		{
			return (FISPeriod)GetChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PERIOD);
		}
		set
		{
			RemoveChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PERIOD);
			if( value != null)
			{
				AddChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PERIOD, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;Amounts&gt;</c> element.</summary>
	/// <param name="Amount">An Amount</param>
	///<remarks>
	/// <para>This form of <c>setAmounts</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>Amounts</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetAmounts( FISAmount Amount ) {
		RemoveChild( HrfinDTD.FINANCIALINCOMESTATEMENT_AMOUNTS);
		AddChild( HrfinDTD.FINANCIALINCOMESTATEMENT_AMOUNTS, new FISAmounts( Amount ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Amounts&gt;</c> element.
	/// </summary>
	/// <value> A FISAmounts </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Amount in the account."</para>
	/// <para>To remove the <c>FISAmounts</c>, set <c>Amounts</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public FISAmounts Amounts
	{
		get
		{
			return (FISAmounts)GetChild( HrfinDTD.FINANCIALINCOMESTATEMENT_AMOUNTS);
		}
		set
		{
			RemoveChild( HrfinDTD.FINANCIALINCOMESTATEMENT_AMOUNTS);
			if( value != null)
			{
				AddChild( HrfinDTD.FINANCIALINCOMESTATEMENT_AMOUNTS, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;Program&gt;</c> element.</summary>
	/// <param name="Type">The type of the program. Type identifies the list of values.</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;Program&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setProgram</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>Program</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetProgram( FinancialProgram Type, string Value ) {
		RemoveChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PROGRAM);
		AddChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PROGRAM, new Program( Type, Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Program&gt;</c> element.
	/// </summary>
	/// <value> A Program </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "See 5.1.23 Program."</para>
	/// <para>To remove the <c>Program</c>, set <c>Program</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public Program Program
	{
		get
		{
			return (Program)GetChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PROGRAM);
		}
		set
		{
			RemoveChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PROGRAM);
			if( value != null)
			{
				AddChild( HrfinDTD.FINANCIALINCOMESTATEMENT_PROGRAM, value );
			}
		}
	}

}}
