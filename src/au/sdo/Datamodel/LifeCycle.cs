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

namespace OpenADK.Library.au.Datamodel{

/// <summary>This common metadata element describes the life cycle of the object it represents, based on the IEEE LOM LifeCycle element</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class LifeCycle : SifElement
{
	/// <summary>
	/// Creates an instance of a LifeCycle
	/// </summary>
	public LifeCycle() : base ( DatamodelDTD.LIFECYCLE ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected LifeCycle( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Sets the value of the <c>&lt;Created&gt;</c> element.</summary>
	/// <param name="DateTime">A DateTime</param>
	///<remarks>
	/// <para>This form of <c>setCreated</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>Created</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetCreated( DateTime? DateTime ) {
		RemoveChild( DatamodelDTD.LIFECYCLE_CREATED);
		AddChild( DatamodelDTD.LIFECYCLE_CREATED, new Created( DateTime ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Created&gt;</c> element.
	/// </summary>
	/// <value> A Created </value>
	/// <remarks>
	/// <para>To remove the <c>Created</c>, set <c>Created</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public Created Created
	{
		get
		{
			return (Created)GetChild( DatamodelDTD.LIFECYCLE_CREATED);
		}
		set
		{
			RemoveChild( DatamodelDTD.LIFECYCLE_CREATED);
			if( value != null)
			{
				AddChild( DatamodelDTD.LIFECYCLE_CREATED, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ModificationHistory&gt;</c> element.
	/// </summary>
	/// <value> A ModificationHistory </value>
	/// <remarks>
	/// <para>To remove the <c>ModificationHistory</c>, set <c>ModificationHistory</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public ModificationHistory ModificationHistory
	{
		get
		{
			return (ModificationHistory)GetChild( DatamodelDTD.LIFECYCLE_MODIFICATIONHISTORY);
		}
		set
		{
			RemoveChild( DatamodelDTD.LIFECYCLE_MODIFICATIONHISTORY);
			if( value != null)
			{
				AddChild( DatamodelDTD.LIFECYCLE_MODIFICATIONHISTORY, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;TimeElements&gt;</c> element.
	/// </summary>
	/// <value> A TimeElements </value>
	/// <remarks>
	/// <para>To remove the <c>TimeElements</c>, set <c>TimeElements</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public TimeElements TimeElements
	{
		get
		{
			return (TimeElements)GetChild( DatamodelDTD.LIFECYCLE_TIMEELEMENTS);
		}
		set
		{
			RemoveChild( DatamodelDTD.LIFECYCLE_TIMEELEMENTS);
			if( value != null)
			{
				AddChild( DatamodelDTD.LIFECYCLE_TIMEELEMENTS, value );
			}
		}
	}

}}
