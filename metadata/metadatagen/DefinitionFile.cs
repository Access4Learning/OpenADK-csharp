using System;
using System.IO;
using System.Text;
using System.Xml;
using Edustructures.Metadata.DataElements;
using Edustructures.SifWorks;

namespace Edustructures.Metadata
{
    /// <summary>  Parses a definition file.
    /// *
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// 
    /// </version>
    public class DefinitionFile
    {
        /// <summary>  The local package for all classes generated from this file, defined by
        /// the #package directive. This name is used to construct a fully-qualified
        /// package name by concatenating the package prefix specified on the
        /// adkgen command-line with the version of SIF associated with the
        /// definition file. The result is a package name "{prefix}.{version}.{local-package}".
        /// For example, "com.edustructures.sifworks.sif10r1.student"
        /// </summary>
        protected internal String fPackage;

        protected internal String fLocalPackage;

        /// <summary>  The SIF version to which all definitions in this file apply
        /// </summary>
        protected internal SifVersion fVersion;

        protected internal String fNamespace;

        /// <summary>The file 
        /// </summary>
        protected internal FileInfo fSrc;

        protected internal String fSrcDir;

        /// <summary>The DOM Document 
        /// </summary>
        protected internal XmlDocument fDoc;

        /// <summary>The ObjectDef we're currently processing 
        /// </summary>
        protected internal ObjectDef fObjectDef;

        /// <summary>The FieldDef we're currently processing 
        /// </summary>
        protected internal FieldDef fFieldDef;

        /// <summary>The EnumDef we're currently processing 
        /// </summary>
        protected internal EnumDef fEnumDef;

        /// <summary>The DB object to which all definitions are written 
        /// </summary>
        protected internal DB fDB;

        /// <summary>Used to assign sequential IDs to <object>'s 
        /// </summary>
        protected internal int sID = 1;


        /// <summary>  Constructor
        /// </summary>
        public DefinitionFile( FileInfo f )
            : this( f.FullName ) {}

        /// <summary>  Constructor
        /// </summary>
        public DefinitionFile( String f )
        {
            fSrc = new FileInfo( f );
            fSrcDir = fSrc.FullName;
            int i = fSrcDir.LastIndexOf( Path.DirectorySeparatorChar.ToString() );
            if ( i != - 1 ) {
                fSrcDir = fSrcDir.Substring( 0, (i) - (0) );
            }
        }

        /// <summary>  Parses this definition file
        /// </summary>
        public virtual void parse()
        {
            Console.Out.WriteLine( "- " + fSrc );

            XmlDocument doc = new XmlDocument();
            doc.Load( fSrc.FullName );
            traverse( doc.DocumentElement );
        }


