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

namespace OpenADK.Library.us.Hrfin{

/// <summary>This object contains information about the time worked by an employee on a specific job.  SIF_Events are reported.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class TimeWorked : SifDataObject
{
	/// <summary>
	/// Creates an instance of a TimeWorked
	/// </summary>
	public TimeWorked() : base( Adk.SifVersion, HrfinDTD.TIMEWORKED ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">GUID for this object. The application that owns this object is responsible for generating this unique ID.</param>
	///<param name="employeePersonalRefId">References associated EmployeePersonal object.</param>
	///<param name="locationInfoRefId">Site where employee actually worked.  References LocationInfo object.</param>
	///<param name="payPeriod">Pay period hours were worked in.</param>
	///
	public TimeWorked( string refId, string employeePersonalRefId, string locationInfoRefId, string payPeriod ) : base( Adk.SifVersion, HrfinDTD.TIMEWORKED )
	{
		this.RefId = refId;
		this.EmployeePersonalRefId = employeePersonalRefId;
		this.LocationInfoRefId = locationInfoRefId;
		this.PayPeriod = payPeriod;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected TimeWorked( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { HrfinDTD.TIMEWORKED_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "GUID for this object. The application that owns this object is responsible for generating this unique ID."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.TIMEWORKED_REFID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.TIMEWORKED_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;EmployeePersonalRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>EmployeePersonalRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "References associated EmployeePersonal object."</para>
	/// <para>This element is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering elements of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;EmployeePersonalRefId&gt;</description></item>
	/// </list>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string EmployeePersonalRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.TIMEWORKED_EMPLOYEEPERSONALREFID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.TIMEWORKED_EMPLOYEEPERSONALREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;LocationInfoRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>LocationInfoRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Site where employee actually worked.  References LocationInfo object."</para>
	/// <para>This element is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering elements of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;LocationInfoRefId&gt;</description></item>
	/// </list>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string LocationInfoRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.TIMEWORKED_LOCATIONINFOREFID ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.TIMEWORKED_LOCATIONINFOREFID, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;JobFunction&gt;</c> element.</summary>
	/// <param name="Code">Code representing the type of job function.</param>
	///<remarks>
	/// <para>This form of <c>setJobFunction</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>JobFunction</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetJobFunction( JobFunctionCode Code ) {
		RemoveChild( HrfinDTD.TIMEWORKED_JOBFUNCTION);
		AddChild( HrfinDTD.TIMEWORKED_JOBFUNCTION, new JobFunction( Code ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;JobFunction&gt;</c> element.
	/// </summary>
	/// <value> A JobFunction </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "The purpose of the activities as related to students."</para>
	/// <para>To remove the <c>JobFunction</c>, set <c>JobFunction</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public JobFunction JobFunction
	{
		get
		{
			return (JobFunction)GetChild( HrfinDTD.TIMEWORKED_JOBFUNCTION);
		}
		set
		{
			RemoveChild( HrfinDTD.TIMEWORKED_JOBFUNCTION);
			if( value != null)
			{
				AddChild( HrfinDTD.TIMEWORKED_JOBFUNCTION, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;RegularHours&gt;</c> element.
	/// </summary>
	/// <value> The <c>RegularHours</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Regular hours worked.  Enter 0 if no hours worked."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public decimal? RegularHours
	{
		get
		{
			return (decimal?) GetSifSimpleFieldValue( HrfinDTD.TIMEWORKED_REGULARHOURS ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.TIMEWORKED_REGULARHOURS, new SifDecimal( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;OvertimeHours&gt;</c> element.
	/// </summary>
	/// <value> The <c>OvertimeHours</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Overtime hours worked.  Enter 0 if no overtime hours worked."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public decimal? OvertimeHours
	{
		get
		{
			return (decimal?) GetSifSimpleFieldValue( HrfinDTD.TIMEWORKED_OVERTIMEHOURS ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.TIMEWORKED_OVERTIMEHOURS, new SifDecimal( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;PayPeriod&gt;</c> element.
	/// </summary>
	/// <value> The <c>PayPeriod</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Pay period hours were worked in."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string PayPeriod
	{
		get
		{
			return (string) GetSifSimpleFieldValue( HrfinDTD.TIMEWORKED_PAYPERIOD ) ;
		}
		set
		{
			SetFieldValue( HrfinDTD.TIMEWORKED_PAYPERIOD, new SifString( value ), value );
		}
	}

}}
