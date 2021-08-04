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
    public partial class NetworkInfo
    {
        private NetworkInterface _instance;
        public NetworkInterface NetworkInterface => _instance;

        private NetworkInfo(NetworkInterface network)
        {
            if (network == null)
                throw new ArgumentNullException(nameof(NetworkInterface));

            _instance = network;

            speed = new InternetSpeed();
            _Statistics = new Lazy<IPInterfaceStatistics>(() => _instance.GetIPStatistics());
            _Ipv4Statistics = new Lazy<IPv4InterfaceStatistics>(() => _instance.GetIPv4Statistics());
            _AddressIpv6 = new Lazy<IPAddress>(() => _instance.GetIPProperties().UnicastAddresses
            .FirstOrDefault(x => x.IPv4Mask.ToString().Equals("0.0.0.0"))?.Address);
            _AddressIpv4 = new Lazy<IPAddress>(() => _instance.GetIPProperties().UnicastAddresses
            .FirstOrDefault(x => !x.IPv4Mask.ToString().Equals("0.0.0.0"))?.Address);
        }

        #region 接口信息
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


        #endregion

        #region 网络流量信息

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


        private InternetSpeed speed;


        public InternetSpeed GetInternetSpeed()
        {
            var newNetWorkInterface = _instance;
            var newNetwork = newNetWorkInterface.GetIPStatistics();

            long rec = newNetwork.BytesReceived;
            long send = newNetwork.BytesSent;

            var time = DateTime.Now;
            var second = (time - speed.LastTime).TotalSeconds;

            if (speed.IsNotNull)
            {
                speed._ReceivedSizeInfo = SizeInfo.Get(newNetwork.BytesReceived - speed.LastRec);
                speed._ReceivedSizeInfo.Size = Math.Round(speed._ReceivedSizeInfo.Size / (decimal)second, 1);
                speed._SentSizeInfo = SizeInfo.Get(newNetwork.BytesSent - speed.LastSend);
                speed._SentSizeInfo.Size = Math.Round(speed._SentSizeInfo.Size / (decimal)second, 1);
            }

            speed.IsNotNull = true;
            speed.LastSend = send;
            speed.LastRec = rec;
            speed.LastTime = time;
            return speed;
        }

        /// <summary>
        /// Internet speed<br />
        /// 获取当前网卡的网络速度<br />
        /// <code>
        /// var speed = new NetworkInfo.InternetSpeed();<br />
        /// var result = GetInternetSpeed(ref speed)
        /// </code>
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="Milliseconds">间隔时间</param>
        /// <param name="ip4"></param>
        /// <returns></returns>
        public ref InternetSpeed GetInternetSpeed(ref InternetSpeed speed, int Milliseconds = 1000)
        {
            var newNetWorkInterface = _instance;
            var newNetwork = newNetWorkInterface.GetIPStatistics();

            long rec = newNetwork.BytesReceived;
            long send = newNetwork.BytesSent;

            var time = DateTime.Now;
            var second = (time - speed.LastTime).TotalSeconds;

            if (speed.IsNotNull)
            {
                Thread.Sleep(Milliseconds);
                speed._ReceivedSizeInfo = SizeInfo.Get(newNetwork.BytesReceived - speed.LastRec);
                speed._ReceivedSizeInfo.Size = Math.Round(speed._ReceivedSizeInfo.Size / (decimal)second, 1);
                speed._SentSizeInfo = SizeInfo.Get(newNetwork.BytesSent - speed.LastSend);
                speed._SentSizeInfo.Size = Math.Round(speed._SentSizeInfo.Size / (decimal)second, 1);
            }

            speed.IsNotNull = true;
            speed.LastSend = send;
            speed.LastRec = rec;
            speed.LastTime = time;
            return ref speed;
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

        public static NetworkInfo[] GetRealNetworkInfos()
        {
            return
                NetworkInterface.GetAllNetworkInterfaces()
                .Where(x => x.OperationalStatus == OperationalStatus.Up
                                            && x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                            && x.GetIPProperties().UnicastAddresses.Any(i=> !i.IPv4Mask.ToString().Equals("0.0.0.0")))
                .Select(x => new NetworkInfo(x))
                .ToArray();
        }

    }
}
