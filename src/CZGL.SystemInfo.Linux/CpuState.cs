namespace CZGL.SystemInfo.Linux
{
    /// <summary>
    /// CPU 状态
    /// </summary>
    public class CpuState
    {
        /// <summary>
        /// 判断是否能够获取到当前类型的信息
        /// </summary>
        public  bool IsSuccess { get; set; }
        
        /// <summary>
        /// 用户占用CPU负载百分比
        /// </summary>
        public double UserSpace { get; set; }
        
        /// <summary>
        /// 系统内核占用CPU负载百分比
        /// </summary>
        
        public double Sysctl { get; set; }
        
        /// <summary>
        /// 特殊优先级进程占用CPU负载百分比
        /// </summary>

        public double NI { get; set; }

        /// <summary>
        /// 剩余可用CPU负载百分比
        /// </summary>
        public double Idolt { get; set; }
        
        /// <summary>
        /// IO等待占用CPU负载百分比
        /// </summary>
        public double WaitIO { get; set; }
        
        /// <summary>
        /// 硬中断占用CPU负载百分比
        /// </summary>
        public double HardwareIRQ { get; set; }
        
        /// <summary>
        /// 软中断占用CPU负载百分比
        /// </summary>
        public double SoftwareInterrupts { get; set; }
    }
}