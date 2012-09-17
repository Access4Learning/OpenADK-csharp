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

namespace OpenADK.Library.us.Hrfin{

/// <summary>The specific group of duties or responsibilities of position.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.5r1</para>
/// </remarks>
[Serializable]
public class JobClassification : SifKeyedElement
{
	/// <summary>
	/// Creates an instance of a JobClassification
	/// </summary>
	public JobClassification() : base ( HrfinDTD.JOBCLASSIFICATION ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="code">A Code</param>
	///
	public JobClassification( JobClassificationCode code ) : base( HrfinDTD.JOBCLASSIFICATION )
	{
		this.SetCode( code );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected JobClassification( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public override IElementDef[] KeyFields {
		get { return new IElementDef[] { HrfinDTD.JOBCLASSIFICATION_CODE }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>Code</c> attribute.
	/// </summary>
	/// <value> The <c>Code</c> attribute of this object.</value>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public string Code
	{
		get
		{ 
			return GetFieldValue( HrfinDTD.JOBCLASSIFICATION_CODE );
		}
		set
		{
			SetField( HrfinDTD.JOBCLASSIFICATION_CODE, value );
		}
	}

	/// <summary>
	/// Sets the value of the <c>Code</c> attribute.
	/// </summary>
	/// <param name="val">A JobClassificationCode object</param>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	public void SetCode( JobClassificationCode val )
	{
		SetField( HrfinDTD.JOBCLASSIFICATION_CODE, val );
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
			return (OtherCodeList)GetChild( HrfinDTD.JOBCLASSIFICATION_OTHERCODELIST);
		}
		set
		{
			RemoveChild( HrfinDTD.JOBCLASSIFICATION_OTHERCODELIST);
			if( value != null)
			{
				AddChild( HrfinDTD.JOBCLASSIFICATION_OTHERCODELIST, value );
			}
		}
	}

}}
