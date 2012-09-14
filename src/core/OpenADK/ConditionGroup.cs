//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;

namespace OpenADK.Library
{
    /// <summary>  A group of query conditions.</summary>
    /// <remarks>
    /// A ConditionGroup is a container for Condition objects that are evaluated
    /// together as a group using the Boolean operator passed to the constructor.
    /// ConditionGroups may be nested such that each ConditionGroup is evaluated
    /// using the Boolean operator passed to the constructor.
    /// </remarks>
    public class ConditionGroup
    {
        /// <summary>The Conditions and nested ConditionGroups that comprise this group </summary>
        protected internal List<Object> fConditions;

        /// <summary>The Boolean operator to use when joining conditions in this group </summary>
        protected internal GroupOperator fOp;

        /// <summary>  Constructs a ConditionGroup</summary>
        /// <param name="ops">	The Boolean operator to use when joining conditions in this
        /// group; either <c>Condition.AND</c> or <c>Condition.OR</c>
        /// </param>
        public ConditionGroup( GroupOperator ops )
        {
            fOp = ops;
        }


        /// <summary>  Gets the Boolean operator for joining all conditions in this group</summary>
        /// <returns> A <see cref="GroupOperator"/>
        /// </returns>
        public virtual GroupOperator Operator
        {
            get { return fOp; }
        }

        /// <summary>  Gets the conditions in this group. If the group consists of only nested
        /// ConditionGroups, an empty array is returned; use the getConditionGroups
        /// method to retrieve the nested groups.
        /// </summary>
        /// <returns> An array of Conditions added to this group, or an empty array
        /// if the group consists of only nested ConditionGroups
        /// </returns>
        public virtual Condition[] Conditions
        {
            get
            {
                List<Object> v = new List<Object>();

                if ( fConditions != null )
                {
                    for ( int i = 0; i < fConditions.Count; i++ )
                    {
                        Object o = fConditions[i];
                        if ( o is Condition )
                        {
                            v.Add( fConditions[i] );
                        }
                    }
                }

                Condition[] arr = new Condition[v.Count];
                v.CopyTo( arr );
                return arr;
            }
        }

        /// <summary>  Gets the nested ConditionGroups in this group. If the group does not
        /// contain any nested ConditionGroups and is comprised of only Condition
        /// elements, an empty array is returned
        /// </summary>
        public virtual ConditionGroup[] Groups
        {
            get
            {
                if ( fConditions == null )
                {
                    return new ConditionGroup[0];
                }

                List<ConditionGroup> v = new List<ConditionGroup>();
                foreach ( object o in fConditions )
                {
                    if ( o is ConditionGroup )
                    {
                        v.Add( (ConditionGroup) o );
                    }
                }


                return v.ToArray();
            }
        }


        /// <summary>Adds a Condition to this group</summary>
        /// <param name="cond">The condition to add to this group of conditions</param>
        public virtual void AddCondition( Condition cond )
        {
            if ( fConditions == null )
            {
                fConditions = new List<object>( 10 );
            }

            fConditions.Add( cond );
        }

        /// <summary>Adds a condition to this group</summary>
        /// <remarks>
        /// This method of adding conditions is convenient for adding conditions involving 
        /// root attributes or elements to a query. If you need to add conditions on deeply
        /// nested elements, use <see cref="ConditionGroup.AddCondition(String, ComparisonOperators, String)" />
        /// </remarks>
        public virtual void AddCondition( IElementDef field,
                                          ComparisonOperators ops,
                                          string val )
        {
            AddCondition( new Condition( field, ops, val ) );
        }


        /// <summary>
        /// Internal only. Adds a query condition to this condition group by evaluating the XPath.
        /// If possible, the IElementDef representing the field will be evaluated
        /// </summary>
        /// <param name="objectDef">The metadata definition of the parent object</param>
        /// <param name="xPath">The xpath to the field</param>
        /// <param name="ops">The ComparisonOperator to apply</param>
        /// <param name="value">The value to compare the field to</param>
        internal void AddCondition( IElementDef objectDef, String xPath, ComparisonOperators ops, String value )
        {
            AddCondition( new Condition( objectDef, xPath, ops, value ) );
        }


