using CZGL.SystemInfo.Cpu;
using System.Buffers.Binary;
using System.Runtime.InteropServices;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// Windows 时间，获取 CPU 使用率
    /// </summary>
    public class WindowsCPU
    {
        private WindowsCPUValue _value;

        public WindowsCPUValue Value => _value;




        /// <summary>
        /// 在多处理器系统上，返回的值是所有处理器指定时间的总和
        /// </summary>
        /// <remarks>https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getsystemtimes</remarks>
        /// <param name="lpIdleTime">指向接收系统空闲时间量的FILETIME结构的指针</param>
        /// <param name="lpKernelTime">指向FILETIME结构的指针，该结构接收系统在内核模式下执行所花费的时间（包括所有进程中的所有线程，所有处理器上）。该时间值还包括系统空闲的时间量</param>
        /// <param name="lpUserTime">指向FILETIME结构的指针，该结构接收系统在用户模式下执行所花费的时间量（包括所有进程中所有处理器上的所有线程）</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetSystemTimes(out FILETIME lpIdleTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public WindowsCPUValue GetValue()
        {
            Refresh();
            return _value;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public void Refresh()
        {
            FILETIME lpIdleTime, lpKernelTime, lpUserTime;
            if (!GetSystemTimes(out lpIdleTime, out lpKernelTime, out lpUserTime))
            {
                throw new PlatformNotSupportedException("获取 CPU 信息失败");
            }
            _value = new WindowsCPUValue
            {
                LpIdleTime = MemoryMarshal.Cast<uint, ulong>(new uint[] { lpIdleTime.DateTimeLow, lpIdleTime.DateTimeHigh })[0],
                LpKernelTime = MemoryMarshal.Cast<uint, ulong>(new uint[] { lpKernelTime.DateTimeLow, lpKernelTime.DateTimeHigh })[0],
                LpUserTime = MemoryMarshal.Cast<uint, ulong>(new uint[] { lpUserTime.DateTimeLow, lpUserTime.DateTimeHigh })[0]
            };
        }

        private CPUTime LastValue;

        private struct CPUTime
        {
            public DateTime StartTime { get; set; }
            public WindowsCPUValue Value { get; set; }
        }


        /// <summary>
        /// 开始记录
        /// </summary>
        public void Start()
        {
            Refresh();
            LastValue = new CPUTime
            {
                StartTime = DateTime.Now,
                Value = _value
            };
        }

        /// <summary>
        /// 获取 CPU 使用率
        /// </summary>
        /// <returns></returns>
        public int GetCpuUsed()
        {
            var last = LastValue;
            Start();
            var now = LastValue;

            var idle = now.Value.LpIdleTime - last.Value.LpIdleTime;
            var kernel = now.Value.LpKernelTime - last.Value.LpKernelTime;
            var user = now.Value.LpUserTime - last.Value.LpUserTime;

            var total = kernel + user;
            var used = total - idle;
            var percent = used * 100 / total;
            return (int)percent;
        }

    }

    /// <summary>
    /// Linux CPU 信息，获取使用率
    /// </summary>
    public class LinuxCPU
    {
        const string Path = "/proc/stat";

        private LinuxCPUValue[] _values = new LinuxCPUValue[0];

        /// <summary>
        /// 获取每个 CPU 的信息
        /// </summary>
        public LinuxCPUValue[] Cpus => _values;

        /// <summary>
        /// 
        /// </summary>
        public LinuxCPU()
        {
            Refresh();
        }

        /// <summary>
        /// 获取 CPU
        /// </summary>
        /// <returns></returns>
        public LinuxCPUValue[] GetValue()
        {
            Refresh();
            return _values;
        }

        /// <summary>
        /// 刷新 CPU 状态
        /// </summary>
        public void Refresh()
        {
            var text = File.ReadAllLines(Path);
            List<LinuxCPUValue> cpus = new List<LinuxCPUValue>();


            foreach (var item in text)
            {
                var value = item.Split(" ").Where(x => x != " ").ToArray();
                var cpu = new LinuxCPUValue
                {
                    Name = value[0],
                    User = int.Parse(value[1]),
                    Nice = int.Parse(value[2]),
                    System = int.Parse(value[3]),
                    Idle = int.Parse(value[4])
                };
                cpus.Add(cpu);
            }
            _values = cpus.ToArray();
        }

        private struct CPUTime
        {
            public DateTime StartTime { get; set; }
            public LinuxCPUValue Value { get; set; }
        }

        private CPUTime[] LastCPU;

        /// <summary>
        /// 开始记录 CPU
        /// </summary>
        public void Start()
        {
            Refresh();
            LastCPU = _values.Select(x => new CPUTime
            {
                StartTime = DateTime.Now,
                Value = x
            }).ToArray();
        }

        /// <summary>
        /// 获取每个 CPU 的使用率
        /// </summary>
        /// <returns></returns>
        public (decimal Average, decimal[] Values) GetCpuUse()
        {
            var last = LastCPU;
            Start();
            var now = LastCPU;

            List<decimal> cpus = new List<decimal>();
            var length = now.Length;
            for (int i = 0; i < length; i++)
            {
                var cpu = GetCpuUse(last[i], now[i]);
                cpus.Add(cpu);
            }
            return (Math.Round(cpus.Average(), 2), cpus.ToArray());
        }


        /// <summary>
        /// 获取 CPU 已用百分比
        /// </summary>
        /// <returns></returns>
        private decimal GetCpuUse(CPUTime last, CPUTime now)
        {
            ulong od, nd;
            ulong id, sd;
            decimal cpu_use;


            od = (ulong)(last.Value.User + last.Value.Nice + last.Value.System + last.Value.Idle);  // 用户+优先级+系统+空闲
            nd = (ulong)(now.Value.User + now.Value.Nice + now.Value.System + now.Value.Idle);      //  用户+优先级+系统+空闲

            id = (ulong)(now.Value.User - last.Value.User);         // 用户第一次和第二次的时间之差
            sd = (ulong)(now.Value.System - last.Value.System);     // 统第一次和第二次的时间之差

            if ((nd - od) != 0)
                cpu_use = Math.Round(((decimal)(sd + id) * 100) / (nd - od), 2); //((用户+系统)乘100)除(第一次和第二次的时间差)再赋给g_cpu_used
            else
                cpu_use = 0;
            return cpu_use;
        }
    }
}
