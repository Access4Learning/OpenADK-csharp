//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using OpenADK.Library.Tools.Mapping;
using System.Collections;

namespace OpenADK.Library.Tools.Queries
{
    /// <summary>  An implementation of a QueryFormatter that formats queries for inclusion in
    /// an SQL <c>WHERE</c> clause.</summary>
    /// <remarks>
    /// 
    /// When preparing a dictionary to be passed to the <c>SQLQueryFormatter.format</c>
    /// method, the caller must map ElementDef constants to instances of the <c>SQLField</c>
    /// class. The constructor to that class requires two parameters: the application-
    /// defined name of the field, and a constant from the <see cref="System.Data.DbType"/> class,
    /// which instructs the SQLQueryFormatter how to render the field value in the
    /// query string (e.g. strings are quoted with a single quote, numeric fields
    /// are rendered as-is, etc.)
    /// </remarks>
    /// <example>
    /// For example,
    /// 
    /// <code>
    /// IDictionary m = new Hashtable();
    /// m[ SIFDTD.STUDENTPERSONAL_REFID ] =
    ///		new SQLField( "Students.Foreign_ID", DbType.String ) ;
    /// m[ SIFDTD.NAME_LASTNAME ],
    ///		new SQLField( "Students.Last_Name",  DbType.String ) ;
    /// m[ SIFDTD.NAME_FIRSTNAME ],
    ///		new SQLField( "First_Name", DbType.String ) ;
    /// m[ SIFDTD.DEMOGRAPHICS_CITIZENSHIPSTATUS ],
    ///		new SQLField( "Students.US_Citizen_Bool{04=1,=0}", DbType.Int32 ) ;
    /// </code>
    /// 
    /// The above example might result in a string such as "( Students.US_Citizen_Bool = 0 )"
    /// or "( Students.Foreign_ID = '898' ) OR ( Students.Last_Name = 'Cortez' AND First_Name = 'Robert' )"
    /// </example>
    public class SQLQueryFormatter : QueryFormatter
    {
        private IDictionary fFields;

        /// <summary>  Return the text that should be inserted for an opening brace</summary>
        protected internal override string OpenBrace
        {
            get { return "( "; }
        }

        /// <summary>  Return the text that should be inserted for a closing brace</summary>
        protected internal override string CloseBrace
        {
            get { return " )"; }
        }


        /// <summary>  Return the text that should be inserted for a logical <c>AND</c> operation</summary>
        public override string GetOperator( ComparisonOperators op )
        {
            switch (op)
            {
                case ComparisonOperators.EQ:
                    return " = ";
                case ComparisonOperators.NE:
                    return " != ";
                case ComparisonOperators.GT:
                    return " > ";
                case ComparisonOperators.LT:
                    return " < ";
                case ComparisonOperators.GE:
                    return " >= ";
                case ComparisonOperators.LE:
                    return " <= ";
            }

            return "";
        }

        public override string GetOperator( GroupOperator op )
        {
            switch ( op ) {
                case GroupOperator.And:
                    return " AND ";

                case GroupOperator.Or:
                    return " OR ";
            }
            return "";
        }


            public String Format( Query query, bool isExplicit )
    {
        if ( fFields == null && query.HasConditions )
        {
            throw new QueryFormatterException( "Agent is not configured to respond to query conditions" );
        }
        return Format(query, fFields, isExplicit);
    }


        /// <summary>  Return the text for a field name</summary>
        /// <param name="field">The field
        /// </param>
        /// <param name="def">The corresponding field definition from the Map passed to
        /// the <c>format</c> method
        /// </param>
        /// <returns> The implementation returns the field name in whatever form is
        /// appropriate to the implementation, using the supplied <i>def</i>
        /// Object if necessary to obtain additional field information.
        /// </returns>
        public override string RenderField( IElementDef field,
                                            Object def )
        {
            try {
                return ExtractFieldName( ((SQLField) def).Name );
            }
            catch ( InvalidCastException cce ) {
                throw new QueryFormatterException
                    ( "SQLQueryFormatter requires that the Map passed to the format method consist of SQLField instances (not " +
                      cce.Message + " instances)" );
            }
        }

