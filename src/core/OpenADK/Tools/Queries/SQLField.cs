//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Data;

namespace OpenADK.Library.Tools.Queries
{
    /// <summary>  Encapsulates a field name and type from a <see cref="System.Data.DbType"/> </summary>
    /// <remarks>
    /// 
    /// When preparing a dictionary to be passed to the <c>SQLQueryFormatter.format</c>
    /// method, the caller must map ElementDef constants to instances of SQLField.
    /// The <see cref="System.Data.DbType"/> enum value is used to render the field according to its data
    /// type (e.g. strings are quoted with a single quote, numeric fields are rendered
    /// as-is, etc.)
    /// </remarks>
    /// <example>
    /// 
    /// For example,
    /// 
    /// <code>
    /// IDictionary m = new Hashtable();
    /// m[ SIFDTD.STUDENTPERSONAL_REFID ] =
    ///	new SQLField( "Students.Foreign_ID", DbType.String ) ;
    /// m[ SIFDTD.NAME_LASTNAME ],
    ///	new SQLField( "Students.Last_Name",  DbType.String ) ;
    /// m[ SIFDTD.NAME_FIRSTNAME ],
    ///	new SQLField( "First_Name", DbType.String ) ;
    /// m[ SIFDTD.DEMOGRAPHICS_CITIZENSHIPSTATUS ],
    ///	new SQLField( "Students.US_Citizen_Bool{04=1,=0}", DbType.Int32 ) ;
    /// </code>
    /// 
    /// 
    /// The above example might result in a string such as "( Students.US_Citizen_Bool = 0 )"
    /// or "( Students.Foreign_ID = '898' ) OR ( Students.Last_Name = 'Cortez' AND First_Name = 'Robert' )"
    /// </example>
    public class SQLField
    {
        private readonly Dialect fDialect;
        private readonly string fName;
        private readonly DbType fDbType;

        public string Name
        {
            get { return fName; }
        }

        public DbType DbType
        {
            get { return fDbType; }
        }

        /// <summary>  Constructor</summary>
        /// <param name="name">The application-defined field name
        /// </param>
        /// <param name="type">A constant from the System.Data.DbType enumeration. The type is used
        /// by SQLQueryBuilder to property format the field value
        /// </param>
        public SQLField( string name,
                         DbType type )
        {
            fName = name;
            fDbType = type;
            fDialect = SqlDialect.DEFAULT;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="dialect"></param>
        public SQLField(string name,
                 DbType type, Dialect dialect)
        {
            fName = name;
            fDbType = type;
            fDialect = dialect;
        }

        /// <summary>  Render a field value given the System.Data.DbType enum value passed to the constructor</summary>
        public virtual string Render( string fieldValue )
        {
            switch ( fDbType ) {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return RenderString( fieldValue );

                case DbType.Date:
                case DbType.DateTime:
                    return RenderDate( fieldValue );

                case DbType.Time:
                    return RenderTime( fieldValue );

                case DbType.Guid:
                    return RenderGuid( fieldValue );

                default:
                    return fieldValue;
            }
        }

        /// <summary>  Render a field value as a string</summary>
        public virtual string RenderString( string fieldValue )
        {
            return fDialect.RenderString( fieldValue );
        }

        /// <summary>  Render a field value as a number</summary>
        public virtual string RenderNumeric( string fieldValue )
        {
            return fDialect.RenderNumeric( fieldValue );
        }

        /// <summary>  Render a field value as a date</summary>
        public virtual string RenderDate( string fieldValue )
        {
            return fDialect.RenderDate( fieldValue );
        }

        /// <summary>  Render a field value as a guid</summary>
        public virtual string RenderGuid( string fieldValue )
        {
            return fDialect.RenderGuid( fieldValue );
        }


        /// <summary>  Render a field value as a time</summary>
        public virtual string RenderTime( string fieldValue )
        {
            return fDialect.RenderTime( fieldValue );
        }
    }
}
