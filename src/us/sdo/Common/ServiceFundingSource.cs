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

/// <summary>A ServiceFundingSource</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class ServiceFundingSource : SifElement
{
	/// <summary>
	/// Creates an instance of a ServiceFundingSource
	/// </summary>
	public ServiceFundingSource() : base ( CommonDTD.SERVICEFUNDINGSOURCE ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ServiceFundingSource( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <value> The <c>Code</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Funding source for the program, may be more than one."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public string Code
	{
		get
		{
			return (string) GetSifSimpleFieldValue( CommonDTD.SERVICEFUNDINGSOURCE_CODE ) ;
		}
		set
		{
			SetFieldValue( CommonDTD.SERVICEFUNDINGSOURCE_CODE, new SifString( value ), value );
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
			return (OtherCodeList)GetChild( CommonDTD.SERVICEFUNDINGSOURCE_OTHERCODELIST);
		}
		set
		{
			RemoveChild( CommonDTD.SERVICEFUNDINGSOURCE_OTHERCODELIST);
			if( value != null)
			{
				AddChild( CommonDTD.SERVICEFUNDINGSOURCE_OTHERCODELIST, value );
			}
		}
	}

}}
