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
    public struct Speed
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 此网卡总接收网络流量字节数
        /// </summary>
        public long ReceivedLength { get; set; }

        /// <summary>
        /// 此网卡总发送网络流量字节数
        /// </summary>
        public long SendLength { get; set; }
    }

}