        /// <summary>
        /// Writes the specified object to the XmlDocument passed in. Writes only the object fields, not recursive
        /// </summary>
        /// <param name="def">The object to write</param>
        /// <param name="doc">The document to write the object to</param>
        public static void WriteObjectToDom( ObjectDef def,
                                             XmlDocument doc )
        {
            // TODO: Search for the element first and don't write it if it already exists

            string query = "object[@name=\"" + def.Name + "\"]";
            XmlElement node = (XmlElement) doc.DocumentElement.SelectSingleNode( query );
            if ( node == null ) {
                node = doc.CreateElement( "object" );
                //XmlSignificantWhitespace whitespace = doc.CreateSignificantWhitespace( "\r\n\r\n" );
                //doc.DocumentElement.AppendChild( whitespace );
                XmlComment comment = doc.CreateComment( def.Name );
                doc.DocumentElement.AppendChild( comment );
                //doc.DocumentElement.AppendChild( doc.CreateSignificantWhitespace( "\r\n\r\n" ) );
                doc.DocumentElement.AppendChild( node );
            }
            else {
                // TODO: We may want to retain node information in the future and do updates instead.
                // This has not yet been implemented
                node.RemoveAll();
            }


            node.SetAttribute( "name", def.Name );


            if ( def.RenderAs != null ) {
                node.SetAttribute( "renderAs", def.RenderAs );
            }
            else {
                if ( node.HasAttribute( "renderAs" ) ) {
                    node.RemoveAttribute( "renderAs" );
                }
            }

            if ( !def.ShouldValidate ) {
                node.SetAttribute( "validate", "false" );
            }
            else {
                if ( node.HasAttribute( "validate" ) ) {
                    node.RemoveAttribute( "validate" );
                }
            }

            if ( def.Topic ) {
                node.SetAttribute( "topic", "true" );
            }
            else {
                if ( node.HasAttribute( "topic" ) ) {
                    node.RemoveAttribute( "topic" );
                }
            }

            if ( (def.Flags & ObjectDef.FLAG_EMPTYOBJECT) > 0 ) {
                node.SetAttribute( "empty", "true" );
            }
            else {
                if ( node.HasAttribute( "empty" ) ) {
                    node.RemoveAttribute( "empty" );
                }
            }


            if ( def.Superclass != null ) {
                if (
                    ! (def.Superclass == "SIFMessagePayload" || def.Superclass == "SIFDataObject" ||
                       def.Superclass == "SIFElement") ) {
                    node.SetAttribute( "type", def.Superclass );
                }
                else {
                    if ( node.HasAttribute( "type" ) ) {
                        node.RemoveAttribute( "type" );
                    }
                }
            }

            if ( def.Draft ) {
                node.SetAttribute( "draft", "true" );
            }
            else {
                if ( node.HasAttribute( "draft" ) ) {
                    node.RemoveAttribute( "draft" );
                }
            }

            if ( def.ExtrasFile != null ) {
                FileInfo info = new FileInfo( def.ExtrasFile );
                node.SetAttribute( "extras", info.Name );
            }
            else {
                if ( node.HasAttribute( "draft" ) ) {
                    node.RemoveAttribute( "draft" );
                }
            }

            WriteDesc( node, def.Desc );
            WriteFieldsToDom( node, def );
        }

        public static void WriteEnumToDom( EnumDef def,
                                           XmlDocument doc )
        {
            string query = "enum[@name=\"" + def.Name + "\"]";
            XmlElement node = (XmlElement) doc.DocumentElement.SelectSingleNode( query );
            if ( node == null ) {
                node = doc.CreateElement( "enum" );
                //XmlSignificantWhitespace whitespace = doc.CreateSignificantWhitespace( "\r\n\r\n" );
                //doc.DocumentElement.AppendChild( whitespace );
                XmlComment comment = doc.CreateComment( def.Name );
                doc.DocumentElement.AppendChild( comment );
                //doc.DocumentElement.AppendChild( doc.CreateSignificantWhitespace( "\r\n\r\n" ) );
                doc.DocumentElement.AppendChild( node );
            }
            else {
                // TODO: We may want to retain node information in the future and do updates instead.
                // This has not yet been implemented
                node.RemoveAll();
            }
            node.SetAttribute( "name", def.Name );
            if ( def.Values.Count > 500 ) {
                XmlComment comment =
                    node.OwnerDocument.CreateComment
                        ( "Too many values to write: Value Count - " + def.Values.Count );
                node.AppendChild( comment );
            }
            else {
                foreach ( ValueDef val in def.Values) {
                    string qry = "value[@value=\"" + val.Value + "\"]";
                    XmlElement valueElement = (XmlElement) node.SelectSingleNode( qry );
                    if ( valueElement == null ) {
                        valueElement = node.OwnerDocument.CreateElement( "value" );
                        node.AppendChild( valueElement );
                    }
                    valueElement.SetAttribute( "name", createEnumName( val ) );
                    valueElement.SetAttribute( "value", val.Value );
                    if ( val.Desc != null && val.Desc.Length > 0 ) {
                        valueElement.SetAttribute( "desc", val.Desc );
                    }
                }
            }
        }

        private static String createEnumName( ValueDef value )
        {
            if( String.IsNullOrEmpty( value.Desc ) )
            {
                return makeName( null, value.Desc );
            }else
            {
                return makeName( value.Value, value.Desc );
            }
        }

