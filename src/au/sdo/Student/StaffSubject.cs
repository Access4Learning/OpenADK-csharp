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

namespace OpenADK.Library.au.Student{

/// <summary>A StaffSubject</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class StaffSubject : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a StaffSubject
	/// </summary>
	public StaffSubject() : base ( StudentDTD.STAFFSUBJECT ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="preferenceNumber">Priority of Subject to Teach</param>
	///
	public StaffSubject( int? preferenceNumber ) : base( StudentDTD.STAFFSUBJECT )
	{
		this.PreferenceNumber = preferenceNumber;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected StaffSubject( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { StudentDTD.STAFFSUBJECT_PREFERENCENUMBER }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;PreferenceNumber&gt;</c> element.
	/// </summary>
	/// <value> The <c>PreferenceNumber</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Priority of Subject to Teach"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public int? PreferenceNumber
	{
		get
		{
			return (int?) GetSifSimpleFieldValue( StudentDTD.STAFFSUBJECT_PREFERENCENUMBER ) ;
		}
		set
		{
			SetFieldValue( StudentDTD.STAFFSUBJECT_PREFERENCENUMBER, new SifInt( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubjectLocalId&gt;</c> element.
	/// </summary>
	/// <value> The <c>SubjectLocalId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Local Subject Id"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string SubjectLocalId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( StudentDTD.STAFFSUBJECT_SUBJECTLOCALID ) ;
		}
		set
		{
			SetFieldValue( StudentDTD.STAFFSUBJECT_SUBJECTLOCALID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;TimeTableSubjectRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>TimeTableSubjectRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "RefId of TimeTableSubject"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string TimeTableSubjectRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( StudentDTD.STAFFSUBJECT_TIMETABLESUBJECTREFID ) ;
		}
		set
		{
			SetFieldValue( StudentDTD.STAFFSUBJECT_TIMETABLESUBJECTREFID, new SifString( value ), value );
		}
	}

}}
