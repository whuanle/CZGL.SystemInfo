using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Memory
{
    internal struct Sysinfo
    {
        public long uptime;             /* Seconds since boot */

        /// <summary>
        /// 获取 1，5，15 分钟内存的平均使用量，数组大小为 3
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ulong[] loads;

        /// <summary>
        /// 总物理内存
        /// </summary>
        public ulong totalram;

        /// <summary>
        /// 可用内存
        /// </summary>
        public ulong freeram;

        /// <summary>
        /// 共享内存
        /// </summary>
        public ulong sharedram;
        public ulong bufferram; /* Memory used by buffers */
        public ulong totalswap; /* Total swap space size */
        public ulong freeswap;  /* swap space still available */
        public ushort procs;    /* Number of current processes */
        public ulong totalhigh; /* Total high memory size */
        public ulong freehigh;  /* Available high memory size */
        public uint mem_unit;   /* Memory unit size in bytes */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] _f; /* Padding to 64 bytes */
    }
}
