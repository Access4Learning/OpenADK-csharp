//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using OpenADK.Library.Global;
using OpenADK.Util;

/**
 * 	This private interface is implemented by the SIFDTD classes of each 
 * 	data model variant. The methods in this interface are considered internal
 * 	to the ADK and therefore not exposed through the DTD interface.
 * 
 *  @since 2.3
 */

namespace OpenADK.Library.Impl
{
    public abstract class DTDInternals : IDtd
    {

        // SIF_Message mapping used internally by SIFParser
        public static IElementDef SIF_MESSAGE = new ElementDefImpl(null, "SIF_Message", null, 0, "Impl", SifVersion.SIF11, SifVersion.LATEST);
        public static IElementDef SIF_MESSAGE_VERSION = new ElementDefImpl(SIF_MESSAGE, "Version", null, 1, SifDtd.INFRA, null, (byte)(ElementDefImpl.FD_FIELD), SifVersion.SIF11, SifVersion.LATEST);

        protected DTDInternals()
	    {
            fElementDefs = new Dictionary<String, IElementDef>(704);
		    fElementDefs[ "SIF_Message" ] = SIF_MESSAGE;
		    fElementDefs[ "SIF_Message_Version" ] = SIF_MESSAGE_VERSION;
	    }


        protected int fLoaded = 0;
        protected IDictionary<String, IElementDef> fElementDefs;

        public abstract string BaseNamespace { get;}
        public abstract string BasePackageName { get; }

        /// <summary>
        /// Looks up the element def with the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IElementDef LookupElementDef(String key)
        {
            IElementDef returnValue;
            fElementDefs.TryGetValue(key, out returnValue);
            return returnValue;
        }

        /// <summary>
        /// Looks up the ElementDef with the given key within the context of its parent
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childTag"></param>
        /// <returns></returns>
        public IElementDef LookupElementDef(IElementDef parent, string childTag)
        {
            IElementDef returnValue;
            fElementDefs.TryGetValue(parent.Name + "_" + childTag, out returnValue);
            return returnValue;
        }

        /// <summary>
        /// Loads the SDO Libraries specified by using flags from the <c>SdoLibraryType</c> enum
        /// </summary>
        /// <param name="libraries"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void LoadLibraries(int libraries)
        {
            string adkAssembly = GetType().Assembly.GetName().Name;
            string sdoAssembly = this.SDOAssembly;
            if (fLoaded == 0)
            {
                foreach (int intrinsic in GetSdoTypes(IntrinsicLibraries))
                {
                    LoadLibrary(intrinsic, adkAssembly);
                }
            }

            LoadLibrary((int)IntrinsicLibraryType.Common, sdoAssembly);
 
            //Remove the libraries that have already been loaded.
            int toLoad = libraries & ~IntrinsicLibraries;

            toLoad = toLoad & ~(int) IntrinsicLibraryType.Common;

            foreach (int lib in GetSdoTypes(toLoad))
            {
                LoadLibrary(lib, sdoAssembly);
            }
        }

        public abstract String SDOAssembly
        { get;
        }


        /// <summary>
        /// These are the libraries that are always loaded by the ADK because they are
        /// a required part of the infrastructure
        /// </summary>
        protected int IntrinsicLibraries 
        { 
            get
            {
                return (int)(IntrinsicLibraryType.Global | IntrinsicLibraryType.Infra );
            }
        }

        protected abstract String GetLibraryName(int type);

        /// <summary>
        /// Loads in individiual library, given it's common name
        /// </summary>
        /// <remarks>
        /// This method does not load libraries from strong-named assemblies, which
        /// would require specifying the public key token
        /// </remarks>e
        /// <param name="type"></param>
        /// <param name="assemblyName"></param>
        private void LoadLibrary( int type, string assemblyName)
        {
            if ( (fLoaded & type) == 0 )
            {
                string baseNamespace;
                if( assemblyName.Contains("SDO-" ) )
                {
                    baseNamespace = this.BasePackageName;
                }
                else
                {
                    baseNamespace = typeof (Adk).Namespace;
                }
                string name = GetLibraryName(type);
                String cls = baseNamespace + "." + name + "." + name + "DTD";
                cls = cls + ", " + assemblyName;
                try
                {
                    SdoLibraryImpl lib = (SdoLibraryImpl)ClassFactory.CreateInstance(cls);
                    lib.Load();
                    lib.AddElementMappings(fElementDefs);
                }
                catch (TypeLoadException cnfe)
                {
                    throw new AdkException("SDOLibrary class \"" +
                                            cls +
                                            "\" not found (make sure the appropriate SDO Library dll is on the classpath)",
                                            null, cnfe);
                }
                catch (Exception ie)
                {
                    throw new AdkException("Cannot load the \"" +
                                            name + "\" SIF Data Object library: " + ie, null, ie);
                }
                fLoaded |= (int)type;
            }
        }


  


        /// <summary>  Gets the individual SdoLibrary type values represented in the given
        /// set of flags</summary>
        /// <param name="library">The library identifier (e.g. SdoLibraryType.Food)</param>
        /// <returns>An array of SdoLibraryTypes naming all Sdo Libraries identified by the
        /// libraries value</returns>
        protected abstract List<int> GetSdoTypes(int libraryTypes);
        

        /// <summary>  Get the SIF namespace for a given version of the specification.</summary>
        /// <returns> If the SifVersion is less than SIF 1.1, a namespace in the form
        /// "http://www.sifinfo.org/v1.0r2/messages" is returned, where the full
        /// SIF Version number is included in the namespace. For SIF 1.x and
        /// later, a namespace in the form "http://www.sifinfo.org/infrastructure/1.x"
        /// is returned, where only the major version number is included in the
        /// namespace.
        /// </returns>
        public string GetNamespace(SifVersion version)
        {
            return version.Xmlns;
        }

        /// <summary>
        /// Gets the element tag name of a SifMessageType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetElementTag(int type)
        {
            if (Enum.IsDefined(typeof(SifMessageType), type))
            {
                return type.ToString();
            }
            else
            {
                return null;
            }
        }

        public SifMessageType GetElementType(string name)
        {
            try
            {
                SifMessageType type = (SifMessageType)Enum.Parse(typeof(SifMessageType), name, true);
                return type;
            }
            catch
            {
                return SifMessageType.None;
            }
        }

