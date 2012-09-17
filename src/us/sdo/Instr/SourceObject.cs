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

/// <summary>A SourceObject</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class SourceObject : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a SourceObject
	/// </summary>
	public SourceObject() : base ( InstrDTD.SOURCEOBJECT ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="sifRefObject">The type of learning object the source object is.</param>
	///<param name="value">Gets or sets the content value of the &amp;lt;SourceObject&amp;gt; element</param>
	///
	public SourceObject( string sifRefObject, string value ) : base( InstrDTD.SOURCEOBJECT )
	{
		this.SIF_RefObject = sifRefObject;
		this.Value = value;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SourceObject( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { InstrDTD.SOURCEOBJECT_SIF_REFOBJECT }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SourceObject&gt;</c> element.
	/// </summary>
	/// <value> The <c>Value</c> of the content of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this of the content as: "Gets or sets the content value of the &amp;lt;SourceObject&amp;gt; element"</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Value
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InstrDTD.SOURCEOBJECT ) ;
		}
		set
		{
			SetFieldValue( InstrDTD.SOURCEOBJECT, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>SIF_RefObject</c> attribute.
	/// </summary>
	/// <value> The <c>SIF_RefObject</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "The type of learning object the source object is."</para>
	/// <para>This attribute is known by more than one tag name depending on the version of SIF in use. 
	/// The ADK will use the tag names shown below when parsing and rendering attributes of this kind.</para>
	/// <list type="table"><listheader><term>Version</term><description>Tag</description></listheader>;
	/// <item><term>2.0 (and greater)</term><description>&lt;SIF_RefObject&gt;</description></item>
	/// </list>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string SIF_RefObject
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InstrDTD.SOURCEOBJECT_SIF_REFOBJECT ) ;
		}
		set
		{
			SetFieldValue( InstrDTD.SOURCEOBJECT_SIF_REFOBJECT, new SifString( value ), value );
		}
	}

}}
