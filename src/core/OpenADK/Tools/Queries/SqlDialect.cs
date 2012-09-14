//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library.Tools.Queries
{
    public class SqlDialect : Dialect
    {
        /// <summary>
        /// The default SQL Dialect 
        /// </summary>
        public static Dialect DEFAULT = new SqlDialect( '\'' );

        /// <summary>
        /// A SQL Dialect with support for Microsoft Access 
        /// </summary>
        public static Dialect MS_ACCESS = new SqlDialect( '"' );

        protected SqlDialect( char quoteCharacter ) : base( quoteCharacter ) { }

    }
}
