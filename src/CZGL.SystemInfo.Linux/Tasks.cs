namespace CZGL.SystemInfo.Linux
{
    /// <summary>
    /// 总进程数
    /// </summary>
    public class Tasks
    {
        /// <summary>
        /// 判断是否能够获取到当前类型的信息
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 总进程数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 正在运行
        /// </summary>
        public int Running { get; set; }

        /// <summary>
        /// 休眠
        /// </summary>
        public int Sleeping { get; set; }

        /// <summary>
        /// 已停止
        /// </summary>
        public int Stopped { get; set; }

        /// <summary>
        /// 僵尸进程
        /// </summary>
        public int Zombie { get; set; }
    }
}