        private static string makeName( String prefix, string desc )
        {


            if (desc.IndexOf("—") > 0)
            {
                desc = desc.Substring(0, desc.IndexOf("—"));
            }
            if (desc.Length > 50)
            {
                desc = desc.Substring(0, 50);
            }


            StringBuilder str = new StringBuilder( desc.ToUpper( ) );

            if(!String.IsNullOrEmpty( prefix ) && !(String.Compare(  prefix, desc, StringComparison.InvariantCultureIgnoreCase ) == 0 ) )
            {
                str.Insert(0, '_' );
                str.Insert( 0, prefix );
            }



            str = str.Replace(" A ", "");
            str = str.Replace(" AN ", "");
            str = str.Replace(" AND ", "");
            str = str.Replace(" THE ", "");
            str = str.Replace(" FOR ", "");
            str = str.Replace("OTHER ", "OT ");
            str = str.Replace("AUTHORISED", "AUTH");
            str = str.Replace("AUTHORIZED", "AUTH");
            str = str.Replace("COMPREHENSIVE", "COMP");

            str = str.Replace("&", "");
            str = str.Replace("\"", "");
            str = str.Replace("'", "");
            str = str.Replace(",", "");
            str = str.Replace(":", "");
            str = str.Replace("#", "");
            str = str.Replace('/', '_');
            str = str.Replace("-", "");
            str = str.Replace('(', ' ');
            str = str.Replace(')', ' ');
            str = str.Replace('\\', '_');
            str = str.Replace("  ", " ");
            str = str.Replace(' ', '_');
            str = str.Replace( "__", "_");

            String name = str.ToString();

            int len = name.LastIndexOf("_");
            if (len >= 20)
            {
                name = name.Substring(0, len);
            }
            while (name.EndsWith("_"))
            {
                name = name.Substring(0, name.Length - 1);
            }
            while( name.StartsWith( "_" ) )
            {
                  name = name.Substring(1, name.Length - 1);
            }
            return name;
        }

        private static void WriteDesc( XmlElement element,
                                       string desc )
        {
            if ( !String.IsNullOrEmpty( desc )  ) {
                desc = desc.Trim();
                desc = desc.Replace( "\r\n", "" );
                XmlElement descElement = element.OwnerDocument.CreateElement( "desc" );
                descElement.InnerText = desc;
                element.AppendChild( descElement );
            }
        }

        private static void WriteFieldsToDom( XmlElement objectElement,
                                              ObjectDef def )
        {
            FieldDef[] fields = def.Attributes;
            Array.Sort( fields, new FieldDefComparer() );
            foreach ( FieldDef attr in fields ) {
                XmlElement fieldAttribute = objectElement.OwnerDocument.CreateElement( "attribute" );
                objectElement.AppendChild( fieldAttribute );
                WriteFieldToDom( fieldAttribute, attr );
            }


            fields = def.Elements;
            Array.Sort( fields, new FieldDefComparer() );
            foreach ( FieldDef ele in def.Elements ) {
                XmlElement fieldElement = objectElement.OwnerDocument.CreateElement( "element" );
                objectElement.AppendChild( fieldElement );
                WriteFieldToDom( fieldElement, ele );
            }
        }

        private static void WriteFieldToDom( XmlElement fieldElement,
                                             FieldDef field )
        {
            fieldElement.SetAttribute( "name", field.Name );
            FieldType type = field.FieldType;
            if ( type.IsEnum ) {
                fieldElement.SetAttribute( "enum", type.Enum );
            }
            else {
                fieldElement.SetAttribute( "type", type.MetadataType );
            }


            if ( field.RenderAs != null ) {
                fieldElement.SetAttribute( "renderAs", field.RenderAs );
            }

            fieldElement.SetAttribute( "flags", field.GetFlags() );

            if ( field.Draft ) {
                fieldElement.SetAttribute( "draft", "true" );
            }

            if ( field.Attribute && (field.fFlags & FieldDef.FLAG_NOT_A_KEY) > 0 ) {
                fieldElement.SetAttribute( "key", "false" );
            }

      
                WriteDesc( fieldElement, field.Desc );
        }

