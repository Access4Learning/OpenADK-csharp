using System;
using System.Collections;
using System.Collections.Generic;
using OpenADK.Library;
using OpenADK.Library.us.Common;
using OpenADK.Library.us.Student;
using OpenADK.Library.Tools.Mapping;
using log4net;
using NUnit.Framework;

namespace Library.Nunit.US.Library.Tools.Mapping
{
    [TestFixture]
    public class MappingsSelectTests
    {
        private static readonly ILog logger = LogManager.GetLogger( typeof ( MappingsSelectTests ) );

        [SetUp]
        public void setUp()
        {
            Adk.Initialize();
        }

        private Mappings buildMappings1()
        {
            // root
            Mappings root = new Mappings();

            // root.Default
            Mappings defaults = new Mappings( root, "Default", null, null, null );
            root.AddChild( defaults );

            // root.Default.StudentPersonal
            ObjectMapping studentMappings = new ObjectMapping( "StudentPersonal" );
            defaults.AddRules( studentMappings );
            studentMappings.AddRule( new FieldMapping( "REFID", "@RefId" ) );

            // root.Default.MA
            Mappings massachusetts = new Mappings( defaults, "MA", null, null, null );
            defaults.AddChild( massachusetts );

            // root.Default.MA.StudentPersonal
            studentMappings = new ObjectMapping( "StudentPersonal" );
            massachusetts.AddRules( studentMappings );
            studentMappings.AddRule( new FieldMapping( "LOCALID", "LocalId" ) );
            studentMappings.AddRule( new FieldMapping( "FIRSTNAME",
                                                       "Name[@Type='01']/FirstName" ) );
            studentMappings.AddRule( new FieldMapping( "LASTNAME",
                                                       "Name[@Type='01']/LastName" ) );

            // root.Default.MA.Boston
            Mappings boston = new Mappings( massachusetts, "Boston", null, "Boston",
                                            null );
            massachusetts.AddChild( boston );

            // root.Default.MA.Boston.StudentPersonal
            studentMappings = new ObjectMapping( "StudentPersonal" );
            boston.AddRules( studentMappings );
            studentMappings.AddRule( new FieldMapping( "BIRTHDAY",
                                                       "Demographics/BirthDate" ) );

            return root;
        }

        [Test]
        public void testMap2()
        {
            StudentPersonal studentPersonal = makeStudentPersonal( "34",
                                                                   NameType.BIRTH, "David", "Ortiz" );
            IDictionary student = new Dictionary<String, String>();
            StringMapAdaptor sma = new StringMapAdaptor( student );
            Mappings mappings = buildMappings1();

            logger.Debug( "========= Root Mappings =========" );
            dumpMappings( mappings );

            if ( mappings.GetMappings( "Default" ) != null )
            {
                mappings = mappings.GetMappings( "Default" );
                logger.Debug( "======= Default Mappings ========" );
                dumpMappings( mappings );
                if ( mappings.GetMappings( "MA" ) != null )
                {
                    mappings = mappings.GetMappings( "MA" );
                    logger.Debug( "========== MA Mappings ==========" );
                    dumpMappings( mappings );
                    if ( mappings.GetMappings( "Boston" ) != null )
                    {
                        mappings = mappings.GetMappings( "Boston" );
                        logger.Debug( "======= Boston Mappings =========" );
                        dumpMappings( mappings );
                    }
                }
            }

            // This line gets a "Mappings.select can only be called..." exception
            // mappings = mappings.select("anyzone", null, Adk.SifVersion());
            logger.Debug( "======= Selected (" + mappings.Id
                          + ") Mappings =========" );
            dumpMappings( mappings );

            mappings.MapInbound( studentPersonal, sma );
            logger.Debug( "==================================" );

            foreach ( DictionaryEntry de in student )
            {
                logger.Debug( String.Format( "{0} = {1}", de.Key, de.Value ) );
            }
        }

        public static StudentPersonal makeStudentPersonal( String localId,
                                                           NameType nameType, String firstName, String lastName )
        {
            StudentPersonal s = new StudentPersonal();
            s.RefId = Adk.MakeGuid();
            s.LocalId = localId;
            Name name = new Name( nameType, lastName, firstName );
            s.Name = name;
            return s;
        }

        private static int indent = 0;

        private static String INDENT = "                                          ";

        private void dumpMappings( Mappings root )
        {
            string line = root.Id + " (" + root.ChildCount + ") v="
                          + root.SIFVersionFilterString + " s="
                          + root.SourceIdFilterString + " z="
                          + root.ZoneIdFilterString;
            logger.Debug( INDENT.Substring( 0, indent ) + line );
            indent += 2;
            foreach ( Mappings child in root.Children )
            {
                dumpMappings( child );
            }
            indent -= 2;
        }
    }
}