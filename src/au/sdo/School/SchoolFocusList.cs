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

namespace OpenADK.Library.au.School{

/// <summary>A SchoolFocusList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class SchoolFocusList : SifKeyedList<SchoolFocus>
{
	/// <summary>
	/// Creates an instance of a SchoolFocusList
	/// </summary>
	public SchoolFocusList() : base ( SchoolDTD.SCHOOLFOCUSLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="schoolFocus">TThe type of educational institution as classified by its focus.</param>
	///
	public SchoolFocusList( SchoolFocus schoolFocus ) : base( SchoolDTD.SCHOOLFOCUSLIST )
	{
		this.SafeAddChild( SchoolDTD.SCHOOLFOCUSLIST_SCHOOLFOCUS, schoolFocus );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SchoolFocusList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { SchoolDTD.SCHOOLFOCUSLIST_SCHOOLFOCUS }; }
	}

	///<summary>Adds the value of the <c>&lt;SchoolFocus&gt;</c> element.</summary>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;SchoolFocus&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setSchoolFocus</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddSchoolFocus</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void AddSchoolFocus( AUSchoolFocusCodeType Value ) {
		AddChild( SchoolDTD.SCHOOLFOCUSLIST_SCHOOLFOCUS, new SchoolFocus( Value ) );
	}

}}
