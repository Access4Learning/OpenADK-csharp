//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using OpenADK.Util;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>  An entry in a ValueSet.
    /// 
    /// Each ValueSet entry describes a mapping between a local application value
    /// and a SIF value. Additional fields include a display title and display order
    /// for presenting ValueSet entries in a user interface.
    /// 
    /// </summary>
    /// <author> Data Solutions
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    [Serializable]
    public class ValueSetEntry
    {
        /// <summary>  The name of this entry (i.e. the application-defined value)</summary>
        public string Name;

        /// <summary>  The value of this entry (i.e. the corresponding SIF-defined value)</summary>
        public string Value;

        /// <summary>  An optional title for displaying ValueSetEntries in a user interface</summary>
        public string Title;

        /// <summary>  An optional display order for arranging ValueSetEntries in a user interface</summary>
        public int DisplayOrder;

        /// <summary>  The optional DOM XmlElement that defines this ValueSetEntry in the configuration file</summary>
        [NonSerialized()] public XmlElement Node;


        /// <summary>  Constructor</summary>
        /// <param name="name">The name of this entry (i.e. the application-defined value)
        /// </param>
        /// <param name="val">The value of this entry (i.e. the corresponding SIF-defined value)
        /// </param>
        /// <param name="title">An optional title for displaying ValueSetEntries in a user interface
        /// </param>
        public ValueSetEntry(string name,
                             string val,
                             string title)
            : this(name, val, title, 0)
        {
        }

        /// <summary>  Constructor</summary>
        /// <param name="name">The name of this entry (i.e. the application-defined value)
        /// </param>
        /// <param name="val">The value of this entry (i.e. the corresponding SIF-defined value)
        /// </param>
        /// <param name="title">An optional title for displaying ValueSetEntries in a user interface
        /// </param>
        /// <param name="displayOrder">The order in which to display this entry</param>
        public ValueSetEntry(string name,
                             string val,
                             string title,
                             int displayOrder)
        {
            Name = name;
            Value = val;
            if (!String.IsNullOrEmpty(title))
            {
                Title = title;
            }
            DisplayOrder = displayOrder;
        }

        public void ToXml(XmlElement element)
        {
            element.SetAttribute("name", Name);
            XmlUtils.SetOrRemoveAttribute(element, "title", Title);
            element.InnerText = Value;
        }
    }
}
