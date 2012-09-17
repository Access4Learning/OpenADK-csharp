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

/// <summary>A ProgramAvailability</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class ProgramAvailability : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a ProgramAvailability
	/// </summary>
	public ProgramAvailability() : base ( ProgramsDTD.PROGRAMAVAILABILITY ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="code">Describes the availability of the program.</param>
	///
	public ProgramAvailability( ProgramAvailabilityCode code ) : base( ProgramsDTD.PROGRAMAVAILABILITY )
	{
		this.SetCode( code );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ProgramAvailability( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { ProgramsDTD.PROGRAMAVAILABILITY_CODE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <value> The <c>Code</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Describes the availability of the program."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public string Code
	{
		get
		{ 
			return GetFieldValue( ProgramsDTD.PROGRAMAVAILABILITY_CODE );
		}
		set
		{
			SetField( ProgramsDTD.PROGRAMAVAILABILITY_CODE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>&lt;Code&gt;</c> element.
	/// </summary>
	/// <param name="val">A ProgramAvailabilityCode object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Describes the availability of the program."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void SetCode( ProgramAvailabilityCode val )
	{
		SetField( ProgramsDTD.PROGRAMAVAILABILITY_CODE, val );
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
			return (OtherCodeList)GetChild( ProgramsDTD.PROGRAMAVAILABILITY_OTHERCODELIST);
		}
		set
		{
			RemoveChild( ProgramsDTD.PROGRAMAVAILABILITY_OTHERCODELIST);
			if( value != null)
			{
				AddChild( ProgramsDTD.PROGRAMAVAILABILITY_OTHERCODELIST, value );
			}
		}
	}

}}
