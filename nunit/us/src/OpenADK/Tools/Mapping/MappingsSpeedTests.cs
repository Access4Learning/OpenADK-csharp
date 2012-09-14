using System;
using System.Collections;
using OpenADK.Library;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.Cfg;
using OpenADK.Library.Tools.Mapping;
using NUnit.Framework;

namespace Library.Nunit.US.Library.Tools.Mapping
{
    [TestFixture]
    public class MappingsSpeedTests
    {
        private AgentConfig fCfg;


        /**
	 * Test simply maps a StudentPersonal object to a IDictionary 50000 times and
	 * records the amount of time it took.
	 * 
	 * Test Took 16.609 seconds against Adk 2.0 before JXPath support - 15.859
	 * seconds against Adk 2.0 after JXPath support - 17.218 seconds against Adk
	 * 2.0 after support for 1.5r1 mappings - 21.704 seconds against Adk 2.0
	 * after switching to the multi-version agent.cfg - 22.015 seconds after
	 * adding better version-dependent tag name matching in XPath
	 * 
	 * @throws AdkException
	 */

        [Test]
        public void testInBoundMapping50000()
        {
            fCfg = new AgentConfig();
            fCfg
                .Read(
                "..\\..\\Library\\Tools\\Mapping\\MultiVersion.agent.cfg",
                false );

            int mappingIterations = 0;
            //
            // UNCOMMENT THIS LINE TO RUN THE SPEED TEST
            //
            // mappingIterations = 50000;

            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPersonalTest();
            StringMapAdaptor sma = new StringMapAdaptor( map );

            // Fill out the student personal using outbound mappings first
            StudentPersonal template = new StudentPersonal();
            mappings.MapOutbound( sma, template );
            Console.WriteLine( template.ToXml() );

            DateTime start = DateTime.Now;
            for ( int x = 0; x < mappingIterations; x++ )
            {
                map.Clear();
                mappings.MapInbound( template, sma );
                if ( x%500 == 0 )
                {
                    Console.WriteLine( "Iteration " + x + " of "
                                       + mappingIterations );
                }
            }

            DateTime end = DateTime.Now;
            Console.WriteLine( "Mapping "
                               + mappingIterations
                               + " Students inbound took " + end.Subtract( start ) );
        }

        /**
	 *  - Took 30.516 Seconds using Adk 2.0 - Took 31.297 Seconds after using
	 * better version-independent tag names - Took 28.688 Seconds after adding
	 * the caching of the target ElementDef in XPathRule - Took 28.734 Seconds
	 * after changing SifXPathContext to test for repeatability of elements
	 */

        [Test]
        public void testOutBoundMapping50000()
        {
            fCfg = new AgentConfig();
            fCfg
                .Read(
                "..\\..\\Library\\Tools\\Mapping\\MultiVersion.agent.cfg",
                false );

            int mappingIterations = 0;
            //
            // UNCOMMENT THIS LINE TO RUN THE SPEED TEST
            //
            //mappingIterations = 50000;

            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPersonalTest();
            StringMapAdaptor sma = new StringMapAdaptor( map );

            StudentPersonal template = new StudentPersonal();
            mappings.MapOutbound( sma, template );
            Console.WriteLine( template.ToXml() );

            DateTime start = DateTime.Now;
            for ( int x = 0; x < mappingIterations; x++ )
            {
                template = new StudentPersonal();
                mappings.MapOutbound( sma, template );
                if ( x%500 == 0 )
                {
                    Console.WriteLine( "Iteration " + x + " of "
                                       + mappingIterations );
                }
            }

            DateTime end = DateTime.Now;
            Console.WriteLine( "Mapping "
                               + mappingIterations
                               + " Students inbound took " + end.Subtract( start ) );
        }

        /**
	 *  - Took 30.516 Seconds using Adk 2.0 - Took 26.938 Seconds after
	 * refactoring Mappings - Took 29.172 Seconds after adding better
	 * version-dependent tag name matching - Took 23.313 Seconds after adding
	 * the caching of the target ElementDef in XPathRule
	 */

