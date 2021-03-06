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

/// <summary>A CommentList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class CommentList : SifKeyedList<CommentType>
{
	/// <summary>
	/// Creates an instance of a CommentList
	/// </summary>
	public CommentList() : base ( CommonDTD.COMMENTLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="comment">A Comment</param>
	///
	public CommentList( CommentType comment ) : base( CommonDTD.COMMENTLIST )
	{
		this.SafeAddChild( CommonDTD.COMMENTLIST_COMMENT, comment );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected CommentList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { CommonDTD.COMMENTLIST_COMMENT }; }
	}

	///<summary>Adds the value of the <c>&lt;Comment&gt;</c> element.</summary>
	/// <param name="CommentCode">A CommentCode</param>
	///<remarks>
	/// <para>This form of <c>setComment</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddComment</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void AddComment( string CommentCode ) {
		AddChild( CommonDTD.COMMENTLIST_COMMENT, new CommentType( CommentCode ) );
	}

}}
