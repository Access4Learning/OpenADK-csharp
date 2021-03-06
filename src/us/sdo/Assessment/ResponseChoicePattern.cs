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

namespace OpenADK.Library.us.Assessment{

/// <summary>A ResponseChoicePattern</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class ResponseChoicePattern : SifElement
{
	/// <summary>
	/// Creates an instance of a ResponseChoicePattern
	/// </summary>
	public ResponseChoicePattern() : base ( AssessmentDTD.RESPONSECHOICEPATTERN ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ResponseChoicePattern( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Sets the value of the <c>&lt;Choice&gt;</c> element.</summary>
	/// <param name="ChoiceContent">The text of the choice, such as true, 27, or Important economic and social factors.</param>
	///<remarks>
	/// <para>This form of <c>setChoice</c> is provided as a convenience method
	/// that is functionally equivalent to the <c>Choice</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public void SetChoice( ContentElement ChoiceContent ) {
		RemoveChild( AssessmentDTD.RESPONSECHOICEPATTERN_CHOICE);
		AddChild( AssessmentDTD.RESPONSECHOICEPATTERN_CHOICE, new Choice( ChoiceContent ) );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Choice&gt;</c> element.
	/// </summary>
	/// <value> A Choice </value>
	/// <remarks>
	/// <para>To remove the <c>Choice</c>, set <c>Choice</c> to <c>null</c></para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public Choice Choice
	{
		get
		{
			return (Choice)GetChild( AssessmentDTD.RESPONSECHOICEPATTERN_CHOICE);
		}
		set
		{
			RemoveChild( AssessmentDTD.RESPONSECHOICEPATTERN_CHOICE);
			if( value != null)
			{
				AddChild( AssessmentDTD.RESPONSECHOICEPATTERN_CHOICE, value );
			}
		}
	}

}}
