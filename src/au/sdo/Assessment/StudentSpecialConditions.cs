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

/// <summary>A StudentSpecialConditions</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.4</para>
/// </remarks>
[Serializable]
public class StudentSpecialConditions : SifKeyedList<StudentSpecialCondition>
{
	/// <summary>
	/// Creates an instance of a StudentSpecialConditions
	/// </summary>
	public StudentSpecialConditions() : base ( AssessmentDTD.STUDENTSPECIALCONDITIONS ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="studentSpecialCondition">A description of the special condition.  Student special conditions are different
	/// from special conditions of the test.</param>
	///
	public StudentSpecialConditions( StudentSpecialCondition studentSpecialCondition ) : base( AssessmentDTD.STUDENTSPECIALCONDITIONS )
	{
		this.SafeAddChild( AssessmentDTD.STUDENTSPECIALCONDITIONS_STUDENTSPECIALCONDITION, studentSpecialCondition );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected StudentSpecialConditions( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { AssessmentDTD.STUDENTSPECIALCONDITIONS_STUDENTSPECIALCONDITION }; }
	}

	///<summary>Adds the value of the <c>&lt;StudentSpecialCondition&gt;</c> element.</summary>
	/// <param name="Code">A code indicating the type of special condition.</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;StudentSpecialCondition&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setStudentSpecialCondition</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddStudentSpecialCondition</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void AddStudentSpecialCondition( string Code, string Value ) {
		AddChild( AssessmentDTD.STUDENTSPECIALCONDITIONS_STUDENTSPECIALCONDITION, new StudentSpecialCondition( Code, Value ) );
	}

}}
