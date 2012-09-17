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

namespace OpenADK.Library.us.Food{

/// <summary>A Prices</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class Prices : SifKeyedList<Price>
{
	/// <summary>
	/// Creates an instance of a Prices
	/// </summary>
	public Prices() : base ( FoodDTD.PRICES ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="price">A Price</param>
	///
	public Prices( Price price ) : base( FoodDTD.PRICES )
	{
		this.SafeAddChild( FoodDTD.PRICES_PRICE, price );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected Prices( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;Price&gt;</c> element.</summary>
	/// <param name="Value">The price value</param>
	/// <param name="MealStatus">This gives information about a student's meal status.</param>
	/// <param name="MealType">This gives information about a meal type</param>
	/// <param name="GradeLevels">Refer to section 5.1.12 GradeLevels.</param>
	///<remarks>
	/// <para>This form of <c>setPrice</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddPrice</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void AddPrice( decimal? Value, MealStatus MealStatus, MealTypes MealType, GradeLevels GradeLevels ) {
		AddChild( FoodDTD.PRICES_PRICE, new Price( Value, MealStatus, MealType, GradeLevels ) );
	}

}}
