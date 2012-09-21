// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.us.Common
{
	///<summary>
	/// Defines the set of values that can be specified whenever a PhoneNumberType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  a PhoneNumberType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 1.1</para>
	/// </remarks>
	[Serializable]
	public class PhoneNumberType : SifEnum
	{
	/// <summary>Alternate telephone number ("0350")</summary>
	public static readonly PhoneNumberType ALT = new PhoneNumberType("0350");

	/// <summary>Other residential facsimile number ("08")</summary>
	public static readonly PhoneNumberType SIF1x_OTHER_RES_FAX = new PhoneNumberType("08");

	/// <summary>Telephone extension ("04")</summary>
	public static readonly PhoneNumberType SIF1x_EXT = new PhoneNumberType("04");

	/// <summary>Home telephone number ("06")</summary>
	public static readonly PhoneNumberType SIF1x_HOME_PHONE = new PhoneNumberType("06");

	/// <summary>Other residential telephone number ("09")</summary>
	public static readonly PhoneNumberType SIF1x_OTHER_RES_PHONE = new PhoneNumberType("09");

	/// <summary>Media conferencing number ("0486")</summary>
	public static readonly PhoneNumberType MEDIA_CONFERENCE = new PhoneNumberType("0486");

	/// <summary>Work facsimile number ("17")</summary>
	public static readonly PhoneNumberType SIF1x_WORK_FAX = new PhoneNumberType("17");

	/// <summary>Beeper number ("0370")</summary>
	public static readonly PhoneNumberType BEEPER = new PhoneNumberType("0370");

	/// <summary>Work telephone number ("18")</summary>
	public static readonly PhoneNumberType SIF1x_WORK_PHONE = new PhoneNumberType("18");

	/// <summary>Home facsimile number ("05")</summary>
	public static readonly PhoneNumberType SIF1x_HOME_FAX = new PhoneNumberType("05");

	/// <summary>Work cellular number ("16")</summary>
	public static readonly PhoneNumberType SIF1x_WORK_CELL = new PhoneNumberType("16");

	/// <summary>Telemail number ("14")</summary>
	public static readonly PhoneNumberType SIF1x_TELEMAIL = new PhoneNumberType("14");

	/// <summary>Appointment telephone number ("10")</summary>
	public static readonly PhoneNumberType SIF1x_APPT_NUMBER = new PhoneNumberType("10");

	/// <summary>Instant messaging number ("0478")</summary>
	public static readonly PhoneNumberType INSTANT_MESSAGING = new PhoneNumberType("0478");

	/// <summary>Personal cellular number ("11")</summary>
	public static readonly PhoneNumberType SIF1x_PERSONAL_CELL = new PhoneNumberType("11");

	/// <summary>Personal telephone number ("12")</summary>
	public static readonly PhoneNumberType SIF1x_PERSONAL_PHONE = new PhoneNumberType("12");

	/// <summary>Main telephone number ("0096")</summary>
	public static readonly PhoneNumberType PRIMARY = new PhoneNumberType("0096");

	/// <summary>Answering service ("0359")</summary>
	public static readonly PhoneNumberType ANSWERING_SERVICE = new PhoneNumberType("0359");

	/// <summary>Night telephone number ("07")</summary>
	public static readonly PhoneNumberType SIF1x_NIGHT_PHONE = new PhoneNumberType("07");

	/// <summary>Voicemail ("15")</summary>
	public static readonly PhoneNumberType SIF1x_VOICEMAIL = new PhoneNumberType("15");

	/// <summary>Telex number ("0426")</summary>
	public static readonly PhoneNumberType TELEX = new PhoneNumberType("0426");

	/// <summary>Telex number ("13")</summary>
	public static readonly PhoneNumberType SIF1x_TELEX = new PhoneNumberType("13");

	/// <summary>Alternate telephone number ("01")</summary>
	public static readonly PhoneNumberType SIF1x_ALT = new PhoneNumberType("01");

	/// <summary>Voice mail ("0448")</summary>
	public static readonly PhoneNumberType VOICE_MAIL = new PhoneNumberType("0448");

	/// <summary>Telemail ("0437")</summary>
	public static readonly PhoneNumberType TELEMAIL = new PhoneNumberType("0437");

	/// <summary>Answering service ("02")</summary>
	public static readonly PhoneNumberType SIF1x_ANSWERING_SERVICE = new PhoneNumberType("02");

	/// <summary>Facsimile number ("2364")</summary>
	public static readonly PhoneNumberType FAX = new PhoneNumberType("2364");

	/// <summary>Appointment telephone number ("0400")</summary>
	public static readonly PhoneNumberType APPOINTMENT = new PhoneNumberType("0400");

	/// <summary>Beeper number ("03")</summary>
	public static readonly PhoneNumberType SIF1x_BEEPER = new PhoneNumberType("03");

	///<summary>Wrap an arbitrary string value in a PhoneNumberType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static PhoneNumberType Wrap( String wrappedValue ) {
		return new PhoneNumberType( wrappedValue );
	}

	private PhoneNumberType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
