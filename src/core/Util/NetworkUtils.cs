//
// Copyright (c)1998-2011 Pearson Education, Inc. or its affiliate(s). 
// All rights reserved.
//

using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace OpenADK.Util

{
    /// <summary>
    /// This class contains methods for retreiving local network information
    /// </summary>
    /// <remarks>
    /// <para>
    /// </para>
    /// <example>
    /// For example, to list the local machine's live,non-loopback ipv4 IP addresses:
    /// <code>
    /// string def;
    /// bool UseIPV6 = false;
    /// def = (string)NetworkUtils.GetLocalAddresses(UseIPV6)[0].ToString();
    /// </code>
    /// </example>
    /// </remarks>
    public sealed class NetworkUtils
    {
        /// <summary>
        /// Returns a list of IP Addresses
        /// </summary>
        /// <param name="allowIPV6">If true, IPv6 addresses will be preferred. If false, only IPv4 addresses will be returned</param>
        /// <returns></returns>
        internal static IList<IPAddress> GetLocalAddresses( bool allowIPV6 )
        {
            List<IPAddress> list = new List<IPAddress>();
            if ( !NetworkInterface.GetIsNetworkAvailable() )
            {
                // No Network is available. Return a loopback address
                if ( allowIPV6 )
                {
                    list.Add( IPAddress.IPv6Loopback );
                }
                else
                {
                    list.Add( IPAddress.Loopback );
                }
                return list;
            }

            foreach ( NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces() )
            {
                // Disallow VPN connections
                // If they are necessary, configure the agent manually to us it
                if ( ni.OperationalStatus == OperationalStatus.Up &&
                     ni.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                     ni.NetworkInterfaceType != NetworkInterfaceType.Ppp )
                {
                    IPInterfaceProperties niProperties = ni.GetIPProperties();
                    UnicastIPAddressInformationCollection addresses = niProperties.UnicastAddresses;
                    bool added = false;

                    if ( allowIPV6 && ni.Supports( NetworkInterfaceComponent.IPv6 ) )
                    {
                        foreach ( UnicastIPAddressInformation uipi in addresses )
                        {
                            if ( uipi.IPv4Mask.Equals( IPAddress.Any ) )
                            {
                                // Add Ipv6 addresses to the front of the list
                                list.Insert( 0, uipi.Address );
                                added = true;
                                break;
                            }
                        }
                    }

                    // If we haven't added an address from this network adaptor yet in our 
                    // IPv6 search above, add it now, if possible
                    if ( !added && ni.Supports( NetworkInterfaceComponent.IPv4 ) )
                    {
                        IPv4InterfaceProperties ipv4ip = niProperties.GetIPv4Properties();
                        if ( ipv4ip != null )
                        {
                            foreach ( UnicastIPAddressInformation uipi in addresses )
                            {
                                if ( !uipi.IPv4Mask.Equals( IPAddress.Any ) )
                                {
                                    list.Add( uipi.Address );
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }
    }
}
