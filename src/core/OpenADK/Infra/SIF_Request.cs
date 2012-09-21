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

namespace OpenADK.Library.Infra{

/// <summary>Contains one of the SIF message types.</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.1</para>
/// </remarks>
[Serializable]
public class SIF_Request : SifMessagePayload
{
	/// <summary>
	/// Creates an instance of a SIF_Request
	/// </summary>
	public SIF_Request() : base ( InfraDTD.SIF_REQUEST ){}
	/// <summary>
	/// Creates an instance of a SIF_Request
	/// </summary>
	///  <param name="sifVersion">The version of SIF to render this message in</param>
	///
	public SIF_Request( SifVersion sifVersion ) : base( sifVersion, InfraDTD.SIF_REQUEST ){}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SIF_Request( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Header&gt;</c> element.
	/// </summary>
	/// <value> A SIF_Header </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Header information associated with this message."</para>
	/// <para>To remove the <c>SIF_Header</c>, set <c>SIF_Header</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public SIF_Header SIF_Header
	{
		get
		{
			return (SIF_Header)GetChild( InfraDTD.SIF_REQUEST_SIF_HEADER);
		}
		set
		{
			RemoveChild( InfraDTD.SIF_REQUEST_SIF_HEADER);
			if( value != null)
			{
				AddChild( InfraDTD.SIF_REQUEST_SIF_HEADER, value );
			}
		}
	}

	/// <summary>Adds a new <c>&lt;SIF_Version&gt;</c> child element.</summary>
	/// <param name="val">A SIF_Version object</param>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "
	///         Specifies which SIF Specification version should be used when returning the response data; wildcards are allowed.
	///         The version specified by SIF_Message/@Version SHOULD be the preferred version
	///         to return if it exists in the list, either explicitly or implicitly via a wildcard.  Otherwise the responding agent
	///         can return data in any version it chooses that matches/satisfies one of the versions in the list.
	///       "</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public void AddSIF_Version( SIF_Version val ) { 
		AddChild( InfraDTD.SIF_REQUEST_SIF_VERSION, val );
	}

	/// <summary>
	/// Removes a <see cref="SIF_Version"/> object instance. More than one instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <param name="Value">Identifies the SIF_Version object to remove by its Value value</param>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public void RemoveSIF_Version( string Value ) { 
		RemoveChild( InfraDTD.SIF_REQUEST_SIF_VERSION, new String[] { Value.ToString() } );
	}

