namespace CZGL.SystemInfo
{
    /// <summary>
    /// 
    /// </summary>
    public struct CPUTime
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idleTime"></param>
        /// <param name="systemTime"></param>
        public CPUTime(ulong idleTime, ulong systemTime)
        {
            IdleTime = idleTime;
            SystemTime = systemTime;
        }

        /// <summary>
        /// CPU 空闲时间
        /// </summary>
        public ulong IdleTime { get; private set; }

        /// <summary>
        /// CPU 工作时间
        /// </summary>
        public ulong SystemTime { get; private set; }
    }
}
