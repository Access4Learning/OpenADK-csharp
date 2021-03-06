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

namespace OpenADK.Library.au.School{

/// <summary>A SchoolCourseInfoOverride</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class SchoolCourseInfoOverride : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a SchoolCourseInfoOverride
	/// </summary>
	public SchoolCourseInfoOverride() : base ( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="overrideValue">An Override</param>
	///
	public SchoolCourseInfoOverride( YesNoMixed overrideValue ) : base( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE )
	{
		this.SetOverride( overrideValue );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SchoolCourseInfoOverride( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_OVERRIDE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>Override</c> attribute.
	/// </summary>
	/// <value> The <c>Override</c> attribute of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Override
	{
		get
		{ 
			return GetFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_OVERRIDE );
		}
		set
		{
			SetField( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_OVERRIDE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>Override</c> attribute.
	/// </summary>
	/// <param name="val">A YesNoMixed object</param>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetOverride( YesNoMixed val )
	{
		SetField( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_OVERRIDE, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;CourseCode&gt;</c> element.
	/// </summary>
	/// <value> The <c>CourseCode</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string CourseCode
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_COURSECODE ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_COURSECODE, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StateCourseCode&gt;</c> element.
	/// </summary>
	/// <value> The <c>StateCourseCode</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string StateCourseCode
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_STATECOURSECODE ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_STATECOURSECODE, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;DistrictCourseCode&gt;</c> element.
	/// </summary>
	/// <value> The <c>DistrictCourseCode</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string DistrictCourseCode
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_DISTRICTCOURSECODE ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_DISTRICTCOURSECODE, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;SubjectArea&gt;</c> element.</summary>
	/// <param name="Code">The subject area details</param>
	///<remarks>
	/// <para>This form of <c>setSubjectArea</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>SubjectArea</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetSubjectArea( string Code ) {
		RemoveChild( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_SUBJECTAREA);
		AddChild( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_SUBJECTAREA, new SubjectArea( Code ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubjectArea&gt;</c> element.
	/// </summary>
	/// <value> A SubjectArea </value>
	/// <remarks>
	/// <para>To remove the <c>SubjectArea</c>, set <c>SubjectArea</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public SubjectArea SubjectArea
	{
		get
		{
			return (SubjectArea)GetChild( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_SUBJECTAREA);
		}
		set
		{
			RemoveChild( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_SUBJECTAREA);
			if( value != null)
			{
				AddChild( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_SUBJECTAREA, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;CourseTitle&gt;</c> element.
	/// </summary>
	/// <value> The <c>CourseTitle</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string CourseTitle
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_COURSETITLE ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_COURSETITLE, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;InstructionalLevel&gt;</c> element.
	/// </summary>
	/// <value> The <c>InstructionalLevel</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string InstructionalLevel
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_INSTRUCTIONALLEVEL ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_INSTRUCTIONALLEVEL, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;CourseCredits&gt;</c> element.
	/// </summary>
	/// <value> The <c>CourseCredits</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string CourseCredits
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_COURSECREDITS ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.SCHOOLCOURSEINFOOVERRIDE_COURSECREDITS, new SifString( value ), value );
		}
	}

}}
