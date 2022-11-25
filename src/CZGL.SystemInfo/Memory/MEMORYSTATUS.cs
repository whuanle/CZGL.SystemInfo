using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Memory
{
    internal struct MEMORYSTATUS
    {
        internal uint dwLength;
        internal uint dwMemoryLoad;
        internal uint dwTotalPhys;
        internal uint dwAvailPhys;
        internal uint dwTotalPageFile;
        internal uint dwAvailPageFile;
        internal uint dwTotalVirtual;
        internal uint dwAvailVirtual;
    }
}
