using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CZGL.SystemInfo.Memory
{
    /// <summary>
    /// 
    /// </summary>
    public static class MemoryHelper
    {
        /// <summary>
        /// 获取当前系统的内存信息
        /// </summary>
        /// <returns></returns>
        public static MemoryValue GetMemoryValue()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return WindowsMemory.GetMemory();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return LinuxMemory.GetMemory();

            return default;
        }
    }
}
