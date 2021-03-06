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
using OpenADK.Library.uk.Common;
using OpenADK.Library.Infra;

namespace OpenADK.Library.uk.Reporting{

/// <summary>This object is here only as necessary for the ADK, but not supported in the UK data model</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.6</para>
/// <para>Since: 2.0</para>
/// </remarks>
[Serializable]
public class ReportData : SifElement
{
	/// <summary>
	/// Creates an instance of a ReportData
	/// </summary>
	public ReportData() : base ( ReportingDTD.REPORTDATA ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected ReportData( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
}}