        /// <summary>  Traverse a node
        /// </summary>
        public virtual void traverse( XmlElement node )
        {
            // is there anything to do?
            if ( node == null ) {
                return;
            }

            String name = node.Name;
            if ( name.StartsWith( "adk" ) ) {
                onRoot( node );
            }
            else if ( name.StartsWith( "obj" ) ) {
                onObject( node, false );
            }
            else if ( name.StartsWith( "enu" ) ) {
                onEnumeration( node );
            }
            else if ( name.StartsWith( "att" ) ) {
                onAttribute( node );
            }
            else if ( name.StartsWith( "ele" ) ) {
                onElement( node );
            }
            else if ( name.StartsWith( "des" ) ) {
                onDesc( node );
            }
            else if ( name.StartsWith( "val" ) ) {
                onValue( node );
            }
            else if ( name.StartsWith( "inf" ) ) {
                onObject( node, true );
            }
            else {
                throw new ParseException( "<" + name + "> is not a recognized element" );
            }

            XmlNodeList children = node.ChildNodes;
            if ( children != null ) {
                int len = children.Count;
                for ( int i = 0; i < len; i++ ) {
                    if ( children[i].NodeType == XmlNodeType.Element ) {
                        traverse( (XmlElement) children[i] );
                    }
                }
            }
        }

        protected internal virtual void onRoot( XmlElement node )
        {
            fLocalPackage = getAttr( node, "package" );
            String ver = getAttr( node, "version" );
            fNamespace = getAttr( node, "namespace" );

            if ( fLocalPackage == null || ver == null || fNamespace == null ) {
                throw new ParseException
                    ( "<adk> must specify the package=, version=, and namespace= attributes" );
            }

            fVersion = SifVersion.Parse( ver );
            fPackage = "Edustructures.SifWorks." + fLocalPackage;

            Console.Out.WriteLine( "    Package=" + fPackage );
            Console.Out.WriteLine( "    Version=" + ver + " (" + this.fVersion + ")" );
            Console.Out.WriteLine( "    Namespace=" + fNamespace );

            fDB = MetaDataSchema.getDB( fVersion );
        }

        protected internal virtual void onDesc( XmlElement node )
        {
            String desc = getText( node );
            if ( fFieldDef != null ) {
                fFieldDef.Desc = desc;
            }
            else if ( fObjectDef != null ) {
                fObjectDef.Desc = desc;
            }
        }

