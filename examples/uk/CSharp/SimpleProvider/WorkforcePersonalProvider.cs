//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using OpenADK.Library;
using OpenADK.Library.uk.Workforce;

public class WorkforcePersonalProvider : AbstractPersonProvider
{
    protected override IElementDef getElementDef()
    {
        return WorkforceDTD.WORKFORCEPERSONAL;
    }


    protected override SifDataObject createPersonObject(string id)
    {
        WorkforcePersonal wp = new WorkforcePersonal();
        wp.LocalId = id;
        return wp;
    }
}
