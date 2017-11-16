using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using UltimateAdmin.Core.Logging;

namespace UltimateAdmin.Core.Network
{
    public static class NetStatus
    {
        [DllImport("dnsapi.dll", EntryPoint = "DnsFlushResolverCache")]
        private static extern UInt32 DnsFlushResolverCache();

        public static string GetIP(string computerName)
        {
            try
            {
                IPAddress address = Dns.GetHostEntry(computerName).AddressList[0];
                Ping ping = new Ping();
                byte[] buffer = new byte[32];
                PingOptions options = new PingOptions();
                options.Ttl = 128;
                PingReply pingReply = ping.Send(address, 2000, buffer, options);
                if (pingReply.Status == IPStatus.Success)
                {
                    return address.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (PingException e)
            {
                return null;
            }
            catch (SocketException ex)
            {
                return null;
            }
        }

        public static bool IsOnline(string computer)
        {
            FlushDNS();
            bool isOnline = PingHost(computer);
            return isOnline;
        }

        private static bool PingHost(string computerName)
        {
            try
            {
                IPAddress address = Dns.GetHostEntry(computerName).AddressList[0];
                Ping ping = new Ping();
                byte[] buffer = new byte[32];
                PingOptions options = new PingOptions();
                options.Ttl = 128;
                PingReply pingReply = ping.Send(address, 2000, buffer, options);
                if (pingReply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (PingException e)
            {
                return false;
            }
            catch (SocketException ex)
            {
                return false;
            }
        }

        public static long GetPingTime(string computerName)
        {
            try
            {
                IPAddress address = Dns.GetHostEntry(computerName).AddressList[0];
                Ping ping = new Ping();
                byte[] buffer = new byte[32];
                PingOptions options = new PingOptions();
                options.Ttl = 128;
                PingReply pingReply = ping.Send(address, 2000, buffer, options);
                if (pingReply.Status == IPStatus.Success)
                {
                    var pingTime = pingReply.RoundtripTime;
                    return pingTime;
                }
                else
                {
                    return 0;
                }
            }
            catch (PingException e)
            {
                return 0;
            }
            catch (SocketException ex)
            {
                return 0;
            }
        }

        public static PingResult GetPingWithResult(string computerName)
        {
            try
            {
                IPAddress address = Dns.GetHostEntry(computerName).AddressList[0];
                if(address.AddressFamily == AddressFamily.InterNetworkV6 || computerName.Equals(GetMachineNameFromIPAddress(address.ToString())))
                {
                    return new PingResult(address);
                }
                else
                {
                    return new PingResult(null);
                }
            }
            catch (SocketException e)
            {
                string logMessage = String.Format("Error attempting to ping: {0}. Exception message: {1}.", computerName, e.Message);
                Logger.Log(logMessage, MethodBase.GetCurrentMethod().Name);
                return new PingResult(null);
            }
        }

        public static void FlushDNS()
        {
            UInt32 result = DnsFlushResolverCache();
        }

        public static string GetMachineNameFromIPAddress(string ipAdress)
        {
            string machineName = string.Empty;
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(ipAdress);
                machineName = hostEntry.HostName.Split('.')[0].ToUpper();
            }
            catch (Exception ex)
            {
                // Machine not found...
            }
            return machineName;
        }
    }
}