using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Network
{
    /// <summary>
    /// 统计 IPV4、IPV6 的流量
    /// </summary>
    public class Ipv46Speed
    {
        private NetworkInterface _instance;

        /// <summary>
        /// 提供针对本地计算机上的网络接口的 Internet 协议 (IP) 统计数据<br />
        /// </summary>
        public IPInterfaceStatistics IpStatistics => _IpStatistics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="network"></param>
        internal Ipv46Speed(NetworkInterface network)
        {
            _instance = network;
            _IpStatistics = _instance.GetIPStatistics();
        }

        private IPInterfaceStatistics _IpStatistics;

        /// <summary>
        /// 此网卡总接收网络流量字节数
        /// </summary>
        public long ReceivedLength => _IpStatistics.BytesReceived;

        /// <summary>
        /// 此网卡总发送网络流量字节数
        /// </summary>
        public long SendLength => _IpStatistics.BytesSent;

        private Speed LastSpeed;

        /// <summary>
        /// 开始监控网络流量
        /// </summary>
        public void Start() => Refresh();

        private void Refresh()
        {
            _IpStatistics = _instance.GetIPStatistics();
            LastSpeed = new Speed
            {
                StartTime = DateTime.Now,
                ReceivedLength = _IpStatistics.BytesReceived,
                SendLength = _IpStatistics.BytesSent
            };
        }

        /// <summary>
        /// 获取网络速度
        /// </summary>
        /// <remarks>不是很准确！</remarks>
        /// <returns></returns>
        public (SizeInfo ReceiveSpeed, SizeInfo SendSpeed) GetSpeed()
        {
            var before = LastSpeed;
            Refresh();

            var receive = LastSpeed.ReceivedLength - before.ReceivedLength;
            var send = LastSpeed.SendLength - before.SendLength;
            var interval = (LastSpeed.StartTime - before.StartTime).TotalSeconds;

            long rSpeed = (long)(receive / interval);
            long sSpeed = (long)(send / interval);

            return (SizeInfo.Get(rSpeed), SizeInfo.Get(sSpeed));
        }
    }
}
