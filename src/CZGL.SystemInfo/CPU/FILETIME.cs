using System.Runtime.InteropServices;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
    {
        /// <summary>
        /// 时间的低位部分
        /// </summary>
        public uint DateTimeLow;

        /// <summary>
        /// 时间的高位部分
        /// </summary>
        public uint DateTimeHigh;
    }
}
