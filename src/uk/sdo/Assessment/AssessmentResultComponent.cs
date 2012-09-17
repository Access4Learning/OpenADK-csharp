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

/// <summary>An AssessmentResultComponent</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.1</para>
/// </remarks>
[Serializable]
public class AssessmentResultComponent : SifDataObject
{
	/// <summary>
	/// Creates an instance of an AssessmentResultComponent
	/// </summary>
	public AssessmentResultComponent() : base( Adk.SifVersion, AssessmentDTD.ASSESSMENTRESULTCOMPONENT ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">The ID (GUID) of this aspect or subtest.</param>
	///<param name="name">The name used to identify this component or aspect.</param>
	///<param name="shortDescription">Shorter description used for column headers in marksheets, etc.</param>
	///<param name="description">Longer text describing features of the component.</param>
	///<param name="componentType">Defines the associated result format.</param>
	///<param name="resultQualifier">Defines the format or type of result(s) awarded.</param>
	///<param name="assessmentMethod">The method or format of the Assessment.</param>
	///
	public AssessmentResultComponent( string refId, string name, string shortDescription, string description, ComponentType componentType, AssessmentResultQualifierType resultQualifier, AssessmentMethodType assessmentMethod ) : base( Adk.SifVersion, AssessmentDTD.ASSESSMENTRESULTCOMPONENT )
	{
		this.RefId = refId;
		this.Name = name;
		this.ShortDescription = shortDescription;
		this.Description = description;
		this.SetComponentType( componentType );
		this.SetResultQualifier( resultQualifier );
		this.SetAssessmentMethod( assessmentMethod );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected AssessmentResultComponent( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { AssessmentDTD.ASSESSMENTRESULTCOMPONENT_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The ID (GUID) of this aspect or subtest."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_REFID ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Name&gt;</c> element.
	/// </summary>
	/// <value> The <c>Name</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The name used to identify this component or aspect."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string Name
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_NAME ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_NAME, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;LocalId&gt;</c> element.
	/// </summary>
	/// <value> The <c>LocalId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The identifier used within the publishing application."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string LocalId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_LOCALID ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_LOCALID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ShortDescription&gt;</c> element.
	/// </summary>
	/// <value> The <c>ShortDescription</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Shorter description used for column headers in marksheets, etc."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string ShortDescription
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_SHORTDESCRIPTION ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_SHORTDESCRIPTION, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Description&gt;</c> element.
	/// </summary>
	/// <value> The <c>Description</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Longer text describing features of the component."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string Description
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_DESCRIPTION ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_DESCRIPTION, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;YearList&gt;</c> element.</summary>
	/// <param name="Year">A year with which the component grouping is applicable (by convention this is the end year of an academic year eg: 2007/8  resolves to 2008).</param>
	///<remarks>
	/// <para>This form of <c>setYearList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>YearList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetYearList( Year Year ) {
		RemoveChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_YEARLIST);
		AddChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_YEARLIST, new YearList( Year ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;YearList&gt;</c> element.
	/// </summary>
	/// <value> A YearList </value>
	/// <remarks>
	/// <para>To remove the <c>YearList</c>, set <c>YearList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public YearList YearList
	{
		get
		{
			return (YearList)GetChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_YEARLIST);
		}
		set
		{
			RemoveChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_YEARLIST);
			if( value != null)
			{
				AddChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_YEARLIST, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;AssessmentSubjectList&gt;</c> element.</summary>
	/// <param name="Subject">A subject area associated with this assessment component or aspect.</param>
	///<remarks>
	/// <para>This form of <c>setAssessmentSubjectList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>AssessmentSubjectList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetAssessmentSubjectList( AssessmentSubject Subject ) {
		RemoveChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTSUBJECTLIST);
		AddChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTSUBJECTLIST, new AssessmentSubjectList( Subject ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AssessmentSubjectList&gt;</c> element.
	/// </summary>
	/// <value> An AssessmentSubjectList </value>
	/// <remarks>
	/// <para>To remove the <c>AssessmentSubjectList</c>, set <c>AssessmentSubjectList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public AssessmentSubjectList AssessmentSubjectList
	{
		get
		{
			return (AssessmentSubjectList)GetChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTSUBJECTLIST);
		}
		set
		{
			RemoveChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTSUBJECTLIST);
			if( value != null)
			{
				AddChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTSUBJECTLIST, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;StageList&gt;</c> element.</summary>
	/// <param name="Stage">The assessed stage (this may well be a Keystage List, but there is no reason why it couldn’t be used for other concepts such as NcYear applicability, Level  Exam Level GCSE, A-Level etc.).</param>
	///<remarks>
	/// <para>This form of <c>setStageList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>StageList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetStageList( AssessmentStageRefId Stage ) {
		RemoveChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_STAGELIST);
		AddChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_STAGELIST, new StageList( Stage ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StageList&gt;</c> element.
	/// </summary>
	/// <value> A StageList </value>
	/// <remarks>
	/// <para>To remove the <c>StageList</c>, set <c>StageList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public StageList StageList
	{
		get
		{
			return (StageList)GetChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_STAGELIST);
		}
		set
		{
			RemoveChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_STAGELIST);
			if( value != null)
			{
				AddChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_STAGELIST, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AssessmentResultGradeSetRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>AssessmentResultGradeSetRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The gradeset associated with the component or aspect."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string AssessmentResultGradeSetRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTRESULTGRADESETREFID ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTRESULTGRADESETREFID, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;MarkSetList&gt;</c> element.</summary>
	/// <param name="MarkSet">A MarkSet</param>
	///<remarks>
	/// <para>This form of <c>setMarkSetList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>MarkSetList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetMarkSetList( MarkSet MarkSet ) {
		RemoveChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_MARKSETLIST);
		AddChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_MARKSETLIST, new MarkSetList( MarkSet ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;MarkSetList&gt;</c> element.
	/// </summary>
	/// <value> A MarkSetList </value>
	/// <remarks>
	/// <para>To remove the <c>MarkSetList</c>, set <c>MarkSetList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public MarkSetList MarkSetList
	{
		get
		{
			return (MarkSetList)GetChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_MARKSETLIST);
		}
		set
		{
			RemoveChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_MARKSETLIST);
			if( value != null)
			{
				AddChild( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_MARKSETLIST, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ComponentType&gt;</c> element.
	/// </summary>
	/// <value> The <c>ComponentType</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Defines the associated result format."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string ComponentType
	{
		get
		{ 
			return GetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_COMPONENTTYPE );
		}
		set
		{
			SetField( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_COMPONENTTYPE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;ComponentType&gt;</c> element.
	/// </summary>
	/// <param name="val">A ComponentType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Defines the associated result format."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetComponentType( ComponentType val )
	{
		SetField( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_COMPONENTTYPE, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ResultQualifier&gt;</c> element.
	/// </summary>
	/// <value> The <c>ResultQualifier</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Defines the format or type of result(s) awarded."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string ResultQualifier
	{
		get
		{ 
			return GetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_RESULTQUALIFIER );
		}
		set
		{
			SetField( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_RESULTQUALIFIER, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;ResultQualifier&gt;</c> element.
	/// </summary>
	/// <param name="val">A AssessmentResultQualifierType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Defines the format or type of result(s) awarded."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetResultQualifier( AssessmentResultQualifierType val )
	{
		SetField( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_RESULTQUALIFIER, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AssessmentMethod&gt;</c> element.
	/// </summary>
	/// <value> The <c>AssessmentMethod</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The method or format of the Assessment."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string AssessmentMethod
	{
		get
		{ 
			return GetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTMETHOD );
		}
		set
		{
			SetField( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTMETHOD, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;AssessmentMethod&gt;</c> element.
	/// </summary>
	/// <param name="val">A AssessmentMethodType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The method or format of the Assessment."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetAssessmentMethod( AssessmentMethodType val )
	{
		SetField( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_ASSESSMENTMETHOD, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SupplierName&gt;</c> element.
	/// </summary>
	/// <value> The <c>SupplierName</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The supplier/originator/designer/owner of the aspect."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string SupplierName
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_SUPPLIERNAME ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.ASSESSMENTRESULTCOMPONENT_SUPPLIERNAME, new SifString( value ), value );
		}
	}

}}