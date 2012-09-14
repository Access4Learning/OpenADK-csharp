//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Text;

namespace OpenADK.Library.Tools.Queries
{
    /// <summary>  The abstract base class for query formatters, which format SIF_Query queries
    /// in another form such as an SQL WHERE clause. The way in which a query is
    /// formatted is determined by the subclass implementation. A subclass must
    /// implement these methods:
    /// 
    /// 
    /// <ul>
    /// <li>getOpenBrace</li>
    /// <li>getCloseBrace</li>
    /// <li>getOperator</li>
    /// <li>renderField</li>
    /// <li>renderValue</li>
    /// </ul>
    /// 
    /// 
    /// </summary>
    /// <author> Data Solutions
    /// </author>
    /// <version>  ADK 1.0
    /// </version>
    public abstract class QueryFormatter
    {
        /// <summary>  Return the text that should be inserted for an opening brace</summary>
        protected internal abstract string OpenBrace { get; }

        /// <summary>  Return the text that should be inserted for a closing brace</summary>
        protected internal abstract string CloseBrace { get; }

        /// <summary>  Constructs a QueryFormatter</summary>
        public QueryFormatter()
        {
        }

        /// <summary>  Builds a query string given a dictionary of mappings and a Query
        /// instance. This method evaluates the conditions of that Query to produce
        /// a textual query string in format determined by the implementation.</summary>
        /// <para>
        /// The Map should contain application-defined field values that map
        /// to <c>ElementDef</c> key elements. Whenever a SIF element or attribute
        /// is found in the Query, the corresponding application-defined field is
        /// used in its place.
        /// </para> 
        /// <para>
        /// A special convention allows agents to also map field values: If a field
        /// in the Map is expressed in the form "field-name{value1=cons1;value2=cons2;..}",
        /// the list of value replacements within the curly braces is applied to the
        /// value of the SIF_Value element in the SIF_Query. For example, the
        /// acceptable values for LibraryPatronStatus/@SifRefIdType attribute are
        /// "StudentPersonal" and "StaffPersonal". If in your application you
        /// represent these values as numeric types - say, 1 and 2, respectively -
        /// you could create the following field mapping to instruct the QueryFormatter
        /// to substitute "StudentPersonal" with "1" and "StaffPersonal" with "2":
        /// 
        /// "MyCircStatus.Type{StudentPersonal=1;StaffPersonal=2}"
        /// </para>
        /// <param name="query">An ADK Query object, usually obtained during the processing
        /// of a SIF_Request by a <i>Publisher</i> message handler
        /// </param>
        /// <param name="table">A dictionary that maps SIFDTD ElementDef constants to
        /// application-defined field values
        /// </param>
        /// <exception cref="QueryFormatterException">Thrown if the query contains conditions that are not mapped in the table</exception>
       public virtual string Format(Library.Query query,
                                     IDictionary table)
        {
            return Format(query, table, true);
        }

       public virtual string Format(Library.Query query,
                                     IDictionary table, bool isExplicit)
        {
            StringBuilder str = new StringBuilder();

            ConditionGroup[] grp = query.Conditions;
            for (int c = 0; c < grp.Length; c++)
            {
                str.Append(OpenBrace);
                EvaluateConditionGroup(query, grp[c], str, table, isExplicit);
                str.Append(CloseBrace);

                if (c != grp.Length - 1)
                {
                    str.Append(GetOperator(query.RootConditionGroup.Operator));
                }
            }

            return str.ToString();
        }


       protected internal virtual void EvaluateConditionGroup(Library.Query query,
                                                               ConditionGroup grp,
                                                               StringBuilder str,
                                                               IDictionary table,
                                                               bool isExplicit)
        {
            Condition[] conds = grp.Conditions;

            if (conds.Length != 0)
            {
                for (int i = 0; i < conds.Length; i++)
                {
                    EvaluateCondition(query, conds[i], str, table, isExplicit);

                    if (i != conds.Length - 1)
                    {
                        str.Append(GetOperator(grp.Operator));
                    }
                }
            }
            else
            {
                ConditionGroup[] groups = grp.Groups;

                for (int i = 0; i < groups.Length; i++)
                {
                    str.Append(OpenBrace);
                    EvaluateConditionGroup(query, groups[i], str, table, isExplicit);
                    str.Append(CloseBrace);

                    if (i != groups.Length - 1)
                    {
                        str.Append(GetOperator(grp.Operator));
                    }
                }
            }
        }

