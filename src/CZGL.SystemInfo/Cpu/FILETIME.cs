using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Cpu
{
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
