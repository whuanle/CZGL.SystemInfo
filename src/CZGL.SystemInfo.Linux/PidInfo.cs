namespace CZGL.SystemInfo.Linux
{
    /// <summary>
    /// PID
    /// </summary>
    public class PidInfo
    {

        /// <summary>
        /// 判断是否能够获取到当前类型的信息
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 进程id
        /// </summary>
        public int PID { get; set; }

        /// <summary>
        /// 进程名称
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 进程优先级
        /// </summary>
        public string PR { get; set; }

        /// <summary>
        /// 高低优先级
        /// </summary>
        public int Nice { get; set; }

        /// <summary>
        /// 进程占用虚拟内存
        /// </summary>
        public string VIRT { get; set; }

        /// <summary>
        /// 进程占用的物理内存
        /// </summary>
        public string RES { get; set; }

        /// <summary>
        /// 共享内存大小
        /// </summary>
        public string SHR { get; set; }

        /// <summary>
        /// 进程状态
        /// D 不可中断的睡眠状态
        /// R 运行
        /// S 睡眠
        /// T 跟踪/停止
        /// Z 僵尸进程
        /// </summary>
        public char State { get; set; }

        /// <summary>
        /// 进程最近占用CPU负载百分比
        /// </summary>
        public double CPU { get; set; }

        /// <summary>
        /// 进程使用物理内存的百分比
        /// </summary>
        public double Mem { get; set; }
    }
}