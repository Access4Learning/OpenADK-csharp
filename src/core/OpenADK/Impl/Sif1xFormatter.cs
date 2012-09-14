//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using OpenADK.Util;

namespace OpenADK.Library.Impl
{
    public class Sif1xFormatter : SifFormatter
    {
        private const string SIF1XTIMEFORMAT = "HH:mm:ss";
        private const string SIF1XDATEFORMAT = "yyyyMMdd";


        public static string FormatTimeZone(DateTime date)
        {
            TimeSpan difference = TimeZone.CurrentTimeZone.GetUtcOffset(date);
            return
                String.Format
                    ("UTC{0:00}:{1:00}", new object[] {difference.Hours, difference.Minutes});
        }

        public override string ToDateString(DateTime? date)
        {
            if (!date.HasValue)
            {
                return String.Empty;
            }
            return date.Value.ToString(SIF1XDATEFORMAT);
        }

        /// <summary>
        /// Returns a string representation of the specified date. Since SIF 1.5 does
        /// not support the datetime datatype, this field formats the date as a SIF
        /// Date ("yyyyMMdd")
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public override string ToDateTimeString(DateTime? date)
        {
            return ToDateString( date );
        }

        public override string ToTimeString(DateTime? time)
        {
            if (!time.HasValue)
            {
                return String.Empty;
            }
            return time.Value.ToString(SIF1XTIMEFORMAT);
        }

        public override string ToString(int? intValue)
        {
            if (!intValue.HasValue)
            {
                return String.Empty;
            }
            return Convert.ToString(intValue.Value);
        }

        public override string ToString(long? longValue)
        {
            if (!longValue.HasValue)
            {
                return String.Empty;
            }
            return Convert.ToString(longValue.Value);
        }

        public override string ToString(decimal? decimalValue)
        {
            if (!decimalValue.HasValue)
            {
                return "";
            }
            return Convert.ToString(decimalValue.Value);
        }

