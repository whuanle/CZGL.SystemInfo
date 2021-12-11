using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Cpu
{
    /// <summary>
    /// Linux 每个 CPU 核的相应值
    /// </summary>
    public struct LinuxCPUValue
    {
        /// <summary>
        /// CPU 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 从系统启动开始累积到当前时刻，用户已用 CPU
        /// </summary>
        public int User { get; set; }

        /// <summary>
        /// 从系统启动开始累计到当前时刻，内核已用 CPU
        /// </summary>
        public int Nice { get; set; }

        /// <summary>
        /// 从系统启动开始累计到当前时刻，核心时间
        /// </summary>
        public int System { get; set; }

        /// <summary>
        /// 从系统启动开始累积到当前时刻，除硬盘IO等待时间意外其他等待时间
        /// </summary>
        public int Idle { get; set; }
    }
}