        protected internal virtual void onObject( XmlElement node,
                                                  bool infra )
        {
            String name = getAttr( node, "name" );
            if ( name == null ) {
                throw new ParseException( "<object> or <infra> must specify a name= attribute" );
            }

            System.Diagnostics.Debug.Assert( !(name == "LAInfo") );

            if ( infra ) {
                Console.Out.WriteLine( "    Infra: " + name );
            }
            else {
                Console.Out.WriteLine( "    Object: " + name );
            }

            fObjectDef = fDB.defineObject( sID++, name, fLocalPackage, fSrc.FullName );

            if( infra )
            {
                fObjectDef.setInfra();
            }

            // Note: for the purpose of comparision, we want to define this object using the 
            // RenderAs value, if present. This is different behavior than ADKGen Does
            System.Diagnostics.Debug.Assert( !(name.ToLower() == "ftamount" ) );
            
            
            // default Validate to True
            string validate = node.GetAttribute( "validate" );
            fObjectDef.ShouldValidate = !(validate.ToLower() == "false");


            fFieldDef = null;
            fEnumDef = null;
            String topic = getAttr( node, "topic" );
            fObjectDef.Topic = topic != null &&
                               (topic.ToUpper().Equals( "yes".ToUpper() ) ||
                                topic.ToUpper().Equals( "true".ToUpper() ));
            fObjectDef.RenderAs = getAttr( node, "renderAs" );
            fObjectDef.LatestVersion = fVersion;
            fObjectDef.EarliestVersion = fVersion;

            if ( getBooleanAttr( node, "empty", false ) ) {
                fObjectDef.Flags = fObjectDef.Flags | ObjectDef.FLAG_EMPTYOBJECT;
            }
            if ( !getBooleanAttr( node, "sifdtd", true ) ) {
                fObjectDef.Flags = fObjectDef.Flags | ObjectDef.FLAG_NO_SIFDTD;
            }
            if ( getBooleanAttr( node, "shared", false ) ) {
                fObjectDef.Shared = true;
            }

            String override_Renamed = getAttr( node, "sequenceOverride" );
            if ( override_Renamed != null ) {
                try {
                    fObjectDef.SequenceOverride = Int32.Parse( override_Renamed );
                }
                catch ( FormatException nfe ) {
                    throw new ParseException
                        ( "Invalid sequenceOverride value: " + override_Renamed, nfe );
                }
            }

            String supercls = getAttr( node, "superclass" );
            if ( supercls == null ) {
                if ( infra ) {
                    supercls = "SIFMessagePayload";
                }
                else if ( fObjectDef.Topic ) {
                    supercls = "SIFDataObject";
                }
                else {
                    supercls = "SIFElement";
                }
            }

            String typ = getAttr( node, "type" );
            if ( typ != null ) {
                supercls = typ;
            }

            String draft = getAttr( node, "draft" );
            if ( draft != null && draft.ToUpper().Equals( "true".ToUpper() ) ) {
                fObjectDef.setDraft();
            }

            fObjectDef.Superclass = supercls;
            String extras = getAttr( node, "extras" );
            if ( extras != null ) {
                fObjectDef.ExtrasFile = fSrcDir + Path.DirectorySeparatorChar + extras;
            }
        }

        protected internal virtual void onEnumeration( XmlElement node )
        {
            String name = getAttr( node, "name" );
            fEnumDef = new EnumDef( name, fSrc.FullName );
            String value = getAttr( node, "value" );
     
            Console.Out.WriteLine( "    Enumeration: " + name );

            fEnumDef.LatestVersion = fVersion;
            fEnumDef.EarliestVersion = fVersion;
            fDB.defineEnum( name, fEnumDef );
        }

        protected internal virtual void onValue( XmlElement node )
        {
            if ( fEnumDef != null ) {
                String val = getAttr( node, "value" );
                String desc = getAttr( node, "desc" );
                String name = getAttr( node, "name" );
                if ( name == null ) {
                    name = val;
                }
                //fEnumDef.defineValue(name, value_Renamed, desc);
                fEnumDef.DefineValue( name, val, desc );
            }
        }

        protected internal virtual void onAttribute( XmlElement node )
        {
            fEnumDef = null;

            String name = getAttr( node, "name" );
            String type = getAttr( node, "type" );
            String flags = getAttr( node, "flags" );
            String enum_Renamed = getAttr( node, "enum" );

            System.Diagnostics.Debug.Assert( enum_Renamed != "FTAmountType" );

            StringBuilder buffer = new StringBuilder( "    - " );
            buffer.Append( name );
            if ( type != null ) {
                buffer.Append( "{" );
                buffer.Append( type );
                buffer.Append( "}" );
            }
            // Note: for the purpose of comparision, we want to define this object using the 
            // RenderAs value, if present. This is different behavior than ADKGen Does

            string renderAs = getAttr( node, "renderAs" );
            if ( renderAs != null ) {
                if ( renderAs.StartsWith( "xml:" ) ) {
                    renderAs = renderAs.Substring( 4 );
                }
                fFieldDef = fObjectDef.DefineAttr( renderAs, type );
                fFieldDef.Name = name;
                fFieldDef.RenderAs = renderAs;
            }
            else {
                fFieldDef = fObjectDef.DefineAttr( name, type );
            }

            if ( flags != null ) {
                fFieldDef.SetFlags( flags );
            }
            fFieldDef.SetEnum( enum_Renamed );

            if ( (fFieldDef.Flags & FieldDef.FLAG_REQUIRED) != 0 &&
                 !getBooleanAttr( node, "key", true ) ) {
                //  By default all attributes with a "R" flag are used to generate
                //  the object's key. However, some attributes have an "R" flag but
                //  are not part of the key. When the key="false" attribute is
                //  specified, set the FLAG_NOT_A_KEY flag.

                fFieldDef.Flags = fFieldDef.Flags | FieldDef.FLAG_NOT_A_KEY;
            }

            fFieldDef.LatestVersion = fVersion;
            fFieldDef.EarliestVersion = fVersion;

            Console.Out.WriteLine( buffer );
        }

