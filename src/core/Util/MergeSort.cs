//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;

namespace OpenADK.Util
{
    /// <summary>
    /// A simple Merge sort algorithm
    /// </summary>
    sealed class MergeSort
    {
        
        public static void Sort<T>(IList<T> list, IComparer<T> comparer )
        {
            Sort(list, 0, list.Count - 1, comparer );
        }

        private static void Sort<T>(IList<T> list, int from, int to, IComparer<T> comparer)
        {
            if (from < to)
            {
                int mid;
                mid = (from + to) / 2;

                Sort(list, from, mid, comparer);
                Sort(list, mid + 1, to, comparer );

                int end_low;
                end_low = mid;
                int start_high;
                start_high = mid + 1;

                while (from <= end_low & start_high <= to)
                {
                    if (comparer.Compare(list[from], list[start_high]) < 1)
                    {
                        from++;
                    }
                    else
                    {
                        T tmp;
                        tmp = list[start_high];
                        int i;
                        for (i = start_high - 1; i >= from; i--)
                        {
                            list[i + 1] = list[i];
                        }
                        list[from] = tmp;
                        from++;
                        end_low++;
                        start_high++;
                    }
                }
            }
        }




    }
}
