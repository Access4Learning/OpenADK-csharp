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

namespace OpenADK.Library.us.Common{

/// <summary>This list contains information that describes each specific instructional, related or transitional service that has been prescribed or recommended in the program plan developed for a student who has been placed in an individualized special program.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.4</para>
/// </remarks>
[Serializable]
public class StudentPlacementList : SifKeyedList<StudentPlacementData>
{
	/// <summary>
	/// Creates an instance of a StudentPlacementList
	/// </summary>
	public StudentPlacementList() : base ( CommonDTD.STUDENTPLACEMENTLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="studentPlacementData">Collects elements of interest for each StudentPlacement.</param>
	///
	public StudentPlacementList( StudentPlacementData studentPlacementData ) : base( CommonDTD.STUDENTPLACEMENTLIST )
	{
		this.SafeAddChild( CommonDTD.STUDENTPLACEMENTLIST_STUDENTPLACEMENTDATA, studentPlacementData );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected StudentPlacementList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { CommonDTD.STUDENTPLACEMENTLIST_STUDENTPLACEMENTDATA }; }
	}

	///<summary>Adds the value of the <c>&lt;StudentPlacementData&gt;</c> element.</summary>
	/// <param name="Service">A Service</param>
	///<remarks>
	/// <para>This form of <c>setStudentPlacementData</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddStudentPlacementData</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void AddStudentPlacementData( Service Service ) {
		AddChild( CommonDTD.STUDENTPLACEMENTLIST_STUDENTPLACEMENTDATA, new StudentPlacementData( Service ) );
	}

}}
