using System;
using System.Collections;

namespace Edustructures.Metadata.DataElements
{
    /// <summary>
    /// Summary description for FieldDefComparer.
    /// </summary>
    public class FieldDefComparer : IComparer
    {
        #region IComparer Members

        public int Compare( object x,
                            object y )
        {
            return (((FieldDef) x).CompareTo( y ));
        }

        #endregion
    }
}