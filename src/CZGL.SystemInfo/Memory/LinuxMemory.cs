using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CZGL.SystemInfo.Memory;

namespace CZGL.SystemInfo
{

    /// <summary>
    /// 
    /// </summary>
    public partial class LinuxMemory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MemoryValue GetMemory()
        {
            Sysinfo info = new Sysinfo();
            if (sysinfo(ref info) != 0)
            {
                return default;
            }
            var UsedPercentage = ((info.totalram - info.freeram) / info.totalram);
            MemoryValue value = new MemoryValue(info.totalram, info.freeram, UsedPercentage, info.totalswap, info.freeswap);
            return value;
        }

#if NET7_0_OR_GREATER

        /// <summary>
        /// 返回整个系统统计信息,<see href="https://linux.die.net/man/2/sysinfo"/>
        /// </summary>
        /// <remarks>int sysinfo(struct sysinfo *info);</remarks>
        /// <param name="info"></param>
        /// <returns></returns>
        [LibraryImport("libc.so.6", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.SysInt)]
        public static partial int sysinfo(ref Sysinfo info);

#else

        /// <summary>
        /// 返回整个系统统计信息,<see href="https://linux.die.net/man/2/sysinfo"/>
        /// </summary>
        /// <remarks>int sysinfo(struct sysinfo *info);</remarks>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("libc.so.6", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.SysInt)]
            public static extern int sysinfo(ref Sysinfo info);

#endif
    }
}
