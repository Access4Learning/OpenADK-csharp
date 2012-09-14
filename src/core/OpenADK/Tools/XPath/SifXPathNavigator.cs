//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using System.Xml.XPath;

namespace OpenADK.Library.Tools.XPath
{
    /// <summary>
    /// Implements an XPathNavigator over a SIF Element
    /// </summary>
    public class SifXPathNavigator : XPathNavigator
    {
        private INavCursor fCursor;
        private readonly XmlNameTable fNameTable;
        private readonly SifXsltContext fParentContext;


        /// <summary>
        /// Creates an instance of SIFXPathNavigator
        /// </summary>
        /// <param name="context">The SIFXPathContext around this navigator</param>
        /// <param name="nameTable">The nametable</param>
        /// <param name="cursor">The cursor</param>
        private SifXPathNavigator( SifXsltContext context, XmlNameTable nameTable, INavCursor cursor )
        {
            fParentContext = context;
            fNameTable = nameTable;
            fCursor = cursor;
        }

        /// <summary>
        /// Creates an instance of SIFXPathNavigator
        /// </summary>
        /// <param name="context">The SIFXPathContext around this navigator</param>
        /// <param name="pointer">The pointer that this navigator initial points to </param>
        internal SifXPathNavigator( SifXsltContext context, INodePointer pointer )
        {
            fParentContext = context;
            fNameTable = new NameTable();
            fCursor = new RootCursor( pointer );
        }


        ///<summary>
        ///When overridden in a derived class, creates a new <see cref="T:System.Xml.XPath.XPathNavigator"></see> positioned at the same node as this <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A new <see cref="T:System.Xml.XPath.XPathNavigator"></see> positioned at the same node as this <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
        ///</returns>
        ///
        public override XPathNavigator Clone()
        {
            SifXPathNavigator navigator = new SifXPathNavigator( fParentContext, fNameTable, fCursor.Clone() );
            return navigator;
        }

        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the first attribute of the current node.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the first attribute of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        public override bool MoveToFirstAttribute()
        {
            INodeIterator iterator = fCursor.Current.GetAttributes();
            if ( iterator != null )
            {
                NavigableIterator navIterator = new NavigableIterator( iterator, fCursor );
                if ( navIterator.MoveNext() )
                {
                    fCursor = navIterator;
                    return true;
                }
            }
            // No Attributes defined
            return false;
        }


        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the next attribute.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the next attribute; false if there are no more attributes. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        public override bool MoveToNextAttribute()
        {
            return fCursor.MoveNext();
        }

        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the first namespace node that matches the <see cref="T:System.Xml.XPath.XPathNamespaceScope"></see> specified.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the first namespace node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        ///<param name="namespaceScope">An <see cref="T:System.Xml.XPath.XPathNamespaceScope"></see> value describing the namespace scope. </param>
        public override bool MoveToFirstNamespace( XPathNamespaceScope namespaceScope )
        {
            return false;
        }

        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the next namespace node matching the <see cref="T:System.Xml.XPath.XPathNamespaceScope"></see> specified.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the next namespace node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        ///<param name="namespaceScope">An <see cref="T:System.Xml.XPath.XPathNamespaceScope"></see> value describing the namespace scope. </param>
        public override bool MoveToNextNamespace( XPathNamespaceScope namespaceScope )
        {
            return false;
        }

        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the next sibling node of the current node.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the next sibling node; otherwise, false if there are no more siblings or if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is currently positioned on an attribute node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        public override bool MoveToNext()
        {
            return fCursor.MoveNext();
        }

        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the previous sibling node of the current node.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the previous sibling node; otherwise, false if there is no previous sibling node or if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is currently positioned on an attribute node. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        public override bool MoveToPrevious()
        {
            return fCursor.MovePrevious();
        }

        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the first child node of the current node.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the first child node of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        public override bool MoveToFirstChild()
        {
            INodeIterator iterator = fCursor.Current.GetChildren();
            if ( iterator != null )
            {
                NavigableIterator navIterator = new NavigableIterator( iterator, fCursor );
                if ( navIterator.MoveNext() )
                {
                    fCursor = navIterator;
                    return true;
                }
            }
            // No Attributes defined
            return false;
        }

        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the parent node of the current node.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the parent node of the current node; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        public override bool MoveToParent()
        {
            if ( fCursor.ParentIterator != null )
            {
                fCursor = fCursor.ParentIterator;
                return true;
            }
            return false;
        }

