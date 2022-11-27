using System.Runtime.InteropServices;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 
    /// </summary>
    public static class CPUHelper
    {
        /// <summary>
        /// 获取当前系统消耗的 CPU 时间
        /// </summary>
        /// <returns></returns>
        public static CPUTime GetCPUTime()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WindowsCPU.GetCPUTime();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return LinuxCPU.GetCPUTime();
            return new CPUTime();
        }

        /// <summary>
        /// 计算 CPU 使用率
        /// </summary>
        /// <param name="oldTime"></param>
        /// <param name="newTime"></param>
        /// <returns></returns>
        public static double CalculateCPULoad(CPUTime oldTime, CPUTime newTime)
        {
            ulong totalTicksSinceLastTime = newTime.SystemTime - oldTime.SystemTime;
            ulong idleTicksSinceLastTime = newTime.IdleTime - oldTime.IdleTime;

            double ret = 1.0f - ((totalTicksSinceLastTime > 0) ? ((double)idleTicksSinceLastTime) / totalTicksSinceLastTime : 0);

            return ret;
        }
    }
}
