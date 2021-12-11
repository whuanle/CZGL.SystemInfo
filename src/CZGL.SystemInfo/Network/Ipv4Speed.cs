using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Network
{

    /// <summary>
    /// 统计 IPV4 的流量
    /// </summary>
    public class Ipv4Speed
    {
        private NetworkInterface _instance;

        /// <summary>
        /// 提供针对本地计算机上的网络接口的 Internet 协议 (IP) 统计数据<br />
        /// </summary>
        internal IPv4InterfaceStatistics Ipv4Statistics => _Ipv4Statistics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="network"></param>
        public Ipv4Speed(NetworkInterface network)
        {
            _instance = network;
            _Ipv4Statistics = _instance.GetIPv4Statistics();
        }

        private IPv4InterfaceStatistics _Ipv4Statistics;

        /// <summary>
        /// 此网卡总接收网络流量字节数
        /// </summary>
        public long ReceivedLength => _Ipv4Statistics.BytesReceived;

        /// <summary>
        /// 此网卡总发送网络流量字节数
        /// </summary>
        public long SendLength => _Ipv4Statistics.BytesSent;

        private Speed LastSpeed;

        /// <summary>
        /// 开始监控网络流量
        /// </summary>
        public void Start() => Refresh();

        private void Refresh()
        {
            _Ipv4Statistics = _instance.GetIPv4Statistics();
            LastSpeed = new Speed
            {
                StartTime = DateTime.Now,
                ReceivedLength = _Ipv4Statistics.BytesReceived,
                SendLength = _Ipv4Statistics.BytesSent
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
            var interval = Math.Round((LastSpeed.StartTime - before.StartTime).TotalSeconds, 2);

            long rSpeed = (long)(receive / interval);
            long sSpeed = (long)(send / interval);

            return (SizeInfo.Get(rSpeed), SizeInfo.Get(sSpeed));
        }

    }
}
