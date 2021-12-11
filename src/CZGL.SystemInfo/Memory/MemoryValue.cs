using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Memory
{
    /// <summary>
    /// 内存值表示
    /// </summary>
    public class MemoryValue
    {
        /// <summary>
        /// 物理内存字节数
        /// </summary>
        public ulong TotalPhysicalMemory { get; set; }

        /// <summary>
        /// 可用的物理内存字节数
        /// </summary>
        public ulong AvailablePhysicalMemory { get; set; }

        /// <summary>
        /// 已用物理内存字节数
        /// </summary>
        public ulong UsedPhysicalMemory => TotalPhysicalMemory - AvailablePhysicalMemory;

        /// <summary>
        /// 已用物理内存百分比，0~100，100表示内存已用尽
        /// </summary>
        public int UsedPercentage { get; set; }

        /// <summary>
        /// 虚拟内存字节数
        /// </summary>
        public ulong TotalVirtualMemory { get; set; }

        /// <summary>
        /// 可用虚拟内存字节数
        /// </summary>
        public ulong AvailableVirtualMemory { get; set; }

        /// <summary>
        /// 已用虚拟内存字节数
        /// </summary>
        public ulong UsedVirtualMemory => TotalVirtualMemory - AvailableVirtualMemory;
    }

}