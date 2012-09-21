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

namespace OpenADK.Library.au.Assessment{

/// <summary>An Items</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.4</para>
/// </remarks>
[Serializable]
public class Items : SifList<Item>
{
	/// <summary>
	/// Creates an instance of an Items
	/// </summary>
	public Items() : base ( AssessmentDTD.ITEMS ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="item">An Item</param>
	///
	public Items( Item item ) : base( AssessmentDTD.ITEMS )
	{
		this.SafeAddChild( AssessmentDTD.ITEMS_ITEM, item );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected Items( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
}}
