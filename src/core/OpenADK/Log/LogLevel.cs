//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace Edustructures.SifWorks.Log
{
	/// <summary>  Defines the acceptable set of SIF_LogEntry LogLevel codes.
	/// 
	/// </summary>
	/// <version>  1.5
	/// @since 1.5
	/// </version>
	public class LogLevel : SifEnum
	{
		/// <summary>  Identifies an warning log entry ("Warning")</summary>
		public static readonly LogLevel WARNING = new LogLevel( "Warning" );

		/// <summary>  Identifies an informative log entry ("Info")</summary>
		public static readonly LogLevel INFO = new LogLevel( "Info" );

		/// <summary>  Identifies an error log entry ("Error")</summary>
		public static readonly LogLevel ERROR = new LogLevel( "Error" );

		/// <summary>  Wrap an arbitrary string value to assign to the SIF_LogEntry[@LogLevel]
		/// attribute.
		/// </summary>
		/// <param name="val">The attribute value
		/// </param>
		public static LogLevel Wrap( string val )
		{
			return new LogLevel( val );
		}

		private LogLevel( string val ) : base( val )
		{}
	}
}
