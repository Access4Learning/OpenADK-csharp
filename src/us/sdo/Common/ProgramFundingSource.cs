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

namespace OpenADK.Library.us.Common{

/// <summary>The specific group of duties or responsibilities of position.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class ProgramFundingSource : SifElement
{
	/// <summary>
	/// Creates an instance of a ProgramFundingSource
	/// </summary>
	public ProgramFundingSource() : base ( CommonDTD.PROGRAMFUNDINGSOURCE ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ProgramFundingSource( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <value> The <c>Code</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Funding source for the program, may be more than one."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Code
	{
		get
		{
			return (string) GetSifSimpleFieldValue( CommonDTD.PROGRAMFUNDINGSOURCE_CODE ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.PROGRAMFUNDINGSOURCE_CODE, new SifString( value ), value );
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
			return (OtherCodeList)GetChild( CommonDTD.PROGRAMFUNDINGSOURCE_OTHERCODELIST);
		}
		set
		{
			RemoveChild( CommonDTD.PROGRAMFUNDINGSOURCE_OTHERCODELIST);
			if( value != null)
			{
				AddChild( CommonDTD.PROGRAMFUNDINGSOURCE_OTHERCODELIST, value );
			}
		}
	}

}}