        private static bool XPATH_DEBUG = false;

        /// <summary>  Lookup an IElementDef given a SIF Query Pattern string.</summary>
        /// <param name="relativeTo">An IElementDef for a root SIF Data Object to which the
        /// query is relative (e.g. SifDtd.STUDENTPERSONAL)
        /// </param>
        /// <param name="query">A SIF Query Pattern (e.g. "@RefId", "Name/FirstName", etc.)
        /// </param>
        public IElementDef LookupElementDefBySQP(IElementDef relativeTo, string query)
        {
            return LookupElementDefBySQP(relativeTo, query, 0);
        }


        /// <summary>
        /// Attempts to convert the path to the specified version of SIF. If it is unable to perform
        /// a conversion, due to being unable to parse the path, the original path will be returned, instead,
        /// and a warning will be written to the ADK log
        /// </summary>
        /// <param name="objectType">The metadata object representing the object type</param>
        /// <param name="path">The XPath query (such as "Demographics/Ethnicity")</param>
        /// <param name="version">The SIF version to render the path in, such as SIFVersion.SIF20</param>
        /// <returns></returns>
        public String TranslateSQP(IElementDef objectType, String path, SifVersion version)
        {
            String returnValue = path;
            try
            {
                List<Segment> segments = ParseSQP(objectType, path);
                if (segments == null)
                {
                    Adk.Log.Warn("Unable to translate SIF Query Pattern: " + path);
                    return string.Empty;
                }
                if (segments.Count > 0)
                {
                    StringBuilder pathBuilder = new StringBuilder();
                    int actualSegments = 0;
                    foreach (Segment segment in segments)
                    {
                        int segmentStart = pathBuilder.Length;
                        if (segment.AppendToSQP(pathBuilder, version))
                        {
                            actualSegments++;
                        }
                        if (actualSegments > 1)
                        {
                            pathBuilder.Insert(segmentStart, "/");
                        }
                    }
                    returnValue = pathBuilder.ToString();
                }
            }
            catch (Exception iae)
            {
                Adk.Log.Warn("Unable to translate SIF Query Pattern: " + path + " Error: " + iae, iae);
            }
            return returnValue;
        }


        /// <summary>
        /// Looks up an IElementDef by SQP using recursion
        /// </summary>
        /// <param name="relativeTo"></param>
        /// <param name="query"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private IElementDef LookupElementDefBySQP(IElementDef relativeTo, string query, int startIndex)
        {
            if (relativeTo == null || query.Length == startIndex)
            {
                return null;
            }
            if (query.Length > (startIndex - 1) && query[startIndex] == '@')
            {
                //  Query identifies an attribute of relativeTo
                return LookupElementDef(relativeTo, query.Substring(startIndex + 1));
            }

            //  Query identifies a child element of relativeTo
            int segment = query.IndexOf('/', startIndex);

            // Look for and remove any xpath predicates.
            int segmentEnd = query.IndexOf('[', startIndex);

            if (segment == -1)
            {
                // This is the final segment in the SQP
                if (segmentEnd == -1)
                {
                    // No xpath predicate was found
                    segmentEnd = query.Length;
                }
                return LookupElementDef(relativeTo, query.Substring(startIndex, segmentEnd - startIndex));
            }
            else
            {
                // There are more segments to process in the SQP
                if (segmentEnd == -1)
                {
                    // No predicate was found. The end of the segment should be the index of the '/'
                    segmentEnd = segment;
                }
                else if (segmentEnd > segment)
                {
                    // A predicate was found, but it was past the current segment
                    segmentEnd = segment;
                }

                if (segmentEnd == startIndex)
                {
                    // Double slashes in the SQP path should resolve to null;
                    return null;
                }
                IElementDef segmentDef =
                    LookupElementDef(relativeTo, query.Substring(startIndex, segmentEnd - startIndex));
                return LookupElementDefBySQP(segmentDef, query, segment + 1);
            }
        }


        /// <summary>  Find an SifElement given a SIF Query Pattern string.</summary>
        /// <param name="relativeTo">An IElementDef identifying a SIF Data Object to which
        /// the query is relative to (e.g. STUDENTPERSONAL, BUSINFO, etc.)</param>
        /// <param name="query">A SIF Query Pattern string as described by the SIF 1.0r1
        /// Specification</param>
        /// <returns> The Element satisfying the query, or <c>null</c> if no
        /// match was found. If the query resolves to an attribute, a SimpleField
        /// object is returned. If the query resolves to an element, a SifElement
        /// object is returned. In both cases the caller can obtain the text
        /// value of the attribute or element by calling its <c>TextValue</c>
        /// property.
        /// </returns>
        public Element LookupBySQP(SifDataObject relativeTo, string query)
        {
            return LookupByXPath(relativeTo, query, null);
        }


        /// <summary>
        /// Creates a parsed list of segments from the SIF Query Pattern xpath string
        /// </summary>
        /// <param name="relativeTo"></param>
        /// <param name="query"></param>
        /// <returns>A list of segments or null if the query cannot be parsed into segments</returns>
        private List<Segment> ParseSQP(IElementDef relativeTo, String query)
        {
            List<Segment> segments = new List<Segment>();
            if (relativeTo == null)
            {
                return null;
            }

            IElementDef previousSegment = relativeTo;

            String[] segmentTokens = query.Split('/');
            foreach (String token in segmentTokens)
            {
                if (token.Length == 0)
                {
                    Adk.Log.Warn("Unable to parse empty segment in SQP: " + query);
                    // Empty segment. Exit 
                    // We could throw an exception here, but the previous ADK code
                    // allowed for empty segments (and returned null).
                    break;
                }

                // Look for any xpath predicates.
                String elementName = token;
                int segmentEnd = token.IndexOf('[');
                if (segmentEnd > -1)
                {
                    elementName = token.Substring(0, segmentEnd);
                }
                if (elementName[0] == '@')
                {
                    elementName = elementName.Substring(1);
                }
                IElementDef current = lookupChild(previousSegment, elementName);
                if (current == null)
                {
                    // Unable to parse this path. Return null;
                    return null;
                }
                // Look for any "collapsed" element definitions. For example,
                // "Demographics/Ethnicity" should translate to "Demographics/RaceList/Race"
                IElementDef parent = current.Parent;
                if (parent != null)
                {
                    IElementVersionInfo sif15Info = parent.GetVersionInfo(SifVersion.SIF15r1);
                    if (sif15Info != null)
                    {
                        IElementDef candidate = lookupChild(previousSegment, sif15Info.Tag);
                        if (candidate != null && candidate.IsCollapsed(SifVersion.SIF15r1))
                        {
                            segments.Add(new Segment(candidate));
                        }
                    }
                }


                Segment next = new Segment(current);
                if (segmentEnd > -1)
                {
                    int predicateEnd = token.IndexOf("]", segmentEnd);
                    if (predicateEnd == -1)
                    {
                        predicateEnd = token.Length;
                    }
                    ParsePredicates(next, token.Substring(segmentEnd + 1, predicateEnd - segmentEnd - 1));
                }
                segments.Add(next);
                previousSegment = current;
            }


            return segments;
        }

