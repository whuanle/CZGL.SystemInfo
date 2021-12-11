using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZGL.SystemInfo.Memory
{
    /// <summary>
    /// 获取内存信息
    /// </summary>
    public interface IMemory
    {
        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns></returns>
        MemoryValue GetValue();

        /// <summary>
        /// 刷新值
        /// </summary>
        /// <param name="value"></param>
        void Refresh(MemoryValue value);
    }
}
