//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using OpenADK.Library;
using OpenADK.Library.Global;
using OpenADK.Library.uk.Common;
using OpenADK.Library.uk.Learner;
/**
 *  The StudentProvider agent responds to SIF_Request messages for StudentPersonal.
 *  It demonstrates the basics of publishing with the ADK.
 *
 *  @version ADK 1.0
 */
/**
 * @author Andrew
 *
 */

public class LearnerPersonalProvider : AbstractPersonProvider, IPublisher
{
    protected override SifDataObject createPersonObject(string id)
    {
        LearnerPersonal lp = new LearnerPersonal();
        InCare inCare = new InCare(YesNoUnknown.YES, "323");
        lp.InCare = inCare;
        lp.UPN = id;

        lp.AlertMsgList =
            new AlertMsgList(new AlertMsg(AlertMsgType.DISCIPLINE, "Student Discipline note"));
        //lp.AlertMsgList.GetEnumerator
        return lp;
    }


    protected override IElementDef getElementDef()
    {
        return LearnerDTD.LEARNERPERSONAL;
    }
}
