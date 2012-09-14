using System;

namespace Edustructures.Metadata
{
    internal class DataItem
    {
        private String fName;

        public String Name
        {
            get { return fName; }
            set { fName = value; }
        }

        private string fDescription;

        public string Description
        {
            get { return fDescription; }
            set { fDescription = value; }
        }
    }
}