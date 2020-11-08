using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 记录某一时刻操作系统的资源数据
    /// </summary>
    public class ProcessInfo
    {
        private readonly Lazy<Process> _process;
        public Process Process => _process.Value;
        private ProcessInfo(Process process)
        {
            _process = new Lazy<Process>(() => process);
        }

        /// <summary>
        /// Gets the base priority of the associated process. <see cref="ProcessPriorityClass"/><br />
        /// 获取此进程的基本优先级 <see cref="ProcessPriorityClass"/>
        /// </summary>
        public ProcessPriorityClass BasePriority { get; private set; }

        /// <summary>
        /// The moment the information is recorded<br />
        /// 记录信息的时刻
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// The module name of the process<br />
        /// 进程所在模块名称
        /// </summary>
        public string MainModule { get; private set; }

        /// <summary>
        /// Whether this process has exited<br />
        /// 此进程是否已经退出
        /// </summary>
        public bool HasExited { get; private set; }

        /// <summary>
        /// Gets the maximum allowable working set size, in bytes, for the associated process.<br />
        /// 进程允许的最大内存
        /// </summary>
        public int MaxMemory { get; private set; }

        /// <summary>
        /// Gets the minimum allowable working set size, in bytes, for the associated process.<br />
        /// 进程允许的最小内存
        /// </summary>
        public int MinMemory { get; private set; }

        /// <summary>
        /// Gets the amount of physical memory, in bytes, allocated for the associated process.<br />
        /// 当前进程已使用的物理内存字节数(byte)
        /// </summary>
        public long PhysicalUsedMemory { get; private set; }

        /// <summary>
        /// The Id of the process<br />
        /// 进程的 Id
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Process name<br />
        /// 进程的名称
        /// </summary>
        public string ProcessName { get; private set; }


        /// <summary>
        /// The time the process starts<br />
        /// 进程启动的时间
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Gets the total processor time for this process.<br />
        /// 此进程的总处理器时间(毫秒)
        /// </summary>
        public TimeSpan TotalProcessorTime { get; private set; }

        private const int SpinCount = 500;
        private const int Count = 3;

        /// <summary>
        /// 获取一个进程 CPU 使用率
        /// </summary>
        /// <param name="processId">进程的 Id</param>
        /// <returns></returns>
        public static decimal GetCpuPercentage(int processId)
        {
            int count = SpinCount;

            var process = Process.GetProcessById(processId);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[] times = new double[Count];

            TimeSpan kernelStartTime = process.PrivilegedProcessorTime;
            TimeSpan userStartTime = process.UserProcessorTime;

            for (int i = 0; i < Count; i++)
            {
                System.Threading.Thread.Sleep(count);
                stopwatch.Stop();
                process = Process.GetProcessById(processId);

                TimeSpan kernelEndTime = process.PrivilegedProcessorTime;
                TimeSpan userEndTime = process.UserProcessorTime;

                var useTime = kernelEndTime + userEndTime - (kernelStartTime + userStartTime);

                times[i] = useTime.TotalMilliseconds / stopwatch.Elapsed.TotalMilliseconds;

                kernelStartTime = kernelEndTime;
                userStartTime = userEndTime;
            }
            return new decimal(Math.Round(times.Average(), 4));
        }

        /// <summary>
        /// 刷新并初始化
        /// </summary>
        public void Refresh()
        {
            _process.Value.Refresh();
            PhysicalUsedMemory = _process.Value.WorkingSet64;
            TotalProcessorTime = _process.Value.TotalProcessorTime;
            BasePriority = BasePriorityValue(_process.Value.BasePriority);
            HasExited = _process.Value.HasExited;
            StartTime = _process.Value.StartTime;
            ProcessId = _process.Value.Id;
            MainModule = _process.Value.MainModule.ModuleName;
            ProcessName = _process.Value.ProcessName;
            try
            {
                MaxMemory = _process.Value.MaxWorkingSet.ToInt32();
                MinMemory = _process.Value.MinWorkingSet.ToInt32();

            }
            catch
            {
                MaxMemory = -1;
                MinMemory = -1;
            }

            DateTime = DateTime.Now;
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
        /// Gets the number of milliseconds elapsed since the system started.<br />
        /// 操作系统自开机后系统已运行时间(毫秒)
        /// </summary>
        public static int TickCount => Environment.TickCount;

        /// <summary>
        /// Get current process<br />
        /// 获取当前进程
        /// </summary>
        /// <returns></returns>
        public static ProcessInfo GetCurrentProcess()
        {
            return new ProcessInfo(Process.GetCurrentProcess());
        }

        /// <summary>
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
    }
}