        [Test]
        public void testOutBoundMappingContext50000()
        {
            fCfg = new AgentConfig();
            fCfg
                .Read(
                "..\\..\\Library\\Tools\\Mapping\\MultiVersion.agent.cfg",
                false );

            int mappingIterations = 0;
            //
            // UNCOMMENT THIS LINE TO RUN THE SPEED TEST
            //
            //mappingIterations = 50000;

            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );
            IDictionary map = buildIDictionaryForStudentPersonalTest();
            StringMapAdaptor sma = new StringMapAdaptor( map );
            StudentPersonal template = new StudentPersonal();

            MappingsContext mc = mappings.SelectOutbound( template.ElementDef,
                                                          SifVersion.SIF20, null, null );
            mc.Map( template, sma );
            Console.WriteLine( template.ToXml() );

            DateTime start = DateTime.Now;
            for ( int x = 0; x < mappingIterations; x++ )
            {
                template = new StudentPersonal();
                mc.Map( template, sma );
                if ( x%500 == 0 )
                {
                    Console.WriteLine( "Iteration " + x + " of "
                                       + mappingIterations );
                }
            }
            Console.WriteLine( template.ToXml() );
            DateTime end = DateTime.Now;
            Console.WriteLine( "Mapping "
                               + mappingIterations
                               + " Students inbound took " + end.Subtract( start ) );
        }

        /**
	 * Test simply maps a StudentPersonal object to a IDictionary 50000 times and
	 * records the amount of time it took.
	 *  - 16.828 seconds against Adk 2.0 after MappingsContext created - 17.217
	 * seconds against Adk 2.0 after switching to the multi-version agent.cfg -
	 * 19.109 seconds after adding better version-dependent name matching
	 * 
	 * @throws AdkException
	 */

        [Test]
        public void testInBoundMappingContext50000()
        {
            fCfg = new AgentConfig();
            fCfg
                .Read(
                "..\\..\\Library\\Tools\\Mapping\\MultiVersion.agent.cfg",
                false );

            int mappingIterations = 0;
            //
            // UNCOMMENT THIS LINE TO RUN THE SPEED TEST
            //
            // mappingIterations = 50000;

            Mappings mappings = fCfg.Mappings.GetMappings( "Default" );

            // Fill out the student personal using outbound mappings first
            StudentPersonal template = new StudentPersonal();

            IDictionary map = buildIDictionaryForStudentPersonalTest();
            StringMapAdaptor sma = new StringMapAdaptor( map );
            mappings.MapOutbound( sma, template );
            Console.WriteLine( template.ToXml() );

            MappingsContext mc = mappings.SelectInbound( template.ElementDef,
                                                         SifVersion.SIF20, null, null );

            DateTime start = DateTime.Now;
            for ( int x = 0; x < mappingIterations; x++ )
            {
                map.Clear();
                mc.Map( template, sma );
                if ( x%500 == 0 )
                {
                    Console.WriteLine( "Iteration " + x + " of "
                                       + mappingIterations );
                }
            }

            DateTime end = DateTime.Now;
            Console.WriteLine( "Mapping "
                               + mappingIterations
                               + " Students inbound took " + end.Subtract( start ) );
        }

        private IDictionary buildIDictionaryForStudentPersonalTest()
        {
            IDictionary data = new Hashtable();
            data.Add( "STUDENT_NUM", "998" );
            data.Add( "LAST_NAME", "Johnson" );
            data.Add( "MIDDLE_NAME", "George" );
            data.Add( "FIRST_NAME", "Betty" );
            data.Add( "BIRTHDATE", "19900101" );
            data.Add( "ETHNICITY", "4" );
            data.Add( "HOME_PHONE", "202-358-6687" );
            data.Add( "CELL_PHONE", "202-502-4856" );
            data.Add( "ALT_PHONE", "201-668-1245" );
            data.Add( "ALT_PHONE_TYPE", "TE" );

            return data;
        }
    }
}