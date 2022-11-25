using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CZGL.SystemInfo.CPU.Windows
{
    /// <summary>
    /// Windows
    /// </summary>
    public partial class WindowsCPU
    {
        /*
        IdleTime 空闲时间
        KernelTime 内核时间
        UserTime 用户时间

        系统时间 = 内核时间 + 用户时间
        SystemTime = KernelTime + UserTime

         */

        /// <summary>
        /// 在多处理器系统上，返回的值是所有处理器指定时间的总和
        /// </summary>
        /// <remarks><see href="https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getsystemtimes"/></remarks>
        /// <param name="lpIdleTime">指向 FILETIME 结构的指针，该结构接收系统空闲的时间量</param>
        /// <param name="lpKernelTime">指向 FILETIME 结构的指针，该结构接收系统在内核模式下执行的时间量(包括所有进程中的所有线程以及所有处理器上的所有线程)。此时间值还包括系统空闲的时间</param>
        /// <param name="lpUserTime">指向 FILETIME 结构的指针，该结构接收系统在 User 模式下执行的时间量(包括所有进程中的所有线程以及所有处理器上的所有线程)</param>
        /// <returns></returns>
        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetSystemTimes(out FILETIME lpIdleTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);


        /// <summary>
        /// 获取 CPU 工作时间
        /// </summary>
        /// <param name="lpIdleTime"></param>
        /// <param name="lpKernelTime"></param>
        /// <param name="lpUserTime"></param>
        /// <returns></returns>
        public static CPUTime GetCPUTime(FILETIME lpIdleTime, FILETIME lpKernelTime, FILETIME lpUserTime)
        {
            var IdleTime = ((ulong)lpIdleTime.DateTimeHigh << 32) | lpIdleTime.DateTimeLow;
            var KernelTime = ((ulong)lpKernelTime.DateTimeHigh << 32) | lpKernelTime.DateTimeLow;
            var UserTime = ((ulong)lpUserTime.DateTimeHigh << 32) | lpUserTime.DateTimeLow;

            var SystemTime = KernelTime + UserTime;

            return new CPUTime(IdleTime, SystemTime);
        }

        /// <summary>
        /// 获取 CPU 工作时间
        /// </summary>
        /// <returns></returns>
        public static CPUTime GetCPUTime()
        {
            if (!GetSystemTimes(out FILETIME lpIdleTime, out FILETIME lpKernelTime, out FILETIME lpUserTime))
            {
                return default;
            }
            return GetCPUTime(lpIdleTime, lpKernelTime, lpUserTime);
        }
    }
}
