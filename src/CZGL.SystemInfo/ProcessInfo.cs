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
    /// 记录某一时刻进程资源消耗
    /// </summary>
    public partial class ProcessInfo
    {
        private readonly Process _process;

        public Process Process => _process;

        private ProcessInfo(Process process)
        {
            _process =  process;
            RecordDateTime = DateTime.Now;
        }



        /// <summary>
        /// Physical memory usage(byte)<br />
        /// 物理内存使用量(byte)
        /// </summary>
        public long PhysicalUsedMemory => _process.WorkingSet64;


        /// <summary>
        /// Gets the base priority of the associated process. <see cref="ProcessPriorityClass"/><br />
        /// 获取此进程的基本优先级 <see cref="ProcessPriorityClass"/>
        /// </summary>
        public ProcessPriorityClass BasePriority => BasePriorityValue(_process.BasePriority);

        /// <summary>
        /// The moment the information is recorded<br />
        /// 记录信息的时刻
        /// </summary>
        public DateTime RecordDateTime { get; private set; }

        /// <summary>
        /// The module name of the process<br />
        /// 进程所在模块名称
        /// </summary>
        public string MainModule => _process.MainModule.ModuleName;

        /// <summary>
        /// Whether this process has exited<br />
        /// 此进程是否已经退出
        /// </summary>
        public bool HasExited => _process.HasExited;


        /// <summary>
        /// The Id of the process<br />
        /// 进程的 Id
        /// </summary>
        public int ProcessId => _process.Id;

        /// <summary>
        /// Process name<br />
        /// 进程的名称
        /// </summary>
        public string ProcessName => _process.ProcessName;


        /// <summary>
        /// The time the process starts<br />
        /// 进程启动的时间
        /// </summary>
        public DateTime StartTime => _process.StartTime;

        /// <summary>
        /// Gets the total processor time for this process.<br />
        /// 此进程的总处理器时间(毫秒)
        /// </summary>
        public TimeSpan TotalProcessorTime=> _process.TotalProcessorTime;

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


    }
}
