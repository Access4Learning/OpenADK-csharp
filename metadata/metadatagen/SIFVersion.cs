////////////////////////////////////////////////////////////////////////////////
//
//  Copyright (c)2011 Pearson Education, Inc., or associates.
//  All rights reserved.
//
//  This software is the confidential and proprietary information of
//  Data Solutions ("Confidential Information").  You shall not disclose
//  such Confidential Information and shall use it only in accordance with the
//  terms of the llicense agreement you entered into with Data Solutions..
//
using System;
using System.Collections.Generic;

namespace Edustructures.SifWorks
{
    /// <summary>	Encapsulates a SIF version number.
    /// 
    /// The Adk uses instances of SifVersion rather than strings to identify
    /// versions of SIF. Typically you do not need to obtain a SifVersion instance
    /// directly except for when initializing the class framework with the
    /// <c>Adk.initialize</c> method. Rather, classes for which SIF version is
    /// a property, such as SifDataObject and Query, provide a <c>getSIFVersion</c>
    /// method to obtain the version associated with an object.
    /// </summary>
    /// <author>  Eric Petersen
    /// </author>
    /// <version>  1.0
    /// </version>
    [Serializable]
    public class SifVersion : IComparable
    {
        /// <summary>  Gets the major version number</summary>
        public virtual int Major
        {
            get { return fVersion.Major; }
        }

        /// <summary>  Gets the minor version number</summary>
        public virtual int Minor
        {
            get { return fVersion.Minor; }
        }

        /// <summary>  Gets the revision number</summary>
        public virtual int Revision
        {
            get { return fVersion.Revision; }
        }

        /// <summary>  Get the SIF namespace for this version of the specification.
        /// 
        /// </summary>
        /// <returns> If the SifVersion is less than SIF 1.1, a namespace in the form
        /// "http://www.sifinfo.org/v1.0r2/messages" is returned, where the full
        /// SIF Version number is included in the namespace. For SIF 1.x and
        /// later, a namespace in the form "http://www.sifinfo.org/infrastructure/1.x"
        /// is returned, where only the major version number is included in the
        /// namespace.
        /// </returns>
        public virtual string Xmlns
        {
            get
            {
                if (CompareTo(SIF11) < 0)
                {
                    return "http://www.sifinfo.org/v" + this.ToString() + "/messages";
                }

                return "http://specification.sifinfo.org" + "/" + fVersion.Major + ".x";
            }
        }


        /// <summary>Identifies the SIF 1.1 Specification </summary>
        public static readonly SifVersion SIF11 = new SifVersion(1, 1, 0);

        /// <summary>Identifies the SIF 1.5r1 Specification</summary>
        public static readonly SifVersion SIF15r1 = new SifVersion(1, 5, 1);

        /// <summary>Identifies the SIF 2.0 Specification</summary>
        public static readonly SifVersion SIF20 = new SifVersion(2, 0, 0);

        /// <summary>Identifies the SIF 2.0r1 Specification</summary>
        public static readonly SifVersion SIF20r1 = new SifVersion(2, 0, 1);

        /// <summary>Identifies the SIF 2.1 Specification</summary>
        public static readonly SifVersion SIF21 = new SifVersion(2, 1, 0);

        /// <summary>Identifies the SIF 2.2 Specification</summary>
        public static readonly SifVersion SIF22 = new SifVersion(2, 2, 0);



        //// WARNING: MAKE SURE TO UPDATE THE GETINSTANCE METHOD WHEN ADDING NEW VERSIONS ////
        /// <summary>Identifies the latest SIF Specification supported by the SIFWorks Adk </summary>
        public static readonly SifVersion LATEST = SIF21;

        /// <summary>
        /// Returns the earliest SIFVersion supported by the ADK for the major version
        /// number of SIF specified. For example, passing the value 1 returns <c>SifVersion.SIF11</c>
        /// </summary>
        /// <param name="major"></param>
        /// <returns>The latest version of SIF that the ADK supports for the specified
        /// major version</returns>
        public static SifVersion GetEarliest(int major)
        {
            switch (major)
            {
                case 1:
                    return SifVersion.SIF11;
                case 2:
                    return SifVersion.SIF20r1;
            }
            return null;
        }


