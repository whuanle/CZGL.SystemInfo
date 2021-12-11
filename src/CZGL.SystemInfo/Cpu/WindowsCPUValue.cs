using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Cpu
{
    public struct WindowsCPUValue
    {
        /// <summary>
        /// 空闲时间
        /// </summary>
        public ulong LpIdleTime { get; set; }

        /// <summary>
        /// 内核时间
        /// </summary>
        public ulong LpKernelTime { get; set; }

        /// <summary>
        /// 用户时间
        /// </summary>
        public ulong LpUserTime { get; set; }
    }

}