        protected internal virtual void onElement( XmlElement node )
        {
            fEnumDef = null;

            String name = getAttr( node, "name" );
            String type = getAttr( node, "type" );
            String flags = getAttr( node, "flags" );
            String enumVal = getAttr( node, "enum" );

            StringBuilder buf = new StringBuilder( "    > " );
            buf.Append( name );
            if ( type != null ) {
                buf.Append( "{" );
                buf.Append( type );
                buf.Append( "}" );
            }

            fFieldDef = fObjectDef.DefineElement( name, type );
            if ( flags != null ) {
                fFieldDef.SetFlags( flags );
            }
            if ( enumVal != null ) {
                fFieldDef.SetEnum( enumVal );
            }
            fFieldDef.RenderAs = getAttr( node, "renderAs" );

            String seqOverride = getAttr( node, "sequenceOverride" );
            if ( seqOverride != null ) {
                try {
                    fFieldDef.SequenceOverride = int.Parse( seqOverride );
                }
                catch ( FormatException nfe ) {
                    throw new ParseException( "Invalid sequenceOverride value: " + seqOverride, nfe );
                }
            }

            if ( !getBooleanAttr( node, "sifdtd", true ) ) {
                fFieldDef.Flags = fFieldDef.Flags | FieldDef.FLAG_NO_SIFDTD;
            }
            if ( !getBooleanAttr( node, "encode", true ) ) {
                fFieldDef.Flags = fFieldDef.Flags | FieldDef.FLAG_DO_NOT_ENCODE;
            }

            if ( getBooleanAttr( node, "collapsed", false ) ) {
                fFieldDef.Flags = fFieldDef.Flags | FieldDef.FLAG_COLLAPSED;
            }


            String draft = getAttr( node, "draft" );
            if ( draft != null && bool.Parse( draft ) ) {
                fFieldDef.setDraft();
            }

            fFieldDef.LatestVersion = fVersion;
            fFieldDef.EarliestVersion = fVersion;

            if ( (fFieldDef.Flags & FieldDef.FLAG_MANDATORY) != 0 &&
                 !getBooleanAttr( node, "key", true ) ) {
                //  By default all attributes with a "R" flag are used to generate
                //  the object's key. However, some attributes have an "R" flag but
                //  are not part of the key. When the key="false" attribute is
                //  specified, set the FLAG_NOT_A_KEY flag.

                fFieldDef.Flags = fFieldDef.Flags | FieldDef.FLAG_NOT_A_KEY;
            }

            Console.WriteLine( buf );

            fFieldDef.Validate();
        }

        protected internal virtual String getText( XmlElement node )
        {
            if ( node != null ) {
                XmlNodeList ch = node.ChildNodes;
                for ( int i = 0; i < ch.Count; i++ ) {
                    if ( ch[i].NodeType == XmlNodeType.Text ) {
                        return ch[i].InnerText;
                    }
                }
            }
            return null;
        }

        protected internal virtual String getAttr( XmlElement node,
                                                   String attr )
        {
            string val = node.GetAttribute( attr );
            if ( val.Length == 0 ) {
                return null;
            }
            else {
                return val;
            }
        }

        protected internal virtual bool getBooleanAttr( XmlElement node,
                                                        String attr,
                                                        bool defValue )
        {
            String s = getAttr( node, attr );
            if ( s == null ) {
                return defValue;
            }
            if ( s.ToUpper().Equals( "true".ToUpper() ) || s.ToUpper().Equals( "yes".ToUpper() ) ) {
                return true;
            }

            return false;
        }
    }
}