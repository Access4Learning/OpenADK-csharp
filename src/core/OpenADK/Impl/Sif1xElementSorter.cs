//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenADK.Library.Impl
{
    class Sif1xElementSorter<T> : ElementSorter<T> where T: Element
    {
        public Sif1xElementSorter( SifVersion version )
            : base( version )
        {
           
        }


        /**
         *  Determines whether Element <i>o1</i>comes before or after Element <i>o2</i>
         *  given the ElementDef sequence number of the two objects.
         */
        public override int Compare( T o1, T o2 )
        {
            Element parent1 = o1.Parent;
            Element parent2 = o2.Parent;
            if (parent1 == parent2 || parent1 == null || parent2 == null)
            {
                return base.Compare(o1, o2);
            }

            // One of these elements has a parent that was collapsed and it is now
            // being compared with it's uncles and aunts, rather than its siblings
            // The logic is simple: Determine which element is the niece or nephew. That
            // element will use it's parent sequence to compare with the relative.
            if (parent1.Parent == parent2)
            {
                int cmp1 = parent1.ElementDef.GetSequence(fVersion);
                int cmp2 = o2.ElementDef.GetSequence(fVersion);
                return compareSequences(cmp1, cmp2);
            }
            else if (parent2.Parent == parent1)
            {
                int cmp1 = o1.ElementDef.GetSequence(fVersion);
                int cmp2 = parent2.ElementDef.GetSequence(fVersion);
                return compareSequences(cmp1, cmp2);
            }
            else if (parent1.Parent == parent2.Parent)
            {
                int cmp1 = parent1.ElementDef.GetSequence(fVersion);
                int cmp2 = parent2.ElementDef.GetSequence(fVersion);
                return compareSequences(cmp1, cmp2);
            }
            // Indeterminate. Do the safe thing and exit gracefully
            return base.Compare(o1, o2);


        }

    }
}
