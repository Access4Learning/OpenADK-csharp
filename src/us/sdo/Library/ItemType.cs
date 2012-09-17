// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Library
{
	///<summary>
	/// Defines the set of values that can be specified whenever an ItemType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an ItemType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	[Serializable]
	public class ItemType : SifEnum
	{
	/// <summary>LibraryMaterial ("LibraryMaterial")</summary>
	public static readonly ItemType LIBRARYMATERIAL = new ItemType("LibraryMaterial");

	/// <summary>Textbook ("Textbook")</summary>
	public static readonly ItemType TEXTBOOK = new ItemType("Textbook");

	/// <summary>Asset ("Asset")</summary>
	public static readonly ItemType ASSET = new ItemType("Asset");

	/// <summary>Media ("Media")</summary>
	public static readonly ItemType MEDIA = new ItemType("Media");

	///<summary>Wrap an arbitrary string value in an ItemType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static ItemType Wrap( String wrappedValue ) {
		return new ItemType( wrappedValue );
	}

	private ItemType( string enumDefValue ) : base( enumDefValue ) {}
	}
}