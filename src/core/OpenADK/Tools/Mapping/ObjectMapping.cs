//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Xml;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  An ObjectMapping defines a set of field mapping rules for a specific SIF Data
    /// Object type such as StudentPersonal, StaffPersonal, or BusInfo. ObjectMapping
    /// is comprised of zero or more FieldMapping children.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public class ObjectMapping
    {
        /// <summary>  Gets the optional DOM XmlElement associated with this ObjectMapping instance. 
        /// The DOM XmlElement is usually set by the parent Mappings object when an 
        /// ObjectMapping is populated from a DOM Document.
        /// </summary>
        /// <summary>  Sets the optional DOM XmlElement associated with this ObjectMapping instance. 
        /// The DOM XmlElement is usually set by the parent Mappings object when an 
        /// ObjectMapping is populated from a DOM Document.
        /// </summary>
        public XmlElement XmlElement
        {
            get { return fNode; }

            set { fNode = value; }
        }

        /// <summary>  Gets the SIF Data Object type of this ObjectMapping</summary>
        /// <returns> An object type name such as "StudentPersonal" or "BusInfo"
        /// </returns>
        public string ObjectType
        {
            get { return fObjType; }
        }

        /// <summary>  Count the number of FieldMapping definitions.</summary>
        public int RuleCount
        {
            get { return fFieldRules == null ? 0 : fFieldRules.Count; }
        }

        /// <summary>  The SIF Data Object type (e.g. StudentPersonal)</summary>
        protected internal string fObjType;

        /// <summary>  Optional DOM XmlElement from which this ObjectType was produced</summary>
        protected internal XmlElement fNode;

        /// <summary>  Vector of FieldMappings</summary>
        protected internal List<FieldMapping> fFieldRules = null;

        /// <summary>  The parent Mappings object</summary>
        protected internal Mappings fParent;


        /// <summary>  Constructor</summary>
        /// <param name="objType">The name of a SIF Data Object (e.g. "StudentPersonal")
        /// </param>
        public ObjectMapping(string objType)
            : this(objType, null)
        {
        }

        /// <summary>  Constructor</summary>
        /// <param name="objType">The name of a SIF Data Object (e.g. "StudentPersonal")
        /// </param>
        /// <param name="node">The optional DOM XmlElement from which this ObjectType was produced
        /// </param>
        public ObjectMapping(string objType,
                             XmlElement node)
        {
            fObjType = objType;
            fNode = node;
        }

        /// <summary>  Creates a copy this ObjectMapping instance.
        /// 
        /// This method performs a "deep copy", such that a clone is made of each 
        /// child FieldMapping. The parent of the new ObjectMapping will be the 
        /// Mappings object passed to this function. Any DOM Nodes assigned to this
        /// object or its children are cloned and appended to the parent Mappings's
        /// DOM XmlElement if one exists.
        /// 
        /// </summary>
        /// <returns> A "deep copy" of this object
        /// </returns>
        public ObjectMapping Copy(Mappings newParent)
        {
            //	Create a new ObjectMapping instance
            ObjectMapping m = new ObjectMapping(fObjType);

            //  Copy the DOM XmlElement
            if (fNode != null)
            {
                if (newParent.fNode != null)
                {
                    XmlElement newNode = (XmlElement) newParent.fNode.OwnerDocument.ImportNode( fNode, false );
                    newParent.fNode.AppendChild( newNode );
                    m.fNode = newNode;
                }
            }

            //  Copy fFieldRules
            if (fFieldRules != null)
            {
                if (m.fFieldRules == null)
                {
                    m.fFieldRules = new List<FieldMapping>();
                }

                for (int i = 0; i < fFieldRules.Count; i++)
                {
                    FieldMapping copy = (fFieldRules[i]).Copy(m);
                    m.AddRule(copy);
                }
            }

            return m;
        }

        /// <summary>  Appends a FieldMapping definition
        /// 
        /// </summary>
        /// <param name="mapping">A FieldMapping that defines the rules for mapping a field 
        /// of the application to an element or attribute of a SIF Data Object.
        /// There can only be one FieldMapping per unique field name (i.e. if 
        /// you have defined a FieldMapping rule with a field name of 'STUDENTNUM',
        /// there cannot be another FieldMapping rule with that same field name.) 
        /// To map a single application field to more than one SIF element or 
        /// attribute, create a FieldMapping with a unique field name (e.g. 'STUDENTNUM_2') 
        /// and call the <code>setAlias</code> method to define it as an alias 
        /// of an existing field.
        /// 
        /// </param>
        /// <exception cref="AdkMappingException">  thrown if there is already a FieldMapping
        /// with the specified field name
        /// </exception>
        public void AddRule(FieldMapping mapping)
        {
            InsertRule(mapping, fFieldRules == null ? 0 : fFieldRules.Count, true);
        }

        /// <summary>  Appends a FieldMapping definition
        /// 
        /// </summary>
        /// <param name="mapping">A FieldMapping that defines the rules for mapping a field 
        /// of the application to an element or attribute of a SIF Data Object.
        /// There can only be one FieldMapping per unique field name (i.e. if 
        /// you have defined a FieldMapping rule with a field name of 'STUDENTNUM',
        /// there cannot be another FieldMapping rule with that same field name.) 
        /// To map a single application field to more than one SIF element or 
        /// attribute, create a FieldMapping with a unique field name (e.g. 'STUDENTNUM_2') 
        /// and call the <code>setAlias</code> method to define it as an alias 
        /// of an existing field.
        /// 
        /// </param>
        /// <param name="buildDomTree">true to create a DOM XmlElement element for this 
        /// FieldMapping and append it to the parent XmlElement
        /// 
        /// </param>
        /// <exception cref="AdkMappingException">  thrown if there is already a FieldMapping
        /// with the specified field name
        /// </exception>
        protected internal void AddRule(FieldMapping mapping,
                                        bool buildDomTree)
        {
            InsertRule(mapping, fFieldRules == null ? 0 : fFieldRules.Count, buildDomTree);
        }

        /// <summary>  Insert a FieldMapping definition at the specified index.
        /// 
        /// </summary>
        /// <param name="mapping">A FieldMapping that defines the rules for mapping a field 
        /// of the application to an element or attribute of a SIF Data Object.
        /// There can only be one FieldMapping per unique field name (i.e. if 
        /// you have defined a FieldMapping rule with a field name of 'STUDENTNUM',
        /// there cannot be another FieldMapping rule with that same field name.) 
        /// To map a single application field to more than one SIF element or 
        /// attribute, create a FieldMapping with a unique field name (e.g. 'STUDENTNUM_2') 
        /// and call the <code>setAlias</code> method to define it as an alias 
        /// of an existing field.
        /// 
        /// </param>
        /// <exception cref="AdkMappingException">  thrown if there is already a FieldMapping
        /// with the specified field name
        /// </exception>
        /// <param name="index">The index to insert the rule at</param>
        public void InsertRule(FieldMapping mapping,
                               int index)
        {
            InsertRule(mapping, index, true);
        }

        /// <summary>  Insert a FieldMapping definition at the specified index.
        /// 
        /// </summary>
        /// <param name="mapping">A FieldMapping that defines the rules for mapping a field 
        /// of the application to an element or attribute of a SIF Data Object.
        /// There can only be one FieldMapping per unique field name (i.e. if 
        /// you have defined a FieldMapping rule with a field name of 'STUDENTNUM',
        /// there cannot be another FieldMapping rule with that same field name.) 
        /// To map a single application field to more than one SIF element or 
        /// attribute, create a FieldMapping with a unique field name (e.g. 'STUDENTNUM_2') 
        /// and call the <code>setAlias</code> method to define it as an alias 
        /// of an existing field.
        /// 
        /// </param>
        /// <param name="index">The index to insert the rule at</param>
        /// <param name="buildDomTree">true to create a DOM XmlElement element for this 
        /// FieldMapping and append it to the parent XmlElement
        /// 
        /// </param>
        /// <exception cref="OpenADK.Library.Tools.Mapping.AdkMappingException"> AdkMappingException thrown if there is already a FieldMapping
        /// with the specified field name
        /// </exception>
        protected internal void InsertRule(FieldMapping mapping,
                                           int index,
                                           bool buildDomTree)
        {
            XmlElement relativeTo = null;

            if (fFieldRules == null)
            {
                fFieldRules = new List<FieldMapping>();
            }
            else
            {
                //  Check for duplicate

               foreach( FieldMapping existing in fFieldRules )
               {
                   if( existing.Key == mapping.Key )
                   {
                       throw new AdkMappingException("Duplicate field mapping: " + mapping.Key, null);
                   }
               }

                //	If we'll be building a child DOM XmlElement, find the existing XmlElement
                //	that it will be inserted at
                if (buildDomTree && fNode != null && fFieldRules.Count > index)
                {
                    relativeTo = fFieldRules[index].XmlElement;
                }
            }

            try
            {
                fFieldRules.Insert(index, mapping);
            }
            catch (Exception ex)
            {
                throw new AdkMappingException(ex.ToString(), null);
            }

            if (buildDomTree && fNode != null)
            {
                //  Create and insert a child DOM XmlElement
                XmlElement element = fNode.OwnerDocument.CreateElement(Mappings.XML_FIELD);
                mapping.XmlElement = element;
                mapping.ToXml( element);
    
                if (relativeTo != null)
                {
                    fNode.InsertBefore(element, relativeTo);
                }
                else
                {
                    fNode.AppendChild(element);
                }
            }
        }

        /// <summary>  Remove a FieldMapping definition</summary>
        public void RemoveRule(FieldMapping mapping)
        {
            if (fFieldRules != null)
            {
                fFieldRules.Remove(mapping);

                //  Remove the DOM XmlElement if there is one
                XmlElement n = mapping.XmlElement;
                if (n != null)
                {
                    n.ParentNode.RemoveChild(n);
                }
            }
        }

        /// <summary>  Removes the FieldMapping at the specified index</summary>
        /// <param name="index">The zero-based index of the FieldMapping
        /// </param>
        public void RemoveRule(int index)
        {
            if (fFieldRules != null && index >= 0 && index < fFieldRules.Count)
            {
                FieldMapping existing = fFieldRules[index];

                fFieldRules.RemoveAt(index);

                //  Remove the DOM XmlElement if there is one
                XmlElement n = existing == null ? null : existing.XmlElement;
                if (n != null)
                {
                    n.ParentNode.RemoveChild(n);
                }
            }
        }

        /// <summary>  Return an array of all FieldMapping definitions</summary>
        /// <param name="inherit">True to inherit FieldMapping definitions from the
        /// parent Mappings ancestry
        /// </param>
        public IList<FieldMapping> GetRulesList(bool inherit)
        {
            List<FieldMapping> rules = new List<FieldMapping>();
            if (inherit)
            {
                IDictionary<String, Object> set = new Dictionary<String, Object>();
                // Keep the rules in a list because the ordering of
                // rules is important
                Mappings m = fParent;
                while (m != null)
                {
                    ObjectMapping om = m.GetRules(fObjType, false);
                    if (om != null && om.fFieldRules != null)
                    {
                        foreach (FieldMapping fm in om.fFieldRules)
                        {
                            String key = fm.Key;
                            if (!set.ContainsKey(key))
                            {
                                set.Add(key, null);
                                rules.Add(fm);
                            }
                        }
                    }
                    m = m.fParent;
                }
            }
            else if (fFieldRules != null)
            {
                rules.AddRange(fFieldRules);
            }

            return rules;
        }

        /// <summary>  Gets the FieldMapping at the specified index</summary>
        /// <param name="index">The zero-based index of the FieldMapping
        /// </param>
        public FieldMapping GetRule(int index)
        {
            return
                fFieldRules == null || index >= fFieldRules.Count
                    ? null
                    : fFieldRules[index];
        }

        /// <summary>  Clear all FieldMapping definitions.</summary>
        public void ClearRules()
        {
            if (fFieldRules != null)
            {
                if (fNode != null)
                {
                    foreach (FieldMapping fm in fFieldRules)
                    {
                        XmlElement n = fm.XmlElement;
                        if (n != null)
                        {
                            n.ParentNode.RemoveChild(n);
                        }
                    }
                }
            }
        }
    }
}
