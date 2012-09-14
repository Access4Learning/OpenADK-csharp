using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Metadataupdater
{
    class MetadataSorter : IComparer<XmlElement>
    {
        #region IComparer<XmlElement> Members

        public int Compare(XmlElement x, XmlElement y)
        {
            string namex = x.GetAttribute("name");
            string namey = y.GetAttribute("name");
            return namex.CompareTo(namey);
        }

        #endregion
    }
}
