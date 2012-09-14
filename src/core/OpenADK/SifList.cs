//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OpenADK.Library
{
    /// <summary>
    /// Represents a SIF Repeatable Element list container element.
    /// </summary>
    /// <typeparam name="TValue">The specific type of SIFElement contained in the list</typeparam>
    [Serializable]
    public abstract class SifList<TValue>
        :
            SifElement,
            ICollection,
            IEnumerable,
            ICollection<TValue>,
            IEnumerable<TValue>
        where TValue : SifElement
    {
        protected SifList(IElementDef def)
            : base(def)
        {
        }


        /// <summary>
        /// Validates that the child being added is the same type as TValue
        /// </summary>
        /// <param name="element">The child that is about to be added</param>
        protected override void EvaluateChild(SifElement element)
        {
            base.EvaluateChild(element);
            if (!(element is TValue))
            {
                _throwInvalidChild(element);
            }
        }

        /// <summary>
        /// Returns the count of the children of this list
        /// </summary>
        public int Count
        {
            get { return ChildCount; }
        }

        /// <summary>
        /// Retrieves the all the elements that match the conditions defined by the specified predicate. 
        /// </summary>
        /// <param name="match">The Predicate delegate that defines the conditions of the elements to search for</param>
        /// <returns>A List containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty List</returns>
        /// <exception cref="NotSupportedException">Can be thrown if the SIFElement uses a custom list type that doesn't support FindAll(). 
        /// However, this will not occur with normal usage of the ADK. </exception>
        public IList<TValue> FindAll(Predicate<TValue> match)
        {
            // By default, we can support this really easily because
            // we happen to know that the base class uses the List<T> class
            // as it's implementation. If we need to make this support any generic
            // IList<T> implementation, we will need to add a little more code.
            List<TValue> attempt = ChildList() as List<TValue>;
            if (attempt != null)
            {
                return attempt.FindAll(match);
            }
            else
            {
                throw new NotSupportedException
                    ("FindAll<T> is not supported by the underlying list");
            }
        }

        /// <summary>
        /// Gets an array of the children of this repeatable element list. Note: SifList implements
        /// the ICollection interface, and it is more efficient to use the ICollection interface to
        /// retrieve children elements.
        /// </summary>
        /// <returns></returns>
        public TValue[] ToArray()
        {
            TValue[] children = new TValue[ChildCount];
            CopyTo(children, 0);
            return children;
        }

        /// <summary>
        /// Returns the item from the list with the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue ItemAt(int index)
        {
            return ChildList()[index] as TValue;
        }

 
        /// <summary>
        /// Adds a set of child SifElement to this List container element
        /// </summary>
        /// <param name="items"></param>
        public virtual void AddRange(params TValue[] items)
        {
           if (items == null)
           {
              return;
           }
            foreach (TValue item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Completely replaces the elements of this list
        /// </summary>
        /// <param name="items"></param>
       public virtual void SetChildren(params TValue[] items)
       {
          this.Clear();
          this.AddRange(items);
       }


        #region ICollection Members

        /// <summary>
        /// Implementation of ICollection.CopyTo();
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        void ICollection.CopyTo(Array array,
                                int index)
        {
            if (ChildCount > 0)
            {
                lock (fSyncLock)
                {
                    ((ICollection) ChildList()).CopyTo(array, index);
                }
            }
        }

        /// <summary>
        /// Implementation of ICollection.Count
        /// </summary>
        int ICollection.Count
        {
            get { return ChildCount; }
        }

        /// <summary>
        /// Implementation of ICollection.IsSynchronized
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection) ChildList()).IsSynchronized; }
        }

        /// <summary>
        /// Implemenation of ICollection.SyncRoot
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return fSyncLock; }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Implementation of IEnumerable.getEnumerator();
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return ChildList().GetEnumerator();
        }

        #endregion

        #region ICollection<TValue> Members

        /// <summary>
        /// Adds a child SifElement to this List container element
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(TValue item)
        {
            AddChild(item);
        }

        /// <summary>
        /// Removes all SifElements from this list
        /// </summary>
        public void Clear()
        {
            lock (fSyncLock)
            {
                if (ChildCount > 0)
                {
                    IList<SifElement> v = ChildList();
                    // Go through the vector in reverse order, removing any children of this type
                    for (int i = v.Count - 1; i >= 0; i--)
                    {
                        SifElement o = v[i];
                        o.Parent = null;
                        v.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if this list of SIFElements already contains the specified list
        /// item. 
        /// </summary>
        /// <remarks>
        /// The comparison is done using reference comparison, not value comparison. 
        /// Therefore, this method will only return true if the same exact object that is
        /// contained in the list is present. 
        /// </remarks>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TValue item)
        {
            lock (fSyncLock)
            {
                if (ChildCount == 0)
                {
                    return false;
                }
                else
                {
                    return ChildList().Contains(item);
                }
            }
        }

        /// <summary>
        /// Copies the child elements to the specified array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(TValue[] array,
                           int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("Array cannot be null", "array");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex is less than 0", "arrayIndex");
            }
            lock (fSyncLock)
            {
                int count = ChildCount;
                if ((arrayIndex + count) > array.Length)
                {
                    throw new ArgumentException
                        ("The number of elements in the source List is greater than " +
                         "the available space from arrayIndex to the end of the destination array.",
                         "arrayIndex");
                }
                if (ChildCount > 0)
                {
                    foreach (SifElement child in ChildList())
                    {
                        TValue element = child as TValue;
                        if (element == null)
                        {
                            _throwInvalidChild(child);
                        }
                        array[arrayIndex++] = (TValue) child;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the count of items in this collection. Overriden explicitly to retain ChildCount as the
        /// primary property to use
        /// </summary>
        int ICollection<TValue>.Count
        {
            get { return ChildCount; }
        }

        /// <summary>
        /// Returns False because a SifList is never read-only.
        /// </summary>
        bool ICollection<TValue>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes a child element from this list
        /// </summary>
        /// <param name="item"></param>
        /// <returns><c>true</c> if the elements was found and removed, otherwise<c>false</c></returns>
        /// <exception cref="ArgumentNullException">Thrown if the parameter is null</exception>
        public bool Remove(TValue item)
        {
            return RemoveChild(item);
        }

        #endregion

        #region IEnumerable<TValue> Members

        /// <summary>
        /// Gets the enumerator for this list
        /// </summary>
        /// <returns></returns>
        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return new SifListEnumerator(ChildList().GetEnumerator());
        }

        #endregion

        /// <summary>
        /// Throws an exception if a developer attempts to add the wrong type
        /// of child to this list, which is typed to contain only a single type of element
        /// </summary>
        /// <param name="child"></param>
        private void _throwInvalidChild(SifElement child)
        {
            throw new InvalidCastException
                (string.Format
                     ("This <{0}> instance is in an invalid state. Cannot contain a child of type <{1}>.",
                      ElementDef.Name, child.ElementDef.Name));
        }

        /// <summary>
        /// .Net Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        protected SifList(SerializationInfo info,
                          StreamingContext context)
            : base(info, context)
        {
        }


        private class SifListEnumerator : IEnumerator<TValue>
        {
            private IEnumerator fWrappedEnumerator;

            public SifListEnumerator(IEnumerator e)
            {
                fWrappedEnumerator = e;
            }

            #region IEnumerator<TValue> Members

            public TValue Current
            {
                get { return (TValue) fWrappedEnumerator.Current; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                IDisposable dis = fWrappedEnumerator as IDisposable;
                if (dis != null)
                {
                    dis.Dispose();
                }
            }

            #endregion

            #region IEnumerator Members

            object IEnumerator.Current
            {
                get { return fWrappedEnumerator.Current; }
            }

            public bool MoveNext()
            {
                return fWrappedEnumerator.MoveNext();
            }

            public void Reset()
            {
                fWrappedEnumerator.Reset();
            }

            #endregion
        }
    }
}
