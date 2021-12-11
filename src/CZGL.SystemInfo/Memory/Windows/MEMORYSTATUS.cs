using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Memory.Windows
{
    internal struct MEMORYSTATUS
    {
        internal UInt32 dwLength;
        internal UInt32 dwMemoryLoad;
        internal UInt32 dwTotalPhys;
        internal UInt32 dwAvailPhys;
        internal UInt32 dwTotalPageFile;
        internal UInt32 dwAvailPageFile;
        internal UInt32 dwTotalVirtual;
        internal UInt32 dwAvailVirtual;
    }
}
