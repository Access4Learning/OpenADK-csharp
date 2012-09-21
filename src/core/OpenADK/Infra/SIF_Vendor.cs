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

/// <summary>Information about a ZIS vendor</summary>
/// <remarks>
///
/// <para>Author: Generated by adkgen</para>
/// <para>Version: 2.5</para>
/// <para>Since: 1.1</para>
/// </remarks>
[Serializable]
public class SIF_Vendor : SifElement
{
	/// <summary>
	/// Creates an instance of a SIF_Vendor
	/// </summary>
	public SIF_Vendor() : base ( InfraDTD.SIF_VENDOR ){}

	/// <summary>
	/// Constructor that accepts values for all mandatory fields
	/// </summary>
	///<param name="sifName">The name of the company who wrote the ZIS.</param>
	///<param name="sifProduct">The product name assigned by the vendor to identify this ZIS.</param>
	///<param name="sifVersion">The version of the vendor's product--not necessarily the SIF version.</param>
	///
	public SIF_Vendor( string sifName, string sifProduct, string sifVersion ) : base( InfraDTD.SIF_VENDOR )
	{
		this.SIF_Name = sifName;
		this.SIF_Product = sifProduct;
		this.SIF_Version = sifVersion;
	}

	/// <summary>
	/// Constructor used by the .Net Serialization formatter
	/// </summary>
	[SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )] 
	protected SIF_Vendor( SerializationInfo info, StreamingContext context ) : base( info, context ) {} 
	/// <summary>
	/// Gets the metadata fields that make up the key of this object
	/// </summary>
	/// <value>
	/// an array of metadata fields that make up the object's key
	/// </value>
	public  IElementDef[] KeyFields {
		get { return new IElementDef[] { InfraDTD.SIF_VENDOR_SIF_NAME, InfraDTD.SIF_VENDOR_SIF_PRODUCT, InfraDTD.SIF_VENDOR_SIF_VERSION }; }
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Name&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_Name</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The name of the company who wrote the ZIS."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string SIF_Name
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InfraDTD.SIF_VENDOR_SIF_NAME ) ;
		}
		set
		{
			SetFieldValue( InfraDTD.SIF_VENDOR_SIF_NAME, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Product&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_Product</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The product name assigned by the vendor to identify this ZIS."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string SIF_Product
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InfraDTD.SIF_VENDOR_SIF_PRODUCT ) ;
		}
		set
		{
			SetFieldValue( InfraDTD.SIF_VENDOR_SIF_PRODUCT, new SifString( value ), value );
		}
	}

	/// <summary>
	/// Gets or sets the value of the <c>&lt;SIF_Version&gt;</c> element.
	/// </summary>
	/// <value> The <c>SIF_Version</c> element of this object.</value>
	/// <remarks>
	/// <para>The SIF specification defines the meaning of this element as: "The version of the vendor's product--not necessarily the SIF version."</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	public string SIF_Version
	{
		get
		{
			return (string) GetSifSimpleFieldValue( InfraDTD.SIF_VENDOR_SIF_VERSION ) ;
		}
		set
		{
			SetFieldValue( InfraDTD.SIF_VENDOR_SIF_VERSION, new SifString( value ), value );
		}
	}

}}
