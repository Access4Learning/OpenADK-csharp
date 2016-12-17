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

namespace OpenADK.Library.us.Assessment{

/// <summary>A StaffPersonalRefIdSif3</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class StaffPersonalRefIdSif3 : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a StaffPersonalRefIdSif3
	/// </summary>
	public StaffPersonalRefIdSif3() : base ( AssessmentDTD.STAFFPERSONALREFIDSIF3 ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="value">Gets or sets the content value of the &amp;lt;StaffPersonalRefIdSif3&amp;gt; element</param>
	///
	public StaffPersonalRefIdSif3( string value ) : base( AssessmentDTD.STAFFPERSONALREFIDSIF3 )
	{
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected StaffPersonalRefIdSif3( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { AssessmentDTD.STAFFPERSONALREFIDSIF3 }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;StaffPersonalRefIdSif3&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;StaffPersonalRefIdSif3&amp;gt; element"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.STAFFPERSONALREFIDSIF3 ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.STAFFPERSONALREFIDSIF3, new SifString( value ), value );
		}
	}

}}