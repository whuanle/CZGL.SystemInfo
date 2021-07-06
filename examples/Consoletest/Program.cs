using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using CZGL.ProcessMetrics;
using CZGL.SystemInfo;

namespace Consoletest
{
    class Program
    {
        // 需要使用管理员权限
        public static void Main()
        {
            MetricsServer metricsServer = new MetricsServer("http://*:1234/metrics/");
            metricsServer.Start();
            Console.ReadKey();
        }
    }
}