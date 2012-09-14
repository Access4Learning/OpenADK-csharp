using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using OpenADK.Library;
using OpenADK.Library.Tools.Cfg;
using OpenADK.Library.Tools.Mapping;
using NUnit.Framework;

namespace Library.Nunit.US.Library.Tools.Mapping
{
    [TestFixture]
    public class MappingsObjectTests
    {
        private AgentConfig fCfg;

        [SetUp]
        public virtual void setUp()
        {
            Adk.Initialize();
            fCfg = new AgentConfig();
            fCfg.Read( "..\\..\\Library\\Tools\\Mapping\\SIF1.5.agent.cfg",
                       false );
        }

        /**
	 * This test "builds up" a set of mappings programmatically, writes it to
	 * disk and the reads it again. The assertions assert that all of the
	 * attributes of each associated part of the mappings hierarchy is
	 * preserved.
	 */

        [Test]
        public void testReadAndWriteMappings()
        {
            String FILE_NAME = "tmp.cfg";

            // TODO: Right now we don't have a way to build up a Mappings hierarchy
            // without reading it from a file. This should be fixed and this
            // test should be updated as a result.
            AgentConfig cfg = createMappings();

            debug( cfg.Document );

            // save the mappings to a file
            FileInfo f = new FileInfo( FILE_NAME );
            if ( f.Exists )
            {
                f.Delete();
            }

            using ( StreamWriter fs = new StreamWriter( FILE_NAME ) )
            {
                try
                {
                    cfg.Save( fs );
                }
                finally
                {
                    fs.Close();
                }
            }

            // Read the new mappings
            cfg = new AgentConfig();
            cfg.Read( FILE_NAME, false );

            Mappings reparsed = cfg.Mappings;
            assertMappings( reparsed );
        }

        /**
	 * Returns a new mappings root, with the passed-in Mappings copied into it
	 * as a child, obtained by using the Mappings.copy() method
	 * 
	 * @param mappingSet
	 *            The set of mappings to be copied into the new root
	 * @return
	 */

        private Mappings getCopy( Mappings mappingSet )
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml( "<mappings/>" );
            Mappings newRoot = new Mappings();
            newRoot.XmlElement = dom.DocumentElement;
            mappingSet.Copy( newRoot );

            debug( dom );

            return newRoot;
        }

        /**
	 * Writes the DOM to System.out
	 * 
	 * @param dom
	 */

        private void debug( XmlDocument dom )
        {
            Console.WriteLine( dom.DocumentElement.OuterXml );
        }

        /**
	 * Builds up a mappings hierarchy, and then calls the "copy" method to copy
	 * the hierarchy. The copy is then asserted to ensure it has the proper
	 * behavior.
	 * 
	 * @throws AdkException
	 */

        [Test]
        public void testCopyMappings()
        {
            AgentConfig cfg = createMappings();
            Mappings m = cfg.Mappings;
            Mappings newRoot = getCopy( m.GetMappings( "Test" ) );
            assertMappings( newRoot );
        }

        [Test]
        public void testCreateMappings()
        {
            // Create the root instance
            Mappings root = new Mappings();
            // Create the default set of universal mappings
            Mappings defaults = new Mappings( root, "Default" );
            root.AddChild( defaults );

            // Create an ObjectMapping for StudentPersonal
            ObjectMapping studentMappings = new ObjectMapping( "StudentPersonal" );
            defaults.AddRules( studentMappings );
            // Add field rules
            studentMappings.AddRule( new FieldMapping( "FIRSTNAME",
                                                       "Name[@Type='04']/FirstName" ) );
            studentMappings.AddRule( new FieldMapping( "LASTNAME",
                                                       "Name[@Type='04']/LastName" ) );

            // Create a set of mappings for the state of Wisconsin
            Mappings wisconsin = new Mappings( defaults, "Wisconsin" );
            defaults.AddChild( wisconsin );
            // Create a set of mappings for the Neillsville School District
            Mappings neillsville = new Mappings( wisconsin, "Neillsville" );

            wisconsin.AddChild( neillsville );

            XmlDocument doc = new XmlDocument( );
            doc.LoadXml( "<agent/>");

            XmlElement n = root.ToDom( doc );
            debug( doc );
        }

