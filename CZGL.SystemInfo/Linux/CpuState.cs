namespace CZGL.SystemInfo.Linux
{
    [InfoName(ChinaName = "CPU运行状态")]
    public class CpuState
    {
        /// <summary>
        /// 判断是否能够获取到当前类型的信息
        /// </summary>
        public  bool IsSuccess { get; set; }
        
        /// <summary>
        /// 用户占用CPU负载百分比
        /// </summary>
        [InfoName(ChinaName = "用户占用CPU负载百分比")]
        public double UserSpace { get; set; }
        
        /// <summary>
        /// 系统内核占用CPU负载百分比
        /// </summary>
        
        [InfoName(ChinaName = "系统内核占用CPU负载百分比")]
        public double Sysctl { get; set; }
        
        /// <summary>
        /// 特殊优先级进程占用CPU负载百分比
        /// </summary>

        [InfoName(ChinaName = "特殊优先级进程占用CPU负载百分比")]
        public double NI { get; set; }

        /// <summary>
        /// 剩余可用CPU负载百分比
        /// </summary>
        [InfoName(ChinaName = "剩余可用CPU负载百分比")]
        public double Idolt { get; set; }
        
        /// <summary>
        /// IO等待占用CPU负载百分比
        /// </summary>

        [InfoName(ChinaName = "IO等待占用CPU负载百分比")]
        public double WaitIO { get; set; }
        
        /// <summary>
        /// 硬中断占用CPU负载百分比
        /// </summary>

        [InfoName(ChinaName = "硬中断占用CPU负载百分比")]
        public double HardwareIRQ { get; set; }
        
        /// <summary>
        /// 软中断占用CPU负载百分比
        /// </summary>
        
        [InfoName(ChinaName = "软中断占用CPU负载百分比")]
        public double SoftwareInterrupts { get; set; }
    }
}