        ///<summary>
        ///When overridden in a derived class, moves the <see cref="T:System.Xml.XPath.XPathNavigator"></see> to the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving to the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator"></see>; otherwise, false. If false, the position of the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is unchanged.
        ///</returns>
        ///
        ///<param name="other">The <see cref="T:System.Xml.XPath.XPathNavigator"></see> positioned on the node that you want to move to. </param>
        public override bool MoveTo( XPathNavigator other )
        {
            if ( other is SifXPathNavigator )
            {
                SifXPathNavigator sxn = (SifXPathNavigator) other;
                fCursor = sxn.fCursor.Clone();
                return true;
            }
            return false;
        }

        ///<summary>
        ///When overridden in a derived class, moves to the node that has an attribute of type ID whose value matches the specified <see cref="T:System.String"></see>.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Xml.XPath.XPathNavigator"></see> is successful moving; otherwise, false. If false, the position of the navigator is unchanged.
        ///</returns>
        ///
        ///<param name="id">A <see cref="T:System.String"></see> representing the ID value of the node to which you want to move.</param>
        public override bool MoveToId( string id )
        {
            // Retrieving elements by ID is not currently supported in SIF or the ADK
            return false;
        }

        ///<summary>
        ///When overridden in a derived class, determines whether the current <see cref="T:System.Xml.XPath.XPathNavigator"></see> is at the same position as the specified <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the two <see cref="T:System.Xml.XPath.XPathNavigator"></see> objects have the same position; otherwise, false.
        ///</returns>
        ///
        ///<param name="other">The <see cref="T:System.Xml.XPath.XPathNavigator"></see> to compare to this <see cref="T:System.Xml.XPath.XPathNavigator"></see>.</param>
        public override bool IsSamePosition( XPathNavigator other )
        {
            return ReferenceEquals( UnderlyingObject, other.UnderlyingObject );
        }

        ///<summary>
        ///When overridden in a derived class, gets the <see cref="T:System.Xml.XmlNameTable"></see> of the <see cref="T:System.Xml.XPath.XPathNavigator"></see>.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Xml.XmlNameTable"></see> object enabling you to get the atomized version of a <see cref="T:System.String"></see> within the XML document.
        ///</returns>
        ///
        public override XmlNameTable NameTable
        {
            get { return fNameTable; }
        }

        ///<summary>
        ///When overridden in a derived class, gets the <see cref="T:System.Xml.XPath.XPathNodeType"></see> of the current node.
        ///</summary>
        ///
        ///<returns>
        ///One of the <see cref="T:System.Xml.XPath.XPathNodeType"></see> values representing the current node.
        ///</returns>
        ///
        public override XPathNodeType NodeType
        {
            get { return fCursor.Current.NodeType; }
        }

        ///<summary>
        ///When overridden in a derived class, gets the <see cref="P:System.Xml.XPath.XPathNavigator.Name"></see> of the current node without any namespace prefix.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that contains the local name of the current node, or <see cref="F:System.String.Empty"></see> if the current node does not have a name (for example, text or comment nodes).
        ///</returns>
        ///
        public override string LocalName
        {
            get { return fNameTable.Add( fCursor.Current.Name ); }
        }

        ///<summary>
        ///When overridden in a derived class, gets the qualified name of the current node.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that contains the qualified <see cref="P:System.Xml.XPath.XPathNavigator.Name"></see> of the current node, or <see cref="F:System.String.Empty"></see> if the current node does not have a name (for example, text or comment nodes).
        ///</returns>
        ///
        public override string Name
        {
            get { return fNameTable.Add( fCursor.Current.Name ); }
        }

        ///<summary>
        ///When overridden in a derived class, gets the namespace URI of the current node.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that contains the namespace URI of the current node, or <see cref="F:System.String.Empty"></see> if the current node has no namespace URI.
        ///</returns>
        ///
        public override string NamespaceURI
        {
            get { return fNameTable.Add( string.Empty ); }
        }

        ///<summary>
        ///When overridden in a derived class, gets the namespace prefix associated with the current node.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that contains the namespace prefix associated with the current node.
        ///</returns>
        ///
        public override string Prefix
        {
            get { return fNameTable.Add( string.Empty ); }
        }

        ///<summary>
        ///When overridden in a derived class, gets the base URI for the current node.
        ///</summary>
        ///
        ///<returns>
        ///The location from which the node was loaded, or <see cref="F:System.String.Empty"></see> if there is no value.
        ///</returns>
        ///
        public override string BaseURI
        {
            get { return string.Empty; }
        }

