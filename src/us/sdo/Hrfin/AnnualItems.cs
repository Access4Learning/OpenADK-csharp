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

/// <summary>An AnnualItems</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class AnnualItems : SifList<AnnualItem>
{
	/// <summary>
	/// Creates an instance of an AnnualItems
	/// </summary>
	public AnnualItems() : base ( HrfinDTD.ANNUALITEMS ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="annualItem">An AnnualItem</param>
	///
	public AnnualItems( AnnualItem annualItem ) : base( HrfinDTD.ANNUALITEMS )
	{
		this.SafeAddChild( HrfinDTD.ANNUALITEMS_ANNUALITEM, annualItem );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected AnnualItems( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;AnnualItem&gt;</c> element.</summary>
	/// <param name="AccountType">Classification of budgetary account code summary</param>
	/// <param name="FundType">Is the account a general or special account code designation?</param>
	/// <param name="Function">Function break in account code for summarization.</param>
	/// <param name="Amount">Summarized amount.</param>
	///<remarks>
	/// <para>This form of <c>setAnnualItem</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddAnnualItem</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddAnnualItem( AccountClass AccountType, FundType FundType, string Function, MonetaryAmount Amount ) {
		AddChild( HrfinDTD.ANNUALITEMS_ANNUALITEM, new AnnualItem( AccountType, FundType, Function, Amount ) );
	}

}}
