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

namespace OpenADK.Library.uk.Learner{

/// <summary>A LearnerAttendance</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.1</para>
/// </remarks>
[Serializable]
public class LearnerAttendance : SifDataObject
{
	/// <summary>
	/// Creates an instance of a LearnerAttendance
	/// </summary>
	public LearnerAttendance() : base( Adk.SifVersion, LearnerDTD.LEARNERATTENDANCE ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">The ID (GUID) assigned to uniquely identify this attendance record.</param>
	///<param name="learnerPersonalRefId">The ID (GUID) of the learner for which this attendance mark is recorded.</param>
	///<param name="schoolInfoRefId">The school where the attendance mark was taken.</param>
	///<param name="attendanceDate">The date of the attendance period.</param>
	///<param name="attendanceDomain">The attendance type.</param>
	///<param name="attendanceCode">The attendance mark/code.</param>
	///<param name="inputSource">The source of the attendance record data.  Could be Workforce member, biometric device, or other.</param>
	///
	public LearnerAttendance( string refId, string learnerPersonalRefId, string schoolInfoRefId, DateTime? attendanceDate, AttendanceDomain attendanceDomain, AttendanceCodeType attendanceCode, AttendanceSourceType inputSource ) : base( Adk.SifVersion, LearnerDTD.LEARNERATTENDANCE )
	{
		this.RefId = refId;
		this.LearnerPersonalRefId = learnerPersonalRefId;
		this.SchoolInfoRefId = schoolInfoRefId;
		this.AttendanceDate = attendanceDate;
		this.SetAttendanceDomain( attendanceDomain );
		this.SetAttendanceCode( attendanceCode );
		this.SetInputSource( inputSource );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected LearnerAttendance( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { LearnerDTD.LEARNERATTENDANCE_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The ID (GUID) assigned to uniquely identify this attendance record."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_REFID ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>LearnerPersonalRefId</c> attribute.
	/// </summary>
	/// <value> The <c>LearnerPersonalRefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The ID (GUID) of the learner for which this attendance mark is recorded."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string LearnerPersonalRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_LEARNERPERSONALREFID ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_LEARNERPERSONALREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>SchoolInfoRefId</c> attribute.
	/// </summary>
	/// <value> The <c>SchoolInfoRefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The school where the attendance mark was taken."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string SchoolInfoRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_SCHOOLINFOREFID ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_SCHOOLINFOREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AttendanceDate&gt;</c> element.
	/// </summary>
	/// <value> The <c>AttendanceDate</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The date of the attendance period."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public DateTime? AttendanceDate
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_ATTENDANCEDATE ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_ATTENDANCEDATE, new SifDate( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;TimeIn&gt;</c> element.
	/// </summary>
	/// <value> The <c>TimeIn</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The time when the learner began the attendance period."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public DateTime? TimeIn
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_TIMEIN ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_TIMEIN, new SifTime( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StartTime&gt;</c> element.
	/// </summary>
	/// <value> The <c>StartTime</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The time the attendance period started. StartTime is required when this object represents a lesson"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public DateTime? StartTime
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_STARTTIME ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_STARTTIME, new SifTime( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;FinishTime&gt;</c> element.
	/// </summary>
	/// <value> The <c>FinishTime</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The time the attendance period finished."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public DateTime? FinishTime
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_FINISHTIME ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_FINISHTIME, new SifTime( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Session&gt;</c> element.
	/// </summary>
	/// <value> The <c>Session</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The session within which this attendance mark applies (e.g. AM/PM).  Session is required when StartTime is not specified and the object refers to a session attendance mark (i.e. AttendanceDomain is session)."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string Session
	{
		get
		{ 
			return GetFieldValue( LearnerDTD.LEARNERATTENDANCE_SESSION );
		}
		set
		{
			SetField( LearnerDTD.LEARNERATTENDANCE_SESSION, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;Session&gt;</c> element.
	/// </summary>
	/// <param name="val">A AttendanceSession object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The session within which this attendance mark applies (e.g. AM/PM).  Session is required when StartTime is not specified and the object refers to a session attendance mark (i.e. AttendanceDomain is session)."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetSession( AttendanceSession val )
	{
		SetField( LearnerDTD.LEARNERATTENDANCE_SESSION, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SessionLabel&gt;</c> element.
	/// </summary>
	/// <value> The <c>SessionLabel</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "A label associated with the session to further describe the session, when applicable."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string SessionLabel
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_SESSIONLABEL ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_SESSIONLABEL, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AttendanceDomain&gt;</c> element.
	/// </summary>
	/// <value> The <c>AttendanceDomain</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The attendance type."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string AttendanceDomain
	{
		get
		{ 
			return GetFieldValue( LearnerDTD.LEARNERATTENDANCE_ATTENDANCEDOMAIN );
		}
		set
		{
			SetField( LearnerDTD.LEARNERATTENDANCE_ATTENDANCEDOMAIN, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;AttendanceDomain&gt;</c> element.
	/// </summary>
	/// <param name="val">A AttendanceDomain object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The attendance type."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetAttendanceDomain( AttendanceDomain val )
	{
		SetField( LearnerDTD.LEARNERATTENDANCE_ATTENDANCEDOMAIN, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AttendanceCode&gt;</c> element.
	/// </summary>
	/// <value> The <c>AttendanceCode</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The attendance mark/code."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string AttendanceCode
	{
		get
		{ 
			return GetFieldValue( LearnerDTD.LEARNERATTENDANCE_ATTENDANCECODE );
		}
		set
		{
			SetField( LearnerDTD.LEARNERATTENDANCE_ATTENDANCECODE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;AttendanceCode&gt;</c> element.
	/// </summary>
	/// <param name="val">A AttendanceCodeType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The attendance mark/code."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetAttendanceCode( AttendanceCodeType val )
	{
		SetField( LearnerDTD.LEARNERATTENDANCE_ATTENDANCECODE, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AttendanceNote&gt;</c> element.
	/// </summary>
	/// <value> The <c>AttendanceNote</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Additional comments. Required when record is specified as a correction."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string AttendanceNote
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_ATTENDANCENOTE ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_ATTENDANCENOTE, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;RecordTimestamp&gt;</c> element.
	/// </summary>
	/// <value> The <c>RecordTimestamp</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The date/time when recorded. When more than one record exists using the same RefId, the record with the latest timestamp is assumed to be the most current. This is possible when a record is republished due to corrections."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public DateTime? RecordTimestamp
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_RECORDTIMESTAMP ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_RECORDTIMESTAMP, new SifDateTime( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;InputSource&gt;</c> element.
	/// </summary>
	/// <value> The <c>InputSource</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The source of the attendance record data.  Could be Workforce member, biometric device, or other."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string InputSource
	{
		get
		{ 
			return GetFieldValue( LearnerDTD.LEARNERATTENDANCE_INPUTSOURCE );
		}
		set
		{
			SetField( LearnerDTD.LEARNERATTENDANCE_INPUTSOURCE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;InputSource&gt;</c> element.
	/// </summary>
	/// <param name="val">A AttendanceSourceType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The source of the attendance record data.  Could be Workforce member, biometric device, or other."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public void SetInputSource( AttendanceSourceType val )
	{
		SetField( LearnerDTD.LEARNERATTENDANCE_INPUTSOURCE, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;WorkforcePersonalRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>WorkforcePersonalRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The ID (GUID) of the staff member responsible for recording this attendance information."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string WorkforcePersonalRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_WORKFORCEPERSONALREFID ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_WORKFORCEPERSONALREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SchoolGroupRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>SchoolGroupRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The ID (GUID) of the group that this attendance applies to. Useful when the attendance period corresponds to a lesson time."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string SchoolGroupRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.LEARNERATTENDANCE_SCHOOLGROUPREFID ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.LEARNERATTENDANCE_SCHOOLGROUPREFID, new SifString( value ), value );
		}
	}

}}