        private void ParsePredicates(Segment segment, String predicates)
        {
            // Very simplistic... Only support comma-seperated predicates
            String[] predicateTokens = predicates.Split(',');
            foreach (String token in predicateTokens)
            {
                // Very simplistic.... Only support simple predicates (field=value)
                int pos = token.IndexOf('=');
                if (pos == -1)
                {
                    throw new ArgumentException("Unable to parse predicate expression: " + predicates);
                }
                int start = token.StartsWith("@") ? 1 : 0;
                IElementDef def = lookupChild(segment.fElementDef, token.Substring(start, pos - start).Trim());
                if (def == null)
                {
                    throw new ArgumentException("Unable to resolve element or attribute: " + predicates);
                }
                segment.AddPredicate(def, token.Substring(pos));
            }
        }


        /// <summary>
        /// Looks up a child element def
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="token"></param>
        /// <returns>The looked up metadata def or null if not found</returns>
        private IElementDef lookupChild(IElementDef parent, String token)
        {
            IElementDef current = LookupElementDef(parent, token);
            if (current == null)
            {
                current = LookupElementDef(token);
            }
            if (current == null)
            {
                if (String.Compare(token, "SIF_ExtendedElements", true) == 0)
                {
                    current = GlobalDTD.SIF_EXTENDEDELEMENTS;
                }
                else if (String.Compare(token, "SIF_Metadata", true) == 0)
                {
                    // TODO: Fix this to return SIF_Metadata from the current namespace
                    current = null;
                }
            }
            return current;
        }

        private class Segment
        {
            internal IElementDef fElementDef;
            private List<Predicate> fPredicates;


            /// <summary>
            /// Creates a segment representing a portion of an XPath
            /// </summary>
            /// <param name="def"></param>
            internal Segment(IElementDef def)
            {
                fElementDef = def;
            }


            /// <summary>
            /// Adds a predicate condition to this segment of the XPath
            /// </summary>
            /// <param name="def"></param>
            /// <param name="condition"></param>
            internal void AddPredicate(IElementDef def, String condition)
            {
                if (fPredicates == null)
                {
                    fPredicates = new List<Predicate>();
                }
                fPredicates.Add(new Predicate(def, condition));
            }

            /// <summary>
            /// Writes this segment to the StringBuilder using the specified version of SIF
            /// </summary>
            /// <param name="builder">the StringBuilder to append to</param>
            /// <param name="version">The version of SIF to render in</param>
            /// <returns></returns>
            internal bool AppendToSQP(StringBuilder builder, SifVersion version)
            {
                IElementVersionInfo evi = fElementDef.GetVersionInfo(version);
                if (evi == null)
                {
                    // This element is not supported in this version of SIF
                    throw new ArgumentException(fElementDef.Name + " is not supported in SIF Version " + version);
                }
                if (evi.IsCollapsed)
                {
                    return false;
                }
                if (evi.IsAttribute)
                {
                    builder.Append('@');
                }
                builder.Append(evi.Tag);
                if (fPredicates != null)
                {
                    builder.Append('[');
                    for (int a = 0; a < fPredicates.Count; a++)
                    {
                        if (a > 0)
                        {
                            builder.Append(" and ");
                        }
                        Predicate p = fPredicates[a];
                        p.AppendToSQP(builder, version);
                    }
                    builder.Append(']');
                }

                return true;
            }
        }

        private class Predicate
        {
            private IElementDef fElementDef;
            private String fCondition;

            internal Predicate(IElementDef def, String condition)
            {
                fElementDef = def;
                fCondition = condition;
            }

            internal void AppendToSQP(StringBuilder builder, SifVersion version)
            {
                IElementVersionInfo evi = fElementDef.GetVersionInfo(version);
                if (evi.IsAttribute)
                {
                    builder.Append('@');
                }
                builder.Append(evi.Tag);
                builder.Append(fCondition);
            }
        }


        /// <summary>  Find an SifElement given an XPath-like query string.</summary>
        /// <remarks>
        /// Query strings can only take one of these forms:
        /// 
        /// <list type="bullet">
        /// <item><term><c>@Attr</c></term></item>
        /// <item><term><c>Element</c></term></item>
        /// <item><term><c>Element/@Attr</c></term></item>
        /// <item><term><c>Element/Element/.../@Attr</c></term></item>
        /// <item><term><c>Element[@Attr='value1']</c></term></item>
        /// <item><term><c>Element[@Attr1='value1',@Attr2='value2',...]/Element/...</c></term></item>
        /// </list>
        /// 
        /// </remarks>
        /// <param name="relativeTo">A SifDataObject to which the query is relative to.
        /// </param>
        /// <param name="query">An XPath-like query string as described above
        /// 
        /// </param>
        /// <returns> The Element satisfying the query, or <c>null</c> if no
        /// match was found. If the query resolves to an attribute, a SimpleField
        /// object is returned. If the query resolves to an element, a SifElement
        /// object is returned. In both cases the caller can obtain the text
        /// value of the attribute or element by calling its <c>TextValue</c>
        /// property.
        /// </returns>
        public Element LookupByXPath(SifDataObject relativeTo, string query)
        {
            return LookupByXPath(relativeTo, query, null);
        }

