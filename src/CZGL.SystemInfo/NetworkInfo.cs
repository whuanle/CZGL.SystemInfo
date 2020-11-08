using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 网络接口信息
    /// </summary>
    public class NetworkInfo
    {
        private NetworkInterface _instance;
        public NetworkInterface NetworkInterface => _instance;

        private NetworkInfo(NetworkInterface network)
        {
            _instance = network;
            _Statistics = new Lazy<IPInterfaceStatistics>(() => _instance.GetIPStatistics());
            _Ipv4Statistics = new Lazy<IPv4InterfaceStatistics>(() => _instance.GetIPv4Statistics());
            _AddressIpv6 = new Lazy<IPAddress>(() => _instance.GetIPProperties().UnicastAddresses
             .FirstOrDefault(x => x.IPv4Mask.ToString().Equals("0.0.0.0")).Address);
            _AddressIpv4 = new Lazy<IPAddress>(() => _instance.GetIPProperties().UnicastAddresses
            .FirstOrDefault(x => !x.IPv4Mask.ToString().Equals("0.0.0.0")).Address);
        }

        /// <summary>
        /// Gets the identifier of the network adapter<br />
        /// 获取网络适配器的标识符
        /// </summary>
        public string Id => _instance.Id;

        /// <summary>
        /// The Mac address of the network<br />
        /// 网络的 Mac 地址
        /// </summary>
        public string Mac => _instance.GetPhysicalAddress().ToString();

        /// <summary>
        /// The network equipment name<br />
        /// 网卡名称
        /// <para>ex:WLAN</para>
        /// </summary>
        public string Name => _instance.Name;


        /// <summary>
        /// User-readable text that describes the network interface<br />
        /// 描述网络接口的用户可读文本
        /// <para> On Windows, it typically describes the interface vendor, type (for example, Ethernet), brand, and model<br />
        /// 在 Windows 上，它通常描述接口供应商、类型 (例如，以太网) 、品牌和型号；</para>
        /// </summary>
        public string Trademark => _instance.Description;

        /// <summary>
        /// Returns the Media Access Control (MAC) or physical address for this adapter<br />
        /// 获取当前网卡的 mac 地址
        /// </summary>
        public string PhysicalMac => _instance.GetPhysicalAddress().ToString();

        /// <summary>
        /// Specifies the operational state of a network interface<br />
        /// 获取网络连接的当前操作状态<br />
        /// documentation:    <see href="https://docs.microsoft.com/zh-cn/dotnet/api/system.net.networkinformation.operationalstatus?view=netcore-3.1"/>
        /// </summary>
        public OperationalStatus Status => _instance.OperationalStatus;

        /// <summary>
        /// Specifies types of network interfaces<br />
        /// 获取网卡接口类型<br />
        /// <see href="https://docs.microsoft.com/zh-cn/dotnet/api/system.net.networkinformation.networkinterfacetype?view=netcore-3.1"/>
        /// </summary>
        public NetworkInterfaceType NetworkType => _instance.NetworkInterfaceType;

        #region 网络流量信息W

        private Lazy<IPInterfaceStatistics> _Statistics;

        /// <summary>
        /// Provides Internet Protocol (IP) statistical data for an network interface on the local computer<br />
        /// 提供针对本地计算机上的网络接口的 Internet 协议 (IP) 统计数据<br />
        /// <see href="https://docs.microsoft.com/zh-cn/dotnet/api/system.net.networkinformation.ipinterfacestatistics?view=netcore-3.1"/>
        /// </summary>
        public IPInterfaceStatistics Statistics => _Statistics.Value;

        private Lazy<IPv4InterfaceStatistics> _Ipv4Statistics;

        /// <summary>
        /// Provides Internet Protocol (IP) statistical data for an network interface on the local computer<br />
        /// 提供针对本地计算机上的网络接口的 Internet 协议 (IP) 统计数据<br />
        /// <see href="https://docs.microsoft.com/zh-cn/dotnet/api/system.net.networkinformation.ipv4interfacestatistics?view=netcore-3.1"/>
        /// </summary>
        public IPv4InterfaceStatistics Ipv4Statistics => _Ipv4Statistics.Value;

        /// <summary>
        /// Gets the number of bytes that were received on the interface<br />
        /// 获取该接口上接收到的字节数，即网络下载总量。
        /// </summary>
        public long ReceivedLength => _Statistics.Value.BytesReceived;

        /// <summary>
        /// Gets the number of bytes that were ipv4 received on the interface<br />
        /// 获取该接口 ipv4 上接收到的字节数，即网络下载总量。
        /// </summary>
        public long ReceivedLengthIpv4 => _Ipv4Statistics.Value.BytesReceived;


        /// <summary>
        /// Gets the number of bytes that were sent on the interface<br />
        /// 获取该接口上发送的字节数，即网络上传总量
        /// </summary>
        public long SendLength => _Statistics.Value.BytesSent;

        /// <summary>
        /// Gets the number of bytes that were sent on the ipv4 interface<br />
        /// 获取该接口 ipv4 上发送的字节数，即网络上传总量
        /// </summary>
        public long SendLengthIpv4 => _Ipv4Statistics.Value.BytesSent;

        #endregion

        /// <summary>
        /// Indicates whether any network connection is available.A network connection is considered to be available if any network interface is marked "up" and is not a loopback or tunnel interface.<br />
        /// 当前主机是否有可用网络，如果任何网络接口标记为 "up" 且不是环回或隧道接口，则认为网络连接可用。
        /// </summary>
        public static bool IsAvailable => NetworkInterface.GetIsNetworkAvailable();

        /// <summary>
        /// Network card link speed, per byte/second in units<br />
        /// 网卡链接速度，每字节/秒为单位
        /// </summary>
        /// <remarks>如果是-1，则说明无法获取此网卡的链接速度；例如 270_000_000 表示是 270MB 的链接速度</remarks>
        public long Speed => _instance.Speed;

        /// <summary>
        /// Whether Ipv4 is supported<br />
        /// 是否支持 Ipv4
        /// </summary>
        public bool IsSupportIpv4 => _instance.Supports(NetworkInterfaceComponent.IPv4);

        /// <summary>
        /// Whether Ipv6 is supported<br />
        /// 是否支持 Ipv6
        /// </summary>
        public bool IsSupportIpv6 => _instance.Supports(NetworkInterfaceComponent.IPv6);

        #region 地址相关
        /// <summary>
        /// DnsSuffix<br />
        ///  连接特定的 DNS 后缀
        /// </summary>
        public string DnsSuffix => _instance.GetIPProperties().DnsSuffix;

        /// <summary>
        /// DNS Server address collection
        /// </summary>
        public IPAddressCollection DNSAddresses => _instance.GetIPProperties().DnsAddresses;

        /// <summary>
        /// Gets the unicast addresses assigned to this interface<br />
        /// 获取分配给此接口的单播地址
        /// </summary>
        public UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection => _instance.GetIPProperties().UnicastAddresses;

        private Lazy<IPAddress> _AddressIpv6;
        private Lazy<IPAddress> _AddressIpv4;

        /// <summary>
        /// Local link IPv6 address<br />
        /// 本地链接 IPv6 地址
        /// </summary>
        public IPAddress AddressIpv6 => _AddressIpv6.Value;

        /// Local link IPv4 address<br />
        /// 本地链接 IPv4 地址
        /// </summary>
        public IPAddress AddressIpv4 => _AddressIpv4.Value;


        #endregion

        /// <summary>
        /// Internet speed<br />
        /// 获取当前网卡的网络速度<br />
        /// </summary>
        /// <param name="Milliseconds">间隔时间</param>
        /// <returns></returns>
        public (int Received, int Send) GetInternetSpeed(int Milliseconds)
        {
            var newNetwork = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => x.Id == this.Id).GetIPStatistics();

            long rec = ReceivedLength;
            long send = SendLength;
            Thread.Sleep(Milliseconds);
            return ((int)(newNetwork.BytesReceived - rec), (int)(newNetwork.BytesSent - send));
        }

        /// <summary>
        /// Internet speed<br />
        /// 获取当前网卡的网络速度<br />
        /// </summary>
        /// <param name="Milliseconds">间隔时间</param>
        /// <returns></returns>
        public (int Received, int Send) GetInternetSpeedIpv4(int Milliseconds)
        {
            var newNetwork = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => x.Id == this.Id).GetIPv4Statistics();

            long rec = ReceivedLengthIpv4;
            long send = SendLengthIpv4;
            Thread.Sleep(Milliseconds);
            return ((int)(newNetwork.BytesReceived - rec), (int)(newNetwork.BytesSent - send));
        }

        /// <summary>
        /// Get the MAC address of the physical network card (unique)<br />
        /// 获取物理网卡的 MAC 地址(唯一)，<b>获取当前能够联网的网卡的mac</b>，如果你本次使用wifi上网，下次换成网线接口，那么mac地址会变！
        /// </summary>
        /// <remarks>标记为 "up" 且不是环回或隧道接口</remarks>
        public static string GetPhysicalMac => NetworkInterface.GetAllNetworkInterfaces()
                                              .FirstOrDefault(x => x.OperationalStatus == OperationalStatus.Up
                                              && x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                              && x.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                                              .GetPhysicalAddress().ToString();

        /// <summary>
        /// Get traffic statistics for the current network card being connected<br />
        /// 获取当前正在联网的网卡流量统计
        /// </summary>
        /// <param name="Milliseconds"></param>
        /// <returns></returns>
        public static (int Received, int send) GetNowInternetSpeed(int Milliseconds)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return (0, 0);

            NetworkInterface inter = GetNetworkInfo().NetworkInterface;
            var statistics = inter.GetIPStatistics();
            long rec = statistics.BytesReceived;
            long send = statistics.BytesSent;
            Thread.Sleep(Milliseconds);
            return ((int)(statistics.BytesReceived - rec), (int)(statistics.BytesSent - send));
        }

        /// <summary>
        /// Get traffic statistics for the current network card being connected<br />
        /// 获取当前正在联网的网卡流量统计
        /// </summary>
        /// <param name="Milliseconds"></param>
        /// <returns></returns>
        public static (int Received, int send) GetNowInternetSpeedIpv4(int Milliseconds)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                return (0, 0);

            NetworkInterface inter = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => x.OperationalStatus == OperationalStatus.Up);
            var statistics = inter.GetIPv4Statistics();
            long rec = statistics.BytesReceived;
            long send = statistics.BytesSent;
            Thread.Sleep(Milliseconds);
            return ((int)(statistics.BytesReceived - rec), (int)(statistics.BytesSent - send));
        }


        /// <summary>
        /// Network card information currently in the network<br />
        /// 当前正在联网的网卡信息<br />标记为 "up" 且不是环回或隧道接口
        /// </summary>
        /// <returns></returns>
        public static NetworkInfo GetNetworkInfo()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                return new NetworkInfo(NetworkInterface.GetAllNetworkInterfaces()
                                  .FirstOrDefault(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                  && x.NetworkInterfaceType != NetworkInterfaceType.Ethernet));

            return new NetworkInfo(NetworkInterface.GetAllNetworkInterfaces()
                                              .FirstOrDefault(x => x.OperationalStatus == OperationalStatus.Up
                                              && x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                              && x.NetworkInterfaceType != NetworkInterfaceType.Ethernet));
        }

        /// <summary>
        /// All network card information<br />
        /// 所有网卡信息
        /// </summary>
        /// <returns></returns>
        public static NetworkInfo[] GetNetworkInfos()
        {
            return NetworkInterface.GetAllNetworkInterfaces().Select(x => new NetworkInfo(x)).ToArray();
        }

    }
}
