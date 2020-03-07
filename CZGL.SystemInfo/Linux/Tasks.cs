namespace CZGL.SystemInfo.Linux
{
    /// <summary>
    /// 总进程数
    /// </summary>
    [InfoName(ChinaName = "进程统计")]
    public class Tasks
    {
        /// <summary>
        /// 判断是否能够获取到当前类型的信息
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 总进程数
        /// </summary>
        [InfoName(ChinaName = "总进程数")]
        public int Total { get; set; }

        /// <summary>
        /// 正在运行
        /// </summary>

        [InfoName(ChinaName = "正在运行")]
        public int Running { get; set; }

        /// <summary>
        /// 休眠
        /// </summary>

        [InfoName(ChinaName = "休眠")]
        public int Sleeping { get; set; }

        /// <summary>
        /// 已停止
        /// </summary>

        [InfoName(ChinaName = "已停止")]
        public int Stopped { get; set; }

        /// <summary>
        /// 僵尸进程
        /// </summary>

        [InfoName(ChinaName = "僵尸进程")]
        public int Zombie { get; set; }
    }
}