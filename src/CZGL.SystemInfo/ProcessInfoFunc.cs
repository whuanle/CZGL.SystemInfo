using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo
{
    public partial class ProcessInfo
    {        /// <summary>
             /// Get process list<br />
             /// 获取操作系统中所有进程
             /// </summary>
             /// <returns></returns>
        public static ProcessInfo[] GetProcesses()
        {
            return Process.GetProcesses().Select(item => new ProcessInfo(item)).ToArray();
        }

        /// <summary>
        /// Get process list<br />
        /// 获取进程列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetProcessList()
        {
            return Process.GetProcesses().ToDictionary(x => x.Id, x => x.ProcessName);
        }

        /// <summary>
        /// Monitor this process with the process ID<br />
        /// 通过进程 id 监控此进程
        /// </summary>
        /// <param name="processId">进程的id</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">指定的进程号不存在</exception>
        public static ProcessInfo GetProcess(int processId)
        {
            return new ProcessInfo(Process.GetProcessById(processId));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ProcessPriorityClass BasePriorityValue(int value)
        {
            switch (value)
            {
                case 0: return 0;
                case 4:
                    return ProcessPriorityClass.Idle;
                case 8:
                    return ProcessPriorityClass.Normal;
                case 13:
                    return ProcessPriorityClass.High;
                case 24:
                    return ProcessPriorityClass.RealTime;
                default:
                    throw new InvalidOperationException("The priority of this process cannot be identified | 无法识别此进程的优先级");
            }
        }


        /// <summary>
        /// 刷新并初始化
        /// </summary>
        public void Refresh()
        {
            _process.Refresh();
            RecordDateTime = DateTime.Now;
        }

        /// <summary>
        /// 计算一个进程的 CPU 使用率
        /// </summary>
        /// <param name="processInfo"></param>
        /// <returns></returns>
        public static decimal GetCpuUsage(ProcessInfo processInfo)
        {
            var startTime = processInfo.RecordDateTime;
            var startCpuUseage = processInfo.TotalProcessorTime.TotalMilliseconds;

            processInfo.Refresh();
            var endTime = processInfo.RecordDateTime;
            var endCpuUseage = processInfo.TotalProcessorTime.TotalMilliseconds;

            var cpuUsedMs = (endCpuUseage - startCpuUseage);
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            var value = Math.Round((decimal)cpuUsageTotal * 100, 2);
            return value;
        }

    }
}
