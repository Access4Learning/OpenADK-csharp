//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Xml;

namespace OpenADK.Util
{
    public class XmlUtils
    {
        /// <summary>
        /// This method returns the attribute value or NULL if the attribute does not exist, which differs from the behavior of the 
        /// XmlElement.GetAttribute Method;
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetAttributeValue( XmlElement element,
                                                string attributeName )
        {
            XmlAttribute attr = element.GetAttributeNode( attributeName );
            return attr == null ? null : attr.Value;
        }

        /// <summary>
        /// Builds an array of XmlElements from an XmlNodeList
        /// </summary>
        /// <param name="list">an XmlNodeList</param>
        /// <param name="filter">If true, returns only the elements with an "enabled" property set to "True" or "Yes"</param>
        /// <returns>the array of elements</returns>
        /// <remarks>
        /// If there are no elements in the list or that are enabled, an empty array will be returned
        /// </remarks>
        public static XmlElement [] ElementArrayFromNodeList( XmlNodeList list,
                                                              bool filter )
        {
            ArrayList v = new ArrayList();
            IEnumerator enumerator;
            if ( filter ) {
                enumerator = new FilteredElementList( list ).GetEnumerator();
            }
            else {
                enumerator = list.GetEnumerator();
            }
            while ( enumerator.MoveNext() ) {
                v.Add( enumerator.Current );
            }
            return (XmlElement []) v.ToArray( typeof ( XmlElement ) );
        }

        public static XmlElement GetElementByAttribute( XmlElement parent,
                                                        string elementName,
                                                        string attributeName,
                                                        string attributeValue,
                                                        bool filtered )
        {
            if ( parent == null || elementName == null || attributeName == null ||
                 attributeValue == null ) {
                return null;
            }
            // TODO: Implement better filtering in the xpath expression so that the multiple steps of operations are not necessary
            XmlElement element = null;
            string xPath = elementName + "[@" + attributeName + "=\"" + attributeValue + "\"]";
            if ( filtered ) {
                FilteredElementList list = new FilteredElementList( parent.SelectNodes( xPath ) );
                IEnumerator enumerator = list.GetEnumerator();
                if ( enumerator.MoveNext() ) {
                    element = (XmlElement) enumerator.Current;
                }
            }
            else {
                element = (XmlElement) parent.SelectSingleNode( xPath );
            }
            return element;
        }

        /// <summary>
        /// Returns true only if the element has an "enabled" property set to "True" or "Yes"
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsElementEnabled( XmlElement element )
        {
            string val = element.GetAttribute( "enabled" ).ToUpper();
            return val.Length == 0 || val == "TRUE" || val == "YES";
        }

        /// <summary>  Sets the value of a <code>&lt;property&gt;</code> child of the specified
        /// node. If a <code>&lt;property&gt;</code> element already exists, its value
        /// is updated; otherwise a new element is appended to the node.
        /// 
        /// </summary>
        /// <param name="parentNode">The parent node of the property
        /// </param>
        /// <param name="property">The name of the property
        /// </param>
        /// <param name="value">The property value
        /// </param>
        public static void SetProperty( XmlElement parentNode,
                                        string property,
                                        string val )
        {
            XmlElement propN =
                GetElementByAttribute
                    ( parentNode, AdkXmlConstants.Property.ELEMENT, AdkXmlConstants.Property.NAME,
                      property, false );

            if ( propN == null ) {
                propN = parentNode.OwnerDocument.CreateElement( AdkXmlConstants.Property.ELEMENT );
                // Search for another node in the config that matches the prefix of the current
                // property element and insert the node immediately after it, if found.
                // This helps to keep the property file in an easier to read format.
                int loc = property.Length - 1;
                XmlNode lastSibling = null;
                while ( lastSibling == null && (loc = property.LastIndexOf( '.', loc - 1 )) > -1 ) {
                    string prefix = property.Substring( 0, loc + 1 );
                    lastSibling = FindLastPropertySibling( parentNode, prefix );
                }

                if ( lastSibling == null ) {
                    // Find the last property element
                    lastSibling = FindLastPropertySibling( parentNode, null );
                }

                if ( lastSibling != null ) {
                    parentNode.InsertAfter( propN, lastSibling );
                }
                else {
                    parentNode.AppendChild( propN );
                }
            }

            propN.SetAttribute( AdkXmlConstants.Property.NAME, property );
            propN.SetAttribute( AdkXmlConstants.Property.VALUE, val );
        }

