// THIS FILE WAS AUTO-GENERATED BY ADKGEN -- DO NOT MODIFY!

//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s).
// All rights reserved.
//

using System;
using OpenADK.Library;

namespace OpenADK.Library.au.Common
{
	///<summary>
	/// Defines the set of values that can be specified whenever an AttendanceCodeType
	/// is used as a parameter to a method or constructor. 
	///</summary>
	/// <remarks>
	/// Alternatively, the static
	///  <see cref="Wrap"/> method can be called to encapsulate any string value in
	///  an AttendanceCodeType object.
	/// <para>Author: Generated by adkgen</para>
	/// <para>Version: 2.5</para>
	/// <para>Since: 2.3</para>
	/// </remarks>
	[Serializable]
	public class AttendanceCodeType : SifEnum
	{
	/// <summary>Refusal ("208")</summary>
	public static readonly AttendanceCodeType C208_REFUSAL = new AttendanceCodeType("208");

	/// <summary>Excursion ("604")</summary>
	public static readonly AttendanceCodeType C604_EXCURSION = new AttendanceCodeType("604");

	/// <summary>Other ("999")</summary>
	public static readonly AttendanceCodeType C999_OTHER = new AttendanceCodeType("999");

	/// <summary>Counselling ("203")</summary>
	public static readonly AttendanceCodeType C203_COUNSELLING = new AttendanceCodeType("203");

	/// <summary>Medical ("200")</summary>
	public static readonly AttendanceCodeType C200_MEDICAL = new AttendanceCodeType("200");

	/// <summary>Sick Bay ("204")</summary>
	public static readonly AttendanceCodeType C204_SICK_BAY = new AttendanceCodeType("204");

	/// <summary>Parental Arrangement ("801")</summary>
	public static readonly AttendanceCodeType C801_PARENTAL_ARRANGEMENT = new AttendanceCodeType("801");

	/// <summary>Exit ("701")</summary>
	public static readonly AttendanceCodeType C701_EXIT = new AttendanceCodeType("701");

	/// <summary>Exempt ("802")</summary>
	public static readonly AttendanceCodeType C802_EXEMPT = new AttendanceCodeType("802");

	/// <summary>Late arrival at School ("111")</summary>
	public static readonly AttendanceCodeType C111_LATE_ARRIVAL_AT_SCHOOL = new AttendanceCodeType("111");

	/// <summary>Late arrival unexplained ("113")</summary>
	public static readonly AttendanceCodeType C113_LATE_ARRIVAL_UNEXPLAINED = new AttendanceCodeType("113");

	/// <summary>Accident ("202")</summary>
	public static readonly AttendanceCodeType C202_ACCIDENT = new AttendanceCodeType("202");

	/// <summary>Medical/Welfare ("210")</summary>
	public static readonly AttendanceCodeType C210_MEDICAL_WELFARE = new AttendanceCodeType("210");

	/// <summary>Parental Acknowledgement ("803")</summary>
	public static readonly AttendanceCodeType C803_PARENTAL_ACKNOWLEDGEMENT = new AttendanceCodeType("803");

	/// <summary>Unexplained ("500")</summary>
	public static readonly AttendanceCodeType C500_UNEXPLAINED = new AttendanceCodeType("500");

	/// <summary>Camp ("606")</summary>
	public static readonly AttendanceCodeType C606_CAMP = new AttendanceCodeType("606");

	/// <summary>Extended Family Holidays ("804")</summary>
	public static readonly AttendanceCodeType C804_EXTENDED_FAMILY_HOLIDAYS = new AttendanceCodeType("804");

	/// <summary>Truancy ("300")</summary>
	public static readonly AttendanceCodeType C300_TRUANCY = new AttendanceCodeType("300");

	/// <summary>Special Event ("605")</summary>
	public static readonly AttendanceCodeType C605_SPECIAL_EVENT = new AttendanceCodeType("605");

	/// <summary>Bereavement ("211")</summary>
	public static readonly AttendanceCodeType C211_BEREAVEMENT = new AttendanceCodeType("211");

	/// <summary>Suspension ("400")</summary>
	public static readonly AttendanceCodeType C400_SUSPENSION = new AttendanceCodeType("400");

	/// <summary>Medical Appointment ("205")</summary>
	public static readonly AttendanceCodeType C205_MEDICAL_APPOINTMENT = new AttendanceCodeType("205");

	/// <summary>Religious/Cultural Observance ("805")</summary>
	public static readonly AttendanceCodeType C805_RELIGIOUS_CULTURAL = new AttendanceCodeType("805");

	/// <summary>Early departure from School ("112")</summary>
	public static readonly AttendanceCodeType C112_EARLY_DEPARTURE_FROM = new AttendanceCodeType("112");

	/// <summary>Parent Choice ("800")</summary>
	public static readonly AttendanceCodeType C800_PARENT_CHOICE = new AttendanceCodeType("800");

	/// <summary>Duty Student ("603")</summary>
	public static readonly AttendanceCodeType C603_DUTY_STUDENT = new AttendanceCodeType("603");

	/// <summary>Educational ("600")</summary>
	public static readonly AttendanceCodeType C600_EDUCATIONAL = new AttendanceCodeType("600");