        /// <summary>	Constructs a version object
        /// 
        /// </summary>
        /// <param name="major">The major version number
        /// </param>
        /// <param name="minor">The minor version number
        /// </param>
        /// <param name="revision">The revision number
        /// </param>
        private SifVersion(int major,
                            int minor,
                            int revision)
        {
            fVersion = new Version(major, minor, 0, revision);
        }

        /// <summary>  Gets a SifVersion instance</summary>
        /// <remarks>
        /// <para>
        /// This method always returns the same version instance for the given version
        /// numbers. If the version number match on official version supported by the ADK,
        /// that version instance is returned. Otherwise, a new SIFVersion instance is
        /// created and returned. The sam SifVersion instance will always be returned for 
        /// the same paramters
        /// </para>
        /// </remarks>
        /// <returns> A SifVersion instance to encapsulate the version information
        /// provided to this method. If the <i>major</i>, <i>minor</i>, and
        /// <i>revision</i> numbers match one of the versions supported by the
        /// Adk (e.g. SifVersion.SIF10r1, SifVersion.SIF10r2, etc.), that object
        /// is returned. Otherwise, a new instance is created. Thus, you are
        /// guaranteed to always receive the same SifVersion instance or a given
        /// version number supported by the Adk.
        /// </returns>
        private static SifVersion GetInstance(int major,
                                               int minor,
                                               int revision)
        {
            // Check for versions explicitly supported by the ADK first
            if (major == 2)
            {
                if (minor == 0)
                {
                    if (revision == 0)
                    {
                        return SIF20;
                    }
                    else if (revision == 1)
                    {
                        return SIF20r1;
                    }
                }
                if (minor == 1 && revision == 0)
                {
                    return SIF21;
                }

            }
            else if (major == 1)
            {
                if (minor == 5 && revision == 1)
                {
                    return SIF15r1;
                }
                else if (minor == 1 && revision == 0)
                {
                    return SIF11;
                }
            }

            // No explicit support found. Return a newly-fabricated instance
            // to support this version of SIF
            String tag = ToString(major, minor, revision, '.');

            SifVersion ver;
            if (sVersions == null)
            {
                sVersions = new Dictionary<string, SifVersion>();
            }
            if (!sVersions.TryGetValue(tag, out ver))
            {
                ver = new SifVersion(major, minor, revision);
                sVersions[tag] = ver;
            }
            return ver;
        }


        /// <summary>  Parse a <c>SifVersion</c> from a string</summary>
        /// <param name="versionStr">A version string in the format "1.0r1"</param>
        /// <returns> A SifVersion instance encapsulating the version string</returns>
        /// <exception cref="ArgumentException">is thrown if the version string is invalid</exception>
        public static SifVersion Parse(string versionStr)
        {
            if (versionStr == null)
            {
                throw new ArgumentNullException("Version to parse cannot be null", "versionStr");
            }
            try
            {
                string v = versionStr.ToLower();

                int i = v.IndexOf('.');
                int major = Int32.Parse(v.Substring(0, i));
                int minor;
                int revision = 0;

                int r = v.IndexOf('r');
                if (r == -1)
                {
                    String minorStr = v.Substring(i + 1);
                    if (minorStr.Equals("*"))
                    {
                        // hack
                        //return Adk.SifVersion;
                        return SifVersion.SIF20r1;


                        // This a 1.* or 2.* version. Return the latest version supported
                        //return getLatest( major );
                        //					if( twoDotStar.compareTo( ADK.getSIFVersion() ) > 0 ){
                        //						twoDotStar = ADK.getSIFVersion();
                        //					}
                        //					return twoDotStar;
                    }
                    else
                    {
                        minor = Int32.Parse(minorStr);
                    }
                }
                else
                {
                    minor = Int32.Parse(v.Substring(i + 1, r - i - 1));
                    revision = Int32.Parse(v.Substring(r + 1));
                }

                return GetInstance(major, minor, revision);
            }
            catch (Exception thr)
            {
                throw new ArgumentException(versionStr + " is an invalid version string", thr);
            }
        }


        /// <summary>
        /// Returns the latest SIFVersion supported by the ADK for the major version
        /// number of SIF specified. For example, passing the value 1 returns 
        /// <c>SifVersion.15r1</c>
        /// </summary>
        /// <param name="major"></param>
        /// <returns></returns>
        public static SifVersion GetLatest(int major)
        {
            switch (major)
            {
                case 1:
                    return SIF15r1;
                case 2:
                    return LATEST;
            }
            return null;
        }


