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

namespace OpenADK.Library.au.Sif3assessment{

/// <summary>A FormIdentifier</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class FormIdentifier : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a FormIdentifier
	/// </summary>
	public FormIdentifier() : base ( Sif3assessmentDTD.FORMIDENTIFIER ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="formIdentifierType">A FormIdentifierType</param>
	///<param name="value">Gets or sets the content value of the &amp;lt;FormIdentifier&amp;gt; element</param>
	///
	public FormIdentifier( AssessmentIdType formIdentifierType, string value ) : base( Sif3assessmentDTD.FORMIDENTIFIER )
	{
		this.SetFormIdentifierType( formIdentifierType );
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected FormIdentifier( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { Sif3assessmentDTD.FORMIDENTIFIER_FORMIDENTIFIERTYPE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;FormIdentifier&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;FormIdentifier&amp;gt; element"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.FORMIDENTIFIER ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.FORMIDENTIFIER, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>FormIdentifierType</c> attribute.
	/// </summary>
	/// <value> The <c>FormIdentifierType</c> attribute of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string FormIdentifierType
	{
		get
		{ 
			return GetFieldValue( Sif3assessmentDTD.FORMIDENTIFIER_FORMIDENTIFIERTYPE );
		}
		set
		{
			SetField( Sif3assessmentDTD.FORMIDENTIFIER_FORMIDENTIFIERTYPE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>FormIdentifierType</c> attribute.
	/// </summary>
	/// <param name="val">A AssessmentIdType object</param>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetFormIdentifierType( AssessmentIdType val )
	{
		SetField( Sif3assessmentDTD.FORMIDENTIFIER_FORMIDENTIFIERTYPE, val );
	}

}}
