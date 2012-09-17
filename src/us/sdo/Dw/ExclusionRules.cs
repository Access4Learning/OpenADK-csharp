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

namespace OpenADK.Library.us.Dw{

/// <summary>An ExclusionRules</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class ExclusionRules : SifKeyedList<ExclusionRule>
{
	/// <summary>
	/// Creates an instance of an ExclusionRules
	/// </summary>
	public ExclusionRules() : base ( DwDTD.EXCLUSIONRULES ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="exclusionRule">Rule for which statistic may not be reported</param>
	///
	public ExclusionRules( ExclusionRule exclusionRule ) : base( DwDTD.EXCLUSIONRULES )
	{
		this.SafeAddChild( DwDTD.EXCLUSIONRULES_EXCLUSIONRULE, exclusionRule );
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ExclusionRules( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	///<summary>Adds the value of the <c>&lt;ExclusionRule&gt;</c> element.</summary>
	/// <param name="Type">Values: Sample, Size, Description</param>
	/// <param name="Value">Gets or sets the content value of the &amp;lt;ExclusionRule&amp;gt; element</param>
	///<remarks>
	/// <para>This form of <c>setExclusionRule</c> is provided as a convenience method
	/// that is functionally equivalent to the method <c>AddExclusionRule</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public void AddExclusionRule( ExclusionRuleType Type, string Value ) {
		AddChild( DwDTD.EXCLUSIONRULES_EXCLUSIONRULE, new ExclusionRule( Type, Value ) );
	}

}}
