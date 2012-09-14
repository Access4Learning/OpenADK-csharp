//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;

namespace OpenADK.Library.Tools.XPath.Compiler
{
    public class AdkNodeNameTest : AdkNodeTest
    {
        private String fName;

        public AdkNodeNameTest( String nodeName )
        {
            fName = nodeName;
        }

        public String NodeName
        {
            get { return fName; }
        }

        public override String ToString()
        {
            return fName;
        }
    }
}
