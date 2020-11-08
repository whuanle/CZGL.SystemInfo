namespace CZGL.SystemInfo.Linux
{
    /// <summary>
    /// 内存使用情况，单位 kb
    /// </summary>
    public class Mem
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
        /// 剩余内存
        /// </summary>
        public long Free { get; set; }

        /// <summary>
        /// 缓存内存
        /// </summary>

        public long Buffers { get; set; }

        /// <summary>
        /// 实际剩余可用内存
        /// </summary>
        public long CanUsed
        {
            get { return Free + Buffers; }
        }
    }
}