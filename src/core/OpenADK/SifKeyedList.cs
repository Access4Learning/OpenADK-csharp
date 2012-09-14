//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using OpenADK.Library.Impl;

namespace OpenADK.Library
{
    public class SifKeyedList<TValue> : SifList<TValue>
        where TValue : SifKeyedElement
    {
          /// <summary>
        /// Creates an instance of a SifActionList
        /// </summary>
        /// <param name="def"></param>
        public SifKeyedList( IElementDef def )
            : base( def ) {}


        /// <summary>
        /// .Net Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission( SecurityAction.Demand, SerializationFormatter=true )]
        protected SifKeyedList(SerializationInfo info,
                                 StreamingContext context )
            : base( info, context ) {}


        /// <summary>
        /// Removes the child indicated by the specified key
        /// </summary>
        /// <param name="key">The key for the repeatable child element</param>
        public bool Remove( object key )
        {
            lock ( fSyncLock ) {
                TValue child = this[key];
                if ( child != null ) {
                    return base.Remove( child );
                }
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the child indicated by the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[ object key ]
        {
            get
            {
                foreach ( TValue child in this.ChildList() ) {
                    if ( child.KeyEquals( key ) ) {
                        return child;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Indicates whether the action list contains an element with the specified key.
        /// </summary>
        /// <remarks>Individual elements can be retrieved using the indexer on this class property</remarks>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains( object key )
        {
            return this[key] != null;
        }

        /**
	 *  Gets the child object with the matching element name and key<p>
	 *  @param name The version-independent element name. Note the element name
	 *      is not necessarily the same as the element tag, which is version
	 *      dependent.
	 *  @param key The key to match
	 *  @return The SIFElement that has a matching element name and key, or null
	 *      if no matches found
	 */
        public override SifElement GetChild(String name, String key)
	{
		IList<SifElement> children = ChildList();
		lock( children )
		{
			foreach( SifElement o in children ) {
				if( ((ElementDefImpl)o.fElementDef).InternalName.Equals(name) && ( key == null || ( o.Key.Equals(key) ) ) )
			    	return o;
			}
		}

		return null;
	}

        /**
         *  Gets a child object identified by its ElementDef and composite key<p>
         *  @param id A ElementDef defined by the SIFDTD class to uniquely identify this field
         *  @param compKey The key values in sequential order
         */
        public override SifElement GetChild(IElementDef id, String[] compKey)
        {
            StringBuilder b = new StringBuilder(compKey[0]);
            for (int i = 1; i < compKey.Length; i++)
            {
                b.Append(".");
                b.Append(compKey[i]);
            }

            return GetChild(id, b.ToString());
        }

        //protected override IList<SifElement> createList()
        //{
        //    return new SifActionChildrenList<TKey>();
        //}

        //private class SifActionChildrenList<TKey> : IList<SifElement>
        //{
        //    private Dictionary<TKey, SifKeyedElement> fChildren;


        //    ///<summary>
        //    ///Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
        //    ///</summary>
        //    ///
        //    ///<returns>
        //    ///The index of item if found in the list; otherwise, -1.
        //    ///</returns>
        //    ///
        //    ///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        //    public int IndexOf( SifElement item )
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
        //    ///</summary>
        //    ///
        //    ///<param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        //    ///<param name="index">The zero-based index at which item should be inserted.</param>
        //    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        //    ///<exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        //    public void Insert( int index, SifElement item )
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
        //    ///</summary>
        //    ///
        //    ///<param name="index">The zero-based index of the item to remove.</param>
        //    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        //    ///<exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        //    public void RemoveAt( int index )
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Gets or sets the element at the specified index.
        //    ///</summary>
        //    ///
        //    ///<returns>
        //    ///The element at the specified index.
        //    ///</returns>
        //    ///
        //    ///<param name="index">The zero-based index of the element to get or set.</param>
        //    ///<exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        //    ///<exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        //    public SifElement this[ int index ]
        //    {
        //        get { throw new NotImplementedException(); }
        //        set { throw new NotImplementedException(); }
        //    }

        //    ///<summary>
        //    ///Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        //    ///</summary>
        //    ///
        //    ///<param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        //    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        //    public void Add( SifElement item )
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        //    ///</summary>
        //    ///
        //    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        //    public void Clear()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        //    ///</summary>
        //    ///
        //    ///<returns>
        //    ///true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        //    ///</returns>
        //    ///
        //    ///<param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        //    public bool Contains( SifElement item )
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        //    ///</summary>
        //    ///
        //    ///<param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        //    ///<param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        //    ///<exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        //    ///<exception cref="T:System.ArgumentNullException">array is null.</exception>
        //    ///<exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        //    public void CopyTo( SifElement [] array, int arrayIndex )
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        //    ///</summary>
        //    ///
        //    ///<returns>
        //    ///true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        //    ///</returns>
        //    ///
        //    ///<param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        //    ///<exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        //    public bool Remove( SifElement item )
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        //    ///</summary>
        //    ///
        //    ///<returns>
        //    ///The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        //    ///</returns>
        //    ///
        //    public int Count
        //    {
        //        get { throw new NotImplementedException(); }
        //    }

        //    ///<summary>
        //    ///Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        //    ///</summary>
        //    ///
        //    ///<returns>
        //    ///true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.
        //    ///</returns>
        //    ///
        //    public bool IsReadOnly
        //    {
        //        get { throw new NotImplementedException(); }
        //    }

        //    ///<summary>
        //    ///Returns an enumerator that iterates through the collection.
        //    ///</summary>
        //    ///
        //    ///<returns>
        //    ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        //    ///</returns>
        //    ///<filterpriority>1</filterpriority>
        //    IEnumerator<SifElement> IEnumerable<SifElement>.GetEnumerator()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    ///<summary>
        //    ///Returns an enumerator that iterates through a collection.
        //    ///</summary>
        //    ///
        //    ///<returns>
        //    ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        //    ///</returns>
        //    ///<filterpriority>2</filterpriority>
        //    public IEnumerator GetEnumerator()
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