        /// <summary>  Find an SifElement given an XPath-like query string.</summary>
        /// <remarks> 
        /// <para>
        /// Query strings can only take one of these forms:
        /// </para>
        /// <list type="bullet">
        /// <item><term><c>@Attr</c></term></item>
        /// <item><term><c>Element</c></term></item>
        /// <item><term><c>Element/@Attr</c></term></item>
        /// <item><term><c>Element/Element/.../@Attr</c></term></item>
        /// <item><term><c>Element[@Attr='value1']</c></term></item>
        /// <item><term><c>Element[@Attr1='value1',@Attr2='value2',...]/Element/...</c></term></item>
        /// </list>
        /// 
        /// <para>
        /// When the <i>create</i> parameter is true, the method will ensure that
        /// the elements and attributes specified in the query string are created in
        /// the SifDataObject. All values are evaluated by the ValueBuilder implementation
        /// passed to this method. In addition, the query string may end with a value
        /// expression in the form "<c>Element[@Attribute='val']=<b><i>expression</i></b></c>",
        /// where <i>expression</i> is evaluated by the <c>ValueBuilder</c>.
        /// Refer to the <c>DefaultValueBuilder</c> class for a description of
        /// how value expressions are evaluated by in XPath query strings by default.
        /// </para>
        /// <para>
        /// Note that when <i>create</i> is true, this method will attempt to create
        /// a new element when a set of attributes is specified and an element does
        /// not already exist with those same attribute settings. This is not always
        /// desirable, however. For example, if you call this method in succession
        /// with the following XPath query strings, the result will be a single
        /// <c>OtherId[@Type='ZZ']</c> element with a value of "$(School)".
        /// This is because each call will match the <c>OtherId[@Type='ZZ']</c>
        /// element created by the first call, and will replace its value instead of
        /// creating an new instance of the <c>OtherId</c> element:
        /// </para>
        /// 
        /// <para>
        /// <code>
        /// OtherId[@Type='ZZ']=GRADE:$(Grade)
        /// OtherId[@Type='ZZ']=HOMEROOM:$(HomeRoom)
        /// OtherId[@Type='ZZ']=SCHOOL:$(School)
        /// </code>
        /// </para>
        /// 
        /// <para>
        /// Produces:
        /// </para>
        /// <para>
        /// <c>&lt;OtherId Type='ZZ'&gt;SCHOOL:$(School)&lt;/OtherId&gt;</c>
        /// </para>
        /// <para>
        /// To instruct the function to always create a new instance of an element
        /// even when a matching element is found, append the attribute list with a
        /// plus sign. The plus sign must come immediately before the closing right
        /// backet regardless of how many attributes are specified in the attribute
        /// list:
        /// </para>
        /// <para>
        /// <code>
        /// OtherId[@Type='ZZ'+]=GRADE:$(Grade)
        /// OtherId[@Type='ZZ'+]=HOMEROOM:$(HomeRoom)
        /// OtherId[@Type='ZZ'+]=SCHOOL:$(School)
        /// </code>
        /// </para>
        /// <para>
        /// Produces:
        /// </para>
        /// <para>
        /// <code>
        /// &lt;OtherId Type='ZZ'&gt;GRADE:$(Grade)&lt;/OtherId&gt;
        /// &lt;OtherId Type='ZZ'&gt;HOMEROOM:$(HomeRoom)&lt;/OtherId&gt;
        /// &lt;OtherId Type='ZZ'&gt;SCHOOL:$(School)&lt;/OtherId&gt;
        /// </code>
        /// </para>
        /// </remarks>
        /// <param name="relativeTo">A SifDataObject to which the query is relative to.</param>
        /// <param name="query">An XPath-like query string as described above.</param>
        /// <returns> The Element satisfying the query, or <c>null</c> if no
        /// match was found (unless the <i>create</i> parameter is true). If the
        /// query resolves to an attribute, a SimpleField object is returned. If
        /// it resolves to an element, a SifElement object is returned. In both
        /// cases the caller can obtain the text value of the attribute or
        /// element by calling its <c>TextValue</c> property.
        /// </returns>
        /// <param name="valueBuilder"></param>
        public Element LookupByXPath(SifDataObject relativeTo, string query, IValueBuilder valueBuilder)
        {
            int i = query.IndexOf('/');

            Element result =
                _xpath(relativeTo, query.Substring(0, (i == -1 ? query.Length : i) - (0)),
                        i == -1 ? null : query.Substring(i + 1));

            return result;
        }


        /// <summary>
        /// Create all elements and attributes referenced by the XPath-like query string.
        /// </summary>
        /// <param name="relativeTo">The element that is the starting point of the path</param>
        /// <param name="query">The xPath query to build out</param>
        /// <param name="valueBuilder">The class to use for </param>
        /// <returns></returns>
        public Element CreateElementOrAttributeFromXPath(
            SifElement relativeTo,
            String query,
            IValueBuilder valueBuilder)
        {
            SifVersion version = Adk.SifVersion;
            SifFormatter pathFormatter = GetFormatter(version);
            SifFormatter textFormatter = Adk.TextFormatter;
            return CreateElementOrAttributeFromXPath(
                relativeTo, query, valueBuilder, version, textFormatter, pathFormatter);
        }


        /// <summary>
        /// Create all elements and attributes referenced by the XPath-like query string.
        /// </summary>
        /// <param name="relativeTo">The element that is the starting point of the path</param>
        /// <param name="query">The xPath query to build out</param>
        /// <param name="valueBuilder">The class to use for</param>
        /// <param name="version">The version of SIF for which this mapping operation is being evaluated</param>
        /// <param name="textFormatter">The SIFFormatter instance used to parse strings into strongly-typed data values.
        /// For many uses of this API, this formatter is equivalent to Adk.TextFormatter</param>
        /// <param name="pathFormatter">The SIFFormatter instance used for setting child SIFElements on their parents.
        /// This formatter may be different than the text formatter. The text formatter is, for
        /// compatibility's sake defaulted to SIF 1.x. However, the path formatter must be 
        /// correct for the mappings path being evaluated. </param>
        /// <returns></returns>
        public Element CreateElementOrAttributeFromXPath(
            SifElement relativeTo,
            String query,
            IValueBuilder valueBuilder,
            SifVersion version,
            SifFormatter textFormatter,
            SifFormatter pathFormatter)
        {
            int i = query.IndexOf('/');
            String currentSegment;
            String nextSegment = null;
            if (i == -1)
            {
                currentSegment = query;
            }
            else
            {
                currentSegment = query.Substring(0, i);
                nextSegment = query.Substring(i + 1);
            }
            Element result = _xpathBuild(relativeTo,
                                          new StringBuilder(),
                                          currentSegment,
                                          nextSegment,
                                          null,
                                          valueBuilder,
                                          version,
                                          textFormatter,
                                          pathFormatter);

            return result;
        }


