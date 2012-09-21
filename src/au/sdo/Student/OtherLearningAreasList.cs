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

/// <summary>An OtherLearningAreasList</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class OtherLearningAreasList : SifKeyedList<OtherLearningArea>
{
	/// <summary>
	/// Creates an instance of an OtherLearningAreasList
	/// </summary>
	public OtherLearningAreasList() : base ( StudentDTD.OTHERLEARNINGAREASLIST ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="otherLearningArea">An OtherLearningArea</param>
	///
	public OtherLearningAreasList( OtherLearningArea otherLearningArea ) : base( StudentDTD.OTHERLEARNINGAREASLIST )
	{
		this.SafeAddChild( StudentDTD.OTHERLEARNINGAREASLIST_OTHERLEARNINGAREA, otherLearningArea );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected OtherLearningAreasList( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { StudentDTD.OTHERLEARNINGAREASLIST_OTHERLEARNINGAREA }; }
	}

	///<summary>Adds the value of the <c>&lt;OtherLearningArea&gt;</c> element.</summary>
	/// <param name="Description">Description of Other Learning Area.</param>
	///<remarks>
	/// <para>This form of <c>setOtherLearningArea</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddOtherLearningArea</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void AddOtherLearningArea( string Description ) {
		AddChild( StudentDTD.OTHERLEARNINGAREASLIST_OTHERLEARNINGAREA, new OtherLearningArea( Description ) );
	}

}}
