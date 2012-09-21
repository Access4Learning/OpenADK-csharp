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
using OpenADK.Library.uk.Common;

namespace OpenADK.Library.uk.Learning{

/// <summary>A NCYearList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class NCYearList : SifKeyedList<NCYearGroup>
{
	/// <summary>
	/// Creates an instance of a NCYearList
	/// </summary>
	public NCYearList() : base ( LearningDTD.NCYEARLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="ncYear">NC Year Group</param>
	///
	public NCYearList( NCYearGroup ncYear ) : base( LearningDTD.NCYEARLIST )
	{
		this.SafeAddChild( LearningDTD.NCYEARLIST_NCYEAR, ncYear );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected NCYearList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { LearningDTD.NCYEARLIST_NCYEAR }; }
	}

	///<summary>Adds the value of the <c>&lt;NCYear&gt;</c> element.</summary>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;NCYearGroup&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setNCYear</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddNCYear</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void AddNCYear( NCYear Value ) {
		AddChild( LearningDTD.NCYEARLIST_NCYEAR, new NCYearGroup( Value ) );
	}

}}
