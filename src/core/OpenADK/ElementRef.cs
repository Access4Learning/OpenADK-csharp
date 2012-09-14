//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library
{
    /// <summary>
    /// Represents a reference to a SIF Element, identified by the root element
    /// and a relative XPath
    /// </summary>
    public class ElementRef
    {
        /// <summary>
        /// The field being referenced, or null if it cannot be parsed
        /// </summary>
        private IElementDef fField;

        /// <summary>
        /// The XPath
        /// </summary>
        private String fXPath;

        /// <summary>
        /// Creates an ElementRef instance by XPath
        /// </summary>
        /// <param name="root"></param>
        /// <param name="xPath"></param>
        /// <param name="version"></param>
        public ElementRef(IElementDef root, String xPath, SifVersion version)
        {
            fField = Adk.Dtd.LookupElementDefBySQP(root, xPath);
            fXPath = xPath;
        }

        /// <summary>
        /// Creates an ElementRef instance using the specified IElementDef as the reference path
        /// </summary>
        /// <param name="root"></param>
        /// <param name="referencedField"></param>
        /// <param name="version"></param>
        public ElementRef(IElementDef root, IElementDef referencedField, SifVersion version)
        {
            fField = referencedField;
            fXPath = referencedField.GetSQPPath(version);
        }


        /// <summary>
        /// Returns the referenced field
        /// </summary>
        /// <returns>The referenced field or null if it cannot be resolved</returns>
        public IElementDef Field
        {
            get { return fField; }
        }

        /// <summary>
        /// the XPath representation of this ElementRef
        /// </summary>
        /// <returns>the XPath representation of this ElementRef</returns>
        public String XPath
        {
            get { return fXPath; }
        }
    }
}
