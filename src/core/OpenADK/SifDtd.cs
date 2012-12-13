using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using OpenADK.Util;
using OpenADK.Library;
using OpenADK.Library.Impl;

namespace OpenADK.Library
{

    /// <summary>Metadata for the Schools Interoperability Framework (SIF)</summary>
    /// <remarks>
    /// <para>
    /// SIFDTD defines global {@linkplain com.edustructures.sifworks.ElementDef}
    /// constants that describe SIF Data Objects, elements, and attributes across all
    /// supported versions of the SIF Specification. The ADK uses this metadata
    /// internally to parse and render SIF Data Objects.  In addition, many of the
    /// framework APIs require that the programmer pass an ElementDef constant from
    /// the SIFDTD class to identify an object, element, or attribute.
    /// </para>
    /// <para>
    /// ElementDef constants are named <c>[PARENT_]ENTITY</c>, where
    /// <c>PARENT</c> is the name of the parent element and
    /// <c>ENTITY</c> is the name of the element or attribute
    /// encapsulated by the ElementDef. Some examples of ElementDef constants defined
    /// by this class include:
    /// </para>
    /// <list type="table">
    /// <listheader><term>IElementDef</term><description>Description</description></listheader>
    /// <item><term><c>SIFDTD.STUDENTPERSONAL</c></term><description>Identifies the StudentPersonal data object</description></item>
    /// <item><term><c>SIFDTD.SCHOOLINFO</c></term><description>Identifies the SchoolInfo data object</description></item>
    /// </list>
    /// Many of the Adk's public interfaces require an ElementDef constant to be passed
    /// as a parameter. For example, the first parameter to the <see cref="IZone.SetSubscriber"/>
    /// method is an IElementDef:
    /// <code>myZone.setSubscriber( SIFDTD.BUSINFO, this, ADKFlags.PROV_SUBSCRIBE );</code>
    /// ElementDef also identifies child elements and attributes as demonstrated by the <c>Query.AddCondition</c> method:
    /// <code>
    /// Query query = new Query( SifDtd.STUDENTPERSONAL );
    /// query.AddCondition( SifDtd.STUDENTPERSONAL_REFID, Condition.EQ, "4A37969803F0D00322AF0EB969038483" );
    /// </code>
    /// <para>
    /// <b>SDO Libraries</b>
    /// </para>
    /// <para>
    /// ElementDef metadata is grouped into "SDO Libraries", which are organized along
    /// SIF Working Group boundaries. SDO Libraries are loaded into the <c>SifDtd</c>
    /// class when the Adk is initialized. All or part of the metadata is loaded into depending on the flags passed to the
    /// <see cref="Adk.Initialize(SifVersion, SdoLibraryType)"/> method,
    /// metadata from one or more SDO Libraries may be loaded. For example, the following
    /// call loads metadata for the <c>Student Information Working Group Objects</c>
    /// and <c>Transportation And Geographic Information Working Group Objects</c>
    /// (Common Elements and <c>Infrastructure Working Group Objects</c> metadata is always loaded
    /// </para>
    /// <code>Adk.Initialize( SiFVersion.LATEST, SdoLibraryType.Student | SdoLibraryType.Trans )</code>
    /// <para>
    /// If an given SDO Library is not loaded, all of the SIFDTD constants that belong
    /// to that library will be <code>null</code> and cannot be referenced. For example,
    /// given the SDO Libraries loaded above, attempting to reference the
    /// <code>SIFDTD.LIBRARYPATRONSTATUS</code> object from the Library Automation Working
    /// Group would result in a NullPointerException:
    /// </para>
    /// <code>SifDtd.LIBRARYPATRONSTATUS.Name;</code>
    /// </remarks>
    public abstract class SifDtd : DTDInternals, ISifDtd
    {

        // Declare core object and field elements defined by all versions of SIF
        // supported by the class framework.

        // Package names that comprise the SIF Data Objects library
        public const string COMMON = "Common";
        public const string DATAMODEL = "Datamodel";
        public const string GLOBAL = "Global";
        public const string INFRA = "Infra";

        // The name of the data model variant this class is defined in
        public virtual string Variant
        {
            get { return null; }
        }

        public virtual List<string> LoadedLibraryNames
        {
            get { return null; }
        }

        /** The base xmlns for this edition of the ADK without the version */
        public virtual string XMLNS_BASE
        {
            get
            {
                return "http://www.sifinfo.org/" + ("us".Equals(Variant) ? "/" : Variant + "/") + "infrastructure";
            }
        }

        /**
         *  Returns the package name for all classes in this data model
         */
        public override String BasePackageName {
            get
            {
                return "OpenADK.Library." + Variant;
            }
        }

        /**
         *  Returns the base version-independent namespace for this data model variant
         */
        public override String BaseNamespace {
            get
            {
                if ( "us".Equals( Variant ))
                    return "http://www.sifinfo.org/infrastructure";
                else
                    return "http://www.sifinfo.org/" + Variant + "/infrastructure";
            }
        }

        /// <summary>  Gets the names of all Sdo libraries offered with this version of the Adk (Excluding Common, DataModel, and Infra)</summary>
        public virtual int[] AvailableLibraries
        {
            get { return null; }
        }

        public override string SDOAssembly
        {
            // TODO: This will use reflection in the future
            get { return "OpenADK.SDO-" + Variant.ToUpper(); }
        }

    }

}
