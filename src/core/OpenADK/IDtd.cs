//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library
{
    /// <summary>  Classes that implement the DTD interface provide information about the
    /// schema of elements and attributes. For example, the SifDtd class implements
    /// this interface to define all elements comprising the Schools Interoperability
    /// Framework.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    public interface IDtd
    {
        /// <summary>  Lookup an IElementDef object describing an element or attribute</summary>
        /// <param name="key">The name of the element in the form "parent_field", where
        /// <i>parent</i> is the name of the parent element and <i>field</i> is
        /// the name of the child element or attribute (e.g. "SIF_Ack_SIF_Header",
        /// "StudentPersonal_Name", etc.)
        /// </param>
        /// <returns> The IElementDef that provides metadata about the requested
        /// element or null if not found
        /// </returns>
        IElementDef LookupElementDef(string key);

        /// <summary>
        /// Lookup an IElementDef object describing an element or attribute
        /// </summary>
        /// <param name="parent">The parent IElementDef</param>
        /// <param name="childTag">The tag name of the child element</param>
        /// <returns>The IElementDef that provides metadata about the requested
        /// element or null if not found
        /// </returns>
        IElementDef LookupElementDef(IElementDef parent, String childTag);

        /// <summary>  Gets the namespace associated with this DTD</summary>
        /// <param name="version">The SIF Version
        /// </param>
        /// <returns> The namespace (e.g. the "xmlns" value for SIF_Messages)
        /// </returns>
        string GetNamespace(SifVersion version);

        /// <summary>
        /// Gets the SifFormatter instance to use for this specified version of SIF
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        SifFormatter GetFormatter(SifVersion version);

        /// <summary>  Gets the element tag corresponding to a type ID</summary>
        /// <param name="type">A type identifier defined by the class that implements this
        /// interface (e.g. <c>SifDtd.MSGTYP_PROVIDE</c>)
        /// </param>
        /// <returns> The tag name of the element (e.g. "SIF_Provide")
        /// </returns>
        string GetElementTag(int type);

        /// <summary>  Gets the type ID corresponding to an element tag name</summary>
        /// <param name="name">The tag name of an element (e.g. "SIF_Provide")
        /// </param>
        /// <returns> A type identifier defined by the class that implements this
        /// interface (e.g. <c>SifDtd.MSGTYP_PROVIDE</c>)
        /// </returns>
        SifMessageType GetElementType(string name);

        IElementDef LookupElementDefBySQP(IElementDef relativeTo, string query);

        String TranslateSQP(IElementDef objectType, String path, SifVersion version);

        String SDOAssembly { get;}
        SifDataObject CreateSIFDataObject(IElementDef objType);
        String BaseNamespace{get;}
    }
}
