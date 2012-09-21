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

/// <summary>A PayRates</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class PayRates : SifKeyedList<PayRate>
{
	/// <summary>
	/// Creates an instance of a PayRates
	/// </summary>
	public PayRates() : base ( HrfinDTD.PAYRATES ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="payRate">A PayRate</param>
	///
	public PayRates( PayRate payRate ) : base( HrfinDTD.PAYRATES )
	{
		this.SafeAddChild( HrfinDTD.PAYRATES_PAYRATE, payRate );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected PayRates( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;PayRate&gt;</c> element.</summary>
	/// <param name="Type">Type of pay</param>
	/// <param name="Amount">Pay amount.</param>
	/// <param name="Percentage">Percentage of pay this represents.</param>
	///<remarks>
	/// <para>This form of <c>setPayRate</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddPayRate</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddPayRate( PayRateType Type, MonetaryAmount Amount, decimal? Percentage ) {
		AddChild( HrfinDTD.PAYRATES_PAYRATE, new PayRate( Type, Amount, Percentage ) );
	}

}}
