using CZGL.SystemInfo.Memory;
using System.Runtime.InteropServices;

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
            var usedPercentage = (((double)info.totalram - info.freeram) / (double)info.totalram) * 100;
            MemoryValue value = new MemoryValue(info.totalram, info.freeram, (ulong)usedPercentage, info.totalswap, info.freeswap);
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
        [return: MarshalAs(UnmanagedType.I4)]
        public static partial System.Int32 sysinfo(ref Sysinfo info);

#else

        /// <summary>
        /// 返回整个系统统计信息,<see href="https://linux.die.net/man/2/sysinfo"/>
        /// </summary>
        /// <remarks>int sysinfo(struct sysinfo *info);</remarks>
        /// <param name="info"></param>
        /// <returns></returns>
        [DllImport("libc.so.6", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern System.Int32 sysinfo(ref Sysinfo info);

#endif
    }
}
