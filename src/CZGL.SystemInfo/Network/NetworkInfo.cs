using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 网络接口信息
    /// </summary>
    public class NetworkInfo
    {
        private NetworkInterface _instance;

        private NetworkInfo(NetworkInterface network)
        {
            _instance = network;
        }

        /// <summary>
        /// 当前实例使用的网络接口
        /// </summary>
        public NetworkInterface NetworkInterface => _instance;


        #region 基础信息

        /// <summary>
        /// 获取网络适配器的标识符
        /// </summary>
        /// <remarks>ex：{92D3E528-5363-43C7-82E8-D143DC6617ED}</remarks>
        public string Id => _instance.Id;

        /// <summary>
        /// 网络的 Mac 地址
        /// </summary>
        /// <remarks>ex： 1C997AF108E3</remarks>
        public string Mac => _instance.GetPhysicalAddress().ToString();

        /// <summary>
        /// 网卡名称
        /// </summary>
        /// <remarks>ex：以太网，WLAN</remarks>
        public string Name => _instance.Name;


        /// <summary>
        /// 描述网络接口的用户可读文本，
        /// 在 Windows 上，它通常描述接口供应商、类型 (例如，以太网) 、品牌和型号；
        /// </summary>
        /// <remarks>ex：Realtek PCIe GbE Family Controller、  Realtek 8822CE Wireless LAN 802.11ac PCI-E NIC</remarks>
        public string Trademark => _instance.Description;

        /// <summary>
        /// 获取网络连接的当前操作状态<br />
        /// </summary>
        public OperationalStatus Status => _instance.OperationalStatus;

        /// <summary>
        /// 获取网卡接口类型<br />
        /// </summary>
        public NetworkInterfaceType NetworkType => _instance.NetworkInterfaceType;

        /// <summary>
        /// 网卡链接速度，每字节/秒为单位
        /// </summary>
        /// <remarks>如果是-1，则说明无法获取此网卡的链接速度；例如 270_000_000 表示是 270MB 的链接速度</remarks>
        public long Speed => _instance.Speed;

        /// <summary>
        /// 是否支持 Ipv4
        /// </summary>
        public bool IsSupportIpv4 => _instance.Supports(NetworkInterfaceComponent.IPv4);

        /// <summary>
        /// 获取分配给此接口的任意广播 IP 地址。只支持 Windows
        /// </summary>
        /// <remarks>一般情况下为空数组</remarks>
        public IPAddress[] AnycastAddresses
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return _instance.GetIPProperties().AnycastAddresses.Select(x => x.Address).ToArray();
                }
                else
                {
                    return Array.Empty<IPAddress>();
                }
            }
        }

        /// <summary>
        /// 获取分配给此接口的多播地址，ipv4、ipv6
        /// </summary>
        /// <remarks>ex：ff01::1%9 ff02::1%9<br />
        /// ff02::fb%9<br />
        /// ff02::1:3%9<br />
        /// ff02::1:ff61:9ae7%9<br />
        /// 224.0.0.1</remarks>
        public IPAddress[] MulticastAddresses => _instance.GetIPProperties().MulticastAddresses.Select(x => x.Address).ToArray();

        /// <summary>
        /// 获取分配给此接口的单播地址，ipv4、ipv6
        /// </summary>
        /// <remarks>ex：192.168.3.38</remarks>
        public IPAddress[] UnicastAddresses => _instance.GetIPProperties().UnicastAddresses.Select(x => x.Address).ToArray();

        /// <summary>
        /// 获取此接口的 IPv4 网关地址，ipv4、ipv6
        /// </summary>
        /// <remarks>ex：fe80::1677:40ff:fef9:bf95%5、192.168.3.1</remarks>
        public IPAddress[] GatewayAddresses => _instance.GetIPProperties().GatewayAddresses.Select(x => x.Address).ToArray();

        /// <summary>
        /// 获取此接口的域名系统 (DNS) 服务器的地址，ipv4、ipv6
        /// </summary>
        /// <remarks>ex：fe80::1677:40ff:fef9:bf95%5、192.168.3.1</remarks>
        public IPAddress[] DnsAddresses => _instance.GetIPProperties().DnsAddresses.ToArray();

        /// <summary>
        /// 是否支持 Ipv6
        /// </summary>
        public bool IsSupportIpv6 => _instance.Supports(NetworkInterfaceComponent.IPv6);

        #endregion



        /// <summary>
        /// 当前主机是否能够与其他计算机通讯(公网或内网)，如果任何网络接口标记为 "up" 且不是环回或隧道接口，则认为网络连接可用。
        /// </summary>
        public static bool GetIsNetworkAvailable => NetworkInterface.GetIsNetworkAvailable();

        /// <summary>
        /// 计算 IPV4 的网络流量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">当前网卡不支持 IPV4</exception>
        public Rate GetIpv4Speed()
        {
            // 当前网卡不支持 IPV4
            if (!IsSupportIpv4) return default;
            var ipv4Statistics = _instance.GetIPv4Statistics();
            var speed = new Rate(DateTime.Now, ipv4Statistics.BytesReceived, ipv4Statistics.BytesSent);
            return speed;
        }

        /// <summary>
        /// 计算 IPV4 、IPV6 的网络流量
        /// </summary>
        /// <returns></returns>
        public Rate IpvSpeed()
        {
            var ipvStatistics = _instance.GetIPStatistics();
            var speed = new Rate(DateTime.Now, ipvStatistics.BytesReceived, ipvStatistics.BytesSent);
            return speed;
        }

        /// <summary>
        /// 获取所有 IP 地址
        /// </summary>
        /// <returns></returns>
        public static IPAddress[] GetIPAddresses()
        {
            var hostName = Dns.GetHostName();
            return Dns.GetHostAddresses(hostName);
        }

        /// <summary>
        /// 获取当前真实 IP
        /// </summary>
        /// <returns></returns>
        public static IPAddress? TryGetRealIpv4()
        {
            var addrs = GetIPAddresses();
            var ipv4 = addrs.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
            return ipv4;
        }

        /// <summary>
        /// 获取此主机中所有网卡接口
        /// </summary>
        /// <returns></returns>
        public static NetworkInfo[] GetNetworkInfos()
        {
            return NetworkInterface.GetAllNetworkInterfaces().Select(x => new NetworkInfo(x)).ToArray();
        }

        /// <summary>
        /// 计算网络流量速率
        /// </summary>
        /// <param name="oldRate"></param>
        /// <param name="newRate"></param>
        /// <returns></returns>
        public static (SizeInfo Received, SizeInfo Sent) GetSpeed(Rate oldRate, Rate newRate)
        {
            var receive = newRate.ReceivedLength - oldRate.ReceivedLength;
            var send = newRate.SendLength - oldRate.SendLength;
            var interval = Math.Round((newRate.StartTime - oldRate.StartTime).TotalSeconds, 2);

            long rSpeed = (long)(receive / interval);
            long sSpeed = (long)(send / interval);

            return (SizeInfo.Get(rSpeed), SizeInfo.Get(sSpeed));
        }
    }
}
