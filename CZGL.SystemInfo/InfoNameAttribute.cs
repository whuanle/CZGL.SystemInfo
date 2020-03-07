using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.SystemInfo
{
    /// <summary>
    /// 别名特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public class InfoNameAttribute : Attribute
    {
        public string ChinaName { get; set; }
    }

    public enum RunOS
    {
        WINDOWS = 1,
        LINUX = 2,
        ALL = 3
    }
}