	/// <summary>Staff Meeting ("904")</summary>
	public static readonly AttendanceCodeType C904_STAFF_MEETING = new AttendanceCodeType("904");

	/// <summary>Late arrival to Class ("116")</summary>
	public static readonly AttendanceCodeType C116_LATE_ARRIVAL_TO_CLASS = new AttendanceCodeType("116");

	/// <summary>Early departure unexplained ("114")</summary>
	public static readonly AttendanceCodeType C114_EARLY_DEPARTURE_UNEXPLAINED = new AttendanceCodeType("114");

	/// <summary>Other Educational Activity ("607")</summary>
	public static readonly AttendanceCodeType C607_OT_EDUCATIONAL_ACTIVITY = new AttendanceCodeType("607");

	/// <summary>Quarantine ("207")</summary>
	public static readonly AttendanceCodeType C207_QUARANTINE = new AttendanceCodeType("207");

	/// <summary>Present ("100")</summary>
	public static readonly AttendanceCodeType C100_PRESENT = new AttendanceCodeType("100");

	/// <summary>Study Leave ("612")</summary>
	public static readonly AttendanceCodeType C612_STUDY_LEAVE = new AttendanceCodeType("612");

	/// <summary>Absent - General ("101")</summary>
	public static readonly AttendanceCodeType C101_ABSENT_GENERAL = new AttendanceCodeType("101");

	/// <summary>School Production ("610")</summary>
	public static readonly AttendanceCodeType C610_SCHOOL_PRODUCTION = new AttendanceCodeType("610");

	/// <summary>Not Marked ("0")</summary>
	public static readonly AttendanceCodeType C0_NOT_MARKED = new AttendanceCodeType("0");

	/// <summary>Sports ("611")</summary>
	public static readonly AttendanceCodeType C611_SPORTS = new AttendanceCodeType("611");

	/// <summary>Community Service ("602")</summary>
	public static readonly AttendanceCodeType C602_COMMUNITY_SERVICE = new AttendanceCodeType("602");

	/// <summary>Off-Site Learning Program (eg. TAFE) ("608")</summary>
	public static readonly AttendanceCodeType C608_OFFSITE_LEARNING_PROGRAM_EG_TAFE = new AttendanceCodeType("608");

	/// <summary>Transferred ("702")</summary>
	public static readonly AttendanceCodeType C702_TRANSFERRED = new AttendanceCodeType("702");

	/// <summary>Illness ("201")</summary>
	public static readonly AttendanceCodeType C201_ILLNESS = new AttendanceCodeType("201");

	/// <summary>Hospitalised ("206")</summary>
	public static readonly AttendanceCodeType C206_HOSPITALISED = new AttendanceCodeType("206");

	/// <summary>Flags ("700")</summary>
	public static readonly AttendanceCodeType C700_FLAGS = new AttendanceCodeType("700");

	/// <summary>Weather ("903")</summary>
	public static readonly AttendanceCodeType C903_WEATHER = new AttendanceCodeType("903");

	/// <summary>Suspension - External ("401")</summary>
	public static readonly AttendanceCodeType C401_SUSPENSION_EXTERNAL = new AttendanceCodeType("401");

	/// <summary>Late Class Unexplained  ("118")</summary>
	public static readonly AttendanceCodeType C118_LATE_CLASS_UNEXPLAINED = new AttendanceCodeType("118");

	/// <summary>Group Activity ("601")</summary>
	public static readonly AttendanceCodeType C601_GROUP_ACTIVITY = new AttendanceCodeType("601");

	/// <summary>Early Class Unexplained ("119")</summary>
	public static readonly AttendanceCodeType C119_EARLY_CLASS_UNEXPLAINED = new AttendanceCodeType("119");

	/// <summary>Industrial Action ("901")</summary>
	public static readonly AttendanceCodeType C901_INDUSTRIAL_ACTION = new AttendanceCodeType("901");

	/// <summary>Work Experience ("609")</summary>
	public static readonly AttendanceCodeType C609_WORK_EXPERIENCE = new AttendanceCodeType("609");

	/// <summary>School Choice ("900")</summary>
	public static readonly AttendanceCodeType C900_SCHOOL_CHOICE = new AttendanceCodeType("900");

	/// <summary>Early leaver from Class ("117")</summary>
	public static readonly AttendanceCodeType C117_EARLY_LEAVER_FROM = new AttendanceCodeType("117");

	/// <summary>Facility Damage ("902")</summary>
	public static readonly AttendanceCodeType C902_FACILITY_DAMAGE = new AttendanceCodeType("902");

	/// <summary>Dentist  ("209")</summary>
	public static readonly AttendanceCodeType C209_DENTIST = new AttendanceCodeType("209");

	///<summary>Wrap an arbitrary string value in an AttendanceCodeType object.</summary>
	///<param name="wrappedValue">The element/attribute value.</param>
	///<remarks>This method does not verify
	///that the value is valid according to the SIF Specification</remarks>
	public static AttendanceCodeType Wrap( String wrappedValue ) {
		return new AttendanceCodeType( wrappedValue );
	}

	private AttendanceCodeType( string enumDefValue ) : base( enumDefValue ) {}
	}
}
