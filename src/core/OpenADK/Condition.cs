//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  A query condition.</summary>
    public class Condition
    {
        #region Private Fields

        /// <summary>The field being referenced, or null if it is a deeply-nested query</summary>
        private IElementDef fField;

        /// <summary>
        /// The XPath
        /// </summary>
        private string fXPath;

        /// <summary>The operator </summary>
        private ComparisonOperators fOps;

        /// <summary>The value to evaluate </summary>
        private string fValue;

        #endregion

        #region Constructor

        /// <summary>  Constructs a query condition</summary>
        /// <param name="field">A static constant defined by the <c>Adk</c>
        /// to identify the field (i.e. element or attribute) to evaluate
        /// </param>
        /// <param name="ops">The comparison operator</param>
        /// <param name="val">The value to evaluate
        /// </param>
        public Condition( IElementDef field,
                          ComparisonOperators ops,
                          string val )
        {
            fField = field;
            fOps = ops;
            fValue = val;
            fXPath = field.GetSQPPath( Adk.SifVersion );
        }


        /// <summary>
        /// Constructs a query condition using an xpath query string
        /// </summary>
        /// <param name="xPath">The path to the field. e.g. "Name/FirstName"</param>
        /// <param name="ops">The comparison operator from the ComparisonOperators enum</param>
        /// <param name="value">The value to evaluate</param>
        public Condition( String xPath, ComparisonOperators ops, String value )
        {
            fOps = ops;
            fValue = value;
            fXPath = xPath;
        }


        /// <summary>
        /// Internal only. Creates a query condition by evaluating the XPath.
        /// If possible, the ElementDef representing the field will be evaluated
        /// </summary>
        /// <param name="objectDef">The metadata definition of the parent object</param>
        /// <param name="xPath">The xpath to the field</param>
        /// <param name="ops">The ComparisonOperator to apply</param>
        /// <param name="value">The value to compare the field to</param>
        internal Condition( IElementDef objectDef, String xPath, ComparisonOperators ops, String value )
        {
            fOps = ops;
            fValue = value;
            fXPath = xPath;
            IElementDef target = Adk.Dtd.LookupElementDefBySQP( objectDef, xPath );
            fField = target;
        }

        #endregion

        #region Public Methods

        /// <summary>  Gets the comparision operator</summary>
        /// <returns>The Comparision operator used for this condition
        /// </returns>
        public virtual ComparisonOperators Operators
        {
            get { return fOps; }
        }

        /// <summary>  Gets the metadata for the field to evaluate</summary>
        /// <value>The ElementDef representing the Query condition
        /// or <c>null</c> if the Query condition represents a
        /// deeply nested XPath</value>
        /// <seealso cref="Condition.GetXPath()"/>
        public virtual IElementDef Field
        {
            get { return fField; }
        }

        /// <summary>
        /// Gets the XPath representation of the Query condition. 
        /// e.g. <c>"Name/LastName"</c> or <c>"SIF_ExtendedElements/SIF_ExtendedElement[@Name='eyecolor']"</c>
        /// </summary>
        /// <return>The XPath representation of the query</return>
        public String GetXPath()
        {
            return fXPath;
        }


        /// <summary>
        /// Gets the XPath representation of this query for the specific version of SIF
        /// </summary>
        /// <param name="q">The Query that this condition is associated with</param>
        /// <param name="version">The version of SIF to use when rendering element names in the path</param>
        /// <returns>The XPath representation of this query path in the specified version of SIF</returns>
        public String GetXPath( Query q, SifVersion version )
        {
            return Adk.Dtd.TranslateSQP( q.ObjectType, fXPath, version );
        }


        /// <summary>  Gets the comparison value</summary>
        public virtual string Value
        {
            get { return fValue; }
        }

        #endregion

        #region Static

        /// <summary>  Parses an operator represented as a string.</summary>
        /// <param name="op">An operator value such as "EQ", "LT", "GT" etc.
        /// </param>
        /// <returns> One of the <see cref="ComparisonOperators"/> values
        /// </returns>
        /// <exception cref="AdkUnknownOperatorException">thrown if the operator is not recognized</exception>
        public static ComparisonOperators ParseComparisionOperators( string op )
        {
            try
            {
                return (ComparisonOperators) Enum.Parse( typeof ( ComparisonOperators ), op, true );
            }
            catch ( ArgumentException ae )
            {
                throw new AdkUnknownOperatorException( op, ae );
            }
        }

        /// <summary>  Parses a group operator represented as a string.</summary>
        /// <param name="op">An operator value such as "And", "Or", "None"
        /// </param>
        /// <returns> One of the <see cref="GroupOperator"/> values</returns>
        /// <exception cref="AdkUnknownOperatorException">thrown if the operator is not recognized</exception>
        public static GroupOperator ParseGroupOperator( string op )
        {
            return (GroupOperator) ParseOperator( typeof ( GroupOperator ), op );
        }

        private static object ParseOperator( Type operatorType,
                                             string val )
        {
            try
            {
                return Enum.Parse( operatorType, val, false );
            }
            catch ( Exception ex )
            {
                throw new AdkUnknownOperatorException
                    ( "Val is not a recognized value for " + operatorType.Name, ex );
            }
        }

        #endregion
    }
}
