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

/// <summary>A FISAmounts</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class FISAmounts : SifList<FISAmount>
{
	/// <summary>
	/// Creates an instance of a FISAmounts
	/// </summary>
	public FISAmounts() : base ( HrfinDTD.FISAMOUNTS ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="amount">An Amount</param>
	///
	public FISAmounts( FISAmount amount ) : base( HrfinDTD.FISAMOUNTS )
	{
		this.SafeAddChild( HrfinDTD.FISAMOUNTS_AMOUNT, amount );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected FISAmounts( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { HrfinDTD.FISAMOUNTS_AMOUNT }; }
	}

	///<summary>Adds the value of the <c>&lt;Amount&gt;</c> element.</summary>
	/// <param name="FinancialAccountAccountingPeriodLocationInfoRefId">FinancialAccountAccountingPeriodLocationInfo reference.</param>
	/// <param name="FinancialClassRefId">Class of the financial account.</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;FISAmount&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setAmount</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddAmount</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddAmount( string FinancialAccountAccountingPeriodLocationInfoRefId, string FinancialClassRefId, decimal? Value ) {
		AddChild( HrfinDTD.FISAMOUNTS_AMOUNT, new FISAmount( FinancialAccountAccountingPeriodLocationInfoRefId, FinancialClassRefId, Value ) );
	}

}}
