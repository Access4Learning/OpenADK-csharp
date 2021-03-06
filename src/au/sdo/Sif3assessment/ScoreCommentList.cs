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

/// <summary>A ScoreCommentList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.6</para>
/// </remarks>
[Serializable]
public class ScoreCommentList : SifList<ScoreComment>
{
	/// <summary>
	/// Creates an instance of a ScoreCommentList
	/// </summary>
	public ScoreCommentList() : base ( Sif3assessmentDTD.SCORECOMMENTLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="scoreComment">A ScoreComment</param>
	///
	public ScoreCommentList( ScoreComment scoreComment ) : base( Sif3assessmentDTD.SCORECOMMENTLIST )
	{
		this.SafeAddChild( Sif3assessmentDTD.SCORECOMMENTLIST_SCORECOMMENT, scoreComment );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ScoreCommentList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { Sif3assessmentDTD.SCORECOMMENTLIST_SCORECOMMENT }; }
	}

	///<summary>Adds the value of the <c>&lt;ScoreComment&gt;</c> element.</summary>
	/// <param name="CommentCode">A CommentCode</param>
	/// <param name="Comment">A Comment</param>
	///<remarks>
	/// <para>This form of <c>setScoreComment</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddScoreComment</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.6</para>
	/// </remarks>
	public void AddScoreComment( string CommentCode, AbstractContentElementType Comment ) {
		AddChild( Sif3assessmentDTD.SCORECOMMENTLIST_SCORECOMMENT, new ScoreComment( CommentCode, Comment ) );
	}

}}
