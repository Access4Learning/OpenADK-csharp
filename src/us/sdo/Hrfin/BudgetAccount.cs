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

/// <summary>Information about budget accounts being submitted</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class BudgetAccount : SifElement
{
	/// <summary>
	/// Creates an instance of a BudgetAccount
	/// </summary>
	public BudgetAccount() : base ( HrfinDTD.BUDGETACCOUNT ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="accountClass">Classification of budgetary account code summary</param>
	///<param name="functionBreakdown">Function breakdown of account code for summarization.</param>
	///<param name="budgetAmount">Summarized, positive amount without cents for breakdowns</param>
	///
	public BudgetAccount( AccountClass accountClass, string functionBreakdown, MonetaryAmount budgetAmount ) : base( HrfinDTD.BUDGETACCOUNT )
	{
		this.SetAccountClass( accountClass );
		this.FunctionBreakdown = functionBreakdown;
		this.BudgetAmount = budgetAmount;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected BudgetAccount( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;AccountClass&gt;</c> element.
	/// </summary>
	/// <value> The <c>AccountClass</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Classification of budgetary account code summary"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string AccountClass
	{
		get
		{ 
			return GetFieldValue( HrfinDTD.BUDGETACCOUNT_ACCOUNTCLASS );
		}
		set
		{
			SetField( HrfinDTD.BUDGETACCOUNT_ACCOUNTCLASS, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;AccountClass&gt;</c> element.
	/// </summary>
	/// <param name="val">A AccountClass object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Classification of budgetary account code summary"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetAccountClass( AccountClass val )
	{
		SetField( HrfinDTD.BUDGETACCOUNT_ACCOUNTCLASS, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;FundBreakdown&gt;</c> element.
	/// </summary>
	/// <value> The <c>FundBreakdown</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Fund breakdown of account code for summarization."</para>
	/// <para>This element is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering elements of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;FundBreakdown&gt;</description></item>
	/// </list>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string FundBreakdown
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.BUDGETACCOUNT_FUNDBREAKDOWN ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.BUDGETACCOUNT_FUNDBREAKDOWN, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;FunctionBreakdown&gt;</c> element.
	/// </summary>
	/// <value> The <c>FunctionBreakdown</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Function breakdown of account code for summarization."</para>
	/// <para>This element is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering elements of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;FunctionBreakdown&gt;</description></item>
	/// </list>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string FunctionBreakdown
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.BUDGETACCOUNT_FUNCTIONBREAKDOWN ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.BUDGETACCOUNT_FUNCTIONBREAKDOWN, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ObjectBreakdown&gt;</c> element.
	/// </summary>
	/// <value> The <c>ObjectBreakdown</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Object breakdown of account code for summarization."</para>
	/// <para>This element is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering elements of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;ObjectBreakdown&gt;</description></item>
	/// </list>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string ObjectBreakdown
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.BUDGETACCOUNT_OBJECTBREAKDOWN ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.BUDGETACCOUNT_OBJECTBREAKDOWN, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;BudgetAmount&gt;</c> element.</summary>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;MonetaryAmount&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setBudgetAmount</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>BudgetAmount</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetBudgetAmount( decimal? Value ) {
		RemoveChild( HrfinDTD.BUDGETACCOUNT_BUDGETAMOUNT);
		AddChild( HrfinDTD.BUDGETACCOUNT_BUDGETAMOUNT, new MonetaryAmount( Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;BudgetAmount&gt;</c> element.
	/// </summary>
	/// <value> A MonetaryAmount </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Summarized, positive amount without cents for breakdowns"</para>
	/// <para>To remove the <c>MonetaryAmount</c>, set <c>BudgetAmount</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public MonetaryAmount BudgetAmount
	{
		get
		{
			return (MonetaryAmount)GetChild( HrfinDTD.BUDGETACCOUNT_BUDGETAMOUNT);
		}
		set
		{
			RemoveChild( HrfinDTD.BUDGETACCOUNT_BUDGETAMOUNT);
			if( value != null)
			{
				AddChild( HrfinDTD.BUDGETACCOUNT_BUDGETAMOUNT, value );
			}
		}
	}

}}
