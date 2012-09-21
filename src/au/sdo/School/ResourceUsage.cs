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

namespace OpenADK.Library.au.School{

/// <summary>A ResourceUsage</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.5</para>
/// </remarks>
[Serializable]
public class ResourceUsage : SifDataObject
{
	/// <summary>
	/// Creates an instance of a ResourceUsage
	/// </summary>
	public ResourceUsage() : base( Adk.SifVersion, SchoolDTD.RESOURCEUSAGE ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="refId">A RefId</param>
	///<param name="schoolInfoRefId">A SchoolInfoRefId</param>
	///<param name="resourceUsageContentType">A ResourceUsageContentType</param>
	///<param name="resourceReportColumnList">A ResourceReportColumnList</param>
	///<param name="resourceReportLineList">A ResourceReportLineList</param>
	///
	public ResourceUsage( string refId, string schoolInfoRefId, ResourceUsageContentType resourceUsageContentType, ResourceReportColumn resourceReportColumnList, ResourceReportLine resourceReportLineList ) : base( Adk.SifVersion, SchoolDTD.RESOURCEUSAGE )
	{
		this.RefId = refId;
		this.SchoolInfoRefId = schoolInfoRefId;
		this.ResourceUsageContentType = resourceUsageContentType;
		this.ResourceReportColumnList =  new ResourceReportColumnList( resourceReportColumnList );
		this.ResourceReportLineList =  new ResourceReportLineList( resourceReportLineList );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ResourceUsage( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { SchoolDTD.RESOURCEUSAGE_REFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>RefId</c> attribute.
	/// </summary>
	/// <value> The <c>RefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	public override string RefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.RESOURCEUSAGE_REFID ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.RESOURCEUSAGE_REFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SchoolInfoRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>SchoolInfoRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	public string SchoolInfoRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.RESOURCEUSAGE_SCHOOLINFOREFID ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.RESOURCEUSAGE_SCHOOLINFOREFID, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;ResourceUsageContentType&gt;</c> element.</summary>
	/// <param name="Code">A Code</param>
	///<remarks>
	/// <para>This form of <c>setResourceUsageContentType</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ResourceUsageContentType</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	public void SetResourceUsageContentType( AUCodeSetsResourceUsageContentTypeType Code ) {
		RemoveChild( SchoolDTD.RESOURCEUSAGE_RESOURCEUSAGECONTENTTYPE);
		AddChild( SchoolDTD.RESOURCEUSAGE_RESOURCEUSAGECONTENTTYPE, new ResourceUsageContentType( Code ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ResourceUsageContentType&gt;</c> element.
	/// </summary>
	/// <value> A ResourceUsageContentType </value>
	/// <remarks>
	/// <para>To remove the <c>ResourceUsageContentType</c>, set <c>ResourceUsageContentType</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	public ResourceUsageContentType ResourceUsageContentType
	{
		get
		{
			return (ResourceUsageContentType)GetChild( SchoolDTD.RESOURCEUSAGE_RESOURCEUSAGECONTENTTYPE);
		}
		set
		{
			RemoveChild( SchoolDTD.RESOURCEUSAGE_RESOURCEUSAGECONTENTTYPE);
			if( value != null)
			{
				AddChild( SchoolDTD.RESOURCEUSAGE_RESOURCEUSAGECONTENTTYPE, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;ResourceReportColumnList&gt;</c> element.</summary>
	/// <param name="ResourceReportColumn">A ResourceReportColumn</param>
	///<remarks>
	/// <para>This form of <c>setResourceReportColumnList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ResourceReportColumnList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	public void SetResourceReportColumnList( ResourceReportColumn ResourceReportColumn ) {
		RemoveChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTCOLUMNLIST);
		AddChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTCOLUMNLIST, new ResourceReportColumnList( ResourceReportColumn ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ResourceReportColumnList&gt;</c> element.
	/// </summary>
	/// <value> A ResourceReportColumnList </value>
	/// <remarks>
	/// <para>To remove the <c>ResourceReportColumnList</c>, set <c>ResourceReportColumnList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	public ResourceReportColumnList ResourceReportColumnList
	{
		get
		{
			return (ResourceReportColumnList)GetChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTCOLUMNLIST);
		}
		set
		{
			RemoveChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTCOLUMNLIST);
			if( value != null)
			{
				AddChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTCOLUMNLIST, value );
			}
		}
	}

	///<summary>Sets the value of the <c>&lt;ResourceReportLineList&gt;</c> element.</summary>
	/// <param name="ResourceReportLine">A ResourceReportLine</param>
	///<remarks>
	/// <para>This form of <c>setResourceReportLineList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>ResourceReportLineList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	public void SetResourceReportLineList( ResourceReportLine ResourceReportLine ) {
		RemoveChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTLINELIST);
		AddChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTLINELIST, new ResourceReportLineList( ResourceReportLine ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ResourceReportLineList&gt;</c> element.
	/// </summary>
	/// <value> A ResourceReportLineList </value>
	/// <remarks>
	/// <para>To remove the <c>ResourceReportLineList</c>, set <c>ResourceReportLineList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.5</para>
	/// </remarks>
	public ResourceReportLineList ResourceReportLineList
	{
		get
		{
			return (ResourceReportLineList)GetChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTLINELIST);
		}
		set
		{
			RemoveChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTLINELIST);
			if( value != null)
			{
				AddChild( SchoolDTD.RESOURCEUSAGE_RESOURCEREPORTLINELIST, value );
			}
		}
	}

}}
