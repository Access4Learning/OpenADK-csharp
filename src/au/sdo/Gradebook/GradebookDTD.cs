// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using OpenADK.Util;
using OpenADK.Library;
using OpenADK.Library.Impl;
using CommonDTD = OpenADK.Library.au.Common.CommonDTD;
using ReportingDTD = OpenADK.Library.au.Reporting.ReportingDTD;

namespace OpenADK.Library.au.Gradebook
{

// BEGIN FILE... (SIFDTD_Comments_CS.txt)

/// <summary>Metadata for the Schools Interoperability Framework (SIF)</summary>
	/// <remarks>
	/// <para>
	/// SIFDTD defines global {@linkplain com.edustructures.sifworks.ElementDef} 
	/// constants that describe SIF Data Objects, elements, and attributes across all 
	/// supported versions of the SIF Specification. The ADK uses this metadata 
	/// internally to parse and render SIF Data Objects.  In addition, many of the 
	/// framework APIs require that the programmer pass an ElementDef constant from 
	/// the SIFDTD class to identify an object, element, or attribute.
	/// </para>
	/// <para>
	/// ElementDef constants are named <c>[PARENT_]ENTITY</c>, where 
	/// <c>PARENT</c> is the name of the parent element and 
	/// <c>ENTITY</c> is the name of the element or attribute 
	/// encapsulated by the ElementDef. Some examples of ElementDef constants defined
	/// by this class include:
	/// </para>
	/// <list type="table">
	/// <listheader><term>IElementDef</term><description>Description</description></listheader>
	/// <item><term><c>SIFDTD.STUDENTPERSONAL</c></term><description>Identifies the StudentPersonal data object</description></item>
	/// <item><term><c>SIFDTD.SCHOOLINFO</c></term><description>Identifies the SchoolInfo data object</description></item>
	/// </list>
	/// Many of the Adk's public interfaces require an ElementDef constant to be passed
	/// as a parameter. For example, the first parameter to the <see cref="IZone.SetSubscriber"/>
	/// method is an IElementDef:
	/// <code>myZone.setSubscriber( SIFDTD.BUSINFO, this, ADKFlags.PROV_SUBSCRIBE );</code>
	/// ElementDef also identifies child elements and attributes as demonstrated by the	<c>Query.AddCondition</c> method:
	/// <code>
	/// Query query = new Query( SifDtd.STUDENTPERSONAL );
	/// query.AddCondition( SifDtd.STUDENTPERSONAL_REFID, Condition.EQ, "4A37969803F0D00322AF0EB969038483" );
	/// </code>
	/// <para>
	/// <b>SDO Libraries</b>
	/// </para>
	/// <para>
	/// ElementDef metadata is grouped into "SDO Libraries", which are organized along 
	/// SIF Working Group boundaries. SDO Libraries are loaded into the <c>SifDdt</c> 
	/// class when the Adk is initialized. All or part of the metadata is loaded into depending on the flags passed to the
	/// <see cref="Adk.Initialize(SifVersion, SdoLibraryType)"/> method,
	/// metadata from one or more SDO Libraries may be loaded. For example, the following
	/// call loads metadata for the <c>Student Information Working Group Objects</c>  
	/// and <c>Transportation And Geographic Information Working Group Objects</c> 
	/// (Common Elements and <c>Infrastructure Working Group Objects</c> metadata is always loaded
	/// </para>
	/// <code>Adk.Initialize( SiFVersion.LATEST, SdoLibraryType.Student | SdoLibraryType.Trans )</code>
	/// <para>
	/// If an given SDO Library is not loaded, all of the SIFDTD constants that belong
	/// to that library will be <code>null</code> and cannot be referenced. For example,
	/// given the SDO Libraries loaded above, attempting to reference the 
	/// <code>SIFDTD.LIBRARYPATRONSTATUS</code> object from the Library Automation Working
	/// Group would result in a NullPointerException:
	/// </para>
	/// <code>SifDtd.LIBRARYPATRONSTATUS.Name;</code>
	/// </remarks>
	public class GradebookDTD : OpenADK.Library.Impl.SdoLibraryImpl
	{
	/** Defines the &lt;AuditInfo&gt; SIF Data Object */
	public static IElementDef AUDITINFO = null;
	/** Defines the &lt;CreationUser&gt; SIF Data Object */
	public static IElementDef CREATIONUSER = null;
	/** Defines the &lt;StudentPeriodAttendance&gt; SIF Data Object */
	public static IElementDef STUDENTPERIODATTENDANCE = null;


