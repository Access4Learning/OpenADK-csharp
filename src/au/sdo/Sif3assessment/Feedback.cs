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

/// <summary>A Feedback</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class Feedback : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a Feedback
	/// </summary>
	public Feedback() : base ( Sif3assessmentDTD.FEEDBACK ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="diagnosticStatement">A DiagnosticStatement</param>
	///
	public Feedback( string diagnosticStatement ) : base( Sif3assessmentDTD.FEEDBACK )
	{
		this.DiagnosticStatement = diagnosticStatement;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected Feedback( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { Sif3assessmentDTD.FEEDBACK_DIAGNOSTICSTATEMENT }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;DiagnosticStatement&gt;</c> element.
	/// </summary>
	/// <value> The <c>DiagnosticStatement</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string DiagnosticStatement
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.FEEDBACK_DIAGNOSTICSTATEMENT ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.FEEDBACK_DIAGNOSTICSTATEMENT, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Description&gt;</c> element.
	/// </summary>
	/// <value> The <c>Description</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Description
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.FEEDBACK_DESCRIPTION ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.FEEDBACK_DESCRIPTION, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Source&gt;</c> element.
	/// </summary>
	/// <value> The <c>Source</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public string Source
	{
		get
		{
			return (string) GetSifSimpleFieldValue( Sif3assessmentDTD.FEEDBACK_SOURCE ) ;
		}
		set
		{
			SetFieldValue( Sif3assessmentDTD.FEEDBACK_SOURCE, new SifString( value ), value );
		}
	}

}}