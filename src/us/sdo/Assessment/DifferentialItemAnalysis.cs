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

/// <summary>A DifferentialItemAnalysis</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.3</para>
/// </remarks>
[Serializable]
public class DifferentialItemAnalysis : SifElement
{
	/// <summary>
	/// Creates an instance of a DifferentialItemAnalysis
	/// </summary>
	public DifferentialItemAnalysis() : base ( AssessmentDTD.DIFFERENTIALITEMANALYSIS ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected DifferentialItemAnalysis( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;CMH&gt;</c> element.
	/// </summary>
	/// <value> The <c>CMH</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Cochran-Mantel-Haenszel statistic."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public decimal? CMH
	{
		get
		{
			return (decimal?) GetSifSimpleFieldValue( AssessmentDTD.DIFFERENTIALITEMANALYSIS_CMH ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.DIFFERENTIALITEMANALYSIS_CMH, new SifDecimal( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;MH&gt;</c> element.
	/// </summary>
	/// <value> The <c>MH</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Mantel-Haenszel statistic."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	public decimal? MH
	{
		get
		{
			return (decimal?) GetSifSimpleFieldValue( AssessmentDTD.DIFFERENTIALITEMANALYSIS_MH ) ;
		}
		set
		{
			SetFieldValue( AssessmentDTD.DIFFERENTIALITEMANALYSIS_MH, new SifDecimal( value ), value );
		}
	}

}}
