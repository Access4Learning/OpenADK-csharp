//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library.Tools.Queries
{
    public abstract class Dialect
    {
        private char fQuoteCharacter;
        protected Dialect( char quoteCharacter)
        {
            fQuoteCharacter = quoteCharacter;
        }


        ///
        ///<summary> Render a field value as a string</summary>
        ///
        public String RenderString(String value)
        {
            if (value.IndexOf( fQuoteCharacter ) > -1)
            {
                // Escape any appostrophes
                value = value.Replace( fQuoteCharacter.ToString(), new String( new char[] {fQuoteCharacter, fQuoteCharacter }));
            }
            return fQuoteCharacter + value + fQuoteCharacter;
        }

        ///
        ///<summary> Render a field value as a number</summary>
        ///
        public String RenderNumeric(String value)
        {
            return value;
        }

        ///
        ///<summary> Render a field value as a date</summary>
        ///
        public String RenderDate(String value)
        {
            return value;
        }

        ///
        ///<summary> Render a field value as a time</summary>
        ///
        public String RenderTime(String value)
        {
            return value;
        }

        /// <summary>
        /// Renders a SIFRefId as a Guid datatype
        /// </summary>
        /// <param name="sifRefId"></param>
        /// <returns></returns>
        public String RenderGuid( String sifRefId )
        {
            Guid? g = SifFormatter.SifRefIDToGuid( sifRefId );
            return fQuoteCharacter + g.Value.ToString( "B" ) + fQuoteCharacter;
        }
	
    }
}