        /// <summary>
        /// Add a condition to this group using a deeply nested path. Using this
        /// method of adding query condition allows for specifying deeply nested query
        /// conditions. However, the xpath specified here is specific to the version 
        /// of SIF
        /// </summary>
        /// <remarks>
        /// To ensure your code works with all versions  of SIF, you should use 
        /// <see cref="ConditionGroup.AddCondition(IElementDef, ComparisonOperators, String)" /> whenever possible.
        /// </remarks>
        /// <param name="xPath">The XPath representation of this field</param>
        /// <param name="ops">The ComparisonOperator to apply</param>
        /// <param name="value">The value to compare the field to</param>
        public void AddCondition( string xPath, ComparisonOperators ops, String value )
        {
            AddCondition( new Condition( xPath, ops, value ) );
        }


        /// <summary>  Adds a nested ConditionGroup to this group</summary>
        /// <param name="group">The ConditionGroup to add to this group</param>
        public virtual void AddGroup( ConditionGroup group )
        {
            if ( fConditions == null )
            {
                fConditions = new List<Object>();
            }

            fConditions.Add( group );
        }

        /// <summary>  Determines if there are any conditions in this group, including any
        /// nested ConditionGroups
        /// </summary>
        /// <returns>True if this group contains any conditions</returns>
        public virtual bool HasConditions()
        {
            if ( fConditions != null )
            {
                for ( int i = 0; i < fConditions.Count; i++ )
                {
                    Object o = fConditions[i];
                    if ( o is Condition )
                    {
                        return true;
                    }
                    if ( o is ConditionGroup && ((ConditionGroup) o).HasConditions() )
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>  Tests if this ConditionGroup has a Condition for a specific element or
        /// attribute. Nested ConditionGroups are not included in the search.
        /// 
        /// 
        /// </summary>
        /// <param name="elementOrAttr">The ElementDef constant from the SifDtd class that
        /// identifies the specific attribute or element to search for
        /// </param>
        /// <returns> The matching Condition object or <c>null</c> if the group
        /// does not contain a Condition for the specified element or attribute
        /// </returns>
        public virtual Condition HasCondition( IElementDef elementOrAttr )
        {
            if ( fConditions != null )
            {
                foreach ( Object o in fConditions )
                {
                    if ( o is Condition )
                    {
                        Condition cond = (Condition) o;
                        if ( cond.Field != null && cond.Field.Name == elementOrAttr.Name )
                        {
                            return cond;
                        }
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Tests if this ConditionGroup has a Condition for a specific XPath. 
        /// Nested ConditionGroups are not included in the search.
        /// </summary>
        /// <param name="xPath">The xPath representation of the query field. e.g. "Name/FirstName"</param>
        /// <returns>The matching Condition object or <code>null</code> if the root
        /// condition group does not contain a Condition for the specified path.</returns>
        public Condition HasCondition( String xPath )
        {
            if ( fConditions != null )
            {
                foreach ( Object o in fConditions )
                {
                    if ( o is Condition && ((Condition) o).GetXPath() == xPath )
                    {
                        return (Condition) o;
                    }
                }
            }

            return null;
        }


        /// <summary>  Gets the number of conditions in this group.
        /// 
        /// </summary>
        /// <returns> The number of conditions that will be returned by the
        /// getConditions method, or 0 if there are no conditions in the group
        /// or if the group is comprised of only nested ConditionGroups
        /// </returns>
        public virtual int Size()
        {
            int cnt = 0;
            if ( fConditions != null )
            {
                for ( int i = 0; i < fConditions.Count; i++ )
                {
                    Object o = fConditions[i];
                    if ( o is Condition )
                    {
                        cnt++;
                    }
                }
            }
            return cnt;
        }
    }
}
