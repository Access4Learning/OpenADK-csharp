// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Hrfin
{
	///<summary>
	/// Defines the set of values that can be specified whenever a FederalTaxIdCode
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a FederalTaxIdCode object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.5r1</para>
	/// </remarks>
	[Serializable]
	public class FederalTaxIdCode : SifEnum
	{
	/// <summary>OTHER ("OTHER")</summary>
	public static readonly FederalTaxIdCode OTHER = new FederalTaxIdCode("OTHER");

	/// <summary>EIN ("EIN")</summary>
	public static readonly FederalTaxIdCode EIN = new FederalTaxIdCode("EIN");

	/// <summary>SSNO ("SSNO")</summary>
	public static readonly FederalTaxIdCode SSNO = new FederalTaxIdCode("SSNO");

	/// <summary>ITIN ("ITIN")</summary>
	public static readonly FederalTaxIdCode ITIN = new FederalTaxIdCode("ITIN");

	/// <summary>ATIN ("ATIN")</summary>
	public static readonly FederalTaxIdCode ATIN = new FederalTaxIdCode("ATIN");

	///<summary>Wrap an arbitrary string value in a FederalTaxIdCode object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static FederalTaxIdCode Wrap( String wrappedValue ) {
		return new FederalTaxIdCode( wrappedValue );
	}

	private FederalTaxIdCode( string enumDefValue ) : base( enumDefValue ) {}
	}
}
