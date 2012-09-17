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

namespace OpenADK.Library.au.Programs{

/// <summary>A StudentParticipation</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class StudentParticipation : SifDataObject
{
	/// <summary>
	/// Creates an instance of a StudentParticipation
	/// </summary>
	public StudentParticipation() : base( Adk.SifVersion, ProgramsDTD.STUDENTPARTICIPATION ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">This is the unique identification code</param>
	///<param name="studentPersonalRefId">The GUID of the student that this object is linked to.</param>
	///<param name="studentParticipationAsOfDate">Effective date (NOT the entry date) of this StudentParticipation instance for the identified student and program.        Each time there is a change to the student's program participation profile, a new instance of this object is to be generated with        the appropriate StudentParticipationAsOfDate and a new RefId.</param>
	///<param name="managingSchool">A ManagingSchool</param>
	///
	public StudentParticipation( string refId, string studentPersonalRefId, DateTime? studentParticipationAsOfDate, ManagingSchool managingSchool ) : base( Adk.SifVersion, ProgramsDTD.STUDENTPARTICIPATION )
	{
		this.RefId = refId;
		this.StudentPersonalRefId = studentPersonalRefId;
		this.StudentParticipationAsOfDate = studentParticipationAsOfDate;
		this.ManagingSchool = managingSchool;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected StudentParticipation( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { ProgramsDTD.STUDENTPARTICIPATION_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "This is the unique identification code"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_REFID ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StudentPersonalRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>StudentPersonalRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The GUID of the student that this object is linked to."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string StudentPersonalRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_STUDENTPERSONALREFID ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_STUDENTPERSONALREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StudentParticipationAsOfDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>StudentParticipationAsOfDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Effective date (NOT the entry date) of this StudentParticipation instance for the identified student and program.        Each time there is a change to the student's program participation profile, a new instance of this object is to be generated with        the appropriate StudentParticipationAsOfDate and a new RefId."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public DateTime? StudentParticipationAsOfDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_STUDENTPARTICIPATIONASOFDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_STUDENTPARTICIPATIONASOFDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ProgramType&gt;</c> element.
	/// </summary>
	/// <value> The <c>ProgramType</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Identifies the individualised program for which the student's participation is described in this instance"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string ProgramType
	{
		get
		{ 
			return GetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMTYPE );
		}
		set
		{
			SetField( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMTYPE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;ProgramType&gt;</c> element.
	/// </summary>
	/// <param name="val">A StudentFamilyProgramType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Identifies the individualised program for which the student's participation is described in this instance"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetProgramType( StudentFamilyProgramType val )
	{
		SetField( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMTYPE, val );
	}

	///<summary>Sets the value of the <c>&lt;ProgramFundingSources&gt;</c> element.</summary>
	/// <param name="ProgramFundingSource">A ProgramFundingSource</param>
	///<remarks>
	/// <para>This form of <c>setProgramFundingSources</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ProgramFundingSources</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetProgramFundingSources( ProgramFundingSource ProgramFundingSource ) {
		RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMFUNDINGSOURCES);
		AddChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMFUNDINGSOURCES, new ProgramFundingSources( ProgramFundingSource ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ProgramFundingSources&gt;</c> element.
	/// </summary>
	/// <value> A ProgramFundingSources </value>
	/// <remarks>
	/// <para>To remove the <c>ProgramFundingSources</c>, set <c>ProgramFundingSources</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public ProgramFundingSources ProgramFundingSources
	{
		get
		{
			return (ProgramFundingSources)GetChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMFUNDINGSOURCES);
		}
		set
		{
			RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMFUNDINGSOURCES);
			if( value != null)
			{
				AddChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMFUNDINGSOURCES, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;ManagingSchool&gt;</c> element.</summary>
	/// <param name="SifRefObject">A SIF_RefObject</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;ManagingSchool&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setManagingSchool</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ManagingSchool</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetManagingSchool( ManagingSchoolSIF_RefObject SifRefObject, string Value ) {
		RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_MANAGINGSCHOOL);
		AddChild( ProgramsDTD.STUDENTPARTICIPATION_MANAGINGSCHOOL, new ManagingSchool( SifRefObject, Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ManagingSchool&gt;</c> element.
	/// </summary>
	/// <value> A ManagingSchool </value>
	/// <remarks>
	/// <para>To remove the <c>ManagingSchool</c>, set <c>ManagingSchool</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public ManagingSchool ManagingSchool
	{
		get
		{
			return (ManagingSchool)GetChild( ProgramsDTD.STUDENTPARTICIPATION_MANAGINGSCHOOL);
		}
		set
		{
			RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_MANAGINGSCHOOL);
			if( value != null)
			{
				AddChild( ProgramsDTD.STUDENTPARTICIPATION_MANAGINGSCHOOL, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StudentSpecialEducationFTE&gt;</c> element.
	/// </summary>
	/// <value> The <c>StudentSpecialEducationFTE</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public decimal? StudentSpecialEducationFTE
	{
		get
		{
			return (decimal?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_STUDENTSPECIALEDUCATIONFTE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_STUDENTSPECIALEDUCATIONFTE, new SifDecimal( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ReferralDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>ReferralDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Date student was referred for evaluation/program participation."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? ReferralDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_REFERRALDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_REFERRALDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ParticipationContact&gt;</c> element.
	/// </summary>
	/// <value> The <c>ParticipationContact</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Primary contact for this record."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string ParticipationContact
	{
		get
		{
			return (string) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PARTICIPATIONCONTACT ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PARTICIPATIONCONTACT, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;ReferralSource&gt;</c> element.</summary>
	/// <param name="Code">A Code</param>
	///<remarks>
	/// <para>This form of <c>setReferralSource</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ReferralSource</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void SetReferralSource( ProgramStatusType Code ) {
		RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_REFERRALSOURCE);
		AddChild( ProgramsDTD.STUDENTPARTICIPATION_REFERRALSOURCE, new ReferralSource( Code ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ReferralSource&gt;</c> element.
	/// </summary>
	/// <value> A ReferralSource </value>
	/// <remarks>
	/// <para>To remove the <c>ReferralSource</c>, set <c>ReferralSource</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public ReferralSource ReferralSource
	{
		get
		{
			return (ReferralSource)GetChild( ProgramsDTD.STUDENTPARTICIPATION_REFERRALSOURCE);
		}
		set
		{
			RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_REFERRALSOURCE);
			if( value != null)
			{
				AddChild( ProgramsDTD.STUDENTPARTICIPATION_REFERRALSOURCE, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;ProgramStatus&gt;</c> element.</summary>
	/// <param name="Code">A Code</param>
	///<remarks>
	/// <para>This form of <c>setProgramStatus</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ProgramStatus</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void SetProgramStatus( AUCodeSets0792IdentificationProcedureType Code ) {
		RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMSTATUS);
		AddChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMSTATUS, new ProgramStatus( Code ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ProgramStatus&gt;</c> element.
	/// </summary>
	/// <value> A ProgramStatus </value>
	/// <remarks>
	/// <para>To remove the <c>ProgramStatus</c>, set <c>ProgramStatus</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public ProgramStatus ProgramStatus
	{
		get
		{
			return (ProgramStatus)GetChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMSTATUS);
		}
		set
		{
			RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMSTATUS);
			if( value != null)
			{
				AddChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMSTATUS, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;GiftedEligibilityCriteria&gt;</c> element.
	/// </summary>
	/// <value> The <c>GiftedEligibilityCriteria</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string GiftedEligibilityCriteria
	{
		get
		{ 
			return GetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_GIFTEDELIGIBILITYCRITERIA );
		}
		set
		{
			SetField( ProgramsDTD.STUDENTPARTICIPATION_GIFTEDELIGIBILITYCRITERIA, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;GiftedEligibilityCriteria&gt;</c> element.
	/// </summary>
	/// <param name="val">A AUCodeSetsYesOrNoCategoryType object</param>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void SetGiftedEligibilityCriteria( AUCodeSetsYesOrNoCategoryType val )
	{
		SetField( ProgramsDTD.STUDENTPARTICIPATION_GIFTEDELIGIBILITYCRITERIA, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EvaluationParentalConsentDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>EvaluationParentalConsentDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? EvaluationParentalConsentDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EVALUATIONPARENTALCONSENTDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EVALUATIONPARENTALCONSENTDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EvaluationDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>EvaluationDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? EvaluationDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EVALUATIONDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EVALUATIONDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EvaluationExtensionDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>EvaluationExtensionDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? EvaluationExtensionDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EVALUATIONEXTENSIONDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EVALUATIONEXTENSIONDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ExtensionComments&gt;</c> element.
	/// </summary>
	/// <value> The <c>ExtensionComments</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string ExtensionComments
	{
		get
		{
			return (string) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EXTENSIONCOMMENTS ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EXTENSIONCOMMENTS, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ReevaluationDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>ReevaluationDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? ReevaluationDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_REEVALUATIONDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_REEVALUATIONDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ProgramEligibilityDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>ProgramEligibilityDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? ProgramEligibilityDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMELIGIBILITYDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMELIGIBILITYDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ProgramPlanDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>ProgramPlanDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? ProgramPlanDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMPLANDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMPLANDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ProgramPlanEffectiveDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>ProgramPlanEffectiveDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? ProgramPlanEffectiveDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMPLANEFFECTIVEDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMPLANEFFECTIVEDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;NOREPDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>NOREPDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? NOREPDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_NOREPDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_NOREPDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;PlacementParentalConsentDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>PlacementParentalConsentDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? PlacementParentalConsentDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PLACEMENTPARENTALCONSENTDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PLACEMENTPARENTALCONSENTDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ProgramPlacementDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>ProgramPlacementDate</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? ProgramPlacementDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMPLACEMENTDATE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMPLACEMENTDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ExtendedSchoolYear&gt;</c> element.
	/// </summary>
	/// <value> The <c>ExtendedSchoolYear</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public bool? ExtendedSchoolYear
	{
		get
		{
			return (bool?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EXTENDEDSCHOOLYEAR ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EXTENDEDSCHOOLYEAR, new SifBoolean( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ExtendedDay&gt;</c> element.
	/// </summary>
	/// <value> The <c>ExtendedDay</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public bool? ExtendedDay
	{
		get
		{
			return (bool?) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EXTENDEDDAY ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_EXTENDEDDAY, new SifBoolean( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;ProgramAvailability&gt;</c> element.</summary>
	/// <param name="Code">A Code</param>
	///<remarks>
	/// <para>This form of <c>setProgramAvailability</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ProgramAvailability</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void SetProgramAvailability( AUCodeSets0211ProgramAvailabilityType Code ) {
		RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMAVAILABILITY);
		AddChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMAVAILABILITY, new ProgramAvailability( Code ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ProgramAvailability&gt;</c> element.
	/// </summary>
	/// <value> A ProgramAvailability </value>
	/// <remarks>
	/// <para>To remove the <c>ProgramAvailability</c>, set <c>ProgramAvailability</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public ProgramAvailability ProgramAvailability
	{
		get
		{
			return (ProgramAvailability)GetChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMAVAILABILITY);
		}
		set
		{
			RemoveChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMAVAILABILITY);
			if( value != null)
			{
				AddChild( ProgramsDTD.STUDENTPARTICIPATION_PROGRAMAVAILABILITY, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EntryPerson&gt;</c> element.
	/// </summary>
	/// <value> The <c>EntryPerson</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string EntryPerson
	{
		get
		{
			return (string) GetSifSimpleFieldValue( ProgramsDTD.STUDENTPARTICIPATION_ENTRYPERSON ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.STUDENTPARTICIPATION_ENTRYPERSON, new SifString( value ), value );
		}
	}

}}
