namespace CZGL.SystemInfo.Linux
{
    /// <summary>
    /// Swap交换分区信息，单位 kb
    /// </summary>
    [InfoName(ChinaName = "wap交换分区信息")]
    public class Swap
    {
        /// <summary>
        /// 判断是否能够获取到当前类型的信息
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 总内存大小
        /// </summary>
        [InfoName(ChinaName = "总内存大小")]
        public long Total { get; set; }

        /// <summary>
        /// 已使用内存
        /// </summary>

        [InfoName(ChinaName = "已使用内存")]
        public long Used { get; set; }

        /// <summary>
        /// 剩余可使用内存
        /// </summary>

        [InfoName(ChinaName = "剩余可使用内存")]
        public long Free { get; set; }

        /// <summary>
        /// 进程下一次可分配的物理内存
        /// </summary>

        [InfoName(ChinaName = "进程下一次可分配的物理内存")]
        public long AvailMem { get; set; }
    }
}