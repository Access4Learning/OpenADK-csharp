// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;

namespace OpenADK.Library.uk
{
	/// <summary>
	/// Values identifying each package in the SIF Data Objects library
	/// </summary>
	[Flags]
	public enum SdoLibraryType : int
{
	/// <summary> All SDO libraries </summary>
	All = IntrinsicLibraryType.All,

	/// <summary> No SDO libraries </summary>
	None = IntrinsicLibraryType.None,

	//  These are always loaded regardless of what the user specifies.
	//  They are considered "built-in" SDO libraries but under the hood they're 
	//  treated just like any other SDO package.
	/// <summary>Identifies the Infrastructure Sdo library</summary>
	Global = IntrinsicLibraryType.Global,

	/// <summary>Identifies the Infrastructure Sdo library</summary>
	Infra = IntrinsicLibraryType.Infra,

	/// <summary>Identifies the Infrastructure Sdo library</summary>
	Common = IntrinsicLibraryType.Common,

	/// <summary>Identifies the Catering Sdo library</summary>
	Catering = 0x000001,

	/// <summary>Identifies the null Sdo library</summary>
	Assessment = 0x000002,

	/// <summary>Identifies the null Sdo library</summary>
	Workforce = 0x000004,

	/// <summary>Identifies the null Sdo library</summary>
	School = 0x000008,

	/// <summary>Identifies the null Sdo library</summary>
	Learner = 0x0000010,

	/// <summary>Identifies the Learning Sdo library</summary>
	Learning = 0x0000020,

	/// <summary>Identifies the null Sdo library</summary>
	Reporting = 0x0000040,

	/// <summary>Identifies the Data Model Sdo library</summary>
	Datamodel = 0x0000080,

	/// <summary>Identifies the Infrastructure Sdo library</summary>
	Infrastructure = 0x00000100,

}}
