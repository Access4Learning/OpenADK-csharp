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

namespace OpenADK.Library.us.Instr{

/// <summary>A RelatedLearningStandardItem</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.1</para>
/// </remarks>
[Serializable]
public class RelatedLearningStandardItem : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a RelatedLearningStandardItem
	/// </summary>
	public RelatedLearningStandardItem() : base ( InstrDTD.RELATEDLEARNINGSTANDARDITEM ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="relationshipType">A RelationshipType</param>
	///<param name="value">Gets or sets the content value of the &amp;lt;RelatedLearningStandardItem&amp;gt; element</param>
	///
	public RelatedLearningStandardItem( string relationshipType, string value ) : base( InstrDTD.RELATEDLEARNINGSTANDARDITEM )
	{
		this.RelationshipType = relationshipType;
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected RelatedLearningStandardItem( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { InstrDTD.RELATEDLEARNINGSTANDARDITEM_RELATIONSHIPTYPE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;RelatedLearningStandardItem&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;RelatedLearningStandardItem&amp;gt; element"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InstrDTD.RELATEDLEARNINGSTANDARDITEM ) ;
		}
		set
		{
			SetFieldValue( InstrDTD.RELATEDLEARNINGSTANDARDITEM, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>RelationshipType</c> attribute.
	/// </summary>
	/// <value> The <c>RelationshipType</c> attribute of this object.</value>
	/// <remarks>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.1</para>
	/// </remarks>
	public string RelationshipType
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InstrDTD.RELATEDLEARNINGSTANDARDITEM_RELATIONSHIPTYPE ) ;
		}
		set
		{
			SetFieldValue( InstrDTD.RELATEDLEARNINGSTANDARDITEM_RELATIONSHIPTYPE, new SifString( value ), value );
		}
	}

}}