        /// <summary>  Recursively parse an XPath-like query.</summary>
        /// <param name="relativeTo">The SifElement this iteration is relative to. For the
        /// first call to this method, the <i>relativeTo</i> parameter is usually
        /// a SifDataObject such as StudentPersonal to which the XPath query
        /// string is relative. With each subsequent call it is the SifElement
        /// or SimpleField that was previously processed.</param>
        /// <param name="curSegment">The current segment of the path that is being processed.
        /// For the first call to this method, the <i>curSegment</i> should be
        /// the portion of the XPath query string up to the first forward slash,
        /// exclusive.</param>
        /// <param name="nextSegment">The remaining portion of the path to be processed.
        /// For the first call to this method, the <i>nextSegment</i> should be
        /// the portion of the XPath query string following the first forward
        /// slash.</param>
        /// <returns> The Element satisfying the query, or <c>null</c> if no
        /// match was found (unless the <i>create</i> parameter is true). If the
        /// query resolves to an attribute, a SimpleField object is returned. If
        /// it resolves to an element, a SifElement object is returned. In both
        /// cases the caller can obtain the text value of the attribute or
        /// element by calling its <c>getTextValue</c> method.
        /// </returns>
        private Element _xpath(SifElement relativeTo, string curSegment, string nextSegment)
        {
            SifElement nextEle = null;

            int attr = curSegment.IndexOf('@');
            int bracket = curSegment.IndexOf('[');
            if (bracket != -1)
            {
                if (attr == -1)
                {
                    throw new AdkSchemaException("Invalid query: \"" + curSegment +
                                                  "\" must be in the form [@Attribute='value']");
                }

                string subEleTag = curSegment.Substring(0, (bracket) - (0));
                SifElement subEle = relativeTo.GetChild(subEleTag);
                if (subEle == null)
                {
                    return null;
                }

                int endBracket = curSegment.IndexOf(']', bracket);
                if (bracket == -1)
                {
                    throw new AdkSchemaException("Invalid query: \"" + curSegment +
                                                  "\" must be in the form [@Attribute='value']");
                }

                List<SearchCond> condsList = new List<SearchCond>(10);
                string conds = curSegment.Substring(bracket + 1, (endBracket) - (bracket + 1));
                string[] tokens = conds.Split(',');
                foreach (string thisTok in tokens)
                {
                    if (thisTok[0] != '@')
                    {
                        throw new AdkSchemaException("Attribute names must be preceded with the @ character: " +
                                                      thisTok);
                    }

                    int eq = thisTok.IndexOf('=');
                    if (eq == -1)
                    {
                        throw new AdkSchemaException("Attribute value must be in the form [@Attribute='value']: " +
                                                      thisTok);
                    }

                    //  Lookup the referenced attribute
                    SimpleField attrEle = subEle.GetField(thisTok.Substring(1, (eq) - (1)));
                    if (attrEle == null)
                    {
                        return null;
                    }

                    //  Add the attribute/value to the list
                    string aval = _attrValue(thisTok.Substring(eq + 1));
                    SearchCond sc = new SearchCond(attrEle.ElementDef, aval);
                    condsList.Add(sc);
                }

                //  Search the parent's subEleTag children for matching attributes.
                //  All attributes in the condsList must match.
                SifElementList ch = relativeTo.GetChildList(subEleTag);
                int chLen = ch.Count;
                for (int i = 0; i < chLen && nextEle == null; i++)
                {
                    SifElement cmpEle;
                    cmpEle = ch[i];

                    int matched = 0;

                    //  Compare the attributes
                    for (int x = 0; x < condsList.Count; x++)
                    {
                        SearchCond sc = condsList[x];
                        SimpleField atr = cmpEle.GetField(sc.fAttr);
                        if (atr == null)
                        {
                            break;
                        }
                        if (atr.TextValue.Equals(sc.fValue))
                        {
                            matched++;
                        }
                    }

                    //  If all attributes matched, this is a match
                    if (matched == condsList.Count)
                    {
                        nextEle = cmpEle;

                        //  Continue the search if nextSegment has a value
                        if (nextSegment != null && nextSegment.Length > 0)
                        {
                            int ii = nextSegment.IndexOf('/');

                            Element ee =
                                _xpath(nextEle, ii == -1 ? nextSegment : nextSegment.Substring(0, (ii) - (0)),
                                        ii == -1 ? null : nextSegment.Substring(ii + 1));
                            if (ee != null)
                            {
                                return ee;
                            }
                            else
                            {
                                nextEle = null;
                            }
                        }
                    }
                }

                if (nextEle == null)
                {
                    return null;
                }
            }
            else
            {
                //  Search for the named attribute/element
                if (attr != -1)
                {
                    return relativeTo.GetField(curSegment.Substring(1));
                }
                else
                {
                    nextEle = relativeTo.GetChild(curSegment);
                    if (nextEle == null)
                    {
                        if (nextSegment == null || (nextSegment.Length > 0 && nextSegment[0] == '@'))
                        {
                            return relativeTo.GetField(curSegment);
                        }

                        return null;
                    }
                }
            }

            //  Continue the search if nextSegment has a value
            if ((nextSegment != null && nextSegment.Length > 0))
            {
                int i = nextSegment.IndexOf('/');

                return
                    _xpath(nextEle, i == -1 ? nextSegment : nextSegment.Substring(0, (i) - (0)),
                            i == -1 ? null : nextSegment.Substring(i + 1));
            }

            return nextEle;
        }

