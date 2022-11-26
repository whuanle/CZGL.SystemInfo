using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Memory
{
    /// <summary>
    /// 包含有关物理内存和虚拟内存（包括扩展内存）的当前状态的信息。该 GlobalMemoryStatusEx在这个构造函数存储信息。
    /// <see ref="https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/ns-sysinfoapi-memorystatusex"/>
    /// </summary>
    internal struct MemoryStatusExE
    {
        /// <summary>
        /// 结构的大小，以字节为单位，必须在调用 GlobalMemoryStatusEx 之前设置此成员，可以用 Init 方法提前处理
        /// </summary>
        /// <remarks>应当使用本对象提供的 Init ，而不是使用构造函数！</remarks>
        internal uint dwLength;

        /// <summary>
        /// 一个介于 0 和 100 之间的数字，用于指定正在使用的物理内存的大致百分比（0 表示没有内存使用，100 表示内存已满）。
        /// </summary>
        internal uint dwMemoryLoad;

        /// <summary>
        /// 实际物理内存量，以字节为单位
        /// </summary>
        internal ulong ullTotalPhys;

        /// <summary>
        /// 当前可用的物理内存量，以字节为单位。这是可以立即重用而无需先将其内容写入磁盘的物理内存量。它是备用列表、空闲列表和零列表的大小之和
        /// </summary>
        internal ulong ullAvailPhys;

        /// <summary>
        /// 系统或当前进程的当前已提交内存限制，以字节为单位，以较小者为准。要获得系统范围的承诺内存限制，请调用GetPerformanceInfo
        /// </summary>
        internal ulong ullTotalPageFile;

        /// <summary>
        /// 当前进程可以提交的最大内存量，以字节为单位。该值等于或小于系统范围的可用提交值。要计算整个系统的可承诺值，调用GetPerformanceInfo核减价值CommitTotal从价值CommitLimit
        /// </summary>

        internal ulong ullAvailPageFile;

        /// <summary>
        /// 调用进程的虚拟地址空间的用户模式部分的大小，以字节为单位。该值取决于进程类型、处理器类型和操作系统的配置。例如，对于 x86 处理器上的大多数 32 位进程，此值约为 2 GB，对于在启用4 GB 调整的系统上运行的具有大地址感知能力的 32 位进程约为 3 GB 。
        /// </summary>

        internal ulong ullTotalVirtual;

        /// <summary>
        /// 当前在调用进程的虚拟地址空间的用户模式部分中未保留和未提交的内存量，以字节为单位
        /// </summary>
        internal ulong ullAvailVirtual;


        /// <summary>
        /// 预订的。该值始终为 0
        /// </summary>
        internal ulong ullAvailExtendedVirtual;

        internal void Init()
        {
            dwLength = checked((uint)Marshal.SizeOf(typeof(MemoryStatusExE)));
        }
    }
}
