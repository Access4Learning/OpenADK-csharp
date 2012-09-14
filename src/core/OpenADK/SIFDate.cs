//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  A date value in SIF format
    /// 
    /// To use this class, pass a date string to the constructor or set methods in
    /// the format specified by SIF ("YYYYmmdd"). Alternatively, you may pass a
    /// <c>DateTime</c>instance. The
    /// <c>ToDate</c> returns the SIF date as a DateTime instance.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    [Serializable]
    public class SifDate : AdkDataType<DateTime?>
    {
        /// <summary>  Constructs a SifDate object from a DateTime object</summary>
        /// <param name="date">The date
        /// </param>
        public SifDate( DateTime? date )
            : base( date ) {}

        /// <summary>
        /// Creates an instance of SifDate with the specified SIF 1.x date string value (deprecated)
        /// </summary>
        /// <remarks>This constructor has been retained for backwards compatibility with the 
        /// Library 1.5 ADK.</remarks>
        /// <param name="yyyyMMdd"></param>
        [
            Obsolete(
                "This form of the constructor is retained for backwards compatibility with the Library 1.5 ADK. Please use the constructor overload that takes a DateTime"
                , false )]
        public SifDate( string yyyyMMdd )
            : base( ParseSifDateString( yyyyMMdd, SifVersion.SIF15r1 ) ) {}


        protected override SifTypeConverter<DateTime?> GetTypeConverter()
        {
            return SifTypeConverters.DATE;
        }

        public DateTime? ToDate()
        {
            return this.Value;
        }


        /// <summary>
        /// Parse a SIFDate string, using the appropriate format for the specified version of SIF
        /// </summary>
        /// <param name="sifDate">The string XML value to parse into a date</param>
        /// <param name="version">The SifVersion to us for parsing the string</param>
        /// <returns>The parsed DateTime</returns>
        /// <exception cref="AdkTypeParseException">if the date cannot be parsed</exception>
        public static DateTime? ParseSifDateString( string sifDate,
                                                    SifVersion version )
        {
            if ( version.CompareTo( SifVersion.SIF20 ) >= 0 ) {
                return OpenADK.Library.us.SifDtd.SIF_2X_FORMATTER.ToDate( sifDate ); //Todo: SifDtd should have base class and move non-language related material out
            }
            else {
                return OpenADK.Library.us.SifDtd.SIF_1X_FORMATTER.ToDate(sifDate);//Todo: SifDtd should have base class and move non-language related material out
            }
        }
    }
}
