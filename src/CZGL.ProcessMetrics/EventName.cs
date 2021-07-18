using System;
using System.Collections.Generic;
using System.Text;

namespace CZGL.ProcessMetrics
{
    /// <summary>
    /// <br /> CLR 中包含以下监视器
    /// <br /> Microsoft-Windows-DotNETRuntime
    /// <br /> System.Runtime
    /// <br /> Microsoft-System-Net-Http
    /// <br /> System.Diagnostics.Eventing.FrameworkEventSource
    /// <br /> Microsoft-Diagnostics-DiagnosticSource
    /// <br /> Microsoft-System-Net-Sockets
    /// <br /> Microsoft-System-Net-NameResolution
    /// <br /> System.Threading.Tasks.TplEventSource
    /// <br /> System.Buffers.ArrayPoolEventSource
    /// <br /> Microsoft-System-Net-Security
    /// <br /> System.Collections.Concurrent.ConcurrentCollectionsEventSource
    /// <br />
    /// .NET 中的事件类型，你可以到官网文档查看 <see href="https://docs.microsoft.com/en-us/dotnet/core/diagnostics/available-counters"/>
    /// </summary>
    public static class EventNames
    {
        public const string Microsoft_Windows_DotNETRuntime = "Microsoft-Windows-DotNETRuntime";
        public const string System_Runtime = "System.Runtime";
        public const string Microsoft_System_Net_Http = "Microsoft-System-Net-Http";
        public const string FrameworkEventSource = "System.Diagnostics.Eventing.FrameworkEventSource";
        public const string DiagnosticSource = "Microsoft-Diagnostics-DiagnosticSource";
        public const string System_Net_Sockets = "Microsoft-System-Net-Sockets";
        public const string NameResolution = "Microsoft-System-Net-NameResolution";
        public const string TplEventSource = "System.Threading.Tasks.TplEventSource";
        public const string ArrayPoolEventSource = "System.Buffers.ArrayPoolEventSource";
        public const string Security = "Microsoft-System-Net-Security";
        public const string ConcurrentCollectionsEventSource = "System.Collections.Concurrent.ConcurrentCollectionsEventSource";
        public const string AspNetCore_Http_Connections = "Microsoft.AspNetCore.Http.Connections";
        public static IEnumerable<string> GetAll()
        {
            return new List<string>
            {
                Microsoft_Windows_DotNETRuntime,
                System_Runtime,
                Microsoft_System_Net_Http,
                FrameworkEventSource,
                DiagnosticSource,
                System_Net_Sockets,
                NameResolution,
                TplEventSource,
                ArrayPoolEventSource,
                Security,
                ConcurrentCollectionsEventSource
            };
        }
    }
}
