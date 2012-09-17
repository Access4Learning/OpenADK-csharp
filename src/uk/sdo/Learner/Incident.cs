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

/// <summary>An Incident</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class Incident : SifElement
{
	/// <summary>
	/// Creates an instance of an Incident
	/// </summary>
	public Incident() : base ( LearnerDTD.INCIDENT ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="date">Date and time Incident took place (started to take place)</param>
	///<param name="location">The Location the Incident (mostly) took place</param>
	///<param name="confidentiality">May indicate the type of persons that this should be displayed to</param>
	///
	public Incident( DateTime? date, Location location, Confidentiality confidentiality ) : base( LearnerDTD.INCIDENT )
	{
		this.Date = date;
		this.SetLocation( location );
		this.SetConfidentiality( confidentiality );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected Incident( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { LearnerDTD.INCIDENT_DATE, LearnerDTD.INCIDENT_LOCATION, LearnerDTD.INCIDENT_CONFIDENTIALITY }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Date&gt;</c> element.
	/// </summary>
	/// <value> The <c>Date</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Date and time Incident took place (started to take place)"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public DateTime? Date
	{
		get
		{
			return (DateTime?) GetSifSimpleFieldValue( LearnerDTD.INCIDENT_DATE ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.INCIDENT_DATE, new SifDateTime( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Duration&gt;</c> element.
	/// </summary>
	/// <value> The <c>Duration</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "How long incident lasted (minutes)"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public int? Duration
	{
		get
		{
			return (int?) GetSifSimpleFieldValue( LearnerDTD.INCIDENT_DURATION ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.INCIDENT_DURATION, new SifInt( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Location&gt;</c> element.
	/// </summary>
	/// <value> The <c>Location</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The Location the Incident (mostly) took place"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string Location
	{
		get
		{ 
			return GetFieldValue( LearnerDTD.INCIDENT_LOCATION );
		}
		set
		{
			SetField( LearnerDTD.INCIDENT_LOCATION, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;Location&gt;</c> element.
	/// </summary>
	/// <param name="val">A Location object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The Location the Incident (mostly) took place"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetLocation( Location val )
	{
		SetField( LearnerDTD.INCIDENT_LOCATION, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Comments&gt;</c> element.
	/// </summary>
	/// <value> The <c>Comments</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "free text comments about the incident"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string Comments
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.INCIDENT_COMMENTS ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.INCIDENT_COMMENTS, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;PrecursorRefId&gt;</c> element.
	/// </summary>
	/// <value> The <c>PrecursorRefId</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "A Preceding LearnerBehaviourIncidentRefID to which this behaviour is linked"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string PrecursorRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( LearnerDTD.INCIDENT_PRECURSORREFID ) ;
		}
		set
		{
			SetFieldValue( LearnerDTD.INCIDENT_PRECURSORREFID, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Confidentiality&gt;</c> element.
	/// </summary>
	/// <value> The <c>Confidentiality</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "May indicate the type of persons that this should be displayed to"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string Confidentiality
	{
		get
		{ 
			return GetFieldValue( LearnerDTD.INCIDENT_CONFIDENTIALITY );
		}
		set
		{
			SetField( LearnerDTD.INCIDENT_CONFIDENTIALITY, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;Confidentiality&gt;</c> element.
	/// </summary>
	/// <param name="val">A Confidentiality object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "May indicate the type of persons that this should be displayed to"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetConfidentiality( Confidentiality val )
	{
		SetField( LearnerDTD.INCIDENT_CONFIDENTIALITY, val );
	}

}}
