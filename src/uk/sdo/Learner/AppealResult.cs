// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.uk.Learner
{
	///<summary>
	/// Defines the set of values that can be specified whenever an AppealResult
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an AppealResult object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.0</para>
	/// </remarks>
	[Serializable]
	public class AppealResult : SifEnum
	{
	/// <summary>Reinstatement ("R")</summary>
	public static readonly AppealResult REINSTATEMENT = new AppealResult("R");

	/// <summary>Reinstatement would be appropriate but not in best interest of learner given other circumstances ("O")</summary>
	public static readonly AppealResult NOT_BEST_INTEREST = new AppealResult("O");

	/// <summary>Exclusion stands ("E")</summary>
	public static readonly AppealResult EXCLUSION_STANDS = new AppealResult("E");

	///<summary>Wrap an arbitrary string value in an AppealResult object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static AppealResult Wrap( String wrappedValue ) {
		return new AppealResult( wrappedValue );
	}

	private AppealResult( string enumDefValue ) : base( enumDefValue ) {}
	}
}
