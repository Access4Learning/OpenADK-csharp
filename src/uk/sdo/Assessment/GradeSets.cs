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

namespace OpenADK.Library.uk.Assessment{

/// <summary>A GradeSets</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.1</para>
/// </remarks>
[Serializable]
public class GradeSets : SifList<GradeSet>
{
	/// <summary>
	/// Creates an instance of a GradeSets
	/// </summary>
	public GradeSets() : base ( AssessmentDTD.GRADESETS ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="gradeSet">A GradeSet</param>
	///
	public GradeSets( GradeSet gradeSet ) : base( AssessmentDTD.GRADESETS )
	{
		this.SafeAddChild( AssessmentDTD.GRADESETS_GRADESET, gradeSet );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected GradeSets( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { AssessmentDTD.GRADESETS_GRADESET }; }
	}

	///<summary>Adds the value of the <c>&lt;GradeSet&gt;</c> element.</summary>
	/// <param name="StartDate">The start date from which this version is valid.</param>
	/// <param name="Grades">A Grades</param>
	///<remarks>
	/// <para>This form of <c>setGradeSet</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddGradeSet</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void AddGradeSet( DateTime? StartDate, Grade Grades ) {
		AddChild( AssessmentDTD.GRADESETS_GRADESET, new GradeSet( StartDate, Grades ) );
	}

}}
