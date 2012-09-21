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
using CommonDTD = OpenADK.Library.uk.Common.CommonDTD;
using ReportingDTD = OpenADK.Library.uk.Reporting.ReportingDTD;

namespace OpenADK.Library.uk.Workforce
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
	public class WorkforceDTD : OpenADK.Library.Impl.SdoLibraryImpl
	{
	/** Defines the &lt;CurrentAssignment&gt; SIF Data Object */
	public static IElementDef CURRENTASSIGNMENT = null;
	/** Defines the &lt;CurrentAssignmentList&gt; SIF Data Object */
	public static IElementDef CURRENTASSIGNMENTLIST = null;
	/** Defines the &lt;Post&gt; SIF Data Object */
	public static IElementDef POST = null;
	/** Defines the &lt;Posts&gt; SIF Data Object */
	public static IElementDef POSTS = null;
	/** Defines the &lt;WorkforcePersonal&gt; SIF Data Object */
	public static IElementDef WORKFORCEPERSONAL = null;


	// Field elements of CURRENTASSIGNMENT (3 fields)
	/** Defines the &lt;LAId&gt; element as a child of &lt;CurrentAssignment&gt; */
	public static IElementDef CURRENTASSIGNMENT_LAID = null;
	/** Defines the &lt;EstablishmentId&gt; element as a child of &lt;CurrentAssignment&gt; */
	public static IElementDef CURRENTASSIGNMENT_ESTABLISHMENTID = null;
	/** Defines the &lt;Posts&gt; element as a child of &lt;CurrentAssignment&gt; */
	public static IElementDef CURRENTASSIGNMENT_POSTS = null;

	// Field elements of CURRENTASSIGNMENTLIST (1 fields)
	/** Defines the &lt;CurrentAssignment&gt; element as a child of &lt;CurrentAssignmentList&gt; */
	public static IElementDef CURRENTASSIGNMENTLIST_CURRENTASSIGNMENT = null;

	// Field elements of POST (0 fields)

	// Field elements of POSTS (1 fields)
	/** Defines the &lt;Post&gt; element as a child of &lt;Posts&gt; */
	public static IElementDef POSTS_POST = null;

	// Field elements of WORKFORCEPERSONAL (8 fields)
	/** Defines the RefId attribute as a child of &lt;WorkforcePersonal&gt; */
	public static IElementDef WORKFORCEPERSONAL_REFID = null;
	/** Defines the &lt;LocalId&gt; element as a child of &lt;WorkforcePersonal&gt; */
	public static IElementDef WORKFORCEPERSONAL_LOCALID = null;
	/** Defines the &lt;AlertMsgList&gt; element as a child of &lt;WorkforcePersonal&gt; */
	public static IElementDef WORKFORCEPERSONAL_ALERTMSGLIST = null;
	/** Defines the &lt;MedicalAlertMsgList&gt; element as a child of &lt;WorkforcePersonal&gt; */
	public static IElementDef WORKFORCEPERSONAL_MEDICALALERTMSGLIST = null;
	/** Defines the &lt;PersonalInformation&gt; element as a child of &lt;WorkforcePersonal&gt; */
	public static IElementDef WORKFORCEPERSONAL_PERSONALINFORMATION = null;
	/** Defines the &lt;TeacherNumber&gt; element as a child of &lt;WorkforcePersonal&gt; */
	public static IElementDef WORKFORCEPERSONAL_TEACHERNUMBER = null;
	/** Defines the &lt;NINumber&gt; element as a child of &lt;WorkforcePersonal&gt; */
	public static IElementDef WORKFORCEPERSONAL_NINUMBER = null;
	/** Defines the &lt;CurrentAssignmentList&gt; element as a child of &lt;WorkforcePersonal&gt; */
	public static IElementDef WORKFORCEPERSONAL_CURRENTASSIGNMENTLIST = null;
	/** SIF 1.5 and later: Defines the built-in SIF_ExtendedElements element common to all SIF Data Objects */
	public static IElementDef WORKFORCEPERSONAL_SIF_EXTENDEDELEMENTS = null;
	/** SIF 2.0 and later: Defines the built-in SIF_Metadata element common to all SIF Data Objects */
	public static IElementDef WORKFORCEPERSONAL_SIF_METADATA = null;

	public override void Load()
	{
		//  Objects defined by this SDO Library...

		CURRENTASSIGNMENT = new ElementDefImpl( null, "CurrentAssignment", null, 0, SifDtd.WORKFORCE, "uk", 0, SifVersion.SIF20, SifVersion.SIF25 );
		CURRENTASSIGNMENTLIST = new ElementDefImpl( null, "CurrentAssignmentList", null, 0, SifDtd.WORKFORCE, "uk", 0, SifVersion.SIF20, SifVersion.SIF25 );
		POST = new ElementDefImpl( null, "Post", null, 0, SifDtd.WORKFORCE, "uk", 0, SifVersion.SIF20, SifVersion.SIF25, SifTypeConverters.STRING );
		POSTS = new ElementDefImpl( null, "Posts", null, 0, SifDtd.WORKFORCE, "uk", 0, SifVersion.SIF20, SifVersion.SIF25 );
		WORKFORCEPERSONAL = new ElementDefImpl( null, "WorkforcePersonal", null, 0, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_OBJECT), SifVersion.SIF20, SifVersion.SIF25 );


		// <CurrentAssignment> fields (3 entries)
		WorkforceDTD.CURRENTASSIGNMENT_LAID = new ElementDefImpl( CURRENTASSIGNMENT, "LAId", null, 1, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_FIELD), SifVersion.SIF20, SifVersion.SIF25, SifTypeConverters.STRING );
		WorkforceDTD.CURRENTASSIGNMENT_ESTABLISHMENTID = new ElementDefImpl( CURRENTASSIGNMENT, "EstablishmentId", null, 2, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_FIELD), SifVersion.SIF20, SifVersion.SIF25, SifTypeConverters.STRING );
		WorkforceDTD.CURRENTASSIGNMENT_POSTS = new ElementDefImpl( CURRENTASSIGNMENT, "Posts", null, 3, SifDtd.WORKFORCE, "uk", 0, SifVersion.SIF20, SifVersion.SIF25 );

		// <CurrentAssignmentList> fields (1 entries)
		WorkforceDTD.CURRENTASSIGNMENTLIST_CURRENTASSIGNMENT = new ElementDefImpl( CURRENTASSIGNMENTLIST, "CurrentAssignment", null, 1, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_REPEATABLE), SifVersion.SIF20, SifVersion.SIF25 );

		// <Post> fields (0 entries)

		// <Posts> fields (1 entries)
		WorkforceDTD.POSTS_POST = new ElementDefImpl( POSTS, "Post", null, 1, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_REPEATABLE), SifVersion.SIF20, SifVersion.SIF25, SifTypeConverters.STRING );

		// <WorkforcePersonal> fields (8 entries)
		WorkforceDTD.WORKFORCEPERSONAL_REFID = new ElementDefImpl( WORKFORCEPERSONAL, "RefId", null, 1, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_ATTRIBUTE), SifVersion.SIF20, SifVersion.SIF25, SifTypeConverters.STRING );
		WorkforceDTD.WORKFORCEPERSONAL_LOCALID = new ElementDefImpl( WORKFORCEPERSONAL, "LocalId", null, 2, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_FIELD), SifVersion.SIF20, SifVersion.SIF25, SifTypeConverters.STRING );
		WorkforceDTD.WORKFORCEPERSONAL_ALERTMSGLIST = new ElementDefImpl( WORKFORCEPERSONAL, "AlertMsgList", null, 3, SifDtd.COMMON, "uk", 0, SifVersion.SIF20, SifVersion.SIF25 );
		WorkforceDTD.WORKFORCEPERSONAL_MEDICALALERTMSGLIST = new ElementDefImpl( WORKFORCEPERSONAL, "MedicalAlertMsgList", null, 4, SifDtd.COMMON, "uk", 0, SifVersion.SIF20, SifVersion.SIF25 );
		WorkforceDTD.WORKFORCEPERSONAL_PERSONALINFORMATION = new ElementDefImpl( WORKFORCEPERSONAL, "PersonalInformation", null, 5, SifDtd.COMMON, "uk", 0, SifVersion.SIF20, SifVersion.SIF25 );
		WorkforceDTD.WORKFORCEPERSONAL_TEACHERNUMBER = new ElementDefImpl( WORKFORCEPERSONAL, "TeacherNumber", null, 6, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_FIELD), SifVersion.SIF20, SifVersion.SIF25, SifTypeConverters.STRING );
		WorkforceDTD.WORKFORCEPERSONAL_NINUMBER = new ElementDefImpl( WORKFORCEPERSONAL, "NINumber", null, 7, SifDtd.WORKFORCE, "uk", (ElementDefImpl.FD_FIELD), SifVersion.SIF20, SifVersion.SIF25, SifTypeConverters.STRING );
		WorkforceDTD.WORKFORCEPERSONAL_CURRENTASSIGNMENTLIST = new ElementDefImpl( WORKFORCEPERSONAL, "CurrentAssignmentList", null, 8, SifDtd.WORKFORCE, "uk", 0, SifVersion.SIF20, SifVersion.SIF25 );
		WORKFORCEPERSONAL_SIF_EXTENDEDELEMENTS = new ElementDefImpl( WORKFORCEPERSONAL, "SIF_ExtendedElements", null, 127, SifDtd.GLOBAL, null, (0), SifVersion.SIF15r1, SifVersion.SIF25 );
		WORKFORCEPERSONAL_SIF_METADATA = new ElementDefImpl( WORKFORCEPERSONAL, "SIF_Metadata", null, 128, SifDtd.DATAMODEL, "uk", (0), SifVersion.SIF20, SifVersion.SIF25 );
	}

	#region Update SifDtd
	public override void AddElementMappings( IDictionary<String, IElementDef> dictionary )
	{
		dictionary[ "Posts" ] = POSTS;
		dictionary["Posts_Post"] = WorkforceDTD.POSTS_POST ;
		dictionary[ "WorkforcePersonal" ] = WORKFORCEPERSONAL;
		dictionary[ "WorkforcePersonal_SIF_ExtendedElements" ] = WORKFORCEPERSONAL_SIF_EXTENDEDELEMENTS ;
		dictionary[ "WorkforcePersonal_SIF_Metadata" ] = WORKFORCEPERSONAL_SIF_METADATA;
		dictionary["WorkforcePersonal_AlertMsgList"] = WorkforceDTD.WORKFORCEPERSONAL_ALERTMSGLIST ;
		dictionary["WorkforcePersonal_CurrentAssignmentList"] = WorkforceDTD.WORKFORCEPERSONAL_CURRENTASSIGNMENTLIST ;
		dictionary["WorkforcePersonal_LocalId"] = WorkforceDTD.WORKFORCEPERSONAL_LOCALID ;
		dictionary["WorkforcePersonal_MedicalAlertMsgList"] = WorkforceDTD.WORKFORCEPERSONAL_MEDICALALERTMSGLIST ;
		dictionary["WorkforcePersonal_NINumber"] = WorkforceDTD.WORKFORCEPERSONAL_NINUMBER ;
		dictionary["WorkforcePersonal_PersonalInformation"] = WorkforceDTD.WORKFORCEPERSONAL_PERSONALINFORMATION ;
		dictionary["WorkforcePersonal_RefId"] = WorkforceDTD.WORKFORCEPERSONAL_REFID ;
		dictionary["WorkforcePersonal_TeacherNumber"] = WorkforceDTD.WORKFORCEPERSONAL_TEACHERNUMBER ;
		dictionary[ "CurrentAssignment" ] = CURRENTASSIGNMENT;
		dictionary["CurrentAssignment_EstablishmentId"] = WorkforceDTD.CURRENTASSIGNMENT_ESTABLISHMENTID ;
		dictionary["CurrentAssignment_LAId"] = WorkforceDTD.CURRENTASSIGNMENT_LAID ;
		dictionary["CurrentAssignment_Posts"] = WorkforceDTD.CURRENTASSIGNMENT_POSTS ;
		dictionary[ "Post" ] = POST;
		dictionary[ "CurrentAssignmentList" ] = CURRENTASSIGNMENTLIST;
		dictionary["CurrentAssignmentList_CurrentAssignment"] = WorkforceDTD.CURRENTASSIGNMENTLIST_CURRENTASSIGNMENT ;
	}
	#endregion
}}
