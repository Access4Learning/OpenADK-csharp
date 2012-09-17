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

namespace OpenADK.Library.au.Programs{

/// <summary>A ProgramStatus</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.4</para>
/// </remarks>
[Serializable]
public class ProgramStatus : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a ProgramStatus
	/// </summary>
	public ProgramStatus() : base ( ProgramsDTD.PROGRAMSTATUS ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="code">A Code</param>
	///
	public ProgramStatus( AUCodeSets0792IdentificationProcedureType code ) : base( ProgramsDTD.PROGRAMSTATUS )
	{
		this.SetCode( code );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ProgramStatus( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { ProgramsDTD.PROGRAMSTATUS_CODE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <value> The <c>Code</c> element of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public string Code
	{
		get
		{ 
			return GetFieldValue( ProgramsDTD.PROGRAMSTATUS_CODE );
		}
		set
		{
			SetField( ProgramsDTD.PROGRAMSTATUS_CODE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <param name="val">A AUCodeSets0792IdentificationProcedureType object</param>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public void SetCode( AUCodeSets0792IdentificationProcedureType val )
	{
		SetField( ProgramsDTD.PROGRAMSTATUS_CODE, val );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;OtherCodeList&gt;</c> element.
	/// </summary>
	/// <value> An OtherCodeList </value>
	/// <remarks>
	/// <para>To remove the <c>OtherCodeList</c>, set <c>OtherCodeList</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.4</para>
	/// </remarks>
	public OtherCodeList OtherCodeList
	{
		get
		{
			return (OtherCodeList)GetChild( ProgramsDTD.PROGRAMSTATUS_OTHERCODELIST);
		}
		set
		{
			RemoveChild( ProgramsDTD.PROGRAMSTATUS_OTHERCODELIST);
			if( value != null)
			{
				AddChild( ProgramsDTD.PROGRAMSTATUS_OTHERCODELIST, value );
			}
		}
	}

}}
