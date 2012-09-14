//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using OpenADK.Library.Impl;
using OpenADK.Library.Infra;
using OpenADK.Library.Tools.XPath;

namespace OpenADK.Library
{
    /// <summary>  Encapsulates a SIF Query.
    /// 
    /// An instance of this class is passed to the <c>Zone.query</c> and
    /// <c>Topic.query</c> methods when issuing a SIF Request. A Query object
    /// defines the following parameters to the request:
    /// 
    /// <list type="bullet">
    /// <item><term>The type of SIF Data Object to query for</term></item>
    /// <item><term>Conditions: One or more conditions may be placed on the query to 
    /// select a subset of objects from the responder (when no conditions are
    /// present the responder returns all objects)</term></item>
    /// <item><term>Field Restrictions: An optional list of elements to include in 
    /// responses to the query (when no field restrictions are present the
    /// responder returns the full set of elements for each object)</term></item>
    /// </list>
    /// 
    /// 
    /// To construct a simple Query to query for all objects with no conditions or
    /// field restrictions, call the constructor that accepts an ElementDef constant 
    /// from the SifDtd class:
    /// 
    /// 
    /// <blockquote>
    /// <c>
    /// Query myQuery = new Query( SifDtd.STUDENTPERSONAL );<br/>
    /// </c>
    /// </blockquote>
    /// 
    /// More complex queries can be constructed by specifying conditions and field
    /// restrictions.
    /// 
    /// <b>Conditions</b>
    /// A Query may optionally specify one or more conditions to restrict the number
    /// of objects returned by the responder. (Refer to the SIF Specification for a 
    /// detailed description of how query conditions may be constructed.) When no 
    /// conditions are specified, the responder interprets the query to mean "all 
    /// objects". Note SIF 1.0r2 and earlier limit queries such that only root-level 
    /// attributes may be included in query conditions, and only the equals ("EQ") 
    /// comparison operator may be used. SIF 1.1 and later allow agents to query for
    /// elements within an object, but responders may return an error if they do not
    /// support that functionality.
    /// 
    /// 
    /// Query conditions are encapsulated by the Adk's ConditionGroup class, which is 
    /// used to build SIF_ConditionGroup, SIF_Conditions, and SIF_Condition elements
    /// when the class framework sends a SIF_Request message to a zone. Every Query
    /// with conditions has a root ConditionGroup with one or more child ConditionGroups.
    /// Unless you construct these groups manually, the Query class will automatically
    /// establish a root ConditionGroup and a single child when the <c>addCondition</c>
    /// method is called. Use the <c>addCondition</c> method to add conditions 
    /// to a Query. Note the form of Query constructor you call determines how the 
    /// <c>addCondition</c> method works. If you call the default constructor, 
    /// the Adk automatically establishes a root SIF_ConditionGroup with a Type 
    /// attribute of "None", and a single SIF_Conditions child with a Type attribute 
    /// of "And". ("None" will be used if the query has only one condition.) 
    /// SIF_Condition elements are then added to this element whenever the 
    /// <c>addCondition</c> method is called.
    /// 
    /// For example,
    /// 
    /// <blockquote>
    /// <c>
    /// // Query for a single student by RefId<br/>
    /// Query query = new Query( SifDtd.STUDENTPERSONAL );<br/>
    /// query.addCondition(<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL_REFID, Condition.EQ,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;"4A37969803F0D00322AF0EB969038483" );<br/>
    /// </c>
    /// </blockquote>
    /// 
    /// If you want to specify the "Or" comparision operator instead of the default
    /// of "And", call the constructor that accepts a constant from the Condition
    /// class.
    /// 
    /// For example,
    /// 
    /// <blockquote>
    /// <c>
    /// // Query for student where the RefId is A, B, or C<br/>
    /// Query query = new Query( SifDtd.STUDENTPERSONAL, Condition.OR );<br/>
    /// <br/>
    /// query.addCondition(<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL_REFID, Condition.EQ,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;"4A37969803F0D00322AF0EB969038483" );<br/>
    /// query.addCondition(<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL_REFID, Condition.EQ,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;"5A37969803F0D00322AF0EB969038484" );<br/>
    /// query.addCondition(<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL_REFID, Condition.EQ,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;"6A37969803F0D00322AF0EB969038485" );<br/>
    /// </c>
    /// </blockquote>
    /// 
    /// The above examples show how to add simple conditions to a Query. To construct 
    /// complex queries with nested groups of conditions, create your own root 
    /// SIF_ConditionGroup object by calling the form of constructor that
    /// accepts a ConditionGroup instance. You can specify nested ConditionGroup
    /// children of this root object.
    /// 
    /// 
    /// For example,
    /// 
    /// <blockquote>
    /// <c>
    /// // Query for student where the Last Name is Jones and the First Name is<br/>
    /// // Bob, and the graduation year is 2004, 2005, or 2006<br/>
    /// ConditionGroup root = new ConditionGroup( Condition.AND );<br/>
    /// ConditionGroup grp1 = new ConditionGroup( Condition.AND );<br/>
    /// ConditionGroup grp2 = new ConditionGroup( Condition.OR );<br/>
    /// <br/>
    /// // For nested elements, you cannot reference a SifDtd constant. Instead, use<br/> 
    /// // the lookupElementDefBySQL function to lookup an ElementDef constant<br/>
    /// // given a SIF Query Pattern (SQP)<br/>
    /// ElementDef lname = Adk.Dtd().lookupElementDefBySQP(<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL, "Name/LastName" );</br>
    /// ElementDef fname = Adk.Dtd().lookupElementDefBySQP(<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL, "Name/FirstName" );</br>
    /// grp1.addCondition( lname, Condition.EQ, "Jones" );<br/>
    /// grp1.addCondition( fname, Condition.EQ, "Bob" );<br/>
    /// <br/>
    /// grp2.addCondition( SifDtd.STUDENTPERSONAL_GRADYEAR, Condition.EQ, "2004" );<br/>
    /// grp2.addCondition( SifDtd.STUDENTPERSONAL_GRADYEAR, Condition.EQ, "2005" );<br/>
    /// grp2.addCondition( SifDtd.STUDENTPERSONAL_GRADYEAR, Condition.EQ, "2006" );<br/>
    /// <br/>
    /// // Add condition groups to the root group<br/>
    /// root.addGroup( grp1 );<br/>
    /// root.addGroup( grp2 );<br/>
    /// <br/>
    /// // Query for student with the conditions prepared above by passing the<br/>
    /// // root ConditionGroup to the constructor<br/>
    /// Query query = new Query( SifDtd.STUDENTPERSONAL, root );<br/>
    /// </c>
    /// </blockquote>
    /// 
    /// <b>Field Restrictions</b>
    /// If only a subset of elements and attributes are requested, use the
    /// <c>setFieldRestrictions</c> method to indicate which elements and
    /// attributes should be returned to your agent by the responder. For example,
    /// to request the &lt;StudentPersonal&gt; object with RefId "4A37969803F0D00322AF0EB969038483"
    /// but to only include the <c>RefId</c> attribute and <c>Name</c>
    /// and <c>PhoneNumber</c> elements in the response,
    /// 
    /// <blockquote>
    /// <c>
    /// 
    /// // Query for a single student by RefId<br/>
    /// Query query = new Query( SifDtd.STUDENTPERSONAL );<br/>
    /// <br/>
    /// query.addCondition(<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL_REFID, Condition.EQ,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;"4A37969803F0D00322AF0EB969038483" );<br/>
    /// 
    /// query.setFieldRestrictions(<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;new ElementDef[] {<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL_REFID,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL_NAME,<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SifDtd.STUDENTPERSONAL_PHONENUMBER<br/>
    /// &nbsp;&nbsp;&nbsp;&nbsp;}
    /// );
    /// </c>
    /// </blockquote>
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    public sealed class Query
    {
        /// <summary>The object to query </summary>
        private IElementDef fObjType;

        /// <summary>The version of SIF associated with the query </summary>
        private SifVersion[] fVersions = new SifVersion[0];

        /// <summary>
        /// The SIF Context that this Query applies to
        /// </summary>
        private SifContext fContext = SifContext.DEFAULT;


        /// <summary>Root condition groups </summary>
        private ConditionGroup fRoot;

        /// <summary>Fields to include in the result of the query (null = all fields) </summary>
        private List<ElementRef> fFieldRestrictions;

        /// <summary>
        /// User state
        /// </summary>
        private object fUserData;


        /// <summary>  Constructs a Query object with no initial conditions or field
        /// restrictions. If conditions are subsequently added to the Query, they
        /// will be evaluated as a group with the logical AND operator. To specify
        /// that the logical OR operator be used, call the form of constructor that
        /// accepts an alternate operator.
        /// 
        /// 
        /// </summary>
        /// <param name="objectType">An ElementDef describing the object type to query (e.g.
        /// <c>Adk.Dtd.STUDENTPERSONAL</c>)
        /// </param>
        public Query(IElementDef objectType)
        {
            if (!objectType.Object)
            {
                throw new ArgumentException
                    ("\"" + objectType.Name + "\" is not a root-level SIF Data Object");
            }

            fObjType = objectType;
            fRoot = null;
        }

        /// <summary>  Constructs a Query object with one ConditionGroup where all conditions
        /// in the group are evaluated using the supplied grouping operator. All Conditions
        /// subsequently added to this Query will be placed into the ConditionGroup
        /// created by the constructor.
        /// 
        /// This constructor is provided as a convenience so that callers do
        /// not have to explicitly create a ConditionGroup for simple queries.
        /// 
        /// </summary>
        /// <param name="objectType">An ElementDef describing the object type to query (e.g.
        /// <c>StudentDTD.STUDENTPERSONAL</c>)
        /// </param>
        /// <param name="logicalOp">The logical operator that defines how to compare this group
        /// with other condition groups that comprise the query (e.g. Condition.OR)
        /// </param>
        public Query(IElementDef objectType,
                      GroupOperator logicalOp)
        {
            if (!objectType.Object)
            {
                throw new ArgumentException
                    ("\"" + objectType.Name + "\" is not a root-level SIF Data Object");
            }

            fObjType = objectType;
            fRoot = new ConditionGroup(logicalOp);
        }

        /// <summary>  Constructs a Query object with a ConditionGroup.
        /// 
        /// 
        /// </summary>
        /// <param name="objectType">An ElementDef describing the object type to query (e.g.
        /// <c>StudentDtd.STUDENTPERSONAL</c>)
        /// </param>
        /// <param name="conditions">A ConditionGroup comprised of one or more query Conditions
        /// </param>
        public Query(IElementDef objectType,
                      ConditionGroup conditions)
        {
            if (!objectType.Object)
            {
                throw new ArgumentException
                    ("\"" + objectType.Name + "\" is not a root-level SIF Data Object");
            }

            fObjType = objectType;
            fRoot = conditions;
        }

        /// <summary>  Constructs a Query object from a SIF_QueryObject.
        /// 
        /// This constructor is not typically called by agents but is used internally
        /// by the class framework. The other constructors can be used to safely
        /// create Query instances to request a specific SIF Data Object. Use the
        /// <c>addCondition</c> and <c>setFieldRestrictions</c> methods
        /// to further define the conditions and SIF elements specified by the query.
        /// 
        /// </summary>
        /// <param name="query">A SIF_Query object received in a SIF_Request message
        /// </param>
        /// <exception cref="AdkUnknownOperatorException">If one of the operators in the SIF_Query is
        /// unrecognized by the ADK</exception>
        /// <exception cref="AdkSchemaException">If the object or elements defined in the query or
        /// not recognized by the ADK </exception>
        public Query(SIF_Query query)
        {
            SIF_QueryObject qo = query.SIF_QueryObject;
            if (qo == null)
            {
                throw new ArgumentException("SIF_Query must have a SIF_QueryObject element");
            }

            fObjType = Adk.Dtd.LookupElementDef(qo.ObjectName);
            if (fObjType == null)
            {
                throw new AdkSchemaException
                    (qo.ObjectName +
                      " is not a recognized SIF Data Object, or the agent is not configured to support this object type");
            }
            fRoot = null;

            SIF_ConditionGroup cg = query.SIF_ConditionGroup;
            if (cg != null && cg.GetSIF_Conditionses() != null)
            {
                GroupOperator grpOp;

                try
                {
                    grpOp = Condition.ParseGroupOperator(cg.Type);
                }
                catch (AdkUnknownOperatorException)
                {
                    grpOp = GroupOperator.None;
                }

                fRoot = new ConditionGroup(grpOp);

                SIF_Conditions[] sifConds = cg.GetSIF_Conditionses();

                if (sifConds.Length == 1)
                {
                    //  There is one SIF_ConditionGroup with one SIF_Conditions,
                    //  so just add all of the conditions (no nested groups)
                    string typ = sifConds[0].Type;
                    if (typ == null)
                    {
                        throw new AdkSchemaException
                            ("SIF_Conditions/@Type is a required attribute");
                    }

                    fRoot.fOp = Condition.ParseGroupOperator(typ);
                    SIF_Condition[] clist = sifConds[0].GetSIF_Conditions();
                    PopulateConditions(query, clist, fRoot);
                }
                else
                {
                    //  There are multiple SIF_Conditions, so add each as a nested
                    //  ConditionGroup of the fRoot
                    for (int i = 0; i < sifConds.Length; i++)
                    {
                        ConditionGroup nested =
                            new ConditionGroup(Condition.ParseGroupOperator(sifConds[i].Type));
                        PopulateConditions(query, sifConds[i].GetSIF_Conditions(), nested);
                        fRoot.AddGroup(nested);
                    }
                }
            }

            SifVersion[] reqVersions = null;
            // First, try to get the version from the SIF_Request
            Element parent = query.Parent;
            if (parent != null)
            {
                if (parent is SIF_Request)
                {
                    SIF_Request request = (SIF_Request)parent;
                    SifVersion[] versions = request.parseRequestVersions(Adk.Log);
                    if (versions.Length > 0)
                    {
                        reqVersions = versions;
                    }
                }
            }

            if (reqVersions == null)
            {
                SifVersion version = query.EffectiveSIFVersion;
                if (version != null)
                {
                    reqVersions = new SifVersion[] { version };
                }
            }

            if (reqVersions == null || reqVersions.Length == 0)
            {
                throw new ArgumentException(
                    "SIF_Query is not contained in a SIF_Request that has a SIF_Version element; cannot determine version of SIF to associated with this Query object");
            }
            else
            {
                fVersions = reqVersions;
            }

            SIF_Element[] fields = query.SIF_QueryObject.GetSIF_Elements();
            if (fields != null && fields.Length > 0)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    string xPath = fields[i].TextValue;
                    if (xPath == null || xPath.Length == 0)
                    {
                        continue;
                    }
                    AddFieldRestriction(xPath);
                }
            }
        }

        /// <summary>  Gets the object type being queried</summary>
        /// <returns> The name of the object passed to the constructor
        /// </returns>
        public IElementDef ObjectType
        {
            get { return fObjType; }
        }

        /// <summary>  Gets the tag name of the object type being queried</summary>
        /// <returns> The tag name of the object passed to the constructor
        /// </returns>
        public string ObjectTag
        {
            get { return fObjType.Tag(Adk.GetLatestSupportedVersion(fVersions)); }
        }


        /// <summary>
        /// Gets and Sets custom state associated with this request. The state object used must be 
        /// serializable and for performance reasons should be reasonably small.
        /// </summary>
        public object UserData
        {
            get { return fUserData; }
            set { fUserData = value; }
        }

        /// <summary>Gets or sets the fields to include in the result of the query.</summary>
        /// <value> An array of fields that should be included in the results of
        /// this query, or null if all fields are to be included
        /// </value>
        public IElementDef[] FieldRestrictions
        {
            get
            {
                if (fFieldRestrictions == null)
                {
                    return null;
                }


                IElementDef[] returnValue = new IElementDef[fFieldRestrictions.Count];
                for (int i = 0; i < returnValue.Length; i++)
                {
                    returnValue[i] = fFieldRestrictions[i].Field;
                }

                return returnValue;
            }

            set
            {
                if (fFieldRestrictions != null)
                {
                    fFieldRestrictions.Clear();
                }
                foreach (IElementDef def in value)
                {
                    AddFieldRestriction(def);
                }
            }
        }

        /// <summary>
        /// Gets the fields that will be include din the result of the query
        /// </summary>
        /// <value>An array of field references that should be included
        /// in the results of the query or null if all fields are to be included</value>
        public IList<ElementRef> FieldRestrictionRefs
        {
            get { return fFieldRestrictions; }
        }

        /// <summary>  Gets the conditions placed on this query.</summary>
        /// <returns> An array of ConditionGroup objects in evaluation order. The 
        /// children of the root ConditionGroup are returned. If no conditions
        /// have been specified, an empty array is returned.
        /// </returns>
        public ConditionGroup[] Conditions
        {
            get
            {
                if (fRoot == null)
                {
                    return new ConditionGroup[0];
                }

                ConditionGroup[] groups = fRoot.Groups;
                if (groups != null && groups.Length > 0)
                {
                    return groups;
                }

                //	There is a fRoot group -- which means the user must have called
                //	the default constructor and then called addCondition() to add one
                //	or more conditions -- but the root group does not itself have any
                //	nested groups. So, just return the root group...

                return new ConditionGroup[] { fRoot };
            }
        }

        /// <summary> 	Gets the root ConditionGroup.</summary>
        /// <returns> The root ConditionGroup that was established by the constructor.
        /// If this query has no conditions, null is returned.
        /// </returns>
        public ConditionGroup RootConditionGroup
        {
            get { return fRoot; }
        }

        /// <summary>Gets or Sets the value of the SIF_Request/SIF_Version element. By default,
        /// this value is set to the version of SIF declared for the agent when the
        /// Adk was initialized.
        /// 
        /// </summary>
        /// <value> The version of SIF the responding agent should use when
        /// returning SIF_Response messages for this query
        /// </value>
        public SifVersion[] SifVersions
        {
            get { return fVersions; }

            set { fVersions = value; }
        }


        /// <summary>
        /// From the list of SifVersions associated with this Query, returns the latest SifVersion
        /// supported by the current ADK instance.
        /// </summary>
        /// <seealso cref="Adk.GetLatestSupportedVersion"/>
        public SifVersion EffectiveVersion
        {
            get { return Adk.GetLatestSupportedVersion(fVersions); }
        }

        /// <summary> 	Sets the root ConditionGroup. 
        /// 
        /// By default a Query is constructed with a ConditionGroup to which 
        /// individual conditions will be added by the <c>addCondition</c> 
        /// methods. You can call this method to prepare a ConditionGroup ahead of
        /// time and replace the default with your own.
        /// 
        /// Note calling this method after <c>addCondition</c> will replace 
        /// any conditions previously added to the Query with the conditions in the 
        /// supplied ConditionGroup.
        /// </summary>
        public ConditionGroup ConditionGroup
        {
            set { fRoot = value; }
        }


        private void PopulateConditions(SIF_Query query,
                                         SIF_Condition[] clist,
                                         ConditionGroup target)
        {
            for (int i = 0; i < clist.Length; i++)
            {
                String o = clist[i].SIF_Operator;
                ComparisonOperators ops = Condition.ParseComparisionOperators(o);
                String val = clist[i].SIF_Value;
                String path = clist[i].SIF_Element;
                target.AddCondition(fObjType, path, ops, val);
            }
        }


        /// <summary>
        /// Add a condition to this query.
        /// </summary>
        /// <remarks>
        /// This method of adding conditions is convenient for adding conditions involving 
        /// root attributes or elements to a query. If you need to add conditions on deeply
        /// nested elements, use <see cref="AddCondition(string,ComparisonOperators,string)"/>
        /// </remarks>
        /// <param name="field">A constant from the package DTD class that identifies an element
        /// or attribute of the data object (e.g. <c>StudentDTD.STUDENTPERSONAL_REFID</c>)</param>
        /// <param name="ops">The comparison operator. Comparison operator constants are
        /// defined by the ComparisionOperators enum</param>
        /// <param name="value">The data that is used to compare to the element or attribute</param>
        /// <exception cref="ArgumentException">if the ElementDef does not represent an immediate
        /// child of the object being queried.</exception>
        public void AddCondition(IElementDef field, ComparisonOperators ops, String value)
        {
            // Do some validation to try to prevent invalid query paths from being created
            String relativePath = field.GetSQPPath(Adk.SifVersion);
            IElementDef lookedUp = Adk.Dtd.LookupElementDefBySQP(fObjType, relativePath);
            if (lookedUp == null)
            {
                throw new ArgumentException("Invalid path: " + fObjType.Name + "/" + relativePath +
                             " is unable to be resolved");
            }

            AddCondition(new Condition(fObjType, relativePath, ops, value));
        }

        /// <summary>
        /// Add a condition to this query. 
        /// </summary>
        /// <param name="condition">The condition to add. This condition is added to the root
        /// condition group.</param>
        /// <seealso cref="Query.RootConditionGroup"/>
        /// <seealso cref="Conditions"/>
        public void AddCondition(Condition condition)
        {
            if (fRoot == null)
            {
                fRoot = new ConditionGroup(GroupOperator.And);
            }
            fRoot.AddCondition(condition);
        }

        /// <summary>
        /// Add a condition to this query using a deeply nested path. Using this
        /// method of adding query condition allows for specifying deeply nested query
        /// conditions. However, the xpath specified here is specific to the version 
        /// of SIF
        /// </summary>
        /// <remarks>To ensure your code works with all versions  of SIF, you should use 
        /// <see cref="Query.AddCondition(IElementDef, ComparisonOperators, String)"/> 
        /// whenever possible.</remarks>
        /// <param name="xPath">he Simple XPath to use for this query condition. E.g. 
        /// <c>SIF_ExendedElements/SIF_ExtendedElement[@Name='eyecolor']</c></param>
        /// <param name="ops">Comparison operator value from the 
        /// ComparisonOperators enum</param>
        /// <param name="value">The data that is used to compare to the element or attribute</param>
        public void AddCondition(String xPath, ComparisonOperators ops, String value)
        {
            AddCondition(new Condition(fObjType, xPath, ops, value));
        }

        /// <summary>
        /// Add a condition to this query. This form of the <c>AddCondition</c>
        /// method is intended to be called internally by the ADK when parsing an
        /// incoming SIF_Query element. To ensure your code works with all versions
        /// of SIF, you should use the other form of this method that accepts an
        /// ElementDef constant for the <i>field</i> parameter whenever possible.
        /// </summary>
        /// <param name="field">
        ///  Identifies an element or attribute of the data object in
        ///  SIF Query Pattern form as described by the SIF Specification
        ///  (e.g. "@RefId").  With SIF 1.5r1 and earlier, only root-level
        ///  attributes may be specified in a query. Note this string is specific
        ///  to the version of SIF associated with the Query as element and
        ///  attribute names may vary from one version of SIF to the next. The
        ///  version defaults to the version of SIF in effect for the agent or the
        ///  version of SIF associated with the <c>SIF_Query</c> object
        ///  passed to the constructor.
        /// </param>
        /// <param name="ops">A value from the ComparisonOperators enum</param>
        /// <param name="value">The data that is used to compare to the element or attribute</param>
        public void AddCondition(String field, String ops, String value)
        {
            try
            {
                AddCondition(
                    field, Condition.ParseComparisionOperators(ops), value);
            }
            catch (AdkUnknownOperatorException uoe)
            {
                Adk.Log.WarnFormat("Unable to parse operator: {0} {1}", ops, uoe, uoe);
                AddCondition(field, ComparisonOperators.EQ, value);
            }
        }


        /// <summary>  Restricts the query to a specific field (i.e. element or attribute) of
        /// the data object being requested. If invoked, the results of the query
        /// will only contain the elements or attributes specified by the fields for
        /// which this method is called (call this method repeatedly for each field).
        /// Otherwise, the results will contain a complete object.
        /// 
        /// </summary>
        /// <param name="field">A <c>ElementDef</c> object defined by the static
        /// constants of the <c>SifDtd</c> class. For example, to restrict
        /// a query for the StudentPersonal topic to include only the StatePr
        /// element of the student address, pass <c>SifDtd.ADDRESS_STATEPR</c>.
        /// This would cause the query results to include only
        /// <c>StudentPersonal/Address/StatePr</c> elements.
        /// </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddFieldRestriction(IElementDef field)
        {
            if (field == null)
            {
                throw new ArgumentException("Field cannot be null");
            }

            if (fFieldRestrictions == null)
            {
                fFieldRestrictions = new List<ElementRef>();
            }

            fFieldRestrictions.Add(new ElementRef(fObjType, field, EffectiveVersion));
        }


        /// <summary>
        /// Restricts the query to a specific field (i.e. element or attribute) of
        /// the data object being requested. If invoked, the results of the query
        /// will only contain the elements or attributes specified by the fields for
        /// which this method is called (call this method repeatedly for each field).
        /// Otherwise, the results will contain a complete object.
        /// </summary>
        /// <param name="xPath">An XPath representing the field being referenced</param>
        public void AddFieldRestriction(String xPath)
        {
            if (xPath == null || xPath.Length == 0)
            {
                throw new ArgumentException("Field cannot be null or zero-length : " + xPath);
            }

            if (fFieldRestrictions == null)
            {
                fFieldRestrictions = new List<ElementRef>();
            }

            fFieldRestrictions.Add(new ElementRef(fObjType, xPath, EffectiveVersion));
        }


        /// <summary>  Determines if this Query has any conditions</summary>
        /// <value> true if the query has one or more conditions
        /// </value>
        /// <seealso cref="Conditions">
        /// </seealso>
        /// <seealso cref="AddCondition(IElementDef,ComparisonOperators,string)">
        /// </seealso>
        public bool HasConditions
        {
            get { return fRoot != null && fRoot.HasConditions(); }
        }

        /// <summary>  Determines if this Query has any field restrictions</summary>
        /// <value> true if the query specifies a subset of fields to be returned;
        /// false if the query returns all elements and attributes of each object
        /// matching the query conditions
        /// </value>
        /// <seealso cref="FieldRestrictions">
        /// </seealso>
        /// <seealso cref="AddFieldRestriction(string)"></seealso>
        /// <seealso cref="AddFieldRestriction(IElementDef)"/>
        public bool HasFieldRestrictions
        {
            get { return fFieldRestrictions != null && fFieldRestrictions.Count > 0; }
        }

        /// <summary>  Tests if this Query has a specific element or attribute condition</summary>
        /// <param name="elementOrAttr">The ElementDef constant from the SifDtd class that
        /// identifies the specific attribute or element to search for
        /// </param>
        /// <returns>The Condition object representing the condition. If no
        /// Condition exists for the element or attribute, null is returned</returns>
        public Condition HasCondition(IElementDef elementOrAttr)
        {
            ConditionGroup[] grps = Conditions;

            for (int i = 0; i < grps.Length; i++)
            {
                Condition c = grps[i].HasCondition(elementOrAttr);
                if (c != null)
                {
                    return c;
                }
            }

            return null;
        }


        /// <summary>
        /// Tests if this Query has a condition referencing a specific xPath
        /// </summary>
        /// <param name="xPath">The Xpath which identifies the specific attribute or element to search for</param>
        /// <returns>The Condition object representing the condition. If no
        /// Condition exists for the element or attribute, null is returned</returns>
        public Condition HasCondition(String xPath)
        {
            ConditionGroup[] grps = Conditions;

            for (int i = 0; i < grps.Length; i++)
            {
                Condition c = grps[i].HasCondition(xPath);
                if (c != null)
                    return c;
            }

            return null;
        }


        /// <summary>
        /// Returns the XML representation of this Query in the format required by SIF
        /// </summary>
        /// <returns>a string containing the XML representation as a SIF_Query element. If an error
        /// occurs during the conversion, an empty string ("") is returned.</returns>
        public String ToXml()
        {
            return ToXml(EffectiveVersion);
        }


        /// <summary>
        /// Returns the XML representation of this Query in the format required by SIF
        /// for the specified version
        /// </summary>
        /// <param name="version">The SIF Version to render the Query in. The ADK will attempt to render
        /// the query path using the proper element or attribute names for the version of SIF
        /// </param>
        /// <returns>a string containing the XML representation as a SIF_Query element. If an error
        /// occurs during the conversion, an empty string ("") is returned.
        /// </returns>
        public String ToXml(SifVersion version)
        {
            // Create a SIF_Query object
            SIF_Query sifQ = SIFPrimitives.CreateSIF_Query(this, version, true);
            try
            {
                using (StringWriter outStream = new StringWriter())
                {
                    SifWriter w = new SifWriter(outStream);
                    w.Write(sifQ);
                    w.Flush();
                    return outStream.ToString();
                }
            }
            catch (Exception e)
            {
                Adk.Log.Warn("Error creating XML equivalent of Query: " + e, e);
                return "";
            }
        }


        /// <summary>
        ///  Returns the SIF_Query representation of this Query in the format required by SIF
        /// </summary>
        /// <returns>A SIF_Query element</returns>
        public SIF_Query ToSIF_Query()
        {
            return ToSIF_Query(Adk.SifVersion);
        }

        /// <summary>the SIF_Query representation of this Query in the format required by SIF
        /// for the specified version
        /// </summary>
        /// <param name="version">The SIF Version to render the Query in. The ADK will attempt to render
        /// the query path using the proper element or attribute names for the version of SIF</param>
        /// <returns>A SIF_Query element</returns>
        public SIF_Query ToSIF_Query(SifVersion version)
        {
            return SIFPrimitives.CreateSIF_Query(this, version, true);
        }


        /// <summary>
        /// Evaluate the given the SIFDataObject against the conditions provided in the
        /// Query. All conditions are evaluated using standard string comparisons using
        /// the Invariant Culture
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="OpenADK.Library.AdkSchemaException">If the condition contains references to invalid elements</exception>
        public bool Evaluate(SifDataObject obj)
        {
            return Evaluate(obj, CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Evaluate the given the SIFDataObject against the conditions provided in the
        /// Query. All conditions are evaluated using the provided comparer
        /// </summary>
        /// <param name="obj"> The SIFDataObject to evalaute against this query</param>
        /// <param name="culture">The culture info used to do string comparisons</param>
        /// <returns></returns>
        /// <exception cref="OpenADK.Library.AdkSchemaException">If the condition contains references to invalid elements</exception>
        public bool Evaluate(SifDataObject obj,
                              CultureInfo culture)
        {
            if (!(obj.ElementDef == fObjType))
            {
                return false;
            }
            if (fRoot != null)
            {
                SifXPathContext context = SifXPathContext.NewSIFContext(obj, EffectiveVersion);
                return EvaluateConditionGroup(context, fRoot, culture);
            }
            return true;
        }


        /// <summary>
        /// Evaluates a condition group against a SifDataObject to determine if
        /// they are a match or not
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <returns>True if the result of evaluating the condition groups is true</returns>
        /// <exception cref="OpenADK.Library.AdkSchemaException">If the condition contains references to invalid elements</exception>
        private bool EvaluateConditionGroup(SifXPathContext context,
                                             ConditionGroup grp,
                                             CultureInfo culture)
        {
            Condition[] conds = grp.Conditions;
            if (conds.Length > 0)
            {
                bool returnOnFirstMatch = grp.Operator == GroupOperator.Or ? true : false;

                foreach (Condition c in conds)
                {
                    if ((EvaluateCondition(context, c, culture)) == returnOnFirstMatch)
                    {
                        // If this is an OR group, return true on the first match
                        // If this is an AND Group, return false on the first failure
                        return returnOnFirstMatch;
                    }
                }
                // None of the conditions matched the returnOnFirstMathValue. Therefore,
                // return the opposite value
                return !returnOnFirstMatch;
            }
            else
            {
                return EvaluateConditionGroups(context, grp.Operator, grp.Groups, culture);
            }
        }


        /// <summary>
        /// Evaluates the condition groups and returns True if the Operator is OR and at least
        /// one of the groups evaluates to TRUE. If the Operator is AND, all of the condition
        /// groups have to evaluate to TRUE
        /// </summary>
        /// <param name="op"></param>
        /// <param name="grps"></param>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="OpenADK.Library.AdkSchemaException">If the condition contains references to invalid elements</exception>
        private bool EvaluateConditionGroups(SifXPathContext context,
                                              GroupOperator op,
                                              ConditionGroup[] grps,
                                              CultureInfo culture)
        {
            bool isMatch = true;
            for (int c = 0; c < grps.Length; c++)
            {
                bool singleMatch = EvaluateConditionGroup(context, grps[c], culture);
                if (op == GroupOperator.Or)
                {
                    if (singleMatch)
                    {
                        // In OR mode, return as soon as we evaluate to True
                        return true;
                    }
                    isMatch |= singleMatch;
                }
                else
                {
                    isMatch &= singleMatch;
                }
                // As soon as the evaluation fails, return
                if (!isMatch)
                {
                    return false;
                }
            }
            return isMatch;
        }


        /// <summary>
        /// Evaluates a single SIF_Condition against an object and returns whether it matches or not
        /// </summary>
        /// <param name="cond"></param>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="OpenADK.Library.AdkSchemaException">If the condition contains references to invalid elements</exception>
        private bool EvaluateCondition(SifXPathContext context,
                                        Condition cond,
                                        CultureInfo culture)
        {
            // TODO: Add support for comparison using the SIF Data Types
            Element def = context.GetElementOrAttribute(cond.GetXPath());
            String conditionValue = cond.Value;


            String elementValue = null;
            if (def != null)
            {
                SifSimpleType value = def.SifValue;
                if (value != null)
                {
                    // Format the value to string, based on the query version
                    elementValue = value.ToString(EffectiveVersion);
                }
                else
                {
                    // TODO: Not sure if this would ever return a value if the above does not
                    elementValue = def.TextValue;
                }
            }

            if (elementValue == null || conditionValue == null)
            {
                // Don't use standard comparision because it will fail. If
                // one or the other value is null, it cannot be compared, except for
                // if the operator is EQ or NOT
                bool bothAreNull = (elementValue == null && conditionValue == null);
                switch (cond.Operators)
                {
                    case ComparisonOperators.EQ:
                    case ComparisonOperators.GE:
                    case ComparisonOperators.LE:
                        return bothAreNull;
                    case ComparisonOperators.NE:
                        return !bothAreNull;
                    default:
                        // For any other operator, the results are indeterminate with
                        // null values. Return false in this case.
                        return false;
                }
            }

            int compareLevel = String.Compare(elementValue, conditionValue, false, culture);

            switch (cond.Operators)
            {
                case ComparisonOperators.EQ:
                    return compareLevel == 0;
                case ComparisonOperators.NE:
                    return compareLevel != 0;
                case ComparisonOperators.GT:
                    return compareLevel > 0;
                case ComparisonOperators.LT:
                    return compareLevel < 0;
                case ComparisonOperators.GE:
                    return compareLevel >= 0;
                case ComparisonOperators.LE:
                    return compareLevel <= 0;
            }
            return false;
        }

        ///<summary>
        /// Sets the SIFContext that this query should apply to
        /// </summary> 
        /// <value>The SIF Context that this query applies to</value>
        public SifContext SifContext
        {
            get { return fContext; }
            set { fContext = value; }
        }


        /// <summary>
        /// If SIFElement restrictions are placed on this query, this method
        /// will take the SIFDataObject and call setChanged(false). It will then 
        /// go through each of the SIFElement restrictions, resolve them, and 
        /// call setChanged(true) on those elements only. This will cause the
        /// object to be rendered properly using SIFWriter.
        /// </summary>
        /// <param name="sdo"></param>
        public void SetRenderingRestrictionsTo(SifDataObject sdo)
        {
            if (sdo == null || fFieldRestrictions == null)
            {
                return;
            }

            sdo.SetChanged(false);

            // Go through and only set the filtered items to true
            SifXPathContext context = SifXPathContext.NewSIFContext(sdo);
            foreach (ElementRef elementRef in fFieldRestrictions)
            {
                String xPath = elementRef.XPath;
                Element e = context.GetElementOrAttribute(xPath);
                if (e != null)
                {
                    e.SetChanged();
                }
            }
            sdo.EnsureRootElementRendered();
        }
    }
}