	// Field elements of AUDITINFO (2 fields)
	/** Defines the &lt;CreationUser&gt; element as a child of &lt;AuditInfo&gt; */
	public static IElementDef AUDITINFO_CREATIONUSER = null;
	/** Defines the &lt;CreationDateTime&gt; element as a child of &lt;AuditInfo&gt; */
	public static IElementDef AUDITINFO_CREATIONDATETIME = null;

	// Field elements of CREATIONUSER (2 fields)
	/** Defines the Type attribute as a child of &lt;CreationUser&gt; */
	public static IElementDef CREATIONUSER_TYPE = null;
	/** Defines the &lt;UserId&gt; element as a child of &lt;CreationUser&gt; */
	public static IElementDef CREATIONUSER_USERID = null;

	// Field elements of STUDENTPERIODATTENDANCE (13 fields)
	/** Defines the RefId attribute as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_REFID = null;
	/** Defines the &lt;StudentPersonalRefId&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_STUDENTPERSONALREFID = null;
	/** Defines the &lt;SchoolInfoRefId&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_SCHOOLINFOREFID = null;
	/** Defines the &lt;Date&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_DATE = null;
	/** Defines the &lt;SessionInfoRefId&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_SESSIONINFOREFID = null;
	/** Defines the &lt;TimetablePeriod&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_TIMETABLEPERIOD = null;
	/** Defines the &lt;TimeIn&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_TIMEIN = null;
	/** Defines the &lt;AttendanceCode&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_ATTENDANCECODE = null;
	/** Defines the &lt;AttendanceStatus&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_ATTENDANCESTATUS = null;
	/** Defines the &lt;TimeOut&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_TIMEOUT = null;
	/** Defines the &lt;SchoolYear&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_SCHOOLYEAR = null;
	/** Defines the &lt;AuditInfo&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_AUDITINFO = null;
	/** Defines the &lt;AttendanceComment&gt; element as a child of &lt;StudentPeriodAttendance&gt; */
	public static IElementDef STUDENTPERIODATTENDANCE_ATTENDANCECOMMENT = null;
	/** SIF 1.5 and later: Defines the built-in SIF_ExtendedElements element common to all SIF Data Objects */
	public static IElementDef STUDENTPERIODATTENDANCE_SIF_EXTENDEDELEMENTS = null;
	/** SIF 2.0 and later: Defines the built-in SIF_Metadata element common to all SIF Data Objects */
	public static IElementDef STUDENTPERIODATTENDANCE_SIF_METADATA = null;

	public override void Load()
	{
		//  Objects defined by this SDO Library...

		AUDITINFO = new ElementDefImpl( null, "AuditInfo", null, 0, SifDtd.GRADEBOOK, "au", 0, SifVersion.SIF24, SifVersion.SIF25 );
		CREATIONUSER = new ElementDefImpl( null, "CreationUser", null, 0, SifDtd.GRADEBOOK, "au", 0, SifVersion.SIF24, SifVersion.SIF25 );
		STUDENTPERIODATTENDANCE = new ElementDefImpl( null, "StudentPeriodAttendance", null, 0, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_OBJECT), SifVersion.SIF23, SifVersion.SIF25 );


		// <AuditInfo> fields (2 entries)
		GradebookDTD.AUDITINFO_CREATIONUSER = new ElementDefImpl( AUDITINFO, "CreationUser", null, 1, SifDtd.GRADEBOOK, "au", 0, SifVersion.SIF24, SifVersion.SIF25 );
		GradebookDTD.AUDITINFO_CREATIONDATETIME = new ElementDefImpl( AUDITINFO, "CreationDateTime", null, 2, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF24, SifVersion.SIF25, SifTypeConverters.DATETIME );

		// <CreationUser> fields (2 entries)
		GradebookDTD.CREATIONUSER_TYPE = new ElementDefImpl( CREATIONUSER, "Type", null, 1, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_ATTRIBUTE), SifVersion.SIF24, SifVersion.SIF25, SifTypeConverters.STRING );
		GradebookDTD.CREATIONUSER_USERID = new ElementDefImpl( CREATIONUSER, "UserId", null, 2, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF24, SifVersion.SIF25, SifTypeConverters.STRING );

