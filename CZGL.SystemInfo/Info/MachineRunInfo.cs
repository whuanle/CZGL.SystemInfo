using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CZGL.SystemInfo.Info
{
    /// <summary>
    /// 当前机器占用资源情况
    /// </summary>
    [InfoName(ChinaName = "主机运行信息")]
    public class MachineRunInfo
    {
        /// <summary>
        /// 静态 MachineRunInfo 实例
        /// </summary>
        public static MachineRunInfo Instance => new MachineRunInfo();
        private double _thisUsedMem;
        private double _UsedCPUTime;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MachineRunInfo()
        {
            var proc = Process.GetCurrentProcess();
            var mem = proc.WorkingSet64;
            var cpu = proc.TotalProcessorTime;
            _thisUsedMem = mem / 1024.0;
            _UsedCPUTime = cpu.TotalMilliseconds;
        }
        /// <summary>
        /// 当前进程已使用物理内存(单位 kb)
        /// </summary>
        [InfoName(ChinaName = "当前进程已使用物理内存")]
        public double ThisUsedMem { get { return _thisUsedMem; } }
        /// <summary>
        /// 当前进程已占耗CPU时间(单位 ms)
        /// </summary>
        [InfoName(ChinaName = "当前进程已占耗CPU时间")]
        public double APPUsedCPUTime { get { return _UsedCPUTime; } }
        /// <summary>
        /// 系统所有进程各种使用的内存(单位 kb)
        /// </summary>

        [InfoName(ChinaName = "系统所有进程各种使用的内存")]
        public KeyValuePair<string, long>[] AllProcessMemory
        {
            get
            {
                List<KeyValuePair<string, long>> dic = new List<KeyValuePair<string, long>>();

                Process[] p = Process.GetProcesses();
                Int64 totalMem = 0;
                foreach (Process pr in p)
                {
                    totalMem += pr.WorkingSet64 / 1024;
                    dic.Add(new KeyValuePair<string, long>(pr.ProcessName, pr.WorkingSet64 / 1024));  //每个进程的内存
                }
                _UsedMemory = totalMem;
                return dic.ToArray();
            }
        }
        private long _UsedMemory;
        /// <summary>
        /// 系统已使用内存(单位 kb)
        /// </summary>
        [InfoName(ChinaName = "系统已使用内存")]
        public long UsedMemory
        {
            get { return _UsedMemory; }
        }
    }
}
