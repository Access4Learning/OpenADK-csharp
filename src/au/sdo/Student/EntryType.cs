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

namespace OpenADK.Library.au.Student{

/// <summary>An EntryType</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class EntryType : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of an EntryType
	/// </summary>
	public EntryType() : base ( StudentDTD.ENTRYTYPE ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="code">Code indicating the type of entry for this enrollment</param>
	///
	public EntryType( AU0619EntryType code ) : base( StudentDTD.ENTRYTYPE )
	{
		this.SetCode( code );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected EntryType( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { StudentDTD.ENTRYTYPE_CODE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <value> The <c>Code</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Code indicating the type of entry for this enrollment"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string Code
	{
		get
		{ 
			return GetFieldValue( StudentDTD.ENTRYTYPE_CODE );
		}
		set
		{
			SetField( StudentDTD.ENTRYTYPE_CODE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <param name="val">A AU0619EntryType object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Code indicating the type of entry for this enrollment"</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetCode( AU0619EntryType val )
	{
		SetField( StudentDTD.ENTRYTYPE_CODE, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;OtherCodeList&gt;</c> element.
	/// </summary>
	/// <value> An OtherCodeList </value>
	/// <remarks>
	/// <para>To remove the <c>OtherCodeList</c>, set <c>OtherCodeList</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public OtherCodeList OtherCodeList
	{
		get
		{
			return (OtherCodeList)GetChild( StudentDTD.ENTRYTYPE_OTHERCODELIST);
		}
		set
		{
			RemoveChild( StudentDTD.ENTRYTYPE_OTHERCODELIST);
			if( value != null)
			{
				AddChild( StudentDTD.ENTRYTYPE_OTHERCODELIST, value );
			}
		}
	}

}}