        /// <summary>
        /// Searches for the last property element with the specified prefix
        /// </summary>
        /// <param name="parent">The parent node to search</param>
        /// <param name="propertyPrefix">The prefix to search for, or NULL if the last property element is to be returned</param>
        /// <returns>The last property element found with the specified prefix or null, if none found</returns>
        private static XmlElement FindLastPropertySibling( XmlElement parent,
                                                           string propertyPrefix )
        {
            XmlNodeList children = parent.ChildNodes;
            for ( int a = children.Count - 1; a > -1; a-- ) {
                XmlElement child = children[a] as XmlElement;
                if ( child != null &&
                     child.Name == AdkXmlConstants.Property.ELEMENT ) {
                    if ( propertyPrefix == null ||
                         child.GetAttribute( AdkXmlConstants.Property.NAME ).StartsWith
                             ( propertyPrefix ) ) {
                        return child;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Filters the list of elements, and returns only those with an "enabled" property set to "True" or "Yes"
        /// </summary>
        public class FilteredElementList : IEnumerable
        {
            public FilteredElementList( XmlNodeList list )
            {
                fNodeList = list;
            }

            public IEnumerator GetEnumerator()
            {
                return new FilteredEnumerator( fNodeList.GetEnumerator() );
            }

            private XmlNodeList fNodeList;

            private class FilteredEnumerator : EnumeratorWrapper
            {
                public FilteredEnumerator( IEnumerator enumerator )
                    : base( enumerator ) {}

                public override bool MoveNext()
                {
                    while ( this.WrappedEnumerator.MoveNext() ) {
                        XmlElement element = (XmlElement) this.Current;
                        if ( IsElementEnabled( element ) ) {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Allows safe iteration of Xml Child elements of a given element
        /// </summary>
        /// <remarks>
        /// Example of code without using this class
        /// <code>
        /// foreach( XmlNode a_o in new a_XmlDoc.DocumentElement )
        /// {
        ///		if( a_o is XmlElement )
        ///		{
        ///			XmlElement a_Element = (XmlElement)a_o
        /// 		Console.WriteLine( a_Element.Name );
        /// 		foreach( XmlNode a_ChildElement in a_Element )
        /// 		{
        /// 			if( a_ChildElement is XmlElement )
        ///				{
        /// 				Console.WriteLine( "   " + ((XmlElement)a_ChildElement).Name );
        /// 			}
        /// 		}
        /// 	}
        /// }		
        /// </code>
        /// Example Usage using the XmlElementEnumerator
        /// <code>
        /// foreach( XmlElement a_Element in new XmlElementEnumerator( a_XmlDoc.DocumentElement ) )
        /// {
        /// 	Console.WriteLine( a_Element.Name );
        /// 	foreach( XmlElement a_ChildElement in new XmlElementEnumerator( a_Element ) )
        /// 	{
        /// 		Console.WriteLine( "   " + a_ChildElement.Name );
        /// 	}
        /// }		
        /// </code>
        /// </remarks>
        public sealed class XmlElementEnumerator : IEnumerable
        {
            public XmlElementEnumerator( XmlElement parentNode )
            {
                fParent = parentNode;
            }

            public XmlElementEnumerator( XmlNodeList parentList )
            {
                fParent = parentList;
            }


            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ElementEnumerator( fParent.GetEnumerator() );
            }

            private class ElementEnumerator : IEnumerator
            {
                internal ElementEnumerator( IEnumerator enumerator )
                {
                    fNodeEnumerator = enumerator;
                }

                public bool MoveNext()
                {
                    while ( fNodeEnumerator.MoveNext() ) {
                        if ( fNodeEnumerator.Current is XmlElement ) {
                            return true;
                        }
                    }
                    return false;
                }

                public void Reset()
                {
                    fNodeEnumerator.Reset();
                }

                public object Current
                {
                    get { return fNodeEnumerator.Current; }
                }

                private IEnumerator fNodeEnumerator;
            }

            private IEnumerable fParent;
        }

        /// <summary>
        /// Sets an XML Attribute, or removes it if the value is null
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        public static void SetOrRemoveAttribute(XmlElement element, string attributeName, string value)
        {
            if( value == null )
            {
                element.RemoveAttribute( attributeName );
            } else
            {
                element.SetAttribute(attributeName, value);
            }
        }

        /// <summary>
        /// Returns the first child element found with the specified name, ignoring differences in case
        /// </summary>
        /// <param name="element">The element to search</param>
        /// <param name="nodeName">The element tag name to search for</param>
        /// <returns>The matching child element with the specified name, or NULL if not found</returns>
        public static XmlElement GetFirstElementIgnoreCase(XmlElement element, string nodeName )
        {
            if (nodeName != null && element != null)
            {
                foreach( XmlNode child in element.ChildNodes )
                {
                    if( child.NodeType == XmlNodeType.Element && String.Compare( nodeName, child.Name, true ) == 0 ) 
                    {
                        return (XmlElement)child;
                    }
                }
            }

            return null;
        }
    }
}
