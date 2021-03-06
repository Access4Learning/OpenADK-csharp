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

/// <summary>This object describes the federal, state, local and other reimbursement rates for a school.  SIF_Events are reported.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class FoodserviceReimbursementRates : SifDataObject
{
	/// <summary>
	/// Creates an instance of a FoodserviceReimbursementRates
	/// </summary>
	public FoodserviceReimbursementRates() : base( Adk.SifVersion, FoodDTD.FOODSERVICEREIMBURSEMENTRATES ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">GUID that identifies this object.</param>
	///<param name="schoolInfoRefId">GUID that identifies the school.</param>
	///<param name="startDate">Effective start date for the reimbursement rates</param>
	///<param name="endDate">Effective end date for the reimbursement rates</param>
	///<param name="program">Refer to 5.1.23 Program.</param>
	///<param name="agencies">Reimbursing agency</param>
	///
	public FoodserviceReimbursementRates( string refId, string schoolInfoRefId, DateTime? startDate, DateTime? endDate, Program program, Agencies agencies ) : base( Adk.SifVersion, FoodDTD.FOODSERVICEREIMBURSEMENTRATES )
	{
		this.RefId = refId;
		this.SchoolInfoRefId = schoolInfoRefId;
		this.StartDate = startDate;
		this.EndDate = endDate;
		this.Program = program;
		this.Agencies = agencies;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected FoodserviceReimbursementRates( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { FoodDTD.FOODSERVICEREIMBURSEMENTRATES_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "GUID that identifies this object."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_REFID ) ;
		}
		set
		{
			SetFieldValue( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SchoolInfoRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>SchoolInfoRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "GUID that identifies the school."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string SchoolInfoRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_SCHOOLINFOREFID ) ;
		}
		set
		{
			SetFieldValue( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_SCHOOLINFOREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StartDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>StartDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Effective start date for the reimbursement rates"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public DateTime? StartDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_STARTDATE ) ;
		}
		set
		{
			SetFieldValue( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_STARTDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EndDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>EndDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Effective end date for the reimbursement rates"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public DateTime? EndDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_ENDDATE ) ;
		}
		set
		{
			SetFieldValue( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_ENDDATE, new SifDate( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;Program&gt;</c> element.</summary>
	/// <param name="Type">The type of the program. Type identifies the list of values.</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;Program&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setProgram</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>Program</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetProgram( FinancialProgram Type, string Value ) {
		RemoveChild( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_PROGRAM);
		AddChild( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_PROGRAM, new Program( Type, Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Program&gt;</c> element.
	/// </summary>
	/// <value> A Program </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Refer to 5.1.23 Program."</para>
	/// <para>To remove the <c>Program</c>, set <c>Program</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public Program Program
	{
		get
		{
			return (Program)GetChild( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_PROGRAM);
		}
		set
		{
			RemoveChild( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_PROGRAM);
			if( value != null)
			{
				AddChild( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_PROGRAM, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Agencies&gt;</c> element.
	/// </summary>
	/// <value> An Agencies </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Reimbursing agency"</para>
	/// <para>To remove the <c>Agencies</c>, set <c>Agencies</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public Agencies Agencies
	{
		get
		{
			return (Agencies)GetChild( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_AGENCIES);
		}
		set
		{
			RemoveChild( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_AGENCIES);
			if( value != null)
			{
				AddChild( FoodDTD.FOODSERVICEREIMBURSEMENTRATES_AGENCIES, value );
			}
		}
	}

}}