        /// <summary>  Return the text for a field value</summary>
        /// <param name="valu">The field value</param>
        /// <param name="def">The corresponding field definition from the Map passed to
        /// the <c>format</c> method
        /// </param>
        /// <returns> The implementation returns the field value in whatever form is
        /// appropriate to the implementation, using the supplied <i>def</i>
        /// Object if necessary to obtain additional field information
        /// </returns>
        public override string RenderValue(string valu,
                                            Object def)
        {
            try
            {
                //  The elements in the Map are expected to be SQLField instances
                SQLField f = (SQLField) def;
                return f.Render( DoValueSubstitution( valu, f.Name ) );
            }
            catch ( InvalidCastException cce )
            {
                throw new QueryFormatterException
                    ( "SQLQueryFormatter requires that the Map passed to the format method consist of SQLField instances (not " +
                      cce.Message + " instances)", cce );
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="def"></param>
        /// <param name="field"></param>
        public void AddField(IElementDef def, SQLField field)
        {
            if (fFields == null)
            {
                fFields = new Dictionary<Object, Object>();
            }
            fFields[def] = field;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="field"></param>
        public void AddField(String xPath, SQLField field)
        {
            if (fFields == null)
            {
                fFields = new Dictionary<Object, Object>();
            }
            fFields[xPath] = field;
        }


        /// <summary>
        ///   Adds an SQLField to use for rendering an SQL Where clause using
        /// the <see cref="SQLQueryFormatter#Query"/> method.
        /// </summary>
        /// <param name="def"> The ElementDef that is represented by the field</param>
        /// <param name="field">The SQL representation of the field</param>
        public void AddField(IElementDef def, IQueryField field)
        {
            if (fFields == null)
            {
                fFields = new Dictionary<Object, Object>();
            }
            fFields[def] = field;
        }



        /// <summary>
        /// Adds a QueryField to use for rendering an SQL Where clause
        /// </summary>
        /// <param name="xPath"></param>
        /// <param name="field"></param>
        public void AddField(String xPath, IQueryField field)
        {
            if (fFields == null)
            {
                fFields = new Dictionary<Object, Object>();
            }
            fFields[xPath] = field;
        }

        /// <summary>
        /// Adds SQLFields to represent each field rule in the specified MappingsContext.
        /// </summary>
        /// <param name="context"></param>
        public void AddFields(MappingsContext context)
        {
            AddFields(context, SqlDialect.DEFAULT);
        }


    

        /// <summary>
        /// Adds SQLFields to represent each field rule in the specified MappingsContext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dialect"></param>
        public void AddFields( MappingsContext context, Dialect dialect )
        {
            foreach ( FieldMapping mapping in context.FieldMappings )
            {
                Rule rule = mapping.fRule;
                if ( rule is XPathRule )
                {
                    TypeConverter converter = null;
                    XPathRule xRule = (XPathRule) rule;
                    IElementDef targetDef = xRule.LookupTargetDef( context.ObjectDef );
                    if ( targetDef != null )
                    {
                        converter = targetDef.TypeConverter;
                    }
                    if ( converter == null )
                    {
                        converter = SifTypeConverters.STRING;
                    }
                    if ( mapping.ValueSetID != null )
                    {
                        ValueSet vs = context.Mappings.GetValueSet( mapping.ValueSetID, true );
                        if ( vs != null )
                        {
    					    //Create the lookup table for generating the SQL lookup
                            StringBuilder buffer = new StringBuilder();
                            // e.g. CircRecord.PatronType{StudentPersonal=1;StaffPersonal=2}
                            //buffer.Append( tablePrefix );
                            buffer.Append( mapping.FieldName );
                            buffer.Append( '{' );
                            ValueSetEntry[] entries = vs.Entries;
                            for ( int a = 0; a < entries.Length; a++ )
                            {
                                if ( a > 0 )
                                {
                                    buffer.Append( ';' );
                                }
                                buffer.Append( entries[a].Value );
                                buffer.Append( '=' );
                                ;
                                buffer.Append( entries[a].Name );
                            }
                            buffer.Append( '}' );
                            SQLField f = new SQLField( buffer.ToString(), converter.DbType, dialect );
                            AddField( xRule.XPath, f );
                            continue;
                        }
                    }

                    SQLField field = new SQLField( mapping.FieldName, converter.DbType, dialect );
                    AddField( xRule.XPath, field );
                }
            }
        }
    }
}
