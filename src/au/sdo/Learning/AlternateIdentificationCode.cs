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

namespace OpenADK.Library.au.Learning{

/// <summary>An AlternateIdentificationCode</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.4</para>
/// </remarks>
[Serializable]
public class AlternateIdentificationCode : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of an AlternateIdentificationCode
	/// </summary>
	public AlternateIdentificationCode() : base ( LearningDTD.ALTERNATEIDENTIFICATIONCODE ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="value">Gets or sets the content value of the &amp;lt;AlternateIdentificationCode&amp;gt; element</param>
	///
	public AlternateIdentificationCode( string value ) : base( LearningDTD.ALTERNATEIDENTIFICATIONCODE )
	{
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected AlternateIdentificationCode( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { LearningDTD.ALTERNATEIDENTIFICATIONCODE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;AlternateIdentificationCode&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;AlternateIdentificationCode&amp;gt; element"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearningDTD.ALTERNATEIDENTIFICATIONCODE ) ;
		}
		set
		{
			SetFieldValue( LearningDTD.ALTERNATEIDENTIFICATIONCODE, new SifString( value ), value );
		}
	}

}}
