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

namespace OpenADK.Library.au.Common{

/// <summary>A SchoolContactList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class SchoolContactList : SifKeyedList<SchoolContact>
{
	/// <summary>
	/// Creates an instance of a SchoolContactList
	/// </summary>
	public SchoolContactList() : base ( CommonDTD.SCHOOLCONTACTLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="schoolContact">A SchoolContact</param>
	///
	public SchoolContactList( SchoolContact schoolContact ) : base( CommonDTD.SCHOOLCONTACTLIST )
	{
		this.SafeAddChild( CommonDTD.SCHOOLCONTACTLIST_SCHOOLCONTACT, schoolContact );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SchoolContactList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { CommonDTD.SCHOOLCONTACTLIST_SCHOOLCONTACT }; }
	}

	///<summary>Adds the value of the <c>&lt;SchoolContact&gt;</c> element.</summary>
	/// <param name="ContactInfo">A ContactInfo</param>
	///<remarks>
	/// <para>This form of <c>setSchoolContact</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddSchoolContact</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void AddSchoolContact( ContactInfo ContactInfo ) {
		AddChild( CommonDTD.SCHOOLCONTACTLIST_SCHOOLCONTACT, new SchoolContact( ContactInfo ) );
	}

}}
