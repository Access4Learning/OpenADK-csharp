//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System;
using System.Xml;
using OpenADK.Library.Tools.Cfg;

namespace OpenADK.Library.Tools.Mapping
{
    /// <summary>	Encapsulates optional filtering attributes for field mapping rules.
    /// 
    /// A field mapping rule can define any of the following filters:
    /// 
    /// <ul>
    /// <li>
    /// <b>SIF Version</b>. The rule will only be applied if the 
    /// SifVersion instance passed to the <code>Mappings.map</code> 
    /// method matches this value. The version filter is a string
    /// comprised of a comparison operator ("=" for equals, "-"
    /// for Less Than or Equal To, or "+" for Greater Than or
    /// Equal To) followed by a SIF Version identifier. For example, 
    /// "=1.5" matches SIF 1.5; "+1.1" matches all versions of SIF 
    /// equal to and greater than 1.1; and "-1.1" matches all versions
    /// of SIF less than or equal to SIF 1.1. Filter on SIF Version 
    /// to map values to version-specific elements or attributes 
    /// of SIF Data Objects.
    /// </li>
    /// <li>
    /// <b>Direction</b>. The rule will only be applied if the
    /// Direction flag (<code>Mappings.DIRECTION_INBOUND</code>
    /// or <code>Mappings.DIRECTION_OUTBOUND</code>) passed to 
    /// the <code>Mappings.map</code> method matches this value.
    /// Filter on Direction to create field mapping rules that
    /// differ for inbound messages like SIF_Request than for
    /// outbound messages like SIF_Event and SIF_Response.
    /// </li>
    /// </ul>
    /// </summary>
    public class MappingsFilter
    {
        /// <summary>Gets or Sets the SIF Version filter.</summary>
        /// <value> A SIF Version string prefixed by a comparision 
        /// operator "=", "&lt;" or "&gt";. For example, specify "=1.5"
        /// to match SIF 1.5; ">1.1" to match all versions of SIF equal
        /// to or greater than 1.1, etc. If no comparision operator 
        /// is specified Equal To ("=") is assumed.
        /// </value>
        public string SifVersion
        {
            get { return fVersion; }

            set
            {
                if (value == null)
                {
                    fVersion = null;
                }
                else if (!value.StartsWith("=") && !value.StartsWith("+") &&
                         !value.StartsWith("-"))
                {
                    fVersion = "=" + value;
                }
                else
                {
                    fVersion = value;
                }
            }
        }

        /// <summary>Gets or Sets the Message Direction filter.
        /// </summary>
        public MappingDirection Direction
        {
            get { return fDirection; }

            set { fDirection = value; }
        }

        /// <summary> 	SifVersion filter</summary>
        protected internal string fVersion = null;

        /// <summary> 	Direction filter</summary>
        protected internal MappingDirection fDirection = MappingDirection.Unspecified;

        /// <summary> 	Determines if the SIF Version filter is specified</summary>
        public bool HasVersionFilter
        {
            get { return fVersion != null; }
        }

        /// <summary> 	Determines if the Message Direction filter is specified</summary>
        public bool HasDirectionFilter
        {
            get { return fDirection != MappingDirection.Unspecified; }
        }

        /// <summary> 	Evaluates the Message Direction filter against a direction flag.</summary>
        /// <param name="flag">Any <code>Mappings.DIRECTION_</code> constant
        /// </param>
        /// <returns> true If the message direction filter is undefined or 
        /// matches the specified flag <code>flag</code>
        /// </returns>
        public bool EvalDirection(MappingDirection flag)
        {
            return fDirection == MappingDirection.Unspecified || fDirection == flag;
        }

        /// <summary> 	Evaluates the SIF Version filter against a SifVersion instance.</summary>
        /// <param name="version">A SifVersion instance
        /// </param>
        /// <returns> true If the SIF Version filter is undefined, or if the
        /// <code>version</code> parameter evaluates true given the 
        /// comparision operator and version of SIF specified by the
        /// current filter
        /// </returns>
        public bool EvalVersion(SifVersion version)
        {
            if (fVersion == null || version == null)
            {
                return true;
            }

            SifVersion flt = Library.SifVersion.Parse(fVersion.Substring(1));
            if (flt != null)
            {
                if (fVersion[0] == '=')
                {
                    return flt.CompareTo(version) == 0;
                } // Prefix of = means SIF_Version == filter version
                if (fVersion[0] == '+')
                {
                    return version.CompareTo(flt) >= 0;
                } // Prefix of + means filter applies to messages with SIF_Version 
                if (fVersion[0] == '-')
                {
                    return version.CompareTo(flt) <= 0;
                } // Prefix of - means SIF_Version <= filter version
            }

            return false;
        }


        /// <summary>
        /// Persists this mapping filter to the specified XmlElement
        /// </summary>
        /// <param name="filter">The filter to save</param>
        /// <param name="element"></param>
        internal static void Save(MappingsFilter filter,
                                  XmlElement element)
        {
            if (filter != null && filter.HasVersionFilter)
            {
                element.SetAttribute("sifVersion", filter.SifVersion);
            }
            else
            {
                element.RemoveAttribute("sifVersion");
            }

            if (filter != null && filter.HasDirectionFilter)
            {
                element.SetAttribute("direction", filter.Direction.ToString("G"));
                switch (filter.Direction)
                {
                    case MappingDirection.Inbound:
                        element.SetAttribute("direction", "inbound");
                        break;

                    case MappingDirection.Outbound:
                        element.SetAttribute("direction", "outbound");
                        break;
                }
            }
            else
            {
                element.RemoveAttribute("direction");
            }
        }

        /// <summary>
        /// Creates an returns a mappings filter from the specified XmlElement, or null if there is no filter defined
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal static MappingsFilter Load(XmlElement node)
        {
            string filtVer = node.GetAttribute("sifVersion");
            string filtDir = node.GetAttribute("direction");

            if (filtVer.Length > 0 || filtDir.Length > 0)
            {
                MappingsFilter filt = new MappingsFilter();

                if (filtVer.Length > 0)
                {
                    filt.SifVersion = filtVer;
                }

                if (filtDir.Length > 0)
                {
                    try
                    {
                        filt.Direction =
                            (MappingDirection)
                            Enum.Parse(typeof (MappingDirection), filtDir, true);
                    }
                    catch (Exception ex)
                    {
                        throw new AdkConfigException
                            ("Field mapping rule specifies an unknown Direction flag: '" + filtDir +
                             "'", ex);
                    }
                }
                return filt;
            }
            else
            {
                return null;
            }
        }
    }
}
