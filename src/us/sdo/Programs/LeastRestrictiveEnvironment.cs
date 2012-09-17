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

namespace OpenADK.Library.us.Programs{

/// <summary>A LeastRestrictiveEnvironment</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class LeastRestrictiveEnvironment : SifElement
{
	/// <summary>
	/// Creates an instance of a LeastRestrictiveEnvironment
	/// </summary>
	public LeastRestrictiveEnvironment() : base ( ProgramsDTD.LEASTRESTRICTIVEENVIRONMENT ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected LeastRestrictiveEnvironment( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <value> The <c>Code</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Category represents the optimal educational setting in which the student should be placed (setting where child has most desirable learning environment).  Relates specifically to special education."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public string Code
	{
		get
		{
			return (string) GetSifSimpleFieldValue( ProgramsDTD.LEASTRESTRICTIVEENVIRONMENT_CODE ) ;
		}
		set
		{
			SetFieldValue( ProgramsDTD.LEASTRESTRICTIVEENVIRONMENT_CODE, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;OtherCodeList&gt;</c> element.
	/// </summary>
	/// <value> An OtherCodeList </value>
	/// <remarks>
	/// <para>To remove the <c>OtherCodeList</c>, set <c>OtherCodeList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public OtherCodeList OtherCodeList
	{
		get
		{
			return (OtherCodeList)GetChild( ProgramsDTD.LEASTRESTRICTIVEENVIRONMENT_OTHERCODELIST);
		}
		set
		{
			RemoveChild( ProgramsDTD.LEASTRESTRICTIVEENVIRONMENT_OTHERCODELIST);
			if( value != null)
			{
				AddChild( ProgramsDTD.LEASTRESTRICTIVEENVIRONMENT_OTHERCODELIST, value );
			}
		}
	}

}}