        /// <summary>
        /// Evaluates a single SIF_Condition
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cond"></param>
        /// <param name="str"></param>
        /// <param name="table"></param>
        /// <exception cref="QueryFormatterException">Thrown if no mapping was found for a requested element</exception>
        /// <param name="isExplicit"></param>
       protected virtual void EvaluateCondition(Library.Query query,
                                                 Condition cond,
                                                 StringBuilder str,
                                                 IDictionary table,
                                                 bool isExplicit)
        {
            String path = cond.GetXPath();
            Object o = table[path];
            if (o == null)
            {
                IElementDef field = cond.Field;
                if (field != null)
                {
                    o = table[ field ];
                }
            }

            if (o == null)
            {
                if (isExplicit)
                {
                    throw new QueryFormatterException
                        (
                        "QueryFormatter was not provided with an application-defined field value for " +
                        query.ObjectTag + "/" + cond.GetXPath() +
                        "; cannot format the SIF_Query");
                }
                else
                {
                    // Render a default operation that will always return true
                    // as a placeholder
                    str.Append("1=1");
                    return;
                }
            }

            if (o is IQueryField)
            {
                str.Append(((IQueryField)o).Render(this, query, cond));
            }
            else
            {
                str.Append(RenderField(cond.Field, o));
                str.Append(GetOperator(cond.Operators));
                str.Append(RenderValue(cond.Value, o));
            }
        }

        /// <summary>  Return the text that should be inserted for a logical <c>AND</c> or <c>OR</c> operation</summary>
        public abstract string GetOperator(GroupOperator op);

        /// <summary>  Return the text that should be inserted for a logical greater than, less than, equal or not equal comparison</summary>
        public abstract string GetOperator(ComparisonOperators op);

        /// <summary>  Return the text for a field name</summary>
        /// <param name="field">The field name
        /// </param>
        /// <param name="def">The corresponding field definition from the Map passed to
        /// the <c>format</c> method
        /// </param>
        /// <returns> The implementation returns the field name in whatever form is
        /// appropriate to the implementation, using the supplied <i>def</i>
        /// Object if necessary to obtain additional field information.
        /// </returns>
        public abstract string RenderField(IElementDef field,
                                           Object def);

        /// <summary>  Return the text for a field value</summary>
        /// <param name="valu">The field value
        /// </param>
        /// <param name="def">The corresponding field definition from the Map passed to
        /// the <c>format</c> method
        /// </param>
        /// <returns> The implementation returns the field value in whatever form is
        /// appropriate to the implementation, using the supplied <i>def</i>
        /// Object if necessary to obtain additional field information
        /// </returns>
        public abstract string RenderValue(string valu,
                                           Object def);

        /// <summary>  Extracts a field name from a string in the form "field-name{...}"</summary>
        protected internal virtual string ExtractFieldName(string def)
        {
            int i = def.IndexOf( '{');
            if (i == - 1)
            {
                return def;
            }

            return def.Substring(0, (i) - (0));
        }

        /// <summary>  Applies the value substitutions defined for a field as described in the
        /// class comments. For example, if the source string passed to this method
        /// has the value "Blue" and the field mapping definition has the value
        /// "Color{Red=0;Green=1;Blue=2}", a value of "2" will be returned.
        /// 
        /// 
        /// </summary>
        /// <param name="src">The value to process
        /// </param>
        /// <param name="def">The application-defined field mapping to apply
        /// </param>
        protected internal virtual string DoValueSubstitution(string src,
                                                              string def)
        {
            int i = def.IndexOf('{');
            if (i == - 1)
            {
                return src;
            }

            int end = def.IndexOf( '}', i + 1);
            if (end == - 1)
            {
                return src;
            }

            string trans = def.Substring(i + 1, (end) - (i + 1));
            foreach (string s in trans.Split(';'))
            {
                i = s.IndexOf( '=');
                if (i != - 1)
                {
                    if (i == 0)
                    {
                        return s.Substring(1);
                    }
                    string cmp = s.Substring(0, (i) - (0));
                    if (cmp.Equals(src))
                    {
                        return s.Substring(i + 1);
                    }
                }
            }

            return src;
        }
    }
}
