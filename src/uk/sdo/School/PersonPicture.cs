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

namespace OpenADK.Library.uk.School{

/// <summary>This object contains information about the learner's picture. Compare with US/Canada object: StudentPicture</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class PersonPicture : SifDataObject
{
	/// <summary>
	/// Creates an instance of a PersonPicture
	/// </summary>
	public PersonPicture() : base( Adk.SifVersion, SchoolDTD.PERSONPICTURE ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="personRefId">It is important to note that using the same GUID a person may have a LearnerPersonal record, a WorkforcePersonal record, and a ContactPersonal record all at the same time.  This same picture is applicable regardless of the associated object type(s).</param>
	///<param name="sifRefObjectList">A list of one or more "personal" object type codes representing objects that can be requested for this person using the specified PersonRefId.</param>
	///<param name="schoolYear">School year for which this enrolment is applicable, expressed as the four-digit year in which the school year ends (e.g. 2007 for the 2006/07 school year).</param>
	///<param name="pictureSource">This element defines the picture. If the Type attribute is URL, this is the location of the picture in [JPEG] format; if Type is JPEG, this is the [JPEG] image data encoded using the Base64 Content-Transfer-Encoding defined in Section 6.8 of [RFC 2045]. CBDS: 100019</param>
	///
	public PersonPicture( string personRefId, SIF_RefObject sifRefObjectList, int? schoolYear, PictureSource pictureSource ) : base( Adk.SifVersion, SchoolDTD.PERSONPICTURE )
	{
		this.PersonRefId = personRefId;
		this.SIF_RefObjectList =  new SIF_RefObjectList( sifRefObjectList );
		this.SchoolYear = schoolYear;
		this.PictureSource = pictureSource;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected PersonPicture( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { SchoolDTD.PERSONPICTURE_PERSONREFID }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>PersonRefId</c> attribute.
	/// </summary>
	/// <value> The <c>PersonRefId</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "It is important to note that using the same GUID a person may have a LearnerPersonal record, a WorkforcePersonal record, and a ContactPersonal record all at the same time.  This same picture is applicable regardless of the associated object type(s)."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public string PersonRefId
	{
		get
		{
			return (string) GetSifSimpleFieldValue( SchoolDTD.PERSONPICTURE_PERSONREFID ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.PERSONPICTURE_PERSONREFID, new SifString( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;SIF_RefObjectList&gt;</c> element.</summary>
	/// <param name="SifRefObject">The name of the "personal" object that this picture represents. Values: LearnerPersonal, WorkforcePersonal, ContactPersonal</param>
	///<remarks>
	/// <para>This form of <c>setSIF_RefObjectList</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>SIF_RefObjectList</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void SetSIF_RefObjectList( SIF_RefObject SifRefObject ) {
		RemoveChild( SchoolDTD.PERSONPICTURE_SIF_REFOBJECTLIST);
		AddChild( SchoolDTD.PERSONPICTURE_SIF_REFOBJECTLIST, new SIF_RefObjectList( SifRefObject ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_RefObjectList&gt;</c> element.
	/// </summary>
	/// <value> A SIF_RefObjectList </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "A list of one or more "personal" object type codes representing objects that can be requested for this person using the specified PersonRefId."</para>
	/// <para>To remove the <c>SIF_RefObjectList</c>, set <c>SIF_RefObjectList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public SIF_RefObjectList SIF_RefObjectList
	{
		get
		{
			return (SIF_RefObjectList)GetChild( SchoolDTD.PERSONPICTURE_SIF_REFOBJECTLIST);
		}
		set
		{
			RemoveChild( SchoolDTD.PERSONPICTURE_SIF_REFOBJECTLIST);
			if( value != null)
			{
				AddChild( SchoolDTD.PERSONPICTURE_SIF_REFOBJECTLIST, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SchoolYear&gt;</c> element.
	/// </summary>
	/// <value> The <c>SchoolYear</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "School year for which this enrolment is applicable, expressed as the four-digit year in which the school year ends (e.g. 2007 for the 2006/07 school year)."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public int? SchoolYear
	{
		get
		{
			return (int?) GetSifSimpleFieldValue( SchoolDTD.PERSONPICTURE_SCHOOLYEAR ) ;
		}
		set
		{
			SetFieldValue( SchoolDTD.PERSONPICTURE_SCHOOLYEAR, new SifInt( value ), value );
		}
	}

	///<summary>Sets the value of the <c>&lt;PictureSource&gt;</c> element.</summary>
	/// <param name="Type">The way the picture is specified.</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;PictureSource&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setPictureSource</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>PictureSource</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void SetPictureSource( PictureType Type, string Value ) {
		RemoveChild( SchoolDTD.PERSONPICTURE_PICTURESOURCE);
		AddChild( SchoolDTD.PERSONPICTURE_PICTURESOURCE, new PictureSource( Type, Value ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;PictureSource&gt;</c> element.
	/// </summary>
	/// <value> A PictureSource </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "This element defines the picture. If the Type attribute is URL, this is the location of the picture in [JPEG] format; if Type is JPEG, this is the [JPEG] image data encoded using the Base64 Content-Transfer-Encoding defined in Section 6.8 of [RFC 2045]. CBDS: 100019"</para>
	/// <para>To remove the <c>PictureSource</c>, set <c>PictureSource</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public PictureSource PictureSource
	{
		get
		{
			return (PictureSource)GetChild( SchoolDTD.PERSONPICTURE_PICTURESOURCE);
		}
		set
		{
			RemoveChild( SchoolDTD.PERSONPICTURE_PICTURESOURCE);
			if( value != null)
			{
				AddChild( SchoolDTD.PERSONPICTURE_PICTURESOURCE, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;OKToPublish&gt;</c> element.
	/// </summary>
	/// <value> The <c>OKToPublish</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Can the picture be published?"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public string OKToPublish
	{
		get
		{ 
			return GetFieldValue( SchoolDTD.PERSONPICTURE_OKTOPUBLISH );
		}
		set
		{
			SetField( SchoolDTD.PERSONPICTURE_OKTOPUBLISH, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;OKToPublish&gt;</c> element.
	/// </summary>
	/// <param name="val">A YesNo object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Can the picture be published?"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void SetOKToPublish( YesNo val )
	{
		SetField( SchoolDTD.PERSONPICTURE_OKTOPUBLISH, val );
	}

}}
