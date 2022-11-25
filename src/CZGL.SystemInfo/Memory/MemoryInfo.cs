using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using CZGL.SystemInfo.Memory;
using Microsoft.Win32;
namespace CZGL.SystemInfo
{
    /// <summary>
    /// 获取内存信息
    /// </summary>
    public class MemoryInfo : IMemory
    {
        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public MemoryInfo()
        {
            if (OperatingSystem.IsWindows())
            {
                _memory = new Windows();
            }
            else if (OperatingSystem.IsLinux())
            {
                _memory = new Linux();
            }
            else
            {
                throw new PlatformNotSupportedException("抱歉，暂不支持该操作系统！");

            }
        }

        private readonly IMemory _memory;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public MemoryValue GetValue() => _memory.GetValue();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public void Refresh(MemoryValue value) => _memory.Refresh(value);

        #region Windows

        private class Windows : IMemory
        {
            private MemoryStatusExE _memoryStatusEx;

            public Windows()
            {
                // 检查 Windows 内核版本，是否为旧系统
                if (Environment.OSVersion.Version.Major < 5)
                {
                    throw new PlatformNotSupportedException($"系统版本太旧！你的系统主要版本：{Environment.OSVersion.Version.Major}，请查看 https://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions");
                }
            }

            /// <inheritdoc/>
            public MemoryValue GetValue()
            {
                MemoryValue value = new MemoryValue();
                Refresh(value);
                return value;
            }

            /// <inheritdoc/>
            /// <exception cref="Win32Exception"></exception>
            public void Refresh(MemoryValue value)
            {
                _memoryStatusEx = new MemoryStatusExE();
                // 重新初始化结构的大小
                _memoryStatusEx.Refresh();
                // 刷新值
                if (!GlobalMemoryStatusEx(ref _memoryStatusEx)) throw new Win32Exception("无法获得内存信息");

                value.TotalPhysicalMemory = _memoryStatusEx.ullTotalPhys;
                value.AvailablePhysicalMemory = _memoryStatusEx.ullAvailPhys;
                value.TotalVirtualMemory = _memoryStatusEx.ullTotalVirtual;
                value.AvailableVirtualMemory = _memoryStatusEx.ullAvailVirtual;
                value.UsedPercentage = (int)_memoryStatusEx.dwMemoryLoad;
            }

            #region Kernel32接口


            ///// <summary>
            ///// 在内存超过 4 GB 的计算机上， GlobalMemoryStatus函数可能返回不正确的信息，报告值 –1 表示溢出。因此，应用程序应改用 GlobalMemoryStatusEx函数。
            ///// </summary>
            ///// <remarks>Windows XP [仅限桌面应用程序];最低支持服务器 Windows Server 2003 [仅限桌面应用程序]</remarks>
            ///// <param name="lpBuffer"></param>
            //[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            //internal static extern void GlobalMemoryStatus(ref MEMORYSTATUS lpBuffer);


            /// <summary>
            /// 检索有关系统当前使用物理和虚拟内存的信息
            /// </summary>
            /// <remarks><see href="https://docs.microsoft.com/zh-cn/windows/win32/api/sysinfoapi/nf-sysinfoapi-globalmemorystatusex"/></remarks>
            /// <param name="lpBuffer"></param>
            /// <returns></returns>
            [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern Boolean GlobalMemoryStatusEx(ref MemoryStatusExE lpBuffer);

            #endregion

        }

        #endregion

        private class Linux : IMemory
        {
            private Sysinfo _sysinfo;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <returns></returns>
            public MemoryValue GetValue()
            {
                MemoryValue value = new MemoryValue();
                Refresh(value);
                return value;
            }

            public void Refresh(MemoryValue value)
            {
                var result = sysinfo(ref _sysinfo);
                if (result != 0) throw new PlatformNotSupportedException("无法获得内存信息");
                value.TotalPhysicalMemory = _sysinfo.totalram;
                value.AvailablePhysicalMemory = _sysinfo.freeram;
                value.TotalVirtualMemory = _sysinfo.totalswap;
                value.AvailableVirtualMemory = _sysinfo.freeswap;
                value.UsedPercentage = (int)((_sysinfo.totalram - _sysinfo.freeram) / _sysinfo.totalram);
            }

            #region Linux API

            /// <summary>
            /// 返回整个系统统计信息,<see href="https://linux.die.net/man/2/sysinfo"/>
            /// </summary>
            /// <remarks>int sysinfo(struct sysinfo *info);</remarks>
            /// <param name="info"></param>
            /// <returns></returns>
            [DllImport("libc.so.6", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.SysInt)]
            public static extern int sysinfo(ref Sysinfo info);

            #endregion

        }

    }
}