        public override string ToString(bool? boolValue)
        {
            if (!boolValue.HasValue)
            {
                return String.Empty;
            }
            if (boolValue.Value)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        public override DateTime? ToDate(string dateValue)
        {
            if (dateValue == null)
            {
                return null;
            }
            dateValue = dateValue.Trim();
            if (dateValue.Length == 0)
            {
                return null;
            }
            return DateTime.ParseExact
                (dateValue, SIF1XDATEFORMAT, null, DateTimeStyles.AllowWhiteSpaces);
        }

        public override DateTime? ToDateTime(string xmlValue)
        {
            return ToDate( xmlValue );
        }

        public override DateTime? ToTime(string xmlValue)
        {
            if (xmlValue == null)
            {
                return null;
            }
            xmlValue = xmlValue.Trim();
            if( xmlValue.Length == 0 )
            {
                return null;
            }

            return DateTime.ParseExact
                (xmlValue, SIF1XTIMEFORMAT, null, DateTimeStyles.AllowWhiteSpaces);
        }

        public override int? ToInt(string intValue)
        {
            if (intValue == null)
            {
                return null;
            }
            intValue = intValue.Trim();
            if (intValue.Length == 0)
            {
                return null;
            }
            return int.Parse(intValue);
        }


        /// <summary>
        /// Converts a SIF XML integer value to a  .Net <code>long?</code> value
        /// </summary>
        public override long? ToLong(string longValue)
        {
            if (longValue == null)
            {
                return null;
            }
            longValue = longValue.Trim();
            if (longValue.Length == 0)
            {
                return null;
            }
            return long.Parse(longValue);
        }

        public override decimal? ToDecimal(string decimalValue)
        {
            if (decimalValue == null)
            {
                return null;
            }
            decimalValue = decimalValue.Trim();
            if (decimalValue.Length == 0)
            {
                return null;
            }
            // SIF 1.5 allows some values to contain a percentage symbol. These can
            // be converted to decimal values after removing the percentage sign
            if (decimalValue.EndsWith("%"))
            {
                decimalValue = decimalValue.Substring(0, decimalValue.Length - 1);
            }
            return Decimal.Parse(decimalValue);
        }

        public override bool? ToBool(string inValue)
        {
            if (inValue == null)
            {
                return null;
            }
            inValue = inValue.Trim();
            if (inValue.Length == 0)
            {
                return null;
            }
            if (String.Compare(inValue, "yes", true) == 0)
            {
                return true;
            }
            else if (String.Compare(inValue, "no", true) == 0)
            {
                return false;
            }
            return Boolean.Parse(inValue);
        }

        public override bool SupportsNamespaces
        {
            get { return false; }
        }

        /// <summary>
        /// Converts a <code>TimeSpan?</code> value to a String XML representation as an XML duration
        /// </summary>
        /// <param name="duration">A nullable TimeSpan to convert</param>
        /// <returns></returns>
        public override string ToString( TimeSpan? duration )
        {
            throw new NotImplementedException( "XML Durations are not supported in SIF 1.x" );
        }

        /// <summary>
        /// Converts a SIF XML duration value to a .NET <code>TimeSpan?</code> valueS
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override TimeSpan? ToTimeSpan( string value )
        {
            throw new NotImplementedException( "XML Durations are not supported in SIF 1.x" );
        }


        /// <summary>
        /// Gets the content from the SIFElement for the specified version of SIF. Only
        /// elements that apply to the requested version of SIF will be returned.
        /// </summary>
        /// <param name="element">The element to retrieve content from</param>
        /// <param name="version"></param>
        /// <returns></returns>
        public override IList<Element> GetContent(SifElement element, SifVersion version)
        {
            List<Element> returnValue = new List<Element>();
            ICollection<SimpleField> fields = element.GetFields();
            foreach (SimpleField val in fields)
            {
                IElementDef def = val.ElementDef;
                if (def.IsSupported(version) &&
                     !def.IsAttribute(version) &&
                     def.Field )
                {
                    returnValue.Add(val);
                }
            }

            IList<SifElement> children = element.GetChildList();
            foreach (SifElement val in children)
            {
                IElementDef def = val.ElementDef;
                if (def.IsSupported(version))
                {
                    if (def.IsCollapsed(version))
                    {
                        ICollection<Element> subElements = GetContent(val, version);
                        // FIXUP the ElementDef for this version of SIF.
                        // for example, StudentPersonal/EmailList/Email needs it's
                        // ElementDef set to "StudentPersonal_Email"
                        foreach (Element e in subElements)
                        {
                            IElementDef subElementDef = e.ElementDef;
                            if (version.CompareTo(subElementDef.EarliestVersion) >= 0)
                            {
                                String tag = subElementDef.Tag(Adk.SifVersion);
                                IElementDef restoredDef = Adk.Dtd.LookupElementDef(element.ElementDef, tag);
                                if (restoredDef != null)
                                {
                                    e.ElementDef = restoredDef;
                                }
                                returnValue.Add(e);
                            }
                        }
                    }
                    else
                    {
                        returnValue.Add(val);
                    }
                }
            }
            MergeSort.Sort<Element>(returnValue, ElementSorter<Element>.GetInstance(version));
            //returnValue.Sort(ElementSorter<Element>.GetInstance(version));
            return returnValue;
        }


                /// <summary>
        /// Adds a SimpleField parsed from a specific version of SIF to the parent.
        /// </summary>
        /// <param name="contentParent">The element to add content to</param>
        /// <param name="fieldDef">The metadata definition of the field to set</param>
        /// <param name="data">The value to set to the field</param>
        /// <param name="version">The version of SIF that the SIFElement is being constructed
        /// from</param>
        /// <returns></returns>
        public override SimpleField SetField(SifElement contentParent,
            IElementDef fieldDef,
            SifSimpleType data,
            SifVersion version)
                {
                    return GetContainer(contentParent, fieldDef, version).SetField(fieldDef, data);
                }

        private SifElement GetContainer(SifElement contentParent, IElementDef childDef, SifVersion version)
        {

            IElementDef elementParentDef = childDef.Parent;
            if (elementParentDef != null && elementParentDef != contentParent.ElementDef)
            {
                // The element does not appear to belong to this parent. Attempt to look
                // for a container element that might be missing in between the two
                // If the parent of this element were collapsed in a previous version
                // of SIF, check for or re-add the parent element and add this new
                // child instead.
                //
                // For example, a child could be an Email element from the
                // common package that's being added to StudentPersonal. In this
                // case,
                // we need to actually find or create an instance of the new
                // EmailList
                // container element and add the child to it, instead of to "this"
                String tag = elementParentDef.Tag(Adk.SifVersion);
                IElementDef missingLink = Adk.Dtd.LookupElementDef(contentParent.ElementDef, tag);
                if (missingLink != null && missingLink.IsCollapsed(version))
                {
                    SifElement container = contentParent.GetChild(missingLink);
                    if (container == null)
                    {
                            container = SifElement.Create(contentParent, missingLink);
                    }
                    AddChild(contentParent, container, version);
                    return container;
                }
            }

            return contentParent;
        }

        /// <summary>
        /// Adds a SIFElement parsed from a specific version of SIF to the parent.
        /// The formatter instance may use version-specific rules to ensure that the
        /// hierarchy is properly maintained when the source of the content is from
        /// this version of SIF
        /// </summary>
        /// <param name="contentParent"> The element to add content to</param>
        /// <param name="content">The element to add</param>
        /// <param name="version"> The version of SIF that the SIFElement is being constructed
        /// from</param>
        public override SifElement AddChild(
            SifElement contentParent, 
            SifElement content,
            SifVersion version)
        {
            contentParent.RestoreImplementationDef(content);
            return GetContainer(contentParent, content.ElementDef, version).AddChild(content); 

        }

    }
}