        ///<summary>
        ///When overridden in a derived class, gets a value indicating whether the current node is an empty element without an end element tag.
        ///</summary>
        ///
        ///<returns>
        ///Returns true if the current node is an empty element; otherwise, false.
        ///</returns>
        ///
        public override bool IsEmptyElement
        {
            get { return fCursor.Current.IsEmptyElement; }
        }

        public override XPathExpression Compile( string xpath )
        {
            XPathExpression compiled = base.Compile( xpath );
            compiled.SetContext( fParentContext );
            return compiled;
        }

        ///<summary>
        ///When overridden in a derived class, gets the string value of the item.
        ///</summary>
        ///
        ///<returns>
        ///The string value of the item.
        ///</returns>
        ///
        public override string Value
        {
            get
            {
                if ( fCursor.Current != null )
                {
                    object value = fCursor.Current.Value;
                    if ( value != null )
                    {
                        return value.ToString();
                    }
                }
                return null;
            }
        }

        public override object TypedValue
        {
            get
            {
                if ( fCursor.Current != null )
                {
                    return fCursor.Current.Value;
                }
                return null;
            }
        }

        public INodePointer UnderlyingPointer
        {
            get { return fCursor.Current; }
        }

        public override object UnderlyingObject
        {
            get { return fCursor.Current.Node; }
        }


        private interface INavCursor
        {
            INodePointer Current { get; }

            INavCursor ParentIterator { get; }

            bool MovePrevious();
            bool MoveNext();
            INavCursor Clone();
            XPathNodeType NodeType { get; }
        }

        private class RootCursor : INavCursor
        {
            private INodePointer fRoot;
            public RootCursor( INodePointer root )
            {
                fRoot = root;
            }

            public INodePointer Current
            {
                get { return fRoot; }
            }

            public INavCursor ParentIterator
            {
                get { return null; }
            }

            public bool MovePrevious()
            {
                return false;
            }

            public bool MoveNext()
            {
                return false;
            }

            public INavCursor Clone()
            {
                return new RootCursor( fRoot );
            }

            public XPathNodeType NodeType
            {
                get { return XPathNodeType.Root; }
            }
        }

        private class NavigableIterator : INavCursor
        {
            private INavCursor fParentIterator;
            private readonly INodeIterator fCurrentIterator;
            private INodePointer fCurrentPointer;

            public NavigableIterator( INodeIterator wrappedIterator, INavCursor parentIterator )
            {
                fCurrentIterator = wrappedIterator;
                fParentIterator = parentIterator;
            }

            private NavigableIterator(INodeIterator wrappedIterator, INavCursor parentIterator, INodePointer current)
            {
                fCurrentIterator = wrappedIterator;
                fParentIterator = parentIterator;
                fCurrentPointer = current;
            }


            public INavCursor ParentIterator
            {
                get { return fParentIterator; }
            }


            /// <summary>
            /// Returns true if the iterator has moved to the next node. Otherwise false.
            /// </summary>
            public bool MoveNext()
            {
                INodePointer next = fCurrentIterator.MoveNext();
                if ( next != null )
                {
                    fCurrentPointer = next;
                    return true;
                }
                return false;
            }

            /// <summary>
            /// The current node being pointed to
            /// </summary>
            public INodePointer Current
            {
                get { return fCurrentPointer; }
            }


            /// <summary>
            /// Returns true if the iterator has moved to the previous node. Otherwise false.
            /// </summary>
            public bool MovePrevious()
            {
                INodePointer previous = fCurrentIterator.MovePrevious();
                if ( previous != null )
                {
                    fCurrentPointer = previous;
                    return true;
                }
                return false;
            }

            INavCursor INavCursor.Clone()
            {
                INodeIterator clonedIterator = fCurrentIterator.Clone();
                INavCursor clonedParent = null;
                if ( fParentIterator != null )
                {
                    clonedParent = fParentIterator.Clone();
                }
                INodePointer clonedPointer = null;
                if ( fCurrentPointer != null )
                {
                    clonedPointer = fCurrentPointer.Clone();
                }
                return new NavigableIterator( clonedIterator, clonedParent, clonedPointer );
            }

            public XPathNodeType NodeType
            {
                get { return fCurrentPointer.NodeType; }
            }
        }

        
    }
}
