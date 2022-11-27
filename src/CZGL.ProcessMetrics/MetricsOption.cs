using CZGL.SystemInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CZGL.ProcessMetrics
{
    /// <summary>
    /// 自定义设置
    /// </summary>
    public class MetricsOption
    {
        public MetricsOption()
        {
            var process = Process.GetCurrentProcess();
            _Labels = new Dictionary<string, string>();
            _Labels.Add("machine_name", SystemPlatformInfo.MachineName);
            _Labels.Add("process_name", AppDomain.CurrentDomain.FriendlyName);

            _ListenerNames = new List<string>();
            Listeners = _ListenerNames;
            _ListenerNames.Add(EventNames.System_Runtime);
            _ListenerNames.Add(EventNames.AspNetCore_Http_Connections);

            _Assemblies = new List<Assembly>();
            JobName = process.ProcessName.ToLower();
        }

        internal readonly Dictionary<string, string> _Labels;
        internal readonly List<string> _ListenerNames;
        internal readonly List<Assembly> _Assemblies;
        internal static List<string> Listeners;

        public string JobName { get; set; }

        /// <summary>
        /// 全局标签值，所有数据中都会带上这些标签，以便与其它应用或机器区分。
        /// </summary>
        public Dictionary<string, string> Labels => _Labels;

        /// <summary>
        /// 要监控的事件源，在 <see cref="EventNames"/> 中已经列举了一些。
        /// <para>.NET 中的事件类型，你可以到官网文档查看 <see href="https://docs.microsoft.com/en-us/dotnet/core/diagnostics/available-counters"/></para>
        /// </summary>
        public List<string> ListenerNames => _ListenerNames;

        /// <summary>
        /// 支持自定义监控源 <see cref="IMerticsSource"/>，这里可以填写这些监控源所在的程序集。
        /// </summary>
        public List<Assembly> Assemblies => _Assemblies;
    }
}
