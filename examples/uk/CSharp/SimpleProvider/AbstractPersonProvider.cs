//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Threading;
using OpenADK.Library;
using OpenADK.Library.uk.Common;

public abstract class AbstractPersonProvider : IPublisher
{
    public AbstractPersonProvider()
    {
        try
        {
            initializeDB();
        }
        catch (AdkSchemaException adkse)
        {
            Console.WriteLine(adkse);
        }
    }

    /**
	 * The amount of time to wait in between sending change events 
	 */
    private int EVENT_INTERVAL = 30000;
    /**
	 * The data provided by this simple provider class 
	 */
    private List<SifDataObject> fData;
    /**
	 *  The zone to send SIF events to
	 */
    private IZone fZone;

    public void OnRequest(IDataObjectOutputStream dataStream,
                          Query query,
                          IZone zone,
                          IMessageInfo info)
    {
        // To be a successful publisher of data using the ADK, follow these steps
        // 1) Examine the query conditions. If they are too complex for your agent, 
        // 	throw the appropriate SIFException
        // This example agent uses the autoFilter() capability of DataObjectOutputStream. Using
        // this capability, any object can be written to the output stream and the stream will 
        // filter out any objects that don't meet the conditions of the Query. However, a more
        // robust agent with large amounts of data would want to pre-filter the data when it does its
        // initial database query.
        dataStream.Filter = query;

        Console.WriteLine("Responding to SIF_Request for StudentPersonal");

        // 2) Write any data to the output stream
        foreach (SifDataObject sp in fData)
        {
            dataStream.Write(sp);
        }
    }

    /**
	 * @param zone
	 * @throws ADKException
	 */

    public void provision(IZone zone)
    {
        zone.SetPublisher(this, getElementDef(), new PublishingOptions( ));
    }

    protected abstract IElementDef getElementDef();

    /**
	 * This class periodically publishes change events to the zone. This 
	 * method starts up the thread that publishes the change events.
	 * 
	 * A normal SIF Agent would, instead look for changes in the application's database
	 * and publish those as changes.
	 */

    public void startEventProcessing(IZone zone)
    {
        if (fZone != null)
        {
            throw new AdkException("Event Processing thread is already running", zone);
        }
        fZone = zone;


        //Thread thr = new Thread( this, "SIF-EventTHR" );
        //thr.setDaemon( true );
        //thr.start();
    }

    public void run()
    {
        /**
		 * This class periodically publishes change events to the zone. This 
		 * method starts up the thread that publishes the change events.
		 * 
		 * A normal SIF Agent would, instead look for changes in the application's database
		 * and publish those as changes.
		 */

        Console.WriteLine
            ("Event publishing enabled with an interval of " + EVENT_INTERVAL/1000 +
             " seconds.");

        Random random = new Random();

        bool isProcessing = true;
        // Go into a loop and send events
        while (isProcessing)
        {
            try
            {
                Thread.Sleep(EVENT_INTERVAL);

                SifDataObject changedObject = fData[random.Next(fData.Count)];
                SifDataObject eventObject = Adk.Dtd.CreateSIFDataObject(getElementDef());
                eventObject.SetElementOrAttribute
                    ("@PersonRefId",
                     changedObject.GetElementOrAttribute("@PersonRefId").
                         TextValue);

                // Create a change event with a random Learner ID;
                string newNum = "A" + random.Next(999999);
                eventObject.SetElementOrAttribute("LocalId", newNum);

                fZone.ReportEvent(eventObject, EventAction.Change);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during event processing: " + ex);
                isProcessing = false;
            }
        }
    }

    protected abstract SifDataObject createPersonObject(string id);

    protected void initializeDB()
    {
        fData = new List<SifDataObject>();
        fData.Add
            (createPerson
                 ("C831540004395", "Ackerman", "Brian",
                  "133", "Devon Drive", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4541000", "F", "07", EthnicityCodes.WHITE_BRITISH, "19910102"));
        fData.Add
            (createPerson
                 ("M830540004340", "Acosta", "Stacey",
                  "17", "Wellesley Park", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4541001", "F", "08", EthnicityCodes.BLACK_AFRICAN, "19871012"));
        fData.Add
            (createPerson
                 ("X831540004405", "Addicks", "Amber",
                  "162", "Crossfield Road", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4541002", "F", "07", EthnicityCodes.WHITE_BRITISH, "19920402"));
        fData.Add
            (createPerson
                 ("E830540004179", "Aguilar", "Mike",
                  "189", "Writtle Mews", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4541003", "M", "07", EthnicityCodes.WHITE_BRITISH, "19910102"));
        fData.Add
            (createPerson
                 ("A831540004820", "Alaev", "Dianna",
                  "737", "Writtle Mews", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4541004", "F", "07", EthnicityCodes.WHITE_SCOTTISH, "19980704"));
        fData.Add
            (createPerson
                 ("G831540004469", "Balboa", "Amy",
                  "87", "Almond Avenue", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4545000", "F", "01", EthnicityCodes.WHITE_BRITISH, "19901205"));
        fData.Add
            (createPerson
                 ("H831540004078", "Baldwin", "Joshua",
                  "142", "Clarendon Avenue", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4545001", "F", "02", EthnicityCodes.WHITE_BRITISH, "19960105"));
        fData.Add
            (createPerson
                 ("J830540004064", "Ballard", "Aimee",
                  "134", "Dixon Close", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4545002", "F", "03", EthnicityCodes.WHITE_BRITISH, "19940506"));
        fData.Add
            (createPerson
                 ("V831540004012", "Banas", "Ryan",
                  "26", "Mountview Drive", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4545003", "M", "04", EthnicityCodes.WHITE_BRITISH, "19930808"));
        fData.Add
            (createPerson
                 ("B830540004119", "Barba", "Ashley",
                  "958", "Manchester Road", "Edgebaston", "Birmingham", "B15 3NE",
                  "0121 4545004", "F", "05", EthnicityCodes.WHITE_EASTERN_EUROPEAN,
                  "19890102"));
    }

    private SifDataObject createPerson(string id,
                                       string lastName,
                                       string firstName,
                                       string number,
                                       string street,
                                       string locality,
                                       string town,
                                       string post,
                                       string phone,
                                       string gender,
                                       string grade,
                                       EthnicityCodes ethnicity,
                                       string birthDateyyyyMMdd)

    {
        SifDataObject person = createPersonObject(id);
        person.SetElementOrAttribute("@RefId", Adk.MakeGuid());

        Name name = new Name(NameType.CURRENT_LEGAL, firstName, lastName);
        PersonalInformation personal = new PersonalInformation(name);

        person.AddChild(CommonDTD.PERSONALINFORMATION, personal);

        AddressableObjectName aon = new AddressableObjectName();
        aon.StartNumber = number;
        Address address = new Address(AddressType.CURRENT, aon);
        address.Street = street;
        address.Locality = locality;
        address.Town = town;
        address.PostCode = post;
        address.SetCountry(CountryCode.GBR);
        personal.Address = address;

        personal.PhoneNumber = new PhoneNumber(PhoneType.HOME, phone);

        Demographics dem = new Demographics();
        dem.SetEthnicityList(new Ethnicity(ethnicity));
        dem.SetGender(Gender.Wrap(gender));
        try
        {
            dem.BirthDate = SifDate.ParseSifDateString(birthDateyyyyMMdd, SifVersion.SIF15r1);
        }
        catch (Exception pex)
        {
            Console.WriteLine(pex);
        }

        personal.Demographics = dem;

        return person;
    }
}
