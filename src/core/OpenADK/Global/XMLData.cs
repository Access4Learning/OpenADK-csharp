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

namespace OpenADK.Library.Global{

/// <summary>Contains an arbitary XML element, encoded in UTF-8</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class XMLData : SifElement
{
	/// <summary>
	/// Creates an instance of a XMLData
	/// </summary>
	public XMLData() : base ( GlobalDTD.XMLDATA ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected XMLData( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>Description</c> attribute.
	/// </summary>
	/// <value> The <c>Description</c> attribute of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this attribute as: "Contains an optional description of the content or a processing hint with regard to its structure (e.g. named standard, file layout or XSD). Contents may be mandated in instances of this type, or types that follow the AbstractContentPackageType pattern."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public string Description
	{
		get
		{
			return (string) GetSifSimpleFieldValue( GlobalDTD.XMLDATA_DESCRIPTION ) ;
		}
		set
		{
			SetFieldValue( GlobalDTD.XMLDATA_DESCRIPTION, new SifString( value ), value );
		}
	}

		#region EXTRA METHODS

// BEGIN EXTRA METHODS (C:/dev/OpenADK-java/adk-generator/datadef/core/sif20/XMLData.txt.cs)

	/// <summary>
	/// The XML DOM Document that is a child of this element
	/// </summary>
	private System.Xml.XmlDocument fXmlData;
	
	///
	/// <summary>Gets or sets the Xml Document that is a child of this element</summary>
	///
	public System.Xml.XmlDocument Xml
	{
		get
		{
			return fXmlData;
		}
		set
		{
			fXmlData = value;
		}
	}
	

// END EXTRA METHODS

		#endregion // EXTRA METHODS
}}
