using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Network
{

    /// <summary>
    /// 
    /// </summary>
    public struct Rate
    {
        public Rate(DateTime startTime, long receivedLength, long sendLength)
        {
            StartTime = startTime;
            ReceivedLength = receivedLength;
            SendLength = sendLength;
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// 此网卡总接收网络流量字节数
        /// </summary>
        public long ReceivedLength { get; private set; }

        /// <summary>
        /// 此网卡总发送网络流量字节数
        /// </summary>
        public long SendLength { get; private set; }
    }

}