        /**
	 * Builds up a mappings hierarchy, and then calls the toDOM() method to copy
	 * the hierarchy. The copy is then read back into a new Mappings object and
	 * asserted to ensure it has the proper behavior.
	 * 
	 * @throws AdkException
	 */

        [Test]
        public void testCopyMappingsThroughDOM()
        {
            AgentConfig cfg = createMappings();
            Mappings m = cfg.Mappings;
            Mappings newRoot = getCopyFromDOM( m.GetMappings( "Test" ) );
            assertMappings( newRoot );
        }

        [Test]
        public void testCopyMappingsThroughCopyThenDOM()
        {
            AgentConfig cfg = createMappings();
            Mappings m = cfg.Mappings;
            Mappings newRoot = getCopy( m.GetMappings( "Test" ) );
            newRoot= getCopyFromDOM( newRoot.GetMappings( "Test") );
            assertMappings(newRoot);
        }



        /**
	 * Returns a new mappings root, with the passed-in Mappings copied into it
	 * as a child, obtained by using the Mappings.copy() method
	 * 
	 * @param root
	 *            The set of mappings to be copied into the new root
	 * @return
	 */

        private Mappings getCopyFromDOM( Mappings mappingSet )
        {
            XmlDocument dom = new XmlDocument();
            dom.LoadXml( "<agent/>" );
            XmlNode mappingsNode = dom.ImportNode( mappingSet.XmlElement, true );
            dom.DocumentElement.AppendChild( mappingsNode );

            Mappings newRoot = new Mappings();
            newRoot.Populate( dom, (XmlElement) mappingsNode );
            return newRoot;
        }

        /**
	 * Creates a set of mappings that operations can be applied to, such as
	 * saving to a DOM or Agent.cfg. The results can be asserted by calling
	 * {@see #assertMappings(Mappings)}.
	 * 
	 * NOTE: This method returns an AgentConfig instance instead of a mappings
	 * instance because there is no way set the Mappings instance on
	 * AgentConfig. This might change in the future
	 * 
	 * @return
	 */

        private AgentConfig createMappings()
        {
            Mappings root = fCfg.Mappings;
            // Remove the mappings being used
            root.RemoveChild( root.GetMappings( "Default" ) );
            root.RemoveChild( root.GetMappings( "TestID" ) );

            Mappings newMappings = root.CreateChild( "Test" );

            // Add an object mapping
            ObjectMapping objMap = new ObjectMapping( "StudentPersonal" );
            // Currently, the Adk code requires that an Object Mapping be added
            // to it's parent before fields are added.
            // We should re-examine this and perhaps fix it, if possible
            newMappings.AddRules( objMap );

            objMap.AddRule( new FieldMapping( "FIELD1", "Name/FirstName" ) );

            // Field 2
            FieldMapping field2 = new FieldMapping( "FIELD2", "Name/LastName" );
            field2.ValueSetID = "VS1";
            field2.Alias = "ALIAS1";
            field2.DefaultValue = "DEFAULT1";
            MappingsFilter mf = new MappingsFilter();
            mf.Direction = MappingDirection.Inbound;
            mf.SifVersion = SifVersion.SIF11.ToString();
            field2.Filter = mf;
            objMap.AddRule( field2 );

            // Field 3 test setting the XML values after it's been added to the
            // parent object (the code paths are different)
            FieldMapping field3 = new FieldMapping( "FIELD3", "Name/MiddleName" );
            objMap.AddRule( field3 );
            field3.ValueSetID = "VS2";
            field3.Alias = "ALIAS2";
            field3.DefaultValue = "DEFAULT2";
            MappingsFilter mf2 = new MappingsFilter();
            mf2.Direction = MappingDirection.Outbound;
            mf2.SifVersion = SifVersion.SIF15r1.ToString();
            field3.Filter = mf2;
            field3.NullBehavior = MappingBehavior.IfNullDefault;

            OtherIdMapping oim = new OtherIdMapping( "ZZ", "BUSROUTE" );
            FieldMapping field4 = new FieldMapping( "FIELD4", oim );
            objMap.AddRule( field4 );
            field4.DefaultValue = "Default";
            field4.ValueSetID = "vs";
            field4.Alias = "alias";
            field4.DefaultValue = null;
            field4.ValueSetID = null;
            field4.Alias = null;
            field4.NullBehavior = MappingBehavior.IfNullSuppress;

            // Field4 tests the new datatype attribute
            FieldMapping field5 = new FieldMapping( "FIELD5",
                                                    "Demographics/BirthDate" );
            objMap.AddRule( field5 );
            field5.DataType = SifDataType.Date;

            // Add a valueset translation
            ValueSet vs = new ValueSet( "VS1" );
            newMappings.AddValueSet( vs );
            // Add a few definitions
            for ( int a = 0; a < 10; a++ )
            {
                vs.Define( "Value" + a, "SifValue" + a, "Title" + a );
            }

            vs.Define( "AppDefault", "0000", "Default App Value" );
            vs.SetAppDefault( "AppDefault", true );

            vs.Define( "0000", "SifDefault", "Default Sif Value" );
            vs.SetSifDefault( "SifDefault", false );

            // Add a valueset translation
            vs = new ValueSet( "VS2" );
            newMappings.AddValueSet( vs );
            // Add a few definitions
            for ( int a = 0; a < 3; a++ )
            {
                vs.Define( "q" + a, "w" + a, "t" + a );
            }

            vs.Define( "AppDefault", "0000", "Default Value" );
            vs.SetAppDefault( "AppDefault", true );
            vs.SetSifDefault( "0000", true );

            return fCfg;
        }

