using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Xr.Common.Common
{
    /// <summary>
    /// 网络相关处理单元
    /// </summary>
    public static class NetUtils
    {
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetIPAddress()
        {
            foreach (var address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily.ToString() == "InterNetwork")
                {
                    return address;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取IP地址字符串
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddressString()
        {
            var address = GetIPAddress();
            return address != null ? address.ToString() : string.Empty;
        }


        /// <summary>
        /// 获取MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    return ni.GetPhysicalAddress().ToString();
                }
            }

            return string.Empty;
        }
    }
}