        //
        //  Expand any escaped characters in an attribute value string and return
        //  the resulting string. If the value begins with a single or double quote,
        //  it must also end with a single or double quote; only the quoted text is
        //  returned.
        //
        private string _attrValue(string val)
        {
            if (val == null || val.Length == 0)
            {
                return val;
            }

            int st = 0, strlen = val.Length;
            if (val[strlen - 1] == '+')
            {
                strlen--;
            }
            int end = strlen - 1;

            if (val[0] == '\'' || val[0] == '"')
            {
                st++;
                end--;
                if (!(val[strlen - 1] == '\'' || val[strlen - 1] == '"'))
                {
                    throw new AdkSchemaException("Attribute value must be enclosed in quotes: " + val);
                }
            }

            return val.Substring(st, (end + 1) - (st));
        }


        /// <summary>  Recursively builds elements and attributes relative to a SifElement.</summary>
        /// <param name="relativeTo">The SifElement this iteration is relative to. For the
        /// first call to this method, the <i>relativeTo</i> parameter is usually
        /// a SifDataObject such as StudentPersonal to which the XPath query
        /// string is relative. With each subsequent call it is the SifElement
        /// or SimpleField that was previously processed.</param>
        /// <param name="path">A running path of the segments processed thus far; an empty
        /// StringBuffer should be passed to this parameter the first time the
        /// method is called </param>
        /// <param name="curSegment">The current segment of the path that is being processed.
        /// For the first call to this method, the <i>curSegment</i> should be
        /// the portion of the XPath query string up to the first forward slash,
        /// exclusive.</param>
        /// <param name="nextSegment">The remaining portion of the path to be processed.
        /// For the first call to this method, the <i>nextSegment</i> should be
        /// the portion of the XPath query string following the first forward
        /// slash.</param>
        /// <param name="prevAttributes">An optional array of attribute values that were
        /// used to construct an Element in the processing of the last segment.
        /// The array is comprised of attribute name and value pairs such that
        /// element N is an attribute name and N+1 is its value. For the first
        /// call to this method, the array should be null. For subsequent calls,
        /// it should be null unless an Element was constructed from attribute
        /// values.</param>
        /// <param name="version">The version of SIF for which this mapping operation is being evaluated</param>
        /// <param name="textFormatter">The SIFFormatter instance used to parse strings into strongly-typed data values.
        /// For many uses of this API, this formatter is equivalent to ADK.TextFormatter</param>
        /// <param name="pathFormatter">The SIFFormatter instance used for setting child SIFElements on their parents.
        /// This formatter may be different than the text formatter. The text formatter is, for
        /// compatibility's sake defaulted to SIF 1.x. However, the path formatter must be 
        /// correct for the mappings path being evaluated.</param>
        /// <param name="valueBuilder"></param>
        /// <returns> The Element satisfying the query, or <c>null</c> if no
        /// match was found (unless the <i>create</i> parameter is true). If the
        /// query resolves to an attribute, a SimpleField object is returned. If
        /// it resolves to an element, a SifElement object is returned. In both
        /// cases the caller can obtain the text value of the attribute or
        /// element by calling its <c>TextValue</c> Property.
        /// </returns>
        private Element _xpathBuild(
            SifElement relativeTo,
            StringBuilder path,
            string curSegment,
            string nextSegment,
            string[] prevAttributes,
            IValueBuilder valueBuilder,
            SifVersion version,
            SifFormatter textFormatter,
            SifFormatter pathFormatter)
        {
            string[] _prevAttributes = null;
            SifElement nextEle = null;

            int asgnEq = curSegment.LastIndexOf('=');
            int attr = curSegment.LastIndexOf('@', asgnEq == -1 ? curSegment.Length - 1 : asgnEq - 1);
            int bracket = curSegment.IndexOf('[');
            if (bracket != -1)
            {
                if (attr == -1)
                {
                    throw new AdkSchemaException("Invalid query: \"" + curSegment +
                                                  "\" must be in the form [@Attr='value1','value2',...]");
                }

                string subEleTag = curSegment.Substring(0, (bracket) - (0));

                int lastBracket = curSegment.LastIndexOf(']');
                string[] attrList = curSegment.Substring(bracket + 1, (lastBracket) - (bracket + 1)).Split(',');
                _prevAttributes = new string[attrList.Length * 2];
                int _prevI = 0;

                for (int a = 0; a < attrList.Length; a++)
                {
                    string _curSegment = attrList[a];

                    //  Determine the value of the attribute
                    int eq = _curSegment.IndexOf("=");
                    string val = _curSegment.Substring(eq + 1);
                    string v = null;
                    if (val[0] == '\'')
                    {
                        int end = val.IndexOf('\'', 1);
                        if (end != -1)
                        {
                            v = valueBuilder == null
                                    ? val.Substring(1, (end) - (1))
                                    : valueBuilder.Evaluate(val.Substring(1, (end) - (1)));
                        }
                    }

                    if (v == null)
                    {
                        throw new AdkSchemaException("Attribute value (" + val +
                                                      ") must be in the form @Attribute='value'");
                    }

                    string attrName = _curSegment.Substring(1, (eq) - (1));

                    _prevAttributes[_prevI++] = attrName;
                    _prevAttributes[_prevI++] = v;

                    if (nextEle == null)
                    {
                        //
                        //  Look at all of the peer elements to determine if any have the
                        //  attribute value set. If so, return it; otherwise create a new
                        //  instance of the element with the attribute set. For example, if
                        //  curSegment is "Address[@Type='M']", we must look at all of the
                        //  Address children of relativeTo in order to determine if any
                        //  currently exist with a Type field set to a value of 'M'. If one
                        //  does, then it already exists and there is nothing to do; if
                        //  not found, however, a new Address child must be added with a
                        //  Type field of 'M'.
                        //

                        //  Lookup the IElementDef of relativeTo
                        IElementDef subEleDef = Adk.Dtd.LookupElementDef(relativeTo.ElementDef, subEleTag);
                        if (subEleDef == null)
                        {
                            subEleDef = Adk.Dtd.LookupElementDef(subEleTag);
                            if (subEleDef == null)
                            {
                                throw new AdkSchemaException(subEleTag + " is not a recognized attribute of " +
                                                              relativeTo.Tag);
                            }
                        }

                        bool repeatable = subEleDef.IsRepeatable(relativeTo.SifVersion);

                        SifElementList peers = relativeTo.GetChildList(subEleDef);

                        if (curSegment.IndexOf("+]") == -1)
                        {
                            //
                            //  Determine if relatSifElementListiveTo has any children that already
                            //  define this attribute/value; if not, create a new instance.
                            //  If subEleDef is not repeatable, however, we cannot add
                            //  another instance of it regardless.
                            //
                            for (int i = 0; i < peers.Count && nextEle == null; i++)
                            {
                                SimpleField ftest = peers[i].GetField(attrName);
                                if (ftest != null && ftest.TextValue.Equals(v))
                                {
                                    nextEle = peers[i];
                                }
                            }
                        }

                        if (nextEle == null)
                        {
                            if (!(peers.Count > 0 && !repeatable))
                            {
                                nextEle =
                                    _createChild(relativeTo, subEleTag, valueBuilder, version, textFormatter,
                                                  pathFormatter);
                            }
                            else
                            {
                                //
                                //  subEleDef is not repeatable, so we need to back up
                                //  and add this attribute/value to a fresh instance of
                                //  relativeTo if possible. First use _xpath(path) to try
                                //  to select that instance in case it already exists (otherwise
                                //  we'd create a new instance each iteration, which is
                                //  not the desired result.)
                                //

                                string _tmp;
                                if (path.Length > 0)
                                {
                                    _tmp = path + "/" + curSegment;
                                }
                                else
                                {
                                    _tmp = curSegment;
                                }
                                if (XPATH_DEBUG)
                                {
                                    Console.Out.Write("Searching for path relative to " +
                                                       relativeTo.Root.ElementDef.Name + ": " + _tmp);
                                }

                                int _del = _tmp.IndexOf('/');
                                nextEle =
                                    (SifElement)
                                    _xpath((SifElement)relativeTo.Root,
                                            _tmp.Substring(0, (_del == -1 ? _tmp.Length : _del) - (0)),
                                            _del == -1 ? null : _tmp.Substring(_del + 1));

                                if (XPATH_DEBUG)
                                {
                                    if (nextEle == null)
                                    {
                                        Console.Out.WriteLine("; not found, a new instance will be created");
                                    }
                                    else
                                    {
                                        Console.Out.WriteLine("; found");
                                    }
                                }

                                if (nextEle == null)
                                {
                                    if (relativeTo.ElementDef.IsRepeatable(relativeTo.SifVersion))
                                    {
                                        //  Clone relativeTo
                                        SifElement grandParent = (SifElement)relativeTo.Parent;
                                        nextEle = SifElement.Create(grandParent, relativeTo.ElementDef);
                                        pathFormatter.AddChild(grandParent, nextEle, version);

                                        //  Clone subEleDef; this now becomes nextEle
                                        SifElement newEle = SifElement.Create(nextEle, subEleDef);
                                        pathFormatter.AddChild(nextEle, newEle, version);
                                        _copyAttributes(nextEle, prevAttributes);
                                        nextEle = newEle;
                                    }
                                    else
                                    {
                                        throw new AdkSchemaException(
                                            "It is not possible to create the element or attribute identified by this path: " +
                                            _tmp + (nextSegment == null ? "" : "/" + nextSegment) +
                                            ". The element or attribute is either undefined in this version of SIF, " +
                                            "or an attempt is being made to create another instance of an element that is not Repeatable.");
                                    }
                                }
                            }
                        }
                    }

                    if (nextEle != null)
                    {
                        _createField(nextEle, attrName, v);
                    }

                    if (a == attrList.Length && nextEle == null)
                    {
                        return null;
                    }
                }
            }
            else
            {
                //  Search for the named attribute/element
                if (attr != -1)
                {
                    SimpleField ff = relativeTo.GetField(curSegment.Substring(1));
                    if (ff == null)
                    {
                        ff = _createField(relativeTo, curSegment.Substring(1), null);
                    }

                    return ff;
                }
                else
                {
                    string _tag = curSegment;
                    int eq = curSegment.IndexOf('=');
                    if (eq != -1)
                    {
                        _tag = curSegment.Substring(0, (eq) - (0));
                    }

                    nextEle = relativeTo.GetChild(_tag);
                    if (nextEle == null)
                    {
                        //  The curSegment element does not exist as a child of the relativeTo
                        //  object, so create it.
                        nextEle =
                            _createChild(relativeTo, curSegment, valueBuilder, version, textFormatter, pathFormatter);
                        if (nextEle == null)
                        {
                            if (nextSegment == null)
                            {
                                return relativeTo.GetField(_tag);
                            }
                            return null;
                        }
                    }
                }
            }

            //  Continue the search if nextSegment has a value
            if (nextEle != null && (nextSegment != null && nextSegment.Length > 0))
            {
                int i = nextSegment.IndexOf('/');

                if (path.Length > 0)
                {
                    path.Append("/");
                }
                path.Append(curSegment);

                return _xpathBuild(
                    nextEle,
                    path, i == -1 ? nextSegment : nextSegment.Substring(0, (i)),
                    i == -1 ? null : nextSegment.Substring(i + 1),
                    _prevAttributes,
                    valueBuilder,
                    version,
                    textFormatter,
                    pathFormatter);
            }

            if (nextSegment == null && nextEle != null && (nextEle.TextValue == null || nextEle.TextValue.Length == 0))
            {
                int eq2 = curSegment.LastIndexOf('=');
                if (eq2 != -1)
                {
                    if (bracket == -1 || (curSegment.LastIndexOf(']') < eq2))
                    {
                        //
                        //  An equals sign in the final segment indicates there is
                        //  a value constant or expression following the XPath
                        //  (e.g. "OtherId[@Type='06']=@pad($(PERMNUM),0,5)" ). Use
                        //  the user-supplied ValueBuilder to evaluate it.
                        //
                        string str = curSegment.Substring(eq2 + 1);
                        nextEle.TextValue = valueBuilder == null ? str : valueBuilder.Evaluate(str);
                    }
                }
            }

            return nextEle;
        }

