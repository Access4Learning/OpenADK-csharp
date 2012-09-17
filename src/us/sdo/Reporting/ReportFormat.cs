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
using OpenADK.Library.Infra;

namespace OpenADK.Library.us.Reporting{

/// <summary>References a third-party format that describes the visual representation of the report data.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class ReportFormat : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a ReportFormat
	/// </summary>
	public ReportFormat() : base ( ReportingDTD.REPORTFORMAT ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="type">This attribute indicates the encoding of the format reference.</param>
	///<param name="value">Gets or sets the content value of the &amp;lt;ReportFormat&amp;gt; element</param>
	///
	public ReportFormat( ReportFormatType type, string value ) : base( ReportingDTD.REPORTFORMAT )
	{
		this.SetType( type );
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ReportFormat( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { ReportingDTD.REPORTFORMAT_TYPE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;ReportFormat&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;ReportFormat&amp;gt; element"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( ReportingDTD.REPORTFORMAT ) ;
		}
		set
		{
			SetFieldValue( ReportingDTD.REPORTFORMAT, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>Type</c> attribute.
	/// </summary>
	/// <value> The <c>Type</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "This attribute indicates the encoding of the format reference."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Type
	{
		get
		{ 
			return GetFieldValue( ReportingDTD.REPORTFORMAT_TYPE );
		}
		set
		{
			SetField( ReportingDTD.REPORTFORMAT_TYPE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>Type</c> attribute.
	/// </summary>
	/// <param name="val">A ReportFormatType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "This attribute indicates the encoding of the format reference."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetType( ReportFormatType val )
	{
		SetField( ReportingDTD.REPORTFORMAT_TYPE, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>ContentType</c> attribute.
	/// </summary>
	/// <value> The <c>ContentType</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The vendor-defined content type (e.g. com.vendor.format, PDF, etc."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public string ContentType
	{
		get
		{
			return (string) GetSifSimpleFieldValue( ReportingDTD.REPORTFORMAT_CONTENTTYPE ) ;
		}
		set
		{
			SetFieldValue( ReportingDTD.REPORTFORMAT_CONTENTTYPE, new SifString( value ), value );
		}
	}

}}
