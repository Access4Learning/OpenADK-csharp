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

namespace OpenADK.Library.au.Sif3assessment{

/// <summary>A Sif3AssessmentSubTest</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class Sif3AssessmentSubTest : SifDataObject
{
	/// <summary>
	/// Creates an instance of a Sif3AssessmentSubTest
	/// </summary>
	public Sif3AssessmentSubTest() : base( Adk.SifVersion, Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">A RefId</param>
	///<param name="subTestName">A SubTestName</param>
	///
	public Sif3AssessmentSubTest( string refId, string subTestName ) : base( Adk.SifVersion, Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST )
	{
		this.RefId = refId;
		this.SubTestName = subTestName;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected Sif3AssessmentSubTest( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_REFID ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubTestVersion&gt;</c> element.
	/// </summary>
	/// <value> The <c>SubTestVersion</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string SubTestVersion
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTVERSION ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTVERSION, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubTestPublishDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>SubTestPublishDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public DateTime? SubTestPublishDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTPUBLISHDATE ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTPUBLISHDATE, new SifDateTime( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;SubTestIdentifiers&gt;</c> element.</summary>
	/// <param name="SubTestIdentifier">A SubTestIdentifier</param>
	///<remarks>
	/// <para>This form of <c>setSubTestIdentifiers</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>SubTestIdentifiers</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetSubTestIdentifiers( SubTestIdentifier SubTestIdentifier ) {
		RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTIDENTIFIERS);
		AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTIDENTIFIERS, new SubTestIdentifiers( SubTestIdentifier ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubTestIdentifiers&gt;</c> element.
	/// </summary>
	/// <value> A SubTestIdentifiers </value>
	/// <remarks>
	/// <para>To remove the <c>SubTestIdentifiers</c>, set <c>SubTestIdentifiers</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public SubTestIdentifiers SubTestIdentifiers
	{
		get
		{
			return (SubTestIdentifiers)GetChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTIDENTIFIERS);
		}
		set
		{
			RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTIDENTIFIERS);
			if( value != null)
			{
				AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTIDENTIFIERS, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubTestName&gt;</c> element.
	/// </summary>
	/// <value> The <c>SubTestName</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string SubTestName
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTNAME ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTNAME, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;ScoreReporting&gt;</c> element.</summary>
	/// <param name="ScoreMetric">A ScoreMetric</param>
	///<remarks>
	/// <para>This form of <c>setScoreReporting</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ScoreReporting</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetScoreReporting( AUCodeSetsAssessmentReportingMethodType ScoreMetric ) {
		RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SCOREREPORTING);
		AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SCOREREPORTING, new ScoreReportingType( ScoreMetric ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ScoreReporting&gt;</c> element.
	/// </summary>
	/// <value> A ScoreReportingType </value>
	/// <remarks>
	/// <para>To remove the <c>ScoreReportingType</c>, set <c>ScoreReporting</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public ScoreReportingType ScoreReporting
	{
		get
		{
			return (ScoreReportingType)GetChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SCOREREPORTING);
		}
		set
		{
			RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SCOREREPORTING);
			if( value != null)
			{
				AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SCOREREPORTING, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;SubTestSubjectAreas&gt;</c> element.</summary>
	/// <param name="SubjectArea">Subject matter.</param>
	///<remarks>
	/// <para>This form of <c>setSubTestSubjectAreas</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>SubTestSubjectAreas</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetSubTestSubjectAreas( SubjectArea SubjectArea ) {
		RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTSUBJECTAREAS);
		AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTSUBJECTAREAS, new SubjectAreaList( SubjectArea ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubTestSubjectAreas&gt;</c> element.
	/// </summary>
	/// <value> A SubjectAreaList </value>
	/// <remarks>
	/// <para>To remove the <c>SubjectAreaList</c>, set <c>SubTestSubjectAreas</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public SubjectAreaList SubTestSubjectAreas
	{
		get
		{
			return (SubjectAreaList)GetChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTSUBJECTAREAS);
		}
		set
		{
			RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTSUBJECTAREAS);
			if( value != null)
			{
				AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTSUBJECTAREAS, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;SubTestGradeLevels&gt;</c> element.</summary>
	/// <param name="YearLevel">A YearLevel</param>
	///<remarks>
	/// <para>This form of <c>setSubTestGradeLevels</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>SubTestGradeLevels</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetSubTestGradeLevels( YearLevel YearLevel ) {
		RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTGRADELEVELS);
		AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTGRADELEVELS, new YearLevels( YearLevel ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubTestGradeLevels&gt;</c> element.
	/// </summary>
	/// <value> A YearLevels </value>
	/// <remarks>
	/// <para>To remove the <c>YearLevels</c>, set <c>SubTestGradeLevels</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public YearLevels SubTestGradeLevels
	{
		get
		{
			return (YearLevels)GetChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTGRADELEVELS);
		}
		set
		{
			RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTGRADELEVELS);
			if( value != null)
			{
				AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTGRADELEVELS, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;AssessmentSubTestRefIds&gt;</c> element.</summary>
	/// <param name="AssessmentSubTestRefId">An AssessmentSubTestRefId</param>
	///<remarks>
	/// <para>This form of <c>setAssessmentSubTestRefIds</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>AssessmentSubTestRefIds</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetAssessmentSubTestRefIds( Sif3AssessmentSubTestRefId AssessmentSubTestRefId ) {
		RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTSUBTESTREFIDS);
		AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTSUBTESTREFIDS, new Sif3AssessmentSubTestRefIds( AssessmentSubTestRefId ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AssessmentSubTestRefIds&gt;</c> element.
	/// </summary>
	/// <value> A Sif3AssessmentSubTestRefIds </value>
	/// <remarks>
	/// <para>To remove the <c>Sif3AssessmentSubTestRefIds</c>, set <c>AssessmentSubTestRefIds</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public Sif3AssessmentSubTestRefIds AssessmentSubTestRefIds
	{
		get
		{
			return (Sif3AssessmentSubTestRefIds)GetChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTSUBTESTREFIDS);
		}
		set
		{
			RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTSUBTESTREFIDS);
			if( value != null)
			{
				AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTSUBTESTREFIDS, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ContainerOnly&gt;</c> element.
	/// </summary>
	/// <value> The <c>ContainerOnly</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public bool? ContainerOnly
	{
		get
		{
			return (bool?) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_CONTAINERONLY ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_CONTAINERONLY, new SifBoolean( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SubTestTier&gt;</c> element.
	/// </summary>
	/// <value> The <c>SubTestTier</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public int? SubTestTier
	{
		get
		{
			return (int?) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTTIER ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_SUBTESTTIER, new SifInt( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;LearningStandardItemRefIds&gt;</c> element.</summary>
	/// <param name="LearningStandardItemRefId">A LearningStandardItemRefId</param>
	///<remarks>
	/// <para>This form of <c>setLearningStandardItemRefIds</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>LearningStandardItemRefIds</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetLearningStandardItemRefIds( LearningStandardItemRefId LearningStandardItemRefId ) {
		RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_LEARNINGSTANDARDITEMREFIDS);
		AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_LEARNINGSTANDARDITEMREFIDS, new LearningStandardItems( LearningStandardItemRefId ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;LearningStandardItemRefIds&gt;</c> element.
	/// </summary>
	/// <value> A LearningStandardItems </value>
	/// <remarks>
	/// <para>To remove the <c>LearningStandardItems</c>, set <c>LearningStandardItemRefIds</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public LearningStandardItems LearningStandardItemRefIds
	{
		get
		{
			return (LearningStandardItems)GetChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_LEARNINGSTANDARDITEMREFIDS);
		}
		set
		{
			RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_LEARNINGSTANDARDITEMREFIDS);
			if( value != null)
			{
				AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_LEARNINGSTANDARDITEMREFIDS, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Abbreviation&gt;</c> element.
	/// </summary>
	/// <value> The <c>Abbreviation</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Abbreviation
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ABBREVIATION ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ABBREVIATION, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Description&gt;</c> element.
	/// </summary>
	/// <value> The <c>Description</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Description
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_DESCRIPTION ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_DESCRIPTION, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;NumberOfItems&gt;</c> element.
	/// </summary>
	/// <value> The <c>NumberOfItems</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public int? NumberOfItems
	{
		get
		{
			return (int?) GetSifSimpleFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_NUMBEROFITEMS ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_NUMBEROFITEMS, new SifInt( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;AssessmentItems&gt;</c> element.</summary>
	/// <param name="AssessmentItem">An AssessmentItem</param>
	///<remarks>
	/// <para>This form of <c>setAssessmentItems</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>AssessmentItems</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetAssessmentItems( Sif3AssessmentItem AssessmentItem ) {
		RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTITEMS);
		AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTITEMS, new Sif3AssessmentItemList( AssessmentItem ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AssessmentItems&gt;</c> element.
	/// </summary>
	/// <value> A Sif3AssessmentItemList </value>
	/// <remarks>
	/// <para>To remove the <c>Sif3AssessmentItemList</c>, set <c>AssessmentItems</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public Sif3AssessmentItemList AssessmentItems
	{
		get
		{
			return (Sif3AssessmentItemList)GetChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTITEMS);
		}
		set
		{
			RemoveChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTITEMS);
			if( value != null)
			{
				AddChild( Sif3assessmentDTD.SIF3ASSESSMENTSUBTEST_ASSESSMENTITEMS, value );
			}
		}
	}

}}
