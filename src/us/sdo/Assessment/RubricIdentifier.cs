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

/// <summary>A RubricIdentifier</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class RubricIdentifier : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a RubricIdentifier
	/// </summary>
	public RubricIdentifier() : base ( AssessmentDTD.RUBRICIDENTIFIER ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="value">Gets or sets the content value of the &amp;lt;RubricIdentifier&amp;gt; element</param>
	///
	public RubricIdentifier( string value ) : base( AssessmentDTD.RUBRICIDENTIFIER )
	{
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected RubricIdentifier( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { AssessmentDTD.RUBRICIDENTIFIER }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;RubricIdentifier&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;RubricIdentifier&amp;gt; element"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( AssessmentDTD.RUBRICIDENTIFIER ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.RUBRICIDENTIFIER, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>RubricIdentifierType</c> attribute.
	/// </summary>
	/// <value> The <c>RubricIdentifierType</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The type of identifier that is provided for this ScoreTable."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string RubricIdentifierType
	{
		get
		{ 
			return GetFieldValue( AssessmentDTD.RUBRICIDENTIFIER_RUBRICIDENTIFIERTYPE );
		}
		set
		{
			SetField( AssessmentDTD.RUBRICIDENTIFIER_RUBRICIDENTIFIERTYPE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>RubricIdentifierType</c> attribute.
	/// </summary>
	/// <param name="val">A RubricIdentifierType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The type of identifier that is provided for this ScoreTable."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void SetRubricIdentifierType( RubricIdentifierType val )
	{
		SetField( AssessmentDTD.RUBRICIDENTIFIER_RUBRICIDENTIFIERTYPE, val );
	}

}}
