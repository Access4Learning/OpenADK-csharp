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

/// <summary>Characteristics of the student identified by the district which the state uses to perform its locator matching logic.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.4</para>
/// </remarks>
[Serializable]
public class StudentParameters : SifDataObject
{
	/// <summary>
	/// Creates an instance of a StudentParameters
	/// </summary>
	public StudentParameters() : base( Adk.SifVersion, CommonDTD.STUDENTPARAMETERS ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected StudentParameters( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;StateProvinceId&gt;</c> element.
	/// </summary>
	/// <value> The <c>StateProvinceId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Specified if the requesting agency believes it knows the person’s state Id."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string StateProvinceId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_STATEPROVINCEID ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_STATEPROVINCEID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;LocalId&gt;</c> element.
	/// </summary>
	/// <value> The <c>LocalId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This is the requesting agent’s unique Id for the person."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string LocalId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_LOCALID ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_LOCALID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_RefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_RefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Requesting agency’s local zone’s GUID of the student's data source object."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string SIF_RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_SIF_REFID ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_SIF_REFID, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;GradeLevel&gt;</c> element.</summary>
	/// <param name="Code">Code representing the grade level.</param>
	///<remarks>
	/// <para>This form of <c>setGradeLevel</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>GradeLevel</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void SetGradeLevel( GradeLevelCode Code ) {
		RemoveChild( CommonDTD.STUDENTPARAMETERS_GRADELEVEL);
		AddChild( CommonDTD.STUDENTPARAMETERS_GRADELEVEL, new GradeLevel( Code ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;GradeLevel&gt;</c> element.
	/// </summary>
	/// <value> A GradeLevel </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "The student’s current grade level."</para>
	/// <para>To remove the <c>GradeLevel</c>, set <c>GradeLevel</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public GradeLevel GradeLevel
	{
		get
		{
			return (GradeLevel)GetChild( CommonDTD.STUDENTPARAMETERS_GRADELEVEL);
		}
		set
		{
			RemoveChild( CommonDTD.STUDENTPARAMETERS_GRADELEVEL);
			if( value != null)
			{
				AddChild( CommonDTD.STUDENTPARAMETERS_GRADELEVEL, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SSN&gt;</c> element.
	/// </summary>
	/// <value> The <c>SSN</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Social security number of the person."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string SSN
	{
		get
		{
			return (string) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_SSN ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_SSN, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Names&gt;</c> element.
	/// </summary>
	/// <value> A Names </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "The name(s) of the person."</para>
	/// <para>To remove the <c>Names</c>, set <c>Names</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public Names Names
	{
		get
		{
			return (Names)GetChild( CommonDTD.STUDENTPARAMETERS_NAMES);
		}
		set
		{
			RemoveChild( CommonDTD.STUDENTPARAMETERS_NAMES);
			if( value != null)
			{
				AddChild( CommonDTD.STUDENTPARAMETERS_NAMES, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Addresses&gt;</c> element.
	/// </summary>
	/// <value> An AddressList </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "The address(es) of the person."</para>
	/// <para>To remove the <c>AddressList</c>, set <c>Addresses</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public AddressList Addresses
	{
		get
		{
			return (AddressList)GetChild( CommonDTD.STUDENTPARAMETERS_ADDRESSES);
		}
		set
		{
			RemoveChild( CommonDTD.STUDENTPARAMETERS_ADDRESSES);
			if( value != null)
			{
				AddChild( CommonDTD.STUDENTPARAMETERS_ADDRESSES, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Demographics&gt;</c> element.
	/// </summary>
	/// <value> A Demographics </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Demographic information about the person. This will typically be the core matching information. Privacy considerations may mean that it is filled differently here than it might be in a local object, but it must include enough to drive the state's matching algorithms."</para>
	/// <para>To remove the <c>Demographics</c>, set <c>Demographics</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public Demographics Demographics
	{
		get
		{
			return (Demographics)GetChild( CommonDTD.STUDENTPARAMETERS_DEMOGRAPHICS);
		}
		set
		{
			RemoveChild( CommonDTD.STUDENTPARAMETERS_DEMOGRAPHICS);
			if( value != null)
			{
				AddChild( CommonDTD.STUDENTPARAMETERS_DEMOGRAPHICS, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Contacts&gt;</c> element.
	/// </summary>
	/// <value> A StudentParametersContacts </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Typically used to provide the state information about a person's parents and legal guardians."</para>
	/// <para>To remove the <c>StudentParametersContacts</c>, set <c>Contacts</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public StudentParametersContacts Contacts
	{
		get
		{
			return (StudentParametersContacts)GetChild( CommonDTD.STUDENTPARAMETERS_CONTACTS);
		}
		set
		{
			RemoveChild( CommonDTD.STUDENTPARAMETERS_CONTACTS);
			if( value != null)
			{
				AddChild( CommonDTD.STUDENTPARAMETERS_CONTACTS, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EffectiveDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>EffectiveDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "This should match StudentSchoolEnrollment/EntryDate."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? EffectiveDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_EFFECTIVEDATE ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_EFFECTIVEDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StartDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>StartDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Start date for a range that is being requested. In most cases this will correspond to the school entry date."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? StartDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_STARTDATE ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_STARTDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EndDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>EndDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "End date for a range that is being requested. In most cases this will correspond to the school exit date."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public DateTime? EndDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_ENDDATE ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_ENDDATE, new SifDate( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;GraduationDate&gt;</c> element.</summary>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;PartialDateType&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setGraduationDate</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>GraduationDate</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void SetGraduationDate( string Value ) {
		RemoveChild( CommonDTD.STUDENTPARAMETERS_GRADUATIONDATE);
		AddChild( CommonDTD.STUDENTPARAMETERS_GRADUATIONDATE, new PartialDateType( Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;GraduationDate&gt;</c> element.
	/// </summary>
	/// <value> A PartialDateType </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "ndicates the date the person's graduated Year, Year and Month, or Year, Month and Day may be specified. Intended to facilitate locating persons that may not be currently enrolled in a school or district, particularly in the context of locating student identifiers for initiating a request for a student's academic record. Supplied date values may often be approximations made by the entity endeavoring to request a student's academic record."</para>
	/// <para>To remove the <c>PartialDateType</c>, set <c>GraduationDate</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public PartialDateType GraduationDate
	{
		get
		{
			return (PartialDateType)GetChild( CommonDTD.STUDENTPARAMETERS_GRADUATIONDATE);
		}
		set
		{
			RemoveChild( CommonDTD.STUDENTPARAMETERS_GRADUATIONDATE);
			if( value != null)
			{
				AddChild( CommonDTD.STUDENTPARAMETERS_GRADUATIONDATE, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SchoolAttendedName&gt;</c> element.
	/// </summary>
	/// <value> The <c>SchoolAttendedName</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The name of the school last attended by the person."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string SchoolAttendedName
	{
		get
		{
			return (string) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_SCHOOLATTENDEDNAME ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_SCHOOLATTENDEDNAME, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SchoolAttendedLocation&gt;</c> element.
	/// </summary>
	/// <value> The <c>SchoolAttendedLocation</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The location of the school last attended by the person (e.g. city or county name, district name, etc.)."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string SchoolAttendedLocation
	{
		get
		{
			return (string) GetSifSimpleFieldValue( CommonDTD.STUDENTPARAMETERS_SCHOOLATTENDEDLOCATION ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.STUDENTPARAMETERS_SCHOOLATTENDEDLOCATION, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;OtherIdList&gt;</c> element.
	/// </summary>
	/// <value> An OtherIdList </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Additional person identifiers not represented elsewhere in Characteristics (e.g. the driver's license number of the person )."</para>
	/// <para>To remove the <c>OtherIdList</c>, set <c>OtherIdList</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public OtherIdList OtherIdList
	{
		get
		{
			return (OtherIdList)GetChild( CommonDTD.STUDENTPARAMETERS_OTHERIDLIST);
		}
		set
		{
			RemoveChild( CommonDTD.STUDENTPARAMETERS_OTHERIDLIST);
			if( value != null)
			{
				AddChild( CommonDTD.STUDENTPARAMETERS_OTHERIDLIST, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;TimeElement&gt;</c> element.</summary>
	/// <param name="Type">A Type</param>
	/// <param name="Code">This element provides a place for the application to send structured data (code values, unique identifier, timestamps). This code value can, depending upon the use case agreement between agents, be used to qualify the data in the Value element.</param>
	/// <param name="Name">Contains a human-readable description of the value in Value.</param>
	/// <param name="Value">Contains the human-readable value.</param>
	///<remarks>
	/// <para>This form of <c>setTimeElement</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>TimeElement</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void SetTimeElement( TimeElementType Type, string Code, string Name, string Value ) {
		RemoveChild( CommonDTD.STUDENTPARAMETERS_TIMEELEMENT);
		AddChild( CommonDTD.STUDENTPARAMETERS_TIMEELEMENT, new TimeElement( Type, Code, Name, Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;TimeElement&gt;</c> element.
	/// </summary>
	/// <value> A TimeElement </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "SIF_Metadata’s TimeElement may be specified to indicate a time duration to which the characteristics apply (as opposed to adding new elements to Characteristics). This metadata may be supplied by systems that know a student existed in a district during a certain time period (e.g. a system initiating a student record exchange). If the responder is able to make use of the metadata, it may do so to narrow down the student look-up; otherwise the metadata can be ignored or logged."</para>
	/// <para>To remove the <c>TimeElement</c>, set <c>TimeElement</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public TimeElement TimeElement
	{
		get
		{
			return (TimeElement)GetChild( CommonDTD.STUDENTPARAMETERS_TIMEELEMENT);
		}
		set
		{
			RemoveChild( CommonDTD.STUDENTPARAMETERS_TIMEELEMENT);
			if( value != null)
			{
				AddChild( CommonDTD.STUDENTPARAMETERS_TIMEELEMENT, value );
			}
		}
	}

}}