	/// <summary>
	/// Gets a <see cref="SIF_Version"/> object instance. More than one instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <param name="Value">Identifies the SIF_Version object to return by its "Value" attribute value</param>
	/// <returns>A SIF_Version object</returns>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public SIF_Version GetSIF_Version( string Value ) { 
		return (SIF_Version)GetChild( InfraDTD.SIF_REQUEST_SIF_VERSION, new string[] { Value.ToString() } );
	}

	/// <summary>
	/// Gets all SIF_Version object instances. More than once instance can be defined for this object because it is a repeatable field element.
	/// </summary>
	/// <returns>An array of SIF_Version objects</returns>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public SIF_Version[] GetSIF_Versions()
	{
		return GetChildren<SIF_Version>().ToArray();
	}

	/// <summary>
	/// Sets all SIF_Version object instances. All existing 
	/// <c>SIF_Version</c> instances 
	/// are removed and replaced with this list. Calling this method with the 
	/// parameter value set to null removes all <c>SIF_Versions</c>.
	/// </summary>
	/// <remarks>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public void SetSIF_Versions( SIF_Version[] items)
	{
		SetChildren( InfraDTD.SIF_REQUEST_SIF_VERSION, items );
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_MaxBufferSize&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_MaxBufferSize</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "Specifies the maximum size of a response packet to be returned to the requester. The responder may return packets smaller than, or equal to, the maximum value. If the maximum size is too small to contain a single whole response object, the responder should reject the SIF_Request. To guarantee delivery of response packets, requesting agents must not specify a SIF_MaxBufferSize greater than its registered SIF_Register/SIF_MaxBufferSize."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public int? SIF_MaxBufferSize
	{
		get
		{
			return (int?) GetSifSimpleFieldValue( InfraDTD.SIF_REQUEST_SIF_MAXBUFFERSIZE ) ;
		}
		set
		{
			SetFieldValue( InfraDTD.SIF_REQUEST_SIF_MAXBUFFERSIZE, new SifInt( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Query&gt;</c> element.
	/// </summary>
	/// <value> A SIF_Query </value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this null as: "Either SIF_Query or SIF_ExtendedQuery must be specified, which contain the criteria to be used to match response objects."</para>
	/// <para>To remove the <c>SIF_Query</c>, set <c>SIF_Query</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public SIF_Query SIF_Query
	{
		get
		{
			return (SIF_Query)GetChild( InfraDTD.SIF_REQUEST_SIF_QUERY);
		}
		set
		{
			RemoveChild( InfraDTD.SIF_REQUEST_SIF_QUERY);
			if( value != null)
			{
				AddChild( InfraDTD.SIF_REQUEST_SIF_QUERY, value );
			}
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_ExtendedQuery&gt;</c> element.
	/// </summary>
	/// <value> A SIF_ExtendedQuery </value>
	/// <remarks>
	/// <para>To remove the <c>SIF_ExtendedQuery</c>, set <c>SIF_ExtendedQuery</c> to <c>null</c></para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	public SIF_ExtendedQuery SIF_ExtendedQuery
	{
		get
		{
			return (SIF_ExtendedQuery)GetChild( InfraDTD.SIF_REQUEST_SIF_EXTENDEDQUERY);
		}
		set
		{
			RemoveChild( InfraDTD.SIF_REQUEST_SIF_EXTENDEDQUERY);
			if( value != null)
			{
				AddChild( InfraDTD.SIF_REQUEST_SIF_EXTENDEDQUERY, value );
			}
		}
	}

		#region EXTRA METHODS

// BEGIN EXTRA METHODS (C:/dev/OpenADK-java/adk-generator/datadef/core/sif20/SIF_Request.txt.cs)

 /// <summary>
   /// Parses the list of SIF_Version elements and returns an array of SIFVersions
   /// </summary>
   /// <param name="failureLog">The log to write failures to, if any of the SIFVersions fail
   /// to be parsed.</param>
   /// <returns><An array of SIFVersion elements. This will never be null/returns>
    internal SifVersion[] parseRequestVersions( log4net.ILog failureLog )
	{
		if( failureLog == null ){
			failureLog = Adk.Log;
		}
		System.Collections.Generic.List<SifVersion> versionList = new System.Collections.Generic.List<SifVersion>();
       	foreach( SifElement element in GetChildList( InfraDTD.SIF_REQUEST_SIF_VERSION ) ){
			SIF_Version candidate = (SIF_Version)element;
			SifVersion version = null;
			try {
				// Check for "1.*" and "2.*"
				String ver = candidate.Value;
				if( ver != null ){
					if( ver.IndexOf( ".*" ) > 0 ){
						version = SifVersion.GetLatest( int.Parse( ver.Substring( 0, 1 ) ) );
					} else {
						version = SifVersion.Parse( ver );
					}
				}
			} catch( ArgumentException exc ){
                failureLog.Warn( String.Format( "Unable to parse '{0}' from SIF_Request/SIF_Version as SIFVersion.", candidate.Value), exc  );
			}
            catch( FormatException exc )
            {
                failureLog.Warn( String.Format( "Unable to parse '{0}' from SIF_Request/SIF_Version as SIFVersion.", candidate.Value ), exc );
            } 
			if( version != null && !versionList.Contains( version ) ){
				versionList.Add( version );
			}
		}

        return versionList.ToArray();
	}

// END EXTRA METHODS

		#endregion // EXTRA METHODS
}}