        /// <summary>  Assigns a list of attribute values to a destination SifElement.</summary>
        /// <param name="dst">The SifElement to copy the attributes to
        /// </param>
        /// <param name="attributes">An array where element N is an attribute name and
        /// element N+1 is the attribute value
        /// </param>
        private void _copyAttributes(SifElement dst, string[] attributes)
        {
            for (int i = 0; i < attributes.Length; i++)
            {
                IElementDef attrDef = Adk.Dtd.LookupElementDef(dst.ElementDef + "_" + attributes[i]);
                dst.SetField(attrDef, attributes[++i]);
            }
        }


        /// <summary>
        /// Creates a child element and sets the text value
        /// </summary>
        /// <param name="relativeTo">The parent SIFElement to add the new element to</param>
        /// <param name="tag">The tag name of the element</param>
        /// <param name="valueBuilder">The ValueBuilder instance to use to evaluate macros</param>
        /// <param name="version">The version of SIF for which this mapping operation is being evaluated</param>
        /// <param name="textFormatter">The SIFFormatter instance used to parse strings into strongly-typed data values.
        /// For many uses of this API, this formatter is equivalent to Adk.TextFormatter</param>
        /// <param name="pathFormatter">The SIFFormatter instance used for setting child SIFElements on their parents.
        /// This formatter may be different than the text formatter. The text formatter is, for
        /// compatibility's sake defaulted to SIF 1.x. However, the path formatter must be 
        /// correct for the mappings path being evaluated.</param>
        /// <returns></returns>
        private SifElement _createChild(
            SifElement relativeTo,
            String tag,
            IValueBuilder valueBuilder,
            SifVersion version,
            SifFormatter textFormatter,
            SifFormatter pathFormatter)
        {
            string _tag = tag;
            string assignValue = null;
            int eq = tag.IndexOf('=');
            if (eq != -1)
            {
                _tag = tag.Substring(0, (eq) - (0));
                string str = tag.Substring(eq + 1);
                assignValue = valueBuilder == null ? str : valueBuilder.Evaluate(str);
            }

            //  Lookup the IElementDef
            IElementDef def = Adk.Dtd.LookupElementDef(relativeTo.ElementDef, _tag);
            if (def == null)
            {
                def = Adk.Dtd.LookupElementDef(_tag);
            }
            if (def == null)
            {
                throw new AdkSchemaException(_tag + " is not a recognized element or attribute of " + relativeTo.Tag);
            }

            try
            {
                TypeConverter defConverter = def.TypeConverter;
                if (defConverter == null)
                {
                    defConverter = SifTypeConverters.STRING;
                }
                if (def.Field)
                {
                    SimpleField field = defConverter.ParseField(relativeTo, def, textFormatter, assignValue);
                    relativeTo.SetField(field);
                }
                else
                {
                    //  Create element instance

                    SifElement ele = (SifElement)ClassFactory.CreateInstance(def.FQClassName);
                    ele.ElementDef = def;
                    pathFormatter.AddChild(relativeTo, ele, version);
                    if (assignValue != null)
                    {
                        // TODO: THis needs to be done using the type converter
                        ele.TextValue = assignValue;
                    }

                    return ele;
                }
            }
            catch (TypeLoadException tle)
            {
                throw new SystemException(
                    "The " + def.Package +
                    " Sdo module is not loaded (ensure the Adk is initialized to load this module)", tle);
            }
            catch (Exception thr)
            {
                throw new SystemException(
                    "Failed to create an instance of the " + def.ClassName + " class from the " + def.Package +
                    " Sdo module: ", thr);
            }

            return null;
        }

