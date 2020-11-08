namespace CZGL.SystemInfo.Linux
{
    /// <summary>
    /// Swap交换分区信息，单位 kb
    /// </summary>
    public class Swap
    {
        /// <summary>
        /// 判断是否能够获取到当前类型的信息
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 总内存大小
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 已使用内存
        /// </summary>
        public long Used { get; set; }

        /// <summary>
        /// 剩余可使用内存
        /// </summary>
        public long Free { get; set; }

        /// <summary>
        /// 进程下一次可分配的物理内存
        /// </summary>
        public long AvailMem { get; set; }
    }
}