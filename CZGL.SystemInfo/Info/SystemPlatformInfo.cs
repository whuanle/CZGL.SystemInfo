using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CZGL.SystemInfo.Info
{
    /// <summary>
    /// 系统平台信息
    /// </summary>
    [InfoName(ChinaName = "系统运行平台")]
    public class SystemPlatformInfo
    {
        /// <summary>
        /// 静态实例
        /// </summary>
        public static SystemPlatformInfo Instance => new SystemPlatformInfo();

        /// <summary>
        /// 运行框架
        /// </summary>
        [InfoName(ChinaName = "运行框架")]
        public string FrameworkDescription { get { return RuntimeInformation.FrameworkDescription; } }

        /// <summary>
        /// 操作系统
        /// </summary>
        [InfoName(ChinaName = "操作系统")]
        public string OSDescription { get { return RuntimeInformation.OSDescription; } }

        /// <summary>
        /// 操作系统版本
        /// </summary>
        [InfoName(ChinaName = "操作系统版本")]
        public string OSVersion { get { return Environment.OSVersion.ToString(); } }

        /// <summary>
        /// 平台架构
        /// </summary>
        [InfoName(ChinaName = "平台架构")]
        public string OSArchitecture { get { return RuntimeInformation.OSArchitecture.ToString(); } }
    }
}
