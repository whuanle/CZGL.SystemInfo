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
    /// 网络流量监控
    /// </summary>
    public struct InternetSpeed
    {
        internal bool IsNotNull;
        /// <summary>
        /// 上次接收
        /// </summary>
        internal long LastRec;

        internal long LastSend;

        internal DateTime LastTime;

        internal SizeInfo _SentSizeInfo;
        internal SizeInfo _ReceivedSizeInfo;

        /// <summary>
        /// 上传速度
        /// </summary>
        public SizeInfo Sent => _SentSizeInfo;

        /// <summary>
        /// 下载速度
        /// </summary>
        public SizeInfo Received => _ReceivedSizeInfo;

    }
}