        /// <summary>  Parse a <c>SifVersion</c> from a <i>xmlns</i> attribute value
        /// 
        /// If the xmlns attribute is in the form "http://www.sifinfo.org/v1.0r1/messages",
        /// the version identified by the namespace is returned (e.g. "1.0r1"). If the
        /// xmlns attribute is in the form "http://www.sifinfo.org/infrastructure/1.x",
        /// the latest version of SIF identified by the major version number is
        /// returned.
        /// 
        /// </summary>
        /// <param name="xmlns">A SIF xmlns attribute value (e.g. "http://www.sifinfo.org/v1.0r1/messages",
        /// "http://www.sifinfo.org/infrastructure/1x.", etc)
        /// 
        /// </param>
        /// <returns> A SifVersion object encapsulating the version of SIF identified
        /// by the xmlns value, or null if the xmlns is invalid
        /// </returns>
        public static SifVersion ParseXmlns(string xmlns)
        {
            //
            //  Determine the SIFVersion:
            //
            if (xmlns != null)
            {
                if (xmlns.EndsWith(".x"))
                {
                    //  http://www.sifinfo.org/infrastructure/1.x
                    //  NOTE: This works until SIF 10.x
                    int location = xmlns.LastIndexOf('/');
                    char majorCh = xmlns[location + 1];
                    if (char.IsDigit(majorCh))
                    {
                        int major = majorCh - 48;
                        return GetLatest(major);
                    }
                }
            }
            return null;
        }


        /// <summary>  Gets the string representation of the version</summary>
        /// <returns> The tag passed to the constructor. If null, a version in the
        /// form <i>major</i>.<i>minor</i>r<i>revision</i> is returned with no
        /// padding.
        /// </returns>
        public override string ToString()
        {
            return ToString(this.Major, this.Minor, this.Revision, '.');
        }

        /// <summary>
        /// Formats a SIF Version String for the ToString() and ToSymbol() methods
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        private static String ToString(int major,
                                        int minor,
                                        int revision,
                                        char seperator)
        {
            if (revision < 1)
            {
                return String.Format("{1}{0}{2}", seperator, major, minor);
            }
            else
            {
                return String.Format("{1}{0}{2}r{3}", seperator, major, minor, revision);
            }
        }

        /// <summary>  Gets the string representation of the version using an underscore instead
        /// of a period as the delimiter
        /// </summary>
        public virtual string ToSymbol()
        {
            return ToString(this.Major, this.Minor, this.Revision, '_');
        }

        #region Private Members

        /// <summary>
        /// The dictionary of current active versions
        /// </summary>
        private static IDictionary<String, SifVersion> sVersions = null;

        /// <summary>
        /// The current version
        /// </summary>
        protected readonly Version fVersion;

        #endregion

        #region Comparison


        public override bool Equals(object obj)
        {
            // Test reference comparison first ( fastest )
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if ((!ReferenceEquals(obj, null)) &&
                 (obj.GetType().Equals(this.GetType())))
            {
                return fVersion.Equals(((SifVersion)obj).fVersion);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return fVersion.GetHashCode();
        }

        /// <summary>  Compare this version to another</summary>
        /// <param name="version">The version to compare</param>
        /// <returns> -1 if this version is earlier than <c>version</c>, 0 if
        /// the versions are equal, or 1 if this version is greater than <c>
        /// version</c> or <c>version</c> is null
        /// </returns>
        public int CompareTo(object version)
        {
            if (version == null)
            {
                return 1;
            }
            if (!(version is SifVersion))
            {
                throw new ArgumentException
                    (string.Format
                          ("{0} cannot be compared to a SifVersion object", version.GetType().FullName));
            }
            return fVersion.CompareTo(((SifVersion)version).fVersion);
        }

        public static bool operator >(SifVersion version1,
                                       SifVersion version2)
        {
            return version1.fVersion > version2.fVersion;
        }

        public static bool operator <(SifVersion version1,
                                       SifVersion version2)
        {
            return version1.fVersion < version2.fVersion;
        }

        public static bool operator >=(SifVersion version1,
                                        SifVersion version2)
        {
            return version1.fVersion >= version2.fVersion;
        }

        public static bool operator <=(SifVersion version1,
                                        SifVersion version2)
        {
            return version1.fVersion <= version2.fVersion;
        }

        #endregion
    }
}