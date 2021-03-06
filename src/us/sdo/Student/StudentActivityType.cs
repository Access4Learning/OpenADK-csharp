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

namespace OpenADK.Library.us.Student{

/// <summary>A StudentActivityType</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class StudentActivityType : SifElement
{
	/// <summary>
	/// Creates an instance of a StudentActivityType
	/// </summary>
	public StudentActivityType() : base ( StudentDTD.STUDENTACTIVITYTYPE ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected StudentActivityType( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <value> The <c>Code</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Code identifying the co-curricular or extra-curricular activity."</para>
	/// <para>Version: 2.6</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public string Code
	{
		get
		{
			return (string) GetSifSimpleFieldValue( StudentDTD.STUDENTACTIVITYTYPE_CODE ) ;
		}
		set
		{
			SetFieldValue( StudentDTD.STUDENTACTIVITYTYPE_CODE, new SifString( value ), value );
		}
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
			return (OtherCodeList)GetChild( StudentDTD.STUDENTACTIVITYTYPE_OTHERCODELIST);
		}
		set
		{
			RemoveChild( StudentDTD.STUDENTACTIVITYTYPE_OTHERCODELIST);
			if( value != null)
			{
				AddChild( StudentDTD.STUDENTACTIVITYTYPE_OTHERCODELIST, value );
			}
		}
	}

}}
