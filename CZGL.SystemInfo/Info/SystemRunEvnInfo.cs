using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.SystemInfo.Info
{
    /// <summary>
    /// 系统运行信息
    /// </summary>
    [InfoName(ChinaName = "系统运行信息")]
    public class SystemRunEvnInfo
    {
        /// <summary>
        /// 机器名称
        /// </summary>
        public static SystemRunEvnInfo Instance => new SystemRunEvnInfo();
        [InfoName(ChinaName = "机器名称")]
        public string MachineName { get { return Environment.MachineName; } }

        /// <summary>
        /// 当前关联用户名
        /// </summary>
        [InfoName(ChinaName = "当前关联用户名")]
        public string UserName { get { return Environment.UserName; } }

        /// <summary>
        /// 用户网络域名
        /// </summary>
        [InfoName(ChinaName = "用户网络域名")]
        public string UserDomainName { get { return Environment.UserDomainName; } }

        /// <summary>
        /// 系统已运行时间(毫秒)
        /// </summary>
        [InfoName(ChinaName = "系统已运行时间(毫秒)")]
        public int TickCount { get { return Environment.TickCount; } }

        /// <summary>
        /// Web程序核心框架版本
        /// </summary>
        [InfoName(ChinaName = "Web程序核心框架版本")]
        public string Version { get { return Environment.Version.ToString(); } }

        /// <summary>
        /// 是否在交互模式中运行
        /// </summary>
        [InfoName(ChinaName = "是否在交互模式中运行")]
        public bool UserInteractive { get { return Environment.UserInteractive; } }

        /// <summary>
        /// 分区磁盘
        /// </summary>
        [InfoName(ChinaName = "分区磁盘")]
        public string GetLogicalDrives { get { return string.Join(", ", Environment.GetLogicalDrives()); } }

        /// <summary>
        /// 系统目录
        /// </summary>
        [InfoName(ChinaName = "系统目录")]
        public string SystemDirectory { get { return Environment.SystemDirectory; } }
    }
}