        /// <summary>  Set a field value on a SifElement</summary>
        /// <param name="parent">The SifElement on which to set the field
        /// </param>
        /// <param name="attr">The name of the attribute or simple child element
        /// </param>
        /// <param name="val">The text value to assign to the attribute or simple child element
        /// </param>
        private SimpleField _createField(SifElement parent, string attr, string val)
        {
            //  Lookup the IElementDef relative to the parent
            IElementDef def = Adk.Dtd.LookupElementDef(parent.ElementDef, attr);
            if (def == null)
            {
                throw new AdkSchemaException(attr + " is not a recognized attribute of " + parent.Tag);
            }
            if (!def.Field)
            {
                throw new AdkSchemaException("Query references a complex element ('" + attr +
                                              "') where an attribute or simple field was expected");
            }

            //  Set the field value on the parent
            return parent.SetField(def, val);
        }

        /// <summary>  Creates an instance of a SifDataObject given an IElementDef.
        /// 
        /// </summary>
        /// <param name="objType">An IElementDef constant from the SifDtd class that identifies
        /// a top-level SIF Data Object such as SifDtd.STUDENTPERSONAL, SifDtd.BUSINFO, etc.
        /// </param>
        /// <returns> A new instance of the corresponding SifDataObject class
        /// (e.g. OpenADK.Library.us.Student.StudentPersonal)
        /// 
        /// </returns>
        /// <exception cref="AdkSchemaException"> AdkSchemaException thrown if the <i>objType</i> parameter does
        /// not identify a top-level SIF Data Object, or the Adk was not
        /// initialized to load the Sdo module in which the specified object
        /// type is defined.
        /// </exception>
        public SifDataObject CreateSIFDataObject(IElementDef objType)
        {
            if (objType == null)
            {
                throw new AdkSchemaException(
                    "The Adk was not initialized to load the requested Sdo module");
            }
            if (!objType.Object)
            {
                throw new AdkSchemaException("SifDtd." + objType.Name.ToUpper() + " is not a top-level SIF Data Object");
            }

            //  Create element instance
            return (SifDataObject)ClassFactory.CreateInstance(objType.FQClassName);
        }

        private class SearchCond
        {
            internal IElementDef fAttr;
            internal string fValue;

            public SearchCond(IElementDef attr, string val)
            {
                fAttr = attr;
                fValue = val;
            }
        }

        public SifFormatter GetFormatter(SifVersion version)
        {
            if (version.Major == 2)
            {
                return SIF_2X_FORMATTER;
            }
            else if (version.Major == 1)
            {
                return SIF_1X_FORMATTER;
            }
            else
            {
                throw new AdkException("Formatter not defined for SIFVersion: " + version.ToString(), null);
            }
        }

        internal static SifFormatter SIF_1X_FORMATTER = new Sif1xFormatter();
        internal static SifFormatter SIF_2X_FORMATTER = new Sif2xFormatter();
    }

}