		// <StudentPeriodAttendance> fields (13 entries)
		GradebookDTD.STUDENTPERIODATTENDANCE_REFID = new ElementDefImpl( STUDENTPERIODATTENDANCE, "RefId", null, 1, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_ATTRIBUTE), SifVersion.SIF23, SifVersion.SIF25, SifTypeConverters.STRING );
		GradebookDTD.STUDENTPERIODATTENDANCE_STUDENTPERSONALREFID = new ElementDefImpl( STUDENTPERIODATTENDANCE, "StudentPersonalRefId", null, 2, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF23, SifVersion.SIF25, SifTypeConverters.STRING );
		GradebookDTD.STUDENTPERIODATTENDANCE_SCHOOLINFOREFID = new ElementDefImpl( STUDENTPERIODATTENDANCE, "SchoolInfoRefId", null, 3, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF23, SifVersion.SIF25, SifTypeConverters.STRING );
		GradebookDTD.STUDENTPERIODATTENDANCE_DATE = new ElementDefImpl( STUDENTPERIODATTENDANCE, "Date", null, 4, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF23, SifVersion.SIF25, SifTypeConverters.DATE );
		GradebookDTD.STUDENTPERIODATTENDANCE_SESSIONINFOREFID = new ElementDefImpl( STUDENTPERIODATTENDANCE, "SessionInfoRefId", null, 5, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF23, SifVersion.SIF25, SifTypeConverters.STRING );
		GradebookDTD.STUDENTPERIODATTENDANCE_TIMETABLEPERIOD = new ElementDefImpl( STUDENTPERIODATTENDANCE, "TimetablePeriod", null, 6, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF23, SifVersion.SIF25, SifTypeConverters.STRING );
		GradebookDTD.STUDENTPERIODATTENDANCE_TIMEIN = new ElementDefImpl( STUDENTPERIODATTENDANCE, "TimeIn", null, 7, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF24, SifVersion.SIF25, SifTypeConverters.TIME );
		GradebookDTD.STUDENTPERIODATTENDANCE_ATTENDANCECODE = new ElementDefImpl( STUDENTPERIODATTENDANCE, "AttendanceCode", null, 7, SifDtd.COMMON, "au", 0, SifVersion.SIF23, SifVersion.SIF25 );
		GradebookDTD.STUDENTPERIODATTENDANCE_ATTENDANCECODE.DefineVersionInfo(SifVersion.SIF24, "AttendanceCode", 9, 0); // (Sif 24 alias)
		GradebookDTD.STUDENTPERIODATTENDANCE_ATTENDANCESTATUS = new ElementDefImpl( STUDENTPERIODATTENDANCE, "AttendanceStatus", null, 8, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF23, SifVersion.SIF25, SifTypeConverters.STRING );
		GradebookDTD.STUDENTPERIODATTENDANCE_ATTENDANCESTATUS.DefineVersionInfo(SifVersion.SIF24, "AttendanceStatus", 10, (ElementDefImpl.FD_FIELD)); // (Sif 24 alias)
		GradebookDTD.STUDENTPERIODATTENDANCE_TIMEOUT = new ElementDefImpl( STUDENTPERIODATTENDANCE, "TimeOut", null, 8, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF24, SifVersion.SIF25, SifTypeConverters.TIME );
		GradebookDTD.STUDENTPERIODATTENDANCE_SCHOOLYEAR = new ElementDefImpl( STUDENTPERIODATTENDANCE, "SchoolYear", null, 9, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF23, SifVersion.SIF25, SifTypeConverters.INT );
		GradebookDTD.STUDENTPERIODATTENDANCE_SCHOOLYEAR.DefineVersionInfo(SifVersion.SIF24, "SchoolYear", 11, (ElementDefImpl.FD_FIELD)); // (Sif 24 alias)
		GradebookDTD.STUDENTPERIODATTENDANCE_AUDITINFO = new ElementDefImpl( STUDENTPERIODATTENDANCE, "AuditInfo", null, 12, SifDtd.GRADEBOOK, "au", 0, SifVersion.SIF24, SifVersion.SIF25 );
		GradebookDTD.STUDENTPERIODATTENDANCE_ATTENDANCECOMMENT = new ElementDefImpl( STUDENTPERIODATTENDANCE, "AttendanceComment", null, 13, SifDtd.GRADEBOOK, "au", (ElementDefImpl.FD_FIELD), SifVersion.SIF24, SifVersion.SIF25, SifTypeConverters.STRING );
		STUDENTPERIODATTENDANCE_SIF_EXTENDEDELEMENTS = new ElementDefImpl( STUDENTPERIODATTENDANCE, "SIF_ExtendedElements", null, 127, SifDtd.GLOBAL, null, (0), SifVersion.SIF15r1, SifVersion.SIF25 );
		STUDENTPERIODATTENDANCE_SIF_METADATA = new ElementDefImpl( STUDENTPERIODATTENDANCE, "SIF_Metadata", null, 128, SifDtd.DATAMODEL, "au", (0), SifVersion.SIF20, SifVersion.SIF25 );
	}

	#region Update SifDtd
	public override void AddElementMappings( IDictionary<String, IElementDef> dictionary )
	{
		dictionary[ "CreationUser" ] = CREATIONUSER;
		dictionary["CreationUser_Type"] = GradebookDTD.CREATIONUSER_TYPE ;
		dictionary["CreationUser_UserId"] = GradebookDTD.CREATIONUSER_USERID ;
		dictionary[ "AuditInfo" ] = AUDITINFO;
		dictionary["AuditInfo_CreationDateTime"] = GradebookDTD.AUDITINFO_CREATIONDATETIME ;
		dictionary["AuditInfo_CreationUser"] = GradebookDTD.AUDITINFO_CREATIONUSER ;
		dictionary[ "StudentPeriodAttendance" ] = STUDENTPERIODATTENDANCE;
		dictionary[ "StudentPeriodAttendance_SIF_ExtendedElements" ] = STUDENTPERIODATTENDANCE_SIF_EXTENDEDELEMENTS ;
		dictionary[ "StudentPeriodAttendance_SIF_Metadata" ] = STUDENTPERIODATTENDANCE_SIF_METADATA;
		dictionary["StudentPeriodAttendance_AttendanceCode"] = GradebookDTD.STUDENTPERIODATTENDANCE_ATTENDANCECODE ;
		dictionary["StudentPeriodAttendance_AttendanceComment"] = GradebookDTD.STUDENTPERIODATTENDANCE_ATTENDANCECOMMENT ;
		dictionary["StudentPeriodAttendance_AttendanceStatus"] = GradebookDTD.STUDENTPERIODATTENDANCE_ATTENDANCESTATUS ;
		dictionary["StudentPeriodAttendance_AuditInfo"] = GradebookDTD.STUDENTPERIODATTENDANCE_AUDITINFO ;
		dictionary["StudentPeriodAttendance_Date"] = GradebookDTD.STUDENTPERIODATTENDANCE_DATE ;
		dictionary["StudentPeriodAttendance_RefId"] = GradebookDTD.STUDENTPERIODATTENDANCE_REFID ;
		dictionary["StudentPeriodAttendance_SchoolInfoRefId"] = GradebookDTD.STUDENTPERIODATTENDANCE_SCHOOLINFOREFID ;
		dictionary["StudentPeriodAttendance_SchoolYear"] = GradebookDTD.STUDENTPERIODATTENDANCE_SCHOOLYEAR ;
		dictionary["StudentPeriodAttendance_SessionInfoRefId"] = GradebookDTD.STUDENTPERIODATTENDANCE_SESSIONINFOREFID ;
		dictionary["StudentPeriodAttendance_StudentPersonalRefId"] = GradebookDTD.STUDENTPERIODATTENDANCE_STUDENTPERSONALREFID ;
		dictionary["StudentPeriodAttendance_TimeIn"] = GradebookDTD.STUDENTPERIODATTENDANCE_TIMEIN ;
		dictionary["StudentPeriodAttendance_TimeOut"] = GradebookDTD.STUDENTPERIODATTENDANCE_TIMEOUT ;
		dictionary["StudentPeriodAttendance_TimetablePeriod"] = GradebookDTD.STUDENTPERIODATTENDANCE_TIMETABLEPERIOD ;
	}
	#endregion
}}