        /**
	 * Asserts that the provided set of Mappings matches the one that was
	 * created in createMappings();
	 */

        private void assertMappings( Mappings m )
        {
            Mappings test = m.GetMappings( "Test" );
            Assertion.AssertNotNull( "Test mappings is not present", test );
            Assertion.AssertEquals( "Should have a single Object Mapping", 1, test.GetObjectMappings().Length );

            // TODO: Test the version and sourceId filters more carefully
            /*
		 * Assertion.AssertEquals( "SifVersion attr should be empty", 0,
		 * test.SifVersionFilter().Length ); Assertion.AssertEquals( "SourceId attr
		 * should be empty", 0, test.SourceIdFilter().Length ); Assertion.AssertEquals(
		 * "Zone attr should be empty", 0, test.ZoneIdFilter().Length );
		 */

            // assert the object mapping
            ObjectMapping om = test.GetObjectMapping( "StudentPersonal", false );
            Assertion.AssertNotNull( "StudentPersonal mappings", om );
            Assertion.AssertEquals( "There should be five rules", 5, om.RuleCount );
            IList<FieldMapping> rules = om.GetRulesList( false );

            // Field 1
            Assertion.AssertEquals( "FIELD1 name", "FIELD1", rules[0].FieldName );
            Assertion.AssertEquals( "FIELD1 rule", "Name/FirstName", rules[0].GetRule().ToString() );
            Assertion.AssertEquals( "FIELD1 ifNull", MappingBehavior.IfNullUnspecified, rules[0].NullBehavior );

            // Field 2
            Assertion.AssertEquals( "FIELD2 name", "FIELD2", rules[1].FieldName );
            Assertion.AssertEquals( "FIELD2 rule", "Name/LastName", rules[1].GetRule().ToString() );
            Assertion.AssertEquals( "FIELD2 valueset", "VS1", rules[1].ValueSetID );
            Assertion.AssertEquals( "FIELD2 alias", "ALIAS1", rules[1].Alias );
            Assertion.AssertEquals( "FIELD2 default", "DEFAULT1", rules[1].DefaultValue );
            MappingsFilter filter = rules[1].Filter;
            Assertion.AssertNotNull( "FIELD2 filter is null", filter );
            Assertion.AssertEquals( "filter direction", MappingDirection.Inbound, filter
                                                                                      .Direction );
            Assertion.AssertEquals( "filter sif version", "=" + SifVersion.SIF11.ToString(),
                                    filter.SifVersion );

            // Field 3
            Assertion.AssertEquals( "FIELD3 name", "FIELD3", rules[2].FieldName );
            Assertion.AssertEquals( "FIELD3 rule", "Name/MiddleName", rules[2].GetRule().ToString() );
            Assertion.AssertEquals( "FIELD3 valueset", "VS2", rules[2].ValueSetID );
            Assertion.AssertEquals( "FIELD3 alias", "ALIAS2", rules[2].Alias );
            Assertion.AssertEquals( "FIELD3 default", "DEFAULT2", rules[2].DefaultValue );
            Assertion.AssertEquals( "FIELD3 ifNull", MappingBehavior.IfNullDefault, rules[2].NullBehavior );
            MappingsFilter filter2 = rules[2].Filter;
            Assertion.AssertNotNull( "FIELD3 filter is null", filter2 );
            Assertion.AssertEquals( "filter2 direction", MappingDirection.Outbound, filter2.Direction );
            Assertion.AssertEquals( "filter2 sif version",
                                    "=" + SifVersion.SIF15r1.ToString(), filter2.SifVersion );

            // Field 4
            Assertion.AssertEquals( "FIELD4 name", "FIELD4", rules[3].FieldName );
            Assertion.AssertNull( "FIELD4 valueset", rules[3].ValueSetID );
            Assertion.AssertNull( "FIELD4 alias", rules[3].Alias );
            Assertion.AssertNull( "FIELD4 default", rules[3].DefaultValue );
            Assertion.AssertEquals( "FIELD4 ifNull", MappingBehavior.IfNullSuppress, rules[3].NullBehavior );
            Rule r = rules[3].GetRule();
            Assertion.Assert( "Rule should be OtherIdRule", r is OtherIdRule );

            Assertion.AssertEquals( "FIELD5 name", "FIELD5", rules[4].FieldName );
            Assertion.AssertEquals( "FIELD5 datatype", SifDataType.Date, rules[4]
                                                                             .DataType );

            // TODO: The OtherIdRule doesn't have an API to get at the
            // OtherIdMapping. For now, just
            // convert it to a string and assert the results
            String ruleStr = r.ToString();
            Assertion.Assert( "prefix should be BUSROUTE", ruleStr
                                                               .IndexOf( "prefix='BUSROUTE'" ) > 1 );
            Assertion.Assert( "type should be ZZ", ruleStr.IndexOf( "type='ZZ'" ) > 1 );

            ValueSet vs = test.GetValueSet( "VS1", false );
            Assertion.AssertNotNull( "ValueSet VS1 should not be null", vs );
            Assertion.AssertEquals( "VS1 should have 12 entries", 12, vs.Entries.Length );
            for ( int a = 0; a < 10; a++ )
            {
                Assertion.AssertEquals( "Mapping by appvalue", "SifValue" + a, vs.Translate( "Value" + a ) );
                Assertion.AssertEquals( "Mapping by sifvalue", "Value" + a, vs.TranslateReverse( "SifValue" + a ) );
            }
            // Test the default value entries
            Assertion.AssertEquals( "Expecting app default value", "AppDefault", vs.TranslateReverse( "abcdefg" ) );
            Assertion.AssertEquals( "Expecting app default value", "AppDefault", vs.TranslateReverse( null ) );
            Assertion.AssertEquals( "Expecting sif default value", "SifDefault", vs.Translate( "abcdefg" ) );
            Assertion.AssertNull( "Expecting NULL value", vs.Translate( null ) );

            vs = test.GetValueSet( "VS2", false );
            Assertion.AssertNotNull( "ValueSet VS2 should not be null", vs );
            Assertion.AssertEquals( "VS2 should have 4 entries", 4, vs.Entries.Length );
            for ( int a = 0; a < 3; a++ )
            {
                Assertion.AssertEquals( "Mapping by appvalue", "w" + a, vs.Translate( "q" + a ) );
                Assertion.AssertEquals( "Mapping by sifvalue", "q" + a, vs
                                                                            .TranslateReverse( "w" + a ) );
            }
            // Test the default value entries
            Assertion.AssertEquals( "Expecting app default value", "AppDefault", vs
                                                                                     .TranslateReverse( "abcdefg" ) );
            Assertion.AssertEquals( "Expecting app default value", "AppDefault", vs
                                                                                     .TranslateReverse( null ) );
            Assertion.AssertEquals( "Expecting sif default value", "0000", vs
                                                                               .Translate( "abcdefg" ) );
            Assertion.AssertEquals( "Expecting sif default value", "0000", vs.Translate( null ) );
        }
    }
}