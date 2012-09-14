//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;

namespace OpenADK.Library.Impl
{
    /// <summary>  Sorts SifElements by sequence number.
    /// 
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  Adk 1.0
    /// </version>
    internal class ElementSorter<T> : IComparer<T> where T : Element
    {
        /// <summary>  The SIF version this ElementSorter will use when obtaining sequence
        /// numbers for SifElements
        /// </summary>
        protected internal SifVersion fVersion;

        /// <summary>  Constructor</summary>
        /// <param name="version">The version of SIF to sort against. The sequence numbers
        /// of some elements change from one version of SIF to the next as a
        /// result of new elements or changes in ordering by SIF Working Groups.
        /// </param>
        protected ElementSorter( SifVersion version )
        {
            if( version == null )
            {
                version = Adk.SifVersion;
            }
            fVersion = version;
        }

        /// <summary>  Gets an ElementSorter for a given version of SIF</summary>
        /// <param name="version">The version of SIF to sort against
        /// </param>
        public static ElementSorter<T> GetInstance( SifVersion version )
        {
            if( version == null )
            {
                version = Adk.SifVersion;
            }

            if( version.CompareTo( SifVersion.SIF20 ) < 0 )
            {
                return new Sif1xElementSorter<T>( version );
            }
            return new ElementSorter<T>( version );
           
        }

        /// <summary>  Determines whether Element <i>o1</i>comes before or after Element <i>o2</i>
        /// given the IElementDef sequence number of the two objects.
        /// </summary>
        public virtual int Compare( T o1,
                                    T o2 )
        {
            int cmp1 = o1.ElementDef.GetSequence( fVersion );
            int cmp2 = o2.ElementDef.GetSequence( fVersion );

            return compareSequences( cmp1, cmp2 );

           
        }

        protected int compareSequences( int cmp1, int cmp2 )
        {
            if( cmp1 < cmp2 )
            {
                return -1;
            }
            if( cmp1 > cmp2 )
            {
                return 1;
            }
            return 0;
        }
    }

   
}
