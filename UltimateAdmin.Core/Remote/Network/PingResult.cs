using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Core.Logging;
using UltimateAdmin.Core.Network;

namespace UltimateAdmin.Core
{
    public class PingResult
    {
        private string _hostName;
        public string HostName
        {
            get
            {
                return _hostName;
            }
        }
        private long _responseTime;
        public long ResponseTime
        {
            get
            {
                return _responseTime;
            }
        }
        private bool _isOnline;
        public bool IsOnline
        {
            get
            {
                return _isOnline;
            }
        }

        public PingResult(IPAddress ipAddress)
        {
            if(ipAddress == null)
            {
                _isOnline = false;
                _hostName = "unavailable";
                _responseTime = -1;
            }
            else
            {
                PingMachine(ipAddress.ToString());
            }
        }

        private void PingMachine(string ipAddress)
        {
            try
            {
                Ping ping = new Ping();
                byte[] buffer = new byte[32];
                PingOptions options = new PingOptions();
                options.Ttl = 128;
                PingReply pingReply = ping.Send(ipAddress, 2000, buffer, options);
                if (pingReply.Status == IPStatus.Success)
                {
                    _isOnline = true;
                    _responseTime = pingReply.RoundtripTime;
                    _hostName = NetStatus.GetMachineNameFromIPAddress(ipAddress);
                }
                else
                {
                    _isOnline = false;
                    _responseTime = -1;
                    _hostName = "unavailable";
                }
            }
            catch (PingException e)
            {
                Logger.Log(e.Message, MethodBase.GetCurrentMethod().Name);
            }
            catch (SocketException ex)
            {
                Logger.Log(ex.Message, MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
